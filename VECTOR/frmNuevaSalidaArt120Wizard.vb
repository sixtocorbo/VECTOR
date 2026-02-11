Imports System.Linq

Public Class frmNuevaSalidaArt120Wizard
    Inherits Form

    Private Const CodigoAutorizacionResolucionJuez As String = "RES_JUEZ"
    Private Const CodigoAutorizacionActa As String = "ACTA"
    Private Const PrefijoAutorizacionObservaciones As String = "[AUTORIZACION] "

    Private _idReclusoSeleccionado As Integer?
    Private _pasoActual As Integer = 0

    Private ReadOnly _tabs As New TabControl()
    Private ReadOnly _lblPaso As New Label()
    Private ReadOnly _btnAtras As New Button()
    Private ReadOnly _btnSiguiente As New Button()
    Private ReadOnly _btnGuardar As New Button()
    Private ReadOnly _btnCancelar As New Button()

    Private ReadOnly _txtRecluso As New TextBox()
    Private ReadOnly _lblIdRecluso As New Label()
    Private ReadOnly _btnBuscarRecluso As New Button()

    Private ReadOnly _cboAutorizacion As New ComboBox()
    Private ReadOnly _dtpInicio As New DateTimePicker()
    Private ReadOnly _dtpVencimiento As New DateTimePicker()
    Private ReadOnly _dtpNotificacion As New DateTimePicker()

    Private ReadOnly _txtLugarTrabajo As New TextBox()
    Private ReadOnly _txtHorario As New TextBox()
    Private ReadOnly _txtCustodia As New TextBox()
    Private ReadOnly _txtObservaciones As New TextBox()
    Private ReadOnly _chkActivo As New CheckBox()

    Private ReadOnly _txtResumen As New TextBox()

    Public Sub New()
        Me.Text = "Nueva salida laboral - Art. 120"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowInTaskbar = False
        Me.Width = 760
        Me.Height = 560

        InicializarLayout()
        CargarOpcionesAutorizacion()
        ActualizarPaso()
    End Sub

    Private Sub InicializarLayout()
        _lblPaso.AutoSize = True
        _lblPaso.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        _lblPaso.Location = New Point(16, 16)

        _tabs.Location = New Point(16, 44)
        _tabs.Size = New Size(710, 420)
        _tabs.Appearance = TabAppearance.FlatButtons
        _tabs.ItemSize = New Size(1, 1)
        _tabs.SizeMode = TabSizeMode.Fixed

        Dim tabRecluso = New TabPage()
        Dim tabAutorizacion = New TabPage()
        Dim tabDetalle = New TabPage()
        Dim tabResumen = New TabPage()

        ConstruirPasoRecluso(tabRecluso)
        ConstruirPasoAutorizacion(tabAutorizacion)
        ConstruirPasoDetalle(tabDetalle)
        ConstruirPasoResumen(tabResumen)

        _tabs.TabPages.Add(tabRecluso)
        _tabs.TabPages.Add(tabAutorizacion)
        _tabs.TabPages.Add(tabDetalle)
        _tabs.TabPages.Add(tabResumen)

        _btnAtras.Text = "Atrás"
        _btnAtras.Size = New Size(100, 34)
        _btnAtras.Location = New Point(302, 476)
        AddHandler _btnAtras.Click, AddressOf btnAtras_Click

        _btnSiguiente.Text = "Siguiente"
        _btnSiguiente.Size = New Size(100, 34)
        _btnSiguiente.Location = New Point(410, 476)
        AddHandler _btnSiguiente.Click, AddressOf btnSiguiente_Click

        _btnGuardar.Text = "Registrar"
        _btnGuardar.Size = New Size(100, 34)
        _btnGuardar.Location = New Point(518, 476)
        _btnGuardar.BackColor = Color.ForestGreen
        _btnGuardar.ForeColor = Color.White
        _btnGuardar.FlatStyle = FlatStyle.Flat
        AddHandler _btnGuardar.Click, AddressOf btnGuardar_Click

        _btnCancelar.Text = "Cancelar"
        _btnCancelar.Size = New Size(100, 34)
        _btnCancelar.Location = New Point(626, 476)
        AddHandler _btnCancelar.Click, Sub() Me.Close()

        Controls.Add(_lblPaso)
        Controls.Add(_tabs)
        Controls.Add(_btnAtras)
        Controls.Add(_btnSiguiente)
        Controls.Add(_btnGuardar)
        Controls.Add(_btnCancelar)
    End Sub

    Private Sub ConstruirPasoRecluso(tab As TabPage)
        Dim lblTitulo = New Label With {
            .Text = "Paso 1: Persona privada de libertad",
            .Location = New Point(20, 20),
            .AutoSize = True,
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        }

        Dim lblRecluso = New Label With {
            .Text = "Recluso:",
            .Location = New Point(20, 72),
            .AutoSize = True
        }

        _txtRecluso.Location = New Point(20, 94)
        _txtRecluso.Width = 510
        _txtRecluso.ReadOnly = True

        _btnBuscarRecluso.Text = "Buscar..."
        _btnBuscarRecluso.Location = New Point(540, 92)
        _btnBuscarRecluso.Size = New Size(110, 28)
        AddHandler _btnBuscarRecluso.Click, AddressOf btnBuscarRecluso_Click

        _lblIdRecluso.Text = "ID: (ninguno)"
        _lblIdRecluso.Location = New Point(20, 132)
        _lblIdRecluso.AutoSize = True

        Dim lblAyuda = New Label With {
            .Text = "Busque y seleccione la persona para asociar la autorización.",
            .Location = New Point(20, 170),
            .AutoSize = True,
            .ForeColor = Color.DimGray
        }

        tab.Controls.Add(lblTitulo)
        tab.Controls.Add(lblRecluso)
        tab.Controls.Add(_txtRecluso)
        tab.Controls.Add(_btnBuscarRecluso)
        tab.Controls.Add(_lblIdRecluso)
        tab.Controls.Add(lblAyuda)
    End Sub

    Private Sub ConstruirPasoAutorizacion(tab As TabPage)
        Dim lblTitulo = New Label With {
            .Text = "Paso 2: Autorización y vigencia",
            .Location = New Point(20, 20),
            .AutoSize = True,
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        }

        Dim lblAutorizacion = New Label With {.Text = "Tipo de autorización:", .Location = New Point(20, 72), .AutoSize = True}
        _cboAutorizacion.Location = New Point(20, 94)
        _cboAutorizacion.Width = 420
        _cboAutorizacion.DropDownStyle = ComboBoxStyle.DropDownList

        Dim lblInicio = New Label With {.Text = "Fecha de inicio:", .Location = New Point(20, 138), .AutoSize = True}
        _dtpInicio.Location = New Point(20, 160)
        _dtpInicio.Format = DateTimePickerFormat.Short

        Dim lblVencimiento = New Label With {.Text = "Fecha de vencimiento:", .Location = New Point(200, 138), .AutoSize = True}
        _dtpVencimiento.Location = New Point(200, 160)
        _dtpVencimiento.Format = DateTimePickerFormat.Short
        _dtpVencimiento.Value = Date.Today.AddMonths(1)

        Dim lblNotificacion = New Label With {.Text = "Fecha notificación al juez (opcional):", .Location = New Point(20, 206), .AutoSize = True}
        _dtpNotificacion.Location = New Point(20, 228)
        _dtpNotificacion.Format = DateTimePickerFormat.Short
        _dtpNotificacion.ShowCheckBox = True
        _dtpNotificacion.Checked = False

        tab.Controls.Add(lblTitulo)
        tab.Controls.Add(lblAutorizacion)
        tab.Controls.Add(_cboAutorizacion)
        tab.Controls.Add(lblInicio)
        tab.Controls.Add(_dtpInicio)
        tab.Controls.Add(lblVencimiento)
        tab.Controls.Add(_dtpVencimiento)
        tab.Controls.Add(lblNotificacion)
        tab.Controls.Add(_dtpNotificacion)
    End Sub

    Private Sub ConstruirPasoDetalle(tab As TabPage)
        Dim lblTitulo = New Label With {
            .Text = "Paso 3: Detalle de salida laboral",
            .Location = New Point(20, 20),
            .AutoSize = True,
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        }

        Dim lblLugar = New Label With {.Text = "Lugar de trabajo:", .Location = New Point(20, 62), .AutoSize = True}
        _txtLugarTrabajo.Location = New Point(20, 84)
        _txtLugarTrabajo.Width = 630

        Dim lblHorario = New Label With {.Text = "Horario autorizado:", .Location = New Point(20, 124), .AutoSize = True}
        _txtHorario.Location = New Point(20, 146)
        _txtHorario.Width = 300

        Dim lblCustodia = New Label With {.Text = "Detalle de custodia:", .Location = New Point(20, 186), .AutoSize = True}
        _txtCustodia.Location = New Point(20, 208)
        _txtCustodia.Width = 630
        _txtCustodia.Height = 60
        _txtCustodia.Multiline = True

        Dim lblObs = New Label With {.Text = "Observaciones adicionales:", .Location = New Point(20, 278), .AutoSize = True}
        _txtObservaciones.Location = New Point(20, 300)
        _txtObservaciones.Width = 630
        _txtObservaciones.Height = 70
        _txtObservaciones.Multiline = True

        _chkActivo.Text = "Registro activo"
        _chkActivo.Location = New Point(20, 380)
        _chkActivo.Checked = True

        tab.Controls.Add(lblTitulo)
        tab.Controls.Add(lblLugar)
        tab.Controls.Add(_txtLugarTrabajo)
        tab.Controls.Add(lblHorario)
        tab.Controls.Add(_txtHorario)
        tab.Controls.Add(lblCustodia)
        tab.Controls.Add(_txtCustodia)
        tab.Controls.Add(lblObs)
        tab.Controls.Add(_txtObservaciones)
        tab.Controls.Add(_chkActivo)
    End Sub

    Private Sub ConstruirPasoResumen(tab As TabPage)
        Dim lblTitulo = New Label With {
            .Text = "Paso 4: Confirmación final",
            .Location = New Point(20, 20),
            .AutoSize = True,
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        }

        Dim lblAyuda = New Label With {
            .Text = "Revise los datos antes de registrar.",
            .Location = New Point(20, 52),
            .AutoSize = True,
            .ForeColor = Color.DimGray
        }

        _txtResumen.Location = New Point(20, 84)
        _txtResumen.Width = 630
        _txtResumen.Height = 300
        _txtResumen.Multiline = True
        _txtResumen.ReadOnly = True
        _txtResumen.ScrollBars = ScrollBars.Vertical

        tab.Controls.Add(lblTitulo)
        tab.Controls.Add(lblAyuda)
        tab.Controls.Add(_txtResumen)
    End Sub

    Private Sub CargarOpcionesAutorizacion()
        Dim opciones = New List(Of Object) From {
            New With {.Text = "(seleccione tipo de autorización)", .Value = ""},
            New With {.Text = "Resolución de un Juez", .Value = CodigoAutorizacionResolucionJuez},
            New With {.Text = "Acta", .Value = CodigoAutorizacionActa}
        }

        _cboAutorizacion.DisplayMember = "Text"
        _cboAutorizacion.ValueMember = "Value"
        _cboAutorizacion.DataSource = opciones
        _cboAutorizacion.SelectedIndex = 0
    End Sub

    Private Sub btnBuscarRecluso_Click(sender As Object, e As EventArgs)
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
                _txtRecluso.Text = recluso.NombreCompleto
                _lblIdRecluso.Text = "ID: " & recluso.IdRecluso
            End Using
        End Using
    End Sub

    Private Sub btnAtras_Click(sender As Object, e As EventArgs)
        If _pasoActual <= 0 Then Return
        _pasoActual -= 1
        ActualizarPaso()
    End Sub

    Private Sub btnSiguiente_Click(sender As Object, e As EventArgs)
        If Not ValidarPaso(_pasoActual) Then Return

        If _pasoActual < _tabs.TabPages.Count - 1 Then
            _pasoActual += 1
            ActualizarPaso()
        End If
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs)
        If Not ValidarTodo() Then Return

        Try
            Using uow As New UnitOfWork()
                Dim repo = uow.Repository(Of Tra_SalidasLaborales)()
                Dim entidad As New Tra_SalidasLaborales()

                entidad.IdRecluso = _idReclusoSeleccionado.Value
                entidad.LugarTrabajo = _txtLugarTrabajo.Text.Trim()
                entidad.Horario = _txtHorario.Text.Trim()
                entidad.DetalleCustodia = _txtCustodia.Text.Trim()
                entidad.FechaInicio = _dtpInicio.Value.Date
                entidad.FechaVencimiento = _dtpVencimiento.Value.Date
                entidad.FechaNotificacionJuez = If(_dtpNotificacion.Checked, CType(_dtpNotificacion.Value.Date, Nullable(Of Date)), Nothing)
                entidad.Activo = _chkActivo.Checked
                entidad.Observaciones = ConstruirObservacionesConAutorizacion(ObtenerCodigoAutorizacionSeleccionado(), _txtObservaciones.Text)
                entidad.IdDocumentoRespaldo = Nothing

                repo.Add(entidad)
                uow.Commit()
            End Using

            Notifier.Success(Me, "Registro creado correctamente.")
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            Notifier.[Error](Me, "No se pudo guardar el registro: " & ex.Message)
        End Try
    End Sub

    Private Sub ActualizarPaso()
        _tabs.SelectedIndex = _pasoActual
        _lblPaso.Text = $"Paso {_pasoActual + 1} de {_tabs.TabPages.Count}"

        _btnAtras.Enabled = (_pasoActual > 0)
        _btnSiguiente.Visible = (_pasoActual < _tabs.TabPages.Count - 1)
        _btnGuardar.Visible = (_pasoActual = _tabs.TabPages.Count - 1)

        If _pasoActual = _tabs.TabPages.Count - 1 Then
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
                    _cboAutorizacion.Focus()
                    Return False
                End If

                If _dtpVencimiento.Value.Date < _dtpInicio.Value.Date Then
                    Notifier.Warn(Me, "La fecha de vencimiento no puede ser menor a la fecha de inicio.")
                    Return False
                End If

                If _dtpNotificacion.Checked AndAlso _dtpNotificacion.Value.Date < _dtpInicio.Value.Date Then
                    Notifier.Warn(Me, "La fecha de notificación al juez no puede ser menor al inicio.")
                    Return False
                End If
            Case 2
                If String.IsNullOrWhiteSpace(_txtLugarTrabajo.Text) Then
                    Notifier.Warn(Me, "Ingrese el lugar de trabajo.")
                    Return False
                End If

                If String.IsNullOrWhiteSpace(_txtHorario.Text) Then
                    Notifier.Warn(Me, "Ingrese el horario autorizado.")
                    Return False
                End If

                If String.IsNullOrWhiteSpace(_txtCustodia.Text) Then
                    Notifier.Warn(Me, "Ingrese el detalle de custodia (obligatorio por decreto).")
                    Return False
                End If
        End Select

        Return True
    End Function

    Private Function ValidarTodo() As Boolean
        For i = 0 To _tabs.TabPages.Count - 2
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
            $"- {_txtRecluso.Text.Trim()} ({_lblIdRecluso.Text.Replace("ID:", "").Trim()})",
            "",
            "AUTORIZACIÓN Y VIGENCIA",
            $"- Tipo: {ObtenerDescripcionAutorizacion(ObtenerCodigoAutorizacionSeleccionado())}",
            $"- Inicio: {_dtpInicio.Value:dd/MM/yyyy}",
            $"- Vencimiento: {_dtpVencimiento.Value:dd/MM/yyyy}",
            $"- Notificación al juez: {If(_dtpNotificacion.Checked, _dtpNotificacion.Value.ToString("dd/MM/yyyy"), "No informada")}",
            "",
            "DETALLE DE SALIDA",
            $"- Lugar de trabajo: {_txtLugarTrabajo.Text.Trim()}",
            $"- Horario: {_txtHorario.Text.Trim()}",
            $"- Custodia: {_txtCustodia.Text.Trim()}",
            $"- Activo: {If(_chkActivo.Checked, "Sí", "No")}",
            $"- Observaciones: {If(String.IsNullOrWhiteSpace(_txtObservaciones.Text), "(sin observaciones)", _txtObservaciones.Text.Trim())}"
        }

        _txtResumen.Text = String.Join(Environment.NewLine, lineas)
    End Sub

    Private Function ObtenerCodigoAutorizacionSeleccionado() As String
        Return If(_cboAutorizacion.SelectedValue, "").ToString().Trim().ToUpperInvariant()
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
