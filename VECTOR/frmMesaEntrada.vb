Imports System.Data.Entity
Imports System.Diagnostics
Imports System.Drawing
Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks

Public Class frmMesaEntrada

    Private ReadOnly _unitOfWork As IUnitOfWork

    ' =======================================================
    ' VARIABLES DE ESTADO
    ' =======================================================
    Private _modoRespuesta As Boolean = False
    Private _idDocPadre As Long? = Nothing
    Private _guidHilo As Guid? = Nothing
    Private _asuntoPadre As String = ""

    ' Variable para saber si estamos EDITANDO un existente
    Private _idEdicion As Long? = Nothing

    ' Variable para "DIGITALIZAR" (Cargar a nombre de otro)
    Private _idOrigenForzado As Integer? = Nothing

    ' Variable para controlar si el número se genera solo
    Private _generacionAutomatica As Boolean = False

    ' Adjuntos digitales (almacenados en disco)
    Private _adjuntos As New List(Of AttachmentInfo)()

    ' Búsqueda de Organismo/Oficina (estilo frmPase)
    Private _todasLasOficinas As List(Of Cat_Oficina)
    Private _oficinaOrigenSeleccionada As Cat_Oficina
    Private _filtroVersionOrigen As Integer = 0
    Private _cerrandoFormulario As Boolean = False
    Private _seleccionandoOrigen As Boolean = False
    Private ReadOnly _lstSugerenciasOrigen As New ListBox()

    ' =======================================================
    ' CONSTRUCTORES (POLIMORFISMO)
    ' =======================================================

    ' 1. CONSTRUCTOR PARA NUEVO INGRESO
    Public Sub New()
        _unitOfWork = New UnitOfWork()
        InitializeComponent()
        _modoRespuesta = False
        _idEdicion = Nothing
        _idOrigenForzado = Nothing
    End Sub

    ' 2. CONSTRUCTOR PARA RESPUESTA / DIGITALIZACIÓN
    Public Sub New(idPadre As Long, guidHilo As Guid, asuntoOriginal As String, Optional idOrigenExterno As Integer? = Nothing)
        _unitOfWork = New UnitOfWork()
        InitializeComponent()
        _modoRespuesta = True
        _idDocPadre = idPadre
        _guidHilo = guidHilo
        _asuntoPadre = asuntoOriginal
        _idEdicion = Nothing
        _idOrigenForzado = idOrigenExterno
    End Sub

    ' 3. CONSTRUCTOR PARA EDICIÓN
    Public Sub New(idDocAEditar As Long)
        _unitOfWork = New UnitOfWork()
        InitializeComponent()
        _modoRespuesta = False
        _idEdicion = idDocAEditar
        _idOrigenForzado = Nothing
    End Sub

    ' =======================================================
    ' CARGA DE FORMULARIO
    ' =======================================================
    Private Sub frmMesaEntrada_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarBuscadorOrigen()
        CargarListas()
        UIUtils.SetPlaceholder(txtBuscarOrigen, "Escriba para buscar oficina...")

        If _idEdicion.HasValue Then
            ' MODO A: EDITAR (Update)
            CargarDatosParaEditar()
        ElseIf _modoRespuesta Then
            ' MODO B: RESPUESTA / ADJUNTO (Insert Hijo)
            PreparaModoRespuesta()
        Else
            ' MODO C: NUEVO (Insert Padre)
            LimpiarControles()
            Try
                ' Por defecto, el origen soy yo
                SeleccionarOrigenPorId(SesionGlobal.OficinaID)
            Catch
            End Try
        End If

        CargarAdjuntosIniciales()
    End Sub

    Private Sub CargarListas()
        ' A. CARGA TIPOS DE DOCUMENTO
        ' Primero configuramos QUÉ vamos a usar
        cboTipo.DisplayMember = "Nombre"
        cboTipo.ValueMember = "IdTipo"
        ' Al final cargamos la lista (esto dispara el evento, pero ya sabe qué es el Value)
        cboTipo.DataSource = _unitOfWork.Repository(Of Cat_TipoDocumento)().GetQueryable().ToList()
        cboTipo.SelectedIndex = -1

        ' B. CARGA OFICINAS
        _todasLasOficinas = _unitOfWork.Repository(Of Cat_Oficina)().GetQueryable().OrderBy(Function(o) o.Nombre).ToList()

        ' Combo oculto solo para compatibilidad de lógica existente
        cboOrigen.DisplayMember = "Nombre"
        cboOrigen.ValueMember = "IdOficina"
        cboOrigen.DataSource = _todasLasOficinas
        cboOrigen.SelectedIndex = -1
        _oficinaOrigenSeleccionada = Nothing
    End Sub

    Private Sub ConfigurarBuscadorOrigen()
        _lstSugerenciasOrigen.Name = "lstSugerenciasOrigen"
        _lstSugerenciasOrigen.Font = New Font("Segoe UI", 9.0!)
        _lstSugerenciasOrigen.BackColor = Color.White
        _lstSugerenciasOrigen.BorderStyle = BorderStyle.FixedSingle
        _lstSugerenciasOrigen.IntegralHeight = True
        _lstSugerenciasOrigen.DrawMode = DrawMode.OwnerDrawFixed
        _lstSugerenciasOrigen.ItemHeight = 22
        _lstSugerenciasOrigen.Visible = False
        _lstSugerenciasOrigen.DisplayMember = "Nombre"

        Me.Controls.Add(_lstSugerenciasOrigen)
        _lstSugerenciasOrigen.BringToFront()

        AddHandler _lstSugerenciasOrigen.DoubleClick, AddressOf lstSugerenciasOrigen_Seleccionar
        AddHandler _lstSugerenciasOrigen.MouseClick, AddressOf lstSugerenciasOrigen_Seleccionar
        AddHandler _lstSugerenciasOrigen.KeyDown, AddressOf lstSugerenciasOrigen_KeyDown
        AddHandler _lstSugerenciasOrigen.DrawItem, AddressOf lstSugerenciasOrigen_DrawItem
    End Sub

    Private Sub lstSugerenciasOrigen_DrawItem(sender As Object, e As DrawItemEventArgs)
        If e.Index < 0 Then Return

        Dim oficina = DirectCast(_lstSugerenciasOrigen.Items(e.Index), Cat_Oficina)

        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds)
            TextRenderer.DrawText(e.Graphics, oficina.Nombre, _lstSugerenciasOrigen.Font, e.Bounds, SystemColors.HighlightText, TextFormatFlags.VerticalCenter Or TextFormatFlags.Left)
        Else
            e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds)
            TextRenderer.DrawText(e.Graphics, oficina.Nombre, _lstSugerenciasOrigen.Font, e.Bounds, SystemColors.WindowText, TextFormatFlags.VerticalCenter Or TextFormatFlags.Left)
        End If
    End Sub

    ' =======================================================
    ' LÓGICA DE RANGOS Y NUMERACIÓN (NUEVO)
    ' =======================================================
    Private Sub VerificarRangoNumeracion()
        ' 1. Validaciones de Seguridad
        If _idEdicion.HasValue Then Return

        ' VALIDACIÓN CRÍTICA: Si no hay valor o el valor NO ES UN NÚMERO, salimos.
        ' Esto evita el error InvalidCastException si el combo devuelve un objeto.
        If cboOrigen.SelectedValue Is Nothing OrElse Not IsNumeric(cboOrigen.SelectedValue) Then
            HabilitarEscrituraManual()
            Return
        End If

        ' Ahora es seguro convertir a Integer
        Dim idOrigenSeleccionado As Integer = CInt(cboOrigen.SelectedValue)

        ' Si el origen NO es mi oficina (ej: viene de Gerencia), debo escribir manual
        If idOrigenSeleccionado <> SesionGlobal.OficinaID Then
            HabilitarEscrituraManual()
            Return
        End If

        ' 2. Validación del Tipo
        If cboTipo.SelectedValue Is Nothing OrElse Not IsNumeric(cboTipo.SelectedValue) Then Return

        Dim idTipo As Integer = CInt(cboTipo.SelectedValue)

        ' 3. Buscamos si hay un rango activo
        Dim rangoActivo = _unitOfWork.Repository(Of Mae_NumeracionRangos)().GetQueryable(tracking:=True).FirstOrDefault(Function(r) r.IdTipo = idTipo And r.Activo = True)

        If rangoActivo IsNot Nothing Then
            ' CASO A: HAY RANGO AUTOMÁTICO
            _generacionAutomatica = True
            txtNumeroRef.Text = "(Automático - Próx: " & (rangoActivo.UltimoUtilizado + 1) & ")"
            txtNumeroRef.Enabled = False
            txtNumeroRef.BackColor = Color.LightGoldenrodYellow
        Else
            ' CASO B: NO HAY RANGO (Escritura manual)
            HabilitarEscrituraManual()
        End If
    End Sub

    Private Sub HabilitarEscrituraManual()
        _generacionAutomatica = False
        txtNumeroRef.Enabled = True
        txtNumeroRef.BackColor = Color.White
        ' Si tenía texto automático, lo limpiamos
        If txtNumeroRef.Text.StartsWith("(Auto") Then txtNumeroRef.Clear()
    End Sub

    ' Eventos para disparar la verificación
    Private Sub cboTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipo.SelectedIndexChanged
        VerificarRangoNumeracion()
    End Sub

    Private Sub cboOrigen_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOrigen.SelectedIndexChanged
        VerificarRangoNumeracion()
    End Sub

    Private Async Sub txtBuscarOrigen_TextChanged(sender As Object, e As EventArgs) Handles txtBuscarOrigen.TextChanged
        If _seleccionandoOrigen Then Return

        _oficinaOrigenSeleccionada = Nothing
        cboOrigen.SelectedIndex = -1
        Await FiltrarOficinasOrigenAsync(txtBuscarOrigen.Text)
    End Sub

    Private Async Function FiltrarOficinasOrigenAsync(texto As String) As Task
        Dim versionActual = Interlocked.Increment(_filtroVersionOrigen)

        Try
            Await Task.Delay(300)

            If _cerrandoFormulario OrElse versionActual <> _filtroVersionOrigen Then Return

            Dim textoNormalizado = texto.Trim()
            Dim listaFiltrada As List(Of Cat_Oficina)

            If String.IsNullOrWhiteSpace(textoNormalizado) Then
                listaFiltrada = New List(Of Cat_Oficina)()
            Else
                listaFiltrada = Await Task.Run(Function()
                                                   Dim terminos = textoNormalizado.ToUpper().Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

                                                   Return _todasLasOficinas.
                                                       Where(Function(o) Not String.IsNullOrWhiteSpace(o.Nombre) AndAlso terminos.All(Function(t) o.Nombre.ToUpper().Contains(t))).
                                                       Take(30).
                                                       ToList()
                                               End Function)
            End If

            If _cerrandoFormulario OrElse versionActual <> _filtroVersionOrigen Then Return
            MostrarSugerenciasOrigen(listaFiltrada)
        Catch ex As Exception
            Debug.WriteLine("Error en búsqueda de oficinas: " & ex.Message)
        End Try
    End Function

    Private Sub MostrarSugerenciasOrigen(lista As List(Of Cat_Oficina))
        _lstSugerenciasOrigen.BeginUpdate()
        _lstSugerenciasOrigen.Items.Clear()

        For Each oficina In lista
            _lstSugerenciasOrigen.Items.Add(oficina)
        Next

        _lstSugerenciasOrigen.EndUpdate()

        If lista.Count > 0 Then
            Dim pScreen As Point = txtBuscarOrigen.Parent.PointToScreen(New Point(txtBuscarOrigen.Left, txtBuscarOrigen.Bottom))
            Dim pForm As Point = Me.PointToClient(pScreen)

            _lstSugerenciasOrigen.Location = pForm
            _lstSugerenciasOrigen.Width = txtBuscarOrigen.Width

            Dim alturaNecesaria As Integer = (lista.Count * _lstSugerenciasOrigen.ItemHeight) + 4
            Dim alturaMaxima As Integer = 130
            _lstSugerenciasOrigen.Height = If(alturaNecesaria > alturaMaxima, alturaMaxima, alturaNecesaria)

            _lstSugerenciasOrigen.SelectedIndex = 0
            _lstSugerenciasOrigen.Visible = True
            _lstSugerenciasOrigen.BringToFront()
        Else
            OcultarSugerenciasOrigen()
        End If
    End Sub

    Private Sub txtBuscarOrigen_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBuscarOrigen.KeyDown
        If Not _lstSugerenciasOrigen.Visible Then Return

        Select Case e.KeyCode
            Case Keys.Down
                If _lstSugerenciasOrigen.SelectedIndex < _lstSugerenciasOrigen.Items.Count - 1 Then
                    _lstSugerenciasOrigen.SelectedIndex += 1
                End If
                e.Handled = True
            Case Keys.Up
                If _lstSugerenciasOrigen.SelectedIndex > 0 Then
                    _lstSugerenciasOrigen.SelectedIndex -= 1
                End If
                e.Handled = True
            Case Keys.Enter
                SeleccionarOficinaOrigenActual()
                e.SuppressKeyPress = True
                e.Handled = True
            Case Keys.Escape
                OcultarSugerenciasOrigen()
                e.Handled = True
        End Select
    End Sub

    Private Sub lstSugerenciasOrigen_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            SeleccionarOficinaOrigenActual()
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub

    Private Sub lstSugerenciasOrigen_Seleccionar(sender As Object, e As EventArgs)
        SeleccionarOficinaOrigenActual()
    End Sub

    Private Sub SeleccionarOficinaOrigenActual()
        Dim oficina = TryCast(_lstSugerenciasOrigen.SelectedItem, Cat_Oficina)
        If oficina Is Nothing Then Return

        SeleccionarOrigen(oficina)
        txtBuscarOrigen.Focus()
    End Sub

    Private Sub SeleccionarOrigen(oficina As Cat_Oficina)
        Interlocked.Increment(_filtroVersionOrigen)

        _oficinaOrigenSeleccionada = oficina

        cboOrigen.DataSource = Nothing
        cboOrigen.DataSource = New List(Of Cat_Oficina) From {oficina}
        cboOrigen.DisplayMember = "Nombre"
        cboOrigen.ValueMember = "IdOficina"
        cboOrigen.SelectedIndex = 0

        Try
            _seleccionandoOrigen = True
            txtBuscarOrigen.Text = oficina.Nombre
            txtBuscarOrigen.SelectionStart = txtBuscarOrigen.TextLength
        Finally
            _seleccionandoOrigen = False
        End Try

        OcultarSugerenciasOrigen()
        VerificarRangoNumeracion()
    End Sub

    Private Sub SeleccionarOrigenPorId(idOficina As Integer)
        Dim oficina = _todasLasOficinas.FirstOrDefault(Function(o) o.IdOficina = idOficina)
        If oficina Is Nothing Then Return

        SeleccionarOrigen(oficina)
    End Sub

    Private Sub OcultarSugerenciasOrigen()
        _lstSugerenciasOrigen.Visible = False
    End Sub

    Private Function ObtenerIdOrigenSeleccionado() As Integer?
        If _oficinaOrigenSeleccionada IsNot Nothing Then
            Return _oficinaOrigenSeleccionada.IdOficina
        End If

        Return Nothing
    End Function

    ' =======================================================
    ' CONFIGURACIONES DE MODO
    ' =======================================================
    Private Sub PreparaModoRespuesta()
        Me.Text = "VECTOR - Carga de Actuación / Respuesta"
        Me.BackColor = Color.Ivory
        btnGuardar.Text = "GUARDAR ACTUACIÓN"

        If _idOrigenForzado.HasValue Then
            ' DIGITALIZACIÓN: Viene de afuera, origen fijo, numeración manual.
            SeleccionarOrigenPorId(_idOrigenForzado.Value)
            txtBuscarOrigen.Enabled = False
        Else
            ' RESPUESTA PROPIA: Origen soy yo.
            Try
                SeleccionarOrigenPorId(SesionGlobal.OficinaID)
                txtBuscarOrigen.Enabled = False
            Catch
            End Try
        End If

        ' Verificar rangos después de setear el origen
        VerificarRangoNumeracion()

        txtAsunto.Text = "REF: " & _asuntoPadre
        txtDescripcion.Focus()
    End Sub

    Private Sub CargarDatosParaEditar()
        Dim doc = _unitOfWork.Context.Set(Of Mae_Documento)().Find(_idEdicion.Value)

        Me.Text = "EDITAR DOCUMENTO: " & doc.NumeroOficial
        Me.BackColor = Color.AliceBlue
        btnGuardar.Text = "GUARDAR CAMBIOS"

        cboTipo.SelectedValue = doc.IdTipo

        Dim primerMov = doc.Tra_Movimiento.OrderBy(Function(m) m.IdMovimiento).FirstOrDefault()
        If primerMov IsNot Nothing Then
            SeleccionarOrigenPorId(primerMov.IdOficinaOrigen)
        End If
        txtBuscarOrigen.Enabled = False

        txtNumeroRef.Text = doc.NumeroOficial
        txtAsunto.Text = doc.Asunto
        txtDescripcion.Text = doc.Descripcion
        numFojas.Value = doc.Fojas
        dtpFechaRecepcion.Value = If(doc.FechaRecepcion.HasValue, doc.FechaRecepcion.Value, DateTime.Now)
    End Sub

    ' =======================================================
    ' ADJUNTOS DIGITALES
    ' =======================================================
    Private Sub CargarAdjuntosIniciales()
        If _idEdicion.HasValue Then
            _adjuntos = AttachmentStore.LoadAttachments(_idEdicion.Value)
        Else
            _adjuntos = New List(Of AttachmentInfo)()
        End If
        RefrescarListaAdjuntos()
    End Sub

    Private Sub RefrescarListaAdjuntos()
        lstAdjuntos.DataSource = Nothing
        lstAdjuntos.DataSource = _adjuntos
        lstAdjuntos.DisplayMember = "DisplayLabel"
        lblAdjuntosInfo.Text = $"{_adjuntos.Count} archivo(s)"
    End Sub

    Private Function ObtenerAdjuntoSeleccionado() As AttachmentInfo
        Return TryCast(lstAdjuntos.SelectedItem, AttachmentInfo)
    End Function

    Private Sub btnAdjuntar_Click(sender As Object, e As EventArgs) Handles btnAdjuntar.Click
        Using ofd As New OpenFileDialog()
            ofd.Title = "Seleccionar archivos"
            ofd.Filter = "Documentos e Imágenes|*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.png;*.jpg;*.jpeg;*.tif;*.tiff|Todos los archivos|*.*"
            ofd.Multiselect = True

            If ofd.ShowDialog() <> DialogResult.OK Then Return

            For Each archivo In ofd.FileNames
                If _adjuntos.Any(Function(a) String.Equals(a.OriginalPath, archivo, StringComparison.OrdinalIgnoreCase)) Then
                    Continue For
                End If

                Dim info As New AttachmentInfo() With {
                    .DisplayName = Path.GetFileName(archivo),
                    .OriginalPath = archivo,
                    .StoredName = "",
                    .AddedAt = DateTime.Now
                }
                _adjuntos.Add(info)
            Next
        End Using

        RefrescarListaAdjuntos()
    End Sub

    Private Sub btnQuitarAdjunto_Click(sender As Object, e As EventArgs) Handles btnQuitarAdjunto.Click
        Dim adjunto = ObtenerAdjuntoSeleccionado()
        If adjunto Is Nothing Then Return

        _adjuntos.Remove(adjunto)
        RefrescarListaAdjuntos()
    End Sub

    Private Sub btnAbrirAdjunto_Click(sender As Object, e As EventArgs) Handles btnAbrirAdjunto.Click
        Dim adjunto = ObtenerAdjuntoSeleccionado()
        If adjunto Is Nothing Then Return

        Dim ruta As String = ""

        If Not String.IsNullOrWhiteSpace(adjunto.StoredName) AndAlso _idEdicion.HasValue Then
            ruta = AttachmentStore.GetAttachmentPath(_idEdicion.Value, adjunto.StoredName)
        Else
            ruta = adjunto.OriginalPath
        End If

        If String.IsNullOrWhiteSpace(ruta) OrElse Not File.Exists(ruta) Then
            Toast.Show(Me, "El archivo no está disponible en el sistema.", ToastType.Warning)
            Return
        End If

        Process.Start(New ProcessStartInfo(ruta) With {.UseShellExecute = True})
    End Sub

    ' =======================================================
    ' GUARDAR (INSERT O UPDATE)
    ' =======================================================
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

        ' 1. Validaciones básicas
        If cboTipo.SelectedIndex = -1 Then
            Toast.Show(Me, "Seleccione el TIPO de documento.", ToastType.Warning)
            Return
        End If

        Dim idOrigenSeleccionado = ObtenerIdOrigenSeleccionado()
        If Not idOrigenSeleccionado.HasValue Then
            Toast.Show(Me, "Seleccione Organismo / Oficina de origen.", ToastType.Warning)
            txtBuscarOrigen.Focus()
            Return
        End If

        ' Si es manual, validamos que haya escrito algo
        If Not _generacionAutomatica AndAlso String.IsNullOrWhiteSpace(txtNumeroRef.Text) Then
            Toast.Show(Me, "Ingrese la Referencia/Número.", ToastType.Warning)
            Return
        End If

        Try
            Dim doc As Mae_Documento

            If _idEdicion.HasValue Then
                ' =================================================
                ' CASO UPDATE: EDITAR EXISTENTE
                ' =================================================
                doc = _unitOfWork.Context.Set(Of Mae_Documento)().Find(_idEdicion.Value)

                ' En edición NO cambiamos el número automáticamente, respetamos el que tiene
                doc.IdTipo = CInt(cboTipo.SelectedValue)
                doc.NumeroOficial = txtNumeroRef.Text.Trim()
                doc.Asunto = txtAsunto.Text.ToUpper().Trim()
                doc.Descripcion = txtDescripcion.Text.Trim()
                doc.Fojas = CInt(numFojas.Value)
                doc.FechaRecepcion = dtpFechaRecepcion.Value

                _unitOfWork.Commit()

                GuardarAdjuntos(doc.IdDocumento)
                AuditoriaSistema.RegistrarEvento($"Edición de documento {doc.NumeroOficial} ({cboTipo.Text}). Asunto: {doc.Asunto}. Adjuntos: {_adjuntos.Count}.", "DOCUMENTOS", unitOfWorkExterno:=_unitOfWork)
                _unitOfWork.Commit()
                Toast.Show(Me, "✅ Documento corregido exitosamente.", ToastType.Success)

            Else
                ' =================================================
                ' CASO INSERT: CREAR NUEVO
                ' =================================================
                doc = New Mae_Documento()

                ' --- LÓGICA DE NUMERACIÓN (AUTO vs MANUAL) ---
                If _generacionAutomatica Then
                    Dim idTipo As Integer = CInt(cboTipo.SelectedValue)

                    ' Volvemos a consultar el rango para asegurar concurrencia
                    Dim rango = _unitOfWork.Repository(Of Mae_NumeracionRangos)().GetQueryable(tracking:=True).FirstOrDefault(Function(r) r.IdTipo = idTipo And r.Activo = True)

                    If rango Is Nothing Then
                        Toast.Show(Me, "El rango de numeración se desactivó o no existe. Guarde manualmente.", ToastType.Error)
                        HabilitarEscrituraManual()
                        Return
                    End If

                    If rango.UltimoUtilizado >= rango.NumeroFin Then
                        Toast.Show(Me, "¡SE AGOTÓ LA NUMERACIÓN PARA ESTE TIPO!" & vbCrLf & "Contacte al administrador.", ToastType.Error)
                        Return
                    End If

                    ' Generamos el número
                    Dim nuevoNumero As Integer = rango.UltimoUtilizado + 1
                    Dim anioCorto As String = DateTime.Now.ToString("yy") ' Ej: /26

                    doc.NumeroOficial = nuevoNumero.ToString() & "/" & anioCorto

                    ' Actualizamos el contador
                    rango.UltimoUtilizado = nuevoNumero
                Else
                    ' Manual
                    doc.NumeroOficial = txtNumeroRef.Text.Trim()
                End If
                ' ---------------------------------------------

                doc.IdTipo = CInt(cboTipo.SelectedValue)
                doc.Asunto = txtAsunto.Text.ToUpper().Trim()
                doc.Descripcion = txtDescripcion.Text.Trim()
                doc.Fojas = CInt(numFojas.Value)
                doc.FechaCreacion = DateTime.Now
                doc.FechaRecepcion = dtpFechaRecepcion.Value

                doc.IdEstadoActual = 1
                doc.IdOficinaActual = SesionGlobal.OficinaID
                doc.IdUsuarioCreador = SesionGlobal.UsuarioID

                ' Lógica de Familia
                If _modoRespuesta Then
                    doc.IdHiloConversacion = _guidHilo.Value
                    doc.IdDocumentoPadre = _idDocPadre
                Else
                    doc.IdHiloConversacion = Guid.NewGuid()
                    doc.IdDocumentoPadre = Nothing
                End If

                _unitOfWork.Repository(Of Mae_Documento)().Add(doc)

                ' Trazabilidad
                Dim mov As New Tra_Movimiento()
                mov.IdDocumento = doc.IdDocumento
                mov.FechaMovimiento = DateTime.Now
                mov.IdOficinaOrigen = idOrigenSeleccionado.Value
                mov.IdOficinaDestino = SesionGlobal.OficinaID

                If _modoRespuesta Then
                    If _idOrigenForzado.HasValue Then
                        mov.ObservacionPase = "Carga de actuación externa (Digitalización)."
                    Else
                        mov.ObservacionPase = "Generación de respuesta interna."
                    End If
                Else
                    mov.ObservacionPase = "Ingreso inicial de expediente."
                End If

                mov.IdUsuarioResponsable = SesionGlobal.UsuarioID
                mov.IdEstadoEnEseMomento = 1

                doc.Tra_Movimiento.Add(mov)

                ' EF guardará 'doc', 'mov' y actualizará 'rango' en una sola transacción
                _unitOfWork.Commit()

                Try
                    GuardarAdjuntos(doc.IdDocumento)
                Catch exAdjuntos As Exception
                    Try
                        AttachmentStore.DeleteAttachments(doc.IdDocumento)
                    Catch
                    End Try

                    Try
                        Dim docPersistido = _unitOfWork.Context.Set(Of Mae_Documento)().Find(doc.IdDocumento)
                        If docPersistido IsNot Nothing Then
                            _unitOfWork.Context.Set(Of Mae_Documento)().Remove(docPersistido)
                            _unitOfWork.Commit()
                        End If
                    Catch
                    End Try

                    Throw New Exception("No se pudo completar el guardado del documento con sus adjuntos.", exAdjuntos)
                End Try

                Dim tipoCarga As String = If(_modoRespuesta, "Respuesta/Actuación", "Ingreso")
                AuditoriaSistema.RegistrarEvento($"{tipoCarga} de documento {doc.NumeroOficial} ({cboTipo.Text}). Asunto: {doc.Asunto}. Adjuntos: {_adjuntos.Count}.", "DOCUMENTOS", unitOfWorkExterno:=_unitOfWork)
                _unitOfWork.Commit()

                ' --- MENSAJE PROFESIONAL (NUEVO BLOQUE) ---
                Dim sb As New System.Text.StringBuilder()
                sb.AppendLine("✅ Documento registrado exitosamente.")
                sb.AppendLine()
                sb.AppendLine("📄 REF: " & cboTipo.Text.ToUpper() & " " & doc.NumeroOficial)
                sb.AppendLine("📌 ASUNTO: " & doc.Asunto)
                sb.AppendLine()
                sb.AppendLine("El expediente ya se encuentra disponible en su Bandeja.")

                Toast.Show(Me, sb.ToString(), ToastType.Success)
                ' ------------------------------------------
            End If
            Me.ShowIcon = False
            Me.Close()

        Catch ex As Exception
            Toast.Show(Me, "Error al guardar: " & ex.Message, ToastType.Error)
        End Try
    End Sub

    Private Sub GuardarAdjuntos(idDocumento As Long)
        AttachmentStore.SaveAttachments(idDocumento, _adjuntos)
    End Sub

    Private Sub LimpiarControles()
        txtNumeroRef.Clear()
        txtAsunto.Clear()
        txtDescripcion.Clear()
        cboTipo.SelectedIndex = -1
        cboOrigen.SelectedIndex = -1
        _oficinaOrigenSeleccionada = Nothing
        txtBuscarOrigen.Clear()
        txtBuscarOrigen.Enabled = True
        numFojas.Value = 1
        dtpFechaRecepcion.Value = DateTime.Now
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.ShowIcon = False
        Me.Close()
    End Sub

    Private Sub frmMesaEntrada_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.ShowIcon = False
        _cerrandoFormulario = True
        Interlocked.Increment(_filtroVersionOrigen)
        _unitOfWork.Dispose()
    End Sub

    Private Sub btnBuscarPPL_Click(sender As Object, e As EventArgs) Handles btnBuscarPPL.Click
        Dim f As New frmBuscadorReclusos()

        If f.ShowDialog() = DialogResult.OK Then

            Dim textoAInsertar As String = f.ResultadoFormateado

            ' Lógica de inserción inteligente
            If txtAsunto.Focused Then
                txtAsunto.Text &= " - REF: " & textoAInsertar
                txtAsunto.SelectionStart = txtAsunto.Text.Length

            ElseIf txtDescripcion.Focused Then
                Dim posicion As Integer = txtDescripcion.SelectionStart
                txtDescripcion.Text = txtDescripcion.Text.Insert(posicion, textoAInsertar)
                txtDescripcion.SelectionStart = posicion + textoAInsertar.Length

            Else
                ' Si no hay foco, sugerimos el asunto estándar
                If String.IsNullOrWhiteSpace(txtAsunto.Text) Then
                    txtAsunto.Text = "PPL " & textoAInsertar
                Else
                    txtAsunto.Text &= " (" & textoAInsertar & ")"
                End If
            End If
        End If
    End Sub
End Class
