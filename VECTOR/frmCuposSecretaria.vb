Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Drawing

Public Class frmCuposSecretaria
    Inherits Form

    Private WithEvents dgvCupos As New DataGridView()
    Private WithEvents cmbTipo As New ComboBox()
    Private WithEvents txtCantidad As New TextBox()
    Private WithEvents dtpFecha As New DateTimePicker()
    Private WithEvents txtUsuario As New TextBox()
    Private WithEvents btnAgregar As New Button()
    Private WithEvents btnActualizar As New Button()
    Private WithEvents btnEliminar As New Button()
    Private WithEvents btnCerrar As New Button()

    Private _idSeleccionado As Integer = 0

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Me.Text = "Cupos Secretaría General"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Size = New Size(900, 600)
        Me.BackColor = Color.WhiteSmoke

        dgvCupos.Location = New Point(20, 20)
        dgvCupos.Size = New Size(840, 300)
        dgvCupos.ReadOnly = True
        dgvCupos.MultiSelect = False
        dgvCupos.AllowUserToAddRows = False
        dgvCupos.AllowUserToDeleteRows = False
        dgvCupos.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvCupos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        Dim lblTipo As New Label() With {
            .Text = "Tipo de Documento:",
            .Location = New Point(20, 340),
            .AutoSize = True
        }
        cmbTipo.Location = New Point(20, 365)
        cmbTipo.Size = New Size(260, 28)
        cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList

        Dim lblCantidad As New Label() With {
            .Text = "Cantidad:",
            .Location = New Point(300, 340),
            .AutoSize = True
        }
        txtCantidad.Location = New Point(300, 365)
        txtCantidad.Size = New Size(140, 26)

        Dim lblFecha As New Label() With {
            .Text = "Fecha:",
            .Location = New Point(460, 340),
            .AutoSize = True
        }
        dtpFecha.Location = New Point(460, 365)
        dtpFecha.Size = New Size(200, 26)
        dtpFecha.Format = DateTimePickerFormat.Custom
        dtpFecha.CustomFormat = "dd/MM/yyyy HH:mm"

        Dim lblUsuario As New Label() With {
            .Text = "Usuario:",
            .Location = New Point(680, 340),
            .AutoSize = True
        }
        txtUsuario.Location = New Point(680, 365)
        txtUsuario.Size = New Size(180, 26)
        txtUsuario.ReadOnly = True
        txtUsuario.BackColor = Color.White

        btnAgregar.Text = "Agregar"
        btnAgregar.Location = New Point(20, 420)
        btnAgregar.Size = New Size(120, 40)

        btnActualizar.Text = "Actualizar"
        btnActualizar.Location = New Point(150, 420)
        btnActualizar.Size = New Size(120, 40)

        btnEliminar.Text = "Eliminar"
        btnEliminar.Location = New Point(280, 420)
        btnEliminar.Size = New Size(120, 40)
        btnEliminar.BackColor = Color.IndianRed
        btnEliminar.ForeColor = Color.White
        btnEliminar.FlatStyle = FlatStyle.Flat

        btnCerrar.Text = "Cerrar"
        btnCerrar.Location = New Point(740, 420)
        btnCerrar.Size = New Size(120, 40)

        Me.Controls.Add(dgvCupos)
        Me.Controls.Add(lblTipo)
        Me.Controls.Add(cmbTipo)
        Me.Controls.Add(lblCantidad)
        Me.Controls.Add(txtCantidad)
        Me.Controls.Add(lblFecha)
        Me.Controls.Add(dtpFecha)
        Me.Controls.Add(lblUsuario)
        Me.Controls.Add(txtUsuario)
        Me.Controls.Add(btnAgregar)
        Me.Controls.Add(btnActualizar)
        Me.Controls.Add(btnEliminar)
        Me.Controls.Add(btnCerrar)
    End Sub

    Private Async Sub frmCuposSecretaria_Load(sender As Object, e As EventArgs) Handles Me.Load
        txtUsuario.Text = If(String.IsNullOrWhiteSpace(SesionGlobal.NombreUsuario), "SIN SESIÓN", SesionGlobal.NombreUsuario)
        dtpFecha.Value = DateTime.Now

        Await CargarTiposAsync()
        Await CargarGrillaAsync()
    End Sub

    Private Async Function CargarTiposAsync() As Task
        Using uow As New UnitOfWork()
            Dim repoTipos = uow.Repository(Of Cat_TipoDocumento)()
            Dim tipos = Await repoTipos.GetQueryable().OrderBy(Function(t) t.Nombre).ToListAsync()
            cmbTipo.DataSource = tipos
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "IdTipo"
            cmbTipo.SelectedIndex = -1
        End Using
    End Function

    Private Async Function CargarGrillaAsync() As Task
        Using uow As New UnitOfWork()
            Try
                Dim sql As String = "SELECT c.IdCupo, c.Fecha, c.IdUsuario, c.IdTipo, c.Cantidad, " &
                                    "t.Nombre AS TipoNombre, " &
                                    "u.Nombre AS UsuarioNombre " &
                                    "FROM Mae_CuposSecretaria c " &
                                    "LEFT JOIN Cat_TipoDocumento t ON t.IdTipo = c.IdTipo " &
                                    "LEFT JOIN Cat_Usuario u ON u.IdUsuario = c.IdUsuario " &
                                    "ORDER BY c.Fecha DESC, c.IdCupo DESC"

                Dim lista = Await uow.Context.Database.SqlQuery(Of CupoSecretariaView)(sql).ToListAsync()
                dgvCupos.DataSource = lista

                If dgvCupos.Columns.Count > 0 Then
                    dgvCupos.Columns("IdCupo").Visible = False
                    dgvCupos.Columns("IdUsuario").Visible = False
                    dgvCupos.Columns("IdTipo").Visible = False
                    dgvCupos.Columns("TipoNombre").HeaderText = "Tipo"
                    dgvCupos.Columns("UsuarioNombre").HeaderText = "Usuario"
                    dgvCupos.Columns("Cantidad").HeaderText = "Cantidad"
                    dgvCupos.Columns("Fecha").HeaderText = "Fecha"
                End If
            Catch ex As Exception
                MessageBox.Show("No se pudo cargar la tabla de cupos. Verifique que Mae_CuposSecretaria exista.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Function

    Private Sub dgvCupos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCupos.SelectionChanged
        If dgvCupos.SelectedRows.Count = 0 Then Return

        Dim fila = dgvCupos.SelectedRows(0)
        _idSeleccionado = CInt(fila.Cells("IdCupo").Value)

        Dim idTipo As Integer = CInt(fila.Cells("IdTipo").Value)
        cmbTipo.SelectedValue = idTipo

        txtCantidad.Text = fila.Cells("Cantidad").Value.ToString()
        dtpFecha.Value = CDate(fila.Cells("Fecha").Value)
    End Sub

    Private Async Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Dim cantidad As Integer
        If cmbTipo.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione el tipo de documento.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If Not Integer.TryParse(txtCantidad.Text, cantidad) OrElse cantidad <= 0 Then
            MessageBox.Show("Ingrese una cantidad válida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using uow As New UnitOfWork()
            Dim usuarioId As Integer? = If(SesionGlobal.UsuarioID > 0, SesionGlobal.UsuarioID, CType(Nothing, Integer?))
            Dim parametros As SqlParameter() = {
                New SqlParameter("@Fecha", dtpFecha.Value),
                New SqlParameter("@IdUsuario", If(usuarioId.HasValue, CType(usuarioId.Value, Object), DBNull.Value)),
                New SqlParameter("@IdTipo", CInt(cmbTipo.SelectedValue)),
                New SqlParameter("@Cantidad", cantidad)
            }

            Dim sql As String = "INSERT INTO Mae_CuposSecretaria (Fecha, IdUsuario, IdTipo, Cantidad) " &
                                "VALUES (@Fecha, @IdUsuario, @IdTipo, @Cantidad)"

            Await uow.Context.Database.ExecuteSqlCommandAsync(sql, parametros)
        End Using

        Await CargarGrillaAsync()
        LimpiarSeleccion()
        DialogResult = DialogResult.OK
    End Sub

    Private Async Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        If _idSeleccionado = 0 Then
            MessageBox.Show("Seleccione un registro para actualizar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim cantidad As Integer
        If cmbTipo.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione el tipo de documento.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If Not Integer.TryParse(txtCantidad.Text, cantidad) OrElse cantidad <= 0 Then
            MessageBox.Show("Ingrese una cantidad válida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using uow As New UnitOfWork()
            Dim usuarioId As Integer? = If(SesionGlobal.UsuarioID > 0, SesionGlobal.UsuarioID, CType(Nothing, Integer?))
            Dim parametros As SqlParameter() = {
                New SqlParameter("@IdCupo", _idSeleccionado),
                New SqlParameter("@Fecha", dtpFecha.Value),
                New SqlParameter("@IdUsuario", If(usuarioId.HasValue, CType(usuarioId.Value, Object), DBNull.Value)),
                New SqlParameter("@IdTipo", CInt(cmbTipo.SelectedValue)),
                New SqlParameter("@Cantidad", cantidad)
            }

            Dim sql As String = "UPDATE Mae_CuposSecretaria " &
                                "SET Fecha = @Fecha, IdUsuario = @IdUsuario, IdTipo = @IdTipo, Cantidad = @Cantidad " &
                                "WHERE IdCupo = @IdCupo"

            Await uow.Context.Database.ExecuteSqlCommandAsync(sql, parametros)
        End Using

        Await CargarGrillaAsync()
        LimpiarSeleccion()
        DialogResult = DialogResult.OK
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _idSeleccionado = 0 Then
            MessageBox.Show("Seleccione un registro para eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("¿Desea eliminar este cupo?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
            Return
        End If

        Using uow As New UnitOfWork()
            Dim parametros As SqlParameter() = {
                New SqlParameter("@IdCupo", _idSeleccionado)
            }

            Dim sql As String = "DELETE FROM Mae_CuposSecretaria WHERE IdCupo = @IdCupo"
            Await uow.Context.Database.ExecuteSqlCommandAsync(sql, parametros)
        End Using

        Await CargarGrillaAsync()
        LimpiarSeleccion()
        DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub LimpiarSeleccion()
        _idSeleccionado = 0
        cmbTipo.SelectedIndex = -1
        txtCantidad.Clear()
        dtpFecha.Value = DateTime.Now
    End Sub
End Class
