Imports System.Data.Entity
Imports System.Linq

Public Class frmNuevaSalidaArt120Wizard

    ' Definimos constantes
    Private Const CodigoAutorizacionResolucionJuez As String = "RESOLUCION_JUEZ"
    Private Const CodigoAutorizacionActa As String = "ACTA"
    Private Const PrefijoAutorizacionObservaciones As String = "#AUTORIZACION#:"

    Private Class ObservacionesSalidaDto
        Public Property CodigoAutorizacion As String
        Public Property ObservacionesUsuario As String
    End Class

    Private _idReclusoSeleccionado As Integer?
    Private _idSalidaEditando As Integer?
    Private _pasoActual As Integer = 0
    Private _cargandoEdicion As Boolean

    Public Sub New()
        ' Esta llamada es obligatoria para inicializar los controles definidos en el Diseñador
        InitializeComponent()

        ' Configuraciones iniciales
        CargarOpcionesAutorizacion()
        ActualizarPaso()

        ' Inicializamos vencimiento por defecto
        DtpVencimiento.Value = Date.Today.AddMonths(1)
    End Sub

    Public Sub New(idSalida As Integer)
        Me.New()
        _idSalidaEditando = idSalida
        Me.Text = "Editar salida laboral - Art. 120"
    End Sub

    Private Async Sub frmNuevaSalidaArt120Wizard_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        If _cargandoEdicion OrElse Not _idSalidaEditando.HasValue Then Return

        _cargandoEdicion = True

        Try
            Await CargarDatosEdicionAsync(_idSalidaEditando.Value)
        Catch ex As Exception
            Notifier.Error(Me, "No se pudo cargar la salida seleccionada para edición: " & ex.Message)
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        Finally
            _cargandoEdicion = False
        End Try
    End Sub

    Private Sub CargarOpcionesAutorizacion()
        Dim opciones = New List(Of Object) From {
            New With {.Text = "(seleccione tipo de autorización)", .Value = ""},
            New With {.Text = "Resolución de un Juez", .Value = CodigoAutorizacionResolucionJuez},
            New With {.Text = "Acta", .Value = CodigoAutorizacionActa}
        }

        CboAutorizacion.DisplayMember = "Text"
        CboAutorizacion.ValueMember = "Value"
        CboAutorizacion.DataSource = opciones
        CboAutorizacion.SelectedIndex = 0
    End Sub

    ' --- Eventos de Botones ---

    Private Sub BtnBuscarRecluso_Click(sender As Object, e As EventArgs) Handles BtnBuscarRecluso.Click
        Using f As New frmBuscadorReclusos()
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
                TxtRecluso.Text = recluso.NombreCompleto
                LblIdRecluso.Text = "ID: " & recluso.IdRecluso
            End Using
        End Using
    End Sub

    Private Sub BtnAtras_Click(sender As Object, e As EventArgs) Handles BtnAtras.Click
        If _pasoActual <= 0 Then Return
        _pasoActual -= 1
        ActualizarPaso()
    End Sub

    Private Sub BtnSiguiente_Click(sender As Object, e As EventArgs) Handles BtnSiguiente.Click
        If Not ValidarPaso(_pasoActual) Then Return

        If _pasoActual < Tabs.TabPages.Count - 1 Then
            _pasoActual += 1
            ActualizarPaso()
        End If
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        Me.Close()
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        If Not ValidarTodo() Then Return

        Try
            Using uow As New UnitOfWork()
                Dim repo = uow.Repository(Of Tra_SalidasLaborales)()
                Dim entidad As Tra_SalidasLaborales = Nothing

                If _idSalidaEditando.HasValue Then
                    entidad = repo.GetQueryable(tracking:=True).FirstOrDefault(Function(s) s.IdSalida = _idSalidaEditando.Value)
                    If entidad Is Nothing Then
                        Notifier.Warn(Me, "La salida seleccionada ya no existe.")
                        Return
                    End If
                Else
                    entidad = New Tra_SalidasLaborales()
                    repo.Add(entidad)
                End If

                entidad.IdRecluso = _idReclusoSeleccionado.Value
                entidad.LugarTrabajo = TxtLugarTrabajo.Text.Trim()
                entidad.Horario = TxtHorario.Text.Trim()
                entidad.DetalleCustodia = TxtCustodia.Text.Trim()
                entidad.FechaInicio = DtpInicio.Value.Date
                entidad.FechaVencimiento = DtpVencimiento.Value.Date
                entidad.FechaNotificacionJuez = If(DtpNotificacion.Checked, CType(DtpNotificacion.Value.Date, Nullable(Of Date)), Nothing)
                entidad.Activo = ChkActivo.Checked
                entidad.Observaciones = ConstruirObservacionesConAutorizacion(ObtenerCodigoAutorizacionSeleccionado(), TxtObservaciones.Text)
                If Not _idSalidaEditando.HasValue Then
                    entidad.IdDocumentoRespaldo = Nothing
                End If

                uow.Commit()
            End Using

            Notifier.Success(Me, If(_idSalidaEditando.HasValue, "Registro actualizado correctamente.", "Registro creado correctamente."))
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            Notifier.Error(Me, "No se pudo guardar el registro: " & ex.Message)
        End Try
    End Sub

    ' --- Lógica Privada y Validaciones ---

    Private Sub ActualizarPaso()
        Tabs.SelectedIndex = _pasoActual
        LblPaso.Text = $"Paso {_pasoActual + 1} de {Tabs.TabPages.Count}"

        BtnAtras.Enabled = (_pasoActual > 0)
        BtnSiguiente.Visible = (_pasoActual < Tabs.TabPages.Count - 1)
        BtnGuardar.Visible = (_pasoActual = Tabs.TabPages.Count - 1)

        If _pasoActual = Tabs.TabPages.Count - 1 Then
            ConstruirResumen()
        End If
    End Sub

    Private Function ValidarPaso(indicePaso As Integer) As Boolean
        Select Case indicePaso
            Case 0
                If Not _idReclusoSeleccionado.HasValue Then
                    Notifier.Warn(Me, "Debe seleccionar una persona privada de libertad.")
                    Return False
                End If
            Case 1
                Dim codigo = ObtenerCodigoAutorizacionSeleccionado()
                If String.IsNullOrWhiteSpace(codigo) Then
                    Notifier.Warn(Me, "Indique la autorización de salida (Resolución de un Juez o Acta).")
                    CboAutorizacion.Focus()
                    Return False
                End If

                If DtpVencimiento.Value.Date < DtpInicio.Value.Date Then
                    Notifier.Warn(Me, "La fecha de vencimiento no puede ser menor a la fecha de inicio.")
                    Return False
                End If

                If DtpNotificacion.Checked AndAlso DtpNotificacion.Value.Date < DtpInicio.Value.Date Then
                    Notifier.Warn(Me, "La fecha de notificación al juez no puede ser menor al inicio.")
                    Return False
                End If
            Case 2
                If String.IsNullOrWhiteSpace(TxtLugarTrabajo.Text) Then
                    Notifier.Warn(Me, "Ingrese el lugar de trabajo.")
                    Return False
                End If

                If String.IsNullOrWhiteSpace(TxtHorario.Text) Then
                    Notifier.Warn(Me, "Ingrese el horario autorizado.")
                    Return False
                End If

                If String.IsNullOrWhiteSpace(TxtCustodia.Text) Then
                    Notifier.Warn(Me, "Ingrese el detalle de custodia (obligatorio por decreto).")
                    Return False
                End If
        End Select

        Return True
    End Function

    Private Function ValidarTodo() As Boolean
        For i = 0 To Tabs.TabPages.Count - 2
            If Not ValidarPaso(i) Then
                _pasoActual = i
                ActualizarPaso()
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub ConstruirResumen()
        Dim lineas As New List(Of String) From {
            "RECLUSO",
            $"- {TxtRecluso.Text.Trim()} ({LblIdRecluso.Text.Replace("ID:", "").Trim()})",
            "",
            "AUTORIZACIÓN Y VIGENCIA",
            $"- Tipo: {ObtenerDescripcionAutorizacion(ObtenerCodigoAutorizacionSeleccionado())}",
            $"- Inicio: {DtpInicio.Value:dd/MM/yyyy}",
            $"- Vencimiento: {DtpVencimiento.Value:dd/MM/yyyy}",
            $"- Notificación al juez: {If(DtpNotificacion.Checked, DtpNotificacion.Value.ToString("dd/MM/yyyy"), "No informada")}",
            "",
            "DETALLE DE SALIDA",
            $"- Lugar de trabajo: {TxtLugarTrabajo.Text.Trim()}",
            $"- Horario: {TxtHorario.Text.Trim()}",
            $"- Custodia: {TxtCustodia.Text.Trim()}",
            $"- Activo: {If(ChkActivo.Checked, "Sí", "No")}",
            $"- Observaciones: {If(String.IsNullOrWhiteSpace(TxtObservaciones.Text), "(sin observaciones)", TxtObservaciones.Text.Trim())}"
        }

        TxtResumen.Text = String.Join(Environment.NewLine, lineas)
    End Sub

    Private Function ObtenerCodigoAutorizacionSeleccionado() As String
        Return If(CboAutorizacion.SelectedValue, "").ToString().Trim().ToUpperInvariant()
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

    Private Async Function CargarDatosEdicionAsync(idSalida As Integer) As Task
        Using uow As New UnitOfWork()
            Dim repo = uow.Repository(Of Tra_SalidasLaborales)()

            Dim salida = Await repo.GetQueryable() _
                .Include("Mae_Reclusos") _
                .FirstOrDefaultAsync(Function(s) s.IdSalida = idSalida)

            If salida Is Nothing Then
                Throw New InvalidOperationException("La salida no existe.")
            End If

            _idReclusoSeleccionado = salida.IdRecluso
            TxtRecluso.Text = If(salida.Mae_Reclusos IsNot Nothing, salida.Mae_Reclusos.NombreCompleto, "")
            LblIdRecluso.Text = "ID: " & salida.IdRecluso
            BtnBuscarRecluso.Enabled = False

            Dim datosObservaciones = ParsearObservacionesConAutorizacion(salida.Observaciones)
            SeleccionarAutorizacionEnCombo(datosObservaciones.CodigoAutorizacion)

            DtpInicio.Value = salida.FechaInicio.Date
            DtpVencimiento.Value = salida.FechaVencimiento.Date

            If salida.FechaNotificacionJuez.HasValue Then
                DtpNotificacion.Checked = True
                DtpNotificacion.Value = salida.FechaNotificacionJuez.Value.Date
            Else
                DtpNotificacion.Checked = False
                DtpNotificacion.Value = Date.Today
            End If

            TxtLugarTrabajo.Text = If(salida.LugarTrabajo, "")
            TxtHorario.Text = If(salida.Horario, "")
            TxtCustodia.Text = If(salida.DetalleCustodia, "")
            ChkActivo.Checked = (salida.Activo.HasValue AndAlso salida.Activo.Value)
            TxtObservaciones.Text = datosObservaciones.ObservacionesUsuario
        End Using
    End Function

    Private Sub SeleccionarAutorizacionEnCombo(codigo As String)
        Dim codigoNormalizado = If(codigo, "").Trim().ToUpperInvariant()
        If String.IsNullOrWhiteSpace(codigoNormalizado) Then
            CboAutorizacion.SelectedIndex = 0
            Return
        End If

        CboAutorizacion.SelectedValue = codigoNormalizado
        If If(CboAutorizacion.SelectedValue, "").ToString().Trim().ToUpperInvariant() <> codigoNormalizado Then
            CboAutorizacion.SelectedIndex = 0
        End If
    End Sub

    Private Function ParsearObservacionesConAutorizacion(observaciones As String) As ObservacionesSalidaDto
        Dim resultado As New ObservacionesSalidaDto With {
            .CodigoAutorizacion = "",
            .ObservacionesUsuario = ""
        }
        Dim observacionesLimpias = If(observaciones, "")

        If String.IsNullOrWhiteSpace(observacionesLimpias) Then
            Return resultado
        End If

        Dim lineas = observacionesLimpias.Split({Environment.NewLine}, StringSplitOptions.None).ToList()
        If lineas.Count > 0 Then
            Dim primeraLinea = lineas(0).Trim()

            If primeraLinea.StartsWith(PrefijoAutorizacionObservaciones, StringComparison.OrdinalIgnoreCase) Then
                resultado.CodigoAutorizacion = primeraLinea.Substring(PrefijoAutorizacionObservaciones.Length).Trim().ToUpperInvariant()
                lineas.RemoveAt(0)
            ElseIf primeraLinea.StartsWith("[AUTORIZACION]", StringComparison.OrdinalIgnoreCase) Then
                resultado.CodigoAutorizacion = primeraLinea.Substring("[AUTORIZACION]".Length).Trim().ToUpperInvariant()
                If resultado.CodigoAutorizacion = "RES_JUEZ" Then
                    resultado.CodigoAutorizacion = CodigoAutorizacionResolucionJuez
                End If
                lineas.RemoveAt(0)
            End If
        End If

        resultado.ObservacionesUsuario = String.Join(Environment.NewLine, lineas).Trim()
        Return resultado
    End Function
End Class
