Imports System.Data.Entity
Imports System.Linq
Imports System.Reflection
Imports System.Threading.Tasks

Public Class frmRenovacionesArt120

    Private Const DiasAnticipacionAlerta As Integer = 30

    Private Class SalidaGridDto
        Public Property IdSalida As Integer
        Public Property IdRecluso As Integer
        Public Property Recluso As String
        Public Property LugarTrabajo As String
        Public Property FechaInicio As Date
        Public Property FechaVencimiento As Date
        Public Property DiasRestantes As Integer
        Public Property Estado As String
        Public Property IdDocumentoRespaldo As Nullable(Of Long)
        Public Property Activo As Boolean
        Public Property Observaciones As String
    End Class

    Private _listaOriginal As New List(Of SalidaGridDto)
    Private _idSalidaEditando As Nullable(Of Integer)
    Private _idReclusoSeleccionado As Nullable(Of Integer)
    Private _cargandoDocumentos As Boolean

    Private Async Sub frmRenovacionesArt120_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        UIUtils.SetPlaceholder(txtBuscar, "Buscar por recluso, lugar, estado o documento...")

        Dim typeDGV As Type = dgvSalidas.GetType()
        Dim propertyInfo As PropertyInfo = typeDGV.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        propertyInfo.SetValue(dgvSalidas, True, Nothing)

        LimpiarEditor()
        Await CargarSalidasAsync()
    End Sub

    Private Async Function CargarSalidasAsync() As Task
        Try
            Using uow As New UnitOfWork()
                Dim repo = uow.Repository(Of Tra_SalidasLaborales)()
                Dim hoy = DateTime.Today

                Dim consulta = repo.GetQueryable() _
                    .Include("Mae_Reclusos")

                If chkSoloActivas.Checked Then
                    consulta = consulta.Where(Function(s) Not s.Activo.HasValue OrElse s.Activo.Value)
                End If

                Dim datos = Await consulta _
                    .OrderBy(Function(s) s.FechaVencimiento) _
                    .Select(Function(s) New With {
                        .IdSalida = s.IdSalida,
                        .IdRecluso = s.IdRecluso,
                        .Recluso = s.Mae_Reclusos.NombreCompleto,
                        .LugarTrabajo = s.LugarTrabajo,
                        .FechaInicio = s.FechaInicio,
                        .FechaVencimiento = s.FechaVencimiento,
                        .DiasRestantes = DbFunctions.DiffDays(hoy, s.FechaVencimiento),
                        .IdDocumentoRespaldo = s.IdDocumentoRespaldo,
                        .Activo = s.Activo,
                        .Observaciones = s.Observaciones
                    }).ToListAsync()

                _listaOriginal = datos.Select(Function(x)
                                                  Dim dias = If(x.DiasRestantes, 0)
                                                  Dim estado As String
                                                  If Not x.Activo.GetValueOrDefault(True) Then
                                                      estado = "INACTIVA"
                                                  ElseIf dias < 0 Then
                                                      estado = "VENCIDA"
                                                  ElseIf dias <= DiasAnticipacionAlerta Then
                                                      estado = "ALERTA"
                                                  Else
                                                      estado = "OK"
                                                  End If

                                                  Return New SalidaGridDto With {
                                                    .IdSalida = x.IdSalida,
                                                    .IdRecluso = x.IdRecluso,
                                                    .Recluso = If(x.Recluso, ""),
                                                    .LugarTrabajo = If(x.LugarTrabajo, ""),
                                                    .FechaInicio = x.FechaInicio,
                                                    .FechaVencimiento = x.FechaVencimiento,
                                                    .DiasRestantes = dias,
                                                    .Estado = estado,
                                                    .IdDocumentoRespaldo = x.IdDocumentoRespaldo,
                                                    .Activo = x.Activo.GetValueOrDefault(True),
                                                    .Observaciones = If(x.Observaciones, "")
                                                 }
                                              End Function).ToList()
            End Using

            AplicarFiltro()
        Catch ex As Exception
            Toast.Show(Me, "Error al cargar salidas laborales: " & ex.Message, ToastType.Error)
        End Try
    End Function

    Private Sub AplicarFiltro()
        Dim texto = txtBuscar.Text.Trim().ToUpper()
        Dim lista = _listaOriginal.AsEnumerable()

        If Not String.IsNullOrWhiteSpace(texto) Then
            Dim palabras = texto.Split(" "c)
            lista = lista.Where(Function(s)
                                    Dim baseTexto = $"{s.Recluso} {s.LugarTrabajo} {s.Estado} {s.Observaciones} {If(s.IdDocumentoRespaldo.HasValue, s.IdDocumentoRespaldo.Value.ToString(), "")}".ToUpper()
                                    Return palabras.All(Function(p) String.IsNullOrWhiteSpace(p) OrElse baseTexto.Contains(p))
                                End Function)
        End If

        Dim final = lista.ToList()
        dgvSalidas.DataSource = Nothing
        dgvSalidas.AutoGenerateColumns = True
        dgvSalidas.DataSource = final
        DisenarGrilla()
        Resumir(final)
    End Sub

    Private Sub DisenarGrilla()
        If dgvSalidas.Columns.Count = 0 Then Return

        dgvSalidas.Columns("IdRecluso").Visible = False
        dgvSalidas.Columns("Observaciones").Visible = False
        dgvSalidas.Columns("Activo").Visible = False

        dgvSalidas.Columns("IdSalida").HeaderText = "ID"
        dgvSalidas.Columns("IdSalida").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        dgvSalidas.Columns("Recluso").HeaderText = "Recluso"
        dgvSalidas.Columns("Recluso").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvSalidas.Columns("Recluso").FillWeight = 140

        dgvSalidas.Columns("LugarTrabajo").HeaderText = "Lugar trabajo"
        dgvSalidas.Columns("LugarTrabajo").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvSalidas.Columns("LugarTrabajo").FillWeight = 150

        dgvSalidas.Columns("FechaInicio").HeaderText = "Inicio"
        dgvSalidas.Columns("FechaInicio").DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvSalidas.Columns("FechaInicio").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        dgvSalidas.Columns("FechaVencimiento").HeaderText = "Vencimiento"
        dgvSalidas.Columns("FechaVencimiento").DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvSalidas.Columns("FechaVencimiento").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        dgvSalidas.Columns("DiasRestantes").HeaderText = "Días"
        dgvSalidas.Columns("DiasRestantes").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        dgvSalidas.Columns("Estado").HeaderText = "Estado"
        dgvSalidas.Columns("Estado").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        dgvSalidas.Columns("IdDocumentoRespaldo").HeaderText = "Doc respaldo"
        dgvSalidas.Columns("IdDocumentoRespaldo").AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
    End Sub

    Private Sub Resumir(lista As List(Of SalidaGridDto))
        ' 1. Para el total usamos la propiedad nativa de la lista (es más rápido)
        Dim total = lista.Count

        ' 2. Para contar con condiciones, usamos .Where(...).Count() 
        ' Esto evita la ambigüedad que causa el error en VB.NET
        Dim vencidas = lista.Where(Function(s) s.Estado = "VENCIDA").Count()
        Dim alertas = lista.Where(Function(s) s.Estado = "ALERTA").Count()

        ' 3. Asignación al Label con interpolación de cadenas
        lblResumen.Text = $"Total: {total} | Vencidas: {vencidas} | En alerta ({DiasAnticipacionAlerta} días): {alertas}"
    End Sub

    Private Function ObtenerSeleccionActual() As SalidaGridDto
        If dgvSalidas.SelectedRows.Count = 0 Then Return Nothing
        Return TryCast(dgvSalidas.SelectedRows(0).DataBoundItem, SalidaGridDto)
    End Function

    Private Sub dgvSalidas_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSalidas.SelectionChanged
        Dim selected = ObtenerSeleccionActual()
        btnEditar.Enabled = (selected IsNot Nothing)
        btnDesactivar.Enabled = (selected IsNot Nothing AndAlso selected.Activo)
        btnReactivar.Enabled = (selected IsNot Nothing AndAlso Not selected.Activo)
        btnAbrirDocumento.Enabled = (selected IsNot Nothing AndAlso selected.IdDocumentoRespaldo.HasValue)
    End Sub

    Private Sub dgvSalidas_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles dgvSalidas.RowPrePaint
        If e.RowIndex < 0 Then Return
        Dim row = dgvSalidas.Rows(e.RowIndex)
        Dim data = TryCast(row.DataBoundItem, SalidaGridDto)
        If data Is Nothing Then Return

        If data.Estado = "VENCIDA" Then
            row.DefaultCellStyle.BackColor = Color.MistyRose
            row.DefaultCellStyle.ForeColor = Color.DarkRed
        ElseIf data.Estado = "ALERTA" Then
            row.DefaultCellStyle.BackColor = Color.LightYellow
            row.DefaultCellStyle.ForeColor = Color.DarkGoldenrod
        ElseIf data.Estado = "INACTIVA" Then
            row.DefaultCellStyle.BackColor = Color.Gainsboro
            row.DefaultCellStyle.ForeColor = Color.DimGray
        Else
            row.DefaultCellStyle.BackColor = Color.White
            row.DefaultCellStyle.ForeColor = Color.Black
        End If
    End Sub

    Private Sub btnNueva_Click(sender As Object, e As EventArgs) Handles btnNueva.Click
        LimpiarEditor()
        txtLugarTrabajo.Focus()
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Dim sel = ObtenerSeleccionActual()
        If sel Is Nothing Then
            Toast.Show(Me, "Seleccione una salida para editar.", ToastType.Warning)
            Return
        End If

        _idSalidaEditando = sel.IdSalida
        _idReclusoSeleccionado = sel.IdRecluso

        txtRecluso.Text = sel.Recluso
        lblIdRecluso.Text = "ID: " & sel.IdRecluso
        txtLugarTrabajo.Text = sel.LugarTrabajo
        dtpInicio.Value = sel.FechaInicio
        dtpVencimiento.Value = sel.FechaVencimiento
        chkActivoRegistro.Checked = sel.Activo
        txtObservaciones.Text = sel.Observaciones
        txtLugarTrabajo.Focus()

        CargarDocumentosRespaldo(sel.IdRecluso, sel.Recluso, sel.IdDocumentoRespaldo)
    End Sub

    Private Async Sub btnBuscarRecluso_Click(sender As Object, e As EventArgs) Handles btnBuscarRecluso.Click
        Dim f As New frmBuscadorReclusos()
        If f.ShowDialog(Me) <> DialogResult.OK Then Return

        Using uow As New UnitOfWork()
            Dim repo = uow.Repository(Of Mae_Reclusos)()
            Dim idRecluso = f.ResultadoIdRecluso
            Dim recluso As Mae_Reclusos = Nothing

            If idRecluso.HasValue Then
                recluso = repo.GetQueryable().FirstOrDefault(Function(r) r.IdRecluso = idRecluso.Value)
            ElseIf Not String.IsNullOrWhiteSpace(f.ResultadoFormateado) Then
                Dim nombre = f.ResultadoFormateado
                recluso = repo.GetQueryable().FirstOrDefault(Function(r) r.NombreCompleto = nombre)
            End If
            If recluso Is Nothing Then
                Toast.Show(Me, "No se encontró el recluso seleccionado.", ToastType.Warning)
                Return
            End If

            _idReclusoSeleccionado = recluso.IdRecluso
            txtRecluso.Text = recluso.NombreCompleto
            lblIdRecluso.Text = "ID: " & recluso.IdRecluso
        End Using

        Await CargarDocumentosRespaldoAsync(_idReclusoSeleccionado.Value, txtRecluso.Text.Trim(), Nothing)
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Not ValidarEditor() Then Return

        Try
            Using uow As New UnitOfWork()
                Dim repo = uow.Repository(Of Tra_SalidasLaborales)()
                Dim entidad As Tra_SalidasLaborales

                If _idSalidaEditando.HasValue Then
                    entidad = Await repo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(s) s.IdSalida = _idSalidaEditando.Value)
                    If entidad Is Nothing Then
                        Toast.Show(Me, "La salida seleccionada ya no existe.", ToastType.Warning)
                        Return
                    End If
                Else
                    entidad = New Tra_SalidasLaborales()
                    repo.Add(entidad)
                End If

                entidad.IdRecluso = _idReclusoSeleccionado.Value
                entidad.LugarTrabajo = txtLugarTrabajo.Text.Trim()
                entidad.FechaInicio = dtpInicio.Value.Date
                entidad.FechaVencimiento = dtpVencimiento.Value.Date
                entidad.IdDocumentoRespaldo = ParseNullableLong(cboDocumentoRespaldo.SelectedValue)
                entidad.Activo = chkActivoRegistro.Checked
                entidad.Observaciones = If(String.IsNullOrWhiteSpace(txtObservaciones.Text), Nothing, txtObservaciones.Text.Trim())

                Await uow.CommitAsync()
            End Using

            Toast.Show(Me, "Datos de renovación guardados correctamente.", ToastType.Success)
            LimpiarEditor()
            Await CargarSalidasAsync()
        Catch ex As Exception
            Toast.Show(Me, "No se pudo guardar la salida: " & ex.Message, ToastType.Error)
        End Try
    End Sub

    Private Function ValidarEditor() As Boolean
        If Not _idReclusoSeleccionado.HasValue Then
            Toast.Show(Me, "Debe seleccionar una persona privada de libertad.", ToastType.Warning)
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtLugarTrabajo.Text) Then
            Toast.Show(Me, "Ingrese el lugar de trabajo.", ToastType.Warning)
            Return False
        End If

        If dtpVencimiento.Value.Date < dtpInicio.Value.Date Then
            Toast.Show(Me, "La fecha de vencimiento no puede ser menor a la fecha de inicio.", ToastType.Warning)
            Return False
        End If

        Dim idDoc = ParseNullableLong(cboDocumentoRespaldo.SelectedValue)
        If cboDocumentoRespaldo.SelectedIndex > 0 AndAlso Not idDoc.HasValue Then
            Toast.Show(Me, "El ID de documento respaldo debe ser numérico.", ToastType.Warning)
            Return False
        End If

        If _idSalidaEditando.HasValue AndAlso Not chkActivoRegistro.Checked AndAlso chkSoloActivas.Checked Then
            Toast.Show(Me, "Al guardar, el registro quedará oculto porque tiene activado el filtro 'Solo activas'.", ToastType.Info)
        End If

        Return True
    End Function

    Private Async Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        Await CambiarEstadoSeleccionAsync(False)
    End Sub

    Private Async Sub btnReactivar_Click(sender As Object, e As EventArgs) Handles btnReactivar.Click
        Await CambiarEstadoSeleccionAsync(True)
    End Sub

    Private Async Function CambiarEstadoSeleccionAsync(nuevoEstado As Boolean) As Task
        Dim sel = ObtenerSeleccionActual()
        If sel Is Nothing Then
            Toast.Show(Me, "Seleccione una salida.", ToastType.Warning)
            Return
        End If

        Try
            Using uow As New UnitOfWork()
                Dim repo = uow.Repository(Of Tra_SalidasLaborales)()
                Dim entidad = Await repo.GetQueryable(tracking:=True).FirstOrDefaultAsync(Function(s) s.IdSalida = sel.IdSalida)
                If entidad Is Nothing Then
                    Toast.Show(Me, "La salida seleccionada ya no existe.", ToastType.Warning)
                    Return
                End If

                entidad.Activo = nuevoEstado
                Await uow.CommitAsync()
            End Using

            Toast.Show(Me, If(nuevoEstado, "Salida reactivada.", "Salida desactivada."), ToastType.Success)
            Await CargarSalidasAsync()
        Catch ex As Exception
            Toast.Show(Me, "No se pudo actualizar el estado: " & ex.Message, ToastType.Error)
        End Try
    End Function

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LimpiarEditor()
    End Sub

    Private Async Sub chkSoloActivas_CheckedChanged(sender As Object, e As EventArgs) Handles chkSoloActivas.CheckedChanged
        Await CargarSalidasAsync()
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        AplicarFiltro()
    End Sub

    Private Sub btnAbrirDocumento_Click(sender As Object, e As EventArgs) Handles btnAbrirDocumento.Click
        Dim sel = ObtenerSeleccionActual()
        If sel Is Nothing OrElse Not sel.IdDocumentoRespaldo.HasValue Then
            Toast.Show(Me, "La salida seleccionada no tiene documento respaldo.", ToastType.Warning)
            Return
        End If

        Dim f As New frmDetalleDocumento(sel.IdDocumentoRespaldo.Value)
        ShowFormInMdi(Me, f)
    End Sub

    Private Sub LimpiarEditor()
        _idSalidaEditando = Nothing
        _idReclusoSeleccionado = Nothing

        txtRecluso.Clear()
        lblIdRecluso.Text = "ID: (ninguno)"
        txtLugarTrabajo.Clear()
        dtpInicio.Value = Date.Today
        dtpVencimiento.Value = Date.Today.AddMonths(1)
        chkActivoRegistro.Checked = True
        CargarDocumentosRespaldoVacio()
        txtObservaciones.Clear()
    End Sub

    Private Function ParseNullableLong(value As Object) As Nullable(Of Long)
        Dim txt = If(value, "").ToString()
        If String.IsNullOrWhiteSpace(txt) Then Return Nothing

        Dim n As Long
        If Long.TryParse(txt.Trim(), n) Then
            Return n
        End If

        Return Nothing
    End Function

    Private Sub CargarDocumentosRespaldoVacio()
        _cargandoDocumentos = True
        cboDocumentoRespaldo.DisplayMember = "Text"
        cboDocumentoRespaldo.ValueMember = "Value"
        cboDocumentoRespaldo.DataSource = New List(Of Object) From {
            New With {.Text = "(sin respaldo)", .Value = ""}
        }
        cboDocumentoRespaldo.SelectedIndex = 0
        _cargandoDocumentos = False
    End Sub

    Private Sub CargarDocumentosRespaldo(idRecluso As Integer, nombreRecluso As String, idSeleccionado As Nullable(Of Long))
        CargarDocumentosRespaldoAsync(idRecluso, nombreRecluso, idSeleccionado).GetAwaiter().GetResult()
    End Sub

    Private Async Function CargarDocumentosRespaldoAsync(idRecluso As Integer, nombreRecluso As String, idSeleccionado As Nullable(Of Long)) As Task
        Try
            _cargandoDocumentos = True

            Using uow As New UnitOfWork()
                Dim repoDocs = uow.Repository(Of Mae_Documento)()
                Dim repoSalidas = uow.Repository(Of Tra_SalidasLaborales)()

                Dim idsDesdeSalidas = Await repoSalidas.GetQueryable(tracking:=False) _
                    .Where(Function(s) s.IdRecluso = idRecluso AndAlso s.IdDocumentoRespaldo.HasValue) _
                    .Select(Function(s) s.IdDocumentoRespaldo.Value) _
                    .Distinct() _
                    .ToListAsync()

                Dim nombre = nombreRecluso.Trim()
                Dim docsPorTexto = Await repoDocs.GetQueryable(tracking:=False) _
                    .Where(Function(d) d.Asunto.Contains(nombre) OrElse d.Descripcion.Contains(nombre)) _
                    .OrderByDescending(Function(d) d.FechaCreacion) _
                    .Select(Function(d) New With {
                        .IdDocumento = d.IdDocumento,
                        .NumeroOficial = d.NumeroOficial,
                        .Asunto = d.Asunto,
                        .Fecha = d.FechaCreacion
                    }) _
                    .Take(50) _
                    .ToListAsync()

                Dim ids = idsDesdeSalidas.Union(docsPorTexto.Select(Function(d) CLng(d.IdDocumento))).Distinct().ToList()

                Dim docsIds = Await repoDocs.GetQueryable(tracking:=False) _
                    .Where(Function(d) ids.Contains(d.IdDocumento)) _
                    .OrderByDescending(Function(d) d.FechaCreacion) _
                    .Select(Function(d) New With {
                        .IdDocumento = d.IdDocumento,
                        .NumeroOficial = d.NumeroOficial,
                        .Asunto = d.Asunto,
                        .Fecha = d.FechaCreacion
                    }) _
                    .ToListAsync()

                Dim listaCombo = New List(Of Object) From {
                    New With {.Text = "(sin respaldo)", .Value = ""}
                }

                For Each doc In docsIds
                    Dim fechaTxt = If(doc.Fecha.HasValue, doc.Fecha.Value.ToString("dd/MM/yyyy"), "s/f")
                    Dim numero = If(String.IsNullOrWhiteSpace(doc.NumeroOficial), "S/N", doc.NumeroOficial)
                    Dim asunto = If(String.IsNullOrWhiteSpace(doc.Asunto), "(sin asunto)", doc.Asunto)
                    listaCombo.Add(New With {
                        .Text = $"{doc.IdDocumento} | {numero} | {fechaTxt} | {asunto}",
                        .Value = doc.IdDocumento.ToString()
                    })
                Next

                cboDocumentoRespaldo.DisplayMember = "Text"
                cboDocumentoRespaldo.ValueMember = "Value"
                cboDocumentoRespaldo.DataSource = listaCombo

                If idSeleccionado.HasValue Then
                    cboDocumentoRespaldo.SelectedValue = idSeleccionado.Value.ToString()
                Else
                    cboDocumentoRespaldo.SelectedIndex = 0
                End If
            End Using
        Catch ex As Exception
            CargarDocumentosRespaldoVacio()
            Toast.Show(Me, "No se pudieron cargar expedientes/documentos sugeridos: " & ex.Message, ToastType.Warning)
        Finally
            _cargandoDocumentos = False
        End Try
    End Function

    Private Sub btnRefrescarDocumentos_Click(sender As Object, e As EventArgs) Handles btnRefrescarDocumentos.Click
        If Not _idReclusoSeleccionado.HasValue Then
            Toast.Show(Me, "Primero seleccione una persona privada de libertad.", ToastType.Warning)
            Return
        End If

        CargarDocumentosRespaldo(_idReclusoSeleccionado.Value, txtRecluso.Text.Trim(), ParseNullableLong(cboDocumentoRespaldo.SelectedValue))
    End Sub

    Private Sub cboDocumentoRespaldo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDocumentoRespaldo.SelectedIndexChanged
        If _cargandoDocumentos Then Return
        Dim idDoc = ParseNullableLong(cboDocumentoRespaldo.SelectedValue)
        btnAbrirDocumento.Enabled = idDoc.HasValue
    End Sub
End Class
