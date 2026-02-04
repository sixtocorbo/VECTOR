Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Public Enum ToastType
    Info
    Success
    Warning
    [Error]
End Enum

Public NotInheritable Class Toast
    Inherits Form

    ' === Constantes de estilo/comportamiento ===
    Private Const DefaultDurationMs As Integer = 3000
    Private Const CornerRadius As Integer = 12
    Private Const PaddingPx As Integer = 12
    Private Const MaxWidthPx As Integer = 420
    Private Const MarginFromEdges As Integer = 20
    Private Const StackGap As Integer = 8
    Private Const FadeIntervalMs As Integer = 15
    Private Const FadeStep As Double = 0.08

    ' === Estado compartido ===
    Private Shared ReadOnly OpenToasts As New List(Of Toast)

    ' === Estado de instancia ===
    Private ReadOnly _lifeTimer As New Timer() With {.Interval = DefaultDurationMs}
    Private ReadOnly _fadeTimer As New Timer() With {.Interval = FadeIntervalMs}
    Private _fadingIn As Boolean = True
    Private _targetScreen As Screen

    ' sticky = no autocierre; closeOnClick = si se cierra con clic
    Private _sticky As Boolean = False
    Private _closeOnClick As Boolean = True

    Private ReadOnly _lbl As New Label() With {
        .AutoSize = False,
        .MaximumSize = New Size(MaxWidthPx, 0),
        .Font = New Font("Segoe UI", 9.5!, FontStyle.Regular, GraphicsUnit.Point),
        .ForeColor = Color.White
    }

    ' === P/Invoke para ventana redondeada ===
    <DllImport("gdi32.dll", SetLastError:=True)>
    Private Shared Function CreateRoundRectRgn(leftRect As Integer, topRect As Integer, rightRect As Integer, bottomRect As Integer, widthEllipse As Integer, heightEllipse As Integer) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SetWindowRgn(hWnd As IntPtr, hRgn As IntPtr, bRedraw As Boolean) As Integer
    End Function

    ' === Ctor privado: usar Show / ShowSticky para construir ===
    Private Sub New(texto As String, tipo As ToastType, screen As Screen)
        Me.FormBorderStyle = FormBorderStyle.None
        Me.ShowInTaskbar = False
        Me.TopMost = True
        Me.StartPosition = FormStartPosition.Manual
        Me.Opacity = 0
        Me.DoubleBuffered = True
        Me._targetScreen = screen

        Dim back As Color, border As Color
        Select Case tipo
            Case ToastType.Success : back = Color.FromArgb(46, 125, 50) : border = Color.FromArgb(27, 94, 32)
            Case ToastType.Warning : back = Color.FromArgb(245, 124, 0) : border = Color.FromArgb(230, 81, 0)
            Case ToastType.Error : back = Color.FromArgb(211, 47, 47) : border = Color.FromArgb(183, 28, 28)
            Case Else : back = Color.FromArgb(33, 150, 243) : border = Color.FromArgb(25, 118, 210)
        End Select
        Me.BackColor = back

        _lbl.Text = texto
        _lbl.Padding = New Padding(PaddingPx)
        _lbl.MaximumSize = New Size(MaxWidthPx, 0)
        _lbl.AutoSize = True
        _lbl.BackColor = Color.Transparent

        ' Calcular tamaño según texto
        Using g = CreateGraphics()
            Dim proposed = New Size(MaxWidthPx - PaddingPx * 2, Integer.MaxValue)
            Dim sz = TextRenderer.MeasureText(g, texto, _lbl.Font, proposed, TextFormatFlags.WordBreak)
            Dim width = Math.Min(MaxWidthPx, sz.Width + PaddingPx * 2 + 2)
            Dim height = sz.Height + PaddingPx * 2 + 2
            Me.Size = New Size(width, height)
        End Using

        _lbl.Dock = DockStyle.Fill
        Controls.Add(_lbl)

        Me.Padding = New Padding(1)
        AddHandler Me.Paint, Sub(sender As Object, e As PaintEventArgs)
                                 Using pen As New Pen(border, 1)
                                     Dim rect = New Rectangle(0, 0, Me.ClientSize.Width - 1, Me.ClientSize.Height - 1)
                                     e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
                                     Using gp As New GraphicsPath()
                                         Dim r = CornerRadius
                                         gp.AddArc(rect.X, rect.Y, r, r, 180, 90)
                                         gp.AddArc(rect.Right - r, rect.Y, r, r, 270, 90)
                                         gp.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90)
                                         gp.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90)
                                         gp.CloseFigure()
                                         e.Graphics.DrawPath(pen, gp)
                                     End Using
                                 End Using
                             End Sub

        AddHandler _lifeTimer.Tick, AddressOf LifeTimer_Tick
        AddHandler _fadeTimer.Tick, AddressOf FadeTimer_Tick
        AddHandler Me.Click, Sub() If _closeOnClick Then BeginFadeOut()
        AddHandler _lbl.Click, Sub() If _closeOnClick Then BeginFadeOut()
    End Sub

    ' === Ventana sin foco / toolwindow ===
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp = MyBase.CreateParams
            Const WS_EX_NOACTIVATE As Integer = &H8000000
            Const WS_EX_TOOLWINDOW As Integer = &H80
            cp.ExStyle = cp.ExStyle Or WS_EX_NOACTIVATE Or WS_EX_TOOLWINDOW
            Return cp
        End Get
    End Property

    ' === Aparición y timers ===
    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        Dim rgn = CreateRoundRectRgn(0, 0, Me.Width, Me.Height, CornerRadius, CornerRadius)
        SetWindowRgn(Me.Handle, rgn, True)
        _fadingIn = True
        _fadeTimer.Start()
        If Not _sticky Then
            _lifeTimer.Start()
        End If
    End Sub

    Private Sub ShowForm()
        If Me.IsDisposed Then Return
        MyBase.Show()
    End Sub

    Private Sub LifeTimer_Tick(sender As Object, e As EventArgs)
        _lifeTimer.Stop()
        BeginFadeOut()
    End Sub

    Private Sub FadeTimer_Tick(sender As Object, e As EventArgs)
        If _fadingIn Then
            Me.Opacity = Math.Min(1.0, Me.Opacity + FadeStep)
            If Me.Opacity >= 1.0 Then _fadeTimer.Stop()
        Else
            Me.Opacity = Math.Max(0.0, Me.Opacity - FadeStep)
            If Me.Opacity <= 0 Then
                _fadeTimer.Stop()
                Close()
            End If
        End If
    End Sub

    Private Sub BeginFadeOut()
        If Me.IsDisposed Then Return
        _fadingIn = False
        _fadeTimer.Start()
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)
        SyncLock OpenToasts
            Dim idx = OpenToasts.IndexOf(Me)
            If idx >= 0 Then
                OpenToasts.RemoveAt(idx)
                ' Reacomodar la pila
                For i = idx To OpenToasts.Count - 1
                    Dim t = OpenToasts(i)
                    If t Is Nothing OrElse t.IsDisposed Then Continue For
                    Dim wa2 = t._targetScreen.WorkingArea
                    Dim x2 = wa2.Right - t.Width - MarginFromEdges
                    Dim y2 = wa2.Top + MarginFromEdges + (i * (t.Height + StackGap))
                    t.Location = New Point(x2, y2)

                Next
            End If
        End SyncLock
    End Sub

    ' ================= API PÚBLICA =================

    ' Toast normal (autocierre)
    Public Shared Shadows Sub Show(owner As Form, message As String, Optional type As ToastType = ToastType.Success, Optional durationMs As Integer = DefaultDurationMs)
        ' owner inválido => salir en silencio
        If owner IsNot Nothing AndAlso (owner.IsDisposed OrElse Not owner.IsHandleCreated) Then Exit Sub

        If owner IsNot Nothing AndAlso owner.InvokeRequired Then
            Try
                owner.BeginInvoke(CType(Sub() Show(owner, message, type, durationMs), MethodInvoker))
            Catch
                ' si el owner se está cerrando, ignorar
            End Try
            Return
        End If

        Dim screen As Screen = If(owner IsNot Nothing, Screen.FromControl(owner), Screen.PrimaryScreen)
        Dim t As New Toast(message, type, screen)
        t._lifeTimer.Interval = durationMs

        Dim wa = screen.WorkingArea
        SyncLock OpenToasts
            Dim stackIndex = OpenToasts.Count
            OpenToasts.Add(t)
        End SyncLock
        t.RepositionTopRight()
        t.ShowForm()
    End Sub

    ' Sobrecarga sin owner
    Public Shared Shadows Sub Show(message As String, Optional type As ToastType = ToastType.Success, Optional durationMs As Integer = DefaultDurationMs)
        Show(owner:=Nothing, message:=message, type:=type, durationMs:=durationMs)
    End Sub

    ' Toast persistente para progreso (no se auto-cierra ni con clic)
    Public Shared Function ShowSticky(owner As Form, message As String, Optional type As ToastType = ToastType.Info) As Toast
        If owner IsNot Nothing AndAlso (owner.IsDisposed OrElse Not owner.IsHandleCreated) Then Return Nothing

        Dim screen As Screen = If(owner IsNot Nothing, Screen.FromControl(owner), Screen.PrimaryScreen)
        Dim t As New Toast(message, type, screen) With {
            ._sticky = True,
            ._closeOnClick = False
        }
        t._lifeTimer.Stop()
        t._lifeTimer.Enabled = False

        Dim wa = screen.WorkingArea
        SyncLock OpenToasts
            Dim stackIndex = OpenToasts.Count
            OpenToasts.Add(t)
        End SyncLock
        t.RepositionTopRight()
        t.ShowForm()
        Return t

    End Function

    ' === Métodos de instancia (thread-safe) ===

    ' Actualiza el texto y re-calcula tamaño
    Public Sub UpdateMessage(texto As String)
        If Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return
        If Me.InvokeRequired Then
            Try
                Me.BeginInvoke(CType(Sub() UpdateMessage(texto), MethodInvoker))
            Catch
            End Try
            Return
        End If

        _lbl.Text = texto

        Using g = CreateGraphics()
            Dim proposed = New Size(MaxWidthPx - PaddingPx * 2, Integer.MaxValue)
            Dim sz = TextRenderer.MeasureText(g, texto, _lbl.Font, proposed, TextFormatFlags.WordBreak)
            Dim width = Math.Min(MaxWidthPx, sz.Width + PaddingPx * 2 + 2)
            Dim height = sz.Height + PaddingPx * 2 + 2
            Me.Size = New Size(width, height)
        End Using

        Dim rgn = CreateRoundRectRgn(0, 0, Me.Width, Me.Height, CornerRadius, CornerRadius)
        SetWindowRgn(Me.Handle, rgn, True)
        RepositionTopRight()
    End Sub

    ' Programar cierre con fade-out
    Public Sub CloseAfter(ms As Integer)
        If Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return
        If Me.InvokeRequired Then
            Try
                Me.BeginInvoke(CType(Sub() CloseAfter(ms), MethodInvoker))
            Catch
            End Try
            Return
        End If
        _lifeTimer.Interval = Math.Max(1, ms)
        _lifeTimer.Stop()
        _lifeTimer.Start()
        _closeOnClick = True ' al final, ya puede cerrarse por clic
        _sticky = False
    End Sub

    ' Cierre inmediato
    Public Sub CloseNow()
        If Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return
        If Me.InvokeRequired Then
            Try
                Me.BeginInvoke(CType(Sub() CloseNow(), MethodInvoker))
            Catch
            End Try
            Return
        End If
        _fadingIn = False
        _fadeTimer.Stop()
        Close()
    End Sub
    ' Reposiciona el toast en TOP-RIGHT respetando el índice en la pila
    Private Sub RepositionTopRight()
        If Me.IsDisposed Then Return
        Dim wa = _targetScreen.WorkingArea
        Dim idx As Integer
        SyncLock OpenToasts
            idx = OpenToasts.IndexOf(Me)
            If idx < 0 Then idx = 0
        End SyncLock
        Dim x = wa.Right - Me.Width - MarginFromEdges
        Dim y = wa.Top + MarginFromEdges + (idx * (Me.Height + StackGap))
        Me.Location = New Point(x, y)
    End Sub

End Class
