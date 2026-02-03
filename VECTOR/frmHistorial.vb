Imports System.Data.Entity

Public Class frmHistorial

    Private db As New SecretariaDBEntities()
    Private _idDocumento As Long

    ' Constructor que obliga a pasar el ID
    Public Sub New(idDoc As Long)
        InitializeComponent()
        _idDocumento = idDoc
    End Sub

    Private Sub frmHistorial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarHistorial()
    End Sub

    Private Sub CargarHistorial()
        Try
            ' PASO 1: Identificar familia
            Dim docActual = db.Mae_Documento.Find(_idDocumento)

            If docActual Is Nothing Then
                MessageBox.Show("El documento no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            lblNumero.Text = docActual.Cat_TipoDocumento.Codigo & " " & docActual.NumeroOficial
            lblAsunto.Text = docActual.Asunto
            Dim guidFamilia = docActual.IdHiloConversacion

            ' PASO 2: Consulta LINQ
            Dim historial = From m In db.Tra_Movimiento
                            Where m.Mae_Documento.IdHiloConversacion = guidFamilia
                            Order By m.FechaMovimiento Descending
                            Select New With {
                                .Fecha = m.FechaMovimiento,
                                .ID_Doc = m.IdDocumento,
                                .Documento = m.Mae_Documento.Cat_TipoDocumento.Codigo & " " & m.Mae_Documento.NumeroOficial,
                                .Origen = m.Cat_Oficina.Nombre,
                                .Destino = m.Cat_Oficina1.Nombre,
                                .Estado = m.Cat_Estado.Nombre,
                                .Observacion = m.ObservacionPase,
                                .Responsable = m.Cat_Usuario.UsuarioLogin
                            }

            ' PASO 3: Llenar Grilla
            dgvHistoria.DataSource = historial.ToList()

            ' PASO 4: Estética
            If dgvHistoria.Columns.Count > 0 Then
                With dgvHistoria
                    .Columns("Fecha").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
                    .Columns("Fecha").Width = 110
                    .Columns("ID_Doc").Visible = False
                    .Columns("Documento").Width = 130
                    .Columns("Documento").HeaderText = "Pieza / Doc"
                    .Columns("Origen").Width = 140
                    .Columns("Destino").Width = 140
                    .Columns("Estado").Width = 80
                    .Columns("Responsable").Width = 70
                    .Columns("Observacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

                    ' MEJORA VISUAL EXTRA: Quitar bordes feos de celdas
                    .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
                    .GridColor = Color.WhiteSmoke
                End With
            End If

            ' PASO 5: COLOREADO INTELIGENTE
            For Each row As DataGridViewRow In dgvHistoria.Rows

                ' Si es el documento que estamos rastreando
                If CLng(row.Cells("ID_Doc").Value) = _idDocumento Then

                    ' Color Normal (Amarillo suave)
                    row.DefaultCellStyle.BackColor = Color.LightYellow
                    row.DefaultCellStyle.ForeColor = Color.Black

                    ' TRUCO: Color de Selección (Amarillo fuerte)
                    ' Esto evita que se ponga AZUL cuando le das clic
                    row.DefaultCellStyle.SelectionBackColor = Color.Gold
                    row.DefaultCellStyle.SelectionForeColor = Color.Black

                    ' Opcional: Poner negrita para destacar más
                    row.DefaultCellStyle.Font = New Font(dgvHistoria.Font, FontStyle.Bold)
                Else
                    ' Para el resto, dejamos el blanco y selección azul normal
                    row.DefaultCellStyle.BackColor = Color.White
                End If
            Next

            ' PASO 6: QUITAR EL FOCO INICIAL
            ' Esto hace que al abrir, ninguna fila esté seleccionada (adiós franja azul)
            dgvHistoria.ClearSelection()
            dgvHistoria.CurrentCell = Nothing

        Catch ex As Exception
            MessageBox.Show("Error al leer la trazabilidad: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class