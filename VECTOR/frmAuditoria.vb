Imports System.Data.Entity

Public Class frmAuditoria

    ' --- TEMPORIZADORES PARA EVITAR QUE SE TRABE AL ESCRIBIR (DEBOUNCE) ---
    Private WithEvents tmrBusquedaAuditoria As New Timer With {.Interval = 500}
    Private WithEvents tmrBusquedaTransacciones As New Timer With {.Interval = 500}
    Private _cargandoCombos As Boolean = False

    Private Async Sub frmAuditoria_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InicializarFechas()
        CargarCombos()

        ' Cargar datos iniciales
        Await CargarAuditoriaAsync()
        Await CargarTransaccionesAsync()

        ' Suscribir eventos manuales para Combos (para evitar disparos durante la carga)
        SuscribirEventosCambio()
    End Sub

    Private Sub InicializarFechas()
        Dim hoy = Date.Today
        dtpAuditoriaDesde.Value = hoy.AddDays(-7)
        dtpAuditoriaHasta.Value = hoy
        dtpTransaccionesDesde.Value = hoy.AddDays(-7)
        dtpTransaccionesHasta.Value = hoy
    End Sub

    Private Sub CargarCombos()
        _cargandoCombos = True ' Bloqueamos eventos mientras cargamos
        Try
            Using db As New SecretariaDBEntities()
                Dim usuarios = db.Cat_Usuario.OrderBy(Function(u) u.UsuarioLogin).Select(Function(u) New With {
                    .Id = u.IdUsuario,
                    .Nombre = u.UsuarioLogin & " - " & u.NombreCompleto
                }).ToList()

                usuarios.Insert(0, New With {.Id = 0, .Nombre = "Todos"})

                cmbAuditoriaUsuario.DisplayMember = "Nombre"
                cmbAuditoriaUsuario.ValueMember = "Id"
                cmbAuditoriaUsuario.DataSource = usuarios.ToList()

                cmbTransaccionesUsuario.DisplayMember = "Nombre"
                cmbTransaccionesUsuario.ValueMember = "Id"
                cmbTransaccionesUsuario.DataSource = usuarios.ToList()

                Dim modulos = db.EventosSistema.Select(Function(e) e.Modulo).Distinct().OrderBy(Function(m) m).ToList()
                modulos.Insert(0, "Todos")
                cmbAuditoriaModulo.DataSource = modulos

                Dim oficinas = db.Cat_Oficina.OrderBy(Function(o) o.Nombre).Select(Function(o) New With {
                    .Id = o.IdOficina,
                    .Nombre = o.Nombre
                }).ToList()
                oficinas.Insert(0, New With {.Id = 0, .Nombre = "Todas"})

                cmbTransaccionesOrigen.DisplayMember = "Nombre"
                cmbTransaccionesOrigen.ValueMember = "Id"
                cmbTransaccionesOrigen.DataSource = oficinas.ToList()

                cmbTransaccionesDestino.DisplayMember = "Nombre"
                cmbTransaccionesDestino.ValueMember = "Id"
                cmbTransaccionesDestino.DataSource = oficinas.ToList()
            End Using
        Finally
            _cargandoCombos = False ' Liberamos eventos
        End Try
    End Sub

    Private Sub SuscribirEventosCambio()
        ' Eventos para disparar búsqueda al cambiar selección
        AddHandler cmbAuditoriaUsuario.SelectedIndexChanged, AddressOf FiltroAuditoriaCambiado
        AddHandler cmbAuditoriaModulo.SelectedIndexChanged, AddressOf FiltroAuditoriaCambiado
        AddHandler dtpAuditoriaDesde.ValueChanged, AddressOf FiltroAuditoriaCambiado
        AddHandler dtpAuditoriaHasta.ValueChanged, AddressOf FiltroAuditoriaCambiado

        AddHandler cmbTransaccionesUsuario.SelectedIndexChanged, AddressOf FiltroTransaccionesCambiado
        AddHandler cmbTransaccionesOrigen.SelectedIndexChanged, AddressOf FiltroTransaccionesCambiado
        AddHandler cmbTransaccionesDestino.SelectedIndexChanged, AddressOf FiltroTransaccionesCambiado
        AddHandler dtpTransaccionesDesde.ValueChanged, AddressOf FiltroTransaccionesCambiado
        AddHandler dtpTransaccionesHasta.ValueChanged, AddressOf FiltroTransaccionesCambiado
    End Sub

    ' --- EVENTOS DE TEXTO (LÓGICA ANTI-CONGELAMIENTO) ---
    Private Sub txtAuditoriaBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtAuditoriaBuscar.TextChanged
        ' Reiniciar el timer. La búsqueda se hará cuando el usuario deje de escribir por 500ms
        tmrBusquedaAuditoria.Stop()
        tmrBusquedaAuditoria.Start()
    End Sub

    Private Async Sub tmrBusquedaAuditoria_Tick(sender As Object, e As EventArgs) Handles tmrBusquedaAuditoria.Tick
        tmrBusquedaAuditoria.Stop()
        Await CargarAuditoriaAsync() ' Ejecuta la búsqueda real
    End Sub

    Private Sub txtTransaccionesBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtTransaccionesBuscar.TextChanged
        tmrBusquedaTransacciones.Stop()
        tmrBusquedaTransacciones.Start()
    End Sub

    Private Async Sub tmrBusquedaTransacciones_Tick(sender As Object, e As EventArgs) Handles tmrBusquedaTransacciones.Tick
        tmrBusquedaTransacciones.Stop()
        Await CargarTransaccionesAsync() ' Ejecuta la búsqueda real
    End Sub

    ' --- MANEJADORES DE FILTROS INMEDIATOS ---
    Private Async Sub FiltroAuditoriaCambiado(sender As Object, e As EventArgs)
        If _cargandoCombos Then Return
        Await CargarAuditoriaAsync()
    End Sub

    Private Async Sub FiltroTransaccionesCambiado(sender As Object, e As EventArgs)
        If _cargandoCombos Then Return
        Await CargarTransaccionesAsync()
    End Sub

    ' --- CARGA DE DATOS (CON BÚSQUEDA INTELIGENTE) ---
    Private Async Function CargarAuditoriaAsync() As Task
        Try
            Dim desde = dtpAuditoriaDesde.Value.Date
            Dim hasta = dtpAuditoriaHasta.Value.Date.AddDays(1).AddTicks(-1)

            Dim usuarioId As Integer = 0
            Integer.TryParse(Convert.ToString(cmbAuditoriaUsuario.SelectedValue), usuarioId)

            Dim modulo As String = If(cmbAuditoriaModulo.SelectedItem Is Nothing, "Todos", cmbAuditoriaModulo.SelectedItem.ToString())
            Dim texto = txtAuditoriaBuscar.Text.Trim()

            Dim datos = Await Task.Run(Function()
                                           Using db As New SecretariaDBEntities()
                                               Dim query = db.EventosSistema.Include("Cat_Usuario").AsQueryable()
                                               query = query.Where(Function(e) e.FechaEvento >= desde AndAlso e.FechaEvento <= hasta)

                                               If usuarioId > 0 Then
                                                   query = query.Where(Function(e) e.UsuarioId = usuarioId)
                                               End If

                                               If Not String.Equals(modulo, "Todos", StringComparison.OrdinalIgnoreCase) Then
                                                   query = query.Where(Function(e) e.Modulo = modulo)
                                               End If

                                               ' --- BÚSQUEDA INTELIGENTE (PALABRAS SUELTAS) ---
                                               If Not String.IsNullOrWhiteSpace(texto) Then
                                                   Dim palabras = texto.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                                                   For Each palabra In palabras
                                                       Dim p = palabra ' Variable local para lambda
                                                       ' Busca que la palabra exista en Descripcion O Usuario O Modulo
                                                       query = query.Where(Function(e) e.Descripcion.Contains(p) OrElse
                                                                                       e.Cat_Usuario.UsuarioLogin.Contains(p) OrElse
                                                                                       e.Modulo.Contains(p))
                                                   Next
                                               End If
                                               ' ------------------------------------------------

                                               Return query.OrderByDescending(Function(e) e.FechaEvento).Select(Function(e) New With {
                                                   .IdEvento = e.IdEvento,
                                                   .Fecha = e.FechaEvento,
                                                   .Usuario = e.Cat_Usuario.UsuarioLogin,
                                                   .Modulo = e.Modulo,
                                                   .Descripcion = e.Descripcion
                                               }).ToList()
                                           End Using
                                       End Function)

            dgvAuditoria.DataSource = datos
            ConfigurarGridAuditoria()
            MostrarDetalleAuditoria()
        Catch ex As Exception
            ' Evitar msgbox molestos si se está escribiendo rápido, escribir a consola mejor
            Console.WriteLine("Error búsqueda auditoría: " & ex.Message)
        End Try
    End Function

    Private Async Function CargarTransaccionesAsync() As Task
        Try
            Dim desde = dtpTransaccionesDesde.Value.Date
            Dim hasta = dtpTransaccionesHasta.Value.Date.AddDays(1).AddTicks(-1)

            Dim usuarioId As Integer = 0
            Integer.TryParse(Convert.ToString(cmbTransaccionesUsuario.SelectedValue), usuarioId)

            Dim origenId As Integer = 0
            Integer.TryParse(Convert.ToString(cmbTransaccionesOrigen.SelectedValue), origenId)

            Dim destinoId As Integer = 0
            Integer.TryParse(Convert.ToString(cmbTransaccionesDestino.SelectedValue), destinoId)

            Dim texto = txtTransaccionesBuscar.Text.Trim()

            Dim datos = Await Task.Run(Function()
                                           Using db As New SecretariaDBEntities()
                                               Dim query = db.Tra_Movimiento.Include("Mae_Documento.Cat_TipoDocumento").Include("Cat_Oficina").Include("Cat_Oficina1").Include("Cat_Usuario").Include("Cat_Estado").AsQueryable()
                                               query = query.Where(Function(m) m.FechaMovimiento >= desde AndAlso m.FechaMovimiento <= hasta)

                                               If usuarioId > 0 Then
                                                   query = query.Where(Function(m) m.IdUsuarioResponsable.HasValue AndAlso m.IdUsuarioResponsable.Value = usuarioId)
                                               End If

                                               If origenId > 0 Then
                                                   query = query.Where(Function(m) m.IdOficinaOrigen = origenId)
                                               End If

                                               If destinoId > 0 Then
                                                   query = query.Where(Function(m) m.IdOficinaDestino = destinoId)
                                               End If

                                               ' --- BÚSQUEDA INTELIGENTE AMPLIA (PALABRAS SUELTAS) ---
                                               If Not String.IsNullOrWhiteSpace(texto) Then
                                                   Dim palabras = texto.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                                                   For Each palabra In palabras
                                                       Dim p = palabra
                                                       ' La palabra debe estar en ALGUNO de estos campos.
                                                       ' Al iterar con "Where", forzamos a que CADA palabra del buscador coincida con algo.
                                                       query = query.Where(Function(m) m.Mae_Documento.NumeroOficial.Contains(p) OrElse
                                                                                       m.Mae_Documento.NumeroInterno.Contains(p) OrElse
                                                                                       m.Mae_Documento.Asunto.Contains(p) OrElse
                                                                                       m.ObservacionPase.Contains(p) OrElse
                                                                                       m.Cat_Oficina.Nombre.Contains(p) OrElse
                                                                                       m.Cat_Oficina1.Nombre.Contains(p))
                                                   Next
                                               End If
                                               ' --------------------------------------------------------

                                               Return query.OrderByDescending(Function(m) m.FechaMovimiento).Select(Function(m) New With {
                                                   .IdMovimiento = m.IdMovimiento,
                                                   .Fecha = m.FechaMovimiento,
                                                   .Documento = m.Mae_Documento.Cat_TipoDocumento.Codigo & " " & m.Mae_Documento.NumeroOficial,
                                                   .Origen = m.Cat_Oficina.Nombre,
                                                   .Destino = m.Cat_Oficina1.Nombre,
                                                   .Estado = m.Cat_Estado.Nombre,
                                                   .Responsable = If(m.Cat_Usuario Is Nothing, "", m.Cat_Usuario.UsuarioLogin),
                                                   .Observacion = m.ObservacionPase
                                               }).ToList()
                                           End Using
                                       End Function)

            dgvTransacciones.DataSource = datos
            ' ConfigurarGridTransacciones se llama en DataBindingComplete
            MostrarDetalleTransaccion()
        Catch ex As Exception
            Console.WriteLine("Error búsqueda transacciones: " & ex.Message)
        End Try
    End Function

    ' --- RESTO DE MÉTODOS DE VISUALIZACIÓN (IGUAL QUE ANTES) ---

    Private Sub dgvTransacciones_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvTransacciones.DataBindingComplete
        ConfigurarGridTransacciones()
    End Sub

    Private Sub ConfigurarGridAuditoria()
        If dgvAuditoria.Columns.Count = 0 Then Return
        If dgvAuditoria.Columns.Contains("IdEvento") Then dgvAuditoria.Columns("IdEvento").Visible = False
        If dgvAuditoria.Columns.Contains("Fecha") Then
            dgvAuditoria.Columns("Fecha").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            dgvAuditoria.Columns("Fecha").Width = 140
        End If
        If dgvAuditoria.Columns.Contains("Usuario") Then dgvAuditoria.Columns("Usuario").Width = 160
        If dgvAuditoria.Columns.Contains("Modulo") Then dgvAuditoria.Columns("Modulo").Width = 120
        If dgvAuditoria.Columns.Contains("Descripcion") Then dgvAuditoria.Columns("Descripcion").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvAuditoria.ClearSelection()
    End Sub

    Private Sub ConfigurarGridTransacciones()
        If dgvTransacciones Is Nothing OrElse dgvTransacciones.Columns.Count = 0 Then Return
        Try
            dgvTransacciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            AjustarColumna("IdMovimiento", visible:=False)
            AjustarColumna("Fecha", width:=140, formato:="dd/MM/yyyy HH:mm")
            AjustarColumna("Documento", width:=160)
            AjustarColumna("Origen", width:=160)
            AjustarColumna("Destino", width:=160)
            AjustarColumna("Estado", width:=120)
            AjustarColumna("Responsable", width:=120)
            AjustarColumnaFill("Observacion")
            dgvTransacciones.ClearSelection()
        Catch ex As Exception
            Console.WriteLine("Error configurando grid: " & ex.Message)
        End Try
    End Sub

    Private Sub AjustarColumna(nombre As String, Optional width As Integer? = Nothing, Optional formato As String = Nothing, Optional visible As Boolean? = Nothing)
        Dim col = ObtenerColumnaTransaccion(nombre)
        If col Is Nothing OrElse col.DataGridView Is Nothing Then Exit Sub
        If visible.HasValue Then col.Visible = visible.Value
        If width.HasValue Then
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            col.Width = width.Value
        End If
        If Not String.IsNullOrEmpty(formato) Then col.DefaultCellStyle.Format = formato
    End Sub

    Private Sub AjustarColumnaFill(nombre As String)
        Dim col = ObtenerColumnaTransaccion(nombre)
        If col IsNot Nothing AndAlso col.DataGridView IsNot Nothing Then
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
    End Sub

    Private Function ObtenerColumnaTransaccion(nombre As String) As DataGridViewColumn
        For Each columna As DataGridViewColumn In dgvTransacciones.Columns
            If String.Equals(columna.Name, nombre, StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(columna.DataPropertyName, nombre, StringComparison.OrdinalIgnoreCase) Then
                Return columna
            End If
        Next
        Return Nothing
    End Function

    Private Function ObtenerValorCelda(row As DataGridViewRow, nombreColumna As String) As String
        Dim col = ObtenerColumnaTransaccion(nombreColumna)
        If col Is Nothing Then Return String.Empty
        Dim valor = row.Cells(col.Index).Value
        Return If(valor Is Nothing, String.Empty, Convert.ToString(valor))
    End Function

    Private Async Sub btnAuditoriaBuscar_Click(sender As Object, e As EventArgs) Handles btnAuditoriaBuscar.Click
        Await CargarAuditoriaAsync()
    End Sub

    Private Async Sub btnAuditoriaLimpiar_Click(sender As Object, e As EventArgs) Handles btnAuditoriaLimpiar.Click
        InicializarFechas()
        txtAuditoriaBuscar.Clear()
        cmbAuditoriaUsuario.SelectedIndex = 0
        cmbAuditoriaModulo.SelectedIndex = 0
        Await CargarAuditoriaAsync()
    End Sub

    Private Async Sub btnTransaccionesBuscar_Click(sender As Object, e As EventArgs) Handles btnTransaccionesBuscar.Click
        Await CargarTransaccionesAsync()
    End Sub

    Private Async Sub btnTransaccionesLimpiar_Click(sender As Object, e As EventArgs) Handles btnTransaccionesLimpiar.Click
        InicializarFechas()
        txtTransaccionesBuscar.Clear()
        cmbTransaccionesUsuario.SelectedIndex = 0
        cmbTransaccionesOrigen.SelectedIndex = 0
        cmbTransaccionesDestino.SelectedIndex = 0
        Await CargarTransaccionesAsync()
    End Sub

    Private Sub dgvAuditoria_SelectionChanged(sender As Object, e As EventArgs) Handles dgvAuditoria.SelectionChanged
        MostrarDetalleAuditoria()
    End Sub

    Private Sub dgvTransacciones_SelectionChanged(sender As Object, e As EventArgs) Handles dgvTransacciones.SelectionChanged
        MostrarDetalleTransaccion()
    End Sub

    Private Sub MostrarDetalleAuditoria()
        If dgvAuditoria.CurrentRow Is Nothing Then
            lblDetalleAuditoriaFecha.Text = "Fecha:"
            lblDetalleAuditoriaUsuario.Text = "Usuario:"
            lblDetalleAuditoriaModulo.Text = "Módulo:"
            txtDetalleAuditoriaDescripcion.Text = String.Empty
            Return
        End If
        Dim fila = dgvAuditoria.CurrentRow
        lblDetalleAuditoriaFecha.Text = "Fecha: " & Convert.ToDateTime(fila.Cells("Fecha").Value).ToString("dd/MM/yyyy HH:mm")
        lblDetalleAuditoriaUsuario.Text = "Usuario: " & Convert.ToString(fila.Cells("Usuario").Value)
        lblDetalleAuditoriaModulo.Text = "Módulo: " & Convert.ToString(fila.Cells("Modulo").Value)
        txtDetalleAuditoriaDescripcion.Text = Convert.ToString(fila.Cells("Descripcion").Value)
    End Sub

    Private Sub MostrarDetalleTransaccion()
        If dgvTransacciones.CurrentRow Is Nothing Then
            lblDetalleTransaccionFecha.Text = "Fecha:"
            lblDetalleTransaccionDocumento.Text = "Documento:"
            lblDetalleTransaccionOrigen.Text = "Origen:"
            lblDetalleTransaccionDestino.Text = "Destino:"
            lblDetalleTransaccionEstado.Text = "Estado:"
            lblDetalleTransaccionResponsable.Text = "Responsable:"
            txtDetalleTransaccionObservacion.Text = String.Empty
            Return
        End If
        Dim fila = dgvTransacciones.CurrentRow
        Dim fechaStr = ObtenerValorCelda(fila, "Fecha")
        Dim fecha As DateTime
        If DateTime.TryParse(fechaStr, fecha) Then
            lblDetalleTransaccionFecha.Text = "Fecha: " & fecha.ToString("dd/MM/yyyy HH:mm")
        Else
            lblDetalleTransaccionFecha.Text = "Fecha: " & fechaStr
        End If
        Dim documento = ObtenerValorCelda(fila, "Documento")
        Dim origen = ObtenerValorCelda(fila, "Origen")
        Dim destino = ObtenerValorCelda(fila, "Destino")
        Dim observacion = ObtenerValorCelda(fila, "Observacion")

        lblDetalleTransaccionDocumento.Text = "Documento: " & documento
        lblDetalleTransaccionOrigen.Text = "Origen: " & origen
        lblDetalleTransaccionDestino.Text = "Destino: " & destino
        lblDetalleTransaccionEstado.Text = "Estado: " & ObtenerValorCelda(fila, "Estado")
        lblDetalleTransaccionResponsable.Text = "Responsable: " & ObtenerValorCelda(fila, "Responsable")
        txtDetalleTransaccionObservacion.Text = String.Join(Environment.NewLine,
                                                           "Documento: " & documento,
                                                           "Origen: " & origen,
                                                           "Destino: " & destino,
                                                           String.Empty,
                                                           observacion)
    End Sub

End Class
