Imports System.Runtime.InteropServices
Imports System.Runtime.CompilerServices

Public Module AppTheme

    Public NotInheritable Class Palette
        Private Sub New()
        End Sub

        Public Shared ReadOnly Primary As Color = Color.FromArgb(0, 123, 255)
        Public Shared ReadOnly PrimaryLight As Color = Color.FromArgb(220, 235, 255)
        Public Shared ReadOnly Background As Color = Color.FromArgb(248, 249, 250)
        Public Shared ReadOnly Foreground As Color = Color.FromArgb(33, 37, 41)
        Public Shared ReadOnly Border As Color = Color.FromArgb(222, 226, 230)

        Public Shared ReadOnly NavBackground As Color = Color.FromArgb(51, 51, 76)
        Public Shared ReadOnly NavBackgroundAlt As Color = Color.FromArgb(39, 39, 58)
        Public Shared ReadOnly NavForeground As Color = Color.Gainsboro

        Public Shared ReadOnly BaseFont As New Font("Segoe UI", 9.0F, FontStyle.Regular)
        Public Shared ReadOnly TitleFont As New Font("Segoe UI", 10.0F, FontStyle.Bold)
        Public Shared ReadOnly NavFont As New Font("Segoe UI", 11.25F, FontStyle.Regular)
    End Class

    Public Sub Aplicar(form As Form)
        Apply(form)
    End Sub

    Public Sub Apply(form As Form)
        form.BackColor = Palette.Background
        form.Font = Palette.BaseFont
        form.ForeColor = Palette.Foreground

        HookControlAdded(form)
        ApplyRecursive(form.Controls)
    End Sub

    Private Sub ApplyRecursive(controls As Control.ControlCollection)
        For Each ctrl As Control In controls
            StyleControl(ctrl)
            HookControlAdded(ctrl)

            If ctrl.HasChildren Then
                ApplyRecursive(ctrl.Controls)
            End If
        Next
    End Sub

    Private Sub HookControlAdded(ctrl As Control)
        If ctrl Is Nothing Then Return

        RemoveHandler ctrl.ControlAdded, AddressOf OnControlAdded
        AddHandler ctrl.ControlAdded, AddressOf OnControlAdded
    End Sub

    Private Sub OnControlAdded(sender As Object, e As ControlEventArgs)
        Dim added = e.Control
        If added Is Nothing Then Return

        StyleControl(added)
        HookControlAdded(added)

        If added.HasChildren Then
            ApplyRecursive(added.Controls)
        End If
    End Sub

    Private Sub StyleControl(ctrl As Control)
        If TypeOf ctrl Is Panel Then
            StylePanel(DirectCast(ctrl, Panel))

        ElseIf TypeOf ctrl Is Button Then
            StyleButton(DirectCast(ctrl, Button))

        ElseIf TypeOf ctrl Is DataGridView Then
            StyleDataGridView(DirectCast(ctrl, DataGridView))

        ElseIf TypeOf ctrl Is Label Then
            Dim nameLower = ctrl.Name.ToLower()
            If nameLower.Contains("header") OrElse nameLower.Contains("titulo") Then
                ctrl.Font = Palette.TitleFont
            End If

        ElseIf TypeOf ctrl Is GroupBox Then
            ctrl.Font = Palette.TitleFont
        End If
    End Sub

    Private Sub StylePanel(pnl As Panel)
        Dim nameLower = pnl.Name.ToLower()
        If nameLower.Contains("panelnavegacion") OrElse nameLower.Contains("sidebar") Then
            pnl.BackColor = Palette.NavBackground
        ElseIf nameLower.Contains("panellogo") Then
            pnl.BackColor = Palette.NavBackgroundAlt
        ElseIf nameLower.Contains("filtros") Then
            pnl.BackColor = Color.WhiteSmoke
        Else
            pnl.BackColor = Palette.Background
        End If
    End Sub

    Private ReadOnly StyledControls As New ConditionalWeakTable(Of Control, Object)()
    Private ReadOnly StyledMarkerValue As New Object()

    Private Function IsStyled(ctrl As Control) As Boolean
        Dim dummy As Object = Nothing
        Return StyledControls.TryGetValue(ctrl, dummy)
    End Function

    Private Sub MarkStyled(ctrl As Control)
        If Not IsStyled(ctrl) Then
            StyledControls.Add(ctrl, StyledMarkerValue)
        End If
    End Sub

    Private Sub StyleButton(btn As Button)
        Dim tagStr As String = If(btn.Tag, "").ToString()
        Dim parentIsNav As Boolean = (btn.Parent IsNot Nothing AndAlso
                                      (btn.Parent.Name.ToLower().Contains("panelnavegacion") OrElse
                                       btn.Parent.Name.ToLower().Contains("sidebar")))
        Dim isNav As Boolean = parentIsNav OrElse tagStr.IndexOf("Nav", StringComparison.OrdinalIgnoreCase) >= 0
        Dim keepBack As Boolean = tagStr.IndexOf("KeepBackColor", StringComparison.OrdinalIgnoreCase) >= 0
        Dim isTile As Boolean = IsTileButton(btn, tagStr)

        btn.FlatStyle = FlatStyle.Flat
        btn.UseVisualStyleBackColor = False
        btn.Cursor = Cursors.Hand

        If isNav Then
            ApplyNavButtonStyle(btn)
            btn.FlatAppearance.BorderSize = 0
            MarkStyled(btn)
            Return
        End If

        If keepBack Then
            SyncHoverBackToCurrent(btn)
            SetupReactiveBorder(btn, 1, 0.25F, 0.45F, True)
            Return
        End If

        If isTile Then
            ApplyTileButtonStyle(btn)
            Return
        End If

        ApplyPrimaryButtonStyle(btn)
    End Sub

    Private Sub ApplyNavButtonStyle(btn As Button)
        btn.BackColor = Palette.NavBackground
        btn.ForeColor = Palette.NavForeground
        If btn.Font Is Nothing OrElse btn.Font.Size < Palette.NavFont.Size Then
            btn.Font = Palette.NavFont
        End If
        btn.TextAlign = ContentAlignment.MiddleLeft
        If btn.Padding.Left < 16 Then
            btn.Padding = New Padding(18, 0, 0, 0)
        End If
        btn.FlatAppearance.MouseOverBackColor = Lighten(btn.BackColor, 0.08F)
        btn.FlatAppearance.MouseDownBackColor = Lighten(btn.BackColor, 0.12F)
    End Sub

    Private Function IsTileButton(btn As Button, tagStr As String) As Boolean
        If tagStr.IndexOf("Tile", StringComparison.OrdinalIgnoreCase) >= 0 Then
            Return True
        End If

        Dim parentFlow = TryCast(btn.Parent, FlowLayoutPanel)
        Dim hasLargeMinimum = btn.MinimumSize.Width >= 200 OrElse btn.MinimumSize.Height >= 44
        Dim approxWidth = Math.Max(Math.Max(btn.Width, btn.PreferredSize.Width), btn.MinimumSize.Width)

        If hasLargeMinimum OrElse approxWidth >= 200 Then
            If parentFlow IsNot Nothing Then
                Return True
            End If
        End If

        Return False
    End Function

    Private Sub ApplyTileButtonStyle(btn As Button)
        btn.BackColor = Color.White
        btn.ForeColor = Palette.Foreground
        btn.Font = Palette.BaseFont

        If btn.Padding.Left < 16 Then
            btn.Padding = New Padding(16, btn.Padding.Top, Math.Max(16, btn.Padding.Right), btn.Padding.Bottom)
        End If

        Dim hoverBack = Palette.PrimaryLight
        Dim baseBack = btn.BackColor

        btn.FlatAppearance.BorderSize = 1
        btn.FlatAppearance.BorderColor = Darken(btn.BackColor, 0.25F)
        btn.FlatAppearance.MouseOverBackColor = hoverBack
        btn.FlatAppearance.MouseDownBackColor = Lighten(hoverBack, 0.08F)

        If Not IsStyled(btn) Then
            AddHandler btn.MouseEnter, Sub(s, e) btn.BackColor = hoverBack
            AddHandler btn.MouseLeave, Sub(s, e) btn.BackColor = baseBack
            AddHandler btn.GotFocus, Sub(s, e) btn.BackColor = hoverBack
            AddHandler btn.LostFocus, Sub(s, e) btn.BackColor = baseBack
        End If

        SetupReactiveBorder(btn, 1, 0.18F, 0.35F, True)
    End Sub

    Private Sub ApplyPrimaryButtonStyle(btn As Button)
        btn.BackColor = Palette.Primary
        btn.ForeColor = Color.White
        btn.Font = Palette.BaseFont

        If Not IsStyled(btn) Then
            AddHandler btn.MouseEnter, Sub(s, e) btn.BackColor = Darken(Palette.Primary, 0.1F)
            AddHandler btn.MouseLeave, Sub(s, e) btn.BackColor = Palette.Primary
            AddHandler btn.GotFocus, Sub(s, e) btn.BackColor = Darken(Palette.Primary, 0.1F)
            AddHandler btn.LostFocus, Sub(s, e) btn.BackColor = Palette.Primary
        End If

        SetupReactiveBorder(btn, 1, 0.25F, 0.45F, True)
    End Sub

    Private Sub SyncHoverBackToCurrent(btn As Button)
        btn.FlatAppearance.MouseOverBackColor = btn.BackColor
        btn.FlatAppearance.MouseDownBackColor = btn.BackColor
        If Not IsStyled(btn) Then
            AddHandler btn.BackColorChanged,
                Sub(s, e)
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor
                End Sub
        End If
    End Sub

    Private Sub SetupReactiveBorder(btn As Button,
                                    initialBorderSize As Integer,
                                    darkenBase As Single,
                                    darkenHover As Single,
                                    addHandlersIfNotStyled As Boolean)
        btn.FlatAppearance.BorderSize = initialBorderSize
        btn.FlatAppearance.BorderColor = Darken(btn.BackColor, darkenBase)

        If addHandlersIfNotStyled AndAlso Not IsStyled(btn) Then
            AddHandler btn.MouseEnter,
                Sub(s, e)
                    btn.FlatAppearance.BorderSize = 2
                    btn.FlatAppearance.BorderColor = Darken(btn.BackColor, darkenHover)
                End Sub
            AddHandler btn.MouseLeave,
                Sub(s, e)
                    btn.FlatAppearance.BorderSize = initialBorderSize
                    btn.FlatAppearance.BorderColor = Darken(btn.BackColor, darkenBase)
                End Sub
            AddHandler btn.GotFocus,
                Sub(s, e)
                    btn.FlatAppearance.BorderSize = 2
                    btn.FlatAppearance.BorderColor = Darken(btn.BackColor, darkenHover)
                End Sub
            AddHandler btn.LostFocus,
                Sub(s, e)
                    btn.FlatAppearance.BorderSize = initialBorderSize
                    btn.FlatAppearance.BorderColor = Darken(btn.BackColor, darkenBase)
                End Sub
            AddHandler btn.BackColorChanged,
                Sub(s, e)
                    btn.FlatAppearance.BorderColor = Darken(btn.BackColor, darkenBase)
                End Sub

            MarkStyled(btn)
        End If
    End Sub

    Private Sub StyleDataGridView(dgv As DataGridView)
        dgv.BackgroundColor = Color.White
        dgv.BorderStyle = BorderStyle.None
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.GridColor = Palette.Border

        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Palette.Background
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Palette.Foreground
        dgv.ColumnHeadersDefaultCellStyle.Font = Palette.TitleFont
        dgv.ColumnHeadersDefaultCellStyle.Padding = New Padding(5)

        dgv.DefaultCellStyle.BackColor = Color.White
        dgv.DefaultCellStyle.ForeColor = Palette.Foreground
        dgv.DefaultCellStyle.SelectionBackColor = Palette.PrimaryLight
        dgv.DefaultCellStyle.SelectionForeColor = Palette.Foreground
        dgv.RowHeadersVisible = False
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Palette.Background
    End Sub

    Private Function Darken(c As Color, amount As Single) As Color
        Dim factor As Single = Math.Max(0.0F, Math.Min(1.0F, 1.0F - amount))
        Return Color.FromArgb(c.A, CInt(c.R * factor), CInt(c.G * factor), CInt(c.B * factor))
    End Function

    Private Function Lighten(c As Color, amount As Single) As Color
        Dim factor As Single = Math.Max(0.0F, Math.Min(1.0F, amount))
        Dim r As Integer = CInt(c.R + (255 - c.R) * factor)
        Dim g As Integer = CInt(c.G + (255 - c.G) * factor)
        Dim b As Integer = CInt(c.B + (255 - c.B) * factor)
        Return Color.FromArgb(c.A, r, g, b)
    End Function

    Private NotInheritable Class Native
        Private Sub New()
        End Sub

        Friend Const EM_SETCUEBANNER As Integer = &H1501
        Friend Const CB_SETCUEBANNER As Integer = &H1703

        <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
        Friend Shared Function SendMessage(hWnd As IntPtr,
                                           msg As Integer,
                                           wParam As IntPtr,
                                           lParam As String) As IntPtr
        End Function
    End Class

    Public Sub SetCue(ctrl As Control, text As String, Optional showWhenFocused As Boolean = False)
        If ctrl Is Nothing Then Throw New ArgumentNullException(NameOf(ctrl))

        If TypeOf ctrl Is TextBoxBase Then
            Dim tb = DirectCast(ctrl, TextBoxBase)
            Dim w = If(showWhenFocused, New IntPtr(1), IntPtr.Zero)
            Native.SendMessage(tb.Handle, Native.EM_SETCUEBANNER, w, text)

        ElseIf TypeOf ctrl Is ComboBox Then
            Dim cb = DirectCast(ctrl, ComboBox)
            Native.SendMessage(cb.Handle, Native.CB_SETCUEBANNER, IntPtr.Zero, text)
        Else
        End If
    End Sub


End Module


