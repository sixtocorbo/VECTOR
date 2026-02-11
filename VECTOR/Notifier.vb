' NotifierShim.vb
Imports System.Text.RegularExpressions

Public Module Notifier

    ' ======= Parámetros por defecto de duración =======
    Private Const MIN_MS As Integer = 2000     ' 2s mínimo en pantalla
    Private Const MAX_MS As Integer = 9000     ' 9s máximo
    Private Const OVERHEAD_MS As Integer = 800 ' para dar tiempo de “aparición”
    Private Const MS_PER_WORD As Integer = 300 ' ~200 wpm → ~300 ms por palabra

    ' ======= Estimadores =======
    <System.Diagnostics.DebuggerStepThrough>
    Private Function EstimateDurationMs(message As String) As Integer
        If String.IsNullOrWhiteSpace(message) Then Return MIN_MS
        ' Contar palabras (secuencias \w) – robusto para la mayoría de los idiomas latinos
        Dim wc As Integer = Regex.Matches(message, "\w+", RegexOptions.CultureInvariant).Count
        Dim ms As Integer = OVERHEAD_MS + (wc * MS_PER_WORD)

        ' Alternativa por caracteres (descomenta si preferís):
        ' Dim chars = message.Length
        ' Dim ms As Integer = OVERHEAD_MS + CInt(chars * 35) ' ~35 ms por carácter

        If ms < MIN_MS Then ms = MIN_MS
        If ms > MAX_MS Then ms = MAX_MS
        Return ms
    End Function

    ' ======= API clásica con duración opcional =======
    ' Si ms <= 0, se calcula automáticamente con EstimateDurationMs(msg)

    <System.Diagnostics.DebuggerStepThrough>
    Public Sub Info(f As Form, msg As String, Optional ms As Integer = 0)
        If ms <= 0 Then ms = EstimateDurationMs(msg)
        Toast.Show(owner:=f, message:=msg, type:=ToastType.Info, durationMs:=ms)
    End Sub

    <System.Diagnostics.DebuggerStepThrough>
    Public Sub Success(f As Form, msg As String, Optional ms As Integer = 0)
        If ms <= 0 Then ms = EstimateDurationMs(msg)
        Toast.Show(owner:=f, message:=msg, type:=ToastType.Success, durationMs:=ms)
    End Sub

    <System.Diagnostics.DebuggerStepThrough>
    Public Sub Warn(f As Form, msg As String, Optional ms As Integer = 0)
        If ms <= 0 Then ms = EstimateDurationMs(msg)
        Toast.Show(owner:=f, message:=msg, type:=ToastType.Warning, durationMs:=ms)
    End Sub

    <System.Diagnostics.DebuggerStepThrough>
    Public Sub [Error](f As Form, msg As String, Optional ms As Integer = 0)
        If ms <= 0 Then ms = EstimateDurationMs(msg)
        Toast.Show(owner:=f, message:=msg, type:=ToastType.Error, durationMs:=ms)
    End Sub

    ' ======= Sobrecargas sin owner =======
    <System.Diagnostics.DebuggerStepThrough>
    Public Sub Info(msg As String, Optional ms As Integer = 0)
        If ms <= 0 Then ms = EstimateDurationMs(msg)
        Toast.Show(owner:=Nothing, message:=msg, type:=ToastType.Info, durationMs:=ms)
    End Sub

    <System.Diagnostics.DebuggerStepThrough>
    Public Sub Success(msg As String, Optional ms As Integer = 0)
        If ms <= 0 Then ms = EstimateDurationMs(msg)
        Toast.Show(owner:=Nothing, message:=msg, type:=ToastType.Success, durationMs:=ms)
    End Sub

    <System.Diagnostics.DebuggerStepThrough>
    Public Sub Warn(msg As String, Optional ms As Integer = 0)
        If ms <= 0 Then ms = EstimateDurationMs(msg)
        Toast.Show(owner:=Nothing, message:=msg, type:=ToastType.Warning, durationMs:=ms)
    End Sub

    <System.Diagnostics.DebuggerStepThrough>
    Public Sub [Error](msg As String, Optional ms As Integer = 0)
        If ms <= 0 Then ms = EstimateDurationMs(msg)
        Toast.Show(owner:=Nothing, message:=msg, type:=ToastType.Error, durationMs:=ms)
    End Sub

    ' ======= Progreso/estado pegado =======
    <System.Diagnostics.DebuggerStepThrough>
    Public Function ProgressStart(f As Form, initialMessage As String) As Toast
        ' Persiste hasta que el llamador lo cierre/actualice
        Return Toast.ShowSticky(f, initialMessage, ToastType.Info)
    End Function

End Module
