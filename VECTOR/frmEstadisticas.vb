Imports System.Data.Entity

Public Class frmEstadisticas

    Private Async Sub frmEstadisticas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await CargarEstadisticasAsync()
    End Sub

    Private Async Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        Await CargarEstadisticasAsync()
    End Sub

    Private Async Function CargarEstadisticasAsync() As Task
        btnActualizar.Enabled = False

        Try
            Dim hoy = Date.Today
            Dim desde7 = hoy.AddDays(-6)
            Dim desde30 = hoy.AddDays(-29)
            Dim desdeMes = New Date(hoy.Year, hoy.Month, 1).AddMonths(-11)

            Dim data = Await Task.Run(Function()
                                           Using db As New SecretariaDBEntities()
                                               Dim result As New EstadisticasData With {
                                                   .TotalDocumentos = db.Mae_Documento.Count(),
                                                   .TotalMovimientos = db.Tra_Movimiento.Count(),
                                                   .TotalReclusos = db.Mae_Reclusos.Count(),
                                                   .TotalUsuarios = db.Cat_Usuario.Count(),
                                                   .TotalOficinas = db.Cat_Oficina.Count(),
                                                   .TotalTiposDocumento = db.Cat_TipoDocumento.Count(),
                                                   .TotalEstados = db.Cat_Estado.Count(),
                                                   .TotalRangos = db.Mae_NumeracionRangos.Count(),
                                                   .TotalAdjuntos = db.Tra_AdjuntoDocumento.Count(),
                                                   .DocumentosHoy = db.Mae_Documento.Count(Function(d) d.FechaCreacion.HasValue AndAlso d.FechaCreacion.Value >= hoy),
                                                   .DocumentosUltimos7 = db.Mae_Documento.Count(Function(d) d.FechaCreacion.HasValue AndAlso d.FechaCreacion.Value >= desde7),
                                                   .DocumentosUltimos30 = db.Mae_Documento.Count(Function(d) d.FechaCreacion.HasValue AndAlso d.FechaCreacion.Value >= desde30),
                                                   .MovimientosHoy = db.Tra_Movimiento.Count(Function(m) m.FechaMovimiento.HasValue AndAlso m.FechaMovimiento.Value >= hoy),
                                                   .MovimientosUltimos7 = db.Tra_Movimiento.Count(Function(m) m.FechaMovimiento.HasValue AndAlso m.FechaMovimiento.Value >= desde7),
                                                   .MovimientosUltimos30 = db.Tra_Movimiento.Count(Function(m) m.FechaMovimiento.HasValue AndAlso m.FechaMovimiento.Value >= desde30),
                                                   .UsuariosActivos = db.Cat_Usuario.Count(Function(u) u.Activo.HasValue AndAlso u.Activo.Value),
                                                   .UsuariosInactivos = db.Cat_Usuario.Count(Function(u) u.Activo.HasValue AndAlso Not u.Activo.Value),
                                                   .OficinasExternas = db.Cat_Oficina.Count(Function(o) o.EsExterna.HasValue AndAlso o.EsExterna.Value),
                                                   .OficinasInternas = db.Cat_Oficina.Count(Function(o) Not o.EsExterna.HasValue OrElse Not o.EsExterna.Value)
                                               }

                                               result.DocumentosPorEstado = db.Mae_Documento.GroupBy(Function(d) d.Cat_Estado.Nombre).Select(Function(g) New ConteoItem With {
                                                   .Nombre = g.Key,
                                                   .Total = g.Count()
                                               }).OrderByDescending(Function(i) i.Total).ToList()

                                               result.DocumentosPorTipo = db.Mae_Documento.GroupBy(Function(d) d.Cat_TipoDocumento.Nombre).Select(Function(g) New ConteoItem With {
                                                   .Nombre = g.Key,
                                                   .Total = g.Count()
                                               }).OrderByDescending(Function(i) i.Total).ToList()

                                               result.MovimientosPorOficinaDestino = db.Tra_Movimiento.GroupBy(Function(m) m.Cat_Oficina1.Nombre).Select(Function(g) New ConteoItem With {
                                                   .Nombre = g.Key,
                                                   .Total = g.Count()
                                               }).OrderByDescending(Function(i) i.Total).ToList()

                                               Dim movimientosMes = db.Tra_Movimiento.Where(Function(m) m.FechaMovimiento.HasValue AndAlso m.FechaMovimiento.Value >= desdeMes).
                                                   Select(Function(m) m.FechaMovimiento.Value).ToList()

                                               result.MovimientosPorMes = movimientosMes.
                                                   GroupBy(Function(f) New Date(f.Year, f.Month, 1)).
                                                   OrderBy(Function(g) g.Key).
                                                   Select(Function(g) New MovimientoMesItem With {
                                                       .Periodo = g.Key.ToString("MMM yyyy"),
                                                       .Total = g.Count()
                                                   }).ToList()

                                               Return result
                                           End Using
                                       End Function)

            AplicarDatos(data)
        Catch ex As Exception
            MessageBox.Show("No se pudieron cargar las estadísticas. " & ex.Message, "Estadísticas", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            btnActualizar.Enabled = True
        End Try
    End Function

    Private Sub AplicarDatos(data As EstadisticasData)
        lblTotalDocumentos.Text = data.TotalDocumentos.ToString("N0")
        lblTotalMovimientos.Text = data.TotalMovimientos.ToString("N0")
        lblTotalReclusos.Text = data.TotalReclusos.ToString("N0")
        lblTotalUsuarios.Text = data.TotalUsuarios.ToString("N0")
        lblTotalOficinas.Text = data.TotalOficinas.ToString("N0")
        lblTotalTipos.Text = data.TotalTiposDocumento.ToString("N0")
        lblTotalEstados.Text = data.TotalEstados.ToString("N0")
        lblTotalRangos.Text = data.TotalRangos.ToString("N0")
        lblTotalAdjuntos.Text = data.TotalAdjuntos.ToString("N0")

        lblDocumentosHoy.Text = data.DocumentosHoy.ToString("N0")
        lblDocumentos7.Text = data.DocumentosUltimos7.ToString("N0")
        lblDocumentos30.Text = data.DocumentosUltimos30.ToString("N0")

        lblMovimientosHoy.Text = data.MovimientosHoy.ToString("N0")
        lblMovimientos7.Text = data.MovimientosUltimos7.ToString("N0")
        lblMovimientos30.Text = data.MovimientosUltimos30.ToString("N0")

        lblUsuariosActivos.Text = data.UsuariosActivos.ToString("N0")
        lblUsuariosInactivos.Text = data.UsuariosInactivos.ToString("N0")
        lblOficinasInternas.Text = data.OficinasInternas.ToString("N0")
        lblOficinasExternas.Text = data.OficinasExternas.ToString("N0")

        NormalizarConteos(data.DocumentosPorEstado, "Sin estado")
        NormalizarConteos(data.DocumentosPorTipo, "Sin tipo")
        NormalizarConteos(data.MovimientosPorOficinaDestino, "Sin destino")

        dgvDocumentosPorEstado.DataSource = data.DocumentosPorEstado
        dgvDocumentosPorTipo.DataSource = data.DocumentosPorTipo
        dgvMovimientosPorOficina.DataSource = data.MovimientosPorOficinaDestino
        dgvMovimientosPorMes.DataSource = data.MovimientosPorMes

        ConfigurarGrid(dgvDocumentosPorEstado)
        ConfigurarGrid(dgvDocumentosPorTipo)
        ConfigurarGrid(dgvMovimientosPorOficina)
        ConfigurarGrid(dgvMovimientosPorMes)
    End Sub

    Private Sub NormalizarConteos(items As List(Of ConteoItem), textoPorDefecto As String)
        For Each item In items
            If String.IsNullOrWhiteSpace(item.Nombre) Then
                item.Nombre = textoPorDefecto
            End If
        Next
    End Sub

    Private Sub ConfigurarGrid(grid As DataGridView)
        grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.MultiSelect = False
    End Sub

    Private Class EstadisticasData
        Public Property TotalDocumentos As Integer
        Public Property TotalMovimientos As Integer
        Public Property TotalReclusos As Integer
        Public Property TotalUsuarios As Integer
        Public Property TotalOficinas As Integer
        Public Property TotalTiposDocumento As Integer
        Public Property TotalEstados As Integer
        Public Property TotalRangos As Integer
        Public Property TotalAdjuntos As Integer
        Public Property DocumentosHoy As Integer
        Public Property DocumentosUltimos7 As Integer
        Public Property DocumentosUltimos30 As Integer
        Public Property MovimientosHoy As Integer
        Public Property MovimientosUltimos7 As Integer
        Public Property MovimientosUltimos30 As Integer
        Public Property UsuariosActivos As Integer
        Public Property UsuariosInactivos As Integer
        Public Property OficinasInternas As Integer
        Public Property OficinasExternas As Integer
        Public Property DocumentosPorEstado As List(Of ConteoItem)
        Public Property DocumentosPorTipo As List(Of ConteoItem)
        Public Property MovimientosPorOficinaDestino As List(Of ConteoItem)
        Public Property MovimientosPorMes As List(Of MovimientoMesItem)
    End Class

    Private Class ConteoItem
        Public Property Nombre As String
        Public Property Total As Integer
    End Class

    Private Class MovimientoMesItem
        Public Property Periodo As String
        Public Property Total As Integer
    End Class

End Class
