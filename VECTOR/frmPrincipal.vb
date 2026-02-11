Imports System.ComponentModel

Public Class frmPrincipal

    ' =============================================================================
    ' CARGA Y CONFIGURACIÓN INICIAL
    ' =============================================================================
    Private Sub frmPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.IsMdiContainer = True

        ConfigurarBarraEstado()
        ConfigurarSeguridadMenu()

        ' Abrir la herramienta principal al iniciar
        AbrirFormularioHijo(Of frmBandeja)()
    End Sub

    Private Sub ConfigurarBarraEstado()
        Try
            lblEstadoUsuario.Text = "👤 Usuario: " & SesionGlobal.NombreUsuario
            lblEstadoOficina.Text = "🏢 Oficina: " & SesionGlobal.NombreOficina
        Catch ex As Exception
            lblEstadoUsuario.Text = "Modo Prueba"
        End Try
    End Sub

    Private Sub ConfigurarSeguridadMenu()
        ' Aquí ocultamos o deshabilitamos menús completos según el rol
        Dim esAdmin As Boolean = False
        Try
            esAdmin = SesionGlobal.EsAdmin
        Catch
            esAdmin = True ' Fallback para desarrollo
        End Try

        ' La pestaña SISTEMA y ADMINISTRACIÓN suele ser solo para admins
        ' Asumiendo que has renombrado los ToolStripMenuItem principales:
        ' MenuSistema.Visible = esAdmin
        ' MenuAdministracion.Visible = esAdmin 

        ' Manteniendo la lógica original para usuarios específicos:
        GestionUsuariosToolStripMenuItem.Enabled = esAdmin
    End Sub

    ' =============================================================================
    ' MOTOR MDI (AbrirFormularioHijo + Reparador)
    ' =============================================================================

    Private Sub AbrirFormularioHijo(Of T As {Form, New})()
        ShowUniqueFormInMdi(Of T)(Me, onClosed:=AddressOf AlCerrarCualquierHijo)
    End Sub

    Private Sub AlCerrarCualquierHijo(sender As Object, e As FormClosedEventArgs)
        ' Lógica de auto-reparación visual de la bandeja
        Dim fBandeja = Me.MdiChildren.OfType(Of frmBandeja)().FirstOrDefault()
        If fBandeja IsNot Nothing Then
            fBandeja.WindowState = FormWindowState.Normal
            fBandeja.WindowState = FormWindowState.Maximized
        End If
    End Sub

    ' =============================================================================
    ' SECCIÓN A: INICIO (Operación Diaria)
    ' =============================================================================

    Private Sub BandejaDeEntradaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BandejaDeEntradaToolStripMenuItem.Click
        AbrirFormularioHijo(Of frmBandeja)()
    End Sub

    ' He añadido esto porque vi el archivo frmBuscadorReclusos.vb en tu lista
    ' Te sugiero agregarlo al menú Inicio.
    Private Sub BuscadorReclusosToolStripMenuItem_Click(sender As Object, e As EventArgs)
        AbrirFormularioHijo(Of frmBuscadorReclusos)()
    End Sub

    Private Sub SalirToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalirToolStripMenuItem.Click
        ConfirmarSalida()
    End Sub

    ' =============================================================================
    ' SECCIÓN B: CONTROL (Reportes y Auditoría)
    ' =============================================================================

    Private Sub EstadisticasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EstadisticasToolStripMenuItem.Click
        AbrirFormularioHijo(Of frmEstadisticas)()
    End Sub

    Private Sub AuditoriaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AuditoriaToolStripMenuItem.Click
        AbrirFormularioHijo(Of frmAuditoria)()
    End Sub

    ' =============================================================================
    ' SECCIÓN C: ADMINISTRACIÓN (Datos Maestros)
    ' =============================================================================

    Private Sub GestionUsuariosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GestionUsuariosToolStripMenuItem.Click
        If VerificarPermisoAdmin() Then
            AbrirFormularioHijo(Of frmUsuarios)()
        End If
    End Sub

    Private Sub GestionRangosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GestionRangosToolStripMenuItem.Click
        AbrirFormularioHijo(Of frmGestionRangos)()
    End Sub

    Private Sub UnificarOficinasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnificarOficinasToolStripMenuItem.Click
        ' Esta es una herramienta peligrosa/avanzada, mejor tenerla bajo Admin
        If VerificarPermisoAdmin() Then
            AbrirFormularioHijo(Of frmUnificarOficinas)()
        End If
    End Sub

    ' =============================================================================
    ' SECCIÓN D: SISTEMA (Configuración Técnica)
    ' =============================================================================

    Private Sub ConfiguracionSistemaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfiguracionSistemaToolStripMenuItem.Click
        AbrirFormularioHijo(Of frmConfiguracionSistema)()
    End Sub

    Private Sub GestionTiposDocumentoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GestionTiposDocumentoToolStripMenuItem.Click
        AbrirFormularioHijo(Of frmGestionTiposDocumento)()
    End Sub

    Private Sub GestionTiemposToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GestionTiemposToolStripMenuItem.Click
        AbrirFormularioHijo(Of frmGestionTiempos)()
    End Sub

    ' =============================================================================
    ' UTILIDADES Y CIERRE
    ' =============================================================================

    Private Function VerificarPermisoAdmin() As Boolean
        Try
            If Not SesionGlobal.EsAdmin Then
                Toast.Show(Me, "Acceso denegado. Se requieren permisos de Administrador.", ToastType.Warning)
                Return False
            End If
            Return True
        Catch ex As Exception
            Return True ' Modo Fallback
        End Try
    End Function

    Private Sub ConfirmarSalida()
        If MessageBox.Show("¿Desea salir del sistema?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            AuditoriaSistema.RegistrarEvento("Salida del sistema desde menú.", "SISTEMA")
            Application.Exit()
        End If
    End Sub

    Private Sub frmPrincipal_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            If MessageBox.Show("¿Seguro que desea cerrar la aplicación VECTOR?", "Cerrar Sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                e.Cancel = True
            Else
                AuditoriaSistema.RegistrarEvento("Cierre de aplicación.", "SISTEMA")
            End If
        End If
    End Sub

End Class