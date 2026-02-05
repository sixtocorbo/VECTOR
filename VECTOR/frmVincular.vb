Imports System.Data.Entity
Imports System.Threading.Tasks
Imports System.Text ' Necesario para StringBuilder

Public Class frmVincular

    ' Constructor opcional por si quieres pre-cargar el ID del hijo desde la grilla
    Public Sub New(Optional idHijoSugerido As Long = 0)
        InitializeComponent()
        If idHijoSugerido > 0 Then
            txtIdHijo.Text = idHijoSugerido.ToString()
            ' Ponemos el foco en el padre porque el hijo ya está puesto
            Me.ActiveControl = txtIdPadre
        End If
    End Sub

    Private Sub frmVincular_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
    End Sub

    Private Async Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        ' 1. VALIDACIÓN DE ENTRADA
        If String.IsNullOrWhiteSpace(txtIdHijo.Text) OrElse Not IsNumeric(txtIdHijo.Text) Then
            Toast.Show(Me, "Ingrese un ID de Hijo válido.", ToastType.Warning)
            Return
        End If
        If String.IsNullOrWhiteSpace(txtIdPadre.Text) OrElse Not IsNumeric(txtIdPadre.Text) Then
            Toast.Show(Me, "Ingrese un ID de Padre válido.", ToastType.Warning)
            Return
        End If

        Dim idHijo As Long = CLng(txtIdHijo.Text)
        Dim idNuevoPadre As Long = CLng(txtIdPadre.Text)

        If idHijo = idNuevoPadre Then
            Toast.Show(Me, "El ID Hijo y el ID Padre no pueden ser el mismo.", ToastType.Warning)
            Return
        End If

        btnConfirmar.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            Using uow As New UnitOfWork()
                Dim docRepo = uow.Repository(Of Mae_Documento)()

                ' A. BUSCAR AL HIJO (Incluyendo su Tipo para el mensaje)
                Dim docHijo = Await docRepo.GetQueryable(tracking:=True) _
                                           .Include("Cat_TipoDocumento") _
                                           .FirstOrDefaultAsync(Function(d) d.IdDocumento = idHijo)

                If docHijo Is Nothing Then
                    Toast.Show(Me, "El Documento HIJO no existe.", ToastType.Error)
                    Return
                End If

                ' B. BUSCAR AL NUEVO PADRE (Incluyendo su Tipo para el mensaje)
                Dim docPadreDestino = Await docRepo.GetQueryable(tracking:=True) _
                                                   .Include("Cat_TipoDocumento") _
                                                   .FirstOrDefaultAsync(Function(d) d.IdDocumento = idNuevoPadre)

                If docPadreDestino Is Nothing Then
                    Toast.Show(Me, "El Documento PADRE no existe.", ToastType.Error)
                    Return
                End If

                ' =========================================================================
                ' 🛡️ NIVEL 1: INTEGRIDAD - ¿EL HIJO YA TIENE PADRE?
                ' =========================================================================
                If docHijo.IdDocumentoPadre.HasValue Then
                    Dim idPadreActual = docHijo.IdDocumentoPadre.Value

                    ' Verificamos si ya es hijo del mismo padre que intentamos asignar
                    If idPadreActual = idNuevoPadre Then
                        Toast.Show(Me, "Este documento YA está vinculado a ese Padre.", ToastType.Info)
                        Return
                    End If

                    Dim docPadreActual = Await docRepo.GetQueryable().FirstOrDefaultAsync(Function(d) d.IdDocumento = idPadreActual)
                    Dim infoPadre As String = "ID: " & idPadreActual
                    If docPadreActual IsNot Nothing Then
                        infoPadre = docPadreActual.Cat_TipoDocumento.Codigo & " " & docPadreActual.NumeroOficial
                    End If

                    Toast.Show(Me, "⛔ ACCIÓN BLOQUEADA" & vbCrLf & vbCrLf &
                                    "El documento HIJO no es libre. Actualmente pertenece a:" & vbCrLf &
                                    "📂 " & infoPadre & vbCrLf & vbCrLf &
                                    "Para moverlo, primero debe desvincularlo.", ToastType.Warning)
                    Return
                End If

                ' =========================================================================
                ' 🛡️ NIVEL 2: DETECCIÓN DE ENROQUE (INVERSIÓN DE JERARQUÍA)
                ' =========================================================================
                ' Verificamos si el PADRE DESTINO es en realidad un DESCENDIENTE del HIJO actual.
                Dim esDescendiente As Boolean = False
                Dim tempDoc = docPadreDestino

                ' Bucle hacia arriba desde el Padre Destino
                While tempDoc.IdDocumentoPadre.HasValue
                    If tempDoc.IdDocumentoPadre.Value = docHijo.IdDocumento Then
                        esDescendiente = True
                        Exit While
                    End If
                    tempDoc = Await docRepo.GetQueryable().FirstOrDefaultAsync(Function(d) d.IdDocumento = tempDoc.IdDocumentoPadre.Value)
                    If tempDoc Is Nothing Then Exit While
                End While

                If esDescendiente Then
                    Dim resp = MessageBox.Show("🔄 INVERSIÓN DE JERARQUÍA DETECTADA (ENROQUE)" & vbCrLf & vbCrLf &
                                               "Estás intentando que el PADRE (" & docHijo.NumeroOficial & ")" & vbCrLf &
                                               "se convierta en subordinado de su propio DESCENDIENTE (" & docPadreDestino.NumeroOficial & ")." & vbCrLf & vbCrLf &
                                               "¿Deseas realizar este cambio de roles?",
                                               "Invertir Mandos", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

                    If resp = DialogResult.No Then Return

                    ' Ejecutamos la liberación del Nuevo Padre (que antes era nieto/hijo)
                    docPadreDestino.IdDocumentoPadre = Nothing
                    ' Guardamos este cambio preliminar para romper el ciclo
                    Await uow.CommitAsync()
                Else
                    ' SI NO ES ENROQUE: Lógica normal del Abuelo
                    ' Si el destino es un adjunto, apuntamos al jefe real de ese adjunto
                    If docPadreDestino.IdDocumentoPadre.HasValue Then
                        Dim idAbuelo = docPadreDestino.IdDocumentoPadre.Value
                        docPadreDestino = Await docRepo.GetQueryable(tracking:=True) _
                                                     .Include("Cat_TipoDocumento") _
                                                     .FirstOrDefaultAsync(Function(d) d.IdDocumento = idAbuelo)

                        If docPadreDestino Is Nothing Then
                            Toast.Show(Me, "No se encontró el expediente raíz del destino.", ToastType.Error)
                            Return
                        End If
                        ' Actualizamos el textbox para que el usuario vea el cambio real
                        txtIdPadre.Text = docPadreDestino.IdDocumento.ToString()
                    End If
                End If

                ' Validación final anti-circular
                If docPadreDestino.IdDocumento = docHijo.IdDocumento Then
                    Toast.Show(Me, "⛔ ERROR: Referencia circular directa.", ToastType.Error)
                    Return
                End If

                ' VALIDACIÓN CRONOLÓGICA
                Dim fHijo = If(docHijo.FechaRecepcion, docHijo.FechaCreacion)
                Dim fPadre = If(docPadreDestino.FechaRecepcion, docPadreDestino.FechaCreacion)

                If fHijo < fPadre Then
                    If MessageBox.Show("⚠️ El documento HIJO es más antiguo que el PADRE." & vbCrLf & "¿Continuar de todas formas?",
                                       "Cronología", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return
                End If

                ' =========================================================================
                ' 🚀 EJECUCIÓN: VINCULACIÓN Y ADOPCIÓN
                ' =========================================================================

                ' Construimos etiquetas legibles: "MEMO 2024/123"
                ' Usamos el operador seguro ?. o validamos, pero como usamos Include arriba debería estar bien.
                Dim etiquetaHijo As String = If(docHijo.Cat_TipoDocumento IsNot Nothing, docHijo.Cat_TipoDocumento.Codigo, "DOC") & " " & docHijo.NumeroOficial
                Dim etiquetaPadre As String = If(docPadreDestino.Cat_TipoDocumento IsNot Nothing, docPadreDestino.Cat_TipoDocumento.Codigo, "DOC") & " " & docPadreDestino.NumeroOficial

                Dim mensajeConfirmacion As New StringBuilder()
                mensajeConfirmacion.AppendLine("¿Confirma la vinculación de documentos?")
                mensajeConfirmacion.AppendLine()
                mensajeConfirmacion.AppendLine("📄 DOCUMENTO (Hijo):  " & etiquetaHijo)
                mensajeConfirmacion.AppendLine("📂 SE MOVERÁ AL EXP:  " & etiquetaPadre)
                mensajeConfirmacion.AppendLine()
                mensajeConfirmacion.AppendLine("⚠️ El documento pasará a formar parte del expediente indicado.")

                If MessageBox.Show(mensajeConfirmacion.ToString(),
                                   "Confirmar Vinculación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                    ' Gestión de Hijos Huérfanos (si el hijo que movemos traía familia)
                    Dim hijosDelHijo = Await docRepo.GetQueryable(tracking:=True).Where(Function(h) h.IdDocumentoPadre.HasValue AndAlso h.IdDocumentoPadre.Value = idHijo).ToListAsync()
                    Dim cantidadHijos As Integer = hijosDelHijo.Count

                    If cantidadHijos > 0 Then
                        Dim respuesta = MessageBox.Show("⚠️ REESTRUCTURA FAMILIAR" & vbCrLf & vbCrLf &
                                                        "El documento '" & docHijo.NumeroOficial & "' tiene " & cantidadHijos & " adjuntos propios." & vbCrLf &
                                                        "¿Desea que estos pasen a depender directamente del Nuevo Jefe (" & docPadreDestino.NumeroOficial & ")?" & vbCrLf & vbCrLf &
                                                        "👉 SÍ: Todos se vuelven hermanos (Aplanar jerarquía)." & vbCrLf &
                                                        "👉 NO: Cancelar operación.",
                                                        "Decisión de Jerarquía", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                        If respuesta = DialogResult.No Then Return

                        For Each nieto In hijosDelHijo
                            nieto.IdDocumentoPadre = docPadreDestino.IdDocumento
                            nieto.IdHiloConversacion = docPadreDestino.IdHiloConversacion
                        Next

                        ' CORRECCIÓN: Uso de parámetro con nombre para evitar error de tipos
                        AuditoriaSistema.RegistrarEvento($"Reubicación de {cantidadHijos} adjuntos al exp {docPadreDestino.NumeroOficial}.", "REESTRUCTURA", unitOfWorkExterno:=uow)
                    End If

                    ' Vinculación final
                    docHijo.IdDocumentoPadre = docPadreDestino.IdDocumento
                    docHijo.IdHiloConversacion = docPadreDestino.IdHiloConversacion

                    Await uow.CommitAsync()

                    AuditoriaSistema.RegistrarEvento($"Vinculación manual de {docHijo.NumeroOficial} (ID:{idHijo}) a {docPadreDestino.NumeroOficial} (ID:{docPadreDestino.IdDocumento}).", "DOCUMENTOS")
                    Toast.Show(Me, "✅ Vinculación exitosa.", ToastType.Success)

                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                End If

            End Using

        Catch ex As Exception
            Toast.Show(Me, "Error: " & ex.Message, ToastType.Error)
        Finally
            Me.Cursor = Cursors.Default
            btnConfirmar.Enabled = True
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class