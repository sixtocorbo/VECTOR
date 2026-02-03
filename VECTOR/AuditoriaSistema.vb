Public Module AuditoriaSistema

    Public Sub RegistrarEvento(descripcion As String, modulo As String, Optional usuarioId As Integer? = Nothing)
        Try
            Using db As New SecretariaDBEntities()
                Dim idUsuario = If(usuarioId.HasValue, usuarioId.Value, SesionGlobal.UsuarioID)
                Dim log As New EventosSistema() With {
                    .FechaEvento = DateTime.Now,
                    .Descripcion = descripcion,
                    .UsuarioId = idUsuario,
                    .Modulo = modulo
                }
                db.EventosSistema.Add(log)
                db.SaveChanges()
            End Using
        Catch
            ' No interrumpimos el flujo principal si falla la auditoría.
        End Try
    End Sub

End Module
