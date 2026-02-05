Imports System.Runtime.InteropServices

Public Module UIUtils

    ' Constante de Windows para definir el mensaje de "Cue Banner"
    Private Const EM_SETCUEBANNER As Integer = &H1501

    ' Importamos la función SendMessage de la librería user32.dll de Windows
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As Integer, <MarshalAs(UnmanagedType.LPWStr)> lParam As String) As IntPtr
    End Function

    ''' <summary>
    ''' Agrega un texto de fondo (Placeholder) a un TextBox estándar de Windows Forms.
    ''' </summary>
    Public Sub SetPlaceholder(txt As TextBox, texto As String)
        If txt IsNot Nothing Then
            SendMessage(txt.Handle, EM_SETCUEBANNER, 0, texto)
        End If
    End Sub

End Module