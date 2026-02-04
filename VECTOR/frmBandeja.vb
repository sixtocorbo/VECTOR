Imports System.Data.Entity
Imports System.Drawing
Imports System.Text
Imports System.Reflection ' Necesario para el Doble Buffer

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
    Private Sub frmBandeja_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
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
            CargarGrilla()
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
    Private Sub CargarGrilla()
        Try
            ' ✅ USAR Y TIRAR: Conexión segura y optimizada
            Using db As New SecretariaDBEntities()
                db.Configuration.LazyLoadingEnabled = False

                Dim consulta = db.Mae_Documento.AsQueryable()

                ' A. FILTROS BD
                consulta = consulta.Where(Function(d) d.IdEstadoActual <> 5)

                If Not chkVerDerivados.Checked Then
                    consulta = consulta.Where(Function(d) d.IdOficinaActual = SesionGlobal.OficinaID)
                End If

                consulta = consulta.OrderByDescending(Function(d) d.FechaCreacion)

                ' B. PROYECCIÓN DE DATOS PUROS
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

                ' C. AJUSTES FINALES Y ORDEN DE COLUMNAS (Aquí recuperamos tu diseño "Origen")
                Dim listaFinal = listaDatos.Select(Function(x) New With {
                    .ID = x.ID,
                    .Tipo = x.Tipo,
                    .Referencia = x.Referencia,
                    .Fecha = x.Fecha,
                    .Estado = x.Estado,
                    .Origen = If(String.IsNullOrEmpty(x.Remitente), "Ingreso Inicial", x.Remitente), ' ✅ Nombre "Origen" restaurado
                    .Ubicacion = x.Ubicacion,
                    .Asunto = x.Asunto,
                    .IdOficinaActual = x.IdOficinaActual,
                    .Cant_Respuestas = x.Cant_Respuestas,
                    .EsHijo = x.EsHijo,
                    .RefPadre = x.RefPadre
                }).ToList()

                _listaOriginal = New List(Of Object)(listaFinal)

            End Using ' 🔒 CIERRE DE CONEXIÓN

            ' E. MOSTRAMOS
            AplicarFiltroRapido()

        Catch ex As Exception
            MessageBox.Show("Error al cargar datos: " & ex.Message)
        End Try
    End Sub

    ' =======================================================
    ' 2. LÓGICA DE BÚSQUEDA OPTIMIZADA
    ' =======================================================
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
            .Columns("ID").Width = 50
            .Columns("ID").HeaderText = "ID"
            .Columns("ID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            .Columns("Tipo").Width = 70
            .Columns("Tipo").HeaderText = "Tipo"

            .Columns("Referencia").Width = 100
            .Columns("Referencia").HeaderText = "Referencia"
            .Columns("Referencia").DefaultCellStyle.Font = New Font(dgvPendientes.Font, FontStyle.Bold)

            .Columns("Fecha").Width = 110
            .Columns("Fecha").DefaultCellStyle.Format = "dd/MM/yyyy"

            .Columns("Estado").Width = 90

            .Columns("Origen").Width = 140
            .Columns("Origen").HeaderText = "Origen"

            .Columns("Ubicacion").Width = 140
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

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvPendientes.SelectedRows.Count = 0 Then Return
        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Using db As New SecretariaDBEntities()
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
        End Using

        Dim fEdicion As New frmMesaEntrada(idDoc)
        fEdicion.ShowDialog()
        CargarGrilla()
    End Sub

    Private Sub btnDarPase_Click(sender As Object, e As EventArgs) Handles btnDarPase.Click
        If dgvPendientes.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione el documento.", "Atención")
            Return
        End If

        Dim idOficinaDoc As Integer = CInt(dgvPendientes.SelectedRows(0).Cells("IdOficinaActual").Value)
        If idOficinaDoc <> IdBandejaEntrada Then
            EjecutarRecibirDocumento()
            Return
        End If

        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Using db As New SecretariaDBEntities()
            Dim docSeleccionado = db.Mae_Documento.Find(idDoc)
            Dim idPadreReal As Long = If(docSeleccionado.IdDocumentoPadre.HasValue, docSeleccionado.IdDocumentoPadre.Value, docSeleccionado.IdDocumento)
            Dim docPadre = db.Mae_Documento.Find(idPadreReal)
            Dim hilo = docPadre.IdHiloConversacion

            Dim ultimoHijo = db.Mae_Documento.Where(Function(d) d.IdHiloConversacion = hilo And d.IdDocumento <> docPadre.IdDocumento And d.IdEstadoActual <> 5).OrderByDescending(Function(d) d.FechaCreacion).FirstOrDefault()

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

        CargarGrilla()
    End Sub

    Private Sub btnVincular_Click(sender As Object, e As EventArgs) Handles btnVincular.Click
        If dgvPendientes.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione el HIJO a vincular.", "Atención")
            Return
        End If

        Dim idHijo As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Using db As New SecretariaDBEntities()
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
                AuditoriaSistema.RegistrarEvento($"Vinculación de documento {docHijo.NumeroOficial} como hijo de {docPadre.NumeroOficial}.", "DOCUMENTOS")
                MessageBox.Show("✅ Vinculado.", "Vector")
            End If
        End Using
        CargarGrilla()
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvPendientes.SelectedRows.Count = 0 Then Return
        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Using db As New SecretariaDBEntities()
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
                    AuditoriaSistema.RegistrarEvento($"Eliminación definitiva de documento {doc.NumeroOficial}.", "DOCUMENTOS")
                End If
            Else
                If MessageBox.Show("¿ANULAR expediente?", "Anular", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                    doc.IdEstadoActual = 5
                    Dim mov As New Tra_Movimiento() With {.IdDocumento = idDoc, .FechaMovimiento = DateTime.Now, .IdOficinaOrigen = SesionGlobal.OficinaID, .IdOficinaDestino = SesionGlobal.OficinaID, .IdUsuarioResponsable = SesionGlobal.UsuarioID, .ObservacionPase = "ANULADO", .IdEstadoEnEseMomento = 5}
                    doc.Tra_Movimiento.Add(mov)
                    db.SaveChanges()
                    AuditoriaSistema.RegistrarEvento($"Anulación de documento {doc.NumeroOficial}.", "DOCUMENTOS")
                End If
            End If
        End Using
        CargarGrilla()
    End Sub

    Private Sub btnNuevoIngreso_Click(sender As Object, e As EventArgs) Handles btnNuevoIngreso.Click
        Dim fNuevo As New frmMesaEntrada()
        fNuevo.ShowDialog()
        CargarGrilla()
    End Sub

    Private Sub EjecutarRecibirDocumento()
        If dgvPendientes.SelectedRows.Count = 0 Then Return
        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

        Try
            Dim idPadreReal As Long
            Dim idOficinaOrigen As Integer
            Dim nombreOficinaRemota As String = ""
            Dim docPadreAsunto As String = ""
            Dim docPadreNumero As String = ""
            Dim docPadreHilo As Guid

            Using db As New SecretariaDBEntities()
                Dim docBase = db.Mae_Documento.Find(idDoc)
                If docBase Is Nothing Then Return

                idPadreReal = If(docBase.IdDocumentoPadre.HasValue, docBase.IdDocumentoPadre.Value, docBase.IdDocumento)
                Dim docPadre = db.Mae_Documento.Find(idPadreReal)
                If docPadre Is Nothing Then Return

                docPadreHilo = docPadre.IdHiloConversacion
                idOficinaOrigen = docPadre.IdOficinaActual
                nombreOficinaRemota = docPadre.Cat_Oficina.Nombre
                docPadreAsunto = docPadre.Asunto
                docPadreNumero = docPadre.Cat_TipoDocumento.Codigo & " " & docPadre.NumeroOficial

                If idOficinaOrigen = SesionGlobal.OficinaID Then
                    MessageBox.Show("Este documento/paquete ya está en tu oficina.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If

                Dim docsA_Recibir = db.Mae_Documento.Where(Function(d) d.IdHiloConversacion = docPadreHilo And d.IdOficinaActual = idOficinaOrigen).ToList()
                Dim totalDocs As Integer = docsA_Recibir.Count

                If totalDocs = 0 Then
                    MessageBox.Show("El documento ya no está disponible.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

                If MessageBox.Show(sb.ToString(), "Recibir / Recuperar Paquete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
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
                    AuditoriaSistema.RegistrarEvento($"Recepción de paquete desde {nombreOficinaRemota}. Docs: {totalDocs}. Exp: {docPadreNumero}.", "RECEPCION")
                End If
            End Using

            CargarGrilla()

            If MessageBox.Show("¿Desea cargar una ACTUACIÓN FÍSICA ahora?", "Digitalizar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim fRespuesta As New frmMesaEntrada(idPadreReal, docPadreHilo, docPadreAsunto, idOficinaOrigen)
                fRespuesta.ShowDialog()
                CargarGrilla()
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

    Private Sub chkVerDerivados_CheckedChanged(sender As Object, e As EventArgs) Handles chkVerDerivados.CheckedChanged
        CargarGrilla()
    End Sub
    Private Sub btnRefrescar_Click(sender As Object, e As EventArgs) Handles btnRefrescar.Click
        txtBuscar.Clear()
        CargarGrilla()
    End Sub

End Class
'Imports System.Data.Entity
'Imports System.Drawing
'Imports System.Text
'Imports System.Reflection ' Necesario para el Doble Buffer

'Public Class frmBandeja

'    Private db As New SecretariaDBEntities()
'    Private Const IdBandejaEntrada As Integer = 13
'    Private _fontNormal As Font
'    Private _fontItalic As Font

'    ' 1. LISTA EN MEMORIA (Para búsquedas instantáneas)
'    Private _listaOriginal As New List(Of Object)

'    ' 2. TIMER DE BÚSQUEDA (Para evitar que se trabe al escribir)
'    Private WithEvents _timerBusqueda As New Timer()

'    ' =======================================================
'    ' CARGA INICIAL
'    ' =======================================================
'    Private Sub frmBandeja_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        Try
'            ' --- TRUCO PRO: DOBLE BUFFER (Elimina el parpadeo blanco) ---
'            Dim typeDGV As Type = dgvPendientes.GetType()
'            Dim propertyInfo As PropertyInfo = typeDGV.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
'            propertyInfo.SetValue(dgvPendientes, True, Nothing)

'            ' --- CONFIGURACIÓN ANTI-LAG ---
'            _timerBusqueda.Interval = 500 ' Espera medio segundo antes de buscar

'            ' Configuración Visual
'            Me.WindowState = FormWindowState.Maximized
'            Me.Text = "VECTOR - Bandeja de: " & SesionGlobal.NombreOficina & " (" & SesionGlobal.NombreUsuario & ")"
'            _fontNormal = dgvPendientes.Font
'            _fontItalic = New Font(dgvPendientes.Font, FontStyle.Italic)

'            ConfigurarBotones(False, False, False)
'            CargarGrilla() ' Trae datos de la BD por primera vez
'        Catch ex As Exception
'            Me.Text = "VECTOR - Sistema de Gestión"
'            ConfigurarBotones(False, False, False)
'        End Try
'    End Sub

'    Private Sub frmBandeja_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
'        If _fontItalic IsNot Nothing Then
'            _fontItalic.Dispose()
'            _fontItalic = Nothing
'        End If
'    End Sub

'    ' =======================================================
'    ' 1. CARGA DESDE BASE DE DATOS (Solo al iniciar o actualizar)
'    ' =======================================================
'    Private Sub CargarGrilla()
'        Try
'            Dim consulta = db.Mae_Documento.AsQueryable()

'            ' A. FILTROS BD
'            consulta = consulta.Where(Function(d) d.IdEstadoActual <> 5) ' No anulados

'            If Not chkVerDerivados.Checked Then
'                consulta = consulta.Where(Function(d) d.IdOficinaActual = SesionGlobal.OficinaID)
'            End If

'            consulta = consulta.OrderByDescending(Function(d) d.FechaCreacion)

'            ' B. PROYECCIÓN (Traemos datos limpios a memoria)
'            Dim listaDatos = consulta.Select(Function(d) New With {
'                .ID = d.IdDocumento,
'                .Tipo = d.Cat_TipoDocumento.Codigo,
'                .Referencia = d.NumeroOficial,
'                .Remitente = d.Tra_Movimiento.OrderByDescending(Function(m) m.IdMovimiento).Select(Function(m) m.Cat_Oficina.Nombre).FirstOrDefault(),
'                .Asunto = d.Asunto,
'                .Ubicacion = d.Cat_Oficina.Nombre,
'                .Fecha = d.FechaRecepcion,
'                .Estado = d.Cat_Estado.Nombre,
'                .IdOficinaActual = d.IdOficinaActual,
'                .Cant_Respuestas = db.Mae_Documento.Where(Function(h) h.IdDocumentoPadre = d.IdDocumento And h.IdEstadoActual <> 5).Count(),
'                .EsHijo = d.IdDocumentoPadre.HasValue,
'                .RefPadre = If(d.IdDocumentoPadre.HasValue, db.Mae_Documento.Where(Function(p) p.IdDocumento = d.IdDocumentoPadre).Select(Function(p) p.Cat_TipoDocumento.Codigo & " " & p.NumeroOficial).FirstOrDefault(), "")
'            }).ToList()

'            ' C. AJUSTES FINALES EN MEMORIA Y REORDENAMIENTO DE COLUMNAS
'            ' El orden aquí define el orden visual en el Grid
'            Dim listaFinal = listaDatos.Select(Function(x) New With {
'                .ID = x.ID,
'                .Tipo = x.Tipo,
'                .Referencia = x.Referencia,
'                .Fecha = x.Fecha,
'                .Estado = x.Estado,
'                .Origen = If(String.IsNullOrEmpty(x.Remitente), "Ingreso Inicial", x.Remitente),
'                .Ubicacion = x.Ubicacion,
'                .Asunto = x.Asunto,
'                .IdOficinaActual = x.IdOficinaActual, ' Oculto
'                .Cant_Respuestas = x.Cant_Respuestas, ' Oculto
'                .EsHijo = x.EsHijo, ' Oculto
'                .RefPadre = x.RefPadre ' Oculto
'            }).ToList()

'            ' D. GUARDAMOS LA FOTO
'            _listaOriginal = New List(Of Object)(listaFinal)

'            ' E. MOSTRAMOS
'            AplicarFiltroRapido()

'        Catch ex As Exception
'            MessageBox.Show("Error al cargar datos: " & ex.Message)
'        End Try
'    End Sub

'    ' =======================================================
'    ' 2. LÓGICA DE BÚSQUEDA OPTIMIZADA (Anti-Lag)
'    ' =======================================================

'    ' Evento 1: El usuario escribe (Solo reiniciamos el reloj)
'    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
'        _timerBusqueda.Stop()
'        _timerBusqueda.Start()
'    End Sub

'    ' Evento 2: El reloj termina (El usuario dejó de escribir) -> EJECUTAMOS
'    Private Sub _timerBusqueda_Tick(sender As Object, e As EventArgs) Handles _timerBusqueda.Tick
'        _timerBusqueda.Stop()
'        AplicarFiltroRapido()
'    End Sub

'    ' El Motor del Filtro
'    Private Sub AplicarFiltroRapido()
'        If _listaOriginal Is Nothing Then Return

'        Dim textoBusqueda As String = txtBuscar.Text.ToUpper().Trim()
'        Dim resultado As List(Of Object)

'        If String.IsNullOrWhiteSpace(textoBusqueda) Then
'            resultado = _listaOriginal
'        Else
'            Dim palabrasClave As String() = textoBusqueda.Split(" "c)

'            resultado = _listaOriginal.Where(Function(item As Object)
'                                                 ' Concatenamos todo para buscar (Tip: Usamos Reflection o acceso directo si fuera tipado)
'                                                 ' Como es anónimo, concatenamos las propiedades visibles
'                                                 Dim superString As String = (item.Tipo & " " &
'                                                                          item.Referencia & " " &
'                                                                          item.Origen & " " &
'                                                                          item.Asunto & " " &
'                                                                          item.RefPadre & " " &
'                                                                          item.Ubicacion).ToString().ToUpper()

'                                                 Dim cumpleTodas As Boolean = True
'                                                 For Each palabra In palabrasClave
'                                                     If Not String.IsNullOrWhiteSpace(palabra) Then
'                                                         If Not superString.Contains(palabra) Then
'                                                             cumpleTodas = False
'                                                             Exit For
'                                                         End If
'                                                     End If
'                                                 Next
'                                                 Return cumpleTodas
'                                             End Function).ToList()
'        End If

'        dgvPendientes.DataSource = resultado
'        DiseñarColumnas()
'        dgvPendientes.ClearSelection()
'        If dgvPendientes.RowCount > 0 Then
'            dgvPendientes.Rows(0).Selected = True
'        End If
'        ActualizarBotonesPorSeleccion()

'        lblContador.Text = "Registros: " & dgvPendientes.RowCount
'        dgvPendientes.Refresh()
'    End Sub

'    Private Sub DiseñarColumnas()
'        If dgvPendientes.Columns.Count = 0 Then Return

'        ' 1. Ocultar columnas técnicas
'        dgvPendientes.Columns("Cant_Respuestas").Visible = False
'        dgvPendientes.Columns("IdOficinaActual").Visible = False
'        dgvPendientes.Columns("EsHijo").Visible = False
'        dgvPendientes.Columns("RefPadre").Visible = False

'        ' 2. Configurar Anchos y Títulos (Orden definido en la proyección)
'        ' Orden: ID, Tipo, Referencia, Fecha, Estado, Origen, Ubicacion, Asunto

'        With dgvPendientes
'            .Columns("ID").Width = 50
'            .Columns("ID").HeaderText = "ID"
'            .Columns("ID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

'            .Columns("Tipo").Width = 70
'            .Columns("Tipo").HeaderText = "Tipo"

'            .Columns("Referencia").Width = 100
'            .Columns("Referencia").HeaderText = "Referencia"
'            .Columns("Referencia").DefaultCellStyle.Font = New Font(dgvPendientes.Font, FontStyle.Bold)

'            .Columns("Fecha").Width = 110
'            .Columns("Fecha").DefaultCellStyle.Format = "dd/MM/yyyy"

'            .Columns("Estado").Width = 90

'            .Columns("Origen").Width = 140
'            .Columns("Origen").HeaderText = "Origen"

'            .Columns("Ubicacion").Width = 140
'            .Columns("Ubicacion").HeaderText = "Ubicación"

'            ' El asunto ocupa el resto
'            .Columns("Asunto").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
'        End With
'    End Sub

'    ' =======================================================
'    ' 3. SEMÁFORO DE COLORES (Alertas Visuales)
'    ' =======================================================
'    Private Sub dgvPendientes_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles dgvPendientes.RowPrePaint
'        If e.RowIndex < 0 Then Return
'        If Not dgvPendientes.Columns.Contains("IdOficinaActual") Then Return

'        Dim fila = dgvPendientes.Rows(e.RowIndex)
'        If fila.Cells("IdOficinaActual").Value Is Nothing Then Return

'        Dim idOficinaDoc As Integer = CInt(fila.Cells("IdOficinaActual").Value)
'        Dim esMio As Boolean = (idOficinaDoc = SesionGlobal.OficinaID)

'        ' Diferenciación visual básica: Lo que está en mi oficina vs lo que solo estoy "viendo"
'        If esMio Then
'            fila.DefaultCellStyle.ForeColor = Color.Black
'            fila.DefaultCellStyle.Font = _fontNormal
'        Else
'            fila.DefaultCellStyle.ForeColor = Color.Gray
'            fila.DefaultCellStyle.Font = _fontItalic
'        End If
'    End Sub

'    ' =======================================================
'    ' 4. BOTONES INTELIGENTES (Visual Mejorada)
'    ' =======================================================
'    Private Sub dgvPendientes_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPendientes.SelectionChanged
'        ActualizarBotonesPorSeleccion()
'    End Sub

'    Private Sub ActualizarBotonesPorSeleccion()
'        If dgvPendientes.SelectedRows.Count = 0 Then
'            ConfigurarBotones(False, False, False)
'            Return
'        End If

'        Dim idOficinaDoc As Integer = CInt(dgvPendientes.SelectedRows(0).Cells("IdOficinaActual").Value)
'        Dim esMio As Boolean = (idOficinaDoc = SesionGlobal.OficinaID)
'        Dim esBandejaEntrada As Boolean = (idOficinaDoc = IdBandejaEntrada)
'        ConfigurarBotones(True, esMio, esBandejaEntrada)
'    End Sub

'    Private Sub ConfigurarBotones(haySeleccion As Boolean, esMio As Boolean, esBandejaEntrada As Boolean)
'        ' Colores estándar para "apagado" (Gris normal de Windows)
'        Dim bgApagado As Color = SystemColors.Control
'        Dim fgApagado As Color = SystemColors.GrayText

'        ' 1. BOTONES DE GESTIÓN (Pase, Vincular, etc.)
'        ' Estos se apagan si no hay selección o si el documento no es mío
'        Dim habilitarPase As Boolean = haySeleccion AndAlso ((esMio And esBandejaEntrada) OrElse (Not esBandejaEntrada))
'        btnDarPase.Enabled = habilitarPase
'        btnDarPase.BackColor = If(habilitarPase, Color.ForestGreen, bgApagado)
'        btnDarPase.ForeColor = If(habilitarPase, Color.White, fgApagado)

'        btnVincular.Enabled = haySeleccion And esMio
'        btnEliminar.Enabled = haySeleccion And esMio
'        btnEditar.Enabled = haySeleccion And esMio

'        ' Historial siempre habilitado si hay algo seleccionado
'        btnHistorial.Enabled = haySeleccion

'        ' 2. BOTÓN NUEVO INGRESO (solo crea ingresos)
'        btnNuevoIngreso.Text = "➕ NUEVO INGRESO"
'        btnNuevoIngreso.BackColor = Color.ForestGreen
'        btnNuevoIngreso.ForeColor = Color.White
'    End Sub

'    ' =======================================================
'    ' 5. ACCIONES (CRUD)
'    ' =======================================================

'    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
'        If dgvPendientes.SelectedRows.Count = 0 Then Return
'        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)
'        Dim doc = db.Mae_Documento.Find(idDoc)

'        If doc.IdOficinaActual <> SesionGlobal.OficinaID Then
'            MessageBox.Show("⛔ No puedes editar documentos que no están en tu oficina.", "Error")
'            Return
'        End If

'        Dim tieneRespuestas As Boolean = db.Mae_Documento.Any(Function(d) d.IdDocumentoPadre = idDoc And d.IdEstadoActual <> 5)
'        If tieneRespuestas Then
'            MessageBox.Show("⛔ EDICIÓN BLOQUEADA." & vbCrLf & "Este documento ya tiene respuestas oficiales.", "Integridad", MessageBoxButtons.OK, MessageBoxIcon.Stop)
'            Return
'        End If

'        Dim fEdicion As New frmMesaEntrada(idDoc)
'        fEdicion.ShowDialog()
'        CargarGrilla()
'    End Sub

'    Private Sub btnDarPase_Click(sender As Object, e As EventArgs) Handles btnDarPase.Click
'        If dgvPendientes.SelectedRows.Count = 0 Then
'            MessageBox.Show("Seleccione el documento.", "Atención")
'            Return
'        End If

'        Dim idOficinaDoc As Integer = CInt(dgvPendientes.SelectedRows(0).Cells("IdOficinaActual").Value)
'        If idOficinaDoc <> IdBandejaEntrada Then
'            EjecutarRecibirDocumento()
'            Return
'        End If

'        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)
'        Dim docSeleccionado = db.Mae_Documento.Find(idDoc)
'        Dim idPadreReal As Long = If(docSeleccionado.IdDocumentoPadre.HasValue, docSeleccionado.IdDocumentoPadre.Value, docSeleccionado.IdDocumento)
'        Dim docPadre = db.Mae_Documento.Find(idPadreReal)

'        Dim ultimoHijo = db.Mae_Documento.Where(Function(d) d.IdHiloConversacion = docPadre.IdHiloConversacion And d.IdDocumento <> docPadre.IdDocumento And d.IdEstadoActual <> 5).OrderByDescending(Function(d) d.FechaCreacion).FirstOrDefault()

'        Dim sb As New StringBuilder()
'        sb.AppendLine("Vas a iniciar el trámite de PASE (Salida).")
'        sb.AppendLine("📁 EXPEDIENTE: " & docPadre.Cat_TipoDocumento.Codigo & " " & docPadre.NumeroOficial)
'        sb.AppendLine("   Asunto: " & docPadre.Asunto)
'        If ultimoHijo IsNot Nothing Then
'            sb.AppendLine("📎 ÚLTIMA ACTUACIÓN: " & ultimoHijo.Cat_TipoDocumento.Codigo & " " & ultimoHijo.NumeroOficial)
'        End If
'        sb.AppendLine()
'        sb.AppendLine("¿Deseas agregar una NUEVA RESPUESTA antes de enviarlo?")

'        Dim resp = MessageBox.Show(sb.ToString(), "Flujo de Salida", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
'        If resp = DialogResult.Cancel Then Return

'        If resp = DialogResult.Yes Then
'            Dim fRespuesta As New frmMesaEntrada(idPadreReal, docPadre.IdHiloConversacion, docPadre.Asunto)
'            fRespuesta.ShowDialog()
'        End If

'        Dim fPase As New frmPase(idPadreReal)
'        If fPase.ShowDialog() = DialogResult.OK Then
'            CargarGrilla()
'        End If
'    End Sub

'    Private Sub btnVincular_Click(sender As Object, e As EventArgs) Handles btnVincular.Click
'        If dgvPendientes.SelectedRows.Count = 0 Then
'            MessageBox.Show("Seleccione el HIJO a vincular.", "Atención")
'            Return
'        End If

'        Dim idHijo As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)
'        Dim docHijo = db.Mae_Documento.Find(idHijo)

'        If docHijo.IdDocumentoPadre.HasValue Then
'            MessageBox.Show("Ya está vinculado.", "Error")
'            Return
'        End If

'        Dim input As String = Microsoft.VisualBasic.InputBox("Ingrese ID del PADRE:", "Vincular")
'        If String.IsNullOrWhiteSpace(input) OrElse Not IsNumeric(input) Then Return

'        Dim idPadre As Long = CLng(input)
'        If idPadre = idHijo Then Return

'        Dim docPadre = db.Mae_Documento.Find(idPadre)
'        If docPadre Is Nothing Then
'            MessageBox.Show("Padre no existe.", "Error")
'            Return
'        End If

'        If docPadre.IdDocumentoPadre.HasValue Then
'            docPadre = db.Mae_Documento.Find(docPadre.IdDocumentoPadre.Value)
'        End If

'        Dim fHijo = If(docHijo.FechaRecepcion, docHijo.FechaCreacion)
'        Dim fPadre = If(docPadre.FechaRecepcion, docPadre.FechaCreacion)
'        If fHijo < fPadre Then
'            If MessageBox.Show("⚠️ El Hijo es más antiguo que el Padre. ¿Seguir?", "Cronología", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return
'        End If

'        If MessageBox.Show("¿Vincular " & docHijo.NumeroOficial & " a " & docPadre.NumeroOficial & "?", "Confirmar", MessageBoxButtons.YesNo) = DialogResult.Yes Then
'            docHijo.IdDocumentoPadre = docPadre.IdDocumento
'            docHijo.IdHiloConversacion = docPadre.IdHiloConversacion
'            db.SaveChanges()
'            AuditoriaSistema.RegistrarEvento($"Vinculación de documento {docHijo.NumeroOficial} como hijo de {docPadre.NumeroOficial}.", "DOCUMENTOS")
'            MessageBox.Show("✅ Vinculado.", "Vector")
'            CargarGrilla()
'        End If
'    End Sub

'    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
'        If dgvPendientes.SelectedRows.Count = 0 Then Return
'        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)
'        Dim doc = db.Mae_Documento.Find(idDoc)

'        If db.Mae_Documento.Any(Function(d) d.IdDocumentoPadre = idDoc And d.IdEstadoActual <> 5) Then
'            MessageBox.Show("Tiene hijos activos. No se puede eliminar.", "Error")
'            Return
'        End If

'        If doc.Tra_Movimiento.Count <= 1 Then
'            If MessageBox.Show("¿Borrar definitivamente?", "Eliminar", MessageBoxButtons.YesNo) = DialogResult.Yes Then
'                db.Tra_Movimiento.RemoveRange(doc.Tra_Movimiento)
'                db.Mae_Documento.Remove(doc)
'                db.SaveChanges()
'                AuditoriaSistema.RegistrarEvento($"Eliminación definitiva de documento {doc.NumeroOficial}.", "DOCUMENTOS")
'                CargarGrilla()
'            End If
'        Else
'            If MessageBox.Show("¿ANULAR expediente?", "Anular", MessageBoxButtons.YesNo) = DialogResult.Yes Then
'                doc.IdEstadoActual = 5
'                Dim mov As New Tra_Movimiento() With {.IdDocumento = idDoc, .FechaMovimiento = DateTime.Now, .IdOficinaOrigen = SesionGlobal.OficinaID, .IdOficinaDestino = SesionGlobal.OficinaID, .IdUsuarioResponsable = SesionGlobal.UsuarioID, .ObservacionPase = "ANULADO", .IdEstadoEnEseMomento = 5}
'                doc.Tra_Movimiento.Add(mov)
'                db.SaveChanges()
'                AuditoriaSistema.RegistrarEvento($"Anulación de documento {doc.NumeroOficial}.", "DOCUMENTOS")
'                CargarGrilla()
'            End If
'        End If
'    End Sub

'    ' ACCIÓN: NUEVO
'    Private Sub btnNuevoIngreso_Click(sender As Object, e As EventArgs) Handles btnNuevoIngreso.Click
'        Dim fNuevo As New frmMesaEntrada()
'        fNuevo.ShowDialog()
'        CargarGrilla()
'    End Sub

'    ' Encapsula la lógica de recepción
'    Private Sub EjecutarRecibirDocumento()
'        If dgvPendientes.SelectedRows.Count = 0 Then Return

'        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)

'        Try
'            ' 1. OBTENER Y REFRESCAR EL DOCUMENTO SELECCIONADO
'            Dim docBase = db.Mae_Documento.Find(idDoc)
'            If docBase Is Nothing Then Return
'            db.Entry(docBase).Reload()

'            ' 2. IDENTIFICAR Y REFRESCAR AL PADRE (EXPEDIENTE PRINCIPAL)
'            Dim idPadreReal As Long = If(docBase.IdDocumentoPadre.HasValue, docBase.IdDocumentoPadre.Value, docBase.IdDocumento)
'            Dim docPadre = db.Mae_Documento.Find(idPadreReal)
'            If docPadre Is Nothing Then Return
'            db.Entry(docPadre).Reload()

'            ' 3. DEFINIR UBICACIÓN REAL Y FAMILIA (PAQUETE)
'            Dim guidFamilia = docPadre.IdHiloConversacion
'            Dim idOficinaOrigen = docPadre.IdOficinaActual
'            Dim nombreOficinaRemota As String = docPadre.Cat_Oficina.Nombre

'            ' ✅ VALIDACIÓN CLAVE: si ya está en MI oficina, no hay nada que recibir
'            If idOficinaOrigen = SesionGlobal.OficinaID Then
'                MessageBox.Show("Este documento/paquete ya está en tu oficina. No hay nada para recibir.",
'                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
'                CargarGrilla()
'                Return
'            End If

'            ' 4. BUSCAR EL PAQUETE COMPLETO EN LA UBICACIÓN REAL
'            Dim docsA_Recibir = db.Mae_Documento.
'                Where(Function(d) d.IdHiloConversacion = guidFamilia And d.IdOficinaActual = idOficinaOrigen).
'                ToList()

'            Dim totalDocs As Integer = docsA_Recibir.Count

'            If totalDocs = 0 Then
'                MessageBox.Show("El documento ya no está disponible en la oficina indicada (posiblemente ya fue recibido).",
'                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'                CargarGrilla()
'                Return
'            End If

'            ' 5. RESUMEN Y CONFIRMACIÓN
'            Dim sb As New StringBuilder()
'            sb.AppendLine("¿Confirma la recepción del siguiente PAQUETE?")
'            sb.AppendLine("📦 EXPEDIENTE: " & docPadre.Cat_TipoDocumento.Codigo & " " & docPadre.NumeroOficial)
'            sb.AppendLine("📌 ASUNTO: " & docPadre.Asunto)

'            If totalDocs > 1 Then
'                sb.AppendLine("⚠️ ATENCIÓN: Contiene " & (totalDocs - 1) & " adjunto(s). Total: " & totalDocs)
'            Else
'                sb.AppendLine("📄 Contenido: Documento único.")
'            End If

'            sb.AppendLine("📍 ORIGEN: " & nombreOficinaRemota.ToUpper())

'            ' 6. EJECUCIÓN
'            If MessageBox.Show(sb.ToString(), "Recibir / Recuperar Paquete",
'                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

'                For Each d In docsA_Recibir
'                    d.IdOficinaActual = SesionGlobal.OficinaID
'                    d.IdEstadoActual = 1

'                    Dim mov As New Tra_Movimiento() With {
'                        .IdDocumento = d.IdDocumento,
'                        .FechaMovimiento = DateTime.Now,
'                        .IdOficinaOrigen = idOficinaOrigen,
'                        .IdOficinaDestino = SesionGlobal.OficinaID,
'                        .IdUsuarioResponsable = SesionGlobal.UsuarioID,
'                        .ObservacionPase = "RECUPERADO DESDE RADAR (SINCRONIZADO)",
'                        .IdEstadoEnEseMomento = 1
'                    }
'                    d.Tra_Movimiento.Add(mov)
'                Next

'                db.SaveChanges()
'                AuditoriaSistema.RegistrarEvento($"Recepción de paquete desde {nombreOficinaRemota}. Documentos: {totalDocs}. Expediente: {docPadre.NumeroOficial}.", "RECEPCION")
'                CargarGrilla()

'                ' 7. DIGITALIZACIÓN
'                If MessageBox.Show("✅ Recibido con éxito." & vbCrLf & vbCrLf & "¿Desea cargar una ACTUACIÓN FÍSICA ahora?",
'                                   "Digitalizar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

'                    Dim fRespuesta As New frmMesaEntrada(idPadreReal, docPadre.IdHiloConversacion, docPadre.Asunto, idOficinaOrigen)
'                    fRespuesta.ShowDialog()
'                    CargarGrilla()
'                End If
'            End If

'        Catch ex As Exception
'            MessageBox.Show("Error crítico al intentar recibir: " & ex.Message, "Error de Sistema")
'            CargarGrilla()
'        End Try
'    End Sub

'    Private Sub btnHistorial_Click(sender As Object, e As EventArgs) Handles btnHistorial.Click
'        If dgvPendientes.SelectedRows.Count = 0 Then Return
'        Dim idDoc As Long = CLng(dgvPendientes.SelectedRows(0).Cells("ID").Value)
'        Dim fHist As New frmHistorial(idDoc)
'        fHist.ShowDialog()
'    End Sub

'    ' Botones auxiliares
'    Private Sub chkVerDerivados_CheckedChanged(sender As Object, e As EventArgs) Handles chkVerDerivados.CheckedChanged
'        CargarGrilla()
'    End Sub
'    Private Sub btnRefrescar_Click(sender As Object, e As EventArgs) Handles btnRefrescar.Click
'        txtBuscar.Clear()
'        CargarGrilla()
'    End Sub

'End Class