Imports System.Linq

Public Class frmNuevaSalidaArt120Wizard

    ' Definimos constantes
    Private Const CodigoAutorizacionResolucionJuez As String = "RES_JUEZ"
    Private Const CodigoAutorizacionActa As String = "ACTA"
    Private Const PrefijoAutorizacionObservaciones As String = "[AUTORIZACION] "

    Private _idReclusoSeleccionado As Integer?
    Private _pasoActual As Integer = 0

    Public Sub New()
        ' Esta llamada es obligatoria para inicializar los controles definidos en el Diseñador
        InitializeComponent()

        ' Configuraciones iniciales
        CargarOpcionesAutorizacion()
        ActualizarPaso()

        ' Inicializamos vencimiento por defecto
        DtpVencimiento.Value = Date.Today.AddMonths(1)
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
                Dim entidad As New Tra_SalidasLaborales()

                entidad.IdRecluso = _idReclusoSeleccionado.Value
                entidad.LugarTrabajo = TxtLugarTrabajo.Text.Trim()
                entidad.Horario = TxtHorario.Text.Trim()
                entidad.DetalleCustodia = TxtCustodia.Text.Trim()
                entidad.FechaInicio = DtpInicio.Value.Date
                entidad.FechaVencimiento = DtpVencimiento.Value.Date
                entidad.FechaNotificacionJuez = If(DtpNotificacion.Checked, CType(DtpNotificacion.Value.Date, Nullable(Of Date)), Nothing)
                entidad.Activo = ChkActivo.Checked
                entidad.Observaciones = ConstruirObservacionesConAutorizacion(ObtenerCodigoAutorizacionSeleccionado(), TxtObservaciones.Text)
                entidad.IdDocumentoRespaldo = Nothing

                repo.Add(entidad)
                uow.Commit()
            End Using

            Notifier.Success(Me, "Registro creado correctamente.")
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
End Class