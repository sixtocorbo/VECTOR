Imports System.Data.Entity
Imports System.Data.Entity.Validation
Imports System.Drawing ' Necesario para manipulación de colores

Public Class frmGestionRangos

    ' Variable para controlar si estamos editando (0 = Nuevo, >0 = ID del rango)
    Private _idEdicion As Integer = 0
    Private _ultimoInicioSugerido As Integer? = Nothing
    Private _ultimoUltimoSugerido As Integer? = Nothing
    Private _sugerenciaEnCurso As Boolean = False

    ' =============================================================================================
    ' REGIÓN: STOCK SECRETARÍA GENERAL (Variables a Nivel de Clase)
    ' =============================================================================================
    ' Simulación de cupos otorgados por la Secretaría General.
    ' En producción, esto podría leerse de una tabla 'Mae_CuposAnuales'.
    Private _stockTotalMemorandos As Integer = 5000   ' Cupo: 5.000 Memorandos
    Private _stockTotalNotas As Integer = 2000        ' Cupo: 2.000 Notas
    Private _stockTotalResoluciones As Integer = 1000 ' Cupo: 1.000 Resoluciones
    Private _stockTotalDictamenes As Integer = 500    ' Cupo: 500 Dictámenes
    Private _stockDefault As Integer = 500            ' Cupo base para otros tipos

    ' Diccionario para mapear (IdTipo -> CantidadTotal)
    Private _limitesPorTipo As Dictionary(Of Integer, Integer)
    ' =============================================================================================

    ' Estructura simple para cálculo de huecos
    Private Class RangoSimple
        Public Property Inicio As Integer
        Public Property Fin As Integer
    End Class

    Private Async Sub frmGestionRangos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)

        ' Configuración del año
        Dim anioActual As Integer = DateTime.Now.Year
        If anioActual < numAnio.Minimum Then
            numAnio.Value = numAnio.Minimum
        ElseIf anioActual > numAnio.Maximum Then
            numAnio.Value = numAnio.Maximum
        Else
            numAnio.Value = anioActual
        End If

        Await CargarListasAsync()

        ' Inicializamos los límites de stock (Simulación de Secretaría General)
        InicializarStockSecretaria()

        Await CargarGrillaAsync()
        ModoEdicion(False)
    End Sub

    Private Sub frmGestionRangos_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.ShowIcon = False
    End Sub

    ' =======================================================
    ' LÓGICA DE STOCK (SECRETARÍA GENERAL)
    ' =======================================================
    Private Sub InicializarStockSecretaria()
        _limitesPorTipo = New Dictionary(Of Integer, Integer)()

        ' Mapeamos los cupos basándonos en el Nombre del tipo (ya que los IDs pueden variar)
        If cmbTipo.DataSource IsNot Nothing Then
            Dim listaTipos = TryCast(cmbTipo.DataSource, List(Of Cat_TipoDocumento))
            If listaTipos IsNot Nothing Then
                For Each t In listaTipos
                    Dim cantidad As Integer = _stockDefault

                    ' Asignación inteligente por palabras clave
                    Dim nombreUpper = t.Nombre.ToUpper()
                    If nombreUpper.Contains("MEMORANDO") Then cantidad = _stockTotalMemorandos
                    If nombreUpper.Contains("NOTA") Then cantidad = _stockTotalNotas
                    If nombreUpper.Contains("RESOLUCION") Then cantidad = _stockTotalResoluciones
                    If nombreUpper.Contains("DICTAMEN") Then cantidad = _stockTotalDictamenes

                    _limitesPorTipo.Add(t.IdTipo, cantidad)
                Next
            End If
        End If
    End Sub

    Private Async Function ObtenerStockRestanteAsync(idTipo As Integer, anio As Integer) As Task(Of Integer)
        ' 1. ¿Cuánto nos dio Secretaría General?
        Dim limiteTotal As Integer = _stockDefault
        If _limitesPorTipo IsNot Nothing AndAlso _limitesPorTipo.ContainsKey(idTipo) Then
            limiteTotal = _limitesPorTipo(idTipo)
        End If

        ' 2. ¿Cuánto ya hemos repartido entre todas las oficinas?
        Dim asignadoEnBD As Integer = 0
        Using uow As New UnitOfWork()
            ' Sumamos la cantidad de números (Fin - Inicio + 1) de todos los rangos activos
            Dim rangos = Await uow.Repository(Of Mae_NumeracionRangos)().GetQueryable().
                Where(Function(r) r.IdTipo = idTipo And r.Anio = anio And r.Activo = True).
                ToListAsync()

            For Each r In rangos
                ' Si estamos editando un rango, NO restamos su valor actual (porque lo vamos a sobrescribir)
                If _idEdicion > 0 AndAlso r.IdRango = _idEdicion Then Continue For

                asignadoEnBD += (r.NumeroFin - r.NumeroInicio + 1)
            Next
        End Using

        ' 3. Cálculo final (Cupo Total - Asignado)
        Return limiteTotal - asignadoEnBD
    End Function

    Private Async Sub ActualizarVisualizadorStock()
        ' Verifica si existe el label visual (agrégalo en el diseñador si no existe: lblStockInfo)
        ' Si no existe el control, salimos para evitar error, pero la lógica de validación interna sigue funcionando.
        If Me.Controls.Find("lblStockInfo", True).Length = 0 Then Return

        Dim lblStock = Me.Controls.Find("lblStockInfo", True)(0)

        If cmbTipo.SelectedIndex = -1 Then
            lblStock.Text = "---"
            Return
        End If

        Dim idTipo As Integer = CInt(cmbTipo.SelectedValue)
        Dim anio As Integer = CInt(numAnio.Value)

        Dim restante = Await ObtenerStockRestanteAsync(idTipo, anio)

        lblStock.Text = $"Cupo Secretaría: {restante} disponibles"

        If restante <= 0 Then
            lblStock.ForeColor = Color.Red
            lblStock.Text &= " (AGOTADO)"
        ElseIf restante < 100 Then
            lblStock.ForeColor = Color.Orange
        Else
            lblStock.ForeColor = Color.Green
        End If
    End Sub

    ' =======================================================
    ' CÁLCULOS EN TIEMPO REAL (UI)
    ' =======================================================
    Private Sub ActualizarFinDesdeCantidad()
        Dim ini As Integer
        Dim cantidad As Integer

        If Not Integer.TryParse(txtInicio.Text, ini) Then
            txtFin.Clear()
            Return
        End If

        If Not Integer.TryParse(txtCantidad.Text, cantidad) Then
            txtFin.Clear()
            Return
        End If

        If cantidad < 0 Then
            txtFin.Clear()
            Return
        End If

        ' Cálculo exacto: Inicio + Cantidad - 1
        Dim finCalculado As Integer = ini + cantidad - 1
        txtFin.Text = finCalculado.ToString()
    End Sub

    Private Sub txtInicio_TextChanged(sender As Object, e As EventArgs) Handles txtInicio.TextChanged
        ActualizarFinDesdeCantidad()
    End Sub

    Private Async Sub txtCantidad_TextChanged(sender As Object, e As EventArgs) Handles txtCantidad.TextChanged
        ActualizarFinDesdeCantidad()
        Await SugerirInicioDisponibleAsync()
    End Sub

    Private Async Sub cmbTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipo.SelectedIndexChanged
        ActualizarVisualizadorStock() ' <--- Actualiza el label de stock
        Await SugerirInicioDisponibleAsync()
    End Sub

    Private Async Sub numAnio_ValueChanged(sender As Object, e As EventArgs) Handles numAnio.ValueChanged
        ActualizarVisualizadorStock() ' <--- Actualiza el label de stock si cambia el año
        Await SugerirInicioDisponibleAsync()
    End Sub

    ' =======================================================
    ' CARGA DE DATOS
    ' =======================================================
    Private Async Function CargarListasAsync() As Task
        Using uow As New UnitOfWork()
            ' 1. Tipos de Documento
            Dim repoTipos = uow.Repository(Of Cat_TipoDocumento)()
            cmbTipo.DataSource = Await repoTipos.GetQueryable().OrderBy(Function(t) t.Nombre).ToListAsync()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "IdTipo"
            cmbTipo.SelectedIndex = -1

            ' 2. Oficinas (INCLUYENDO BANDEJA ID 13 - AHORA ES IGUAL A LAS DEMÁS)
            Dim repoOficinas = uow.Repository(Of Cat_Oficina)()

            Dim listaOficinas = Await repoOficinas.GetQueryable().
                                      OrderBy(Function(o) o.Nombre).
                                      ToListAsync()

            cmbOficina.DataSource = listaOficinas
            cmbOficina.DisplayMember = "Nombre"
            cmbOficina.ValueMember = "IdOficina"
            cmbOficina.SelectedIndex = -1
        End Using
    End Function

    Private Async Function CargarGrillaAsync() As Task
        Using uow As New UnitOfWork()
            Dim repoRangos = uow.Repository(Of Mae_NumeracionRangos)()

            ' --- CORRECCIÓN DEL ERROR DE CARGA (InvalidCastException) ---
            ' Pasamos una sola cadena con las tablas separadas por coma, no dos argumentos.
            Dim lista = Await (From r In repoRangos.GetQueryable("Cat_TipoDocumento,Cat_Oficina")
                               Select New With {
                                   .Id = r.IdRango,
                                   .Tipo = r.Cat_TipoDocumento.Codigo,
                                   .Oficina = If(r.Cat_Oficina IsNot Nothing, r.Cat_Oficina.Nombre, "SIN ASIGNAR"),
                                   .Nombre = r.NombreRango,
                                   .Anio = r.Anio,
                                   .Inicio = r.NumeroInicio,
                                   .Fin = r.NumeroFin,
                                   .Actual = r.UltimoUtilizado,
                                   .Vigente = r.Activo
                               }).OrderByDescending(Function(r) r.Id).ToListAsync()

            dgvRangos.DataSource = lista

            ' Ajustes visuales
            If dgvRangos.Columns.Count > 0 Then
                dgvRangos.Columns("Id").Visible = False
                dgvRangos.Columns("Tipo").Width = 60
                dgvRangos.Columns("Oficina").Width = 200
                dgvRangos.Columns("Nombre").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                dgvRangos.Columns("Anio").Width = 50
                dgvRangos.Columns("Inicio").Width = 70
                dgvRangos.Columns("Fin").Width = 70
                dgvRangos.Columns("Actual").Width = 70
                dgvRangos.Columns("Vigente").Width = 60
            End If
        End Using
    End Function

    ' =========================================================================
    ' LÓGICA DE SUGERENCIA DE HUECOS
    ' =========================================================================
    Private Async Function SugerirInicioDisponibleAsync() As Task
        If _idEdicion <> 0 Then Return
        If _sugerenciaEnCurso Then Return
        If cmbTipo.SelectedIndex = -1 Then Return

        Dim cantidad As Integer
        If Not Integer.TryParse(txtCantidad.Text, cantidad) Then Return
        If cantidad <= 0 Then Return

        ' Lógica de reemplazo (Solo si el usuario no ha escrito manualmente algo distinto)
        Dim puedeReemplazar As Boolean = String.IsNullOrWhiteSpace(txtInicio.Text)
        If Not puedeReemplazar AndAlso _ultimoInicioSugerido.HasValue Then
            If txtInicio.Text.Trim() = _ultimoInicioSugerido.Value.ToString() Then puedeReemplazar = True
        End If

        If Not puedeReemplazar Then Return

        _sugerenciaEnCurso = True
        Try
            Dim idTipo As Integer = Convert.ToInt32(cmbTipo.SelectedValue)
            Dim anio As Integer = CInt(numAnio.Value)

            Using uow As New UnitOfWork()
                ' Buscamos TODOS los rangos ocupados globalmente
                Dim rangosOcupados = Await uow.Repository(Of Mae_NumeracionRangos)().GetQueryable().
                    Where(Function(x) x.IdTipo = idTipo And x.Anio = anio And x.Activo = True).
                    Select(Function(x) New RangoSimple With {
                        .Inicio = x.NumeroInicio,
                        .Fin = x.NumeroFin
                    }).ToListAsync()

                Dim sugerido As Integer = CalcularHuecoGlobal(rangosOcupados, cantidad)
                AplicarSugerencia(sugerido)
            End Using
        Finally
            _sugerenciaEnCurso = False
        End Try
    End Function

    Private Function CalcularHuecoGlobal(rangos As List(Of RangoSimple), cantidad As Integer) As Integer
        If rangos Is Nothing OrElse rangos.Count = 0 Then Return 1

        ' Ordenamos por inicio
        Dim ordenados = rangos.OrderBy(Function(r) r.Inicio).ToList()

        ' 1. Verificar si cabe antes del primero
        If ordenados(0).Inicio > 1 Then
            Dim huecoInicial = ordenados(0).Inicio - 1
            If huecoInicial >= cantidad Then Return 1
        End If

        ' 2. Buscar huecos entre rangos
        For i As Integer = 0 To ordenados.Count - 2
            Dim rangoActual = ordenados(i)
            Dim rangoSiguiente = ordenados(i + 1)

            Dim finActual = rangoActual.Fin
            Dim inicioSiguiente = rangoSiguiente.Inicio

            ' Si hay espacio entre Fin del actual e Inicio del siguiente
            If (inicioSiguiente - finActual) > 1 Then
                Dim espacioDisponible = (inicioSiguiente - finActual) - 1
                If espacioDisponible >= cantidad Then
                    Return finActual + 1
                End If
            End If
        Next

        ' 3. Si no cupo en el medio, va al final del último
        Dim ultimoFin = ordenados.Last().Fin
        Return ultimoFin + 1
    End Function

    Private Sub AplicarSugerencia(sugerido As Integer)
        _ultimoInicioSugerido = sugerido
        txtInicio.Text = sugerido.ToString()

        Dim ult As Integer = Math.Max(0, sugerido - 1)
        _ultimoUltimoSugerido = ult
        txtUltimo.Text = ult.ToString()
    End Sub

    ' =======================================================
    ' GESTIÓN DE EDICIÓN
    ' =======================================================
    Private Sub ModoEdicion(habilitar As Boolean)
        pnlEditor.Enabled = habilitar
        btnNuevo.Enabled = Not habilitar
        btnEditar.Enabled = Not habilitar
        btnEliminar.Enabled = Not habilitar
        dgvRangos.Enabled = Not habilitar

        txtInicio.ReadOnly = False
        txtUltimo.ReadOnly = False
        txtInicio.BackColor = Color.White
        txtUltimo.BackColor = Color.White

        If Not habilitar Then
            LimpiarCampos()
            _idEdicion = 0
            ActualizarVisualizadorStock()
        End If
    End Sub

    Private Sub LimpiarCampos()
        txtNombre.Clear()
        txtInicio.Text = ""
        txtFin.Text = ""
        txtCantidad.Text = "100"
        txtUltimo.Text = "0"
        chkActivo.Checked = True
        cmbTipo.SelectedIndex = -1
        cmbOficina.SelectedIndex = -1
        _ultimoInicioSugerido = Nothing
        _ultimoUltimoSugerido = Nothing
    End Sub

    ' =======================================================
    ' BOTONES DE ACCIÓN
    ' =======================================================
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        ModoEdicion(True)
        cmbTipo.Focus()
    End Sub

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvRangos.SelectedRows.Count = 0 Then Return

        _idEdicion = Convert.ToInt32(dgvRangos.SelectedRows(0).Cells("Id").Value)

        Using uow As New UnitOfWork()
            Dim r = Await uow.Repository(Of Mae_NumeracionRangos)().GetByIdAsync(_idEdicion)

            If r IsNot Nothing Then
                cmbTipo.SelectedValue = r.IdTipo
                If r.IdOficina.HasValue Then
                    cmbOficina.SelectedValue = r.IdOficina.Value
                End If

                txtNombre.Text = r.NombreRango
                txtInicio.Text = r.NumeroInicio.ToString()
                numAnio.Value = If(r.Anio > 0, r.Anio, DateTime.Now.Year)

                Dim cantidad = (r.NumeroFin - r.NumeroInicio) + 1
                txtCantidad.Text = cantidad.ToString()

                txtFin.Text = r.NumeroFin.ToString()
                txtUltimo.Text = r.UltimoUtilizado.ToString()
                chkActivo.Checked = r.Activo

                ModoEdicion(True)
            End If
        End Using
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        ModoEdicion(False)
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvRangos.SelectedRows.Count = 0 Then
            Toast.Show(Me, "Seleccione un rango.", ToastType.Warning)
            Return
        End If

        If MessageBox.Show("¿Está seguro de eliminar este rango? Esto podría afectar la generación de números.", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then Return

        Dim idRango = Convert.ToInt32(dgvRangos.SelectedRows(0).Cells("Id").Value)

        Using uow As New UnitOfWork()
            uow.Repository(Of Mae_NumeracionRangos)().RemoveById(idRango)
            Await uow.CommitAsync()
        End Using

        Toast.Show(Me, "Rango eliminado.", ToastType.Success)
        ActualizarVisualizadorStock()
        Await CargarGrillaAsync()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' 1. Validaciones Básicas
        If cmbTipo.SelectedIndex = -1 Then
            Toast.Show(Me, "Seleccione el Tipo de Documento.", ToastType.Warning)
            Return
        End If
        If cmbOficina.SelectedIndex = -1 Then
            Toast.Show(Me, "Debe asignar el rango a una Oficina.", ToastType.Warning)
            Return
        End If

        Dim ini, fin, ult, cantidad As Integer
        If Not Integer.TryParse(txtInicio.Text, ini) OrElse Not Integer.TryParse(txtCantidad.Text, cantidad) Then
            Toast.Show(Me, "Datos numéricos inválidos.", ToastType.Warning)
            Return
        End If

        fin = ini + cantidad - 1
        Integer.TryParse(txtUltimo.Text, ult)

        If ult < (ini - 1) Or ult > fin Then
            Toast.Show(Me, "El campo 'Último Utilizado' está fuera del rango.", ToastType.Warning)
            Return
        End If

        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            txtNombre.Text = $"{cmbTipo.Text} {numAnio.Value} - {cmbOficina.Text}"
        End If

        ' 2. VALIDACIÓN DE STOCK (Control Secretaría General)
        Dim idTipo As Integer = CInt(cmbTipo.SelectedValue)
        Dim anio As Integer = CInt(numAnio.Value)

        Dim stockRestante = Await ObtenerStockRestanteAsync(idTipo, anio)

        ' Si la cantidad solicitada supera el stock disponible...
        If cantidad > stockRestante Then
            Dim msg As String = $"STOCK INSUFICIENTE.{vbCrLf}" &
                               $"Secretaría General solo dispone de {stockRestante} números para este tipo de documento.{vbCrLf}" &
                               $"Usted está intentando asignar {cantidad}.{vbCrLf}{vbCrLf}" &
                               "Por favor, reduzca la cantidad o solicite una ampliación de cupo."

            MessageBox.Show(msg, "Stock Agotado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        ' 3. Guardado en BD
        Try
            Using uow As New UnitOfWork()
                Dim repo = uow.Repository(Of Mae_NumeracionRangos)()

                Dim rango As Mae_NumeracionRangos
                If _idEdicion = 0 Then
                    rango = New Mae_NumeracionRangos() With {.FechaCreacion = DateTime.Now}
                Else
                    rango = Await repo.GetByIdAsync(_idEdicion)
                End If

                rango.IdTipo = CInt(cmbTipo.SelectedValue)
                rango.IdOficina = CInt(cmbOficina.SelectedValue)
                rango.Anio = CInt(numAnio.Value)
                rango.NombreRango = txtNombre.Text
                rango.NumeroInicio = ini
                rango.NumeroFin = fin
                rango.UltimoUtilizado = ult
                rango.Activo = chkActivo.Checked

                ' Validación de Colisiones (Nadie pisa a nadie)
                Dim colisiones = Await repo.GetQueryable().
                    Where(Function(x) x.IdTipo = rango.IdTipo And
                                      x.Anio = rango.Anio And
                                      x.Activo = True And
                                      x.IdRango <> rango.IdRango).ToListAsync()

                For Each c In colisiones
                    If (rango.NumeroInicio <= c.NumeroFin) And (rango.NumeroFin >= c.NumeroInicio) Then
                        Dim nombreOficinaChoque = If(c.Cat_Oficina IsNot Nothing, c.Cat_Oficina.Nombre, "Otra Oficina")
                        Toast.Show(Me, $"CONFLICTO: El rango {rango.NumeroInicio}-{rango.NumeroFin} se superpone con el asignado a '{nombreOficinaChoque}' ({c.NumeroInicio}-{c.NumeroFin}).", ToastType.Error)
                        Return
                    End If
                Next

                If _idEdicion = 0 Then repo.Add(rango)
                Await uow.CommitAsync()

                Toast.Show(Me, "Rango asignado correctamente.", ToastType.Success)
                ModoEdicion(False)
                ActualizarVisualizadorStock()
                Await CargarGrillaAsync()
            End Using
        Catch ex As Exception
            Toast.Show(Me, "Error al guardar: " & ex.Message, ToastType.Error)
        End Try
    End Sub

End Class