Imports System.Data.Entity
Imports System.Drawing.Printing
Imports System.Linq

Public Class frmHistorial

    Private Enum AlcanceHistorial
        DocumentoSeleccionado = 0
        FamiliaPadreAdjuntos = 1
        DocumentoYFamiliaHistorico = 2
    End Enum

    Private db As New SecretariaDBEntities()
    Private _idDocumento As Long
    Private _printBitmap As Bitmap
    Private WithEvents _printDocument As New PrintDocument()
    Private WithEvents cboAlcance As ComboBox
    Private WithEvents chkHistorico As CheckBox
    Private WithEvents dtpDesde As DateTimePicker
    Private WithEvents dtpHasta As DateTimePicker

    Public Sub New(idDoc As Long)
        InitializeComponent()
        _idDocumento = idDoc
    End Sub

    Private Sub frmHistorial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        InicializarControlesFiltro()
        CargarHistorial()
    End Sub

    Private Sub InicializarControlesFiltro()
        cboAlcance = New ComboBox With {
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Width = 260,
            .Location = New Point(540, 20)
        }
        cboAlcance.Items.AddRange(New Object() {
            "Solo documento seleccionado",
            "Familia (padre + adjuntos)",
            "Documento + familia (histórico)"
        })
        cboAlcance.SelectedIndex = CInt(AlcanceHistorial.DocumentoYFamiliaHistorico)

        chkHistorico = New CheckBox With {
            .Text = "Todo el histórico",
            .ForeColor = Color.WhiteSmoke,
            .AutoSize = True,
            .Location = New Point(540, 65),
            .Checked = True
        }

        dtpDesde = New DateTimePicker With {
            .Format = DateTimePickerFormat.Short,
            .Location = New Point(700, 63),
            .Enabled = False,
            .Width = 110
        }

        dtpHasta = New DateTimePicker With {
            .Format = DateTimePickerFormat.Short,
            .Location = New Point(820, 63),
            .Enabled = False,
            .Width = 110
        }

        PanelHeader.Controls.Add(cboAlcance)
        PanelHeader.Controls.Add(chkHistorico)
        PanelHeader.Controls.Add(dtpDesde)
        PanelHeader.Controls.Add(dtpHasta)
    End Sub

    Private Sub CargarHistorial()
        Try
            Dim docActual = db.Mae_Documento.Find(_idDocumento)

            If docActual Is Nothing Then
                Notifier.[Error](Me, "El documento no existe.")
                Return
            End If

            lblNumero.Text = docActual.Cat_TipoDocumento.Codigo & " " & docActual.NumeroOficial
            lblAsunto.Text = docActual.Asunto

            Dim movimientosQuery = db.Tra_Movimiento.AsQueryable()
            Dim alcanceSeleccionado = CType(cboAlcance.SelectedIndex, AlcanceHistorial)

            Select Case alcanceSeleccionado
                Case AlcanceHistorial.DocumentoSeleccionado
                    movimientosQuery = movimientosQuery.Where(Function(m) m.IdDocumento = _idDocumento)

                Case AlcanceHistorial.FamiliaPadreAdjuntos
                    Dim idsFamiliaPadreAdjuntos = ObtenerIdsFamiliaPadreAdjuntos(docActual.IdDocumento)
                    movimientosQuery = movimientosQuery.Where(Function(m) idsFamiliaPadreAdjuntos.Contains(m.IdDocumento))

                Case Else
                    Dim guidFamilia = docActual.IdHiloConversacion
                    movimientosQuery = movimientosQuery.Where(Function(m) m.Mae_Documento.IdHiloConversacion = guidFamilia)
            End Select

            If Not chkHistorico.Checked Then
                Dim fechaDesde = dtpDesde.Value.Date
                Dim fechaHasta = dtpHasta.Value.Date.AddDays(1)
                movimientosQuery = movimientosQuery.Where(Function(m) m.FechaMovimiento >= fechaDesde AndAlso m.FechaMovimiento < fechaHasta)
            End If

            Dim historialDatos = (From m In movimientosQuery
                                  Order By m.FechaMovimiento Ascending
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
                                  }).ToList()

            Dim historialVisual = historialDatos.Select(Function(x) New With {
                .Fecha = x.Fecha,
                .ID_Doc = x.ID_Doc,
                .Documento = x.Tipo & " " & x.Numero & If(x.Cant_Hijos > 0, " (" & x.Cant_Hijos & ")", ""),
                .Origen = x.Origen,
                .Destino = x.Destino,
                .Estado = x.Estado,
                .Observacion = x.Observacion,
                .Responsable = x.Responsable
            }).ToList()

            dgvHistoria.DataSource = historialVisual

            If dgvHistoria.Columns.Count > 0 Then
                With dgvHistoria
                    .Columns("Fecha").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
                    .Columns("Fecha").Width = 110
                    .Columns("ID_Doc").Visible = False

                    .Columns("Documento").Width = 160
                    .Columns("Documento").HeaderText = "Pieza / Doc"

                    .Columns("Origen").Width = 140
                    .Columns("Destino").Width = 140
                    .Columns("Estado").Width = 80
                    .Columns("Responsable").Width = 70
                    .Columns("Observacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

                    .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
                    .GridColor = Color.WhiteSmoke
                End With
            End If

            ResaltarDocumentoActual()
            dgvHistoria.ClearSelection()
            dgvHistoria.CurrentCell = Nothing

        Catch ex As Exception
            Notifier.[Error](Me, "Error al leer la trazabilidad: " & ex.Message)
        End Try
    End Sub

    Private Function ObtenerIdsFamiliaPadreAdjuntos(idDocumento As Long) As List(Of Long)
        Dim ids As New HashSet(Of Long)()
        Dim pendientes As New Queue(Of Long)()

        Dim docBase = db.Mae_Documento.Find(idDocumento)
        If docBase Is Nothing Then
            Return New List(Of Long) From {idDocumento}
        End If

        Dim idRaiz = docBase.IdDocumento
        While docBase.IdDocumentoPadre.HasValue
            idRaiz = docBase.IdDocumentoPadre.Value
            docBase = db.Mae_Documento.Find(idRaiz)
            If docBase Is Nothing Then
                Exit While
            End If
        End While

        pendientes.Enqueue(idRaiz)
        ids.Add(idRaiz)

        While pendientes.Count > 0
            Dim idActual = pendientes.Dequeue()
            Dim hijos = db.Mae_Documento.Where(Function(d) d.IdDocumentoPadre = idActual).Select(Function(d) d.IdDocumento).ToList()

            For Each idHijo In hijos
                If ids.Add(idHijo) Then
                    pendientes.Enqueue(idHijo)
                End If
            Next
        End While

        Return ids.ToList()
    End Function

    Private Sub dgvHistoria_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvHistoria.DataBindingComplete
        ResaltarDocumentoActual()
    End Sub

    Private Sub ResaltarDocumentoActual()
        For Each row As DataGridViewRow In dgvHistoria.Rows
            Dim valorId = row.Cells("ID_Doc").Value
            If valorId IsNot Nothing AndAlso valorId IsNot DBNull.Value AndAlso CLng(valorId) = _idDocumento Then
                row.DefaultCellStyle.BackColor = Color.LightYellow
                row.DefaultCellStyle.ForeColor = Color.Black
                row.DefaultCellStyle.SelectionBackColor = Color.Gold
                row.DefaultCellStyle.SelectionForeColor = Color.Black
                row.DefaultCellStyle.Font = New Font(dgvHistoria.Font, FontStyle.Bold)
            Else
                row.DefaultCellStyle.BackColor = Color.White
                row.DefaultCellStyle.ForeColor = dgvHistoria.DefaultCellStyle.ForeColor
                row.DefaultCellStyle.SelectionBackColor = dgvHistoria.DefaultCellStyle.SelectionBackColor
                row.DefaultCellStyle.SelectionForeColor = dgvHistoria.DefaultCellStyle.SelectionForeColor
                row.DefaultCellStyle.Font = dgvHistoria.Font
            End If
        Next
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Using dialog As New PrintDialog()
            dialog.Document = _printDocument
            If dialog.ShowDialog(Me) = DialogResult.OK Then
                _printBitmap = New Bitmap(Me.ClientSize.Width, Me.ClientSize.Height)
                Me.DrawToBitmap(_printBitmap, New Rectangle(Point.Empty, Me.ClientSize))
                _printDocument.DocumentName = $"Historial ({cboAlcance.Text}) - {lblNumero.Text}"
                _printDocument.DefaultPageSettings.Landscape = Me.ClientSize.Width > Me.ClientSize.Height
                _printDocument.Print()
            End If
        End Using
    End Sub

    Private Sub cboAlcance_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAlcance.SelectedIndexChanged
        If IsHandleCreated Then
            CargarHistorial()
        End If
    End Sub

    Private Sub chkHistorico_CheckedChanged(sender As Object, e As EventArgs) Handles chkHistorico.CheckedChanged
        Dim habilitarRango = Not chkHistorico.Checked
        dtpDesde.Enabled = habilitarRango
        dtpHasta.Enabled = habilitarRango

        If IsHandleCreated Then
            CargarHistorial()
        End If
    End Sub

    Private Sub dtpDesde_ValueChanged(sender As Object, e As EventArgs) Handles dtpDesde.ValueChanged
        If Not chkHistorico.Checked AndAlso IsHandleCreated Then
            CargarHistorial()
        End If
    End Sub

    Private Sub dtpHasta_ValueChanged(sender As Object, e As EventArgs) Handles dtpHasta.ValueChanged
        If Not chkHistorico.Checked AndAlso IsHandleCreated Then
            CargarHistorial()
        End If
    End Sub

    Private Sub PrintDocument_PrintPage(sender As Object, e As PrintPageEventArgs) Handles _printDocument.PrintPage
        If _printBitmap Is Nothing Then
            e.HasMorePages = False
            Return
        End If

        Dim marginBounds = e.MarginBounds
        Dim scale = Math.Min(marginBounds.Width / _printBitmap.Width, marginBounds.Height / _printBitmap.Height)
        Dim drawWidth = CInt(_printBitmap.Width * scale)
        Dim drawHeight = CInt(_printBitmap.Height * scale)
        Dim drawX = marginBounds.X + CInt((marginBounds.Width - drawWidth) / 2)
        Dim drawY = marginBounds.Y + CInt((marginBounds.Height - drawHeight) / 2)

        e.Graphics.DrawImage(_printBitmap, New Rectangle(drawX, drawY, drawWidth, drawHeight))
        e.HasMorePages = False
    End Sub

End Class
