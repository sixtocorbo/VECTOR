Imports System.Data.Entity
Imports System.Drawing
Imports System.Text
Imports System.Reflection
Imports System.Threading.Tasks
Imports System.Windows.Forms ' Necesario para los Anchors

Public Class frmBandeja

    Private Const IdBandejaEntrada As Integer = 13
    Private _fontNormal As Font
    Private _fontItalic As Font

    ' 1. LISTA EN MEMORIA
    Private _listaOriginal As New List(Of Object)

    ' 2. TIMER DE BÚSQUEDA
    Private WithEvents _timerBusqueda As New Timer()
    Private Const DiasAnticipacionAlertaSalidas As Integer = 30
    Private _cantAlertasArt120 As Integer = 0
    Private _cantVencidasArt120 As Integer = 0
    Private _ultimoResumenArt120 As String = ""
    Private _cerrando As Boolean = False

    Private Function FormularioDisponible() As Boolean
        Return Not _cerrando AndAlso Not Me.IsDisposed AndAlso Not Me.Disposing AndAlso dgvPendientes IsNot Nothing AndAlso Not dgvPendientes.IsDisposed
    End Function

    ' =======================================================
    ' CARGA INICIAL
    ' =======================================================
    Private Sub AjustarAlContenedorMdi()
        If Me.MdiParent Is Nothing Then Return

        Me.FormBorderStyle = FormBorderStyle.None
        Me.ControlBox = False
        Me.WindowState = FormWindowState.Normal
        Me.Dock = DockStyle.Fill
    End Sub

    Private Async Sub frmBandeja_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AppTheme.Aplicar(Me)
            ' Configuración inicial

            AjustarAlContenedorMdi()

            UIUtils.SetPlaceholder(txtBuscar, "Buscar por Nombre, ID, Tipo, etc...")

            ' --- TRUCO PRO: DOBLE BUFFER ---
            Dim typeDGV As Type = dgvPendientes.GetType()
            Dim propertyInfo As PropertyInfo = typeDGV.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
            propertyInfo.SetValue(dgvPendientes, True, Nothing)

            ' --- CONFIGURACIÓN ANTI-LAG ---
            _timerBusqueda.Interval = 500

            ' Configuración Visual
            Me.Text = "VECTOR - Bandeja de: " & SesionGlobal.NombreOficina & " (" & SesionGlobal.NombreUsuario & ")"
            _fontNormal = dgvPendientes.Font
            _fontItalic = New Font(dgvPendientes.Font, FontStyle.Italic)

            ConfigurarBotones(False, False, False, False)
            AjustarLayoutInferior()
            Await CargarGrillaAsync()
        Catch ex As Exception
            Me.Text = "VECTOR - Sistema de Gestión"
            ConfigurarBotones(False, False, False, False)
        End Try
    End Sub

    Private Sub frmBandeja_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        AjustarAlContenedorMdi()
        AjustarLayoutInferior()
    End Sub


    ' Recalcula y ancla los botones inferiores para evitar que queden fuera de vista
    ' cuando el formulario se maximiza dentro del contenedor MDI.
    Private Sub AjustarLayoutInferior()
        If PanelInferior.ClientSize.Width <= 0 Then Return

        Dim margen As Integer = 10
        Dim xActual As Integer = PanelInferior.ClientSize.Width - margen
        Dim topPos As Integer = 20 ' Altura estándar según tu diseño

        ' 1. Lista de botones que van a la DERECHA (en orden de aparición de Der a Izq)
        Dim botonesDerecha As Button() = {btnHistorial, btnVincular, btnEliminar, btnEditar, btnDesvincular}

        For Each btn In botonesDerecha
            ' Primero lo colocamos en su sitio
            btn.Location = New Point(xActual - btn.Width, topPos)
            ' Luego le decimos que se quede pegado a la esquina inferior derecha
            btn.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
            ' Calculamos la posición del siguiente botón
            xActual -= (btn.Width + margen)
        Next

        ' 2. Botón de la IZQUIERDA (Refrescar)
        btnRefrescar.Location = New Point(18, topPos)
        btnRefrescar.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
    End Sub

    Private Sub frmBandeja_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If _fontItalic IsNot Nothing Then
            _fontItalic.Dispose()
            _fontItalic = Nothing
        End If
    End Sub

    Private Sub frmBandeja_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        _cerrando = True
        _timerBusqueda.Stop()
        Me.ShowIcon = False
    End Sub

    ' =======================================================
    ' 1. CARGA DESDE BASE DE DATOS (Unit of Work)
    ' =======================================================
    Private Async Function CargarGrillaAsync(Optional forzarNotificacionAlertasArt120 As Boolean = False) As Task
        Try
            Using uow As New UnitOfWork()
                uow.Context.Configuration.LazyLoadingEnabled = False

                Dim repo = uow.Repository(Of Mae_Documento)()
                Dim consulta = repo.GetQueryable()

                ' A. FILTROS BD
                consulta = consulta.Where(Function(d) d.IdEstadoActual <> 5)

                If Not chkVerDerivados.Checked Then
                    consulta = consulta.Where(Function(d) d.IdOficinaActual = SesionGlobal.OficinaID)
                End If

                consulta = consulta.OrderByDescending(Function(d) d.FechaCreacion)

                ' B. PROYECCIÓN DE DATOS PUROS
                Dim listaDatos = Await consulta.Select(Function(d) New With {
                    .ID = d.IdDocumento,
                    .Tipo = d.Cat_TipoDocumento.Nombre,
                    .Referencia = d.NumeroOficial,
                    .Remitente = d.Tra_Movimiento.OrderByDescending(Function(m) m.IdMovimiento).Select(Function(m) m.Cat_Oficina.Nombre).FirstOrDefault(),
                    .Asunto = d.Asunto,
                    .Descripcion = d.Descripcion,
                    .Ubicacion = d.Cat_Oficina.Nombre,
                    .Fecha = d.FechaRecepcion,
                    .Estado = d.Cat_Estado.Nombre,
                    .IdOficinaActual = d.IdOficinaActual,
                    .Cant_Respuestas = d.Mae_Documento1.Where(Function(h) h.IdEstadoActual <> 5).Count(),
                    .EsHijo = d.IdDocumentoPadre.HasValue,
                    .IdDocumentoPadre = d.IdDocumentoPadre,
                    .RefPadre = If(d.IdDocumentoPadre.HasValue, d.Mae_Documento2.Cat_TipoDocumento.Nombre & " " & d.Mae_Documento2.NumeroOficial, ""),
                    .Vencimiento = d.FechaVencimiento,
                    .Semaforo = d.EstadoSemaforo
                }).ToListAsync()

                ' C. AJUSTES FINALES Y FORMATEO DE TEXTO
                Dim listaFinal = listaDatos.Select(Function(x) New With {
                    .ID = x.ID,
                    .Tipo = x.Tipo,
                    .Referencia = x.Referencia,
                    .Fecha = x.Fecha,
                    .Estado = If(x.EsHijo,
                                 "(Adjunto al " & x.RefPadre & ")",
                                 x.Estado & If(x.Cant_Respuestas > 0, " (" & x.Cant_Respuestas & ")", "")),
                    .Origen = If(String.IsNullOrEmpty(x.Remitente), "Ingreso Inicial", x.Remitente),
                    .Ubicacion = x.Ubicacion,
                    .Asunto = x.Asunto,
                    .Descripcion = x.Descripcion,
                    .IdOficinaActual = x.IdOficinaActual,
                    .Cant_Respuestas = x.Cant_Respuestas,
                    .EsHijo = x.EsHijo,
                    .IdDocumentoPadre = x.IdDocumentoPadre,
                    .RefPadre = x.RefPadre,
                    .Vencimiento = x.Vencimiento,
                    .Semaforo = x.Semaforo
                }).ToList()

                _listaOriginal = New List(Of Object)(listaFinal)

            End Using

            Await CargarAlertasSalidasArt120Async(forzarNotificacionAlertasArt120)

            ' E. MOSTRAMOS
            If Not FormularioDisponible() Then Return
            AplicarFiltroRapido()

        Catch ex As ObjectDisposedException
            Return
        Catch ex As Exception
            If FormularioDisponible() Then
                Notifier.[Error](Me, "Error al cargar datos: " & ex.Message)
            End If
        End Try
    End Function

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        _timerBusqueda.Stop()
        _timerBusqueda.Start()
    End Sub

    Private Sub _timerBusqueda_Tick(sender As Object, e As EventArgs) Handles _timerBusqueda.Tick
        _timerBusqueda.Stop()
        AplicarFiltroRapido()
    End Sub

    Private Sub AplicarFiltroRapido()
        If Not FormularioDisponible() Then Return
        If _listaOriginal Is Nothing Then Return

        Dim textoBusqueda As String = txtBuscar.Text.ToUpper().Trim()
        Dim resultado As List(Of Object)

        If String.IsNullOrWhiteSpace(textoBusqueda) Then
            resultado = _listaOriginal
        Else
            Dim palabrasClave As String() = textoBusqueda.Split(" "c)

            resultado = _listaOriginal.Where(Function(item As Object)
                                                 Dim superString As String = (item.ID.ToString() & " " &
                                                  item.Tipo & " " &
                                                  If(item.Referencia IsNot Nothing, item.Referencia.ToString(), "") & " " &
                                                  item.Origen & " " &
                                                  item.Asunto & " " &
                                                  item.Descripcion & " " &
                                                  item.RefPadre & " " &
                                                  item.Ubicacion).ToString().ToUpper()

                                                 Dim cumpleTodas As Boolean = True
                                                 For Each palabra In palabrasClave
                                                     If Not String.IsNullOrWhiteSpace(palabra) Then
                                                         If Not superString.Contains(palabra) Then
                                                             cumpleTodas = False
                                                             Exit For
                                                         End If
                                                     End If
                                                 Next
                                                 Return cumpleTodas
                                             End Function).ToList()
        End If

        dgvPendientes.AutoGenerateColumns = True
        dgvPendientes.Columns.Clear()
        dgvPendientes.DataSource = resultado
        DiseñarColumnas()
        dgvPendientes.ClearSelection()
        If dgvPendientes.RowCount > 0 Then
            dgvPendientes.Rows(0).Selected = True
        End If
        ActualizarBotonesPorSeleccion()

        Dim totalExpedientes As Integer =
    resultado.Select(Function(item)
                         Return If(CBool(item.EsHijo), item.IdDocumentoPadre, item.ID)
                     End Function).
              Distinct().
              Count()

        lblContador.Text = "Expedientes: " & totalExpedientes & " | Alertas Art.120: " & _cantAlertasArt120

        dgvPendientes.Refresh()
    End Sub

    Private Async Function CargarAlertasSalidasArt120Async(Optional forzarNotificacion As Boolean = False) As Task
        Try
            Using uow As New UnitOfWork()
                Dim hoy = DateTime.Today
                Dim repoSalidas = uow.Repository(Of Tra_SalidasLaborales)()

                Dim resumen = Await repoSalidas.GetQueryable() _
                    .Where(Function(s) s.Activo.HasValue AndAlso s.Activo.Value) _
                    .GroupBy(Function(s) 1) _
                    .Select(Function(g) New With {
                        .Vencidas = g.Count(Function(s) DbFunctions.DiffDays(hoy, s.FechaVencimiento) < 0),
                        .Alertas = g.Count(Function(s) DbFunctions.DiffDays(hoy, s.FechaVencimiento) >= 0 AndAlso DbFunctions.DiffDays(hoy, s.FechaVencimiento) <= DiasAnticipacionAlertaSalidas)
                    }) _
                    .FirstOrDefaultAsync()

                _cantVencidasArt120 = If(resumen Is Nothing, 0, resumen.Vencidas)
                _cantAlertasArt120 = If(resumen Is Nothing, 0, resumen.Vencidas + resumen.Alertas)
            End Using

            If _cantAlertasArt120 <= 0 Then
                _ultimoResumenArt120 = ""
                Return
            End If

            Dim resumenTexto = $"Vencidas: {_cantVencidasArt120} | Por vencer ({DiasAnticipacionAlertaSalidas} días): {_cantAlertasArt120 - _cantVencidasArt120}"
            If forzarNotificacion OrElse _ultimoResumenArt120 <> resumenTexto Then
                Notifier.Warn(Me, "⚠ Alertas de salidas laborales Art. 120" & vbCrLf & resumenTexto)
                _ultimoResumenArt120 = resumenTexto
            End If
        Catch ex As Exception
            _cantAlertasArt120 = 0
            _cantVencidasArt120 = 0
        End Try
    End Function

    Private Sub MostrarAlertasInteresantes()
        If _listaOriginal Is Nothing OrElse _listaOriginal.Count = 0 Then Return

        Dim hoy As Date = Date.Today
        Dim proximosDias As Integer = 7

        Dim totalVencidos As Integer = _listaOriginal.Count(Function(item)
                                                                If item.Vencimiento Is Nothing Then Return False
                                                                Dim fechaVenc As Date = CDate(item.Vencimiento)
                                                                Return fechaVenc < hoy
                                                            End Function)

        Dim totalProximos As Integer = _listaOriginal.Count(Function(item)
                                                                If item.Vencimiento Is Nothing Then Return False
                                                                Dim fechaVenc As Date = CDate(item.Vencimiento)
                                                                Return fechaVenc >= hoy AndAlso fechaVenc <= hoy.AddDays(proximosDias)
                                                            End Function)

        Dim totalExternos As Integer = _listaOriginal.Count(Function(item)
                                                                If item.IdOficinaActual Is Nothing Then Return False
                                                                Return CInt(item.IdOficinaActual) <> SesionGlobal.OficinaID
                                                            End Function)

        If totalVencidos > 0 Then
            Notifier.Warn(Me, $"⚠ Bandeja: {totalVencidos} documento(s) vencido(s).")
        End If

        If totalProximos > 0 Then
            Notifier.Info(Me, $"ℹ Bandeja: {totalProximos} documento(s) vencen en los próximos {proximosDias} días.")
        End If

        If totalExternos > 0 Then
            Notifier.Info(Me, $"ℹ Bandeja: {totalExternos} documento(s) están en otra oficina.")
        End If
    End Sub

    Private Sub DiseñarColumnas()
        If Not FormularioDisponible() Then Return
        If dgvPendientes.Columns.Count = 0 Then Return

        ' 1. DEFINIR COLUMNAS QUE QUEREMOS VER
        Dim columnasVisibles As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
            "ID",
            "Vencimiento",
            "Tipo",
            "Referencia",
            "Fecha",
            "Estado",
            "Origen",
            "Ubicacion",
            "Asunto",
            "Descripcion",
            "Observaciones"
        }

        ' 2. OCULTAR COLUMNAS TÉCNICAS
        For Each columna As DataGridViewColumn In dgvPendientes.Columns
            If Not columnasVisibles.Contains(columna.Name) Then
                columna.Visible = False
            End If
        Next

        ' 3. CONFIGURAR COLUMNAS ESPECÍFICAS
        With dgvPendientes
            .Columns("ID").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("ID").HeaderText = "ID"
            .Columns("ID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("ID").Visible = True

            If .Columns.Contains("Vencimiento") Then
                With .Columns("Vencimiento")
                    .Visible = True
                    .HeaderText = "Vence"
                    .DefaultCellStyle.Format = "dd/MM"
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Try
                        .DisplayIndex = 1
                    Catch
                    End Try
                End With
            End If

            .Columns("Tipo").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Tipo").HeaderText = "Tipo"
            .Columns("Tipo").Visible = True

            .Columns("Referencia").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Referencia").HeaderText = "Referencia"
            .Columns("Referencia").DefaultCellStyle.Font = New Font(dgvPendientes.Font, FontStyle.Bold)
            .Columns("Referencia").DisplayIndex = 3
            .Columns("Referencia").Visible = True

            .Columns("Fecha").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Fecha").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("Fecha").Visible = True

            .Columns("Estado").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Estado").Visible = True

            .Columns("Origen").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Origen").HeaderText = "Origen"
            .Columns("Origen").Visible = True

            .Columns("Ubicacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Ubicacion").HeaderText = "Ubicación"
            .Columns("Ubicacion").Visible = True

            If .Columns.Contains("Asunto") Then
                .Columns("Asunto").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                .Columns("Asunto").MinimumWidth = 220
                .Columns("Asunto").Visible = True
            End If

            If .Columns.Contains("Descripcion") Then
                .Columns("Descripcion").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                .Columns("Descripcion").MinimumWidth = 220
                .Columns("Descripcion").HeaderText = "Descripción"
                .Columns("Descripcion").Visible = True
            End If
        End With
    End Sub

    ' =======================================================
    ' 3. SEMÁFORO DE COLORES
    ' =======================================================
    Private Sub dgvPendientes_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles dgvPendientes.RowPrePaint
        If e.RowIndex < 0 Then Return
        If Not dgvPendientes.Columns.Contains("IdOficinaActual") Then Return
        If Not dgvPendientes.Columns.Contains("Cant_Respuestas") Then Return
        If Not dgvPendientes.Columns.Contains("EsHijo") Then Return

        Dim fila = dgvPendientes.Rows(e.RowIndex)
        If fila.Cells("IdOficinaActual").Value Is Nothing Then Return

        Dim idOficinaDoc As Integer = CInt(fila.Cells("IdOficinaActual").Value)
        Dim esMio As Boolean = (idOficinaDoc = SesionGlobal.OficinaID)
        Dim cantRespuestas As Integer = If(fila.Cells("Cant_Respuestas").Value Is Nothing, 0, CInt(fila.Cells("Cant_Respuestas").Value))
        Dim esHijo As Boolean = If(fila.Cells("EsHijo").Value Is Nothing, False, CBool(fila.Cells("EsHijo").Value))
        Dim esDocumentoPadre As Boolean = (Not esHijo) AndAlso (cantRespuestas > 0)

        If esMio Then
            fila.DefaultCellStyle.ForeColor = Color.Black
            fila.DefaultCellStyle.Font = _fontNormal
        Else
            fila.DefaultCellStyle.ForeColor = Color.Gray
            fila.DefaultCellStyle.Font = _fontItalic
        End If

        fila.DefaultCellStyle.BackColor = If(esDocumentoPadre, Color.LightYellow, Color.White)
    End Sub

    ' =======================================================
    ' 4. BOTONES INTELIGENTES
    ' =======================================================
    Private Sub dgvPendientes_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPendientes.SelectionChanged
        ActualizarBotonesPorSeleccion()
    End Sub

    Private Sub ActualizarBotonesPorSeleccion()
        If dgvPendientes.SelectedRows.Count = 0 Then
            ConfigurarBotones(False, False, False, False)
            Return
        End If

        Dim idOficinaDoc As Integer = CInt(dgvPendientes.SelectedRows(0).Cells("IdOficinaActual").Value)
        Dim esMio As Boolean = (idOficinaDoc = SesionGlobal.OficinaID)
        Dim esBandejaEntrada As Boolean = (idOficinaDoc = IdBandejaEntrada)
        Dim esHijo As Boolean = If(dgvPendientes.SelectedRows(0).Cells("EsHijo").Value Is Nothing, False, CBool(dgvPendientes.SelectedRows(0).Cells("EsHijo").Value))
        ConfigurarBotones(True, esMio, esBandejaEntrada, esHijo)
    End Sub

    Private Sub ConfigurarBotones(haySeleccion As Boolean, esMio As Boolean, esBandejaEntrada As Boolean, esHijo As Boolean)
        Dim bgApagado As Color = SystemColors.Control
        Dim fgApagado As Color = SystemColors.GrayText
        Dim colorActivo As Color = AppTheme.Palette.Primary
        Dim colorTextoActivo As Color = Color.White

        Dim habilitarPase As Boolean = haySeleccion AndAlso ((esMio And esBandejaEntrada) OrElse (Not esBandejaEntrada))
        ConfigurarEstadoBoton(btnDarPase, habilitarPase, colorActivo, colorTextoActivo, bgApagado, fgApagado)

        ConfigurarEstadoBoton(btnVincular, haySeleccion And esMio, colorActivo, colorTextoActivo, bgApagado, fgApagado)
        ConfigurarEstadoBoton(btnEliminar, haySeleccion And esMio, colorActivo, colorTextoActivo, bgApagado, fgApagado)
        ConfigurarEstadoBoton(btnEditar, haySeleccion And esMio, colorActivo, colorTextoActivo, bgApagado, fgApagado)
        ConfigurarEstadoBoton(btnHistorial, haySeleccion, colorActivo, colorTextoActivo, bgApagado, fgApagado)
        ConfigurarEstadoBoton(btnDesvincular, haySeleccion And esMio And esHijo, colorActivo, colorTextoActivo, bgApagado, fgApagado)

        btnNuevoIngreso.Text = "➕ NUEVO INGRESO"
        btnNuevoIngreso.BackColor = Color.ForestGreen
        btnNuevoIngreso.ForeColor = Color.White
    End Sub

    Private Sub ConfigurarEstadoBoton(btn As Button,
                                      habilitado As Boolean,
                                      colorActivo As Color,
                                      colorTextoActivo As Color,
                                      colorApagado As Color,
                                      colorTextoApagado As Color)
        btn.Enabled = habilitado
        btn.BackColor = If(habilitado, colorActivo, colorApagado)
        btn.ForeColor = If(habilitado, colorTextoActivo, colorTextoApagado)
    End Sub

    ' =======================================================
    ' 5. ACCIONES (CRUD - CON Unit of Work)
    ' =======================================================

    Private Async Sub RecargarAlCerrarAsync(sender As Object, e As FormClosedEventArgs)
        Await CargarGrillaAsync()
        If Me.MdiParent IsNot Nothing Then
            UIUtils.AjustarFormularioAlContenedorMdi(Me)
        End If
    End Sub

    Private Async Function AbrirPaseAsync(idPadreReal As Long) As Task
        Dim fPase As New frmPase(idPadreReal)

        If Me.MdiParent Is Nothing Then
            fPase.ShowDialog(Me)
            Await CargarGrillaAsync()
            Return
        End If

        ShowFormInMdi(Me, fPase, AddressOf RecargarAlCerrarAsync)
    End Function

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvPendientes.SelectedRows.Count = 0 Then Return
        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Using uow As New UnitOfWork()
            Dim docRepo = uow.Repository(Of Mae_Documento)()
            Dim doc = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idDoc)
            If doc Is Nothing Then
                Notifier.[Error](Me, "Documento no encontrado.")
                Return
            End If
            If doc.IdOficinaActual <> SesionGlobal.OficinaID Then
                Notifier.[Error](Me, "⛔ No puedes editar documentos que no están en tu oficina.")
                Return
            End If

            Dim tieneRespuestas As Boolean = Await docRepo.AnyAsync(Function(d) d.IdDocumentoPadre = idDoc And d.IdEstadoActual <> 5)
            If tieneRespuestas Then
                Notifier.[Error](Me, "⛔ EDICIÓN BLOQUEADA." & vbCrLf & "Este documento ya tiene respuestas oficiales.")
                Return
            End If
        End Using

        Dim fEdicion As New frmMesaEntrada(idDoc)
        ShowFormInMdi(Me, fEdicion, AddressOf RecargarAlCerrarAsync)
    End Sub

    Private Async Sub btnDarPase_Click(sender As Object, e As EventArgs) Handles btnDarPase.Click
        If dgvPendientes.SelectedRows.Count = 0 Then
            Notifier.Warn(Me, "Seleccione el documento.")
            Return
        End If
        Dim idOficinaDoc As Integer = CInt(dgvPendientes.SelectedRows(0).Cells("IdOficinaActual").Value)
        If idOficinaDoc <> IdBandejaEntrada Then
            Await EjecutarRecibirDocumentoAsync()
            Return
        End If
        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Using uow As New UnitOfWork()
            Dim docRepo = uow.Repository(Of Mae_Documento)()

            Dim docSeleccionado = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idDoc)
            If docSeleccionado Is Nothing Then Return

            Dim idPadreReal As Long = If(docSeleccionado.IdDocumentoPadre.HasValue, docSeleccionado.IdDocumentoPadre.Value, docSeleccionado.IdDocumento)
            Dim docPadre = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idPadreReal)
            If docPadre Is Nothing Then Return

            Dim preguntarPorRespuesta As Boolean = True
            Dim miOficinaId = SesionGlobal.OficinaID

            Dim ultimoDocCreado = Await docRepo.GetQueryable() _
                                            .Where(Function(d) d.IdHiloConversacion = docPadre.IdHiloConversacion) _
                                            .OrderByDescending(Function(d) d.FechaCreacion) _
                                            .FirstOrDefaultAsync()

            Dim ultimoMovimientoPadre = Await uow.Context.Set(Of Tra_Movimiento)() _
                                            .Where(Function(m) m.IdDocumento = idPadreReal) _
                                            .OrderByDescending(Function(m) m.FechaMovimiento) _
                                            .FirstOrDefaultAsync()

            Dim cantidadMovimientos As Integer = Await uow.Context.Set(Of Tra_Movimiento)() _
                                            .CountAsync(Function(m) m.IdDocumento = idPadreReal)


            If ultimoDocCreado IsNot Nothing Then
                Dim movCreacionUltimoDoc = Await uow.Context.Set(Of Tra_Movimiento)() _
                                            .Where(Function(m) m.IdDocumento = ultimoDocCreado.IdDocumento) _
                                            .OrderBy(Function(m) m.FechaMovimiento) _
                                            .FirstOrDefaultAsync()

                Dim soyElAutor As Boolean = (movCreacionUltimoDoc IsNot Nothing AndAlso movCreacionUltimoDoc.IdOficinaOrigen = miOficinaId)

                If soyElAutor Then
                    If cantidadMovimientos <= 1 Then
                        preguntarPorRespuesta = False
                    ElseIf ultimoMovimientoPadre IsNot Nothing AndAlso ultimoDocCreado.FechaCreacion >= ultimoMovimientoPadre.FechaMovimiento Then
                        preguntarPorRespuesta = False
                    Else
                        preguntarPorRespuesta = True
                    End If
                Else
                    preguntarPorRespuesta = True
                End If
            End If

            If preguntarPorRespuesta Then
                Dim hilo = docPadre.IdHiloConversacion
                Dim ultimoHijo = Await docRepo.GetQueryable().Where(Function(d) d.IdHiloConversacion = hilo And d.IdDocumento <> docPadre.IdDocumento And d.IdEstadoActual <> 5).OrderByDescending(Function(d) d.FechaCreacion).FirstOrDefaultAsync()

                Dim sb As New StringBuilder()
                sb.AppendLine("Vas a iniciar el trámite de PASE (Salida).")
                sb.AppendLine("📁 EXPEDIENTE: " & docPadre.Cat_TipoDocumento.Codigo & " " & docPadre.NumeroOficial)
                sb.AppendLine("   Asunto: " & docPadre.Asunto)

                If ultimoHijo IsNot Nothing Then
                    sb.AppendLine("📎 ÚLTIMA ACTUACIÓN: " & ultimoHijo.Cat_TipoDocumento.Codigo & " " & ultimoHijo.NumeroOficial)
                End If

                sb.AppendLine()
                sb.AppendLine("¿Deseas agregar una NUEVA RESPUESTA antes de enviarlo?")

                Dim resp = MessageBox.Show(sb.ToString(), "Flujo de Salida", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                If resp = DialogResult.Cancel Then Return
                If resp = DialogResult.Yes Then
                    Dim fRespuesta As New frmMesaEntrada(idPadreReal, docPadre.IdHiloConversacion, docPadre.Asunto)
                    If Me.MdiParent Is Nothing Then
                        fRespuesta.ShowDialog(Me)
                    Else
                        AddHandler fRespuesta.FormClosed, Async Sub(sender2, args2)
                                                              Await AbrirPaseAsync(idPadreReal)
                                                          End Sub
                        ShowFormInMdi(Me, fRespuesta)
                        Return
                    End If
                End If
            End If

            Await AbrirPaseAsync(idPadreReal)
        End Using
    End Sub

    Private Sub btnVincular_Click(sender As Object, e As EventArgs) Handles btnVincular.Click
        Dim idSugerido As Long = 0
        If dgvPendientes.SelectedRows.Count > 0 Then
            idSugerido = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)
        End If

        Dim fVincular As New frmVincular(idSugerido)
        AddHandler fVincular.FormClosed, Async Sub(sender2, args2)
                                             If fVincular.DialogResult = DialogResult.OK Then
                                                 Await CargarGrillaAsync()
                                             End If
                                         End Sub
        ShowFormInMdi(Me, fVincular)
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvPendientes.SelectedRows.Count = 0 Then Return
        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Using uow As New UnitOfWork()
            Dim docRepo = uow.Repository(Of Mae_Documento)()
            Dim doc = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idDoc)
            If doc Is Nothing Then
                Notifier.[Error](Me, "Documento no encontrado.")
                Return
            End If

            If Await docRepo.AnyAsync(Function(d) d.IdDocumentoPadre = idDoc And d.IdEstadoActual <> 5) Then
                Notifier.[Error](Me, "Tiene hijos activos. No se puede eliminar.")
                Return
            End If

            If doc.Tra_Movimiento.Count <= 1 Then
                If MessageBox.Show("¿Borrar definitivamente?", "Eliminar", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                    uow.Context.Set(Of Tra_Movimiento)().RemoveRange(doc.Tra_Movimiento)
                    uow.Context.Set(Of Mae_Documento)().Remove(doc)
                    Await uow.CommitAsync()
                    AuditoriaSistema.RegistrarEvento($"Eliminación definitiva de documento {doc.NumeroOficial}.", "DOCUMENTOS")
                End If
            Else
                If MessageBox.Show("¿ANULAR expediente?", "Anular", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                    doc.IdEstadoActual = 5
                    Dim mov As New Tra_Movimiento() With {.IdDocumento = idDoc, .FechaMovimiento = DateTime.Now, .IdOficinaOrigen = SesionGlobal.OficinaID, .IdOficinaDestino = SesionGlobal.OficinaID, .IdUsuarioResponsable = SesionGlobal.UsuarioID, .ObservacionPase = "ANULADO", .IdEstadoEnEseMomento = 5}
                    doc.Tra_Movimiento.Add(mov)
                    Await uow.CommitAsync()
                    AuditoriaSistema.RegistrarEvento($"Anulación de documento {doc.NumeroOficial}.", "DOCUMENTOS")
                End If
            End If
        End Using
        Await CargarGrillaAsync()
    End Sub

    Private Sub btnNuevoIngreso_Click(sender As Object, e As EventArgs) Handles btnNuevoIngreso.Click
        Dim fNuevo As New frmMesaEntrada()
        UIUtils.ShowFormInMdi(Me, fNuevo)

        AddHandler fNuevo.FormClosed, Async Sub(s, args)
                                          Await CargarGrillaAsync()
                                          UIUtils.AjustarFormularioAlContenedorMdi(Me)
                                      End Sub
    End Sub

    Private Async Function EjecutarRecibirDocumentoAsync() As Task
        If dgvPendientes.SelectedRows.Count = 0 Then Return
        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Dim huboError As Boolean = False
        Dim mensajeError As String = ""

        Try
            Dim idPadreReal As Long
            Dim idOficinaOrigen As Integer
            Dim nombreOficinaRemota As String = ""
            Dim docPadreAsunto As String = ""
            Dim docPadreNumero As String = ""
            Dim docPadreHilo As Guid

            Using uow As New UnitOfWork()
                Dim docRepo = uow.Repository(Of Mae_Documento)()
                Dim docBase = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idDoc)
                If docBase Is Nothing Then Return

                idPadreReal = If(docBase.IdDocumentoPadre.HasValue, docBase.IdDocumentoPadre.Value, docBase.IdDocumento)
                Dim docPadre = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idPadreReal)
                If docPadre Is Nothing Then Return

                docPadreHilo = docPadre.IdHiloConversacion
                idOficinaOrigen = docPadre.IdOficinaActual
                nombreOficinaRemota = docPadre.Cat_Oficina.Nombre
                docPadreAsunto = docPadre.Asunto
                docPadreNumero = docPadre.Cat_TipoDocumento.Codigo & " " & docPadre.NumeroOficial

                If idOficinaOrigen = SesionGlobal.OficinaID Then
                    Notifier.Info(Me, "Este documento/paquete ya está en tu oficina.")
                    Return
                End If

                Dim docsA_Recibir = Await docRepo.GetQueryable(tracking:=True).Where(Function(d) d.IdHiloConversacion = docPadreHilo And d.IdOficinaActual = idOficinaOrigen).ToListAsync()
                Dim totalDocs As Integer = docsA_Recibir.Count

                If totalDocs = 0 Then
                    Notifier.Warn("El documento ya no está disponible")
                    Return
                End If

                Dim sb As New StringBuilder()
                sb.AppendLine("¿Confirma la recepción del siguiente PAQUETE?")
                sb.AppendLine("📦 EXPEDIENTE: " & docPadreNumero)
                sb.AppendLine("📌 ASUNTO: " & docPadreAsunto)
                If totalDocs > 1 Then
                    sb.AppendLine("⚠️ ATENCIÓN: Contiene " & (totalDocs - 1) & " adjunto(s). Total: " & totalDocs)
                Else
                    sb.AppendLine("📄 Contenido: Documento único.")
                End If
                sb.AppendLine("📍 ORIGEN: " & nombreOficinaRemota.ToUpper())

                If MessageBox.Show(sb.ToString(), "Recibir / Recuperar Paquete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                    Return
                End If

                Dim deseaCargarActuacion As Boolean =
                    MessageBox.Show("¿Desea cargar una ACTUACIÓN FÍSICA ahora?", "Digitalizar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes

                For Each d In docsA_Recibir
                    d.IdOficinaActual = SesionGlobal.OficinaID
                    d.IdEstadoActual = 1
                    Dim mov As New Tra_Movimiento() With {
                        .IdDocumento = d.IdDocumento,
                        .FechaMovimiento = DateTime.Now,
                        .IdOficinaOrigen = idOficinaOrigen,
                        .IdOficinaDestino = SesionGlobal.OficinaID,
                        .IdUsuarioResponsable = SesionGlobal.UsuarioID,
                        .ObservacionPase = "RECUPERADO DESDE RADAR (SINCRONIZADO)",
                        .IdEstadoEnEseMomento = 1
                    }
                    d.Tra_Movimiento.Add(mov)
                Next
                Await uow.CommitAsync()
                AuditoriaSistema.RegistrarEvento($"Recepción de paquete desde {nombreOficinaRemota}. Docs: {totalDocs}. Exp: {docPadreNumero}.", "RECEPCION")

                If deseaCargarActuacion Then
                    Dim fRespuesta As New frmMesaEntrada(idPadreReal, docPadreHilo, docPadreAsunto, idOficinaOrigen)
                    ShowFormInMdi(Me, fRespuesta, AddressOf RecargarAlCerrarAsync)
                End If
            End Using

            Await CargarGrillaAsync()

        Catch ex As Exception
            huboError = True
            mensajeError = ex.Message
        End Try

        If huboError Then
            Notifier.[Error]("Error crítico al intentar recibir: " & mensajeError)
            Await CargarGrillaAsync()
        End If
    End Function

    Private Sub btnHistorial_Click(sender As Object, e As EventArgs) Handles btnHistorial.Click
        If dgvPendientes.SelectedRows.Count = 0 Then Return
        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)
        Dim fHist As New frmHistorial(idDoc)
        ShowFormInMdi(Me, fHist)
    End Sub

    Private Async Sub chkVerDerivados_CheckedChanged(sender As Object, e As EventArgs) Handles chkVerDerivados.CheckedChanged
        Await CargarGrillaAsync()
    End Sub

    Private Async Sub btnRefrescar_Click(sender As Object, e As EventArgs) Handles btnRefrescar.Click
        txtBuscar.Clear()
        Await CargarGrillaAsync(forzarNotificacionAlertasArt120:=True)
        MostrarAlertasInteresantes()
    End Sub

    Private Sub btnRenovacionesArt120_Click(sender As Object, e As EventArgs) Handles btnRenovacionesArt120.Click
        ShowUniqueFormInMdi(Of frmRenovacionesArt120)(
            Me,
            onClosed:=Async Sub(sender2, args2)
                          Await CargarGrillaAsync()
                      End Sub)
    End Sub

    Private Async Sub btnDesvincular_Click(sender As Object, e As EventArgs) Handles btnDesvincular.Click
        If dgvPendientes.SelectedRows.Count = 0 Then
            Notifier.Warn("Seleccione el documento a desvincular (sacar de la familia).")
            Return
        End If

        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Using uow As New UnitOfWork()
            Dim docRepo = uow.Repository(Of Mae_Documento)()
            Dim doc = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idDoc)
            If doc Is Nothing Then
                Notifier.[Error](Me, "Documento no encontrado.")
                Return
            End If

            If Not doc.IdDocumentoPadre.HasValue Then
                Notifier.Warn("Este documento YA es independiente (no tiene padre)." & vbCrLf &
                               "No se puede desvincular.")
                Return
            End If

            Dim padre = Await docRepo.GetQueryable().FirstOrDefaultAsync(Function(d) d.IdDocumento = doc.IdDocumentoPadre.Value)
            Dim nombrePadre As String = If(padre IsNot Nothing, padre.NumeroOficial, "Desconocido")

            Dim mensajeConfirmacion As String =
                "¿Está seguro de DESVINCULAR (Sacar) el documento " & doc.NumeroOficial & " (ID " & doc.IdDocumento & ")" & vbCrLf &
                "del expediente " & nombrePadre & " (ID " & doc.IdDocumentoPadre.Value & ")?" & vbCrLf & vbCrLf &
                "👉 El documento se volverá INDEPENDIENTE." & vbCrLf &
                "👉 Tendrá su propio historial separado." & vbCrLf &
                "👉 Aparecerá como una carpeta nueva en la bandeja."

            If MessageBox.Show(mensajeConfirmacion,
                               "Confirmar Desvinculación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                doc.IdDocumentoPadre = Nothing
                Dim nuevoHilo As Guid = Guid.NewGuid()
                doc.IdHiloConversacion = nuevoHilo

                Dim susHijos = Await docRepo.GetQueryable(tracking:=True).Where(Function(h) h.IdDocumentoPadre = doc.IdDocumento).ToListAsync()
                For Each hijo In susHijos
                    hijo.IdHiloConversacion = nuevoHilo
                Next

                Await uow.CommitAsync()
                AuditoriaSistema.RegistrarEvento($"Documento {doc.NumeroOficial} independizado del exp {nombrePadre}.", "DESVINCULACION")
                Notifier.Success("✅ Documento independizado correctamente.")

                Await CargarGrillaAsync()
            End If
        End Using
    End Sub

    Private Sub dgvPendientes_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPendientes.CellDoubleClick
        If e.RowIndex < 0 Then Return
        Dim idDoc As Long = CLng(dgvPendientes.Rows(e.RowIndex).Cells("ID").Value)
        Dim fDetalle As New frmDetalleDocumento(idDoc)
        ShowFormInMdi(Me, fDetalle)
    End Sub

    Private Sub dgvPendientes_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvPendientes.CellFormatting
        If e.RowIndex < 0 Then Return
        If Not dgvPendientes.Columns.Contains("Semaforo") Then Return

        If dgvPendientes.Columns(e.ColumnIndex).Name = "Vencimiento" Then
            Dim semaforo As String = Convert.ToString(dgvPendientes.Rows(e.RowIndex).Cells("Semaforo").Value)
            Select Case semaforo
                Case "ROJO"
                    e.CellStyle.BackColor = Color.Salmon
                    e.CellStyle.ForeColor = Color.White
                    e.CellStyle.SelectionBackColor = Color.DarkRed
                    e.CellStyle.SelectionForeColor = Color.White
                Case "AMARILLO"
                    e.CellStyle.BackColor = Color.Gold
                    e.CellStyle.ForeColor = Color.Black
                    e.CellStyle.SelectionBackColor = Color.Goldenrod
                    e.CellStyle.SelectionForeColor = Color.Black
                Case "VERDE"
                    e.CellStyle.BackColor = Color.LightGreen
                    e.CellStyle.ForeColor = Color.DarkGreen
                    e.CellStyle.SelectionBackColor = Color.Green
                    e.CellStyle.SelectionForeColor = Color.White
            End Select
        End If
    End Sub
End Class
