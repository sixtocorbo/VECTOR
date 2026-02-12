Imports System.Data.Entity
Imports System.Linq
Imports System.Reflection
Imports System.Threading.Tasks

Public Class frmRenovacionesArt120

    Private Class SalidaGridDto
        Public Property IdSalida As Integer
        Public Property IdRecluso As Integer
        Public Property Recluso As String
        Public Property LugarTrabajo As String
        Public Property Horario As String
        Public Property DetalleCustodia As String
        Public Property FechaInicio As Date
        Public Property FechaVencimiento As Date
        Public Property FechaNotificacionJuez As Nullable(Of Date)
        Public Property DiasRestantes As Integer
        Public Property Estado As String
        Public Property NotificacionJudicial As String
        Public Property Autorizacion As String
        Public Property IdDocumentoRespaldo As Nullable(Of Long)
        Public Property ReferenciaDocumentacion As String
        Public Property CantidadDocumentos As Integer
        Public Property Activo As Boolean
        Public Property Observaciones As String
    End Class

    Private Class ObservacionesSalidaDto
        Public Property CodigoAutorizacion As String
        Public Property ObservacionesUsuario As String
    End Class

    Private Class DocumentoRespaldoDto
        Public Property IdDocumento As Long
        Public Property Texto As String
        Public Property Asunto As String
        Public Property TipoDocumento As String
        Public Property NumeroOficial As String
        ' Esto obliga al ListBox a mostrar el texto limpio en lugar del nombre de la clase
        Public Overrides Function ToString() As String
            Return Texto
        End Function
        ' --------------------
    End Class

    Private _listaOriginal As New List(Of SalidaGridDto)
    Private _idSalidaEditando As Nullable(Of Integer)
    Private _idReclusoSeleccionado As Nullable(Of Integer)
    Private _cargandoDocumentos As Boolean
    Private _documentosDisponibles As New List(Of DocumentoRespaldoDto)
    Private _documentosSeleccionados As New List(Of DocumentoRespaldoDto)
    Private _guardando As Boolean
    Private _diasAnticipacionAlerta As Integer = ConfiguracionSistemaService.DiasAlertaRenovacionesPorDefecto

    Private Const CodigoAutorizacionResolucionJuez As String = "RESOLUCION_JUEZ"
    Private Const CodigoAutorizacionActa As String = "ACTA"
    Private Const PrefijoAutorizacionObservaciones As String = "#AUTORIZACION#:"

    Private Const DiasAlertaMinimo As Integer = 1
    Private Const DiasAlertaMaximo As Integer = 365

    Private Async Sub frmRenovacionesArt120_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        UIUtils.SetPlaceholder(txtBuscar, "Buscar por recluso, lugar, estado o documento...")

        Await CargarConfiguracionDiasAlertaAsync()

        Dim typeDGV As Type = dgvSalidas.GetType()
        Dim propertyInfo As PropertyInfo = typeDGV.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        propertyInfo.SetValue(dgvSalidas, True, Nothing)

        PanelEditor.Visible = False
        Await CargarSalidasAsync()
    End Sub

    Private Sub frmRenovacionesArt120_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not _guardando Then Return

        e.Cancel = True
        Notifier.Info(Me, "Hay una operación de guardado en progreso. Espere a que finalice.")
    End Sub

    Private Async Function CargarConfiguracionDiasAlertaAsync() As Task
        Try
            Dim diasConfigurados = Await ConfiguracionSistemaService.ObtenerDiasAlertaRenovacionesAsync()
            _diasAnticipacionAlerta = Math.Max(DiasAlertaMinimo, Math.Min(DiasAlertaMaximo, diasConfigurados))

            Dim mostrarSoloActivas = Await ConfiguracionSistemaService.ObtenerMostrarSoloActivasPorDefectoRenovacionesAsync()
            chkSoloActivas.Checked = mostrarSoloActivas
        Catch ex As Exception
            _diasAnticipacionAlerta = Math.Max(DiasAlertaMinimo, Math.Min(DiasAlertaMaximo, ConfiguracionSistemaService.DiasAlertaRenovacionesPorDefecto))
            chkSoloActivas.Checked = ConfiguracionSistemaService.MostrarSoloActivasPorDefectoRenovacionesPorDefecto
            Notifier.Warn(Me, "No se pudo cargar la configuración de alertas. Se usarán valores por defecto.")
        End Try
    End Function

    Private Async Function CargarSalidasAsync() As Task
        Try
            Using uow As New UnitOfWork()
                Dim repo = uow.Repository(Of Tra_SalidasLaborales)()
                Dim hoy = DateTime.Today

                Dim consulta = repo.GetQueryable() _
                    .Include("Mae_Reclusos")

                ' Lógica Switch: O son Activas O son Inactivas (nunca mezcladas)
                If chkSoloActivas.Checked Then
                    ' Opción A: Mostrar SOLO las ACTIVAS
                    consulta = consulta.Where(Function(s) s.Activo.HasValue AndAlso s.Activo.Value = True)
                Else
                    ' Opción B: Mostrar SOLO las INACTIVAS (Falso o Nulo)
                    consulta = consulta.Where(Function(s) Not s.Activo.HasValue OrElse s.Activo.Value = False)
                End If

                Dim datos = Await consulta _
                    .OrderBy(Function(s) s.FechaVencimiento) _
                    .Select(Function(s) New With {
                        .IdSalida = s.IdSalida,
                        .IdRecluso = s.IdRecluso,
                        .Recluso = s.Mae_Reclusos.NombreCompleto,
                        .LugarTrabajo = s.LugarTrabajo,
                        .Horario = s.Horario,
                        .DetalleCustodia = s.DetalleCustodia,
                        .FechaInicio = s.FechaInicio,
                        .FechaVencimiento = s.FechaVencimiento,
                        .DiasRestantes = DbFunctions.DiffDays(hoy, s.FechaVencimiento),
                        .IdDocumentoRespaldo = s.IdDocumentoRespaldo,
                        .FechaNotificacionJuez = s.FechaNotificacionJuez,
                        .Activo = s.Activo,
                        .Observaciones = s.Observaciones
                    }).ToListAsync()

                Dim idsSalida = datos.Select(Function(x) x.IdSalida).ToList()
                Dim documentosPorSalida = Await ObtenerDocumentosPorSalidaAsync(uow, idsSalida)

                _listaOriginal = datos.Select(Function(x)
                                                  Dim parse = ParsearObservacionesConAutorizacion(x.Observaciones)
                                                  Dim dias = If(x.DiasRestantes, 0)
                                                  Dim estaActiva = x.Activo.HasValue AndAlso x.Activo.Value
                                                  Dim idsDocs = If(documentosPorSalida.ContainsKey(x.IdSalida), documentosPorSalida(x.IdSalida), New List(Of Long)())
                                                  Dim cantidadDocs = idsDocs.Count
                                                  Dim referencia = FormatearReferenciaDocumentacion(cantidadDocs)
                                                  Dim estado = CalcularEstado(estaActiva, dias)

                                                  Return New SalidaGridDto With {
                                                    .IdSalida = x.IdSalida,
                                                    .IdRecluso = x.IdRecluso,
                                                    .Recluso = If(x.Recluso, ""),
                                                    .LugarTrabajo = If(x.LugarTrabajo, ""),
                                                    .Horario = If(x.Horario, ""),
                                                    .DetalleCustodia = If(x.DetalleCustodia, ""),
                                                    .FechaInicio = x.FechaInicio,
                                                    .FechaVencimiento = x.FechaVencimiento,
                                                    .FechaNotificacionJuez = x.FechaNotificacionJuez,
                                                    .DiasRestantes = dias,
                                                    .Estado = estado,
                                                    .NotificacionJudicial = ObtenerEstadoNotificacionJuez(x.FechaNotificacionJuez, x.FechaInicio),
                                                    .Autorizacion = ObtenerDescripcionAutorizacion(parse.CodigoAutorizacion),
                                                    .IdDocumentoRespaldo = x.IdDocumentoRespaldo,
                                                    .ReferenciaDocumentacion = referencia,
                                                    .CantidadDocumentos = cantidadDocs,
                                                    .Activo = estaActiva,
                                                    .Observaciones = parse.ObservacionesUsuario
                                                 }
                                              End Function).ToList()
            End Using

            AplicarFiltro()
        Catch ex As Exception
            Notifier.[Error](Me, "Error al cargar salidas laborales: " & ex.Message)
        End Try
    End Function

    Private Sub AplicarFiltro()
        RecalcularEstadosDesdeConfiguracion()

        Dim texto = txtBuscar.Text.Trim().ToUpper()
        Dim lista = _listaOriginal.AsEnumerable()

        If Not String.IsNullOrWhiteSpace(texto) Then
            Dim palabras = texto.Split(" "c)
            lista = lista.Where(Function(s)
                                    Dim baseTexto = $"{s.Recluso} {s.LugarTrabajo} {s.Horario} {s.DetalleCustodia} {s.Estado} {s.NotificacionJudicial} {s.Autorizacion} {s.Observaciones} {s.ReferenciaDocumentacion} {If(s.IdDocumentoRespaldo.HasValue, s.IdDocumentoRespaldo.Value.ToString(), "")}".ToUpper()
                                    Return palabras.All(Function(p) String.IsNullOrWhiteSpace(p) OrElse baseTexto.Contains(p))
                                End Function)
        End If

        Dim final = lista.ToList()
        dgvSalidas.DataSource = Nothing
        dgvSalidas.AutoGenerateColumns = True
        dgvSalidas.DataSource = final
        DisenarGrilla()
        Resumir(final)
    End Sub

    Private Function ObtenerEstadoNotificacionJuez(fechaNotificacion As Nullable(Of Date), fechaInicio As Date) As String
        If fechaNotificacion.HasValue Then
            Return $"Notificado {fechaNotificacion.Value:dd/MM/yyyy}"
        End If

        Dim horas = DateDiff(DateInterval.Hour, fechaInicio, DateTime.Now)
        If horas > 48 Then
            Return "Pendiente >48h"
        End If

        Return "Pendiente"
    End Function

    Private Function FormatearReferenciaDocumentacion(cantidadDocs As Integer) As String
        If cantidadDocs <= 0 Then
            Return "Sin documentación"
        End If

        Dim sufijo = If(cantidadDocs = 1, "registro", "registros")
        Return $"Documentación({cantidadDocs} {sufijo})"
    End Function

    Private Function CalcularEstado(estaActiva As Boolean, dias As Integer) As String
        If Not estaActiva Then Return "INACTIVA"
        If dias < 0 Then Return "VENCIDA"
        If dias <= ObtenerDiasAnticipacionAlerta() Then Return "ALERTA"
        Return "OK"
    End Function

    Private Function ObtenerDiasAnticipacionAlerta() As Integer
        Return _diasAnticipacionAlerta
    End Function

    Private Sub RecalcularEstadosDesdeConfiguracion()
        For Each salida In _listaOriginal
            salida.Estado = CalcularEstado(salida.Activo, salida.DiasRestantes)
        Next
    End Sub

    Private Sub DisenarGrilla()
        If dgvSalidas.Columns.Count = 0 Then Return

        dgvSalidas.Columns("IdRecluso").Visible = False
        dgvSalidas.Columns("Observaciones").Visible = False
        dgvSalidas.Columns("Activo").Visible = False
        dgvSalidas.Columns("IdDocumentoRespaldo").Visible = False
        dgvSalidas.Columns("CantidadDocumentos").Visible = False

        dgvSalidas.Columns("IdSalida").HeaderText = "ID"
        dgvSalidas.Columns("IdSalida").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        dgvSalidas.Columns("Recluso").HeaderText = "Recluso"
        dgvSalidas.Columns("Recluso").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvSalidas.Columns("Recluso").FillWeight = 140

        dgvSalidas.Columns("LugarTrabajo").HeaderText = "Lugar trabajo"
        dgvSalidas.Columns("LugarTrabajo").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvSalidas.Columns("LugarTrabajo").FillWeight = 130

        dgvSalidas.Columns("Horario").HeaderText = "Horario"
        dgvSalidas.Columns("Horario").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvSalidas.Columns("Horario").FillWeight = 110

        dgvSalidas.Columns("DetalleCustodia").HeaderText = "Custodia"
        dgvSalidas.Columns("DetalleCustodia").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvSalidas.Columns("DetalleCustodia").FillWeight = 100

        dgvSalidas.Columns("FechaInicio").HeaderText = "Inicio"
        dgvSalidas.Columns("FechaInicio").DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvSalidas.Columns("FechaInicio").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        dgvSalidas.Columns("FechaVencimiento").HeaderText = "Vencimiento"
        dgvSalidas.Columns("FechaVencimiento").DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvSalidas.Columns("FechaVencimiento").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        dgvSalidas.Columns("DiasRestantes").HeaderText = "Días"
        dgvSalidas.Columns("DiasRestantes").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        dgvSalidas.Columns("Estado").HeaderText = "Estado"
        dgvSalidas.Columns("Estado").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        dgvSalidas.Columns("Autorizacion").HeaderText = "Autorización"
        dgvSalidas.Columns("Autorizacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        dgvSalidas.Columns("NotificacionJudicial").HeaderText = "Notificación Juez"
        dgvSalidas.Columns("NotificacionJudicial").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        dgvSalidas.Columns("ReferenciaDocumentacion").HeaderText = "Documentación"
        dgvSalidas.Columns("ReferenciaDocumentacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
    End Sub

    Private Sub Resumir(lista As List(Of SalidaGridDto))
        Dim total = lista.Count
        Dim diasAlerta = ObtenerDiasAnticipacionAlerta()

        Dim activas = lista.Where(Function(s) s.Activo).ToList()
        Dim desactivadas = lista.Where(Function(s) Not s.Activo).ToList()

        Dim activasNormales = activas.Where(Function(s) s.DiasRestantes > diasAlerta).Count()
        Dim activasEnAlerta = activas.Where(Function(s) s.DiasRestantes >= 0 AndAlso s.DiasRestantes <= diasAlerta).Count()
        Dim activasVencidas = activas.Where(Function(s) s.DiasRestantes < 0).Count()

        Dim desactivadasNormales = desactivadas.Where(Function(s) s.DiasRestantes > diasAlerta).Count()
        Dim desactivadasEnAlerta = desactivadas.Where(Function(s) s.DiasRestantes >= 0 AndAlso s.DiasRestantes <= diasAlerta).Count()
        Dim desactivadasVencidas = desactivadas.Where(Function(s) s.DiasRestantes < 0).Count()

        Dim conDocumentacion = lista.Where(Function(s) s.CantidadDocumentos > 0).Count()
        Dim sinDocumentacion = total - conDocumentacion

        lblResumen.Text =
            $"Activas ({activas.Count}/{total}) → Normal: {activasNormales} | Alerta ≤{diasAlerta} días: {activasEnAlerta} | Vencidas: {activasVencidas}{Environment.NewLine}" &
            $"Desactivadas ({desactivadas.Count}) → Normal: {desactivadasNormales} | Alerta: {desactivadasEnAlerta} | Vencidas: {desactivadasVencidas}{Environment.NewLine}" &
            $"Documentación → Con respaldo: {conDocumentacion} | Sin respaldo: {sinDocumentacion}"
    End Sub

    Private Function ObtenerSeleccionActual() As SalidaGridDto
        If dgvSalidas.SelectedRows.Count = 0 Then Return Nothing
        Return TryCast(dgvSalidas.SelectedRows(0).DataBoundItem, SalidaGridDto)
    End Function

    Private Sub dgvSalidas_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSalidas.SelectionChanged
        Dim selected = ObtenerSeleccionActual()
        btnEditar.Enabled = (selected IsNot Nothing)
        btnDesactivar.Enabled = (selected IsNot Nothing AndAlso selected.Activo)
        btnReactivar.Enabled = (selected IsNot Nothing AndAlso Not selected.Activo)
        btnAbrirDocumento.Enabled = (selected IsNot Nothing AndAlso selected.CantidadDocumentos > 0)
    End Sub

    Private Sub dgvSalidas_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles dgvSalidas.RowPrePaint
        If e.RowIndex < 0 Then Return
        Dim row = dgvSalidas.Rows(e.RowIndex)
        Dim data = TryCast(row.DataBoundItem, SalidaGridDto)
        If data Is Nothing Then Return

        If data.Estado = "VENCIDA" Then
            row.DefaultCellStyle.BackColor = Color.MistyRose
            row.DefaultCellStyle.ForeColor = Color.DarkRed
            row.DefaultCellStyle.SelectionBackColor = Color.IndianRed
            row.DefaultCellStyle.SelectionForeColor = Color.White
        ElseIf data.Estado = "ALERTA" Then
            row.DefaultCellStyle.BackColor = Color.LightYellow
            row.DefaultCellStyle.ForeColor = Color.DarkGoldenrod
            row.DefaultCellStyle.SelectionBackColor = Color.Goldenrod
            row.DefaultCellStyle.SelectionForeColor = Color.Black
        ElseIf data.Estado = "INACTIVA" Then
            row.DefaultCellStyle.BackColor = Color.Gainsboro
            row.DefaultCellStyle.ForeColor = Color.DimGray
            row.DefaultCellStyle.SelectionBackColor = Color.Gray
            row.DefaultCellStyle.SelectionForeColor = Color.White
        Else
            row.DefaultCellStyle.BackColor = Color.White
            row.DefaultCellStyle.ForeColor = Color.Black
            row.DefaultCellStyle.SelectionBackColor = dgvSalidas.DefaultCellStyle.SelectionBackColor
            row.DefaultCellStyle.SelectionForeColor = dgvSalidas.DefaultCellStyle.SelectionForeColor
        End If
    End Sub

    Private Async Sub btnNueva_Click(sender As Object, e As EventArgs) Handles btnNueva.Click
        Using wizard As New frmNuevaSalidaArt120Wizard()
            If wizard.ShowDialog(Me) = DialogResult.OK Then
                Await CargarSalidasAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Dim sel = ObtenerSeleccionActual()
        If sel Is Nothing Then
            Notifier.Warn(Me, "Seleccione una salida para editar.")
            Return
        End If

        Using wizard As New frmNuevaSalidaArt120Wizard(sel.IdSalida)
            If wizard.ShowDialog(Me) = DialogResult.OK Then
                Await CargarSalidasAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnBuscarRecluso_Click(sender As Object, e As EventArgs) Handles btnBuscarRecluso.Click
        Dim f As New frmBuscadorReclusos()
        If f.ShowDialog(Me) <> DialogResult.OK Then Return

        Using uow As New UnitOfWork()
            Dim repo = uow.Repository(Of Mae_Reclusos)()
            Dim idRecluso = f.ResultadoIdRecluso
            Dim recluso As Mae_Reclusos = Nothing

            If idRecluso.HasValue Then
                recluso = repo.GetQueryable().FirstOrDefault(Function(r) r.IdRecluso = idRecluso.Value)
            ElseIf Not String.IsNullOrWhiteSpace(f.ResultadoFormateado) Then
                Dim nombre = f.ResultadoFormateado
                recluso = repo.GetQueryable().FirstOrDefault(Function(r) r.NombreCompleto = nombre)
            End If
            If recluso Is Nothing Then
                Notifier.Warn(Me, "No se encontró el recluso seleccionado.")
                Return
            End If

            _idReclusoSeleccionado = recluso.IdRecluso
            txtRecluso.Text = recluso.NombreCompleto
            lblIdRecluso.Text = "ID: " & recluso.IdRecluso
        End Using

        Await CargarDocumentosRespaldoAsync(_idReclusoSeleccionado.Value, txtRecluso.Text.Trim(), Nothing)
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If _guardando Then Return

        IncluirDocumentoSeleccionadoEnCombo()
        If Not ValidarEditor() Then Return

        _guardando = True
        CambiarEstadoOperacion(True)

        Try
            Using uow As New UnitOfWork()
                Dim repo = uow.Repository(Of Tra_SalidasLaborales)()
                Dim entidad As Tra_SalidasLaborales

                If _idSalidaEditando.HasValue Then
                    entidad = Await repo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(s) s.IdSalida = _idSalidaEditando.Value)
                    If entidad Is Nothing Then
                        Notifier.Warn(Me, "La salida seleccionada ya no existe.")
                        Return
                    End If
                Else
                    entidad = New Tra_SalidasLaborales()
                    repo.Add(entidad)
                End If

                entidad.IdRecluso = _idReclusoSeleccionado.Value
                entidad.LugarTrabajo = txtLugarTrabajo.Text.Trim()
                entidad.Horario = txtHorario.Text.Trim()
                entidad.DetalleCustodia = txtCustodia.Text.Trim()
                entidad.FechaInicio = dtpInicio.Value.Date
                entidad.FechaVencimiento = dtpVencimiento.Value.Date
                entidad.IdDocumentoRespaldo = ObtenerDocumentoPrincipalSeleccionado()
                entidad.FechaNotificacionJuez = If(dtpFechaNotificacion.Checked, CType(dtpFechaNotificacion.Value.Date, Nullable(Of Date)), Nothing)
                entidad.Activo = chkActivoRegistro.Checked
                entidad.Observaciones = ConstruirObservacionesConAutorizacion(ObtenerCodigoAutorizacionSeleccionado(), txtObservaciones.Text)

                Await uow.CommitAsync()
                Await GuardarDocumentosRespaldoSalidaAsync(uow, entidad.IdSalida, ObtenerIdsDocumentosSeleccionados())
                Await uow.CommitAsync()
            End Using

            Notifier.Success(Me, "Datos de renovación guardados correctamente.")
            LimpiarEditor()
            Await CargarSalidasAsync()
        Catch ex As Exception
            Notifier.[Error](Me, "No se pudo guardar la salida: " & ex.Message)
        Finally
            _guardando = False
            CambiarEstadoOperacion(False)
        End Try
    End Sub

    Private Sub IncluirDocumentoSeleccionadoEnCombo()
        If _cargandoDocumentos Then Return

        Dim idDocCombo = ParseNullableLong(cboDocumentoRespaldo.SelectedValue)
        If Not idDocCombo.HasValue Then Return

        If _documentosSeleccionados.Any(Function(d) d.IdDocumento = idDocCombo.Value) Then Return

        Dim doc = _documentosDisponibles.FirstOrDefault(Function(d) d.IdDocumento = idDocCombo.Value)
        If doc Is Nothing Then
            doc = New DocumentoRespaldoDto With {
                .IdDocumento = idDocCombo.Value,
                .Texto = ConstruirTextoSugeridoDocumento(idDocCombo.Value, Nothing, Nothing, Nothing),
                .Asunto = Nothing
            }
        End If

        _documentosSeleccionados.Add(doc)
        RefrescarListaDocumentosSeleccionados()
    End Sub

    Private Function ValidarEditor() As Boolean
        If Not _idReclusoSeleccionado.HasValue Then
            Notifier.Warn(Me, "Debe seleccionar una persona privada de libertad.")
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtLugarTrabajo.Text) Then
            Notifier.Warn(Me, "Ingrese el lugar de trabajo.")
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtHorario.Text) Then
            Notifier.Warn(Me, "Ingrese el horario autorizado.")
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtCustodia.Text) Then
            Notifier.Warn(Me, "Ingrese el detalle de custodia (obligatorio por decreto).")
            Return False
        End If

        Dim codigoAutorizacion = ObtenerCodigoAutorizacionSeleccionado()
        If String.IsNullOrWhiteSpace(codigoAutorizacion) Then
            Notifier.Warn(Me, "Indique la autorización de salida (Resolución de un Juez o Acta).")
            cboAutorizacion.Focus()
            Return False
        End If

        If dtpVencimiento.Value.Date < dtpInicio.Value.Date Then
            Notifier.Warn(Me, "La fecha de vencimiento no puede ser menor a la fecha de inicio.")
            Return False
        End If

        If dtpFechaNotificacion.Checked AndAlso dtpFechaNotificacion.Value.Date < dtpInicio.Value.Date Then
            Notifier.Warn(Me, "La fecha de notificación al juez no puede ser menor al inicio.")
            Return False
        End If

        If _idSalidaEditando.HasValue AndAlso Not chkActivoRegistro.Checked AndAlso chkSoloActivas.Checked Then
            Notifier.Info(Me, "Al guardar, el registro quedará oculto porque tiene activado el filtro 'Solo activas'.")
        End If

        Return True
    End Function

    Private Async Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        Await CambiarEstadoSeleccionAsync(False)
    End Sub

    Private Async Sub btnReactivar_Click(sender As Object, e As EventArgs) Handles btnReactivar.Click
        Await CambiarEstadoSeleccionAsync(True)
    End Sub

    Private Async Function CambiarEstadoSeleccionAsync(nuevoEstado As Boolean) As Task
        If _guardando Then Return

        Dim sel = ObtenerSeleccionActual()
        If sel Is Nothing Then
            Notifier.Warn(Me, "Seleccione una salida.")
            Return
        End If

        Try
            Using uow As New UnitOfWork()
                Dim repo = uow.Repository(Of Tra_SalidasLaborales)()
                Dim entidad = Await repo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(s) s.IdSalida = sel.IdSalida)
                If entidad Is Nothing Then
                    Notifier.Warn(Me, "La salida seleccionada ya no existe.")
                    Return
                End If

                entidad.Activo = nuevoEstado
                If Not nuevoEstado Then
                    Dim motivo = InputBox("Ingrese motivo de cese (obligatorio):", "Motivo de cese de salidas")
                    If String.IsNullOrWhiteSpace(motivo) Then
                        Notifier.Warn(Me, "Debe ingresar el motivo de cese de salidas.")
                        Return
                    End If

                    Dim marcaTiempo = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
                    Dim detalleSuspension = $"[{marcaTiempo}] Motivo cese: {motivo.Trim()}"
                    If String.IsNullOrWhiteSpace(entidad.Observaciones) Then
                        entidad.Observaciones = detalleSuspension
                    Else
                        entidad.Observaciones &= Environment.NewLine & detalleSuspension
                    End If
                End If

                Await uow.CommitAsync()
            End Using

            Notifier.Success(Me, If(nuevoEstado, "Salida reactivada.", "Salida suspendida y motivo registrado."))
            Await CargarSalidasAsync()
        Catch ex As Exception
            Notifier.[Error](Me, "No se pudo actualizar el estado: " & ex.Message)
        End Try
    End Function

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LimpiarEditor()
    End Sub

    Private Async Sub chkSoloActivas_CheckedChanged(sender As Object, e As EventArgs) Handles chkSoloActivas.CheckedChanged
        If _guardando Then Return

        Await CargarSalidasAsync()
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        AplicarFiltro()
    End Sub

    Private Sub CambiarEstadoOperacion(guardando As Boolean)
        btnGuardar.Enabled = Not guardando
        btnCancelar.Enabled = Not guardando
        btnNueva.Enabled = Not guardando
        btnEditar.Enabled = Not guardando
        btnDesactivar.Enabled = Not guardando
        btnReactivar.Enabled = Not guardando
        btnBuscarRecluso.Enabled = Not guardando
        btnAgregarDocumento.Enabled = Not guardando
        btnQuitarDocumento.Enabled = Not guardando
        btnRefrescarDocumentos.Enabled = Not guardando
        btnAbrirDocumento.Enabled = Not guardando
        btnConfigurarRenovaciones.Enabled = Not guardando

        dgvSalidas.Enabled = Not guardando
        PanelFiltros.Enabled = Not guardando
        PanelEditor.Enabled = Not guardando
        UseWaitCursor = guardando
    End Sub

    Private Sub LimpiarEditor()
        _idSalidaEditando = Nothing
        _idReclusoSeleccionado = Nothing

        txtRecluso.Clear()
        lblIdRecluso.Text = "ID: (ninguno)"
        txtLugarTrabajo.Clear()
        txtHorario.Clear()
        txtCustodia.Clear()
        cboAutorizacion.SelectedIndex = 0
        dtpInicio.Value = Date.Today
        dtpVencimiento.Value = Date.Today.AddMonths(1)
        chkActivoRegistro.Checked = True
        dtpFechaNotificacion.Checked = False
        dtpFechaNotificacion.Value = Date.Today
        CargarDocumentosRespaldoVacio()
        LimpiarDocumentosSeleccionados()
        txtDocumentoDescripcion.Clear()
        txtObservaciones.Clear()
    End Sub



    Private Sub CargarOpcionesAutorizacion()
        Dim opciones = New List(Of Object) From {
            New With {.Text = "(seleccione tipo de autorización)", .Value = ""},
            New With {.Text = "Resolución de un Juez", .Value = CodigoAutorizacionResolucionJuez},
            New With {.Text = "Acta", .Value = CodigoAutorizacionActa}
        }

        cboAutorizacion.DisplayMember = "Text"
        cboAutorizacion.ValueMember = "Value"
        cboAutorizacion.DataSource = opciones
        cboAutorizacion.SelectedIndex = 0
    End Sub

    Private Function ObtenerCodigoAutorizacionSeleccionado() As String
        Return If(cboAutorizacion.SelectedValue, "").ToString().Trim().ToUpperInvariant()
    End Function

    Private Sub SeleccionarAutorizacionEnCombo(codigo As String)
        Dim codigoNormalizado = If(codigo, "").Trim().ToUpperInvariant()
        If String.IsNullOrWhiteSpace(codigoNormalizado) Then
            cboAutorizacion.SelectedIndex = 0
            Return
        End If

        cboAutorizacion.SelectedValue = codigoNormalizado
        If If(cboAutorizacion.SelectedValue, "").ToString().Trim().ToUpperInvariant() <> codigoNormalizado Then
            cboAutorizacion.SelectedIndex = 0
        End If
    End Sub

    Private Function ConstruirObservacionesConAutorizacion(codigoAutorizacion As String, observacionesUsuario As String) As String
        Dim lineas As New List(Of String)()
        Dim codigo = If(codigoAutorizacion, "").Trim().ToUpperInvariant()
        Dim texto = If(observacionesUsuario, "").Trim()

        If Not String.IsNullOrWhiteSpace(codigo) Then
            lineas.Add(PrefijoAutorizacionObservaciones & codigo)
        End If

        If Not String.IsNullOrWhiteSpace(texto) Then
            lineas.Add(texto)
        End If

        If lineas.Count = 0 Then Return Nothing
        Return String.Join(Environment.NewLine, lineas)
    End Function

    Private Function ParsearObservacionesConAutorizacion(observaciones As String) As ObservacionesSalidaDto
        Dim resultado As New ObservacionesSalidaDto With {
            .CodigoAutorizacion = "",
            .ObservacionesUsuario = ""
        }

        If String.IsNullOrWhiteSpace(observaciones) Then
            Return resultado
        End If

        Dim lineas = observaciones.Split({Environment.NewLine}, StringSplitOptions.None).ToList()
        If lineas.Count > 0 AndAlso lineas(0).Trim().StartsWith(PrefijoAutorizacionObservaciones, StringComparison.OrdinalIgnoreCase) Then
            Dim codigo = lineas(0).Trim().Substring(PrefijoAutorizacionObservaciones.Length).Trim().ToUpperInvariant()
            resultado.CodigoAutorizacion = codigo
            lineas.RemoveAt(0)
            resultado.ObservacionesUsuario = String.Join(Environment.NewLine, lineas).Trim()
            Return resultado
        End If

        resultado.CodigoAutorizacion = InferirCodigoAutorizacionDesdeTexto(observaciones)
        resultado.ObservacionesUsuario = observaciones.Trim()
        Return resultado
    End Function

    Private Function InferirCodigoAutorizacionDesdeTexto(texto As String) As String
        Dim textoMayus = If(texto, "").ToUpperInvariant()
        If textoMayus.Contains("RESOLU") AndAlso textoMayus.Contains("JUEZ") Then
            Return CodigoAutorizacionResolucionJuez
        End If

        If textoMayus.Contains("ACTA") Then
            Return CodigoAutorizacionActa
        End If

        Return ""
    End Function

    Private Function ConvertirDescripcionACodigoAutorizacion(descripcion As String) As String
        Dim valor = If(descripcion, "").Trim().ToUpperInvariant()
        If valor = "RESOLUCIÓN DE UN JUEZ" OrElse valor = "RESOLUCION DE UN JUEZ" Then
            Return CodigoAutorizacionResolucionJuez
        End If

        If valor = "ACTA" Then
            Return CodigoAutorizacionActa
        End If

        Return ""
    End Function

    Private Function ObtenerDescripcionAutorizacion(codigo As String) As String
        Select Case If(codigo, "").Trim().ToUpperInvariant()
            Case CodigoAutorizacionResolucionJuez
                Return "Resolución de un Juez"
            Case CodigoAutorizacionActa
                Return "Acta"
            Case Else
                Return "Sin especificar"
        End Select
    End Function

    Private Function ParseNullableLong(value As Object) As Nullable(Of Long)
        Dim txt = If(value, "").ToString()
        If String.IsNullOrWhiteSpace(txt) Then Return Nothing

        Dim n As Long
        If Long.TryParse(txt.Trim(), n) Then
            Return n
        End If

        Return Nothing
    End Function

    Private Sub CargarDocumentosRespaldoVacio()
        _cargandoDocumentos = True
        cboDocumentoRespaldo.DisplayMember = "Text"
        cboDocumentoRespaldo.ValueMember = "Value"
        cboDocumentoRespaldo.DataSource = New List(Of Object) From {
            New With {.Text = "(sin documentos sugeridos)", .Value = ""}
        }
        cboDocumentoRespaldo.SelectedIndex = 0
        _cargandoDocumentos = False
    End Sub

    Private Sub LimpiarDocumentosSeleccionados()
        _documentosSeleccionados = New List(Of DocumentoRespaldoDto)()
        RefrescarListaDocumentosSeleccionados()
    End Sub

    Private Function ConstruirTextoSugeridoDocumento(idDocumento As Long,
                                                     tipoDocumento As String,
                                                     numeroOficial As String,
                                                     asunto As String) As String
        Dim tipo = If(String.IsNullOrWhiteSpace(tipoDocumento), "DOC", tipoDocumento.Trim().ToUpper())
        Dim numero = If(String.IsNullOrWhiteSpace(numeroOficial), "S/N", numeroOficial.Trim())
        Dim asuntoLimpio = If(String.IsNullOrWhiteSpace(asunto), "(sin asunto)", asunto.Trim())

        Return $"{tipo} {numero} | {asuntoLimpio} (ID {idDocumento})"
    End Function

    Private Function NormalizarAsunto(asunto As String) As String
        If String.IsNullOrWhiteSpace(asunto) Then Return String.Empty

        Return New String(asunto.Trim().ToUpperInvariant().Where(Function(c) Char.IsLetterOrDigit(c) OrElse Char.IsWhiteSpace(c)).ToArray())
    End Function

    Private Sub SugerirDocumentosRelacionados(docBase As DocumentoRespaldoDto)
        If docBase Is Nothing Then Return

        Dim asuntoNormalizado = NormalizarAsunto(docBase.Asunto)
        If String.IsNullOrWhiteSpace(asuntoNormalizado) Then Return

        Dim sugeridos = _documentosDisponibles _
            .Where(Function(d) d.IdDocumento <> docBase.IdDocumento) _
            .Where(Function(d) Not _documentosSeleccionados.Any(Function(s) s.IdDocumento = d.IdDocumento)) _
            .Where(Function(d) NormalizarAsunto(d.Asunto) = asuntoNormalizado) _
            .OrderBy(Function(d) d.Texto) _
            .ToList()

        If sugeridos.Count = 0 Then Return

        Dim resultado = MessageBox.Show(Me,
                                        $"Se encontraron {sugeridos.Count} documento(s) con el mismo asunto. ¿Desea agregarlos también?",
                                        "Documentos relacionados",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question)

        If resultado <> DialogResult.Yes Then Return

        _documentosSeleccionados.AddRange(sugeridos)
        RefrescarListaDocumentosSeleccionados()
    End Sub

    Private Sub RefrescarListaDocumentosSeleccionados()
        ' CORRECCIÓN: Quitamos la parte de ".Select(Function(d) New With...)"
        ' Al pasar la lista directa, el ListBox usará correctamente la propiedad "Texto"
        ' definida en el DisplayMember, en lugar de mostrar la estructura interna del objeto.

        Dim lista = _documentosSeleccionados _
            .OrderBy(Function(d) d.Texto) _
            .ToList()

        lstDocumentosRespaldo.DisplayMember = "Texto"
        lstDocumentosRespaldo.ValueMember = "IdDocumento"
        lstDocumentosRespaldo.DataSource = Nothing
        lstDocumentosRespaldo.DataSource = lista

        btnQuitarDocumento.Enabled = (lista.Count > 0)
        ActualizarAsuntoDocumentoSeleccionado()
    End Sub

    Private Sub ActualizarAsuntoDocumentoSeleccionado()
        Dim docSeleccionado = TryCast(lstDocumentosRespaldo.SelectedItem, DocumentoRespaldoDto)

        If docSeleccionado Is Nothing OrElse String.IsNullOrWhiteSpace(docSeleccionado.Asunto) Then
            txtDocumentoDescripcion.Clear()
            Return
        End If

        txtDocumentoDescripcion.Text = docSeleccionado.Asunto.Trim()
    End Sub

    Private Function ObtenerDocumentoPrincipalSeleccionado() As Nullable(Of Long)
        Dim ids = ObtenerIdsDocumentosSeleccionados()
        If ids.Count = 0 Then Return Nothing
        Return ids(0)
    End Function

    Private Function ObtenerIdsDocumentosSeleccionados() As List(Of Long)
        Return _documentosSeleccionados _
            .Select(Function(d) d.IdDocumento) _
            .Distinct() _
            .OrderBy(Function(id) id) _
            .ToList()
    End Function

    Private Async Function CargarDocumentosRespaldoAsync(idRecluso As Integer, nombreRecluso As String, idSeleccionado As Nullable(Of Long)) As Task
        If _cargandoDocumentos Then Return

        Try
            _cargandoDocumentos = True
            btnRefrescarDocumentos.Enabled = False

            Using uow As New UnitOfWork()
                Dim repoDocs = uow.Repository(Of Mae_Documento)()
                Dim repoSalidas = uow.Repository(Of Tra_SalidasLaborales)()

                Dim idsDesdeSalidas = Await repoSalidas.GetQueryable(tracking:=False) _
                    .Where(Function(s) s.IdRecluso = idRecluso AndAlso s.IdDocumentoRespaldo.HasValue) _
                    .Select(Function(s) s.IdDocumentoRespaldo.Value) _
                    .Distinct() _
                    .ToListAsync()

                Dim nombre = nombreRecluso.Trim()
                Dim docsPorTexto = Await repoDocs.GetQueryable(tracking:=False) _
                    .Where(Function(d) d.Asunto.Contains(nombre) OrElse d.Descripcion.Contains(nombre)) _
                    .OrderByDescending(Function(d) d.FechaCreacion) _
                    .Select(Function(d) New With {
                        .IdDocumento = d.IdDocumento,
                        .NumeroOficial = d.NumeroOficial,
                        .Asunto = d.Asunto,
                        .Fecha = d.FechaCreacion
                    }) _
                    .Take(50) _
                    .ToListAsync()

                Dim ids = idsDesdeSalidas.Union(docsPorTexto.Select(Function(d) CLng(d.IdDocumento))).Distinct().ToList()

                Dim docsIds = Await repoDocs.GetQueryable(tracking:=False) _
                    .Include("Cat_TipoDocumento") _
                    .Where(Function(d) ids.Contains(d.IdDocumento)) _
                    .OrderByDescending(Function(d) d.FechaCreacion) _
                    .Select(Function(d) New With {
                        .IdDocumento = d.IdDocumento,
                        .NumeroOficial = d.NumeroOficial,
                        .Asunto = d.Asunto,
                        .Fecha = d.FechaCreacion,
                        .Tipo = d.Cat_TipoDocumento.Nombre
                    }) _
                    .ToListAsync()

                Dim listaCombo = New List(Of Object) From {
                    New With {.Text = "(seleccione para agregar)", .Value = ""}
                }

                _documentosDisponibles = New List(Of DocumentoRespaldoDto)()

                For Each doc In docsIds
                    Dim etiqueta = ConstruirTextoSugeridoDocumento(doc.IdDocumento, doc.Tipo, doc.NumeroOficial, doc.Asunto)

                    _documentosDisponibles.Add(New DocumentoRespaldoDto With {
                        .IdDocumento = doc.IdDocumento,
                        .Texto = etiqueta,
                        .Asunto = doc.Asunto,
                        .TipoDocumento = doc.Tipo,
                        .NumeroOficial = doc.NumeroOficial
                    })

                    listaCombo.Add(New With {
                        .Text = etiqueta,
                        .Value = doc.IdDocumento.ToString()
                    })
                Next

                cboDocumentoRespaldo.DisplayMember = "Text"
                cboDocumentoRespaldo.ValueMember = "Value"
                cboDocumentoRespaldo.DataSource = listaCombo
                cboDocumentoRespaldo.SelectedIndex = 0

                Await CargarDocumentosSeleccionadosAsync(idSeleccionado)
            End Using
        Catch ex As Exception
            CargarDocumentosRespaldoVacio()
            Notifier.Warn(Me, "No se pudieron cargar expedientes/documentos sugeridos: " & ex.Message)
        Finally
            _cargandoDocumentos = False
            btnRefrescarDocumentos.Enabled = True
        End Try
    End Function

    Private Async Function CargarDocumentosSeleccionadosAsync(idSeleccionado As Nullable(Of Long)) As Task
        If Not _idSalidaEditando.HasValue Then
            LimpiarDocumentosSeleccionados()

            If idSeleccionado.HasValue Then
                Dim doc = _documentosDisponibles.FirstOrDefault(Function(d) d.IdDocumento = idSeleccionado.Value)
                If doc IsNot Nothing Then
                    _documentosSeleccionados.Add(doc)
                Else
                    _documentosSeleccionados.Add(New DocumentoRespaldoDto With {
                        .IdDocumento = idSeleccionado.Value,
                        .Texto = ConstruirTextoSugeridoDocumento(idSeleccionado.Value, Nothing, Nothing, Nothing),
                        .Asunto = Nothing
                    })
                End If
            End If

            RefrescarListaDocumentosSeleccionados()
            Return
        End If

        Try
            Using uow As New UnitOfWork()
                Dim tablaExiste = Await ExisteTablaRespaldoAsync(uow)
                If tablaExiste Then
                    Dim sql = "SELECT IdDocumento FROM Tra_SalidasLaboralesDocumentoRespaldo WHERE IdSalida = @p0 ORDER BY IdDocumento"
                    Dim ids = Await uow.Context.Database.SqlQuery(Of Long)(sql, _idSalidaEditando.Value).ToListAsync()

                    _documentosSeleccionados = ids _
                        .Distinct() _
                        .Select(Function(id)
                                    Dim encontrado = _documentosDisponibles.FirstOrDefault(Function(d) d.IdDocumento = id)
                                    If encontrado IsNot Nothing Then Return encontrado
                                    Return New DocumentoRespaldoDto With {
                                        .IdDocumento = id,
                                        .Texto = ConstruirTextoSugeridoDocumento(id, Nothing, Nothing, Nothing),
                                        .Asunto = Nothing
                                    }
                                End Function) _
                        .ToList()

                    If _documentosSeleccionados.Count = 0 AndAlso idSeleccionado.HasValue Then
                        Dim docPrincipal = _documentosDisponibles.FirstOrDefault(Function(d) d.IdDocumento = idSeleccionado.Value)
                        If docPrincipal IsNot Nothing Then
                            _documentosSeleccionados.Add(docPrincipal)
                        Else
                            _documentosSeleccionados.Add(New DocumentoRespaldoDto With {
                                .IdDocumento = idSeleccionado.Value,
                                .Texto = ConstruirTextoSugeridoDocumento(idSeleccionado.Value, Nothing, Nothing, Nothing),
                                .Asunto = Nothing
                            })
                        End If
                    End If
                Else
                    LimpiarDocumentosSeleccionados()
                    If idSeleccionado.HasValue Then
                        Dim doc = _documentosDisponibles.FirstOrDefault(Function(d) d.IdDocumento = idSeleccionado.Value)
                        If doc IsNot Nothing Then
                            _documentosSeleccionados.Add(doc)
                        Else
                            _documentosSeleccionados.Add(New DocumentoRespaldoDto With {
                                .IdDocumento = idSeleccionado.Value,
                                .Texto = ConstruirTextoSugeridoDocumento(idSeleccionado.Value, Nothing, Nothing, Nothing),
                                .Asunto = Nothing
                            })
                        End If
                    End If
                End If
            End Using
        Catch ex As Exception
            LimpiarDocumentosSeleccionados()
            If idSeleccionado.HasValue Then
                _documentosSeleccionados.Add(New DocumentoRespaldoDto With {
                    .IdDocumento = idSeleccionado.Value,
                    .Texto = ConstruirTextoSugeridoDocumento(idSeleccionado.Value, Nothing, Nothing, Nothing),
                    .Asunto = Nothing
                })
            End If
            Notifier.Warn(Me, "No se pudo cargar la lista de documentos respaldo: " & ex.Message)
        End Try

        RefrescarListaDocumentosSeleccionados()
    End Function

    Private Async Function GuardarDocumentosRespaldoSalidaAsync(uow As UnitOfWork, idSalida As Integer, idsDocumento As List(Of Long)) As Task
        If uow Is Nothing Then Return

        Try
            Await uow.Context.Database.ExecuteSqlCommandAsync("DELETE FROM Tra_SalidasLaboralesDocumentoRespaldo WHERE IdSalida = @p0", idSalida)

            For Each idDoc In idsDocumento.Distinct().OrderBy(Function(x) x)
                Await uow.Context.Database.ExecuteSqlCommandAsync(
                    "INSERT INTO Tra_SalidasLaboralesDocumentoRespaldo (IdSalida, IdDocumento) VALUES (@p0, @p1)",
                    idSalida,
                    idDoc)
            Next
        Catch ex As Exception
            Notifier.Warn(Me, "No se pudieron guardar los documentos de respaldo asociados: " & ex.Message)
        End Try
    End Function

    Private Async Function ObtenerDocumentosPorSalidaAsync(uow As UnitOfWork, idsSalida As List(Of Integer)) As Task(Of Dictionary(Of Integer, List(Of Long)))
        Dim resultado = idsSalida.Distinct().ToDictionary(Function(id) id, Function(id) New List(Of Long)())
        If idsSalida.Count = 0 Then Return resultado

        Dim repoSalidas = uow.Repository(Of Tra_SalidasLaborales)()
        Dim docsPrincipal = Await repoSalidas.GetQueryable(tracking:=False) _
            .Where(Function(s) idsSalida.Contains(s.IdSalida) AndAlso s.IdDocumentoRespaldo.HasValue) _
            .Select(Function(s) New With {
                .IdSalida = s.IdSalida,
                .IdDocumento = s.IdDocumentoRespaldo.Value
            }) _
            .ToListAsync()

        For Each item In docsPrincipal
            If Not resultado.ContainsKey(item.IdSalida) Then
                resultado(item.IdSalida) = New List(Of Long)()
            End If

            If Not resultado(item.IdSalida).Contains(item.IdDocumento) Then
                resultado(item.IdSalida).Add(item.IdDocumento)
            End If
        Next

        Try
            Dim docsRelacion = Await uow.Context.Set(Of Tra_SalidasLaboralesDocumentoRespaldo)() _
                .Where(Function(r) idsSalida.Contains(r.IdSalida)) _
                .Select(Function(r) New With {
                    .IdSalida = r.IdSalida,
                    .IdDocumento = r.IdDocumento
                }) _
                .ToListAsync()

            For Each item In docsRelacion
                If Not resultado.ContainsKey(item.IdSalida) Then
                    resultado(item.IdSalida) = New List(Of Long)()
                End If

                If Not resultado(item.IdSalida).Contains(item.IdDocumento) Then
                    resultado(item.IdSalida).Add(item.IdDocumento)
                End If
            Next
        Catch ex As Exception
            Debug.WriteLine("No se pudieron consultar documentos de respaldo por relación: " & ex.Message)
        End Try

        For Each idSalida In resultado.Keys.ToList()
            resultado(idSalida) = resultado(idSalida).Distinct().OrderBy(Function(x) x).ToList()
        Next

        Return resultado
    End Function

    Private Async Function ObtenerDocumentosSalidaAsync(idSalida As Integer) As Task(Of List(Of DocumentoRespaldoDto))
        Using uow As New UnitOfWork()
            Dim docsPorSalida = Await ObtenerDocumentosPorSalidaAsync(uow, New List(Of Integer) From {idSalida})
            Dim ids = If(docsPorSalida.ContainsKey(idSalida), docsPorSalida(idSalida), New List(Of Long)())
            If ids.Count = 0 Then Return New List(Of DocumentoRespaldoDto)()

            Dim repoDocs = uow.Repository(Of Mae_Documento)()
            Dim documentos = Await repoDocs.GetQueryable(tracking:=False) _
                .Include("Cat_TipoDocumento") _
                .Where(Function(d) ids.Contains(d.IdDocumento)) _
                .Select(Function(d) New With {
                    .IdDocumento = d.IdDocumento,
                    .NumeroOficial = d.NumeroOficial,
                    .Asunto = d.Asunto,
                    .Fecha = d.FechaCreacion,
                    .Tipo = d.Cat_TipoDocumento.Nombre
                }) _
                .ToListAsync()

            Return ids.Select(Function(id)
                                  Dim doc = documentos.FirstOrDefault(Function(x) x.IdDocumento = id)
                                  If doc Is Nothing Then
                                      Return New DocumentoRespaldoDto With {
                                          .IdDocumento = id,
                                          .Texto = $"Documento {id}",
                                          .Asunto = Nothing
                                      }
                                  End If

                                  Dim fechaTxt = If(doc.Fecha.HasValue, doc.Fecha.Value.ToString("dd/MM/yyyy"), "s/f")
                                  Dim numero = If(String.IsNullOrWhiteSpace(doc.NumeroOficial), "S/N", doc.NumeroOficial)
                                  Dim asunto = If(String.IsNullOrWhiteSpace(doc.Asunto), "(sin asunto)", doc.Asunto)
                                  Dim tipo = If(String.IsNullOrWhiteSpace(doc.Tipo), "DOC", doc.Tipo.ToUpper())

                                  Return New DocumentoRespaldoDto With {
                                      .IdDocumento = id,
                                      .Texto = $"{tipo} {numero} | {fechaTxt} | {asunto}",
                                      .Asunto = doc.Asunto
                                  }
                              End Function).ToList()
        End Using
    End Function

    Private Function SeleccionarDocumento(docs As List(Of DocumentoRespaldoDto)) As Nullable(Of Long)
        Using selector As New Form()
            selector.Text = "Documentación vinculada"
            selector.StartPosition = FormStartPosition.CenterParent
            selector.FormBorderStyle = FormBorderStyle.FixedDialog
            selector.MaximizeBox = False
            selector.MinimizeBox = False
            selector.Width = 900
            selector.Height = 420

            Dim lst As New ListBox With {
                .Dock = DockStyle.Fill,
                .DisplayMember = "Texto",
                .ValueMember = "IdDocumento",
                .DataSource = docs
            }

            Dim panelBotones As New FlowLayoutPanel With {
                .FlowDirection = FlowDirection.RightToLeft,
                .Dock = DockStyle.Bottom,
                .Height = 45,
                .Padding = New Padding(8)
            }

            Dim btnVer As New Button With {
                .Text = "Ver detalle",
                .Width = 120,
                .DialogResult = DialogResult.OK
            }

            Dim btnCancelar As New Button With {
                .Text = "Cancelar",
                .Width = 100,
                .DialogResult = DialogResult.Cancel
            }

            panelBotones.Controls.Add(btnVer)
            panelBotones.Controls.Add(btnCancelar)
            selector.Controls.Add(lst)
            selector.Controls.Add(panelBotones)
            selector.AcceptButton = btnVer
            selector.CancelButton = btnCancelar

            If selector.ShowDialog(Me) <> DialogResult.OK Then Return Nothing

            Dim seleccionado = TryCast(lst.SelectedItem, DocumentoRespaldoDto)
            If seleccionado Is Nothing Then Return Nothing
            Return seleccionado.IdDocumento
        End Using
    End Function

    Private Async Function ExisteTablaRespaldoAsync(uow As UnitOfWork) As Task(Of Boolean)
        Dim sql = "SELECT COUNT(1) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tra_SalidasLaboralesDocumentoRespaldo'"
        Dim cantidad = Await uow.Context.Database.SqlQuery(Of Integer)(sql).FirstOrDefaultAsync()
        Return cantidad > 0
    End Function

    Private Async Sub btnRefrescarDocumentos_Click(sender As Object, e As EventArgs) Handles btnRefrescarDocumentos.Click
        If Not _idReclusoSeleccionado.HasValue Then
            Notifier.Warn(Me, "Primero seleccione una persona privada de libertad.")
            Return
        End If

        Await CargarDocumentosRespaldoAsync(_idReclusoSeleccionado.Value, txtRecluso.Text.Trim(), ObtenerDocumentoPrincipalSeleccionado())
    End Sub

    Private Sub cboDocumentoRespaldo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDocumentoRespaldo.SelectedIndexChanged
        If _cargandoDocumentos Then Return
        Dim idDoc = ParseNullableLong(cboDocumentoRespaldo.SelectedValue)
        btnAgregarDocumento.Enabled = idDoc.HasValue
    End Sub

    Private Sub btnAgregarDocumento_Click(sender As Object, e As EventArgs) Handles btnAgregarDocumento.Click
        If _cargandoDocumentos Then Return

        Dim idDoc = ParseNullableLong(cboDocumentoRespaldo.SelectedValue)
        If Not idDoc.HasValue Then
            Notifier.Warn(Me, "Seleccione un documento para agregarlo a la lista.")
            Return
        End If

        If _documentosSeleccionados.Any(Function(d) d.IdDocumento = idDoc.Value) Then
            Notifier.Info(Me, "Ese documento ya está agregado.")
            Return
        End If

        Dim doc = _documentosDisponibles.FirstOrDefault(Function(d) d.IdDocumento = idDoc.Value)
        If doc Is Nothing Then
            doc = New DocumentoRespaldoDto With {
                .IdDocumento = idDoc.Value,
                .Texto = ConstruirTextoSugeridoDocumento(idDoc.Value, Nothing, Nothing, Nothing),
                .Asunto = Nothing
            }
        End If

        _documentosSeleccionados.Add(doc)
        RefrescarListaDocumentosSeleccionados()
        SugerirDocumentosRelacionados(doc)
    End Sub

    Private Sub btnQuitarDocumento_Click(sender As Object, e As EventArgs) Handles btnQuitarDocumento.Click
        Dim idDoc = ParseNullableLong(lstDocumentosRespaldo.SelectedValue)
        If Not idDoc.HasValue Then Return

        _documentosSeleccionados = _documentosSeleccionados.Where(Function(d) d.IdDocumento <> idDoc.Value).ToList()
        RefrescarListaDocumentosSeleccionados()
    End Sub

    Private Sub lstDocumentosRespaldo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDocumentosRespaldo.SelectedIndexChanged
        ActualizarAsuntoDocumentoSeleccionado()
    End Sub

    Private Async Sub btnAbrirDocumento_Click(sender As Object, e As EventArgs) Handles btnAbrirDocumento.Click
        Dim sel = ObtenerSeleccionActual()
        If sel Is Nothing Then
            Notifier.Warn(Me, "Seleccione una salida.")
            Return
        End If

        If sel.CantidadDocumentos = 0 Then
            Notifier.Warn(Me, "La salida seleccionada no tiene documentación vinculada.")
            Return
        End If

        Try
            Dim docs = Await ObtenerDocumentosSalidaAsync(sel.IdSalida)
            If docs.Count = 0 Then
                Notifier.Warn(Me, "No se encontraron documentos para la salida seleccionada.")
                Return
            End If

            Dim idSeleccionado As Nullable(Of Long)
            If docs.Count = 1 Then
                idSeleccionado = docs(0).IdDocumento
            Else
                idSeleccionado = SeleccionarDocumento(docs)
            End If

            If Not idSeleccionado.HasValue Then Return

            Dim f As New frmDetalleDocumento(idSeleccionado.Value)
            ShowFormInMdi(Me, f)
        Catch ex As Exception
            Notifier.[Error](Me, "No se pudo abrir la documentación: " & ex.Message)
        End Try
    End Sub

    Private Async Sub btnConfigurarRenovaciones_Click(sender As Object, e As EventArgs) Handles btnConfigurarRenovaciones.Click
        Using f As New frmConfiguracionSistema()
            If f.ShowDialog(Me) = DialogResult.OK Then
                Await CargarConfiguracionDiasAlertaAsync()
                Await CargarSalidasAsync()
            End If
        End Using
    End Sub

End Class
