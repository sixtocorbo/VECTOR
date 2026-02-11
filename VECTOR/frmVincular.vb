Imports System.Data.Entity
Imports System.Threading.Tasks
Imports System.Text
Imports System.Linq

Public Class frmVincular

    Private _listaPadresOriginal As New List(Of Object)
    Private _timerBusqueda As New Timer With {.Interval = 350}

    ' Constructor opcional por si quieres pre-cargar el ID del hijo desde la grilla
    Public Sub New(Optional idHijoSugerido As Long = 0)
        InitializeComponent()
        If idHijoSugerido > 0 Then
            txtIdHijo.Text = idHijoSugerido.ToString()
        End If
    End Sub

    Private Async Sub frmVincular_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        AddHandler _timerBusqueda.Tick, AddressOf _timerBusqueda_Tick

        UIUtils.SetPlaceholder(txtBuscarPadre, "Buscar por ID, tipo, referencia, origen, asunto o ubicación...")

        Await CargarContextoInicialAsync()

        If String.IsNullOrWhiteSpace(txtIdHijo.Text) Then
            Me.ActiveControl = txtIdHijo
        Else
            Me.ActiveControl = txtBuscarPadre
        End If
    End Sub

    Private Async Function CargarContextoInicialAsync() As Task
        Await CargarDetalleHijoAsync()
        Await CargarCandidatosPadreAsync()
        AplicarFiltroPadres()
    End Function

    Private Async Function CargarDetalleHijoAsync() As Task
        lblDetalleHijo.Text = "Sin documento hijo seleccionado."

        If String.IsNullOrWhiteSpace(txtIdHijo.Text) OrElse Not IsNumeric(txtIdHijo.Text) Then
            Return
        End If

        Dim idHijo As Long = CLng(txtIdHijo.Text)

        Using uow As New UnitOfWork()
            Dim docRepo = uow.Repository(Of Mae_Documento)()
            Dim docHijo = Await docRepo.GetQueryable() _
                                       .Include("Cat_TipoDocumento") _
                                       .Include("Cat_Oficina") _
                                       .FirstOrDefaultAsync(Function(d) d.IdDocumento = idHijo)

            If docHijo Is Nothing Then
                lblDetalleHijo.Text = "No se encontró el documento hijo indicado."
                Return
            End If

            Dim tipo = If(docHijo.Cat_TipoDocumento IsNot Nothing, docHijo.Cat_TipoDocumento.Nombre, "(Sin Tipo)")
            Dim referencia = If(String.IsNullOrWhiteSpace(docHijo.NumeroOficial), "(Sin Referencia)", docHijo.NumeroOficial)
            Dim asunto = If(String.IsNullOrWhiteSpace(docHijo.Asunto), "(Sin Asunto)", docHijo.Asunto)
            Dim origen = If(docHijo.Cat_Oficina IsNot Nothing, docHijo.Cat_Oficina.Nombre, "(Sin Ubicación)")

            lblDetalleHijo.Text = $"HIJO SELECCIONADO → ID: {docHijo.IdDocumento} | {tipo} {referencia}{Environment.NewLine}Asunto: {asunto} | Ubicación actual: {origen}"
        End Using
    End Function

    Private Async Function CargarCandidatosPadreAsync() As Task
        Using uow As New UnitOfWork()
            uow.Context.Configuration.LazyLoadingEnabled = False
            Dim repo = uow.Repository(Of Mae_Documento)()

            Dim listaDatos = Await repo.GetQueryable() _
                .Where(Function(d) d.IdEstadoActual <> 5) _
                .OrderByDescending(Function(d) d.FechaCreacion) _
                .Select(Function(d) New With {
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
                    .RefPadre = If(d.IdDocumentoPadre.HasValue, d.Mae_Documento2.Cat_TipoDocumento.Nombre & " " & d.Mae_Documento2.NumeroOficial, "")
                }).ToListAsync()

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
                .RefPadre = x.RefPadre
            }).Cast(Of Object).ToList()

            _listaPadresOriginal = listaFinal
        End Using
    End Function

    Private Sub txtBuscarPadre_TextChanged(sender As Object, e As EventArgs) Handles txtBuscarPadre.TextChanged
        _timerBusqueda.Stop()
        _timerBusqueda.Start()
    End Sub

    Private Sub _timerBusqueda_Tick(sender As Object, e As EventArgs)
        _timerBusqueda.Stop()
        AplicarFiltroPadres()
    End Sub

    Private Async Sub txtIdHijo_Leave(sender As Object, e As EventArgs) Handles txtIdHijo.Leave
        Await CargarDetalleHijoAsync()
    End Sub

    Private Sub AplicarFiltroPadres()
        If _listaPadresOriginal Is Nothing Then Return

        Dim textoBusqueda As String = txtBuscarPadre.Text.ToUpper().Trim()
        Dim resultado As List(Of Object)

        If String.IsNullOrWhiteSpace(textoBusqueda) Then
            resultado = _listaPadresOriginal
        Else
            Dim palabrasClave As String() = textoBusqueda.Split(" "c)

            resultado = _listaPadresOriginal.Where(Function(item As Object)
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
                                                           If Not String.IsNullOrWhiteSpace(palabra) AndAlso Not superString.Contains(palabra) Then
                                                               cumpleTodas = False
                                                               Exit For
                                                           End If
                                                       Next
                                                       Return cumpleTodas
                                                   End Function).ToList()
        End If

        dgvPadres.AutoGenerateColumns = True
        dgvPadres.Columns.Clear()
        dgvPadres.DataSource = resultado
        DiseñarColumnasPadres()

        If dgvPadres.RowCount > 0 Then
            dgvPadres.Rows(0).Selected = True
            CargarPadreDesdeFilaSeleccionada()
        Else
            txtIdPadre.Clear()
        End If
    End Sub

    Private Sub DiseñarColumnasPadres()
        If dgvPadres.Columns.Count = 0 Then Return

        Dim columnasVisibles As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
            "ID", "Tipo", "Referencia", "Fecha", "Estado", "Origen", "Ubicacion", "Asunto", "Descripcion"
        }

        For Each columna As DataGridViewColumn In dgvPadres.Columns
            columna.Visible = columnasVisibles.Contains(columna.Name)
        Next

        With dgvPadres
            .Columns("ID").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("ID").HeaderText = "ID"
            .Columns("ID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            .Columns("Tipo").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Referencia").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Referencia").DefaultCellStyle.Font = New Font(dgvPadres.Font, FontStyle.Bold)

            .Columns("Fecha").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Fecha").DefaultCellStyle.Format = "dd/MM/yyyy"

            .Columns("Estado").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Origen").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Ubicacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns("Ubicacion").HeaderText = "Ubicación"

            If .Columns.Contains("Asunto") Then
                .Columns("Asunto").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                .Columns("Asunto").MinimumWidth = 180
            End If

            If .Columns.Contains("Descripcion") Then
                .Columns("Descripcion").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                .Columns("Descripcion").MinimumWidth = 180
                .Columns("Descripcion").HeaderText = "Descripción"
            End If
        End With
    End Sub

    Private Sub dgvPadres_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPadres.SelectionChanged
        CargarPadreDesdeFilaSeleccionada()
    End Sub

    Private Sub CargarPadreDesdeFilaSeleccionada()
        If dgvPadres.SelectedRows.Count = 0 Then Return
        Dim fila = dgvPadres.SelectedRows(0)
        If fila.Cells("ID").Value Is Nothing Then Return

        txtIdPadre.Text = fila.Cells("ID").Value.ToString()
    End Sub

    Private Async Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        If String.IsNullOrWhiteSpace(txtIdHijo.Text) OrElse Not IsNumeric(txtIdHijo.Text) Then
            Notifier.Warn(Me, "Ingrese un ID de Hijo válido.")
            Return
        End If
        If String.IsNullOrWhiteSpace(txtIdPadre.Text) OrElse Not IsNumeric(txtIdPadre.Text) Then
            Notifier.Warn(Me, "Seleccione un ID de Padre válido desde la grilla.")
            Return
        End If

        Dim idHijo As Long = CLng(txtIdHijo.Text)
        Dim idNuevoPadre As Long = CLng(txtIdPadre.Text)

        If idHijo = idNuevoPadre Then
            Notifier.Warn(Me, "El ID Hijo y el ID Padre no pueden ser el mismo.")
            Return
        End If

        btnConfirmar.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            Using uow As New UnitOfWork()
                Dim docRepo = uow.Repository(Of Mae_Documento)()

                Dim docHijo = Await docRepo.GetQueryable(tracking:=True) _
                                           .Include("Cat_TipoDocumento") _
                                           .FirstOrDefaultAsync(Function(d) d.IdDocumento = idHijo)

                If docHijo Is Nothing Then
                    Notifier.[Error](Me, "El Documento HIJO no existe.")
                    Return
                End If

                Dim docPadreDestino = Await docRepo.GetQueryable(tracking:=True) _
                                                   .Include("Cat_TipoDocumento") _
                                                   .FirstOrDefaultAsync(Function(d) d.IdDocumento = idNuevoPadre)

                If docPadreDestino Is Nothing Then
                    Notifier.[Error](Me, "El Documento PADRE no existe.")
                    Return
                End If

                If docHijo.IdDocumentoPadre.HasValue Then
                    Dim idPadreActual = docHijo.IdDocumentoPadre.Value

                    If idPadreActual = idNuevoPadre Then
                        Notifier.Info(Me, "Este documento YA está vinculado a ese Padre.")
                        Return
                    End If

                    Dim docPadreActual = Await docRepo.GetQueryable().FirstOrDefaultAsync(Function(d) d.IdDocumento = idPadreActual)
                    Dim infoPadre As String = "ID: " & idPadreActual
                    If docPadreActual IsNot Nothing Then
                        infoPadre = docPadreActual.Cat_TipoDocumento.Codigo & " " & docPadreActual.NumeroOficial
                    End If

                    Notifier.Warn(Me, "⛔ ACCIÓN BLOQUEADA" & vbCrLf & vbCrLf &
                                      "El documento HIJO no es libre. Actualmente pertenece a:" & vbCrLf &
                                      "📂 " & infoPadre & vbCrLf & vbCrLf &
                                      "Para moverlo, primero debe desvincularlo.")
                    Return
                End If

                Dim esDescendiente As Boolean = False
                Dim tempDoc = docPadreDestino

                While tempDoc.IdDocumentoPadre.HasValue
                    If tempDoc.IdDocumentoPadre.Value = docHijo.IdDocumento Then
                        esDescendiente = True
                        Exit While
                    End If
                    tempDoc = Await docRepo.GetQueryable().FirstOrDefaultAsync(Function(d) d.IdDocumento = tempDoc.IdDocumentoPadre.Value)
                    If tempDoc Is Nothing Then Exit While
                End While

                If esDescendiente Then
                    Dim resp = MessageBox.Show("🔄 INVERSIÓN DE JERARQUÍA DETECTADA (ENROQUE)" & vbCrLf & vbCrLf &
                                               "Estás intentando que el PADRE (" & docHijo.NumeroOficial & ")" & vbCrLf &
                                               "se convierta en subordinado de su propio DESCENDIENTE (" & docPadreDestino.NumeroOficial & ")." & vbCrLf & vbCrLf &
                                               "¿Deseas realizar este cambio de roles?",
                                               "Invertir Mandos", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

                    If resp = DialogResult.No Then Return

                    docPadreDestino.IdDocumentoPadre = Nothing
                    Await uow.CommitAsync()
                Else
                    If docPadreDestino.IdDocumentoPadre.HasValue Then
                        Dim idAbuelo = docPadreDestino.IdDocumentoPadre.Value
                        docPadreDestino = Await docRepo.GetQueryable(tracking:=True) _
                                                     .Include("Cat_TipoDocumento") _
                                                     .FirstOrDefaultAsync(Function(d) d.IdDocumento = idAbuelo)

                        If docPadreDestino Is Nothing Then
                            Notifier.[Error](Me, "No se encontró el expediente raíz del destino.")
                            Return
                        End If
                        txtIdPadre.Text = docPadreDestino.IdDocumento.ToString()
                    End If
                End If

                If docPadreDestino.IdDocumento = docHijo.IdDocumento Then
                    Notifier.[Error](Me, "⛔ ERROR: Referencia circular directa.")
                    Return
                End If

                Dim fHijo = If(docHijo.FechaRecepcion, docHijo.FechaCreacion)
                Dim fPadre = If(docPadreDestino.FechaRecepcion, docPadreDestino.FechaCreacion)

                If fHijo < fPadre Then
                    If MessageBox.Show("⚠️ El documento HIJO es más antiguo que el PADRE." & vbCrLf & "¿Continuar de todas formas?",
                                       "Cronología", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return
                End If

                Dim etiquetaHijo As String = If(docHijo.Cat_TipoDocumento IsNot Nothing, docHijo.Cat_TipoDocumento.Codigo, "DOC") & " " & docHijo.NumeroOficial
                Dim etiquetaPadre As String = If(docPadreDestino.Cat_TipoDocumento IsNot Nothing, docPadreDestino.Cat_TipoDocumento.Codigo, "DOC") & " " & docPadreDestino.NumeroOficial

                Dim mensajeConfirmacion As New StringBuilder()
                mensajeConfirmacion.AppendLine("¿Confirma la vinculación de documentos?")
                mensajeConfirmacion.AppendLine()
                mensajeConfirmacion.AppendLine("📄 DOCUMENTO (Hijo):  " & etiquetaHijo)
                mensajeConfirmacion.AppendLine("📂 SE MOVERÁ AL EXP:  " & etiquetaPadre)
                mensajeConfirmacion.AppendLine()
                mensajeConfirmacion.AppendLine("⚠️ El documento pasará a formar parte del expediente indicado.")

                If MessageBox.Show(mensajeConfirmacion.ToString(),
                                   "Confirmar Vinculación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                    Dim hijosDelHijo = Await docRepo.GetQueryable(tracking:=True).Where(Function(h) h.IdDocumentoPadre.HasValue AndAlso h.IdDocumentoPadre.Value = idHijo).ToListAsync()
                    Dim cantidadHijos As Integer = hijosDelHijo.Count

                    If cantidadHijos > 0 Then
                        Dim respuesta = MessageBox.Show("⚠️ REESTRUCTURA FAMILIAR" & vbCrLf & vbCrLf &
                                                        "El documento '" & docHijo.NumeroOficial & "' tiene " & cantidadHijos & " adjuntos propios." & vbCrLf &
                                                        "¿Desea que estos pasen a depender directamente del Nuevo Jefe (" & docPadreDestino.NumeroOficial & ")?" & vbCrLf & vbCrLf &
                                                        "👉 SÍ: Todos se vuelven hermanos (Aplanar jerarquía)." & vbCrLf &
                                                        "👉 NO: Cancelar operación.",
                                                        "Decisión de Jerarquía", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                        If respuesta = DialogResult.No Then Return

                        For Each nieto In hijosDelHijo
                            nieto.IdDocumentoPadre = docPadreDestino.IdDocumento
                            nieto.IdHiloConversacion = docPadreDestino.IdHiloConversacion
                        Next

                        AuditoriaSistema.RegistrarEvento($"Reubicación de {cantidadHijos} adjuntos al exp {docPadreDestino.NumeroOficial}.", "REESTRUCTURA", unitOfWorkExterno:=uow)
                    End If

                    docHijo.IdDocumentoPadre = docPadreDestino.IdDocumento
                    docHijo.IdHiloConversacion = docPadreDestino.IdHiloConversacion

                    Await uow.CommitAsync()

                    AuditoriaSistema.RegistrarEvento($"Vinculación manual de {docHijo.NumeroOficial} (ID:{idHijo}) a {docPadreDestino.NumeroOficial} (ID:{docPadreDestino.IdDocumento}).", "DOCUMENTOS")
                    Notifier.Success(Me, "✅ Vinculación exitosa.")

                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                End If

            End Using

        Catch ex As Exception
            Notifier.[Error](Me, "Error: " & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            btnConfirmar.Enabled = True
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
