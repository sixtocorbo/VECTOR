Imports System.Data.Entity

Public Class frmUsuarios

    Private Sub frmUsuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' CORRECCIÓN 1: Usamos SesionGlobal (la clase real de tu proyecto)
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
            ' CORRECCIÓN 2: Usamos Cat_Usuario para no romper la BD
            dgvUsuarios.DataSource = db.Cat_Usuario.Select(Function(u) New With {
                .ID = u.IdUsuario,
                .Nombre = u.NombreCompleto,
                .Login = u.UsuarioLogin,
                .Rol = u.Rol
            }).ToList()
        End Using
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Or String.IsNullOrWhiteSpace(txtClave.Text) Then
            MessageBox.Show("Complete todos los campos.")
            Return
        End If

        Using db As New SecretariaDBEntities()
            Dim login = txtLogin.Text.Trim() ' Asumiendo que tienes un txtLogin

            ' Verificamos duplicados
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

            ' NOTA: Los usuarios nuevos no tienen oficina asignada por defecto? 
            ' Podrías asignarles una o dejarla nula si tu lógica lo permite.

            db.Cat_Usuario.Add(nuevo)
            db.SaveChanges()

            MessageBox.Show("Usuario creado correctamente.")
            CargarUsuarios()
            Limpiar()
        End Using
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvUsuarios.SelectedRows.Count = 0 Then Return

        Dim idUser As Integer = Convert.ToInt32(dgvUsuarios.SelectedRows(0).Cells("ID").Value)

        ' CORRECCIÓN 3: Evitar borrarse a sí mismo usando SesionGlobal
        If idUser = SesionGlobal.UsuarioID Then
            MessageBox.Show("No puedes eliminar tu propio usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("¿Eliminar usuario?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Using db As New SecretariaDBEntities()
                Dim u = db.Cat_Usuario.Find(idUser)

                ' Validación de seguridad: Si el usuario creó documentos, NO LO BORRAMOS FÍSICAMENTE
                If u.Mae_Documento.Count > 0 Or u.Tra_Movimiento.Count > 0 Then
                    If MessageBox.Show("Este usuario tiene historial de documentos. ¿Desea desactivarlo en lugar de borrarlo?", "Integridad", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        u.Activo = False
                        db.SaveChanges()
                        MessageBox.Show("Usuario desactivado (Baja Lógica).")
                    End If
                Else
                    ' Si está limpio, borramos logs y usuario
                    Dim logs = db.EventosSistema.Where(Function(ev) ev.UsuarioId = idUser).ToList()
                    db.EventosSistema.RemoveRange(logs)
                    db.Cat_Usuario.Remove(u)
                    db.SaveChanges()
                    MessageBox.Show("Usuario eliminado permanentemente.")
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