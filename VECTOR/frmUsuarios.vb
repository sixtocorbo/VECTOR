Imports System.Data.Entity

Public Class frmUsuarios

    Private _usuarioEnEdicionId As Integer?

    Private Async Sub frmUsuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        If Not SesionGlobal.EsAdmin Then
            Toast.Show(Me, "Acceso Denegado. Solo Administradores.", ToastType.Error)
            Me.ShowIcon = False
            Me.Close()
            Return
        End If

        Await CargarUsuariosAsync()
        cmbRol.SelectedIndex = 0
    End Sub

    Private Sub frmUsuarios_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.ShowIcon = False
    End Sub

    Private Async Function CargarUsuariosAsync() As Task
        Using uow As New UnitOfWork()
            Dim repoUsuarios = uow.Repository(Of Cat_Usuario)()
            dgvUsuarios.DataSource = Await repoUsuarios.GetQueryable("Cat_Oficina").Select(Function(u) New With {
                .ID = u.IdUsuario,
                .Nombre = u.NombreCompleto,
                .Login = u.UsuarioLogin,
                .Rol = u.Rol,
                .Oficina = If(u.Cat_Oficina Is Nothing, "Sin Oficina", u.Cat_Oficina.Nombre)
            }).ToListAsync()
        End Using
    End Function

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim nombre = txtNombre.Text.Trim()
        Dim login = txtLogin.Text.Trim()
        Dim clave = txtClave.Text.Trim()
        Dim rol = cmbRol.Text

        If String.IsNullOrWhiteSpace(nombre) Or String.IsNullOrWhiteSpace(login) Then
            Toast.Show(Me, "Complete nombre y usuario.", ToastType.Warning)
            Return
        End If

        Using uow As New UnitOfWork()
            Dim repoUsuarios = uow.Repository(Of Cat_Usuario)()
            If _usuarioEnEdicionId.HasValue Then
                Dim usuario = Await repoUsuarios.GetQueryable().FirstOrDefaultAsync(Function(u) u.IdUsuario = _usuarioEnEdicionId.Value)
                If usuario Is Nothing Then
                    Toast.Show(Me, "Usuario no encontrado.", ToastType.Warning)
                    Return
                End If

                If usuario.UsuarioLogin <> login AndAlso Await repoUsuarios.AnyAsync(Function(u) u.UsuarioLogin = login) Then
                    Toast.Show(Me, "El usuario (login) ya existe.", ToastType.Warning)
                    Return
                End If

                usuario.NombreCompleto = nombre
                usuario.UsuarioLogin = login
                usuario.Rol = rol

                If Not String.IsNullOrWhiteSpace(clave) Then
                    usuario.Clave = clave
                End If

                repoUsuarios.Update(usuario)
                Await uow.CommitAsync()
                AuditoriaSistema.RegistrarEvento($"Usuario actualizado: {usuario.UsuarioLogin}.", "USUARIOS")
                Toast.Show(Me, "Usuario actualizado correctamente.", ToastType.Success)
            Else
                If String.IsNullOrWhiteSpace(clave) Then
                    Toast.Show(Me, "Complete la contraseña.", ToastType.Warning)
                    Return
                End If

                If Await repoUsuarios.AnyAsync(Function(u) u.UsuarioLogin = login) Then
                    Toast.Show(Me, "El usuario (login) ya existe.", ToastType.Warning)
                    Return
                End If

                Dim nuevo As New Cat_Usuario()
                nuevo.NombreCompleto = nombre
                nuevo.UsuarioLogin = login
                nuevo.Clave = clave
                nuevo.Rol = rol
                nuevo.Activo = True
                nuevo.IdOficina = 13

                repoUsuarios.Add(nuevo)
                Await uow.CommitAsync()

                AuditoriaSistema.RegistrarEvento($"Alta de usuario {nuevo.UsuarioLogin} en Mesa de Entrada (ID 13).", "USUARIOS")
                Toast.Show(Me, "Usuario creado correctamente.", ToastType.Success)
            End If

            Await CargarUsuariosAsync()
            Limpiar()
        End Using
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvUsuarios.SelectedRows.Count = 0 Then
            Toast.Show(Me, "Seleccione un usuario para editar.", ToastType.Warning)
            Return
        End If

        Dim idUser As Integer = Convert.ToInt32(dgvUsuarios.SelectedRows(0).Cells("ID").Value)
        Dim nombre As String = dgvUsuarios.SelectedRows(0).Cells("Nombre").Value.ToString()
        Dim login As String = dgvUsuarios.SelectedRows(0).Cells("Login").Value.ToString()
        Dim rol As String = dgvUsuarios.SelectedRows(0).Cells("Rol").Value.ToString()

        _usuarioEnEdicionId = idUser
        txtNombre.Text = nombre
        txtLogin.Text = login
        txtClave.Text = ""
        cmbRol.SelectedItem = rol
        lblModoEdicion.Text = "Modo: Edición"
        btnGuardar.Text = "ACTUALIZAR USUARIO"
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
            End Using
        End If
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Limpiar()
    End Sub

    Private Sub Limpiar()
        txtNombre.Text = ""
        txtLogin.Text = ""
        txtClave.Text = ""
        cmbRol.SelectedIndex = 0
        _usuarioEnEdicionId = Nothing
        lblModoEdicion.Text = "Modo: Alta"
        btnGuardar.Text = "GUARDAR USUARIO"
    End Sub

End Class
