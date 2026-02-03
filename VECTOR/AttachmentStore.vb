'------------------------------------------------------------------------------
' Gestión de adjuntos digitales (almacenamiento en disco + manifiesto).
'------------------------------------------------------------------------------

Imports System.IO
Imports System.Xml.Serialization

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

    Public Function GetAttachmentFolder(idDocumento As Long) As String
        Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), BaseFolderName, SubFolderName, idDocumento.ToString())
    End Function

    Public Function GetManifestPath(idDocumento As Long) As String
        Return Path.Combine(GetAttachmentFolder(idDocumento), "manifest.xml")
    End Function

    Public Function LoadAttachments(idDocumento As Long) As List(Of AttachmentInfo)
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
        Return Path.Combine(GetAttachmentFolder(idDocumento), storedName)
    End Function
End Module
