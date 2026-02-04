Imports System.Data.Entity
Imports System.Linq ' Aseguramos que LINQ esté disponible para las listas

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

            ' =============================================================================
            ' NUEVA LÓGICA: DIVIDIMOS EN DOS PASOS PARA PODER CONTAR Y FORMATEAR
            ' =============================================================================

            ' PASO 2: Consulta LINQ - DATOS PUROS (Traemos el conteo desde la BD)
            Dim historialDatos = (From m In db.Tra_Movimiento
                                  Where m.Mae_Documento.IdHiloConversacion = guidFamilia
                                  Order By m.FechaMovimiento Descending
                                  Select New With {
                                      .Fecha = m.FechaMovimiento,
                                      .ID_Doc = m.IdDocumento,
                                      .Tipo = m.Mae_Documento.Cat_TipoDocumento.Codigo,
                                      .Numero = m.Mae_Documento.NumeroOficial,
                                      .Origen = m.Cat_Oficina.Nombre,
                                      .Destino = m.Cat_Oficina1.Nombre,
                                      .Estado = m.Cat_Estado.Nombre,
                                      .Observacion = m.ObservacionPase,
                                      .Responsable = m.Cat_Usuario.UsuarioLogin,
                                      .Cant_Hijos = db.Mae_Documento.Where(Function(h) h.IdDocumentoPadre = m.IdDocumento And h.IdEstadoActual <> 5).Count()
                                  }).ToList() ' <--- Ejecutamos la consulta SQL aquí

            ' PASO 3: Proyección en Memoria - ARMADO VISUAL (Concatenamos el número)
            Dim historialVisual = historialDatos.Select(Function(x) New With {
                .Fecha = x.Fecha,
                .ID_Doc = x.ID_Doc,
                .Documento = x.Tipo & " " & x.Numero & If(x.Cant_Hijos > 0, " (" & x.Cant_Hijos & ")", ""), ' <--- AQUÍ ESTÁ EL CAMBIO
                .Origen = x.Origen,
                .Destino = x.Destino,
                .Estado = x.Estado,
                .Observacion = x.Observacion,
                .Responsable = x.Responsable
            }).ToList()

            ' PASO 4: Llenar Grilla
            dgvHistoria.DataSource = historialVisual

            ' PASO 5: Estética
            If dgvHistoria.Columns.Count > 0 Then
                With dgvHistoria
                    .Columns("Fecha").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
                    .Columns("Fecha").Width = 110
                    .Columns("ID_Doc").Visible = False

                    .Columns("Documento").Width = 160 ' Un poco más ancho para que quepa el número
                    .Columns("Documento").HeaderText = "Pieza / Doc"

                    .Columns("Origen").Width = 140
                    .Columns("Destino").Width = 140
                    .Columns("Estado").Width = 80
                    .Columns("Responsable").Width = 70
                    .Columns("Observacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

                    ' MEJORA VISUAL EXTRA
                    .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
                    .GridColor = Color.WhiteSmoke
                End With
            End If

            ' PASO 6: COLOREADO INTELIGENTE
            For Each row As DataGridViewRow In dgvHistoria.Rows
                ' Si es el documento que estamos rastreando
                If CLng(row.Cells("ID_Doc").Value) = _idDocumento Then
                    row.DefaultCellStyle.BackColor = Color.LightYellow
                    row.DefaultCellStyle.ForeColor = Color.Black
                    row.DefaultCellStyle.SelectionBackColor = Color.Gold
                    row.DefaultCellStyle.SelectionForeColor = Color.Black
                    row.DefaultCellStyle.Font = New Font(dgvHistoria.Font, FontStyle.Bold)
                Else
                    row.DefaultCellStyle.BackColor = Color.White
                End If
            Next

            ' PASO 7: QUITAR EL FOCO INICIAL
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