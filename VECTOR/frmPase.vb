Imports System.Data.Entity
Imports System.Text ' Necesario para StringBuilder
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading
Imports System.Threading.Tasks

Public Class frmPase

    Private db As New SecretariaDBEntities()

    ' ID del documento que el usuario clickeó (puede ser hijo o padre)
    Private _idDocumentoSeleccionado As Long

    ' Lista en memoria para guardar TODO lo que se va a enviar
    Private _documentosAEnviar As List(Of Mae_Documento)
    Private _docPadre As Mae_Documento = Nothing

    ' Cache para búsqueda rápida
    Private _todasLasOficinas As List(Of Cat_Oficina)
    Private _filtroCts As CancellationTokenSource
    Private _oficinaSeleccionada As Cat_Oficina
    Private ReadOnly _lstSugerencias As New ListBox()

    ' =============================================================
    ' CONSTRUCTOR
    ' =============================================================
    Public Sub New(idDoc As Long)
        InitializeComponent()
        _idDocumentoSeleccionado = idDoc
    End Sub

    Private Sub frmPase_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarListaSugerencias()

        ' 1. Cargar datos
        CargarDatosIniciales()

        ' 2. Analizar el expediente
        AnalizarPaquete()

        ' 3. AUTOMATIZACIÓN
        txtObservacion.Text = "Se remite para su conocimiento, consideración y fines pertinentes."

        UIUtils.SetPlaceholder(txtBuscar, "Escriba para buscar oficina...")

        If txtBuscar IsNot Nothing Then
            txtBuscar.Focus()
        End If
    End Sub

    Private Sub ConfigurarListaSugerencias()
        _lstSugerencias.Name = "lstSugerencias"
        _lstSugerencias.Font = New Font("Segoe UI", 9.0!)
        _lstSugerencias.Location = cboDestino.Location
        _lstSugerencias.Size = New Size(cboDestino.Width, 110)
        _lstSugerencias.Visible = False
        _lstSugerencias.DisplayMember = "Nombre"
        _lstSugerencias.IntegralHeight = False

        GroupBox1.Controls.Add(_lstSugerencias)
        _lstSugerencias.BringToFront()

        AddHandler _lstSugerencias.DoubleClick, AddressOf lstSugerencias_Seleccionar
        AddHandler _lstSugerencias.MouseClick, AddressOf lstSugerencias_Seleccionar
        AddHandler _lstSugerencias.KeyDown, AddressOf lstSugerencias_KeyDown

        ' Se mantiene oculto: ahora la UX de selección se hace en el ListBox.
        cboDestino.Visible = False
    End Sub

    ' 1. Carga los combos (Destinos)
    Private Sub CargarDatosIniciales()
        Try
            _todasLasOficinas = db.Cat_Oficina.Where(Function(o) o.IdOficina <> SesionGlobal.OficinaID).
                OrderBy(Function(o) o.Nombre).
                ToList()
        Catch ex As Exception
            Toast.Show(Me, "Error al cargar oficinas: " & ex.Message, ToastType.Error)
        End Try
    End Sub

    ' Búsqueda asíncrona + cancelación para evitar lag al tipear.
    Private Async Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        _oficinaSeleccionada = Nothing
        Await FiltrarOficinasAsync(txtBuscar.Text)
    End Sub

    Private Async Function FiltrarOficinasAsync(texto As String) As Task
        If _todasLasOficinas Is Nothing Then Return

        If _filtroCts IsNot Nothing Then
            _filtroCts.Cancel()
            _filtroCts.Dispose()
        End If

        _filtroCts = New CancellationTokenSource()
        Dim token = _filtroCts.Token

        Try
            Await Task.Delay(150, token) ' debounce

            Dim textoNormalizado = texto.Trim()
            Dim listaFiltrada As List(Of Cat_Oficina)

            If String.IsNullOrWhiteSpace(textoNormalizado) Then
                listaFiltrada = New List(Of Cat_Oficina)()
            Else
                listaFiltrada = Await Task.Run(Function()
                                                   Dim terminos = textoNormalizado.ToUpper().
                                                       Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

                                                   Return _todasLasOficinas.
                                                       Where(Function(o) terminos.All(Function(t) o.Nombre.ToUpper().Contains(t))).
                                                       Take(30).
                                                       ToList()
                                               End Function, token)
            End If

            If token.IsCancellationRequested Then Return
            MostrarSugerencias(listaFiltrada)

        Catch ex As OperationCanceledException
            ' Esperado cuando el usuario escribe rápido.
        End Try
    End Function

    Private Sub MostrarSugerencias(lista As List(Of Cat_Oficina))
        _lstSugerencias.BeginUpdate()
        _lstSugerencias.Items.Clear()

        For Each oficina In lista
            _lstSugerencias.Items.Add(oficina)
        Next

        _lstSugerencias.EndUpdate()

        If lista.Count > 0 Then
            _lstSugerencias.SelectedIndex = 0
            _lstSugerencias.Visible = True
        Else
            OcultarSugerencias()
        End If
    End Sub

    Private Sub txtBuscar_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBuscar.KeyDown
        If Not _lstSugerencias.Visible Then Return

        Select Case e.KeyCode
            Case Keys.Down
                If _lstSugerencias.SelectedIndex < _lstSugerencias.Items.Count - 1 Then
                    _lstSugerencias.SelectedIndex += 1
                End If
                e.Handled = True

            Case Keys.Up
                If _lstSugerencias.SelectedIndex > 0 Then
                    _lstSugerencias.SelectedIndex -= 1
                End If
                e.Handled = True

            Case Keys.Enter
                SeleccionarOficinaActual()
                e.SuppressKeyPress = True
                e.Handled = True

            Case Keys.Escape
                OcultarSugerencias()
                e.Handled = True
        End Select
    End Sub

    Private Sub lstSugerencias_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            SeleccionarOficinaActual()
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub

    Private Sub lstSugerencias_Seleccionar(sender As Object, e As EventArgs)
        SeleccionarOficinaActual()
    End Sub

    Private Sub SeleccionarOficinaActual()
        Dim oficina = TryCast(_lstSugerencias.SelectedItem, Cat_Oficina)
        If oficina Is Nothing Then Return

        _oficinaSeleccionada = oficina

        ' Conservamos el combo interno para no romper el flujo existente.
        cboDestino.DataSource = Nothing
        cboDestino.DataSource = New List(Of Cat_Oficina) From {oficina}
        cboDestino.DisplayMember = "Nombre"
        cboDestino.ValueMember = "IdOficina"
        cboDestino.SelectedIndex = 0

        txtBuscar.Text = oficina.Nombre
        txtBuscar.SelectionStart = txtBuscar.TextLength
        OcultarSugerencias()
    End Sub

    Private Sub OcultarSugerencias()
        _lstSugerencias.Visible = False
    End Sub

    ' 2. LÓGICA INTELIGENTE: Detecta qué se está enviando realmente
    Private Sub AnalizarPaquete()
        Try
            ' Buscamos el documento clickeado para saber su GUID (Familia)
            Dim docBase = db.Mae_Documento.Find(_idDocumentoSeleccionado)
            If docBase Is Nothing Then Return

            Dim guidFamilia = docBase.IdHiloConversacion
            Dim miOficina = SesionGlobal.OficinaID

            ' TRAEMOS A TODA LA FAMILIA QUE ESTÁ CONMIGO
            ' Filtros: Mismo GUID, están en MI oficina, y NO están anulados (ID 5)
            _documentosAEnviar = db.Mae_Documento.Where(Function(d) _
                                              d.IdHiloConversacion = guidFamilia And
                                              d.IdOficinaActual = miOficina And
                                              d.IdEstadoActual <> 5).ToList()

            ' IDENTIFICAMOS AL PADRE (JEFE)
            ' El padre es aquel que NO tiene IdDocumentoPadre
            _docPadre = _documentosAEnviar.FirstOrDefault(Function(d) d.IdDocumentoPadre Is Nothing)

            ' Si no encontramos padre (caso raro), tomamos el más antiguo como Jefe
            If _docPadre Is Nothing Then
                _docPadre = _documentosAEnviar.OrderBy(Function(d) d.FechaCreacion).FirstOrDefault()
            End If

            ' PREPARAMOS EL RESUMEN VISUAL
            Dim sb As New StringBuilder()
            Dim totalFojas As Integer = 0

            sb.AppendLine("📦 EXPEDIENTE PRINCIPAL (PADRE):")
            sb.AppendLine("   " & _docPadre.Cat_TipoDocumento.Codigo & " " & _docPadre.NumeroOficial & " - " & _docPadre.Asunto)
            sb.AppendLine("   Fojas Acumuladas: " & _docPadre.Fojas)
            sb.AppendLine()

            totalFojas += _docPadre.Fojas

            ' Listamos los hijos
            Dim hijos = _documentosAEnviar.Where(Function(d) d.IdDocumento <> _docPadre.IdDocumento).ToList()

            If hijos.Count > 0 Then
                sb.AppendLine("📎 CONTIENE " & hijos.Count & " ADJUNTO(S):")
                For Each h In hijos
                    sb.AppendLine("   + " & h.Cat_TipoDocumento.Codigo & " " & h.NumeroOficial & " (" & h.Asunto & ")")
                    totalFojas += h.Fojas
                Next
            Else
                sb.AppendLine("(No contiene documentos adjuntos)")
            End If

            sb.AppendLine()
            sb.AppendLine("📄 TOTAL PAQUETE: " & _documentosAEnviar.Count & " documentos | " & totalFojas & " fojas.")

            txtResumen.Text = sb.ToString()
            txtResumen.SelectionStart = 0
            txtResumen.SelectionLength = 0

        Catch ex As Exception
            txtResumen.Text = "Error al analizar expediente: " & ex.Message
            btnConfirmar.Enabled = False
        End Try
    End Sub

    Private Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        If _oficinaSeleccionada Is Nothing Then
            Toast.Show(Me, "Seleccione Oficina de Destino.", ToastType.Warning)
            txtBuscar.Focus()
            Return
        End If
        If String.IsNullOrWhiteSpace(txtObservacion.Text) Then
            Toast.Show(Me, "La observación es obligatoria.", ToastType.Warning)
            Return
        End If

        Try
            Dim idDestino As Integer = _oficinaSeleccionada.IdOficina
            Dim nombreDestino As String = _oficinaSeleccionada.Nombre
            Dim obs As String = txtObservacion.Text.Trim()
            Dim fojasNuevas As Integer = CInt(numFojasAgregadas.Value)

            ' --- EJECUCIÓN DEL PASE MASIVO ---
            Dim count As Integer = 0

            For Each doc In _documentosAEnviar
                ' 1. Actualizamos ubicación y estado
                doc.IdOficinaActual = idDestino
                doc.IdEstadoActual = 2 ' En Trámite / Enviado

                ' 2. Sumamos fojas (SOLO AL PADRE)
                If doc.IdDocumento = _docPadre.IdDocumento Then
                    doc.Fojas += fojasNuevas
                End If

                ' 3. Generamos Historial
                Dim mov As New Tra_Movimiento()
                mov.IdDocumento = doc.IdDocumento
                mov.FechaMovimiento = DateTime.Now
                mov.IdOficinaOrigen = SesionGlobal.OficinaID
                mov.IdOficinaDestino = idDestino
                mov.IdUsuarioResponsable = SesionGlobal.UsuarioID
                mov.ObservacionPase = obs
                mov.IdEstadoEnEseMomento = 2

                doc.Tra_Movimiento.Add(mov)
                count += 1
            Next

            db.SaveChanges()

            AuditoriaSistema.RegistrarEvento($"Pase de {_documentosAEnviar.Count} documento(s) a {nombreDestino}. Observación: {obs}. Fojas agregadas: {fojasNuevas}.", "PASE")
            Toast.Show(Me, "✅ PASE EXITOSO." & vbCrLf & vbCrLf &
                            "Se han enviado " & count & " documento(s) a " & nombreDestino & ".", ToastType.Success)

            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            Toast.Show(Me, "Error al guardar: " & ex.Message, ToastType.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmPase_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If _filtroCts IsNot Nothing Then
            _filtroCts.Cancel()
            _filtroCts.Dispose()
            _filtroCts = Nothing
        End If
    End Sub

End Class
