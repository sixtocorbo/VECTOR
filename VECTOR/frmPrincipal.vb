Public Class frmPrincipal

    Private Sub frmPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ' 1. Configuración Visual
        Me.IsMdiContainer = True

        ' 2. Mostrar datos de Sesión en la barra inferior
        Try
            ' NOTA: Asegúrate de que SesionGlobal existe en tu proyecto.
            ' Si da error, comenta estas líneas temporalmente.
            lblEstadoUsuario.Text = "👤 Usuario: " & SesionGlobal.NombreUsuario
            lblEstadoOficina.Text = "🏢 Oficina: " & SesionGlobal.NombreOficina
        Catch ex As Exception
            lblEstadoUsuario.Text = "Usuario: Admin (Modo Prueba)"
            lblEstadoOficina.Text = "Oficina: Mesa Central"
        End Try

        ConfigurarPermisosMenu()

        ' 3. Opcional: Abrir la bandeja automáticamente al iniciar
        ' Si aún no tienes frmBandeja, comenta esta línea
        AbrirFormularioHijo(Of frmBandeja)()
    End Sub

    Private Sub ConfigurarPermisosMenu()
        Dim esAdmin As Boolean

        Try
            esAdmin = SesionGlobal.EsAdmin
        Catch ex As Exception
            esAdmin = True
        End Try

        GestionUsuariosToolStripMenuItem.Enabled = esAdmin
    End Sub

    ' =================================================================
    ' MÉTODO GENÉRICO MAESTRO PARA ABRIR VENTANAS HIJAS
    ' =================================================================
    Private Sub AbrirFormularioHijo(Of T As {Form, New})()

        ' Buscamos si ya hay un hijo de este tipo abierto
        Dim formulario As Form = Me.MdiChildren.FirstOrDefault(Function(f) TypeOf f Is T)

        If formulario Is Nothing Then
            ' A. NO EXISTE -> LO CREAMOS
            formulario = New T()
            formulario.MdiParent = Me
            formulario.WindowState = FormWindowState.Maximized
            formulario.Show()
        Else
            ' B. YA EXISTE -> LO TRAEMOS AL FRENTE
            If formulario.WindowState = FormWindowState.Minimized Then
                formulario.WindowState = FormWindowState.Maximized
            End If

            formulario.Activate()
            formulario.BringToFront()
        End If
    End Sub

    ' =================================================================
    ' EVENTOS DEL MENÚ
    ' =================================================================

    Private Sub BandejaDeEntradaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BandejaDeEntradaToolStripMenuItem.Click
        AbrirFormularioHijo(Of frmBandeja)()
    End Sub

    Private Sub GestionRangosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GestionRangosToolStripMenuItem.Click
        ' Asegúrate de tener frmGestionRangos creado, o comenta esta línea si aún no existe
        AbrirFormularioHijo(Of frmGestionRangos)()
    End Sub

    Private Sub GestionUsuariosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GestionUsuariosToolStripMenuItem.Click
        If Not PuedeGestionarUsuarios() Then
            Toast.Show(Me, "Solo los usuarios con rol Administrador pueden gestionar usuarios.", ToastType.Warning)
            Return
        End If

        AbrirFormularioHijo(Of frmUsuarios)()
    End Sub

    Private Sub AuditoriaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AuditoriaToolStripMenuItem.Click
        AbrirFormularioHijo(Of frmAuditoria)()
    End Sub

    ' --- NUEVO: EVENTO PARA LA HERRAMIENTA DE UNIFICAR ---
    Private Sub UnificarOficinasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnificarOficinasToolStripMenuItem.Click
        AbrirFormularioHijo(Of frmUnificarOficinas)()
    End Sub
    ' -----------------------------------------------------

    Private Sub SalirToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalirToolStripMenuItem.Click
        If MessageBox.Show("¿Desea salir del sistema?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            AuditoriaSistema.RegistrarEvento("Salida del sistema desde menú principal.", "SISTEMA")
            Application.Exit()
        End If
    End Sub

    Private Sub frmPrincipal_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            If MessageBox.Show("¿Seguro que desea cerrar el sistema?", "VECTOR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                e.Cancel = True
            Else
                AuditoriaSistema.RegistrarEvento("Salida del sistema desde cierre de ventana.", "SISTEMA")
            End If
        End If
    End Sub

    Private Function PuedeGestionarUsuarios() As Boolean
        Try
            Return SesionGlobal.EsAdmin
        Catch ex As Exception
            Return True
        End Try
    End Function

End Class
