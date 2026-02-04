Imports System.Data.Entity

Public Class frmUsuarios

    Private Sub frmUsuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Verificamos si es Admin usando tu clase de sesión
        If Not SesionGlobal.EsAdmin Then
            MessageBox.Show("Acceso Denegado. Solo Administradores.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Me.Close()
            Return
        End If

        CargarUsuarios()
        cmbRol.SelectedIndex = 0
    End Sub

    Private Sub CargarUsuarios()
        Using db As New SecretariaDBEntities()
            ' Cargamos usuarios. Incluimos Cat_Oficina solo para visualización si se requiere a futuro.
            dgvUsuarios.DataSource = db.Cat_Usuario.Include("Cat_Oficina").Select(Function(u) New With {
                .ID = u.IdUsuario,
                .Nombre = u.NombreCompleto,
                .Login = u.UsuarioLogin,
                .Rol = u.Rol,
                .Oficina = If(u.Cat_Oficina Is Nothing, "Sin Oficina", u.Cat_Oficina.Nombre)
            }).ToList()
        End Using
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Or String.IsNullOrWhiteSpace(txtClave.Text) Then
            MessageBox.Show("Complete todos los campos.")
            Return
        End If

        Using db As New SecretariaDBEntities()
            Dim login = txtLogin.Text.Trim()

            ' Verificamos que no exista el login
            If db.Cat_Usuario.Any(Function(u) u.UsuarioLogin = login) Then
                MessageBox.Show("El usuario (login) ya existe.")
                Return
            End If

            Dim nuevo As New Cat_Usuario()
            nuevo.NombreCompleto = txtNombre.Text.Trim()
            nuevo.UsuarioLogin = login
            nuevo.Clave = txtClave.Text.Trim()
            nuevo.Rol = cmbRol.Text
            nuevo.Activo = True

            ' --- CAMBIO SOLICITADO: Asignación fija a Mesa de Entrada ---
            nuevo.IdOficina = 13
            ' ------------------------------------------------------------

            db.Cat_Usuario.Add(nuevo)
            db.SaveChanges()

            AuditoriaSistema.RegistrarEvento($"Alta de usuario {nuevo.UsuarioLogin} en Mesa de Entrada (ID 13).", "USUARIOS")
            MessageBox.Show("Usuario creado correctamente.")

            CargarUsuarios()
            Limpiar()
        End Using
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvUsuarios.SelectedRows.Count = 0 Then Return

        Dim idUser As Integer = Convert.ToInt32(dgvUsuarios.SelectedRows(0).Cells("ID").Value)

        ' Protección para no borrarse a uno mismo
        If idUser = SesionGlobal.UsuarioID Then
            MessageBox.Show("No puedes eliminar tu propio usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("¿Eliminar usuario?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Using db As New SecretariaDBEntities()
                Dim u = db.Cat_Usuario.Find(idUser)

                ' Si el usuario ya trabajó (tiene documentos o movimientos), solo lo desactivamos
                If u.Mae_Documento.Count > 0 Or u.Tra_Movimiento.Count > 0 Then
                    If MessageBox.Show("Este usuario tiene historial. ¿Desea desactivarlo (Baja Lógica)?", "Integridad", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        u.Activo = False
                        db.SaveChanges()
                        AuditoriaSistema.RegistrarEvento($"Usuario desactivado: {u.UsuarioLogin}.", "USUARIOS")
                        MessageBox.Show("Usuario desactivado.")
                    End If
                Else
                    ' Si es nuevo y no hizo nada, lo borramos del todo
                    Dim logs = db.EventosSistema.Where(Function(ev) ev.UsuarioId = idUser).ToList()
                    db.EventosSistema.RemoveRange(logs)

                    db.Cat_Usuario.Remove(u)
                    db.SaveChanges()
                    AuditoriaSistema.RegistrarEvento($"Usuario eliminado: {u.UsuarioLogin}.", "USUARIOS")
                    MessageBox.Show("Usuario eliminado.")
                End If

                CargarUsuarios()
            End Using
        End If
    End Sub

    Private Sub Limpiar()
        txtNombre.Text = ""
        txtLogin.Text = ""
        txtClave.Text = ""
    End Sub

End Class