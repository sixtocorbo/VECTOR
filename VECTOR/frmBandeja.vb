Imports System.Data.Entity
Imports System.Drawing
Imports System.Text
Imports System.Reflection ' Necesario para el Doble Buffer

Public Class frmBandeja

    Private db As New SecretariaDBEntities()

    ' 1. LISTA EN MEMORIA (Para búsquedas instantáneas)
    Private _listaOriginal As New List(Of Object)

    ' 2. TIMER DE BÚSQUEDA (Para evitar que se trabe al escribir)
    Private WithEvents _timerBusqueda As New Timer()

    ' =======================================================
    ' CARGA INICIAL
    ' =======================================================
    Private Sub frmBandeja_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' --- TRUCO PRO: DOBLE BUFFER (Elimina el parpadeo blanco) ---
            Dim typeDGV As Type = dgvPendientes.GetType()
            Dim propertyInfo As PropertyInfo = typeDGV.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
            propertyInfo.SetValue(dgvPendientes, True, Nothing)

            ' --- CONFIGURACIÓN ANTI-LAG ---
            _timerBusqueda.Interval = 500 ' Espera medio segundo antes de buscar

            ' Configuración Visual
            Me.WindowState = FormWindowState.Maximized
            Me.Text = "VECTOR - Bandeja de: " & SesionGlobal.NombreOficina & " (" & SesionGlobal.NombreUsuario & ")"

            ConfigurarBotones(False, False)
            CargarGrilla() ' Trae datos de la BD por primera vez
        Catch ex As Exception
            Me.Text = "VECTOR - Sistema de Gestión"
        End Try
    End Sub

    ' =======================================================
    ' 1. CARGA DESDE BASE DE DATOS (Solo al iniciar o actualizar)
    ' =======================================================
    Private Sub CargarGrilla()
        Try
            Dim consulta = db.Mae_Documento.AsQueryable()

            ' A. FILTROS BD
            consulta = consulta.Where(Function(d) d.IdEstadoActual <> 5) ' No anulados

            If Not chkVerDerivados.Checked Then
                consulta = consulta.Where(Function(d) d.IdOficinaActual = SesionGlobal.OficinaID)
            End If

            consulta = consulta.OrderByDescending(Function(d) d.FechaCreacion)

            ' B. PROYECCIÓN (Traemos datos limpios a memoria)
            Dim listaDatos = consulta.Select(Function(d) New With {
                .ID = d.IdDocumento,
                .Tipo = d.Cat_TipoDocumento.Codigo,
                .Referencia = d.NumeroOficial,
                .Remitente = d.Tra_Movimiento.OrderByDescending(Function(m) m.IdMovimiento).Select(Function(m) m.Cat_Oficina.Nombre).FirstOrDefault(),
                .Asunto = d.Asunto,
                .Ubicacion = d.Cat_Oficina.Nombre,
                .Fecha = d.FechaRecepcion,
                .Estado = d.Cat_Estado.Nombre,
                .IdOficinaActual = d.IdOficinaActual,
                .Cant_Respuestas = db.Mae_Documento.Where(Function(h) h.IdDocumentoPadre = d.IdDocumento And h.IdEstadoActual <> 5).Count(),
                .EsHijo = d.IdDocumentoPadre.HasValue,
                .RefPadre = If(d.IdDocumentoPadre.HasValue, db.Mae_Documento.Where(Function(p) p.IdDocumento = d.IdDocumentoPadre).Select(Function(p) p.Cat_TipoDocumento.Codigo & " " & p.NumeroOficial).FirstOrDefault(), "")
            }).ToList()

            ' C. AJUSTES FINALES EN MEMORIA
            Dim listaFinal = listaDatos.Select(Function(x) New With {
                .ID = x.ID,
                .Tipo = x.Tipo,
                .Referencia = x.Referencia,
                .Remitente = If(String.IsNullOrEmpty(x.Remitente), "Ingreso Inicial", x.Remitente),
                .Asunto = x.Asunto,
                .Ubicacion = x.Ubicacion,
                .Fecha = x.Fecha,
                .Estado = x.Estado,
                .IdOficinaActual = x.IdOficinaActual,
                .Cant_Respuestas = x.Cant_Respuestas,
                .EsHijo = x.EsHijo,
                .RefPadre = x.RefPadre
            }).ToList()

            ' D. GUARDAMOS LA FOTO
            _listaOriginal = New List(Of Object)(listaFinal)

            ' E. MOSTRAMOS
            AplicarFiltroRapido()

        Catch ex As Exception
            MessageBox.Show("Error al cargar datos: " & ex.Message)
        End Try
    End Sub

    ' =======================================================
    ' 2. LÓGICA DE BÚSQUEDA OPTIMIZADA (Anti-Lag)
    ' =======================================================

    ' Evento 1: El usuario escribe (Solo reiniciamos el reloj)
    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        _timerBusqueda.Stop()
        _timerBusqueda.Start()
    End Sub

    ' Evento 2: El reloj termina (El usuario dejó de escribir) -> EJECUTAMOS
    Private Sub _timerBusqueda_Tick(sender As Object, e As EventArgs) Handles _timerBusqueda.Tick
        _timerBusqueda.Stop()
        AplicarFiltroRapido()
    End Sub

    ' El Motor del Filtro
    Private Sub AplicarFiltroRapido()
        If _listaOriginal Is Nothing Then Return

        Dim textoBusqueda As String = txtBuscar.Text.ToUpper().Trim()
        Dim resultado As List(Of Object)

        If String.IsNullOrWhiteSpace(textoBusqueda) Then
            resultado = _listaOriginal
        Else
            Dim palabrasClave As String() = textoBusqueda.Split(" "c)

            resultado = _listaOriginal.Where(Function(item As Object)
                                                 Dim superString As String = (item.Tipo & " " &
                                                                              item.Referencia & " " &
                                                                              item.Remitente & " " &
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
        DiseñarColumnas()
        dgvPendientes.ClearSelection()
        ConfigurarBotones(False, False)
        If dgvPendientes.RowCount > 0 Then
            dgvPendientes.Rows(0).Selected = True
        End If

        lblContador.Text = "Registros: " & dgvPendientes.RowCount
        dgvPendientes.Refresh()
    End Sub

    Private Sub DiseñarColumnas()
        If dgvPendientes.Columns.Count = 0 Then Return

        dgvPendientes.Columns("Cant_Respuestas").Visible = False
        dgvPendientes.Columns("IdOficinaActual").Visible = False
        dgvPendientes.Columns("EsHijo").Visible = False
        dgvPendientes.Columns("RefPadre").Visible = False

        dgvPendientes.Columns("ID").Width = 40
        dgvPendientes.Columns("Tipo").Width = 50
        dgvPendientes.Columns("Referencia").Width = 100
        dgvPendientes.Columns("Remitente").Width = 140
        dgvPendientes.Columns("Remitente").HeaderText = "Viene De"
        dgvPendientes.Columns("Ubicacion").Width = 140
        dgvPendientes.Columns("Fecha").Width = 100
        dgvPendientes.Columns("Asunto").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
    End Sub

    ' =======================================================
    ' 3. SEMÁFORO DE COLORES (Alertas Visuales)
    ' =======================================================
    'Private Sub dgvPendientes_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles dgvPendientes.RowPrePaint
    '    If e.RowIndex < 0 Then Return

    '    Dim fila = dgvPendientes.Rows(e.RowIndex)
    '    Dim colFecha As String = "Fecha"

    '    If Not dgvPendientes.Columns.Contains(colFecha) OrElse fila.Cells(colFecha).Value Is Nothing Then Return

    '    ' --- A. TEXTO DINÁMICO DE ESTADO ---
    '    Dim idOficinaDoc As Integer = CInt(fila.Cells("IdOficinaActual").Value)
    '    Dim cantidadRtas As Integer = CInt(fila.Cells("Cant_Respuestas").Value)
    '    Dim esHijo As Boolean = CBool(fila.Cells("EsHijo").Value)
    '    Dim refPadre As String = fila.Cells("RefPadre").Value.ToString()
    '    Dim ubicacionTexto As String = fila.Cells("Ubicacion").Value.ToString()
    '    Dim textoEstado As String = ""

    '    If idOficinaDoc <> SesionGlobal.OficinaID Then
    '        textoEstado = "EN " & ubicacionTexto.ToUpper()
    '        fila.DefaultCellStyle.ForeColor = Color.Gray
    '        fila.DefaultCellStyle.Font = New Font(dgvPendientes.Font, FontStyle.Italic)
    '    Else
    '        If esHijo Then
    '            textoEstado = "↳ ADJUNTO DE " & refPadre
    '            fila.DefaultCellStyle.ForeColor = Color.DarkBlue
    '        ElseIf cantidadRtas > 0 Then
    '            textoEstado = "📂 EN GESTIÓN (" & cantidadRtas & ")"
    '        Else
    '            textoEstado = "📄 INGRESADO"
    '        End If
    '        fila.DefaultCellStyle.Font = New Font(dgvPendientes.Font, FontStyle.Regular)
    '    End If

    '    If fila.Cells("Estado").Value.ToString() <> textoEstado Then
    '        fila.Cells("Estado").Value = textoEstado
    '    End If

    '    ' --- B. COLORES DEL SEMÁFORO ---
    '    Dim fechaTexto As String = fila.Cells(colFecha).Value.ToString()
    '    Dim fechaDoc As DateTime

    '    If DateTime.TryParse(fechaTexto, fechaDoc) Then
    '        Dim diasAtrasado As Integer = (DateTime.Now - fechaDoc).Days

    '        ' Solo pintamos lo que está en MI oficina (lo ajeno ya está gris)
    '        If idOficinaDoc = SesionGlobal.OficinaID Then
    '            If diasAtrasado >= 10 Then
    '                ' 🔴 CRÍTICO
    '                fila.DefaultCellStyle.BackColor = Color.Salmon
    '                fila.DefaultCellStyle.ForeColor = Color.White
    '                fila.DefaultCellStyle.SelectionBackColor = Color.DarkRed
    '                fila.DefaultCellStyle.SelectionForeColor = Color.White
    '            ElseIf diasAtrasado >= 4 Then
    '                ' 🟡 ADVERTENCIA
    '                fila.DefaultCellStyle.BackColor = Color.LightYellow
    '                fila.DefaultCellStyle.ForeColor = Color.Black
    '                fila.DefaultCellStyle.SelectionBackColor = Color.Gold
    '                fila.DefaultCellStyle.SelectionForeColor = Color.Black
    '            Else
    '                ' ⚪ NORMAL
    '                If cantidadRtas > 0 And Not esHijo Then
    '                    fila.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
    '                Else
    '                    fila.DefaultCellStyle.BackColor = Color.White
    '                End If
    '                fila.DefaultCellStyle.ForeColor = Color.Black
    '                fila.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight
    '                fila.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText
    '            End If
    '        End If
    '    End If
    'End Sub

    ' =======================================================
    ' 4. BOTONES INTELIGENTES (Visual Mejorada)
    ' =======================================================
    Private Sub dgvPendientes_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPendientes.SelectionChanged
        If dgvPendientes.SelectedRows.Count > 0 Then
            Dim idOficinaDoc As Integer = CInt(dgvPendientes.SelectedRows(0).Cells("IdOficinaActual").Value)
            Dim esMio As Boolean = (idOficinaDoc = SesionGlobal.OficinaID)
            ConfigurarBotones(True, esMio)
        Else
            ConfigurarBotones(False, False)
        End If
    End Sub

    Private Sub ConfigurarBotones(haySeleccion As Boolean, esMio As Boolean)
        ' Colores estándar para "apagado" (Gris normal de Windows)
        Dim bgApagado As Color = SystemColors.Control
        Dim fgApagado As Color = SystemColors.GrayText

        ' 1. BOTONES DE GESTIÓN (Pase, Vincular, etc.)
        ' Estos se apagan si no hay selección o si el documento no es mío
        btnDarPase.Enabled = haySeleccion And esMio
        btnDarPase.BackColor = If(haySeleccion And esMio, Color.ForestGreen, bgApagado)
        btnDarPase.ForeColor = If(haySeleccion And esMio, Color.White, fgApagado)

        btnVincular.Enabled = haySeleccion And esMio
        btnEliminar.Enabled = haySeleccion And esMio
        btnEditar.Enabled = haySeleccion And esMio

        ' Historial siempre habilitado si hay algo seleccionado
        btnHistorial.Enabled = haySeleccion

        ' 2. LÓGICA DEL BOTÓN ÚNICO DE ENTRADA (btnNuevoIngreso)
        ' Este botón nunca se deshabilita, pero cambia su "identidad"
        If haySeleccion AndAlso Not esMio Then
            ' MODO ALERTA: Hay algo de otra oficina seleccionado
            btnNuevoIngreso.Text = "📥 RECIBIR / NUEVO"
            btnNuevoIngreso.BackColor = Color.DarkCyan ' Color distinto para llamar la atención
            btnNuevoIngreso.ForeColor = Color.White
        Else
            ' MODO NORMAL: Nada seleccionado o es un documento propio
            btnNuevoIngreso.Text = "➕ NUEVO INGRESO"
            btnNuevoIngreso.BackColor = Color.ForestGreen
            btnNuevoIngreso.ForeColor = Color.White
        End If
    End Sub

    ' =======================================================
    ' 5. ACCIONES (CRUD)
    ' =======================================================

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvPendientes.SelectedRows.Count = 0 Then Return
        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)
        Dim doc = db.Mae_Documento.Find(idDoc)

        If doc.IdOficinaActual <> SesionGlobal.OficinaID Then
            MessageBox.Show("⛔ No puedes editar documentos que no están en tu oficina.", "Error")
            Return
        End If

        Dim tieneRespuestas As Boolean = db.Mae_Documento.Any(Function(d) d.IdDocumentoPadre = idDoc And d.IdEstadoActual <> 5)
        If tieneRespuestas Then
            MessageBox.Show("⛔ EDICIÓN BLOQUEADA." & vbCrLf & "Este documento ya tiene respuestas oficiales.", "Integridad", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Return
        End If

        Dim fEdicion As New frmMesaEntrada(idDoc)
        fEdicion.ShowDialog()
        CargarGrilla()
    End Sub

    Private Sub btnDarPase_Click(sender As Object, e As EventArgs) Handles btnDarPase.Click
        If dgvPendientes.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione el documento.", "Atención")
            Return
        End If

        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)
        Dim docSeleccionado = db.Mae_Documento.Find(idDoc)
        Dim idPadreReal As Long = If(docSeleccionado.IdDocumentoPadre.HasValue, docSeleccionado.IdDocumentoPadre.Value, docSeleccionado.IdDocumento)
        Dim docPadre = db.Mae_Documento.Find(idPadreReal)

        Dim ultimoHijo = db.Mae_Documento.Where(Function(d) d.IdHiloConversacion = docPadre.IdHiloConversacion And d.IdDocumento <> docPadre.IdDocumento And d.IdEstadoActual <> 5).OrderByDescending(Function(d) d.FechaCreacion).FirstOrDefault()

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
            CargarGrilla()
        End If
    End Sub

    Private Sub btnVincular_Click(sender As Object, e As EventArgs) Handles btnVincular.Click
        If dgvPendientes.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione el HIJO a vincular.", "Atención")
            Return
        End If

        Dim idHijo As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)
        Dim docHijo = db.Mae_Documento.Find(idHijo)

        If docHijo.IdDocumentoPadre.HasValue Then
            MessageBox.Show("Ya está vinculado.", "Error")
            Return
        End If

        Dim input As String = Microsoft.VisualBasic.InputBox("Ingrese ID del PADRE:", "Vincular")
        If String.IsNullOrWhiteSpace(input) OrElse Not IsNumeric(input) Then Return

        Dim idPadre As Long = CLng(input)
        If idPadre = idHijo Then Return

        Dim docPadre = db.Mae_Documento.Find(idPadre)
        If docPadre Is Nothing Then
            MessageBox.Show("Padre no existe.", "Error")
            Return
        End If

        If docPadre.IdDocumentoPadre.HasValue Then
            docPadre = db.Mae_Documento.Find(docPadre.IdDocumentoPadre.Value)
        End If

        Dim fHijo = If(docHijo.FechaRecepcion, docHijo.FechaCreacion)
        Dim fPadre = If(docPadre.FechaRecepcion, docPadre.FechaCreacion)
        If fHijo < fPadre Then
            If MessageBox.Show("⚠️ El Hijo es más antiguo que el Padre. ¿Seguir?", "Cronología", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return
        End If

        If MessageBox.Show("¿Vincular " & docHijo.NumeroOficial & " a " & docPadre.NumeroOficial & "?", "Confirmar", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            docHijo.IdDocumentoPadre = docPadre.IdDocumento
            docHijo.IdHiloConversacion = docPadre.IdHiloConversacion
            db.SaveChanges()
            MessageBox.Show("✅ Vinculado.", "Vector")
            CargarGrilla()
        End If
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvPendientes.SelectedRows.Count = 0 Then Return
        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)
        Dim doc = db.Mae_Documento.Find(idDoc)

        If db.Mae_Documento.Any(Function(d) d.IdDocumentoPadre = idDoc And d.IdEstadoActual <> 5) Then
            MessageBox.Show("Tiene hijos activos. No se puede eliminar.", "Error")
            Return
        End If

        If doc.Tra_Movimiento.Count <= 1 Then
            If MessageBox.Show("¿Borrar definitivamente?", "Eliminar", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                db.Tra_Movimiento.RemoveRange(doc.Tra_Movimiento)
                db.Mae_Documento.Remove(doc)
                db.SaveChanges()
                CargarGrilla()
            End If
        Else
            If MessageBox.Show("¿ANULAR expediente?", "Anular", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                doc.IdEstadoActual = 5
                Dim mov As New Tra_Movimiento() With {.IdDocumento = idDoc, .FechaMovimiento = DateTime.Now, .IdOficinaOrigen = SesionGlobal.OficinaID, .IdOficinaDestino = SesionGlobal.OficinaID, .IdUsuarioResponsable = SesionGlobal.UsuarioID, .ObservacionPase = "ANULADO", .IdEstadoEnEseMomento = 5}
                doc.Tra_Movimiento.Add(mov)
                db.SaveChanges()
                CargarGrilla()
            End If
        End If
    End Sub

    ' ACCIÓN: NUEVO
    Private Sub btnNuevoIngreso_Click(sender As Object, e As EventArgs) Handles btnNuevoIngreso.Click
        Dim fNuevo As New frmMesaEntrada()
        fNuevo.ShowDialog()
        CargarGrilla()
    End Sub
    ' Encapsula la apertura de un nuevo registro
    Private Sub AbrirNuevoIngreso()
        Dim fNuevo As New frmMesaEntrada()
        fNuevo.ShowDialog()
        CargarGrilla()
    End Sub

    ' Encapsula la lógica de recepción que ya tienes (con el Reload de BD)
    Private Sub EjecutarRecibirDocumento()
        If dgvPendientes.SelectedRows.Count = 0 Then Return

        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Try
            ' 1. OBTENER Y REFRESCAR EL DOCUMENTO SELECCIONADO
            ' Forzamos a Entity Framework a traer los datos más recientes del servidor
            Dim docBase = db.Mae_Documento.Find(idDoc)
            If docBase Is Nothing Then Return
            db.Entry(docBase).Reload()

            ' 2. IDENTIFICAR Y REFRESCAR AL PADRE (EXPEDIENTE PRINCIPAL)
            Dim idPadreReal As Long = If(docBase.IdDocumentoPadre.HasValue, docBase.IdDocumentoPadre.Value, docBase.IdDocumento)
            Dim docPadre = db.Mae_Documento.Find(idPadreReal)
            If docPadre Is Nothing Then Return
            db.Entry(docPadre).Reload()

            ' 3. DEFINIR LA UBICACIÓN REAL ACTUAL
            Dim guidFamilia = docPadre.IdHiloConversacion
            Dim idOficinaDondeEstaRealmente = docPadre.IdOficinaActual
            Dim nombreOficinaRemota As String = docPadre.Cat_Oficina.Nombre

            ' 4. BUSCAR TODO EL PAQUETE EN LA UBICACIÓN REAL
            ' Buscamos todos los documentos del mismo hilo que estén en esa oficina específica
            Dim docsA_Recibir = db.Mae_Documento.Where(Function(d) d.IdHiloConversacion = guidFamilia And d.IdOficinaActual = idOficinaDondeEstaRealmente).ToList()
            Dim totalDocs As Integer = docsA_Recibir.Count

            ' VALIDACIÓN DE DISPONIBILIDAD FRESCA
            If totalDocs = 0 Then
                MessageBox.Show("El documento ya no está disponible en la ubicación indicada (es posible que haya sido movido o recibido por otra oficina).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                CargarGrilla()
                Return
            End If

            ' 5. PREPARAR EL RESUMEN PARA EL USUARIO
            Dim sb As New StringBuilder()
            sb.AppendLine("¿Confirma la recepción del siguiente PAQUETE?")
            sb.AppendLine("📦 EXPEDIENTE: " & docPadre.Cat_TipoDocumento.Codigo & " " & docPadre.NumeroOficial)
            sb.AppendLine("📌 ASUNTO: " & docPadre.Asunto)

            If totalDocs > 1 Then
                sb.AppendLine("⚠️ ATENCIÓN: Contiene " & (totalDocs - 1) & " adjunto(s). Total: " & totalDocs)
            Else
                sb.AppendLine("📄 Contenido: Documento único.")
            End If
            sb.AppendLine("📍 ORIGEN: " & nombreOficinaRemota.ToUpper())

            ' 6. EJECUTAR RECEPCIÓN
            If MessageBox.Show(sb.ToString(), "Recibir / Recuperar Paquete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                For Each d In docsA_Recibir
                    ' Actualizamos ubicación y estado del documento
                    d.IdOficinaActual = SesionGlobal.OficinaID
                    d.IdEstadoActual = 1 ' Estado: Recibido / En Oficina

                    ' Registramos el movimiento en el historial
                    Dim mov As New Tra_Movimiento() With {
                    .IdDocumento = d.IdDocumento,
                    .FechaMovimiento = DateTime.Now,
                    .IdOficinaOrigen = idOficinaDondeEstaRealmente,
                    .IdOficinaDestino = SesionGlobal.OficinaID,
                    .IdUsuarioResponsable = SesionGlobal.UsuarioID,
                    .ObservacionPase = "RECUPERADO DESDE RADAR (SINCRONIZADO)",
                    .IdEstadoEnEseMomento = 1
                }
                    d.Tra_Movimiento.Add(mov)
                Next

                db.SaveChanges()
                CargarGrilla()

                ' 7. OPCIÓN DE DIGITALIZACIÓN INMEDIATA
                If MessageBox.Show("✅ Recibido con éxito." & vbCrLf & vbCrLf & "¿Desea cargar una ACTUACIÓN FÍSICA de " & nombreOficinaRemota & " ahora?", "Digitalizar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    ' Abrimos Mesa de Entrada en modo "Respuesta"
                    Dim fRespuesta As New frmMesaEntrada(idPadreReal, docPadre.IdHiloConversacion, docPadre.Asunto, idOficinaDondeEstaRealmente)
                    fRespuesta.ShowDialog()
                    CargarGrilla()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("Error crítico al intentar recibir: " & ex.Message, "Error de Sistema")
            CargarGrilla()
        End Try
    End Sub

    Private Sub btnHistorial_Click(sender As Object, e As EventArgs) Handles btnHistorial.Click
        If dgvPendientes.SelectedRows.Count = 0 Then Return
        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)
        Dim fHist As New frmHistorial(idDoc)
        fHist.ShowDialog()
    End Sub

    ' Botones auxiliares
    Private Sub chkVerDerivados_CheckedChanged(sender As Object, e As EventArgs) Handles chkVerDerivados.CheckedChanged
        CargarGrilla()
    End Sub
    Private Sub btnRefrescar_Click(sender As Object, e As EventArgs) Handles btnRefrescar.Click
        txtBuscar.Clear()
        CargarGrilla()
    End Sub

    Private Sub btnRecibir_Click(sender As Object, e As EventArgs) Handles btnRecibir.Click
        ' 1. Verificación de seguridad: ¿Hay una fila seleccionada?
        If dgvPendientes.SelectedRows.Count = 0 Then Return

        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Try
            ' 2. SINCRONIZACIÓN FRESCA CON LA BASE DE DATOS
            Dim docBase = db.Mae_Documento.Find(idDoc)
            If docBase Is Nothing Then Return
            db.Entry(docBase).Reload()

            ' 3. IDENTIFICAR AL PADRE (EXPEDIENTE PRINCIPAL)
            Dim idPadreReal As Long = If(docBase.IdDocumentoPadre.HasValue, docBase.IdDocumentoPadre.Value, docBase.IdDocumento)
            Dim docPadre = db.Mae_Documento.Find(idPadreReal)
            If docPadre Is Nothing Then Return
            db.Entry(docPadre).Reload()

            ' (Opcional pero recomendado si LazyLoading está apagado)
            'db.Entry(docPadre).Reference(Function(x) x.Cat_Oficina).Load()
            'db.Entry(docPadre).Reference(Function(x) x.Cat_TipoDocumento).Load()

            ' 4. DEFINIR UBICACIÓN REAL Y FAMILIA (PAQUETE)
            Dim guidFamilia = docPadre.IdHiloConversacion
            Dim idOficinaOrigen = docPadre.IdOficinaActual
            Dim nombreOficinaRemota As String = docPadre.Cat_Oficina.Nombre

            ' ✅ VALIDACIÓN CLAVE: si ya está en MI oficina, no hay nada que recibir
            If idOficinaOrigen = SesionGlobal.OficinaID Then
                MessageBox.Show("Este documento/paquete ya está en tu oficina. No hay nada para recibir.",
                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CargarGrilla()
                Return
            End If

            ' 5. BUSCAR EL PAQUETE COMPLETO EN LA UBICACIÓN REAL
            Dim docsA_Recibir = db.Mae_Documento.
                Where(Function(d) d.IdHiloConversacion = guidFamilia And d.IdOficinaActual = idOficinaOrigen).
                ToList()

            Dim totalDocs As Integer = docsA_Recibir.Count

            If totalDocs = 0 Then
                MessageBox.Show("El documento ya no está disponible en la oficina indicada (posiblemente ya fue recibido).",
                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                CargarGrilla()
                Return
            End If

            ' 6. RESUMEN Y CONFIRMACIÓN
            Dim sb As New StringBuilder()
            sb.AppendLine("¿Confirma la recepción del siguiente PAQUETE?")
            sb.AppendLine("📦 EXPEDIENTE: " & docPadre.Cat_TipoDocumento.Codigo & " " & docPadre.NumeroOficial)
            sb.AppendLine("📌 ASUNTO: " & docPadre.Asunto)

            If totalDocs > 1 Then
                sb.AppendLine("⚠️ ATENCIÓN: Contiene " & (totalDocs - 1) & " adjunto(s). Total: " & totalDocs)
            Else
                sb.AppendLine("📄 Contenido: Documento único.")
            End If

            sb.AppendLine("📍 ORIGEN: " & nombreOficinaRemota.ToUpper())

            ' 7. EJECUCIÓN
            If MessageBox.Show(sb.ToString(), "Recibir / Recuperar Paquete",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

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

                db.SaveChanges()
                CargarGrilla()

                ' 8. DIGITALIZACIÓN
                If MessageBox.Show("✅ Recibido con éxito." & vbCrLf & vbCrLf & "¿Desea cargar una ACTUACIÓN FÍSICA ahora?",
                                   "Digitalizar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                    Dim fRespuesta As New frmMesaEntrada(idPadreReal, docPadre.IdHiloConversacion, docPadre.Asunto, idOficinaOrigen)
                    fRespuesta.ShowDialog()
                    CargarGrilla()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("Error crítico al recibir: " & ex.Message, "Error de Sistema")
            CargarGrilla()
        End Try
    End Sub

End Class