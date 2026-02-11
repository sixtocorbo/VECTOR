Imports System.ComponentModel
Imports System.Threading.Tasks

Public Class frmDashboard
    Inherits Form

    ' =============================================================================
    ' 1. CONFIGURACIÓN VISUAL Y RESPONSIVA (Motor Visual Apex)
    ' =============================================================================
    Private _navExpandedWidth As Integer = 330          ' Ancho expandido (con texto)
    Private _navCollapsedWidth As Integer = 64          ' Ancho colapsado (solo iconos)
    Private _navRatio As Double = 0.18                  ' % de pantalla ideal
    Private _contentMinWidth As Integer = 900           ' Mínimo ancho para el contenido
    Private _navIsCollapsed As Boolean = False          ' Estado actual
    Private ReadOnly _tip As New ToolTip()

    ' ---- Variables de Animación ----
    Private _hoverExpanded As Boolean = False           ' Indica si estamos en modo "espiar" (Peek)
    Private WithEvents _navAnimTimer As New Timer()     ' Timer para suavizar el ancho
    Private WithEvents _hoverOutTimer As New Timer()    ' Timer para detectar salida del mouse
    Private _animFrom As Integer
    Private _animTo As Integer
    Private _animStartTicks As Long
    Private _animDurationMs As Integer = 160            ' Velocidad en ms

    ' =============================================================================
    ' 2. GESTIÓN DE NAVEGACIÓN
    ' =============================================================================
    ' Diccionario para no abrir el mismo formulario dos veces (Singleton)
    Private ReadOnly _formularios As New Dictionary(Of String, Form)

    ' Pila para poder volver atrás (Breadcrumbs lógico)
    Private ReadOnly _navStack As New Stack(Of Form)()

    ' Referencias de estado
    Private _currentBtn As Button
    Private _activeForm As Form
    Private _navBusy As Boolean = False

    ' Mapa de foco: ¿Qué control seleccionar al abrir cada formulario?
    Private ReadOnly _controlFocoPreferido As New Dictionary(Of Type, String()) From {
        {GetType(frmBuscadorReclusos), New String() {"txtBuscar", "txtBusqueda", "txtFiltro"}},
        {GetType(frmMesaEntrada), New String() {"txtBuscar", "btnNuevo"}},
        {GetType(frmUsuarios), New String() {"txtBuscar", "txtNombre"}},
        {GetType(frmGestionRangos), New String() {"txtBuscar"}},
        {GetType(frmConfiguracionSistema), New String() {"txtRuta", "txtParametro"}},
        {GetType(frmBandeja), New String() {"dgvBandeja", "txtFiltro"}}
    }

    Public Sub New()
        InitializeComponent()

        ' Configuración de Timers
        _navAnimTimer.Interval = 15
        _hoverOutTimer.Interval = 220

        ' --- VINCULACIÓN DE EVENTOS CLICK ---
        ' Operativa
        AddHandler btnBandeja.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnReclusos.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnEstadisticas.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnAuditoria.Click, AddressOf AbrirFormularioDesdeMenu_Click

        ' Administración
        AddHandler btnUsuarios.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnRangos.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnUnificar.Click, AddressOf AbrirFormularioDesdeMenu_Click

        ' Sistema
        AddHandler btnConfiguracion.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnTiposDoc.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnTiempos.Click, AddressOf AbrirFormularioDesdeMenu_Click

        ' Salir
        AddHandler btnSalir.Click, AddressOf btnSalir_Click
    End Sub

    ' =============================================================================
    ' 3. CARGA INICIAL
    ' =============================================================================
    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)

        ' Configuración UI
        ConfigurarEstilosIniciales()
        AplicarLayoutResponsivo(force:=True)

        ' Cargar datos del usuario
        ConfigurarInfoSesion()

        ' Eventos de Mouse para Animación "Peek"
        AddHandler panelNavegacion.MouseEnter, AddressOf panelNavegacion_MouseEnter
        AddHandler panelNavegacion.MouseLeave, AddressOf panelNavegacion_MouseLeave
        AddHandler panelContenido.MouseEnter, AddressOf panelContenido_MouseEnter

        ' Eventos extra
        AddHandler panelLogo.Click, AddressOf ToggleNav
        AddHandler Me.Resize, AddressOf frmDashboard_Resize
        AddHandler Me.KeyDown, AddressOf frmDashboard_KeyDown

        ' Hookear eventos a los botones para que no corten el hover
        For Each b In NavButtons()
            AddHandler b.MouseEnter, AddressOf panelNavegacion_MouseEnter
            AddHandler b.MouseLeave, AddressOf panelNavegacion_MouseLeave
        Next

        ' Abrir Inicio
        If btnBandeja IsNot Nothing Then btnBandeja.PerformClick()
    End Sub

    Private Sub ConfigurarInfoSesion()
        Try
            If lblSemanaActual IsNot Nothing Then
                lblSemanaActual.Text = SesionGlobal.NombreUsuario
                _tip.SetToolTip(lblSemanaActual, $"Oficina: {SesionGlobal.NombreOficina}")
            End If
            If lblAppName IsNot Nothing Then lblAppName.Text = "VECTOR"
        Catch
            If lblSemanaActual IsNot Nothing Then lblSemanaActual.Text = "Modo Prueba"
        End Try
    End Sub

    ' =============================================================================
    ' 4. LÓGICA DE NAVEGACIÓN (CORE)
    ' =============================================================================
    Private Sub AbrirFormularioDesdeMenu_Click(sender As Object, e As EventArgs)
        If _navBusy Then Return
        _navBusy = True
        _navStack.Clear() ' Limpiar historial

        Try
            Dim botonClickeado = CType(sender, Button)

            ' Seguridad
            If EsBotonRestringido(botonClickeado) AndAlso Not VerificarPermisoAdmin() Then
                DisableButton()
                Return
            End If

            ' UI
            ActivateButton(botonClickeado)
            panelContenido.Select()

            ' Abrir Formulario
            Dim formType As Type = ObtenerTipoDeFormulario(botonClickeado.Name)
            If formType IsNot Nothing Then
                Dim formToShow = ObtenerOcrearInstancia(formType)
                AbrirFormEnPanel(formToShow)
            End If

            ' Si estaba en modo "espiar" (hover), cerrar suavemente
            If _hoverExpanded Then CollapsePeekAnimated()

        Finally
            _navBusy = False
        End Try
    End Sub

    Private Function ObtenerTipoDeFormulario(nombreBoton As String) As Type
        Select Case nombreBoton
            Case "btnBandeja" : Return GetType(frmBandeja)
            Case "btnReclusos" : Return GetType(frmBuscadorReclusos)
            Case "btnEstadisticas" : Return GetType(frmEstadisticas)
            Case "btnAuditoria" : Return GetType(frmAuditoria)
            Case "btnUsuarios" : Return GetType(frmUsuarios)
            Case "btnRangos" : Return GetType(frmGestionRangos)
            Case "btnUnificar" : Return GetType(frmUnificarOficinas)
            Case "btnConfiguracion" : Return GetType(frmConfiguracionSistema)
            Case "btnTiposDoc" : Return GetType(frmGestionTiposDocumento)
            Case "btnTiempos" : Return GetType(frmGestionTiempos)
            Case Else : Return Nothing
        End Select
    End Function

    Private Function ObtenerOcrearInstancia(formType As Type) As Form
        Dim formName As String = formType.Name
        If _formularios.ContainsKey(formName) Then
            Dim frm = _formularios(formName)
            If frm IsNot Nothing AndAlso Not frm.IsDisposed Then Return frm
        End If
        Dim newForm = CType(Activator.CreateInstance(formType), Form)
        _formularios(formName) = newForm
        Return newForm
    End Function

    Public Sub AbrirFormEnPanel(formToShow As Form, Optional isChild As Boolean = False)
        If formToShow Is Nothing OrElse formToShow.IsDisposed Then Return

        ' Ocultar/Cerrar anterior
        If _activeForm IsNot Nothing Then
            If isChild Then
                _navStack.Push(_activeForm)
                _activeForm.Hide()
            Else
                If Not _formularios.ContainsValue(_activeForm) Then
                    _activeForm.Close()
                Else
                    _activeForm.Hide()
                End If
            End If
        End If

        _activeForm = formToShow

        ' Embeber
        If Not panelContenido.Controls.Contains(formToShow) Then
            formToShow.TopLevel = False
            formToShow.FormBorderStyle = FormBorderStyle.None
            formToShow.Dock = DockStyle.Fill
            panelContenido.Controls.Add(formToShow)

            ' Handler al cerrar (para volver atrás)
            AddHandler formToShow.FormClosed, Sub(s, args)
                                                  panelContenido.Controls.Remove(CType(s, Form))
                                                  If _navStack.Count > 0 Then
                                                      Dim volver = _navStack.Pop()
                                                      If volver IsNot Nothing AndAlso Not volver.IsDisposed Then
                                                          _activeForm = volver
                                                          _activeForm.Show()
                                                          ActualizarTituloDashboard()
                                                          EnfocarControlPreferido(volver)
                                                          Return
                                                      End If
                                                  End If
                                                  ' Fallback
                                                  If btnBandeja IsNot Nothing Then btnBandeja.PerformClick()
                                              End Sub
        End If

        formToShow.Show()
        formToShow.BringToFront()
        EnfocarControlPreferido(formToShow)
        ActualizarTituloDashboard()
    End Sub

    ' =============================================================================
    ' 5. ANIMACIÓN Y COMPORTAMIENTO VISUAL (Apex Logic)
    ' =============================================================================

    Private Sub ToggleNav(sender As Object, e As EventArgs)
        SetNavCollapsedState(Not _navIsCollapsed)
    End Sub

    Private Sub SetNavCollapsedState(collapse As Boolean, Optional manual As Boolean = False, Optional applyWidthNow As Boolean = True)
        _navIsCollapsed = collapse

        For Each b In NavButtons()
            If b.Tag Is Nothing Then b.Tag = b.Text ' Backup del texto

            If _navIsCollapsed Then
                b.Text = ExtractEmoji(CStr(b.Tag))
                b.TextAlign = ContentAlignment.MiddleCenter
                b.Padding = New Padding(0)
                _tip.SetToolTip(b, CStr(b.Tag))
            Else
                b.Text = CStr(b.Tag)
                b.TextAlign = ContentAlignment.MiddleLeft
                b.Padding = New Padding(18, 0, 0, 0)
                _tip.SetToolTip(b, Nothing)
            End If
        Next

        ' Labels
        If lblAppName IsNot Nothing Then lblAppName.Visible = Not _navIsCollapsed
        If lblSemanaActual IsNot Nothing Then lblSemanaActual.Visible = Not _navIsCollapsed

        ' Ajustar altura del panel logo
        panelLogo.Height = If(_navIsCollapsed, 70, 128)

        If applyWidthNow Then
            AnimateNavWidth(If(_navIsCollapsed, _navCollapsedWidth, _navExpandedWidth))
        End If
    End Sub

    ' --- Animación Timer ---
    Private Sub AnimateNavWidth(targetWidth As Integer)
        _animFrom = panelNavegacion.Width
        _animTo = targetWidth
        _animStartTicks = Environment.TickCount
        _navAnimTimer.Start()
    End Sub

    Private Sub NavAnimTimer_Tick(sender As Object, e As EventArgs) Handles _navAnimTimer.Tick
        Dim elapsed As Double = Environment.TickCount - _animStartTicks
        Dim t As Double = Math.Min(1.0, elapsed / Math.Max(1.0, _animDurationMs))
        Dim ease As Double = 1 - Math.Pow(1 - t, 3) ' Ease-Out Cubic
        Dim w As Integer = CInt(_animFrom + ((_animTo - _animFrom) * ease))
        panelNavegacion.Width = w
        If t >= 1.0 Then _navAnimTimer.Stop()
    End Sub

    ' --- Peek (Hover) Logic ---
    Private Sub panelNavegacion_MouseEnter(sender As Object, e As EventArgs)
        ' Solo expandir si está colapsado Y no está ya expandido por hover
        If _navIsCollapsed AndAlso Not _hoverExpanded Then
            ' Truco: Expandimos visualmente pero lógicamente sigue colapsado (_navIsCollapsed = True)
            _hoverExpanded = True
            SetNavCollapsedState(False, manual:=False, applyWidthNow:=False)
            AnimateNavWidth(_navExpandedWidth)
        End If
        _hoverOutTimer.Stop()
    End Sub

    Private Sub panelNavegacion_MouseLeave(sender As Object, e As EventArgs)
        If _hoverExpanded Then _hoverOutTimer.Start()
    End Sub

    Private Sub HoverOutTimer_Tick(sender As Object, e As EventArgs) Handles _hoverOutTimer.Tick
        If Not _hoverExpanded Then
            _hoverOutTimer.Stop()
            Return
        End If

        ' Verificar coordenadas reales
        Dim r As New Rectangle(panelNavegacion.PointToScreen(Point.Empty), panelNavegacion.Size)
        If Not r.Contains(Cursor.Position) Then
            _hoverOutTimer.Stop()
            CollapsePeekAnimated()
        End If
    End Sub

    Private Sub panelContenido_MouseEnter(sender As Object, e As EventArgs)
        If _hoverExpanded Then CollapsePeekAnimated()
    End Sub

    Private Sub CollapsePeekAnimated()
        _hoverExpanded = False
        ' Volver a estado visual colapsado
        SetNavCollapsedState(True, manual:=False, applyWidthNow:=False)
        AnimateNavWidth(_navCollapsedWidth)
    End Sub

    ' =============================================================================
    ' 6. HELPERS Y SEGURIDAD
    ' =============================================================================

    Private Function ExtractEmoji(fullText As String) As String
        If String.IsNullOrWhiteSpace(fullText) Then Return "?"
        Return fullText.Trim().Split(" "c)(0)
    End Function

    Private Iterator Function NavButtons() As IEnumerable(Of Button)
        For Each c In panelNavegacion.Controls.OfType(Of Button)()
            Yield c
        Next
    End Function

    Private Function EsBotonRestringido(btn As Button) As Boolean
        Dim restringidos As String() = {"btnUsuarios", "btnUnificar", "btnRangos"}
        Return restringidos.Contains(btn.Name)
    End Function

    Private Function VerificarPermisoAdmin() As Boolean
        If Not SesionGlobal.EsAdmin Then
            Toast.Show(Me, "Acceso denegado. Se requiere Administrador.", ToastType.Warning)
            Return False
        End If
        Return True
    End Function

    Private Sub ActivateButton(btn As Button)
        DisableButton()
        _currentBtn = btn
        _currentBtn.BackColor = Color.FromArgb(81, 81, 112)
        _currentBtn.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)
    End Sub

    Private Sub DisableButton()
        If _currentBtn IsNot Nothing Then
            _currentBtn.BackColor = AppTheme.Palette.NavBackground
            _currentBtn.Font = AppTheme.Palette.NavFont
        End If
    End Sub

    Private Sub ActualizarTituloDashboard()
        Dim titulo As String = "VECTOR - Sistema de Gestión"
        If _activeForm IsNot Nothing Then titulo &= $" | {_activeForm.Text}"
        Me.Text = titulo
    End Sub

    Private Sub EnfocarControlPreferido(form As Form)
        Dim preferidos As String() = Nothing
        If _controlFocoPreferido.TryGetValue(form.GetType(), preferidos) Then
            form.BeginInvoke(Sub()
                                 For Each nombre In preferidos
                                     Dim c = form.Controls.Find(nombre, True).FirstOrDefault()
                                     If c IsNot Nothing AndAlso c.Visible AndAlso c.Enabled Then
                                         c.Select()
                                         Exit Sub
                                     End If
                                 Next
                             End Sub)
        End If
    End Sub

    Private Sub AplicarLayoutResponsivo(Optional force As Boolean = False)
        Dim ancho = Me.ClientSize.Width
        If ancho < 1000 AndAlso Not _navIsCollapsed Then
            SetNavCollapsedState(True)
        End If
    End Sub

    Private Sub frmDashboard_Resize(sender As Object, e As EventArgs)
        AplicarLayoutResponsivo()
    End Sub

    Private Sub frmDashboard_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.B Then ToggleNav(Nothing, Nothing)
    End Sub

    ' =============================================================================
    ' 7. ESTILOS Y SALIDA
    ' =============================================================================
    Private Sub ConfigurarEstilosIniciales()
        panelNavegacion.BackColor = AppTheme.Palette.NavBackground
        panelLogo.BackColor = AppTheme.Palette.NavBackgroundAlt

        For Each b In NavButtons()
            b.ForeColor = AppTheme.Palette.NavForeground
            b.TextAlign = ContentAlignment.MiddleLeft
            b.Padding = New Padding(18, 0, 0, 0)
            b.FlatAppearance.BorderSize = 0
            b.FlatStyle = FlatStyle.Flat
            If b.Tag Is Nothing Then b.Tag = b.Text
        Next
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs)
        If MessageBox.Show("¿Desea salir del sistema?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            AuditoriaSistema.RegistrarEvento("Salida del sistema desde menú.", "SISTEMA")
            Application.Exit()
        End If
    End Sub

    Private Sub frmDashboard_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            If MessageBox.Show("¿Seguro que desea cerrar la aplicación VECTOR?", "Cerrar Sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                e.Cancel = True
            Else
                AuditoriaSistema.RegistrarEvento("Cierre de aplicación.", "SISTEMA")
            End If
        End If
    End Sub

End Class