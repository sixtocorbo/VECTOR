Public Class CupoSecretariaRecord
    Public Property IdCupo As Integer
    Public Property IdTipo As Integer
    Public Property Cantidad As Integer
    Public Property Fecha As DateTime
End Class

Public Class CupoSecretariaView
    Public Property IdCupo As Integer
    Public Property Fecha As DateTime
    Public Property IdUsuario As Integer?
    Public Property IdTipo As Integer
    Public Property Cantidad As Integer
    Public Property TipoNombre As String
    Public Property UsuarioNombre As String
End Class
