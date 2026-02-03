Imports System.Data.Entity

Public Class frmLogin

    Private Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        Dim u As String = txtUsuario.Text.Trim()
        Dim p As String = txtClave.Text.Trim()

        ' Validación de campos vacíos
        If String.IsNullOrEmpty(u) OrElse String.IsNullOrEmpty(p) Then
            MessageBox.Show("Por favor, ingrese su usuario y contraseña.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            ' Uso del contexto de base de datos de VECTOR
            Using db As New SecretariaDBEntities()

                ' Buscamos al usuario en Cat_Usuario
                ' Verificamos que coincida el login, la clave y que esté activo
                Dim usuario = db.Cat_Usuario.FirstOrDefault(Function(x) x.UsuarioLogin = u AndAlso x.Clave = p AndAlso x.Activo = True)

                If usuario IsNot Nothing Then
                    ' Determinamos la oficina: si no tiene una asignada, usamos la 1 por defecto
                    Dim idOficina As Integer = If(usuario.IdOficina.HasValue, usuario.IdOficina.Value, 1)
                    Dim nombreOficina As String = If(usuario.Cat_Oficina IsNot Nothing, usuario.Cat_Oficina.Nombre, "Mesa de Entrada")

                    ' 1. Inicializamos la Sesión Global con los datos de VECTOR
                    SesionGlobal.Iniciar(usuario.IdUsuario, usuario.NombreCompleto, usuario.Rol, idOficina, nombreOficina)

                    ' 2. Registro en la tabla de auditoría EventosSistema
                    AuditoriaSistema.RegistrarEvento($"Inicio de sesión exitoso. Oficina: {nombreOficina}.", "LOGIN", usuario.IdUsuario)

                    ' 3. Transición al formulario principal
                    Dim principal As New frmPrincipal()
                    principal.Show()
                    Me.Hide()
                Else
                    MessageBox.Show("Credenciales incorrectas o usuario desactivado.", "Error de Acceso", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al conectar con la base de datos: " & ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Application.Exit()
    End Sub

    ' Permite ingresar presionando la tecla Enter en el campo de clave
    Private Sub txtClave_KeyDown(sender As Object, e As KeyEventArgs) Handles txtClave.KeyDown
        If e.KeyCode = Keys.Enter Then btnIngresar.PerformClick()
    End Sub
End Class
