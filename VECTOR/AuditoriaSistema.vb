Public Module AuditoriaSistema

    Public Sub RegistrarEvento(descripcion As String,
                              modulo As String,
                              Optional usuarioId As Integer? = Nothing,
                              Optional unitOfWorkExterno As IUnitOfWork = Nothing)
        Try
            Dim usaUnidadExterna = unitOfWorkExterno IsNot Nothing
            Dim uow As IUnitOfWork = If(usaUnidadExterna, unitOfWorkExterno, New UnitOfWork())

            Try
                Dim repoEventos = uow.Repository(Of EventosSistema)()
                Dim idUsuario = If(usuarioId.HasValue, usuarioId.Value, SesionGlobal.UsuarioID)
                Dim log As New EventosSistema() With {
                    .FechaEvento = DateTime.Now,
                    .Descripcion = descripcion,
                    .UsuarioId = idUsuario,
                    .Modulo = modulo
                }
                repoEventos.Add(log)
                If Not usaUnidadExterna Then
                    uow.Commit()
                End If
            Finally
                If Not usaUnidadExterna Then
                    uow.Dispose()
                End If
            End Try
        Catch
            If unitOfWorkExterno IsNot Nothing Then
                Throw
            End If
            ' No interrumpimos el flujo principal si falla la auditoría.
        End Try
    End Sub

End Module
