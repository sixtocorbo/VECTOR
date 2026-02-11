Imports System.Data.Entity
Imports System.Data.SqlClient

Public NotInheritable Class ConfiguracionSistemaService

    Private Sub New()
    End Sub

    Public Const ClaveDiasAlertaRenovacionesArt120 As String = "RenovacionesArt120.DiasAlerta"
    Public Const ClaveMostrarSoloActivasPorDefectoRenovacionesArt120 As String = "RenovacionesArt120.MostrarSoloActivasPorDefecto"
    Public Const DiasAlertaRenovacionesPorDefecto As Integer = 30
    Public Const MostrarSoloActivasPorDefectoRenovacionesPorDefecto As Boolean = True

    Public Shared Async Function ObtenerDiasAlertaRenovacionesAsync() As Task(Of Integer)
        Dim valorGuardado = Await ObtenerValorAsync(ClaveDiasAlertaRenovacionesArt120).ConfigureAwait(False)
        Dim dias As Integer

        If Integer.TryParse(valorGuardado, dias) AndAlso dias > 0 Then
            Return dias
        End If

        Return DiasAlertaRenovacionesPorDefecto
    End Function

    Public Shared Async Function GuardarDiasAlertaRenovacionesAsync(dias As Integer, usuario As String) As Task
        If dias <= 0 Then
            Throw New ArgumentOutOfRangeException(NameOf(dias), "La cantidad de días debe ser mayor a cero.")
        End If

        Await GuardarValorAsync(
            ClaveDiasAlertaRenovacionesArt120,
            dias.ToString(),
            "Cantidad de días previos al vencimiento para marcar ALERTA en frmRenovacionesArt120.",
            usuario).ConfigureAwait(False)
    End Function

    Public Shared Async Function ObtenerMostrarSoloActivasPorDefectoRenovacionesAsync() As Task(Of Boolean)
        Dim valorGuardado = Await ObtenerValorAsync(ClaveMostrarSoloActivasPorDefectoRenovacionesArt120).ConfigureAwait(False)

        If String.IsNullOrWhiteSpace(valorGuardado) Then
            Return MostrarSoloActivasPorDefectoRenovacionesPorDefecto
        End If

        Dim valorNormalizado = valorGuardado.Trim().ToLowerInvariant()
        If valorNormalizado = "1" OrElse valorNormalizado = "true" OrElse valorNormalizado = "si" OrElse valorNormalizado = "sí" Then
            Return True
        End If

        If valorNormalizado = "0" OrElse valorNormalizado = "false" OrElse valorNormalizado = "no" Then
            Return False
        End If

        Return MostrarSoloActivasPorDefectoRenovacionesPorDefecto
    End Function

    Public Shared Async Function GuardarMostrarSoloActivasPorDefectoRenovacionesAsync(mostrarSoloActivas As Boolean, usuario As String) As Task
        Await GuardarValorAsync(
            ClaveMostrarSoloActivasPorDefectoRenovacionesArt120,
            If(mostrarSoloActivas, "1", "0"),
            "Define si la pantalla frmRenovacionesArt120 inicia filtrando solo salidas activas.",
            usuario).ConfigureAwait(False)
    End Function

    Private Shared Async Function ObtenerValorAsync(clave As String) As Task(Of String)
        Using uow As New UnitOfWork()
            Dim sql = "SELECT TOP(1) Valor FROM dbo.Cfg_SistemaParametros WHERE Clave = @clave"
            Dim parametroClave As New SqlParameter("@clave", clave)
            Dim valores = Await uow.Context.Database.SqlQuery(Of String)(sql, parametroClave).ToListAsync().ConfigureAwait(False)
            Return valores.FirstOrDefault()
        End Using
    End Function

    Private Shared Async Function GuardarValorAsync(clave As String, valor As String, descripcion As String, usuario As String) As Task
        Using uow As New UnitOfWork()
            Dim sql = "IF EXISTS (SELECT 1 FROM dbo.Cfg_SistemaParametros WHERE Clave = @clave) " &
                      "BEGIN " &
                      "UPDATE dbo.Cfg_SistemaParametros " &
                      "SET Valor = @valor, Descripcion = @descripcion, UsuarioActualizacion = @usuario, FechaActualizacion = SYSDATETIME() " &
                      "WHERE Clave = @clave " &
                      "END " &
                      "ELSE " &
                      "BEGIN " &
                      "INSERT INTO dbo.Cfg_SistemaParametros (Clave, Valor, Descripcion, UsuarioActualizacion, FechaActualizacion) " &
                      "VALUES (@clave, @valor, @descripcion, @usuario, SYSDATETIME()) " &
                      "END"

            Await uow.Context.Database.ExecuteSqlCommandAsync(
                sql,
                New SqlParameter("@clave", clave),
                New SqlParameter("@valor", valor),
                New SqlParameter("@descripcion", descripcion),
                New SqlParameter("@usuario", If(String.IsNullOrWhiteSpace(usuario), CType(DBNull.Value, Object), usuario))
            ).ConfigureAwait(False)
        End Using
    End Function

End Class
