Public Module AuditoriaSistema

    Public Sub RegistrarEvento(descripcion As String, modulo As String, Optional usuarioId As Integer? = Nothing)
        Try
            Using uow As New UnitOfWork()
                Dim repoEventos = uow.Repository(Of EventosSistema)()
                Dim idUsuario = If(usuarioId.HasValue, usuarioId.Value, SesionGlobal.UsuarioID)
                Dim log As New EventosSistema() With {
                    .FechaEvento = DateTime.Now,
                    .Descripcion = descripcion,
                    .UsuarioId = idUsuario,
                    .Modulo = modulo
                }
                repoEventos.Add(log)
                uow.Commit()
            End Using
        Catch
            ' No interrumpimos el flujo principal si falla la auditoría.
        End Try
    End Sub

End Module
