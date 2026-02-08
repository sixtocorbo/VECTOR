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
                                .Include("Mae_Documento2") _
                                .Include("Mae_Documento2.Cat_TipoDocumento") _
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

            Dim infoRelacion As String = "Documento independiente."
            Dim infoUltimaActuacion As String = "Última actuación: sin movimientos registrados."

            If doc.IdDocumentoPadre.HasValue AndAlso doc.Mae_Documento2 IsNot Nothing Then
                Dim padre = doc.Mae_Documento2
                Dim padreEtiqueta = $"{padre.Cat_TipoDocumento.Nombre} {padre.NumeroOficial}"
                Dim totalAdjuntos = Await repo.GetQueryable(tracking:=False) _
                    .CountAsync(Function(h) h.IdDocumentoPadre = padre.IdDocumento AndAlso h.IdEstadoActual <> 5)

                infoRelacion = $"Documento hijo de {padreEtiqueta}. Es parte de {totalAdjuntos} adjuntos a ese padre."

                Dim idsFamilia = Await repo.GetQueryable(tracking:=False) _
                    .Where(Function(h) h.IdDocumentoPadre = padre.IdDocumento OrElse h.IdDocumento = padre.IdDocumento) _
                    .Select(Function(h) h.IdDocumento) _
                    .ToListAsync()

                Dim ultima = Await uow.Context.Set(Of Tra_Movimiento)() _
                    .Include("Mae_Documento.Cat_TipoDocumento") _
                    .Where(Function(m) idsFamilia.Contains(m.IdDocumento)) _
                    .OrderByDescending(Function(m) m.FechaMovimiento) _
                    .Select(Function(m) New With {
                        .Fecha = m.FechaMovimiento,
                        .Tipo = m.Mae_Documento.Cat_TipoDocumento.Nombre,
                        .Numero = m.Mae_Documento.NumeroOficial
                    }) _
                    .FirstOrDefaultAsync()

                If ultima IsNot Nothing Then
                    infoUltimaActuacion = $"Última actuación ({ultima.Fecha:dd/MM/yyyy HH:mm}) en {ultima.Tipo} {ultima.Numero}."
                End If
            Else
                Dim ultima = doc.Tra_Movimiento _
                    .OrderByDescending(Function(m) m.FechaMovimiento) _
                    .Select(Function(m) New With {
                        .Fecha = m.FechaMovimiento,
                        .Tipo = doc.Cat_TipoDocumento.Nombre,
                        .Numero = doc.NumeroOficial
                    }) _
                    .FirstOrDefault()

                If ultima IsNot Nothing Then
                    infoUltimaActuacion = $"Última actuación ({ultima.Fecha:dd/MM/yyyy HH:mm}) en {ultima.Tipo} {ultima.Numero}."
                End If
            End If

            lblRelacion.Text = infoRelacion
            lblUltimaActuacion.Text = infoUltimaActuacion

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
