Module SesionGlobal

    ' --- SESIÓN SIMULADA (HARCODEADA) ---
    ' Mientras no tengamos pantalla de Login, usamos estos valores fijos.
    ' Así el sistema piensa que ya entraste.

    ' Asumimos que eres el Usuario 1
    Public UsuarioID As Integer = 1
    Public NombreUsuario As String = "Administrador"

    ' Asumimos que estás en la Oficina 1 (Mesa de Entrada)
    ' Si quisieras probar ser Jurídica, cambiarías este 1 por un 3 aquí mismo.
    Public OficinaID As Integer = 1
    Public NombreOficina As String = "Mesa de Entrada"

End Module