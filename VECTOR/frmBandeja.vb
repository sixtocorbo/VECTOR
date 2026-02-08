Imports System.Data.Entity
Imports System.Drawing
Imports System.Text
Imports System.Reflection
Imports System.Threading.Tasks

Public Class frmBandeja

    Private Const IdBandejaEntrada As Integer = 13
    Private _fontNormal As Font
    Private _fontItalic As Font

    ' 1. LISTA EN MEMORIA
    Private _listaOriginal As New List(Of Object)

    ' 2. TIMER DE BÚSQUEDA
    Private WithEvents _timerBusqueda As New Timer()

    ' =======================================================
    ' CARGA INICIAL
    ' =======================================================
    Private Async Sub frmBandeja_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AppTheme.Aplicar(Me)
            UIUtils.SetPlaceholder(txtBuscar, "Escriba para filtrar documentos...")
            ' --- TRUCO PRO: DOBLE BUFFER ---
            Dim typeDGV As Type = dgvPendientes.GetType()
            Dim propertyInfo As PropertyInfo = typeDGV.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
            propertyInfo.SetValue(dgvPendientes, True, Nothing)

            ' --- CONFIGURACIÓN ANTI-LAG ---
            _timerBusqueda.Interval = 500

            ' Configuración Visual
            Me.WindowState = FormWindowState.Maximized
            Me.Text = "VECTOR - Bandeja de: " & SesionGlobal.NombreOficina & " (" & SesionGlobal.NombreUsuario & ")"
            _fontNormal = dgvPendientes.Font
            _fontItalic = New Font(dgvPendientes.Font, FontStyle.Italic)

            ConfigurarBotones(False, False, False, False)
            Await CargarGrillaAsync()
        Catch ex As Exception
            Me.Text = "VECTOR - Sistema de Gestión"
            ConfigurarBotones(False, False, False, False)
        End Try
    End Sub

    Private Sub frmBandeja_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If _fontItalic IsNot Nothing Then
            _fontItalic.Dispose()
            _fontItalic = Nothing
        End If
    End Sub

    Private Sub frmBandeja_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.ShowIcon = False
    End Sub

    ' =======================================================
    ' 1. CARGA DESDE BASE DE DATOS (Unit of Work)
    ' =======================================================
    Private Async Function CargarGrillaAsync() As Task
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
                    .Ubicacion = d.Cat_Oficina.Nombre,
                    .Fecha = d.FechaRecepcion,
                    .Estado = d.Cat_Estado.Nombre,
                    .IdOficinaActual = d.IdOficinaActual,
                    .Cant_Respuestas = d.Mae_Documento1.Where(Function(h) h.IdEstadoActual <> 5).Count(),
                    .EsHijo = d.IdDocumentoPadre.HasValue,
                    .IdDocumentoPadre = d.IdDocumentoPadre,
                    .RefPadre = If(d.IdDocumentoPadre.HasValue, d.Mae_Documento2.Cat_TipoDocumento.Nombre & " " & d.Mae_Documento2.NumeroOficial, "")
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
                    .IdOficinaActual = x.IdOficinaActual,
                    .Cant_Respuestas = x.Cant_Respuestas,
                    .EsHijo = x.EsHijo,
                    .IdDocumentoPadre = x.IdDocumentoPadre,
                    .RefPadre = x.RefPadre
                }).ToList()

                _listaOriginal = New List(Of Object)(listaFinal)

            End Using

            ' E. MOSTRAMOS
            AplicarFiltroRapido()

        Catch ex As Exception
            Toast.Show(Me, "Error al cargar datos: " & ex.Message, ToastType.Error)
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
        If _listaOriginal Is Nothing Then Return

        Dim textoBusqueda As String = txtBuscar.Text.ToUpper().Trim()
        Dim resultado As List(Of Object)

        If String.IsNullOrWhiteSpace(textoBusqueda) Then
            resultado = _listaOriginal
        Else
            Dim palabrasClave As String() = textoBusqueda.Split(" "c)

            resultado = _listaOriginal.Where(Function(item As Object)
                                                 ' Concatenamos usando la propiedad .Origen restaurada
                                                 Dim superString As String = (item.Tipo & " " &
                                                                              item.Referencia & " " &
                                                                              item.Origen & " " &
                                                                              item.Asunto & " " &
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

        dgvPendientes.DataSource = resultado
        DiseñarColumnas() ' Aplicamos tu diseño visual
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

        lblContador.Text = "Expedientes: " & totalExpedientes

        dgvPendientes.Refresh()
    End Sub

    Private Sub DiseñarColumnas()
        If dgvPendientes.Columns.Count = 0 Then Return

        Dim columnasVisibles As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
            "ID",
            "Tipo",
            "Referencia",
            "Fecha",
            "Estado",
            "Origen",
            "Ubicacion",
            "Asunto",
            "Observaciones"
        }

        ' 1. Ocultar columnas técnicas
        dgvPendientes.Columns("Cant_Respuestas").Visible = False
        dgvPendientes.Columns("IdOficinaActual").Visible = False
        dgvPendientes.Columns("EsHijo").Visible = False
        dgvPendientes.Columns("IdDocumentoPadre").Visible = False
        dgvPendientes.Columns("RefPadre").Visible = False

        For Each columna As DataGridViewColumn In dgvPendientes.Columns
            If Not columnasVisibles.Contains(columna.Name) AndAlso columna.Visible Then
                columna.Visible = False
            End If
        Next

        ' 2. Configurar Anchos y Títulos
        With dgvPendientes
            .Columns("ID").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("ID").HeaderText = "ID"
            .Columns("ID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            .Columns("Tipo").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Tipo").HeaderText = "Tipo"

            .Columns("Referencia").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Referencia").HeaderText = "Referencia"
            .Columns("Referencia").DefaultCellStyle.Font = New Font(dgvPendientes.Font, FontStyle.Bold)

            .Columns("Fecha").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Fecha").DefaultCellStyle.Format = "dd/MM/yyyy"

            .Columns("Estado").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

            .Columns("Origen").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Origen").HeaderText = "Origen"

            .Columns("Ubicacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Ubicacion").HeaderText = "Ubicación"

            If .Columns.Contains("Asunto") Then
                .Columns("Asunto").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                .Columns("Asunto").MinimumWidth = 220
            End If

            If .Columns.Contains("Observaciones") Then
                .Columns("Observaciones").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                .Columns("Observaciones").MinimumWidth = 220
                .Columns("Observaciones").DisplayIndex = .Columns.Count - 1
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

        ' 1. BOTONES DE GESTIÓN
        Dim habilitarPase As Boolean = haySeleccion AndAlso ((esMio And esBandejaEntrada) OrElse (Not esBandejaEntrada))
        ConfigurarEstadoBoton(btnDarPase, habilitarPase, colorActivo, colorTextoActivo, bgApagado, fgApagado)

        ConfigurarEstadoBoton(btnVincular, haySeleccion And esMio, colorActivo, colorTextoActivo, bgApagado, fgApagado)
        ConfigurarEstadoBoton(btnEliminar, haySeleccion And esMio, colorActivo, colorTextoActivo, bgApagado, fgApagado)
        ConfigurarEstadoBoton(btnEditar, haySeleccion And esMio, colorActivo, colorTextoActivo, bgApagado, fgApagado)
        ConfigurarEstadoBoton(btnHistorial, haySeleccion, colorActivo, colorTextoActivo, bgApagado, fgApagado)
        ConfigurarEstadoBoton(btnDesvincular, haySeleccion And esMio And esHijo, colorActivo, colorTextoActivo, bgApagado, fgApagado)

        ' 2. BOTÓN NUEVO INGRESO
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
            Me.WindowState = FormWindowState.Maximized
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
                Toast.Show(Me, "Documento no encontrado.", ToastType.Error)
                Return
            End If
            If doc.IdOficinaActual <> SesionGlobal.OficinaID Then
                Toast.Show(Me, "⛔ No puedes editar documentos que no están en tu oficina.", ToastType.Error)
                Return
            End If

            Dim tieneRespuestas As Boolean = Await docRepo.AnyAsync(Function(d) d.IdDocumentoPadre = idDoc And d.IdEstadoActual <> 5)
            If tieneRespuestas Then
                Toast.Show(Me, "⛔ EDICIÓN BLOQUEADA." & vbCrLf & "Este documento ya tiene respuestas oficiales.", ToastType.Error)
                Return
            End If
        End Using

        Dim fEdicion As New frmMesaEntrada(idDoc)
        ShowFormInMdi(Me, fEdicion, AddressOf RecargarAlCerrarAsync)
    End Sub

    Private Async Sub btnDarPase_Click(sender As Object, e As EventArgs) Handles btnDarPase.Click
        ' [VALIDACIONES INICIALES IGUAL QUE SIEMPRE...]
        If dgvPendientes.SelectedRows.Count = 0 Then
            Toast.Show(Me, "Seleccione el documento.", ToastType.Warning)
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

            ' [OBTENER DOCS - IGUAL QUE SIEMPRE]
            Dim docSeleccionado = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idDoc)
            If docSeleccionado Is Nothing Then Return

            Dim idPadreReal As Long = If(docSeleccionado.IdDocumentoPadre.HasValue, docSeleccionado.IdDocumentoPadre.Value, docSeleccionado.IdDocumento)
            Dim docPadre = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idPadreReal)
            If docPadre Is Nothing Then Return

            ' =========================================================================
            ' 🧠 LÓGICA 4.0: PRINCIPIO DE NOVEDAD
            ' =========================================================================
            Dim preguntarPorRespuesta As Boolean = True
            Dim miOficinaId = SesionGlobal.OficinaID

            ' 1. Obtener el ÚLTIMO documento creado en el hilo (Contenido)
            Dim ultimoDocCreado = Await docRepo.GetQueryable() _
                                            .Where(Function(d) d.IdHiloConversacion = docPadre.IdHiloConversacion) _
                                            .OrderByDescending(Function(d) d.FechaCreacion) _
                                            .FirstOrDefaultAsync()

            ' 2. Obtener el ÚLTIMO movimiento del Padre (La Carpeta)
            Dim ultimoMovimientoPadre = Await uow.Context.Set(Of Tra_Movimiento)() _
                                            .Where(Function(m) m.IdDocumento = idPadreReal) _
                                            .OrderByDescending(Function(m) m.FechaMovimiento) _
                                            .FirstOrDefaultAsync()

            ' 3. Contar movimientos para detectar "Expedientes Nuevos" (Recién nacidos)
            Dim cantidadMovimientos As Integer = Await uow.Context.Set(Of Tra_Movimiento)() _
                                            .CountAsync(Function(m) m.IdDocumento = idPadreReal)


            If ultimoDocCreado IsNot Nothing Then
                ' A. Chequeamos ORIGEN (¿Lo último lo escribí yo?)
                ' Buscamos el primer movimiento de ESE documento específico para saber su autor real
                Dim movCreacionUltimoDoc = Await uow.Context.Set(Of Tra_Movimiento)() _
                                            .Where(Function(m) m.IdDocumento = ultimoDocCreado.IdDocumento) _
                                            .OrderBy(Function(m) m.FechaMovimiento) _
                                            .FirstOrDefaultAsync()

                Dim soyElAutor As Boolean = (movCreacionUltimoDoc IsNot Nothing AndAlso movCreacionUltimoDoc.IdOficinaOrigen = miOficinaId)

                If soyElAutor Then
                    ' B. Chequeamos TEMPORALIDAD (¿Es nuevo o viejo?)

                    If cantidadMovimientos <= 1 Then
                        ' CASO 1: Es un documento NUEVO (recién creado, 1 movimiento).
                        ' Si yo lo creé y es nuevo -> No me pregunto a mí mismo.
                        preguntarPorRespuesta = False

                    ElseIf ultimoMovimientoPadre IsNot Nothing AndAlso ultimoDocCreado.FechaCreacion >= ultimoMovimientoPadre.FechaMovimiento Then
                        ' CASO 2: RESPUESTA RECIENTE.
                        ' Creé el documento DESPUÉS (o al mismo tiempo) de que la carpeta se moviera por última vez.
                        ' Significa que ya actué sobre lo que llegó.
                        preguntarPorRespuesta = False

                    Else
                        ' CASO 3: ARCHIVO / RETORNO / ACTUACIÓN VIEJA.
                        ' Yo fui el último en escribir, PERO la carpeta se movió DESPUÉS de eso (ej. vino del Archivo).
                        ' Entonces lo que escribí es "noticia vieja". Debo preguntar si quiero actuar de nuevo.
                        preguntarPorRespuesta = True
                    End If

                Else
                    ' Si el último documento NO es mío (viene de afuera) -> SIEMPRE PREGUNTAR.
                    preguntarPorRespuesta = True
                End If
            End If

            ' =========================================================================
            ' 🗣️ INTERACCIÓN
            ' =========================================================================
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

            ' =========================================================================
            ' 🚀 PASE DIRECTO
            ' =========================================================================
            Await AbrirPaseAsync(idPadreReal)
        End Using
    End Sub

    Private Sub btnVincular_Click(sender As Object, e As EventArgs) Handles btnVincular.Click
        ' Preparamos un ID sugerido si hay selección, pero no es obligatorio
        Dim idSugerido As Long = 0
        If dgvPendientes.SelectedRows.Count > 0 Then
            idSugerido = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)
        End If

        ' Abrimos el nuevo formulario de vinculación manual
        ' Le pasamos el ID seleccionado para que lo pre-complete en el campo "Hijo"
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
                Toast.Show(Me, "Documento no encontrado.", ToastType.Error)
                Return
            End If

            If Await docRepo.AnyAsync(Function(d) d.IdDocumentoPadre = idDoc And d.IdEstadoActual <> 5) Then
                Toast.Show(Me, "Tiene hijos activos. No se puede eliminar.", ToastType.Error)
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

    ' =========================================================================
    ' NUEVO INGRESO (MODIFICADO PARA MDI ASÍNCRONO)
    ' =========================================================================
    Private Sub btnNuevoIngreso_Click(sender As Object, e As EventArgs) Handles btnNuevoIngreso.Click
        Dim fNuevo As New frmMesaEntrada()

        ' 1. Asignamos el Padre MDI para mantenerlo dentro del contenedor
        fNuevo.MdiParent = Me.MdiParent
        fNuevo.WindowState = FormWindowState.Maximized

        ' 2. MANEJO DE CIERRE:
        ' Como usamos .Show() (no bloqueante), usamos el evento FormClosed
        ' para recargar la grilla solo cuando el usuario termine.
        AddHandler fNuevo.FormClosed, Async Sub(s, args)
                                          Await CargarGrillaAsync()
                                          ' Aseguramos que la bandeja recupere el foco maximizado
                                          Me.WindowState = FormWindowState.Maximized
                                      End Sub

        ' 3. Mostrar formulario de forma no modal
        fNuevo.Show()
    End Sub

    Private Async Function EjecutarRecibirDocumentoAsync() As Task
        If dgvPendientes.SelectedRows.Count = 0 Then Return
        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        ' VARIABLES PARA MANEJO DE ERROR FUERA DEL CATCH
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
                    Toast.Show(Me, "Este documento/paquete ya está en tu oficina.", ToastType.Info)
                    Return
                End If

                Dim docsA_Recibir = Await docRepo.GetQueryable(tracking:=True).Where(Function(d) d.IdHiloConversacion = docPadreHilo And d.IdOficinaActual = idOficinaOrigen).ToListAsync()
                Dim totalDocs As Integer = docsA_Recibir.Count

                If totalDocs = 0 Then
                    Toast.Show("El documento ya no está disponible", ToastType.Warning)
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

                ' Tomamos la decisión ANTES de ejecutar cambios de estado para mantener el flujo atómico.
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
                    ' IMPORTANTE: Asegúrate de que frmMesaEntrada tenga el constructor adecuado (4 parámetros)
                    Dim fRespuesta As New frmMesaEntrada(idPadreReal, docPadreHilo, docPadreAsunto, idOficinaOrigen)
                    ShowFormInMdi(Me, fRespuesta, AddressOf RecargarAlCerrarAsync)
                End If
            End Using

            ' ✅ ÉXITO: Cargamos grilla dentro del flujo normal
            Await CargarGrillaAsync()

        Catch ex As Exception
            ' ⚡ CAPTURA: No usamos Await aquí dentro para evitar el error del compilador
            huboError = True
            mensajeError = ex.Message
        End Try

        ' 🔄 RECUPERACIÓN: Ejecutamos el Await fuera del bloque Catch
        If huboError Then
            Toast.Show("Error crítico al intentar recibir: " & mensajeError, ToastType.Error)
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
        Await CargarGrillaAsync()
    End Sub

    Private Async Sub btnDesvincular_Click(sender As Object, e As EventArgs) Handles btnDesvincular.Click
        ' 1. VALIDACIÓN: ¿HAY ALGO SELECCIONADO?
        If dgvPendientes.SelectedRows.Count = 0 Then
            Toast.Show("Seleccione el documento a desvincular (sacar de la familia).", ToastType.Warning)
            Return
        End If

        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Using uow As New UnitOfWork()
            Dim docRepo = uow.Repository(Of Mae_Documento)()
            Dim doc = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idDoc)
            If doc Is Nothing Then
                Toast.Show(Me, "Documento no encontrado.", ToastType.Error)
                Return
            End If

            ' 2. VALIDACIÓN: ¿REALMENTE ES UN HIJO?
            If Not doc.IdDocumentoPadre.HasValue Then
                Toast.Show("Este documento YA es independiente (no tiene padre)." & vbCrLf &
                                "No se puede desvincular.", ToastType.Warning)
                Return
            End If

            ' Obtenemos datos del padre para el mensaje
            Dim padre = Await docRepo.GetQueryable().FirstOrDefaultAsync(Function(d) d.IdDocumento = doc.IdDocumentoPadre.Value)
            Dim nombrePadre As String = If(padre IsNot Nothing, padre.NumeroOficial, "Desconocido")

            ' 3. CONFIRMACIÓN DE SEGURIDAD
            Dim mensajeConfirmacion As String =
                "¿Está seguro de DESVINCULAR (Sacar) el documento " & doc.NumeroOficial & " (ID " & doc.IdDocumento & ")" & vbCrLf &
                "del expediente " & nombrePadre & " (ID " & doc.IdDocumentoPadre.Value & ")?" & vbCrLf & vbCrLf &
                "👉 El documento se volverá INDEPENDIENTE." & vbCrLf &
                "👉 Tendrá su propio historial separado." & vbCrLf &
                "👉 Aparecerá como una carpeta nueva en la bandeja."

            If MessageBox.Show(mensajeConfirmacion,
                               "Confirmar Desvinculación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                ' 4. EJECUCIÓN: ROMPER CADENAS
                doc.IdDocumentoPadre = Nothing

                ' IMPORTANTE: Le damos un nuevo ADN (Hilo) para que su historia se separe de la familia anterior
                Dim nuevoHilo As Guid = Guid.NewGuid()
                doc.IdHiloConversacion = nuevoHilo

                ' 5. GESTIÓN DE ARRASTRE (Si este hijo tenía sus propios sub-adjuntos, se los lleva con él)
                Dim susHijos = Await docRepo.GetQueryable(tracking:=True).Where(Function(h) h.IdDocumentoPadre = doc.IdDocumento).ToListAsync()
                For Each hijo In susHijos
                    hijo.IdHiloConversacion = nuevoHilo
                Next

                Await uow.CommitAsync()

                AuditoriaSistema.RegistrarEvento($"Documento {doc.NumeroOficial} independizado del exp {nombrePadre}.", "DESVINCULACION")
                Toast.Show("✅ Documento independizado correctamente.", ToastType.Success)

                Await CargarGrillaAsync()
            End If
        End Using
    End Sub
    ' =======================================================
    ' EVENTO: DOBLE CLIC PARA VER DETALLE
    ' =======================================================
    Private Sub dgvPendientes_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPendientes.CellDoubleClick
        ' Validamos que el clic sea en una fila válida (no en la cabecera)
        If e.RowIndex < 0 Then Return

        ' Obtenemos el ID del documento seleccionado
        Dim idDoc As Long = CLng(dgvPendientes.Rows(e.RowIndex).Cells("ID").Value)

        ' Instanciamos el formulario de detalle
        Dim fDetalle As New frmDetalleDocumento(idDoc)

        ' Opción A: Abrirlo como diálogo MODAL (bloquea la bandeja hasta que cierras el detalle)
        ' Es útil para consultas rápidas.
        fDetalle.ShowDialog(Me)

        ' Opción B: Abrirlo como ventana independiente (MDI) si quieres ver varios a la vez
        ' ShowFormInMdi(Me, fDetalle) 
    End Sub
End Class

