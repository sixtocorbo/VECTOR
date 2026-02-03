Public Class frmPrincipal

    Private Sub frmPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 1. Configuración Visual
        Me.IsMdiContainer = True

        ' 2. Mostrar datos de Sesión en la barra inferior
        Try
            lblEstadoUsuario.Text = "👤 Usuario: " & SesionGlobal.NombreUsuario
            lblEstadoOficina.Text = "🏢 Oficina: " & SesionGlobal.NombreOficina
        Catch ex As Exception
            lblEstadoUsuario.Text = "Usuario: Admin (Modo Prueba)"
            lblEstadoOficina.Text = "Oficina: Mesa Central"
        End Try

        ' 3. Opcional: Abrir la bandeja automáticamente al iniciar
        AbrirFormularioHijo(Of frmBandeja)()
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

    ' --- NUEVO EVENTO PARA ABRIR LA GESTIÓN DE RANGOS ---
    Private Sub GestionRangosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GestionRangosToolStripMenuItem.Click
        AbrirFormularioHijo(Of frmGestionRangos)()
    End Sub
    ' -----------------------------------------------------

    Private Sub SalirToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalirToolStripMenuItem.Click
        If MessageBox.Show("¿Desea salir del sistema?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub

    Private Sub frmPrincipal_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            If MessageBox.Show("¿Seguro que desea cerrar el sistema?", "VECTOR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                e.Cancel = True
            End If
        End If
    End Sub

End Class