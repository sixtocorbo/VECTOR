Public Class SesionGlobal

    ' 1. DATOS DE LA SESIÓN
    Public Shared Property UsuarioID As Integer = 0
    Public Shared Property NombreUsuario As String = ""
    Public Shared Property OficinaID As Integer = 0
    Public Shared Property NombreOficina As String = ""

    ' Nuevo: Rol del usuario (ADMINISTRADOR, OPERADOR, etc.)
    Public Shared Property Rol As String = ""

    ' 2. SEGURIDAD INTELIGENTE
    ' Esta propiedad calcula automáticamente si tienes permisos
    Public Shared ReadOnly Property EsAdmin As Boolean
        Get
            If String.IsNullOrWhiteSpace(Rol) Then Return False
            Return Rol.ToUpper() = "ADMINISTRADOR"
        End Get
    End Property

    ' 3. INICIAR SESIÓN
    Public Shared Sub Iniciar(idUser As Integer, nomUser As String, rolUser As String, idOfi As Integer, nomOfi As String)
        UsuarioID = idUser
        NombreUsuario = nomUser
        Rol = rolUser
        OficinaID = idOfi
        NombreOficina = nomOfi
    End Sub

    ' 4. CERRAR SESIÓN
    Public Shared Sub Cerrar()
        UsuarioID = 0
        NombreUsuario = ""
        Rol = ""
        OficinaID = 0
        NombreOficina = ""
    End Sub

End Class