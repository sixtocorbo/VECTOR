Imports System.Data.Entity
Imports System.Threading
Imports System.Threading.Tasks

Public Class frmBuscadorReclusos

    Public Property ResultadoFormateado As String = ""
    Private db As New SecretariaDBEntities()
    Private _versionCarga As Integer = 0

    Private Async Sub frmBuscadorReclusos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await CargarGrillaAsync()
        txtBuscar.Focus()
    End Sub

    Private Async Function CargarGrillaAsync() As Task
        Dim versionActual = Interlocked.Increment(_versionCarga)

        ' Empezamos con todos los activos
        Dim query = db.Mae_Reclusos.Where(Function(r) r.Activo = True)

        ' LÓGICA DE BÚSQUEDA FLEXIBLE (SIN ORDEN)
        If Not String.IsNullOrWhiteSpace(txtBuscar.Text) Then
            ' 1. Rompemos lo que escribió el usuario en palabras sueltas
            Dim palabras As String() = txtBuscar.Text.ToUpper().Split(" "c)

            ' 2. Filtramos: El nombre debe contener TODAS las palabras escritas
            For Each palabra In palabras
                If Not String.IsNullOrWhiteSpace(palabra) Then
                    Dim p = palabra ' Variable local para la lambda
                    query = query.Where(Function(r) r.NombreCompleto.Contains(p))
                End If
            Next
        End If

        ' Proyección simple
        Dim lista = Await query.OrderBy(Function(r) r.NombreCompleto).Select(Function(r) New With {
            .ID = r.IdRecluso,
            .Nombre = r.NombreCompleto
        }).ToListAsync()

        If versionActual <> _versionCarga Then Return

        dgvLista.DataSource = lista

        ' Ajustes visuales
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

        ' Ahora solo capturamos el nombre, limpio y sin documentos
        Dim nombre = dgvLista.SelectedRows(0).Cells("Nombre").Value.ToString()

        Me.ResultadoFormateado = nombre

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
