Imports System.Data.Entity
Imports System.Drawing

Public Class frmGestionTiempos

    Private _guardando As Boolean

    Private Class ItemTiempoViewModel
        Public Property IdTipo As Integer
        Public Property NombreTipo As String
        Public Property DiasPlazo As Integer
        Public Property EsNuevo As Boolean
        Public Property IdConfig As Integer?
    End Class

    Private Async Sub frmGestionTiempos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.Text = "Configuración de Vencimientos"
        Await CargarDatosAsync()
    End Sub

    Private Async Function CargarDatosAsync() As Task
        Try
            dgvTiempos.DataSource = Nothing

            Using uow As New UnitOfWork()
                Dim tipos = Await uow.Repository(Of Cat_TipoDocumento)().GetQueryable().
                    OrderBy(Function(t) t.Nombre).
                    ToListAsync()

                Dim reglas = Await uow.Repository(Of Cfg_TiemposRespuesta)().GetQueryable().
                    Where(Function(c) c.Prioridad = "Normal").
                    ToListAsync()

                Dim listaMostrar As New List(Of ItemTiempoViewModel)

                For Each t In tipos
                    Dim regla = reglas.FirstOrDefault(Function(r) r.IdTipoDocumento = t.IdTipo)

                    Dim item As New ItemTiempoViewModel With {
                        .IdTipo = t.IdTipo,
                        .NombreTipo = t.Nombre,
                        .DiasPlazo = If(regla IsNot Nothing, regla.DiasMaximos, 0),
                        .EsNuevo = (regla Is Nothing),
                        .IdConfig = If(regla IsNot Nothing, CType(regla.IdConfig, Integer?), Nothing)
                    }
                    listaMostrar.Add(item)
                Next

                dgvTiempos.DataSource = listaMostrar
                DiseñarGrilla()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Sub DiseñarGrilla()
        With dgvTiempos
            .Columns("IdTipo").Visible = False
            .Columns("EsNuevo").Visible = False
            .Columns("IdConfig").Visible = False

            .Columns("NombreTipo").HeaderText = "Documento"
            .Columns("NombreTipo").ReadOnly = True
            .Columns("NombreTipo").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            .Columns("DiasPlazo").HeaderText = "Días de Plazo (Vencimiento)"
            .Columns("DiasPlazo").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("DiasPlazo").DefaultCellStyle.BackColor = Color.LightYellow
        End With
    End Sub

    Private Sub dgvTiempos_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvTiempos.DataError
        MessageBox.Show("El valor ingresado no es válido. Ingrese un número entero.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        e.ThrowException = False
    End Sub

    Private Sub frmGestionTiempos_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not _guardando Then Return

        e.Cancel = True
        MessageBox.Show("Hay un guardado en progreso. Espere a que finalice.", "Guardado en progreso", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If _guardando Then Return

        _guardando = True
        CambiarEstadoGuardado(True)

        Try
            Dim lista = TryCast(dgvTiempos.DataSource, List(Of ItemTiempoViewModel))
            If lista Is Nothing Then Return

            If lista.Any(Function(item) item.DiasPlazo < 0) Then
                MessageBox.Show("Los días de plazo no pueden ser negativos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Using uow As New UnitOfWork()
                Dim repoConfig = uow.Repository(Of Cfg_TiemposRespuesta)()

                For Each item In lista
                    If item.EsNuevo Then
                        If item.DiasPlazo > 0 Then
                            Dim nuevaRegla As New Cfg_TiemposRespuesta With {
                                .IdTipoDocumento = item.IdTipo,
                                .Prioridad = "Normal",
                                .DiasMaximos = item.DiasPlazo,
                                .HorasMaximas = 0
                            }
                            repoConfig.Add(nuevaRegla)
                        End If
                    ElseIf item.IdConfig.HasValue Then
                        Dim idCfg = item.IdConfig.Value
                        Dim reglaExistente = Await repoConfig.GetQueryable().
                            FirstOrDefaultAsync(Function(r) r.IdConfig = idCfg)

                        If reglaExistente IsNot Nothing AndAlso reglaExistente.DiasMaximos <> item.DiasPlazo Then
                            reglaExistente.DiasMaximos = item.DiasPlazo
                        End If
                    End If
                Next

                Await uow.CommitAsync()
                MessageBox.Show("✅ Tiempos actualizados correctamente." & vbCrLf & "Los nuevos documentos usarán estos plazos.", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Await CargarDatosAsync()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al guardar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _guardando = False
            CambiarEstadoGuardado(False)
        End Try
    End Sub

    Private Sub CambiarEstadoGuardado(guardando As Boolean)
        btnGuardar.Enabled = Not guardando
        dgvTiempos.Enabled = Not guardando
        UseWaitCursor = guardando
    End Sub
End Class
