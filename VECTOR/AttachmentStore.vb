'------------------------------------------------------------------------------
' Gestión de adjuntos digitales (almacenamiento en disco + manifiesto).
'------------------------------------------------------------------------------

Imports System.IO
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Xml.Serialization
Imports System.Data.Entity.Core.EntityClient

Public Class AttachmentInfo
    Public Property DisplayName As String
    Public Property StoredName As String
    Public Property OriginalPath As String
    Public Property AddedAt As DateTime

    <XmlIgnore>
    Public ReadOnly Property DisplayLabel As String
        Get
            Dim estado As String = If(String.IsNullOrWhiteSpace(StoredName), " (pendiente)", "")
            Return $"{DisplayName}{estado}"
        End Get
    End Property
End Class

Public Module AttachmentStore
    Private Const BaseFolderName As String = "VECTOR"
    Private Const SubFolderName As String = "Adjuntos"
    Private Const DatabaseStorageSettingKey As String = "AttachmentsStoreInDatabase"

    Public Function GetAttachmentFolder(idDocumento As Long) As String
        Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), BaseFolderName, SubFolderName, idDocumento.ToString())
    End Function

    Public Function GetManifestPath(idDocumento As Long) As String
        Return Path.Combine(GetAttachmentFolder(idDocumento), "manifest.xml")
    End Function

    Public Function LoadAttachments(idDocumento As Long) As List(Of AttachmentInfo)
        If UseDatabaseStorage() Then
            Return LoadAttachmentsFromDatabase(idDocumento)
        End If

        Dim manifestPath = GetManifestPath(idDocumento)
        If Not File.Exists(manifestPath) Then Return New List(Of AttachmentInfo)()

        Try
            Using stream = File.OpenRead(manifestPath)
                Dim serializer As New XmlSerializer(GetType(List(Of AttachmentInfo)))
                Dim data = TryCast(serializer.Deserialize(stream), List(Of AttachmentInfo))
                If data Is Nothing Then Return New List(Of AttachmentInfo)()

                For Each adjunto In data
                    adjunto.OriginalPath = ""
                Next

                Return data
            End Using
        Catch
            Return New List(Of AttachmentInfo)()
        End Try
    End Function

    Public Sub SaveAttachments(idDocumento As Long, attachments As List(Of AttachmentInfo))
        If UseDatabaseStorage() Then
            SaveAttachmentsToDatabase(idDocumento, attachments)
            Return
        End If

        Dim folder = GetAttachmentFolder(idDocumento)
        Directory.CreateDirectory(folder)

        Dim anteriores = LoadAttachments(idDocumento)
        Dim storedPrev = New HashSet(Of String)(
            anteriores.Where(Function(a) Not String.IsNullOrWhiteSpace(a.StoredName)).Select(Function(a) a.StoredName),
            StringComparer.OrdinalIgnoreCase)

        For Each adjunto In attachments
            If String.IsNullOrWhiteSpace(adjunto.StoredName) Then
                If Not String.IsNullOrWhiteSpace(adjunto.OriginalPath) AndAlso File.Exists(adjunto.OriginalPath) Then
                    Dim extension = Path.GetExtension(adjunto.OriginalPath)
                    Dim storedName = Guid.NewGuid().ToString("N") & extension
                    Dim destino = Path.Combine(folder, storedName)
                    File.Copy(adjunto.OriginalPath, destino, True)
                    adjunto.StoredName = storedName
                    adjunto.OriginalPath = ""
                End If
            End If
        Next

        Dim storedNew = New HashSet(Of String)(
            attachments.Where(Function(a) Not String.IsNullOrWhiteSpace(a.StoredName)).Select(Function(a) a.StoredName),
            StringComparer.OrdinalIgnoreCase)

        For Each stored In storedPrev
            If Not storedNew.Contains(stored) Then
                Dim filePath = Path.Combine(folder, stored) ' Cambiado a filePath
                If File.Exists(filePath) Then
                    File.Delete(filePath)
                End If
            End If
        Next

        Using stream = File.Create(GetManifestPath(idDocumento))
            Dim serializer As New XmlSerializer(GetType(List(Of AttachmentInfo)))
            serializer.Serialize(stream, attachments)
        End Using
    End Sub

    Public Function GetAttachmentPath(idDocumento As Long, storedName As String) As String
        If UseDatabaseStorage() Then
            Return GetAttachmentPathFromDatabase(idDocumento, storedName)
        End If
        Return Path.Combine(GetAttachmentFolder(idDocumento), storedName)
    End Function

    Public Sub DeleteAttachments(idDocumento As Long)
        If UseDatabaseStorage() Then
            Using conn As New SqlConnection(GetProviderConnectionString())
                conn.Open()
                Using cmd As New SqlCommand("DELETE FROM Tra_AdjuntoDocumento WHERE IdDocumento = @IdDocumento", conn)
                    cmd.Parameters.AddWithValue("@IdDocumento", idDocumento)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            Return
        End If

        Dim folder = GetAttachmentFolder(idDocumento)
        If Directory.Exists(folder) Then
            Directory.Delete(folder, True)
        End If
    End Sub

    Private Function UseDatabaseStorage() As Boolean
        Dim setting = ConfigurationManager.AppSettings(DatabaseStorageSettingKey)
        If String.IsNullOrWhiteSpace(setting) Then Return False

        Dim enabled As Boolean = False
        If Boolean.TryParse(setting, enabled) Then
            Return enabled
        End If

        Return False
    End Function

    Private Function GetProviderConnectionString() As String
        Dim entityConnection = ConfigurationManager.ConnectionStrings("SecretariaDBEntities")
        If entityConnection Is Nothing OrElse String.IsNullOrWhiteSpace(entityConnection.ConnectionString) Then
            Throw New InvalidOperationException("No se encontró la cadena de conexión 'SecretariaDBEntities'.")
        End If

        Dim builder As New EntityConnectionStringBuilder(entityConnection.ConnectionString)
        Return builder.ProviderConnectionString
    End Function

    Private Function LoadAttachmentsFromDatabase(idDocumento As Long) As List(Of AttachmentInfo)
        Dim resultados As New List(Of AttachmentInfo)()

        Try
            Using conn As New SqlConnection(GetProviderConnectionString())
                conn.Open()
                Using cmd As New SqlCommand("SELECT StoredName, DisplayName, AddedAt FROM Tra_AdjuntoDocumento WHERE IdDocumento = @IdDocumento ORDER BY AddedAt", conn)
                    cmd.Parameters.AddWithValue("@IdDocumento", idDocumento)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            resultados.Add(New AttachmentInfo() With {
                                .StoredName = reader.GetString(0),
                                .DisplayName = reader.GetString(1),
                                .AddedAt = reader.GetDateTime(2),
                                .OriginalPath = ""
                            })
                        End While
                    End Using
                End Using
            End Using
        Catch
            Return New List(Of AttachmentInfo)()
        End Try

        Return resultados
    End Function

    Private Sub SaveAttachmentsToDatabase(idDocumento As Long, attachments As List(Of AttachmentInfo))
        Dim anteriores = LoadAttachmentsFromDatabase(idDocumento)
        Dim storedPrev = New HashSet(Of String)(
            anteriores.Where(Function(a) Not String.IsNullOrWhiteSpace(a.StoredName)).Select(Function(a) a.StoredName),
            StringComparer.OrdinalIgnoreCase)

        Using conn As New SqlConnection(GetProviderConnectionString())
            conn.Open()
            Using tran = conn.BeginTransaction()
                Try
                    For Each adjunto In attachments
                        If String.IsNullOrWhiteSpace(adjunto.StoredName) Then
                            If Not String.IsNullOrWhiteSpace(adjunto.OriginalPath) AndAlso File.Exists(adjunto.OriginalPath) Then
                                Dim extension = Path.GetExtension(adjunto.OriginalPath)
                                Dim storedName = Guid.NewGuid().ToString("N") & extension
                                Dim contenido = File.ReadAllBytes(adjunto.OriginalPath)
                                Dim addedAt = If(adjunto.AddedAt = DateTime.MinValue, DateTime.Now, adjunto.AddedAt)

                                Using cmd As New SqlCommand("INSERT INTO Tra_AdjuntoDocumento (IdDocumento, StoredName, DisplayName, AddedAt, Content) VALUES (@IdDocumento, @StoredName, @DisplayName, @AddedAt, @Content)", conn, tran)
                                    cmd.Parameters.AddWithValue("@IdDocumento", idDocumento)
                                    cmd.Parameters.AddWithValue("@StoredName", storedName)
                                    cmd.Parameters.AddWithValue("@DisplayName", adjunto.DisplayName)
                                    cmd.Parameters.AddWithValue("@AddedAt", addedAt)
                                    cmd.Parameters.Add("@Content", System.Data.SqlDbType.VarBinary, -1).Value = contenido
                                    cmd.ExecuteNonQuery()
                                End Using

                                adjunto.StoredName = storedName
                                adjunto.OriginalPath = ""
                            End If
                        End If
                    Next

                    Dim storedNew = New HashSet(Of String)(
                        attachments.Where(Function(a) Not String.IsNullOrWhiteSpace(a.StoredName)).Select(Function(a) a.StoredName),
                        StringComparer.OrdinalIgnoreCase)

                    For Each stored In storedPrev
                        If Not storedNew.Contains(stored) Then
                            Using cmd As New SqlCommand("DELETE FROM Tra_AdjuntoDocumento WHERE IdDocumento = @IdDocumento AND StoredName = @StoredName", conn, tran)
                                cmd.Parameters.AddWithValue("@IdDocumento", idDocumento)
                                cmd.Parameters.AddWithValue("@StoredName", stored)
                                cmd.ExecuteNonQuery()
                            End Using
                        End If
                    Next

                    tran.Commit()
                Catch
                    tran.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    Private Function GetAttachmentPathFromDatabase(idDocumento As Long, storedName As String) As String
        Dim tempFolder = Path.Combine(Path.GetTempPath(), BaseFolderName, SubFolderName, idDocumento.ToString())
        Directory.CreateDirectory(tempFolder)

        Dim tempPath = Path.Combine(tempFolder, storedName)
        If File.Exists(tempPath) Then
            Return tempPath
        End If

        Using conn As New SqlConnection(GetProviderConnectionString())
            conn.Open()
            Using cmd As New SqlCommand("SELECT Content FROM Tra_AdjuntoDocumento WHERE IdDocumento = @IdDocumento AND StoredName = @StoredName", conn)
                cmd.Parameters.AddWithValue("@IdDocumento", idDocumento)
                cmd.Parameters.AddWithValue("@StoredName", storedName)
                Dim content = TryCast(cmd.ExecuteScalar(), Byte())
                If content Is Nothing OrElse content.Length = 0 Then
                    Return ""
                End If

                File.WriteAllBytes(tempPath, content)
            End Using
        End Using

        Return tempPath
    End Function
End Module
