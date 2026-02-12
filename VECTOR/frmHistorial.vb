Imports System.Data.Entity
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.IO
Imports System.Linq
Imports System.Text

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

    Public Sub New(idDoc As Long)
        InitializeComponent()
        _idDocumento = idDoc
    End Sub

    Private Sub frmHistorial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboAlcance.SelectedIndex = CInt(AlcanceHistorial.DocumentoYFamiliaHistorico)
        AppTheme.Aplicar(Me)
        CargarHistorial()
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

    Private Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click
        If dgvHistoria.Rows.Count = 0 Then
            Notifier.Info(Me, "No hay datos para exportar.")
            Return
        End If

        Using dialog As New SaveFileDialog()
            dialog.Filter = "Archivo CSV (*.csv)|*.csv"
            dialog.FileName = ObtenerNombreBaseArchivo() & ".csv"
            dialog.Title = "Guardar historial en Excel (CSV)"

            If dialog.ShowDialog(Me) <> DialogResult.OK Then
                Return
            End If

            Try
                ExportarCsv(dialog.FileName)
                Notifier.Success(Me, "Archivo generado: " & Path.GetFileName(dialog.FileName))
            Catch ex As Exception
                Notifier.Error(Me, "No se pudo exportar el archivo CSV: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub btnExportPdf_Click(sender As Object, e As EventArgs) Handles btnExportPdf.Click
        If dgvHistoria.Rows.Count = 0 Then
            Notifier.Info(Me, "No hay datos para exportar.")
            Return
        End If

        Using dialog As New SaveFileDialog()
            dialog.Filter = "Archivo PDF (*.pdf)|*.pdf"
            dialog.FileName = ObtenerNombreBaseArchivo() & ".pdf"
            dialog.Title = "Guardar historial en PDF"

            If dialog.ShowDialog(Me) <> DialogResult.OK Then
                Return
            End If

            Try
                ExportarPdf(dialog.FileName)
                Notifier.Success(Me, "Archivo generado: " & Path.GetFileName(dialog.FileName))
            Catch ex As Exception
                Notifier.Error(Me, "No se pudo exportar el archivo PDF: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub ExportarCsv(filePath As String)
        Dim sb As New StringBuilder()
        Dim columnas = dgvHistoria.Columns.Cast(Of DataGridViewColumn)().Where(Function(c) c.Visible).OrderBy(Function(c) c.DisplayIndex).ToList()

        sb.AppendLine(String.Join(";", columnas.Select(Function(c) EscaparCsv(c.HeaderText))))

        For Each row As DataGridViewRow In dgvHistoria.Rows
            If row.IsNewRow Then Continue For

            Dim valores = columnas.Select(Function(col)
                                              Dim value = row.Cells(col.Name).Value
                                              If value Is Nothing OrElse value Is DBNull.Value Then
                                                  Return ""
                                              End If

                                              If TypeOf value Is DateTime Then
                                                  Return EscaparCsv(CType(value, DateTime).ToString("dd/MM/yyyy HH:mm"))
                                              End If

                                              Return EscaparCsv(value.ToString())
                                          End Function)

            sb.AppendLine(String.Join(";", valores))
        Next

        File.WriteAllText(filePath, sb.ToString(), New UTF8Encoding(True))
    End Sub

    Private Function EscaparCsv(value As String) As String
        Dim result = If(value, String.Empty).Replace("""", """""")
        Return """" & result & """"
    End Function

    Private Sub ExportarPdf(filePath As String)
        Dim anchoOriginal = dgvHistoria.Width
        Dim altoOriginal = dgvHistoria.Height

        Try
            dgvHistoria.ClearSelection()
            dgvHistoria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            Dim totalWidth = dgvHistoria.Columns.Cast(Of DataGridViewColumn)().Where(Function(c) c.Visible).Sum(Function(c) c.Width)
            dgvHistoria.Width = Math.Max(totalWidth + 30, dgvHistoria.Width)
            dgvHistoria.Height = dgvHistoria.ColumnHeadersHeight + (dgvHistoria.RowTemplate.Height * Math.Max(dgvHistoria.Rows.Count, 1)) + 20

            Using bmp As New Bitmap(dgvHistoria.Width, dgvHistoria.Height)
                dgvHistoria.DrawToBitmap(bmp, New Rectangle(0, 0, bmp.Width, bmp.Height))
                GuardarBitmapComoPdf(bmp, filePath)
            End Using
        Finally
            dgvHistoria.Width = anchoOriginal
            dgvHistoria.Height = altoOriginal
            dgvHistoria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        End Try
    End Sub

    Private Sub GuardarBitmapComoPdf(bmp As Bitmap, filePath As String)
        Dim imageBytes As Byte()
        Using imageStream As New MemoryStream()
            bmp.Save(imageStream, Imaging.ImageFormat.Jpeg)
            imageBytes = imageStream.ToArray()
        End Using

        Dim pageWidth As Integer = 842
        Dim pageHeight As Integer = 595
        Dim margin As Integer = 20
        Dim maxWidth As Integer = pageWidth - (margin * 2)
        Dim maxHeight As Integer = pageHeight - (margin * 2)

        Dim scale = Math.Min(maxWidth / CDbl(bmp.Width), maxHeight / CDbl(bmp.Height))
        Dim drawWidth = CInt(bmp.Width * scale)
        Dim drawHeight = CInt(bmp.Height * scale)
        Dim drawX = margin + CInt((maxWidth - drawWidth) / 2)
        Dim drawY = margin + CInt((maxHeight - drawHeight) / 2)

        Dim imageObjNumber As Integer = 1
        Dim contentObjNumber As Integer = 2
        Dim pageObjNumber As Integer = 3
        Dim pagesObjNumber As Integer = 4
        Dim catalogObjNumber As Integer = 5

        Dim imageObj As String = $"<< /Type /XObject /Subtype /Image /Width {bmp.Width} /Height {bmp.Height} /ColorSpace /DeviceRGB /BitsPerComponent 8 /Filter /DCTDecode /Length {imageBytes.Length} >>"
        Dim contentText As String = $"q{Environment.NewLine}{drawWidth} 0 0 {drawHeight} {drawX} {drawY} cm{Environment.NewLine}/Im0 Do{Environment.NewLine}Q"
        Dim contentBytes = Encoding.ASCII.GetBytes(contentText)
        Dim contentObj As String = $"<< /Length {contentBytes.Length} >>"
        Dim pageObj As String = $"<< /Type /Page /Parent {pagesObjNumber} 0 R /MediaBox [0 0 {pageWidth} {pageHeight}] /Resources << /XObject << /Im0 {imageObjNumber} 0 R >> >> /Contents {contentObjNumber} 0 R >>"
        Dim pagesObj As String = $"<< /Type /Pages /Count 1 /Kids [{pageObjNumber} 0 R] >>"
        Dim catalogObj As String = $"<< /Type /Catalog /Pages {pagesObjNumber} 0 R >>"

        Using fs As New FileStream(filePath, FileMode.Create, FileAccess.Write)
            Dim offsets As New List(Of Integer)()
            Dim enc = Encoding.ASCII

            Dim header = "%PDF-1.4" & Environment.NewLine
            fs.Write(enc.GetBytes(header), 0, header.Length)

            offsets.Add(CInt(fs.Position))
            EscribirObjetoPdf(fs, imageObjNumber, imageObj, imageBytes)

            offsets.Add(CInt(fs.Position))
            EscribirObjetoPdf(fs, contentObjNumber, contentObj, contentBytes)

            offsets.Add(CInt(fs.Position))
            EscribirObjetoPdf(fs, pageObjNumber, pageObj)

            offsets.Add(CInt(fs.Position))
            EscribirObjetoPdf(fs, pagesObjNumber, pagesObj)

            offsets.Add(CInt(fs.Position))
            EscribirObjetoPdf(fs, catalogObjNumber, catalogObj)

            Dim xrefPos = fs.Position
            Dim xrefHeader = $"xref{Environment.NewLine}0 {offsets.Count + 1}{Environment.NewLine}"
            fs.Write(enc.GetBytes(xrefHeader), 0, xrefHeader.Length)
            Dim freeEntry = "0000000000 65535 f " & Environment.NewLine
            fs.Write(enc.GetBytes(freeEntry), 0, freeEntry.Length)

            For Each offset In offsets
                Dim entry = offset.ToString("D10") & " 00000 n " & Environment.NewLine
                fs.Write(enc.GetBytes(entry), 0, entry.Length)
            Next

            Dim trailer = $"trailer{Environment.NewLine}<< /Size {offsets.Count + 1} /Root {catalogObjNumber} 0 R >>{Environment.NewLine}startxref{Environment.NewLine}{xrefPos}{Environment.NewLine}%%EOF"
            fs.Write(enc.GetBytes(trailer), 0, trailer.Length)
        End Using
    End Sub

    Private Sub EscribirObjetoPdf(fs As FileStream, numero As Integer, cuerpo As String, Optional streamBytes As Byte() = Nothing)
        Dim enc = Encoding.ASCII
        Dim inicio = $"{numero} 0 obj{Environment.NewLine}{cuerpo}"
        fs.Write(enc.GetBytes(inicio), 0, inicio.Length)

        If streamBytes IsNot Nothing Then
            Dim streamInicio = Environment.NewLine & "stream" & Environment.NewLine
            fs.Write(enc.GetBytes(streamInicio), 0, streamInicio.Length)
            fs.Write(streamBytes, 0, streamBytes.Length)
            Dim streamFin = Environment.NewLine & "endstream"
            fs.Write(enc.GetBytes(streamFin), 0, streamFin.Length)
        End If

        Dim fin = Environment.NewLine & "endobj" & Environment.NewLine
        fs.Write(enc.GetBytes(fin), 0, fin.Length)
    End Sub

    Private Function ObtenerNombreBaseArchivo() As String
        Dim nombre = $"historial_{lblNumero.Text}_{DateTime.Now:yyyyMMdd_HHmm}"
        For Each invalid In Path.GetInvalidFileNameChars()
            nombre = nombre.Replace(invalid, "_"c)
        Next

        Return nombre
    End Function

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
        dtpDesde.Visible = habilitarRango
        dtpHasta.Visible = habilitarRango
        lblDesde.Visible = habilitarRango
        lblHasta.Visible = habilitarRango

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
