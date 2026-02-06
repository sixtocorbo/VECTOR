Imports System.Data.Entity

Public Class frmUsuarios

    Private usuarioEnEdicionId As Integer? = Nothing

    Private Async Sub frmUsuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        If Not SesionGlobal.EsAdmin Then
            Toast.Show(Me, "Acceso Denegado. Solo Administradores.", ToastType.Error)
            Me.Close()
            Return
        End If

        cmbRol.SelectedIndex = 0
        lblTituloLista.Text = "Usuarios Actuales (doble clic para editar)"
        Await CargarUsuariosAsync()
    End Sub

    Private Async Function CargarUsuariosAsync() As Task
        Using uow As New UnitOfWork()
            Dim repoUsuarios = uow.Repository(Of Cat_Usuario)()
            dgvUsuarios.DataSource = Await repoUsuarios.GetQueryable("Cat_Oficina").Select(Function(u) New With {
                .ID = u.IdUsuario,
                .Nombre = u.NombreCompleto,
                .Login = u.UsuarioLogin,
                .Rol = u.Rol,
                .Activo = u.Activo,
                .Oficina = If(u.Cat_Oficina Is Nothing, "Sin Oficina", u.Cat_Oficina.Nombre)
            }).ToListAsync()
        End Using
    End Function

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Or String.IsNullOrWhiteSpace(txtLogin.Text) Then
            Toast.Show(Me, "Complete nombre y login.", ToastType.Warning)
            Return
        End If

        Using uow As New UnitOfWork()
            Dim repoUsuarios = uow.Repository(Of Cat_Usuario)()
            Dim login = txtLogin.Text.Trim()

            If usuarioEnEdicionId.HasValue Then
                Dim idEdit = usuarioEnEdicionId.Value
                Dim usuario = Await repoUsuarios.FirstOrDefaultAsync(Function(u) u.IdUsuario = idEdit)

                If usuario Is Nothing Then
                    Toast.Show(Me, "Usuario no encontrado para edición.", ToastType.Warning)
                    SalirModoEdicion()
                    Await CargarUsuariosAsync()
                    Return
                End If

                If Await repoUsuarios.AnyAsync(Function(u) u.UsuarioLogin = login AndAlso u.IdUsuario <> idEdit) Then
                    Toast.Show(Me, "El usuario (login) ya existe.", ToastType.Warning)
                    Return
                End If

                usuario.NombreCompleto = txtNombre.Text.Trim()
                usuario.UsuarioLogin = login
                usuario.Rol = cmbRol.Text

                If Not String.IsNullOrWhiteSpace(txtClave.Text) Then
                    usuario.Clave = txtClave.Text.Trim()
                End If

                repoUsuarios.Update(usuario)
                Await uow.CommitAsync()

                AuditoriaSistema.RegistrarEvento($"Edición de usuario {usuario.UsuarioLogin}.", "USUARIOS")
                Toast.Show(Me, "Usuario actualizado correctamente.", ToastType.Success)
            Else
                If String.IsNullOrWhiteSpace(txtClave.Text) Then
                    Toast.Show(Me, "La contraseña es obligatoria para crear un usuario.", ToastType.Warning)
                    Return
                End If

                If Await repoUsuarios.AnyAsync(Function(u) u.UsuarioLogin = login) Then
                    Toast.Show(Me, "El usuario (login) ya existe.", ToastType.Warning)
                    Return
                End If

                Dim nuevo As New Cat_Usuario()
                nuevo.NombreCompleto = txtNombre.Text.Trim()
                nuevo.UsuarioLogin = login
                nuevo.Clave = txtClave.Text.Trim()
                nuevo.Rol = cmbRol.Text
                nuevo.Activo = True
                nuevo.IdOficina = 13

                repoUsuarios.Add(nuevo)
                Await uow.CommitAsync()

                AuditoriaSistema.RegistrarEvento($"Alta de usuario {nuevo.UsuarioLogin} en Mesa de Entrada (ID 13).", "USUARIOS")
                Toast.Show(Me, "Usuario creado correctamente.", ToastType.Success)
            End If

            Await CargarUsuariosAsync()
            Limpiar()
            SalirModoEdicion()
        End Using
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvUsuarios.SelectedRows.Count = 0 Then Return

        Dim idUser As Integer = Convert.ToInt32(dgvUsuarios.SelectedRows(0).Cells("ID").Value)

        If idUser = SesionGlobal.UsuarioID Then
            Toast.Show(Me, "No puedes eliminar tu propio usuario.", ToastType.Warning)
            Return
        End If

        If MessageBox.Show("¿Eliminar usuario?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Using uow As New UnitOfWork()
                Dim repoUsuarios = uow.Repository(Of Cat_Usuario)()
                Dim repoEventos = uow.Repository(Of EventosSistema)()

                Dim u = Await repoUsuarios.GetQueryable("Mae_Documento,Tra_Movimiento").FirstOrDefaultAsync(Function(x) x.IdUsuario = idUser)
                If u Is Nothing Then
                    Toast.Show(Me, "Usuario no encontrado.", ToastType.Warning)
                    Return
                End If

                If u.Mae_Documento.Count > 0 Or u.Tra_Movimiento.Count > 0 Then
                    If MessageBox.Show("Este usuario tiene historial. ¿Desea desactivarlo (Baja Lógica)?", "Integridad", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        u.Activo = False
                        repoUsuarios.Update(u)
                        Await uow.CommitAsync()
                        AuditoriaSistema.RegistrarEvento($"Usuario desactivado: {u.UsuarioLogin}.", "USUARIOS")
                        Toast.Show(Me, "Usuario desactivado.", ToastType.Success)
                    End If
                Else
                    Dim logs = Await repoEventos.GetAllByPredicateAsync(Function(ev) ev.UsuarioId = idUser)
                    repoEventos.RemoveRange(logs)
                    repoUsuarios.Remove(u)
                    Await uow.CommitAsync()
                    AuditoriaSistema.RegistrarEvento($"Usuario eliminado: {u.UsuarioLogin}.", "USUARIOS")
                    Toast.Show(Me, "Usuario eliminado.", ToastType.Success)
                End If

                Await CargarUsuariosAsync()
                Limpiar()
                SalirModoEdicion()
            End Using
        End If
    End Sub

    Private Sub dgvUsuarios_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUsuarios.CellDoubleClick
        If e.RowIndex < 0 OrElse dgvUsuarios.Rows.Count = 0 Then Return

        Dim row = dgvUsuarios.Rows(e.RowIndex)
        usuarioEnEdicionId = Convert.ToInt32(row.Cells("ID").Value)

        txtNombre.Text = Convert.ToString(row.Cells("Nombre").Value)
        txtLogin.Text = Convert.ToString(row.Cells("Login").Value)
        cmbRol.Text = Convert.ToString(row.Cells("Rol").Value)
        txtClave.Text = ""

        btnGuardar.Text = "ACTUALIZAR USUARIO"
        btnCancelarEdicion.Visible = True
        Toast.Show(Me, "Modo edición activado. Si deja la contraseña vacía, se mantiene la actual.", ToastType.Info)
    End Sub

    Private Sub btnCancelarEdicion_Click(sender As Object, e As EventArgs) Handles btnCancelarEdicion.Click
        Limpiar()
        SalirModoEdicion()
    End Sub

    Private Sub SalirModoEdicion()
        usuarioEnEdicionId = Nothing
        btnGuardar.Text = "GUARDAR USUARIO"
        btnCancelarEdicion.Visible = False
    End Sub

    Private Sub Limpiar()
        txtNombre.Text = ""
        txtLogin.Text = ""
        txtClave.Text = ""
        If cmbRol.Items.Count > 0 Then cmbRol.SelectedIndex = 0
    End Sub

End Class
