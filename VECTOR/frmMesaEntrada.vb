Imports System.Data.Entity
Imports System.Diagnostics
Imports System.Drawing
Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks

Public Class frmMesaEntrada

    Private ReadOnly _unitOfWork As IUnitOfWork
    Private Const IdBandejaEntrada As Integer = 13

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
    Private _validacionNumeroVersion As Integer = 0
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
    ' LÓGICA DE RANGOS Y NUMERACIÓN (INTELIGENTE)
    ' =======================================================


    ''' <summary>
    ''' Calcula el próximo número disponible basándose únicamente en los rangos asignados a la oficina.
    ''' </summary>
    Private Function CalcularSiguienteNumero(idTipo As Integer, idOficinaContexto As Integer, anio As Integer) As Integer
        ' 1. Obtener los rangos asignados EXCLUSIVAMENTE a esta oficina
        Dim misRangos = ObtenerTodosLosRangosActivos(idTipo, idOficinaContexto, anio)

        If misRangos.Count = 0 Then Return -1 ' No tiene números asignados

        ' 2. Iterar por mis rangos asignados para encontrar el siguiente hueco
        For Each rango In misRangos
            Dim candidato As Integer = rango.UltimoUtilizado

            ' Si nunca se usó, arrancamos antes del inicio para que al sumar 1 dé el inicio
            If candidato < rango.NumeroInicio Then candidato = rango.NumeroInicio - 1

            ' Verificar si el siguiente número entra en este lote
            If (candidato + 1) <= rango.NumeroFin Then
                Return candidato + 1
            End If

            ' Si este lote está lleno, el bucle pasa al siguiente lote asignado
        Next

        ' Si pasamos todos los lotes y están llenos
        Return -2
    End Function

    Private Sub VerificarRangoNumeracion()
        ' 1. Validaciones de Seguridad
        If _idEdicion.HasValue Then Return
        If cboOrigen.SelectedValue Is Nothing OrElse Not IsNumeric(cboOrigen.SelectedValue) Then
            HabilitarEscrituraManual()
            Return
        End If

        Dim idOrigenSeleccionado As Integer = CInt(cboOrigen.SelectedValue)
        If cboTipo.SelectedValue Is Nothing OrElse Not IsNumeric(cboTipo.SelectedValue) Then Return

        Dim idTipo As Integer = CInt(cboTipo.SelectedValue)
        Dim anioObjetivo As Integer = DateTime.Now.Year

        ' =======================================================
        ' NUEVA REGLA: EXCLUSIVIDAD DE AUTOMATISMO
        ' =======================================================
        ' Solo si la oficina seleccionada es la BANDEJA DE ENTRADA (Id 13)
        ' intentamos sugerir número automático.
        If idOrigenSeleccionado <> IdBandejaEntrada Then
            ' Si es cualquier otra oficina (Unidad 7, Módulo, etc.), es MANUAL.
            HabilitarEscrituraManual()
            Return
        End If

        ' =======================================================
        ' LÓGICA SOLO PARA BANDEJA DE ENTRADA
        ' =======================================================
        Dim proximoNumero = CalcularSiguienteNumero(idTipo, idOrigenSeleccionado, anioObjetivo)

        If proximoNumero > 0 Then
            ' CASO A: TIENE RANGO ASIGNADO Y ESPACIO DISPONIBLE
            _generacionAutomatica = True
            txtNumeroRef.Text = "(Automático - Próx: " & proximoNumero & ")"
            txtNumeroRef.Enabled = False
            txtNumeroRef.BackColor = Color.LightGoldenrodYellow

        ElseIf proximoNumero = -2 Then
            ' CASO B: TIENE RANGO PERO SE AGOTÓ
            HabilitarEscrituraManual()
            txtNumeroRef.Text = "AGOTADO - SOLICITAR NUEVO RANGO"
            txtNumeroRef.BackColor = Color.MistyRose

        Else ' Retorna -1
            ' CASO C: NO TIENE RANGO ASIGNADO
            HabilitarEscrituraManual()
        End If
    End Sub
    Private Sub HabilitarEscrituraManual()
        ' 1. Si veníamos de modo automático (estaba bloqueado o con flag true), 
        '    LIMPIAMOS SÍ O SÍ. Esto evita que quede "Automático - Próx: 10" 
        '    cuando cambias a un tipo de documento sin rango.
        If _generacionAutomatica OrElse txtNumeroRef.Text.StartsWith("(Auto") OrElse txtNumeroRef.Text.StartsWith("AGOTADO") Then
            txtNumeroRef.Clear()
        End If

        ' 2. Reseteamos estados
        _generacionAutomatica = False
        txtNumeroRef.Enabled = True
        txtNumeroRef.BackColor = Color.White
    End Sub

    ' Mantenemos esta función para validaciones puntuales de existencia, aunque usamos la nueva para calcular
    Private Function ObtenerRangoActivo(idTipo As Integer, idOficina As Integer?) As Mae_NumeracionRangos
        Dim buscaPorOficinaEspecifica As Integer? = idOficina
        Dim usarRangoBandeja As Boolean = False
        If idOficina.HasValue AndAlso idOficina.Value = IdBandejaEntrada Then
            buscaPorOficinaEspecifica = Nothing
            usarRangoBandeja = True
        End If

        Return _unitOfWork.Repository(Of Mae_NumeracionRangos)().GetQueryable(tracking:=True).
            FirstOrDefault(Function(r) r.IdTipo = idTipo And r.Activo = True And
                           If(usarRangoBandeja, (r.IdOficina Is Nothing OrElse r.IdOficina = IdBandejaEntrada), r.IdOficina = buscaPorOficinaEspecifica))
    End Function
    ' Nueva función que trae TODOS los rangos, no solo el primero
    ' Nueva función simplificada: Busca rangos asignados estrictamente a la oficina solicitada
    Private Function ObtenerTodosLosRangosActivos(idTipo As Integer, idOficina As Integer?, anio As Integer) As List(Of Mae_NumeracionRangos)
        ' En el modelo Secretaría General, idOficina SIEMPRE debe tener valor (incluso para Bandeja = 13).
        ' Si llega Nothing, es un error de lógica upstream, pero lo manejamos devolviendo lista vacía.
        If Not idOficina.HasValue Then Return New List(Of Mae_NumeracionRangos)()

        Return _unitOfWork.Repository(Of Mae_NumeracionRangos)().GetQueryable(tracking:=True).
            Where(Function(r) r.IdTipo = idTipo And r.Activo = True And r.Anio = anio And r.IdOficina = idOficina.Value).
            OrderBy(Function(r) r.NumeroInicio).
            ToList()
    End Function

    Private Function TryObtenerNumeroBase(numeroTexto As String, ByRef numero As Integer) As Boolean
        numero = 0
        If String.IsNullOrWhiteSpace(numeroTexto) Then Return False
        Dim parteNumero As String = numeroTexto.Trim().Split("/"c)(0).Trim()
        Return Integer.TryParse(parteNumero, numero)
    End Function

    ' Eventos para disparar la verificación
    Private Sub cboTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipo.SelectedIndexChanged
        VerificarRangoNumeracion()
    End Sub

    Private Sub cboOrigen_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOrigen.SelectedIndexChanged
        VerificarRangoNumeracion()
    End Sub
    ' =========================================================================
    ' VALIDACIÓN AL PERDER EL FOCO (Lógica Solicitada)
    ' =========================================================================
    Private Async Sub txtNumeroRef_Leave(sender As Object, e As EventArgs) Handles txtNumeroRef.Leave
        ' Si se está cerrando el formulario o cancelando, no validamos
        If _cerrandoFormulario OrElse btnCancelar.Focused Then Return
        If String.IsNullOrWhiteSpace(txtNumeroRef.Text) Then Return

        ' 1. Obtener datos clave
        If cboTipo.SelectedValue Is Nothing OrElse Not IsNumeric(cboTipo.SelectedValue) Then Return
        Dim idTipoSeleccionado As Integer = CInt(cboTipo.SelectedValue)

        Dim idOrigenSeleccionado = ObtenerIdOrigenSeleccionado()
        If Not idOrigenSeleccionado.HasValue Then Return

        ' 2. Interpretar el número y el año escrito
        Dim numeroTexto = txtNumeroRef.Text.Trim()
        Dim numeroBase As Integer
        If Not TryObtenerNumeroBase(numeroTexto, numeroBase) Then Return ' Si no es numero, lo ignora o valida en Guardar

        Dim anioManual As Integer = DateTime.Now.Year
        Dim partes = numeroTexto.Split("/"c)
        If partes.Length > 1 AndAlso IsNumeric(partes(1)) Then
            Dim dosDigitos = CInt(partes(1))
            anioManual = 2000 + dosDigitos
        Else
            anioManual = dtpFechaRecepcion.Value.Year
        End If

        ' 3. LÓGICA DE NEGOCIO SOLICITADA
        ' Paso A: Verificar si MI oficina (la seleccionada) tiene rangos activos para este tipo/año
        Dim misRangos = ObtenerTodosLosRangosActivos(idTipoSeleccionado, idOrigenSeleccionado.Value, anioManual)

        ' REGLA DE ORO: Si mi oficina NO tiene rangos configurados, aceptamos TODO (Return).
        ' No importa si el número es de otro, si yo no uso rangos, soy libre.
        If misRangos.Count = 0 Then
            txtNumeroRef.BackColor = Color.White
            Return
        End If

        ' Paso B: Si TENGO rangos, verificamos si el número cae dentro de UNO de los míos.
        Dim estaEnMiRango As Boolean = misRangos.Any(Function(r) numeroBase >= r.NumeroInicio And numeroBase <= r.NumeroFin)

        If estaEnMiRango Then
            ' Es mío, todo correcto.
            txtNumeroRef.BackColor = Color.White
            Return
        End If

        ' Paso C: Tengo rangos, el número NO está en mi rango. Verificamos si está RESERVADO por otro.
        ' (Solo buscamos rangos de OTRAS oficinas)
        Dim rangoAjeno As Mae_NumeracionRangos = Nothing

        Using uowCheck As New UnitOfWork()
            rangoAjeno = Await uowCheck.Repository(Of Mae_NumeracionRangos)().GetQueryable(tracking:=False).
                                FirstOrDefaultAsync(Function(r) r.IdTipo = idTipoSeleccionado And
                                                       r.Anio = anioManual And
                                                       r.Activo = True And
                                                       r.IdOficina <> idOrigenSeleccionado.Value And
                                                       numeroBase >= r.NumeroInicio And
                                                       numeroBase <= r.NumeroFin)
        End Using

        If rangoAjeno IsNot Nothing Then
            ' --- ALERTA Y BLOQUEO ---
            Dim nombrePropietario = If(rangoAjeno.Cat_Oficina IsNot Nothing, rangoAjeno.Cat_Oficina.Nombre, "Otra Oficina")

            ' Mostramos el Toast con duración extendida
            Toast.Show(Me, $"⛔ ACCIÓN BLOQUEADA" & vbCrLf &
                           $"El número {numeroBase} pertenece al rango reservado de: {nombrePropietario}." & vbCrLf &
                           $"Tu oficina tiene rangos configurados y debes respetarlos.", ToastType.Error)

            txtNumeroRef.BackColor = Color.MistyRose

            ' Obligamos al usuario a volver (Focus)
            txtNumeroRef.Focus()
            txtNumeroRef.SelectAll()
        Else
            ' No está en mi rango, pero tampoco en el de nadie (Tierra de nadie).
            ' Como tengo rangos activos, podría advertir, pero la solicitud pide alerta si "está reservado".
            ' Si no está reservado, lo dejamos pasar o lo marcamos en amarillo.
            txtNumeroRef.BackColor = Color.LightYellow
        End If
    End Sub
    'Private Async Sub txtNumeroRef_TextChanged(sender As Object, e As EventArgs) Handles txtNumeroRef.TextChanged
    '    If _generacionAutomatica OrElse Not txtNumeroRef.Focused Then Return

    '    Dim versionActual = Interlocked.Increment(_validacionNumeroVersion)
    '    Await Task.Delay(400)

    '    If _cerrandoFormulario OrElse versionActual <> _validacionNumeroVersion Then Return

    '    Dim numeroTexto = txtNumeroRef.Text.Trim()
    '    If String.IsNullOrWhiteSpace(numeroTexto) Then Return

    '    If cboTipo.SelectedValue Is Nothing OrElse Not IsNumeric(cboTipo.SelectedValue) Then Return
    '    Dim idTipoSeleccionado As Integer = CInt(cboTipo.SelectedValue)

    '    Dim idOrigenSeleccionado = ObtenerIdOrigenSeleccionado()
    '    If Not idOrigenSeleccionado.HasValue Then Return

    '    Dim numeroBase As Integer
    '    If Not TryObtenerNumeroBase(numeroTexto, numeroBase) Then
    '        ' No mostramos error mientras escribe texto parcial inválido, solo al final o si es válido numéricamente
    '        Return
    '    End If

    '    Dim anioManual As Integer = DateTime.Now.Year
    '    Dim partes = numeroTexto.Split("/"c)
    '    If partes.Length > 1 AndAlso IsNumeric(partes(1)) Then
    '        Dim dosDigitos = CInt(partes(1))
    '        anioManual = 2000 + dosDigitos
    '    Else
    '        anioManual = dtpFechaRecepcion.Value.Year
    '    End If

    '    ' --- VALIDACIÓN DE PROPIEDAD DEL NÚMERO ---
    '    ' Buscamos si este número pertenece a ALGÚN rango registrado en el sistema
    '    Dim rangoPropietario = _unitOfWork.Repository(Of Mae_NumeracionRangos)().GetQueryable(tracking:=False).
    '        FirstOrDefault(Function(r) r.IdTipo = idTipoSeleccionado And
    '                                   r.Anio = anioManual And
    '                                   r.Activo = True And
    '                                   numeroBase >= r.NumeroInicio And
    '                                   numeroBase <= r.NumeroFin)

    '    If rangoPropietario IsNot Nothing Then
    '        ' Si el número cae en un rango, VERIFICAMOS que el rango sea de la oficina seleccionada
    '        If rangoPropietario.IdOficina <> idOrigenSeleccionado.Value Then
    '            Dim nombrePropietario = If(rangoPropietario.Cat_Oficina IsNot Nothing, rangoPropietario.Cat_Oficina.Nombre, "Otra Oficina")
    '            Toast.Show(Me, $"El número {numeroBase} está reservado para: {nombrePropietario}.", ToastType.Warning)
    '            txtNumeroRef.ForeColor = Color.Red
    '        Else
    '            txtNumeroRef.ForeColor = Color.Black
    '        End If
    '    Else
    '        ' El número no está en ningún rango. Es "Tierra de nadie". 
    '        ' En este modelo, permitimos su uso manual.
    '        txtNumeroRef.ForeColor = Color.Black
    '    End If

    '    ' Validación de duplicados (Scope: Misma Oficina)
    '    Dim existeNumero = _unitOfWork.Repository(Of Mae_Documento)().GetQueryable(tracking:=False).
    '        Any(Function(d) d.IdTipo = idTipoSeleccionado AndAlso
    '                        d.NumeroOficial = numeroTexto AndAlso
    '                        (Not _idEdicion.HasValue OrElse d.IdDocumento <> _idEdicion.Value) AndAlso
    '                        d.Tra_Movimiento.OrderBy(Function(m) m.IdMovimiento).FirstOrDefault().IdOficinaOrigen = idOrigenSeleccionado.Value)

    '    If existeNumero Then
    '        Toast.Show(Me, $"El número {numeroTexto} ya fue utilizado por esta oficina.", ToastType.Warning)
    '    End If
    'End Sub

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

        ' 1. VALIDACIONES BÁSICAS DE INTERFAZ
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

        ' =========================================================================================
        ' VALIDACIÓN DE RANGO EN EDICIÓN MANUAL (MODELO SECRETARÍA GENERAL)
        ' =========================================================================================
        If Not _idEdicion.HasValue AndAlso Not _generacionAutomatica Then
            Dim idTipoSeleccionado As Integer = CInt(cboTipo.SelectedValue)

            ' A. Deducción del Año del documento
            Dim anioManual As Integer = DateTime.Now.Year
            Dim partes = txtNumeroRef.Text.Split("/"c)
            If partes.Length > 1 AndAlso IsNumeric(partes(1)) Then
                Dim dosDigitos = CInt(partes(1))
                anioManual = 2000 + dosDigitos
            Else
                anioManual = dtpFechaRecepcion.Value.Year
            End If

            Dim numeroBase As Integer
            If Not TryObtenerNumeroBase(txtNumeroRef.Text, numeroBase) Then
                Toast.Show(Me, "Ingrese un número válido.", ToastType.Warning)
                Return
            End If

            ' B. VALIDACIÓN DE PROPIEDAD INTELIGENTE
            Using uowCheck As New UnitOfWork()
                ' 1. Primero verificamos: ¿MI OFICINA ACTUAL usa rangos para este tipo de doc?
                Dim yoUsoRangos As Boolean = uowCheck.Repository(Of Mae_NumeracionRangos)().GetQueryable(tracking:=False).
                    Any(Function(r) r.IdTipo = idTipoSeleccionado And
                                    r.IdOficina = idOrigenSeleccionado.Value And
                                    r.Anio = anioManual And
                                    r.Activo = True)

                If yoUsoRangos Then
                    ' CASO 1: SOY UNA OFICINA CON RANGOS (Ej. Sub Dirección).
                    ' Debo respetar la "Ley de Rangos". No puedo usar un número que pertenezca al rango de otro.

                    Dim rangoPropietario = uowCheck.Repository(Of Mae_NumeracionRangos)().GetQueryable(tracking:=False).
                        FirstOrDefault(Function(r) r.IdTipo = idTipoSeleccionado And
                                               r.Anio = anioManual And
                                               r.Activo = True And
                                               r.IdOficina <> idOrigenSeleccionado.Value And ' Que no sea mío
                                               numeroBase >= r.NumeroInicio And
                                               numeroBase <= r.NumeroFin)

                    If rangoPropietario IsNot Nothing Then
                        Dim nombreOficinaDueña = If(rangoPropietario.Cat_Oficina IsNot Nothing, rangoPropietario.Cat_Oficina.Nombre, "Otra Oficina")
                        Toast.Show(Me, $"ERROR DE NUMERACIÓN: Tu oficina trabaja con rangos y el número {numeroBase} invade el rango reservado de: {nombreOficinaDueña}.", ToastType.Error)
                        Return
                    End If
                Else
                    ' CASO 2: SOY UNA OFICINA SIN RANGOS (Ej. ARCHIVO, OFICINAS EXTERNAS).
                    ' Soy libre. Puedo usar el número 10 aunque la Sub Dirección tenga reservado del 1 al 100.
                    ' No hacemos la validación de propiedad ajena.
                End If

                ' C. VALIDACIÓN DE UNICIDAD (Scope: Misma Oficina)
                ' Esto SIEMPRE se ejecuta. No puedo tener dos documentos "Memo 10" en MI misma oficina, tenga rangos o no.
                Dim yaExiste As Boolean = uowCheck.Repository(Of Mae_Documento)().GetQueryable(tracking:=False).
                    Any(Function(d) d.IdTipo = idTipoSeleccionado AndAlso
                                    d.NumeroOficial = txtNumeroRef.Text.Trim() AndAlso
                                    d.Tra_Movimiento.Any(Function(m) m.IdOficinaOrigen = idOrigenSeleccionado.Value))

                If yaExiste Then
                    Toast.Show(Me, $"El número '{txtNumeroRef.Text}' ya fue registrado previamente por esta oficina.", ToastType.Error)
                    Return
                End If
            End Using
        End If

        ' =========================================================================================
        ' PROCESO DE GUARDADO
        ' =========================================================================================
        Try
            Dim doc As Mae_Documento

            If _idEdicion.HasValue Then
                ' --- MODO EDICIÓN (UPDATE) ---
                doc = _unitOfWork.Context.Set(Of Mae_Documento)().Find(_idEdicion.Value)
                doc.IdTipo = CInt(cboTipo.SelectedValue)
                doc.NumeroOficial = txtNumeroRef.Text.Trim()
                doc.Asunto = txtAsunto.Text.ToUpper().Trim()
                doc.Descripcion = txtDescripcion.Text.Trim()
                doc.Fojas = CInt(numFojas.Value)
                doc.FechaRecepcion = dtpFechaRecepcion.Value

                _unitOfWork.Commit()
                GuardarAdjuntos(doc.IdDocumento)
                AuditoriaSistema.RegistrarEvento($"Edición de documento {doc.NumeroOficial}...", "DOCUMENTOS", unitOfWorkExterno:=_unitOfWork)
                _unitOfWork.Commit()
                Toast.Show(Me, "✅ Documento corregido exitosamente.", ToastType.Success)

            Else
                ' --- MODO NUEVO (INSERT) ---
                doc = New Mae_Documento()

                ' 1. Generación / Asignación de Número
                If _generacionAutomatica Then
                    Dim idTipo As Integer = CInt(cboTipo.SelectedValue)
                    Dim anioObjetivo As Integer = DateTime.Now.Year

                    ' Cálculo estricto basado en la oficina seleccionada
                    Dim nuevoNumero As Integer = CalcularSiguienteNumero(idTipo, idOrigenSeleccionado.Value, anioObjetivo)

                    If nuevoNumero <= 0 Then
                        Toast.Show(Me, "Error: No se pudo obtener un número válido o el rango de la oficina se agotó.", ToastType.Error)
                        Return
                    End If

                    ' Actualizamos el contador del rango específico
                    Dim misRangos = ObtenerTodosLosRangosActivos(idTipo, idOrigenSeleccionado.Value, anioObjetivo)
                    Dim rangoA_Actualizar = misRangos.FirstOrDefault(Function(r) nuevoNumero >= r.NumeroInicio And nuevoNumero <= r.NumeroFin)

                    If rangoA_Actualizar IsNot Nothing Then
                        rangoA_Actualizar.UltimoUtilizado = nuevoNumero
                    Else
                        Toast.Show(Me, "Error crítico: Se generó un número fuera de rango.", ToastType.Error)
                        Return
                    End If

                    Dim anioCorto As String = DateTime.Now.ToString("yy")
                    doc.NumeroOficial = nuevoNumero.ToString() & "/" & anioCorto
                Else
                    ' Manual (Validado previamente)
                    doc.NumeroOficial = txtNumeroRef.Text.Trim()
                End If

                ' 2. Propiedades del Documento
                doc.IdTipo = CInt(cboTipo.SelectedValue)
                doc.Asunto = txtAsunto.Text.ToUpper().Trim()
                doc.Descripcion = txtDescripcion.Text.Trim()
                doc.Fojas = CInt(numFojas.Value)
                doc.FechaCreacion = DateTime.Now
                doc.FechaRecepcion = dtpFechaRecepcion.Value
                doc.IdEstadoActual = 1
                doc.IdOficinaActual = SesionGlobal.OficinaID
                doc.IdUsuarioCreador = SesionGlobal.UsuarioID

                If _modoRespuesta Then
                    doc.IdHiloConversacion = _guidHilo.Value
                    doc.IdDocumentoPadre = _idDocPadre
                Else
                    doc.IdHiloConversacion = Guid.NewGuid()
                    doc.IdDocumentoPadre = Nothing
                End If

                _unitOfWork.Repository(Of Mae_Documento)().Add(doc)

                ' 3. Trazabilidad (Primer Movimiento)
                Dim mov As New Tra_Movimiento()
                mov.IdDocumento = doc.IdDocumento
                mov.FechaMovimiento = DateTime.Now
                mov.IdOficinaOrigen = idOrigenSeleccionado.Value ' <-- CRÍTICO: Define el origen lógico del documento
                mov.IdOficinaDestino = SesionGlobal.OficinaID

                If _modoRespuesta Then
                    mov.ObservacionPase = If(_idOrigenForzado.HasValue, "Carga de actuación externa (Digitalización).", "Generación de respuesta interna.")
                Else
                    mov.ObservacionPase = "Ingreso inicial de expediente."
                End If

                mov.IdUsuarioResponsable = SesionGlobal.UsuarioID
                mov.IdEstadoEnEseMomento = 1
                doc.Tra_Movimiento.Add(mov)

                _unitOfWork.Commit()

                ' 4. Manejo de Adjuntos
                Try
                    GuardarAdjuntos(doc.IdDocumento)
                Catch exAdjuntos As Exception
                    ' Rollback manual si fallan los adjuntos
                    Try
                        AttachmentStore.DeleteAttachments(doc.IdDocumento)
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
                AuditoriaSistema.RegistrarEvento($"{tipoCarga} de documento {doc.NumeroOficial}...", "DOCUMENTOS", unitOfWorkExterno:=_unitOfWork)
                _unitOfWork.Commit()

                ' Mensaje final
                Dim sb As New System.Text.StringBuilder()
                sb.AppendLine("✅ Documento registrado exitosamente.")
                sb.AppendLine()
                sb.AppendLine("📄 REF: " & cboTipo.Text.ToUpper() & " " & doc.NumeroOficial)
                sb.AppendLine("📌 ASUNTO: " & doc.Asunto)
                sb.AppendLine()

                Toast.Show(Me, sb.ToString(), ToastType.Success)
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

        AddHandler f.FormClosed, Sub(sender2, args2)
                                     If f.DialogResult <> DialogResult.OK Then Return

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
                                 End Sub
        ShowFormInMdi(Me, f)
    End Sub
End Class
