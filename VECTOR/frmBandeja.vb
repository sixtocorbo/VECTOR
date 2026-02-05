Imports System.Data.Entity
Imports System.Drawing
Imports System.Text
Imports System.Reflection ' Necesario para el Doble Buffer
Imports System.Threading.Tasks

Public Class frmBandeja

    ' ❌ ELIMINADO: Private db As New SecretariaDBEntities() 

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

            ConfigurarBotones(False, False, False)
            Await CargarGrillaAsync()
        Catch ex As Exception
            Me.Text = "VECTOR - Sistema de Gestión"
            ConfigurarBotones(False, False, False)
        End Try
    End Sub

    Private Sub frmBandeja_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If _fontItalic IsNot Nothing Then
            _fontItalic.Dispose()
            _fontItalic = Nothing
        End If
    End Sub

    ' =======================================================
    ' 1. CARGA DESDE BASE DE DATOS (Unit of Work + Tu Diseño)
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
    .RefPadre = If(d.IdDocumentoPadre.HasValue, d.Mae_Documento2.Cat_TipoDocumento.Nombre & " " & d.Mae_Documento2.NumeroOficial, "")
}).ToListAsync()

                ' C. AJUSTES FINALES Y FORMATEO DE TEXTO (La Lógica que pediste)
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

        lblContador.Text = "Registros: " & dgvPendientes.RowCount
        dgvPendientes.Refresh()
    End Sub

    Private Sub DiseñarColumnas()
        If dgvPendientes.Columns.Count = 0 Then Return

        ' 1. Ocultar columnas técnicas
        dgvPendientes.Columns("Cant_Respuestas").Visible = False
        dgvPendientes.Columns("IdOficinaActual").Visible = False
        dgvPendientes.Columns("EsHijo").Visible = False
        dgvPendientes.Columns("RefPadre").Visible = False

        ' 2. Configurar Anchos y Títulos (Tu diseño restaurado)
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

            .Columns("Asunto").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End With
    End Sub

    ' =======================================================
    ' 3. SEMÁFORO DE COLORES
    ' =======================================================
    Private Sub dgvPendientes_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles dgvPendientes.RowPrePaint
        If e.RowIndex < 0 Then Return
        If Not dgvPendientes.Columns.Contains("IdOficinaActual") Then Return

        Dim fila = dgvPendientes.Rows(e.RowIndex)
        If fila.Cells("IdOficinaActual").Value Is Nothing Then Return

        Dim idOficinaDoc As Integer = CInt(fila.Cells("IdOficinaActual").Value)
        Dim esMio As Boolean = (idOficinaDoc = SesionGlobal.OficinaID)

        If esMio Then
            fila.DefaultCellStyle.ForeColor = Color.Black
            fila.DefaultCellStyle.Font = _fontNormal
        Else
            fila.DefaultCellStyle.ForeColor = Color.Gray
            fila.DefaultCellStyle.Font = _fontItalic
        End If
    End Sub

    ' =======================================================
    ' 4. BOTONES INTELIGENTES
    ' =======================================================
    Private Sub dgvPendientes_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPendientes.SelectionChanged
        ActualizarBotonesPorSeleccion()
    End Sub

    Private Sub ActualizarBotonesPorSeleccion()
        If dgvPendientes.SelectedRows.Count = 0 Then
            ConfigurarBotones(False, False, False)
            Return
        End If

        Dim idOficinaDoc As Integer = CInt(dgvPendientes.SelectedRows(0).Cells("IdOficinaActual").Value)
        Dim esMio As Boolean = (idOficinaDoc = SesionGlobal.OficinaID)
        Dim esBandejaEntrada As Boolean = (idOficinaDoc = IdBandejaEntrada)
        ConfigurarBotones(True, esMio, esBandejaEntrada)
    End Sub

    Private Sub ConfigurarBotones(haySeleccion As Boolean, esMio As Boolean, esBandejaEntrada As Boolean)
        Dim bgApagado As Color = SystemColors.Control
        Dim fgApagado As Color = SystemColors.GrayText

        ' 1. BOTONES DE GESTIÓN
        Dim habilitarPase As Boolean = haySeleccion AndAlso ((esMio And esBandejaEntrada) OrElse (Not esBandejaEntrada))
        btnDarPase.Enabled = habilitarPase
        btnDarPase.BackColor = If(habilitarPase, Color.ForestGreen, bgApagado)
        btnDarPase.ForeColor = If(habilitarPase, Color.White, fgApagado)

        btnVincular.Enabled = haySeleccion And esMio
        btnEliminar.Enabled = haySeleccion And esMio
        btnEditar.Enabled = haySeleccion And esMio
        btnHistorial.Enabled = haySeleccion

        ' 2. BOTÓN NUEVO INGRESO
        btnNuevoIngreso.Text = "➕ NUEVO INGRESO"
        btnNuevoIngreso.BackColor = Color.ForestGreen
        btnNuevoIngreso.ForeColor = Color.White
    End Sub

    ' =======================================================
    ' 5. ACCIONES (CRUD - CON Unit of Work)
    ' =======================================================

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
        fEdicion.ShowDialog()
        Await CargarGrillaAsync()
    End Sub

    Private Async Sub btnDarPase_Click(sender As Object, e As EventArgs) Handles btnDarPase.Click
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
            Dim docSeleccionado = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idDoc)
            If docSeleccionado Is Nothing Then
                Toast.Show(Me, "Documento no encontrado.", ToastType.Error)
                Return
            End If
            Dim idPadreReal As Long = If(docSeleccionado.IdDocumentoPadre.HasValue, docSeleccionado.IdDocumentoPadre.Value, docSeleccionado.IdDocumento)
            Dim docPadre = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idPadreReal)
            If docPadre Is Nothing Then
                Toast.Show(Me, "Documento padre no encontrado.", ToastType.Error)
                Return
            End If
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
                fRespuesta.ShowDialog()
            End If

            Dim fPase As New frmPase(idPadreReal)
            If fPase.ShowDialog() = DialogResult.OK Then
                ' No cargamos la grilla aquí, se carga al salir del Using
            End If
        End Using

        Await CargarGrillaAsync()
    End Sub
    Private Async Sub btnVincular_Click(sender As Object, e As EventArgs) Handles btnVincular.Click
        ' 1. VALIDACIÓN BÁSICA DE SELECCIÓN
        If dgvPendientes.SelectedRows.Count = 0 Then
            Toast.Show(Me, "Seleccione el documento a vincular.", ToastType.Warning)
            Return
        End If

        Dim idDocSeleccionado As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Using uow As New UnitOfWork()
            Dim docRepo = uow.Repository(Of Mae_Documento)()
            Dim docAMover = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idDocSeleccionado)
            If docAMover Is Nothing Then
                Toast.Show(Me, "Documento no encontrado.", ToastType.Error)
                Return
            End If

            ' =========================================================================
            ' 🛡️ NIVEL 1: BLOQUEO SI YA ES HIJO (INTEGRIDAD)
            ' =========================================================================
            If docAMover.IdDocumentoPadre.HasValue Then
                Dim idPadreActual = docAMover.IdDocumentoPadre.Value
                Dim docPadreActual = Await docRepo.GetQueryable().FirstOrDefaultAsync(Function(d) d.IdDocumento = idPadreActual)
                Dim infoPadre As String = "ID: " & idPadreActual

                If docPadreActual IsNot Nothing Then
                    infoPadre = docPadreActual.Cat_TipoDocumento.Codigo & " " & docPadreActual.NumeroOficial
                End If

                Toast.Show(Me, "⛔ ACCIÓN BLOQUEADA" & vbCrLf & vbCrLf &
                                "Este documento NO ES LIBRE. Actualmente es un anexo del expediente:" & vbCrLf &
                                "📂 " & infoPadre & vbCrLf & vbCrLf &
                                "Para moverlo, primero debe desvincularlo de su padre actual.", ToastType.Warning)
                Return
            End If

            ' =========================================================================
            ' 📥 SOLICITUD DE NUEVO DESTINO
            ' =========================================================================
            Dim input As String = Microsoft.VisualBasic.InputBox("Ingrese ID del NUEVO PADRE:", "Vincular")
            If String.IsNullOrWhiteSpace(input) OrElse Not IsNumeric(input) Then Return

            Dim idNuevoPadre As Long = CLng(input)
            If idNuevoPadre = idDocSeleccionado Then Return ' No vincularse a sí mismo

            Dim docNuevoPadre = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = idNuevoPadre)
            If docNuevoPadre Is Nothing Then
                Toast.Show(Me, "El Nuevo Padre no existe.", ToastType.Error)
                Return
            End If

            ' 🚨 CAMBIO IMPORTANTE: NO BUSCAMOS AL ABUELO TODAVÍA 🚨
            ' Primero verificamos la inversión con el ID crudo que ingresó el usuario.

            ' =========================================================================
            ' 🛡️ NIVEL 2: DETECCIÓN DE INVERSIÓN DE JERARQUÍA (ENROQUE)
            ' =========================================================================
            Dim esDescendiente As Boolean = False
            Dim tempDoc = docNuevoPadre

            ' Chequeo directo: ¿El destino es hijo directo del origen?
            If tempDoc.IdDocumentoPadre.HasValue AndAlso tempDoc.IdDocumentoPadre.Value = docAMover.IdDocumento Then
                esDescendiente = True
            Else
                ' Chequeo profundo: ¿El destino es un nieto o bisnieto?
                While tempDoc.IdDocumentoPadre.HasValue
                    If tempDoc.IdDocumentoPadre.Value = docAMover.IdDocumento Then
                        esDescendiente = True
                        Exit While
                    End If
                    tempDoc = Await docRepo.GetQueryable().FirstOrDefaultAsync(Function(d) d.IdDocumento = tempDoc.IdDocumentoPadre.Value)
                    If tempDoc Is Nothing Then
                        Exit While
                    End If
                End While
            End If

            If esDescendiente Then
                ' AHORA SÍ SALTARÁ ESTE MENSAJE
                Dim resp = MessageBox.Show("🔄 INVERSIÓN DE JERARQUÍA DETECTADA" & vbCrLf & vbCrLf &
                                           "Estás intentando que el PADRE (" & docAMover.NumeroOficial & ")" & vbCrLf &
                                           "pase a ser subordinado de su propio DESCENDIENTE (" & docNuevoPadre.NumeroOficial & ")." & vbCrLf & vbCrLf &
                                           "¿Deseas realizar este cambio de roles (Enroque)?",
                                           "Invertir Mandos", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

                If resp = DialogResult.No Then Return

                ' Ejecutamos la liberación del hijo para romper el ciclo
                ' IMPORTANTE: Liberamos al documento exacto que eligió el usuario (el nuevo jefe)
                docNuevoPadre.IdDocumentoPadre = Nothing

                ' Opcional: Si quieres que el nuevo jefe herede el hilo del viejo padre para mantener continuidad
                ' docNuevoPadre.IdHiloConversacion = docAMover.IdHiloConversacion 

                Await uow.CommitAsync()
            Else
                ' 🟢 SI NO ES UN ENROQUE, AHORA SÍ APLICAMOS LA LÓGICA DEL ABUELO
                ' Si el destino es un hijo cualquiera (no mío), apuntamos a su verdadero jefe
                If docNuevoPadre.IdDocumentoPadre.HasValue Then
                    docNuevoPadre = Await docRepo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(d) d.IdDocumento = docNuevoPadre.IdDocumentoPadre.Value)
                    If docNuevoPadre Is Nothing Then
                        Toast.Show(Me, "Padre real no encontrado.", ToastType.Error)
                        Return
                    End If
                End If
            End If

            ' Verificación final anti-bucle (por si acaso)
            If docNuevoPadre.IdDocumento = docAMover.IdDocumento Then
                Toast.Show(Me, "⛔ ERROR: Referencia circular directa.", ToastType.Error)
                Return
            End If

            ' VALIDACIÓN CRONOLÓGICA (Opcional)
            Dim fHijo = If(docAMover.FechaRecepcion, docAMover.FechaCreacion)
            Dim fPadre = If(docNuevoPadre.FechaRecepcion, docNuevoPadre.FechaCreacion)
            If fHijo < fPadre Then
                If MessageBox.Show("⚠️ El documento a vincular es más antiguo que su nuevo Padre. ¿Seguir?",
                                   "Cronología", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return
            End If

            ' =========================================================================
            ' 🚀 EJECUCIÓN: ADOPCIÓN MASIVA (Aplanar Familia)
            ' =========================================================================
            If MessageBox.Show("¿Vincular " & docAMover.NumeroOficial & " al expediente " & docNuevoPadre.NumeroOficial & "?",
                               "Confirmar", MessageBoxButtons.YesNo) = DialogResult.Yes Then

                Dim hijosHuerfanos = Await docRepo.GetQueryable(tracking:=True).Where(Function(h) h.IdDocumentoPadre.HasValue AndAlso h.IdDocumentoPadre.Value = idDocSeleccionado).ToListAsync()
                Dim cantidadHijos As Integer = hijosHuerfanos.Count

                If cantidadHijos > 0 Then
                    Dim respuesta = MessageBox.Show("⚠️ REESTRUCTURA FAMILIAR" & vbCrLf & vbCrLf &
                                                    "El documento '" & docAMover.NumeroOficial & "' tiene " & cantidadHijos & " hijos adjuntos." & vbCrLf &
                                                    "¿Desea que estos hijos pasen a depender directamente del Nuevo Jefe (" & docNuevoPadre.NumeroOficial & ")?" & vbCrLf & vbCrLf &
                                                    "👉 SÍ: Todos se vuelven hermanos (Recomendado)." & vbCrLf &
                                                    "👉 NO: Cancelar operación.",
                                                    "Decisión de Jerarquía", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                    If respuesta = DialogResult.No Then Return

                    For Each nieto In hijosHuerfanos
                        nieto.IdDocumentoPadre = docNuevoPadre.IdDocumento
                        nieto.IdHiloConversacion = docNuevoPadre.IdHiloConversacion
                    Next
                    AuditoriaSistema.RegistrarEvento($"Reubicación de {cantidadHijos} adjuntos al exp {docNuevoPadre.NumeroOficial}.", "REESTRUCTURA")
                End If

                docAMover.IdDocumentoPadre = docNuevoPadre.IdDocumento
                docAMover.IdHiloConversacion = docNuevoPadre.IdHiloConversacion

                Await uow.CommitAsync()

                AuditoriaSistema.RegistrarEvento($"Vinculación de {docAMover.NumeroOficial} a {docNuevoPadre.NumeroOficial}.", "DOCUMENTOS")
                Toast.Show(Me, "✅ Operación exitosa.", ToastType.Success)
            End If
        End Using

        Await CargarGrillaAsync()
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

    Private Async Sub btnNuevoIngreso_Click(sender As Object, e As EventArgs) Handles btnNuevoIngreso.Click
        Dim fNuevo As New frmMesaEntrada()
        fNuevo.ShowDialog()
        Await CargarGrillaAsync()
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
            End Using

            ' ✅ ÉXITO: Cargamos grilla dentro del flujo normal
            Await CargarGrillaAsync()

            ' Pregunta post-operación
            If MessageBox.Show("¿Desea cargar una ACTUACIÓN FÍSICA ahora?", "Digitalizar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim fRespuesta As New frmMesaEntrada(idPadreReal, docPadreHilo, docPadreAsunto, idOficinaOrigen)
                fRespuesta.ShowDialog()
                Await CargarGrillaAsync()
            End If

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
        fHist.ShowDialog()
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
            If MessageBox.Show("¿Está seguro de DESVINCULAR (Sacar) este documento del expediente " & nombrePadre & "?" & vbCrLf & vbCrLf &
                               "👉 El documento se volverá INDEPENDIENTE." & vbCrLf &
                               "👉 Tendrá su propio historial separado." & vbCrLf &
                               "👉 Aparecerá como una carpeta nueva en la bandeja.",
                               "Confirmar Desvinculación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                ' 4. EJECUCIÓN: ROMPER CADENAS
                doc.IdDocumentoPadre = Nothing

                ' IMPORTANTE: Le damos un nuevo ADN (Hilo) para que su historia se separe de la familia anterior
                Dim nuevoHilo As Guid = Guid.NewGuid()
                doc.IdHiloConversacion = nuevoHilo

                ' 5. GESTIÓN DE ARRASTRE (Si este hijo tenía sus propios sub-adjuntos, se los lleva con él)
                ' Aunque con el "Aplanamiento" esto es raro, es mejor prevenir y actualizar a sus descendientes
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
End Class
