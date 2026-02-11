Imports System.Runtime.InteropServices

Imports System.Windows.Forms

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

    Private Function ResolveMdiContainer(owner As Form) As Form
        If owner Is Nothing Then
            Return Nothing
        End If

        If owner.IsMdiContainer Then
            Return owner
        End If

        Return owner.MdiParent
    End Function

    Public Sub ShowFormInMdi(owner As Form, child As Form, Optional onClosed As FormClosedEventHandler = Nothing)
        Dim mdiContainer = ResolveMdiContainer(owner)
        If mdiContainer Is Nothing Then
            If onClosed IsNot Nothing Then
                AddHandler child.FormClosed, onClosed
            End If

            child.ShowDialog(owner)
            Return
        End If

        child.MdiParent = mdiContainer
        child.WindowState = FormWindowState.Maximized
        child.ShowIcon = False

        If onClosed IsNot Nothing Then
            AddHandler child.FormClosed, onClosed
        End If

        child.Show()
        child.BringToFront()
    End Sub

    ''' <summary>
    ''' Abre un formulario MDI único sin parámetros (si existe, lo reutiliza).
    ''' </summary>
    Public Sub ShowUniqueFormInMdi(Of T As {Form, New})(owner As Form,
                                                        Optional sourceForm As Form = Nothing,
                                                        Optional onClosed As FormClosedEventHandler = Nothing)
        Dim mdiContainer = ResolveMdiContainer(owner)
        If mdiContainer Is Nothing Then Return

        Dim formulario As Form = mdiContainer.MdiChildren.FirstOrDefault(Function(f) TypeOf f Is T)
        If formulario Is Nothing Then
            formulario = New T()
            If onClosed IsNot Nothing Then
                AddHandler formulario.FormClosed, onClosed
            End If
            ShowFormInMdi(mdiContainer, formulario)
        Else
            formulario.ShowIcon = False
            If formulario.WindowState <> FormWindowState.Maximized Then
                formulario.WindowState = FormWindowState.Maximized
            End If
            formulario.Activate()
            formulario.BringToFront()
        End If

        If sourceForm IsNot Nothing AndAlso Not ReferenceEquals(sourceForm, formulario) Then
            sourceForm.Close()
        End If
    End Sub

    ''' <summary>
    ''' Abre un formulario de detalle único por tipo y reemplaza cualquier instancia previa.
    ''' </summary>
    Public Sub ShowUniqueDetailFormInMdi(Of T As Form)(owner As Form,
                                                       id As Integer,
                                                       Optional sourceForm As Form = Nothing,
                                                       Optional onClosed As FormClosedEventHandler = Nothing)
        Dim mdiContainer = ResolveMdiContainer(owner)
        If mdiContainer Is Nothing Then Return

        Dim existente = mdiContainer.MdiChildren.OfType(Of T)().FirstOrDefault()
        If existente IsNot Nothing Then
            existente.Close()
        End If

        Dim nuevoFormulario = CType(Activator.CreateInstance(GetType(T), id), Form)
        If onClosed IsNot Nothing Then
            AddHandler nuevoFormulario.FormClosed, onClosed
        End If

        ShowFormInMdi(mdiContainer, nuevoFormulario)

        If sourceForm IsNot Nothing AndAlso Not ReferenceEquals(sourceForm, nuevoFormulario) Then
            sourceForm.Close()
        End If
    End Sub

    ''' <summary>
    ''' Abre una nueva instancia en el contenedor MDI actual.
    ''' </summary>
    Public Sub ShowNewInstanceInMdi(owner As Form, formularioAAbrir As Form, Optional onClosed As FormClosedEventHandler = Nothing)
        ShowFormInMdi(owner, formularioAAbrir, onClosed)
    End Sub

End Module
