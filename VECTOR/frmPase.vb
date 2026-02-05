Imports System.Data.Entity
Imports System.Text ' Necesario para StringBuilder
Imports System.Collections.Generic
Imports System.Linq

Public Class frmPase

    Private db As New SecretariaDBEntities()

    ' ID del documento que el usuario clickeó (puede ser hijo o padre)
    Private _idDocumentoSeleccionado As Long

    ' Lista en memoria para guardar TODO lo que se va a enviar
    Private _documentosAEnviar As List(Of Mae_Documento)
    Private _docPadre As Mae_Documento = Nothing

    ' --- NUEVO: Cache para búsqueda rápida ---
    Private _todasLasOficinas As List(Of Cat_Oficina)

    ' =============================================================
    ' CONSTRUCTOR
    ' =============================================================
    Public Sub New(idDoc As Long)
        InitializeComponent()
        _idDocumentoSeleccionado = idDoc
    End Sub

    Private Sub frmPase_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)

        ' 1. Cargar datos
        CargarDatosIniciales()

        ' 2. Analizar el expediente
        AnalizarPaquete()

        ' 3. AUTOMATIZACIÓN
        txtObservacion.Text = "Se remite para su conocimiento, consideración y fines pertinentes."

        ' --- AQUI AGREGAMOS EL PLACEHOLDER CORRECTAMENTE ---
        UIUtils.SetPlaceholder(txtBuscar, "Escriba para buscar oficina...")
        ' ---------------------------------------------------

        ' Ponemos el foco en el buscador
        If txtBuscar IsNot Nothing Then
            txtBuscar.Focus()
        End If
    End Sub

    ' 1. Carga los combos (Destinos)
    Private Sub CargarDatosIniciales()
        Try
            ' Guardamos TODAS las oficinas (menos la mía) en memoria
            ' Esto evita ir a la base de datos cada vez que escribes una letra
            _todasLasOficinas = db.Cat_Oficina.Where(Function(o) o.IdOficina <> SesionGlobal.OficinaID).OrderBy(Function(o) o.Nombre).ToList()

            ' Llenamos el combo inicialmente con todo
            ActualizarComboDestinos(_todasLasOficinas)

        Catch ex As Exception
            Toast.Show(Me, "Error al cargar oficinas: " & ex.Message, ToastType.Error)
        End Try
    End Sub

    ' --- LÓGICA DE BÚSQUEDA INTELIGENTE ---
    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        FiltrarOficinas(txtBuscar.Text)
    End Sub

    Private Sub FiltrarOficinas(texto As String)
        ' Protección por si la lista aún no cargó
        If _todasLasOficinas Is Nothing Then Return

        Dim listaFiltrada As List(Of Cat_Oficina)

        If String.IsNullOrWhiteSpace(texto) Then
            listaFiltrada = _todasLasOficinas
        Else
            ' CORRECCIÓN 1: Usamos 'New Char() {" "c}' para que Split acepte las opciones
            Dim terminos = texto.ToUpper().Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

            ' CORRECCIÓN 2: Simplificamos a una sola línea para evitar el error de "Falta End Function"
            ' La lógica es: Para cada oficina (o), verificamos que SU nombre contenga TODOS los términos buscados.
            listaFiltrada = _todasLasOficinas.Where(Function(o) terminos.All(Function(t) o.Nombre.ToUpper().Contains(t))).ToList()
        End If

        ActualizarComboDestinos(listaFiltrada)
    End Sub

    Private Sub ActualizarComboDestinos(lista As List(Of Cat_Oficina))
        cboDestino.DataSource = Nothing
        cboDestino.DataSource = lista
        cboDestino.DisplayMember = "Nombre"
        cboDestino.ValueMember = "IdOficina"

        ' Si hay resultados y el usuario escribió algo, seleccionamos el primero y desplegamos
        If lista.Count > 0 AndAlso Not String.IsNullOrWhiteSpace(txtBuscar.Text) Then
            cboDestino.SelectedIndex = 0
            cboDestino.DroppedDown = True
        Else
            cboDestino.SelectedIndex = -1
        End If
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

            ' --- CAMBIO: USAMOS EL TEXTBOX SCROLLABLE ---
            txtResumen.Text = sb.ToString()

            ' Truco visual: Quitamos la selección azul automática para que se vea limpio
            txtResumen.SelectionStart = 0
            txtResumen.SelectionLength = 0
            ' --------------------------------------------

        Catch ex As Exception
            txtResumen.Text = "Error al analizar expediente: " & ex.Message
            btnConfirmar.Enabled = False
        End Try
    End Sub

    Private Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        ' Validaciones
        If cboDestino.SelectedIndex = -1 Then
            Toast.Show(Me, "Seleccione Oficina de Destino.", ToastType.Warning)
            txtBuscar.Focus()
            Return
        End If
        If String.IsNullOrWhiteSpace(txtObservacion.Text) Then
            Toast.Show(Me, "La observación es obligatoria.", ToastType.Warning)
            Return
        End If

        Try
            Dim idDestino As Integer = CInt(cboDestino.SelectedValue)
            Dim obs As String = txtObservacion.Text.Trim()
            Dim fojasNuevas As Integer = CInt(numFojasAgregadas.Value)

            ' --- EJECUCIÓN DEL PASE MASIVO ---
            Dim count As Integer = 0

            For Each doc In _documentosAEnviar
                ' 1. Actualizamos ubicación y estado
                doc.IdOficinaActual = idDestino
                doc.IdEstadoActual = 2 ' En Trámite / Enviado

                ' 2. Sumamos fojas (SOLO AL PADRE)
                ' Esto es importante: si agregas fojas, se suman al cuerpo principal, no a los adjuntos.
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

            AuditoriaSistema.RegistrarEvento($"Pase de {_documentosAEnviar.Count} documento(s) a {cboDestino.Text}. Observación: {obs}. Fojas agregadas: {fojasNuevas}.", "PASE")
            Toast.Show(Me, "✅ PASE EXITOSO." & vbCrLf & vbCrLf &
                            "Se han enviado " & count & " documento(s) a " & cboDestino.Text & ".", ToastType.Success)

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

End Class