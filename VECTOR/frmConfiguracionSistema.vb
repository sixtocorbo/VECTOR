Public Class frmConfiguracionSistema

    Private Async Sub frmConfiguracionSistema_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Await CargarConfiguracionAsync()
    End Sub

    Private Async Function CargarConfiguracionAsync() As Task
        Try
            Dim dias = Await ConfiguracionSistemaService.ObtenerDiasAlertaRenovacionesAsync()
            nudDiasAlertaRenovaciones.Value = Math.Max(nudDiasAlertaRenovaciones.Minimum, Math.Min(nudDiasAlertaRenovaciones.Maximum, dias))

            Dim mostrarSoloActivas = Await ConfiguracionSistemaService.ObtenerMostrarSoloActivasPorDefectoRenovacionesAsync()
            chkMostrarSoloActivasPorDefectoRenovaciones.Checked = mostrarSoloActivas
        Catch ex As Exception
            nudDiasAlertaRenovaciones.Value = ConfiguracionSistemaService.DiasAlertaRenovacionesPorDefecto
            chkMostrarSoloActivasPorDefectoRenovaciones.Checked = ConfiguracionSistemaService.MostrarSoloActivasPorDefectoRenovacionesPorDefecto
            Notifier.Warn(Me, "No se pudo cargar la configuración. Se mostrarán valores por defecto.")
        End Try
    End Function

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        btnGuardar.Enabled = False

        Try
            Dim usuario = "SISTEMA"

            Try
                usuario = SesionGlobal.NombreUsuario
            Catch
            End Try

            Await ConfiguracionSistemaService.GuardarDiasAlertaRenovacionesAsync(CInt(nudDiasAlertaRenovaciones.Value), usuario)
            Await ConfiguracionSistemaService.GuardarMostrarSoloActivasPorDefectoRenovacionesAsync(chkMostrarSoloActivasPorDefectoRenovaciones.Checked, usuario)
            Notifier.Success(Me, "Configuración guardada correctamente.")
            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            Notifier.[Error](Me, "No se pudo guardar la configuración: " & ex.Message)
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

End Class
