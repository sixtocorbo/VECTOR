Imports System.Data.Entity
Imports System.Threading
Imports System.Threading.Tasks

Public Class frmBuscadorReclusos

    Public Property ResultadoFormateado As String = ""
    Public Property ResultadoIdRecluso As Nullable(Of Integer)
    Private _versionCarga As Integer = 0

    Private Async Sub frmBuscadorReclusos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        UIUtils.SetPlaceholder(txtBuscar, "Escriba para buscar por apellido, nombre o documento...")
        Await CargarGrillaAsync()
        txtBuscar.Focus()
    End Sub

    Private Async Function CargarGrillaAsync() As Task
        Dim versionActual = Interlocked.Increment(_versionCarga)

        Using uow As New UnitOfWork()
            Dim repoReclusos = uow.Repository(Of Mae_Reclusos)()
            Dim query = repoReclusos.GetQueryable().Where(Function(r) r.Activo = True)

            If Not String.IsNullOrWhiteSpace(txtBuscar.Text) Then
                Dim palabras As String() = txtBuscar.Text.ToUpper().Split(" "c)
                For Each palabra In palabras
                    If Not String.IsNullOrWhiteSpace(palabra) Then
                        Dim p = palabra
                        query = query.Where(Function(r) r.NombreCompleto.Contains(p))
                    End If
                Next
            End If

            Dim lista = Await query.OrderBy(Function(r) r.NombreCompleto).Select(Function(r) New With {
                .ID = r.IdRecluso,
                .Nombre = r.NombreCompleto
            }).ToListAsync()

            If versionActual <> _versionCarga Then Return

            dgvLista.DataSource = lista
        End Using

        If dgvLista.Columns.Count > 0 Then
            dgvLista.Columns("ID").Visible = False
            dgvLista.Columns("Nombre").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvLista.Columns("Nombre").HeaderText = "Nombre del Recluso"
        End If
    End Function

    Private Async Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Await CargarGrillaAsync()
    End Sub

    Private Sub dgvLista_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLista.CellDoubleClick
        SeleccionarYSalir()
    End Sub

    Private Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
        SeleccionarYSalir()
    End Sub

    Private Sub SeleccionarYSalir()
        If dgvLista.SelectedRows.Count = 0 Then Return

        Dim id = CInt(dgvLista.SelectedRows(0).Cells("ID").Value)
        Dim nombre = dgvLista.SelectedRows(0).Cells("Nombre").Value.ToString()

        Me.ResultadoIdRecluso = id
        Me.ResultadoFormateado = nombre

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
