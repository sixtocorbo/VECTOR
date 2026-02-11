Imports System.ComponentModel
Imports System.Data.Entity
Imports System.Drawing

Public Class frmGestionTiposDocumento

    Private Class TipoDocumentoViewModel
        Public Property IdTipo As Integer
        Public Property Nombre As String
        Public Property Codigo As String
        Public Property EsInterno As Boolean
        Public Property EsProtegido As Boolean
        Public Property EsNuevo As Boolean
    End Class

    Private _bindingList As BindingList(Of TipoDocumentoViewModel)
    Private ReadOnly _eliminados As New HashSet(Of Integer)
    Private _guardando As Boolean

    Private Async Sub frmGestionTiposDocumento_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.Text = "Gestión de Tipos de Documento"
        Await CargarDatosAsync()
    End Sub

    Private Async Function CargarDatosAsync() As Task
        Try
            dgvTipos.DataSource = Nothing

            Using uow As New UnitOfWork()
                Dim tipos = Await uow.Repository(Of Cat_TipoDocumento)().GetQueryable().
                    OrderBy(Function(t) t.Nombre).
                    ToListAsync()

                _bindingList = New BindingList(Of TipoDocumentoViewModel)()

                For Each t In tipos
                    Dim protegido = Not (t.EsInterno.HasValue AndAlso t.EsInterno.Value = False)

                    _bindingList.Add(New TipoDocumentoViewModel With {
                        .IdTipo = t.IdTipo,
                        .Nombre = t.Nombre,
                        .Codigo = t.Codigo,
                        .EsInterno = t.EsInterno.GetValueOrDefault(True),
                        .EsProtegido = protegido,
                        .EsNuevo = False
                    })
                Next

                dgvTipos.DataSource = _bindingList
                DiseñarGrilla()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar los tipos de documento: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Sub DiseñarGrilla()
        With dgvTipos
            .Columns("IdTipo").Visible = False
            .Columns("EsInterno").Visible = False
            .Columns("EsNuevo").Visible = False

            .Columns("Nombre").HeaderText = "Nombre"
            .Columns("Nombre").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            .Columns("Codigo").HeaderText = "Código"
            .Columns("Codigo").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            .Columns("EsProtegido").HeaderText = "Protegido"
            .Columns("EsProtegido").ReadOnly = True
            .Columns("EsProtegido").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        End With
    End Sub

    Private Sub dgvTipos_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvTipos.DataBindingComplete
        For Each row As DataGridViewRow In dgvTipos.Rows
            Dim item = TryCast(row.DataBoundItem, TipoDocumentoViewModel)
            If item IsNot Nothing AndAlso item.EsProtegido Then
                row.ReadOnly = True
                row.DefaultCellStyle.BackColor = Color.Gainsboro
            End If
        Next
    End Sub

    Private Sub dgvTipos_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvTipos.DataError
        MessageBox.Show("El valor ingresado no es válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        e.ThrowException = False
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        If _guardando Then
            Return
        End If

        If _bindingList Is Nothing Then
            _bindingList = New BindingList(Of TipoDocumentoViewModel)()
            dgvTipos.DataSource = _bindingList
            DiseñarGrilla()
        End If

        Dim nuevo As New TipoDocumentoViewModel With {
            .IdTipo = 0,
            .Nombre = "",
            .Codigo = "",
            .EsInterno = False,
            .EsProtegido = False,
            .EsNuevo = True
        }

        _bindingList.Add(nuevo)

        Dim rowIndex = _bindingList.IndexOf(nuevo)
        If rowIndex >= 0 Then
            dgvTipos.CurrentCell = dgvTipos.Rows(rowIndex).Cells("Nombre")
            dgvTipos.BeginEdit(True)
        End If
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _guardando Then
            Return
        End If

        Dim row = dgvTipos.CurrentRow
        If row Is Nothing Then
            Return
        End If

        Dim item = TryCast(row.DataBoundItem, TipoDocumentoViewModel)
        If item Is Nothing Then
            Return
        End If

        If item.EsProtegido Then
            MessageBox.Show("Este tipo de documento está protegido y no puede eliminarse.", "Acción no permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("¿Desea eliminar el tipo seleccionado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        If item.IdTipo > 0 Then
            _eliminados.Add(item.IdTipo)
        End If

        _bindingList.Remove(item)
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If _guardando Then
            Return
        End If

        _guardando = True
        SetGuardadoUIState(True)

        Try
            Dim lista = If(_bindingList IsNot Nothing, _bindingList.ToList(), New List(Of TipoDocumentoViewModel))

            If lista.Any(Function(item) Not item.EsProtegido AndAlso (String.IsNullOrWhiteSpace(item.Nombre) OrElse String.IsNullOrWhiteSpace(item.Codigo))) Then
                MessageBox.Show("Complete el nombre y el código para los tipos editables.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim codigoDuplicado = lista.Where(Function(item) Not item.EsProtegido).
                GroupBy(Function(item) item.Codigo.Trim().ToUpperInvariant()).
                Any(Function(grupo) grupo.Count() > 1)

            If codigoDuplicado Then
                MessageBox.Show("No puede haber tipos con código duplicado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Using uow As New UnitOfWork()
                Dim repoTipos = uow.Repository(Of Cat_TipoDocumento)()

                For Each id In _eliminados
                    Dim entidad = Await repoTipos.GetQueryable().FirstOrDefaultAsync(Function(t) t.IdTipo = id)
                    If entidad Is Nothing Then
                        Continue For
                    End If

                    Dim esProtegido = Not (entidad.EsInterno.HasValue AndAlso entidad.EsInterno.Value = False)
                    If esProtegido Then
                        Continue For
                    End If

                    repoTipos.Remove(entidad)
                Next

                For Each item In lista
                    If item.EsProtegido Then
                        Continue For
                    End If

                    Dim nombre = item.Nombre.Trim()
                    Dim codigo = item.Codigo.Trim()

                    If item.EsNuevo Then
                        Dim nuevo As New Cat_TipoDocumento With {
                            .Nombre = nombre,
                            .Codigo = codigo,
                            .EsInterno = False
                        }
                        repoTipos.Add(nuevo)
                    Else
                        Dim entidad = Await repoTipos.GetQueryable().FirstOrDefaultAsync(Function(t) t.IdTipo = item.IdTipo)
                        If entidad Is Nothing Then
                            Continue For
                        End If

                        Dim esProtegido = Not (entidad.EsInterno.HasValue AndAlso entidad.EsInterno.Value = False)
                        If esProtegido Then
                            Continue For
                        End If

                        entidad.Nombre = nombre
                        entidad.Codigo = codigo
                        entidad.EsInterno = False
                    End If
                Next

                Await uow.CommitAsync()
            End Using

            _eliminados.Clear()
            MessageBox.Show("¡Tipos de documento actualizados correctamente!", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Await CargarDatosAsync()
        Catch ex As Exception
            MessageBox.Show("Error al guardar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _guardando = False
            SetGuardadoUIState(False)
        End Try
    End Sub

    Private Sub frmGestionTiposDocumento_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not _guardando Then
            Return
        End If

        e.Cancel = True
        MessageBox.Show("Hay un guardado en progreso. Espere a que finalice para cerrar la ventana.", "Guardado en progreso", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub SetGuardadoUIState(guardando As Boolean)
        btnGuardar.Enabled = Not guardando
        btnNuevo.Enabled = Not guardando
        btnEliminar.Enabled = Not guardando
        dgvTipos.Enabled = Not guardando
        UseWaitCursor = guardando
    End Sub
End Class
