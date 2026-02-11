Imports System.Data.Entity

Public Class frmUsuarios

    Private _usuarioEnEdicionId As Integer?
    Private _operacionEnCurso As Boolean

    Private Async Sub frmUsuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        If Not SesionGlobal.EsAdmin Then
            Notifier.[Error](Me, "Acceso Denegado. Solo Administradores.")
            Me.ShowIcon = False
            Me.Close()
            Return
        End If

        Await CargarUsuariosAsync()
        cmbRol.SelectedIndex = 0
    End Sub

    Private Sub frmUsuarios_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If _operacionEnCurso Then
            e.Cancel = True
            Notifier.Warn(Me, "Hay una operación en curso. Espere a que finalice antes de cerrar.")
            Return
        End If

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
        If _operacionEnCurso Then Return

        Dim nombre = txtNombre.Text.Trim()
        Dim login = txtLogin.Text.Trim()
        Dim clave = txtClave.Text.Trim()
        Dim rol = cmbRol.Text

        If String.IsNullOrWhiteSpace(nombre) Or String.IsNullOrWhiteSpace(login) Then
            Notifier.Warn(Me, "Complete nombre y usuario.")
            Return
        End If

        _operacionEnCurso = True
        SetOperacionEnCursoUI(True)

        Try
            Using uow As New UnitOfWork()
                Dim repoUsuarios = uow.Repository(Of Cat_Usuario)()
                If _usuarioEnEdicionId.HasValue Then
                    Dim usuario = Await repoUsuarios.GetQueryable().FirstOrDefaultAsync(Function(u) u.IdUsuario = _usuarioEnEdicionId.Value)
                    If usuario Is Nothing Then
                        Notifier.Warn(Me, "Usuario no encontrado.")
                        Return
                    End If

                    If usuario.UsuarioLogin <> login AndAlso Await repoUsuarios.AnyAsync(Function(u) u.UsuarioLogin = login) Then
                        Notifier.Warn(Me, "El usuario (login) ya existe.")
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
                    Notifier.Success(Me, "Usuario actualizado correctamente.")
                Else
                    If String.IsNullOrWhiteSpace(clave) Then
                        Notifier.Warn(Me, "Complete la contraseña.")
                        Return
                    End If

                    If Await repoUsuarios.AnyAsync(Function(u) u.UsuarioLogin = login) Then
                        Notifier.Warn(Me, "El usuario (login) ya existe.")
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
                    Notifier.Success(Me, "Usuario creado correctamente.")
                End If

                Await CargarUsuariosAsync()
                Limpiar()
            End Using
        Catch ex As Exception
            Notifier.[Error](Me, "No se pudo guardar el usuario. Intente nuevamente.")
        Finally
            _operacionEnCurso = False
            SetOperacionEnCursoUI(False)
        End Try
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvUsuarios.SelectedRows.Count = 0 Then
            Notifier.Warn(Me, "Seleccione un usuario para editar.")
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
        If _operacionEnCurso Then Return

        If dgvUsuarios.SelectedRows.Count = 0 Then Return

        Dim idUser As Integer = Convert.ToInt32(dgvUsuarios.SelectedRows(0).Cells("ID").Value)

        If idUser = SesionGlobal.UsuarioID Then
            Notifier.Warn(Me, "No puedes eliminar tu propio usuario.")
            Return
        End If

        If MessageBox.Show("¿Eliminar usuario?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            _operacionEnCurso = True
            SetOperacionEnCursoUI(True)

            Try
                Using uow As New UnitOfWork()
                    Dim repoUsuarios = uow.Repository(Of Cat_Usuario)()
                    Dim repoEventos = uow.Repository(Of EventosSistema)()

                    Dim u = Await repoUsuarios.GetQueryable("Mae_Documento,Tra_Movimiento").FirstOrDefaultAsync(Function(x) x.IdUsuario = idUser)
                    If u Is Nothing Then
                        Notifier.Warn(Me, "Usuario no encontrado.")
                        Return
                    End If

                    If u.Mae_Documento.Count > 0 Or u.Tra_Movimiento.Count > 0 Then
                        If MessageBox.Show("Este usuario tiene historial. ¿Desea desactivarlo (Baja Lógica)?", "Integridad", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                            u.Activo = False
                            repoUsuarios.Update(u)
                            Await uow.CommitAsync()
                            AuditoriaSistema.RegistrarEvento($"Usuario desactivado: {u.UsuarioLogin}.", "USUARIOS")
                            Notifier.Success(Me, "Usuario desactivado.")
                        End If
                    Else
                        Dim logs = Await repoEventos.GetAllByPredicateAsync(Function(ev) ev.UsuarioId = idUser)
                        repoEventos.RemoveRange(logs)
                        repoUsuarios.Remove(u)
                        Await uow.CommitAsync()
                        AuditoriaSistema.RegistrarEvento($"Usuario eliminado: {u.UsuarioLogin}.", "USUARIOS")
                        Notifier.Success(Me, "Usuario eliminado.")
                    End If

                    Await CargarUsuariosAsync()
                    Limpiar()
                End Using
            Catch ex As Exception
                Notifier.[Error](Me, "No se pudo completar la operación sobre el usuario. Intente nuevamente.")
            Finally
                _operacionEnCurso = False
                SetOperacionEnCursoUI(False)
            End Try
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

    Private Sub SetOperacionEnCursoUI(operacionEnCurso As Boolean)
        btnGuardar.Enabled = Not operacionEnCurso
        btnEditar.Enabled = Not operacionEnCurso
        btnEliminar.Enabled = Not operacionEnCurso
        btnCancelar.Enabled = Not operacionEnCurso
        dgvUsuarios.Enabled = Not operacionEnCurso
        UseWaitCursor = operacionEnCurso
    End Sub

End Class
