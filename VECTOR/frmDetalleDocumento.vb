Imports System.Data.Entity
Imports System.Threading.Tasks

Public Class frmDetalleDocumento

    Private _idDocumento As Long

    ' Constructor que recibe el ID
    Public Sub New(idDoc As Long)
        InitializeComponent()
        _idDocumento = idDoc
    End Sub

    Private Async Sub frmDetalleDocumento_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Aplicamos tu tema visual si existe
            AppTheme.Aplicar(Me)

            Me.Text = "Cargando detalles..."
            Await CargarDatosAsync()
        Catch ex As Exception
            MessageBox.Show("Error al cargar detalle: " & ex.Message)
        End Try
    End Sub

    Private Async Function CargarDatosAsync() As Task
        Using uow As New UnitOfWork()
            Dim repo = uow.Repository(Of Mae_Documento)()

            ' 1. Consultamos el documento con sus relaciones clave
            ' Usamos Include para traer Tipo, Oficina y Movimientos en una sola consulta
            Dim doc = Await repo.GetQueryable(tracking:=False) _
                                .Include("Cat_TipoDocumento") _
                                .Include("Cat_Oficina") _
                                .Include("Cat_Estado") _
                                .Include("Tra_Movimiento") _
                                .Include("Tra_Movimiento.Cat_Oficina") _
                                .Include("Tra_Movimiento.Cat_Usuario") _
                                .FirstOrDefaultAsync(Function(d) d.IdDocumento = _idDocumento)

            If doc Is Nothing Then
                MessageBox.Show("El documento no existe o fue eliminado.")
                Me.Close()
                Return
            End If

            ' 2. Llenar Cabecera (Asegúrate de tener estos Labels en tu diseño)
            Me.Text = "Detalle: " & doc.NumeroOficial

            ' Supongamos que tienes labels llamados lblNumero, lblAsunto, lblEstado, etc.
            ' Si no los tienes, puedes ajustar esto a tu diseño o usar un MsgBox de prueba.
            lblNumero.Text = doc.Cat_TipoDocumento.Nombre & " " & doc.NumeroOficial
            lblAsunto.Text = doc.Asunto
            lblEstado.Text = doc.Cat_Estado.Nombre
            lblUbicacion.Text = "Ubicación Actual: " & doc.Cat_Oficina.Nombre
            lblFecha.Text = "Creado el: " & doc.FechaCreacion.Value.ToShortDateString()

            ' Colorear estado según sea necesario
            If doc.IdEstadoActual = 5 Then ' Anulado
                lblEstado.ForeColor = Color.Red
            Else
                lblEstado.ForeColor = Color.DarkGreen
            End If

            ' 3. Llenar Grilla de Movimientos (Historial)
            ' Proyectamos los datos para mostrar solo lo útil
            Dim historial = doc.Tra_Movimiento.OrderByDescending(Function(m) m.FechaMovimiento).Select(Function(m) New With {
                .Fecha = m.FechaMovimiento,
                .Origen = If(m.Cat_Oficina IsNot Nothing, m.Cat_Oficina.Nombre, "N/A"),
                .Destino = If(m.Cat_Oficina1 IsNot Nothing, m.Cat_Oficina1.Nombre, "N/A"),
                .Responsable = If(m.Cat_Usuario IsNot Nothing, m.Cat_Usuario.UsuarioLogin, "Sistema"),
                .Observacion = m.ObservacionPase
            }).ToList()

            dgvMovimientos.DataSource = historial
            ConfigurarGrilla()

        End Using
    End Function

    Private Sub ConfigurarGrilla()
        With dgvMovimientos
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True

            ' Ajustes estéticos
            If .Columns("Fecha") IsNot Nothing Then .Columns("Fecha").Width = 120
            If .Columns("Observacion") IsNot Nothing Then .Columns("Observacion").FillWeight = 200
        End With
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub
End Class