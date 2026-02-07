Imports System.Data.Entity
Imports System.Data.Entity.Validation ' Para capturar errores de validación detallados

Public Class frmGestionRangos

    ' Variable para controlar si estamos editando (0 = Nuevo, >0 = ID del rango)
    Private _idEdicion As Integer = 0
    Private _oficinas As List(Of OficinaOption)
    Private _ultimoInicioSugerido As Integer? = Nothing
    Private _ultimoUltimoSugerido As Integer? = Nothing
    Private _sugerenciaEnCurso As Boolean = False

    Private Class OficinaOption
        Public Property IdOficina As Integer?
        Public Property Nombre As String
    End Class

    Private Class RangoSimple
        Public Property Inicio As Integer
        Public Property Fin As Integer
        Public Property IdOficina As Integer?
    End Class

    Private Async Sub frmGestionRangos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Await CargarOficinasAsync()
        Await CargarTiposAsync()
        Await CargarGrillaAsync()
        ModoEdicion(False)
    End Sub

    Private Sub frmGestionRangos_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.ShowIcon = False
    End Sub

    ' --- CORRECCIÓN MATEMÁTICA 1: Cálculo exacto del Fin ---
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

        ' Si inicio es 1 y cantidad es 10, el fin debe ser 10 (1..10), no 11.
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
        Await SugerirInicioDisponibleAsync()
    End Sub

    Private Async Sub cmbOficina_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbOficina.SelectedIndexChanged
        ActualizarModoEntrada()
        Await SugerirInicioDisponibleAsync()
    End Sub

    ' 1. CARGA EL COMBO DE TIPOS DE DOCUMENTO (Desde Cat_TipoDocumento)
    Private Async Function CargarTiposAsync() As Task
        Using uow As New UnitOfWork()
            Dim repoTipos = uow.Repository(Of Cat_TipoDocumento)()
            cmbTipo.DataSource = Await repoTipos.GetQueryable().OrderBy(Function(t) t.Nombre).ToListAsync()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "IdTipo"
        End Using
    End Function

    Private Async Function CargarOficinasAsync() As Task
        Using uow As New UnitOfWork()
            Dim repoOficinas = uow.Repository(Of Cat_Oficina)()

            ' 1. Traemos todas las oficinas de la BD
            Dim oficinasDb = Await repoOficinas.GetQueryable().OrderBy(Function(o) o.Nombre).ToListAsync()

            ' 2. Construimos la lista visual
            _oficinas = New List(Of OficinaOption) From {
                New OficinaOption With {.IdOficina = Nothing, .Nombre = "BANDEJA DE ENTRADA (GENERAL)"}
            }

            ' 3. Agregamos las oficinas reales EXCLUYENDO la Bandeja de Entrada (ID 13)
            Dim idBandeja As Integer = 13

            _oficinas.AddRange(oficinasDb.Where(Function(o) o.IdOficina <> idBandeja).Select(Function(o) New OficinaOption With {
                .IdOficina = o.IdOficina,
                .Nombre = o.Nombre
            }))

            cmbOficina.DataSource = _oficinas
            cmbOficina.DisplayMember = "Nombre"
            cmbOficina.ValueMember = "IdOficina"
        End Using
    End Function

    ' 2. CARGA LA GRILLA DE RANGOS (Desde Mae_NumeracionRangos)
    Private Async Function CargarGrillaAsync() As Task
        Using uow As New UnitOfWork()
            Dim repoRangos = uow.Repository(Of Mae_NumeracionRangos)()
            Dim repoOficinas = uow.Repository(Of Cat_Oficina)()

            ' Hacemos un Left Join para mostrar el nombre de la oficina o "BANDEJA DE ENTRADA"
            Dim lista = Await (From r In repoRangos.GetQueryable("Cat_TipoDocumento")
                               Group Join o In repoOficinas.GetQueryable()
                                   On r.IdOficina Equals o.IdOficina Into oficinaJoin = Group
                               From o In oficinaJoin.DefaultIfEmpty()
                               Select New With {
                                   .Id = r.IdRango,
                                   .Tipo = r.Cat_TipoDocumento.Codigo & " - " & r.Cat_TipoDocumento.Nombre,
                                   .Oficina = If(r.IdOficina.HasValue AndAlso o IsNot Nothing, o.Nombre, "BANDEJA DE ENTRADA"),
                                   .Nombre = r.NombreRango,
                                   .Inicio = r.NumeroInicio,
                                   .Fin = r.NumeroFin,
                                   .Actual = r.UltimoUtilizado,
                                   .Vigente = r.Activo
                               }).OrderByDescending(Function(r) r.Id).ToListAsync()

            dgvRangos.DataSource = lista

            ' Ocultamos el ID y ajustamos columnas
            If dgvRangos.Columns.Count > 0 Then
                dgvRangos.Columns("Id").Visible = False
                dgvRangos.Columns("Tipo").Width = 150
                dgvRangos.Columns("Oficina").Width = 180
                dgvRangos.Columns("Nombre").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                dgvRangos.Columns("Inicio").Width = 80
                dgvRangos.Columns("Fin").Width = 80
                dgvRangos.Columns("Actual").Width = 80
                dgvRangos.Columns("Vigente").Width = 60
            End If
        End Using
    End Function

    ' =========================================================================
    '  LÓGICA CENTRAL DE SUGERENCIA (MODIFICADA PARA COMPORTAMIENTO HÍBRIDO)
    ' =========================================================================
    Private Async Function SugerirInicioDisponibleAsync() As Task
        If _idEdicion <> 0 Then Return
        If _sugerenciaEnCurso Then Return
        If cmbTipo.SelectedIndex = -1 Then Return

        Dim cantidad As Integer
        If Not Integer.TryParse(txtCantidad.Text, cantidad) Then Return
        If cantidad < 0 Then Return

        ' --- REGLAS DE DECISIÓN: ¿DEBO SOBRESCRIBIR EL CAMPO INICIO? ---
        Dim puedeReemplazar As Boolean = False

        ' 1. Si está vacío, siempre sugerimos (ayuda inicial)
        If String.IsNullOrWhiteSpace(txtInicio.Text) Then
            puedeReemplazar = True

            ' 2. Si el campo es ReadOnly (Oficina Específica), el sistema tiene el control total
        ElseIf txtInicio.ReadOnly Then
            puedeReemplazar = True

            ' 3. Si es Editable (Bandeja General), solo sugerimos si el valor actual
            '    coincide con la última sugerencia del sistema (el usuario no lo tocó).
        ElseIf _ultimoInicioSugerido.HasValue AndAlso txtInicio.Text.Trim() = _ultimoInicioSugerido.Value.ToString() Then
            puedeReemplazar = True
        End If
        ' ----------------------------------------------------------------

        If Not puedeReemplazar Then Return

        _sugerenciaEnCurso = True
        Try
            Dim idTipo As Integer = Convert.ToInt32(cmbTipo.SelectedValue)
            Dim esGeneral As Boolean = EsOficinaGeneralSeleccionada()

            Using uow As New UnitOfWork()
                Dim repoRangos = uow.Repository(Of Mae_NumeracionRangos)()
                Dim rangos = Await repoRangos.GetQueryable().
                    Where(Function(x) x.IdTipo = idTipo And x.Activo = True).
                    Select(Function(x) New RangoSimple With {
                        .Inicio = x.NumeroInicio,
                        .Fin = x.NumeroFin,
                        .IdOficina = x.IdOficina
                    }).ToListAsync()

                If esGeneral Then
                    Dim rangosFiltrados = rangos.Where(Function(r) Not r.IdOficina.HasValue).ToList()
                    Dim sugerido As Integer = CalcularInicioDisponible(rangosFiltrados, cantidad)
                    AplicarSugerencia(sugerido)
                Else
                    Dim rangoBase = rangos.Where(Function(r) Not r.IdOficina.HasValue).
                        OrderBy(Function(r) r.Inicio).
                        FirstOrDefault()

                    Dim rangosOcupados = rangos.Where(Function(r) r.IdOficina.HasValue).ToList()
                    Dim sugerido = CalcularInicioDisponibleEnRango(rangoBase, rangosOcupados, cantidad)
                    AplicarSugerencia(sugerido)
                End If
            End Using
        Catch
            ' No interrumpir al usuario si falla la sugerencia automática.
        Finally
            _sugerenciaEnCurso = False
        End Try
    End Function

    ' --- CORRECCIÓN MATEMÁTICA 2: Ajuste de Gaps en General ---
    Private Function CalcularInicioDisponible(rangos As List(Of RangoSimple), cantidad As Integer) As Integer
        If rangos Is Nothing OrElse rangos.Count = 0 Then
            Return 1
        End If

        Dim ordenados = rangos.OrderBy(Function(r) r.Inicio).ToList()
        Dim inicioBase As Integer = 1
        Dim actualInicio As Integer = ordenados(0).Inicio
        Dim actualFin As Integer = ordenados(0).Fin

        ' Si cabe antes del primer rango:
        If inicioBase + cantidad - 1 <= actualInicio - 1 Then
            Return inicioBase
        End If

        For i As Integer = 1 To ordenados.Count - 1
            Dim rango = ordenados(i)
            If rango.Inicio <= actualFin + 1 Then
                actualFin = Math.Max(actualFin, rango.Fin)
            Else
                Dim gapInicio As Integer = actualFin + 1
                Dim gapFin As Integer = rango.Inicio - 1
                ' Verificamos si cabe en el hueco exacto
                If gapInicio + cantidad - 1 <= gapFin Then
                    Return gapInicio
                End If
                actualInicio = rango.Inicio
                actualFin = rango.Fin
            End If
        Next

        Return actualFin + 1
    End Function

    ' --- CORRECCIÓN MATEMÁTICA 3: Ajuste de Gaps en Sub-rangos ---
    Private Function CalcularInicioDisponibleEnRango(rangoBase As RangoSimple, rangosOcupados As List(Of RangoSimple), cantidad As Integer) As Integer?
        If rangoBase Is Nothing Then Return Nothing
        If cantidad < 0 Then Return Nothing

        Dim baseInicio As Integer = rangoBase.Inicio
        Dim baseFin As Integer = rangoBase.Fin

        If baseInicio + cantidad - 1 > baseFin Then Return Nothing

        Dim ocupados = rangosOcupados.
            Where(Function(r) r.Fin >= baseInicio AndAlso r.Inicio <= baseFin).
            OrderBy(Function(r) r.Inicio).
            ToList()

        Dim cursor As Integer = baseInicio

        For Each ocupado In ocupados
            Dim ocupadoInicio As Integer = Math.Max(ocupado.Inicio, baseInicio)
            Dim ocupadoFin As Integer = Math.Min(ocupado.Fin, baseFin)

            ' Verificamos si cabe antes del bloque ocupado
            If cursor + cantidad - 1 <= ocupadoInicio - 1 Then
                Return cursor
            End If

            ' Saltamos el bloque ocupado
            cursor = Math.Max(cursor, ocupadoFin + 1)

            ' Si ya nos salimos del rango base, abortar
            If cursor + cantidad - 1 > baseFin Then Return Nothing
        Next

        ' Verificamos si cabe en el espacio final restante
        If cursor + cantidad - 1 <= baseFin Then
            Return cursor
        End If

        Return Nothing
    End Function

    ' 3. CONTROLA EL ESTADO DE LOS BOTONES (Habilitar/Deshabilitar)
    Private Sub ModoEdicion(habilitar As Boolean)
        pnlEditor.Enabled = habilitar
        btnNuevo.Enabled = Not habilitar
        btnEditar.Enabled = Not habilitar
        btnEliminar.Enabled = Not habilitar
        dgvRangos.Enabled = Not habilitar

        If Not habilitar Then
            ' Limpiar campos al salir del modo edición
            txtNombre.Clear()
            txtInicio.Text = "1"
            txtCantidad.Text = "50"
            ActualizarFinDesdeCantidad()
            txtUltimo.Text = "0"
            cmbTipo.SelectedIndex = -1
            If cmbOficina.Items.Count > 0 Then
                cmbOficina.SelectedIndex = 0
            End If
            chkActivo.Checked = True
            _idEdicion = 0
        Else
            ActualizarModoEntrada()
        End If
    End Sub

    ' --- BOTONES ---

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        ModoEdicion(True)

        ' Limpiamos estos campos para que el sistema sepa que puede 
        ' sobrescribirlos con la sugerencia automática.
        txtInicio.Text = ""
        txtFin.Text = ""
        _ultimoInicioSugerido = Nothing

        cmbTipo.Focus()
        chkActivo.Checked = True
        txtUltimo.Text = "0"
        txtUltimo.Enabled = True
    End Sub

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvRangos.SelectedRows.Count = 0 Then Return

        _idEdicion = Convert.ToInt32(dgvRangos.SelectedRows(0).Cells("Id").Value)

        Using uow As New UnitOfWork()
            Dim repoRangos = uow.Repository(Of Mae_NumeracionRangos)()
            Dim r = Await repoRangos.GetByIdAsync(_idEdicion)

            If r IsNot Nothing Then
                cmbTipo.SelectedValue = r.IdTipo

                If r.IdOficina.HasValue Then
                    cmbOficina.SelectedValue = r.IdOficina.Value
                Else
                    cmbOficina.SelectedIndex = -1
                    If _oficinas IsNot Nothing Then
                        For i As Integer = 0 To _oficinas.Count - 1
                            If _oficinas(i).IdOficina Is Nothing Then
                                cmbOficina.SelectedIndex = i
                                Exit For
                            End If
                        Next
                    End If
                End If

                txtNombre.Text = r.NombreRango
                txtInicio.Text = r.NumeroInicio.ToString()

                ' --- CORRECCIÓN MATEMÁTICA 4: Recuperar cantidad correcta (+1) ---
                ' Rango 1 a 10 son 10 números. (10 - 1) + 1 = 10.
                txtCantidad.Text = Math.Max(0, r.NumeroFin - r.NumeroInicio + 1).ToString()

                txtFin.Text = r.NumeroFin.ToString()
                txtUltimo.Text = r.UltimoUtilizado.ToString()
                chkActivo.Checked = r.Activo

                txtUltimo.Enabled = True

                ModoEdicion(True)
            End If
        End Using
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        ModoEdicion(False)
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvRangos.SelectedRows.Count = 0 Then
            Toast.Show(Me, "Seleccione un rango para eliminar.", ToastType.Warning)
            Return
        End If

        Dim idRango As Integer = Convert.ToInt32(dgvRangos.SelectedRows(0).Cells("Id").Value)

        If MessageBox.Show("¿Eliminar rango seleccionado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
            Return
        End If

        Using uow As New UnitOfWork()
            Dim repoRangos = uow.Repository(Of Mae_NumeracionRangos)()
            Dim rango = Await repoRangos.GetByIdAsync(idRango)

            If rango Is Nothing Then
                Toast.Show(Me, "Rango no encontrado.", ToastType.Warning)
                Return
            End If

            repoRangos.Remove(rango)
            Await uow.CommitAsync()
        End Using

        Toast.Show(Me, "Rango eliminado correctamente.", ToastType.Success)
        Await CargarGrillaAsync()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' A. VALIDACIONES DE INTERFAZ
        If cmbTipo.SelectedIndex = -1 Then
            Toast.Show(Me, "Seleccione el Tipo de Documento.", ToastType.Warning)
            Return
        End If

        Dim ini, fin, ult As Integer
        Dim cantidad As Integer
        Dim inicioValido As Boolean = Integer.TryParse(txtInicio.Text, ini)
        Dim cantidadValida As Boolean = Integer.TryParse(txtCantidad.Text, cantidad)
        Integer.TryParse(txtUltimo.Text, ult)

        If Not inicioValido Then
            Toast.Show(Me, "Ingrese un número de Inicio válido.", ToastType.Warning)
            Return
        End If

        If Not cantidadValida Then
            Toast.Show(Me, "Ingrese una cantidad válida.", ToastType.Warning)
            Return
        End If

        ' --- CORRECCIÓN MATEMÁTICA 5: Confirmar cálculo final al guardar ---
        fin = ini + cantidad - 1
        txtFin.Text = fin.ToString()

        If cantidad < 0 Then
            Toast.Show(Me, "La cantidad de números debe ser mayor o igual a cero.", ToastType.Warning)
            Return
        End If
        If ini > fin Then ' Ajustado a > porque ini == fin es válido (cantidad 1)
            Toast.Show(Me, "El número de Inicio no puede ser mayor al número Fin.", ToastType.Warning)
            Return
        End If
        If ult < (ini - 1) Or ult > fin Then
            Toast.Show(Me, "El 'Último Utilizado' es incoherente con el rango Inicio-Fin.", ToastType.Warning)
            Return
        End If

        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            txtNombre.Text = GenerarNombreRango(ini, fin)
        End If

        ' B. GUARDADO EN BASE DE DATOS
        Try
            Using uow As New UnitOfWork()
                Dim repoRangos = uow.Repository(Of Mae_NumeracionRangos)()
                Dim rango As Mae_NumeracionRangos
                Dim esNuevo As Boolean = (_idEdicion = 0)

                If esNuevo Then
                    rango = New Mae_NumeracionRangos()
                    rango.FechaCreacion = DateTime.Now
                Else
                    rango = Await repoRangos.GetByIdAsync(_idEdicion)
                End If

                If rango Is Nothing Then Return

                ' --- Asignar valores ---
                rango.IdTipo = Convert.ToInt32(cmbTipo.SelectedValue)
                Dim selectedOficina = cmbOficina.SelectedValue

                ' Normalización del ID Oficina
                If selectedOficina Is Nothing OrElse selectedOficina Is DBNull.Value Then
                    rango.IdOficina = Nothing
                Else
                    Dim parsedOficina As Integer
                    If Integer.TryParse(selectedOficina.ToString(), parsedOficina) Then
                        rango.IdOficina = parsedOficina
                    Else
                        rango.IdOficina = Nothing
                    End If
                End If

                rango.NombreRango = txtNombre.Text.Trim()
                rango.NumeroInicio = ini
                rango.NumeroFin = fin
                rango.UltimoUtilizado = ult
                rango.Activo = chkActivo.Checked

                ' =====================================================================
                ' LÓGICA DE VALIDACIÓN DE SUPERPOSICIÓN MULTI-OFICINA
                ' =====================================================================
                Dim soyGeneral As Boolean = Not rango.IdOficina.HasValue

                Dim rangosExistentes = Await repoRangos.GetQueryable().Where(Function(x) x.IdTipo = rango.IdTipo And x.Activo = True And x.IdRango <> rango.IdRango).ToListAsync()

                For Each existente In rangosExistentes
                    Dim existenteEsGeneral As Boolean = Not existente.IdOficina.HasValue

                    ' Verificamos si hay intersección matemática de números
                    Dim hayCruce As Boolean = (rango.NumeroInicio <= existente.NumeroFin) And (rango.NumeroFin >= existente.NumeroInicio)

                    If hayCruce Then
                        ' CASO 1: PELEA DE OFICINAS (Específico vs Específico) -> ERROR
                        If Not soyGeneral AndAlso Not existenteEsGeneral Then
                            Dim nombreOfi = _oficinas.FirstOrDefault(Function(o) o.IdOficina.HasValue AndAlso o.IdOficina.Value = existente.IdOficina.Value)?.Nombre
                            If String.IsNullOrEmpty(nombreOfi) Then nombreOfi = "Oficina Desconocida"

                            Toast.Show(Me, $"CONFLICTO: El rango {rango.NumeroInicio}-{rango.NumeroFin} choca con la oficina '{nombreOfi}' ({existente.NumeroInicio}-{existente.NumeroFin})." & vbCrLf & "Dos oficinas no pueden compartir números.", ToastType.Error)
                            Return
                        End If

                        ' CASO 2: PELEA DE GENERALES (General vs General) -> ERROR
                        If soyGeneral AndAlso existenteEsGeneral Then
                            Toast.Show(Me, $"CONFLICTO: Ya existe un Rango General activo ({existente.NumeroInicio}-{existente.NumeroFin}). Solo puede haber una Bandeja General por tipo.", ToastType.Error)
                            Return
                        End If

                        ' CASO 3: CESIÓN (General vs Específico) -> PERMITIDO
                    End If
                Next

                ' Si paso la validación, desactivo SOLO mis versiones anteriores exactas
                If rango.Activo Then
                    Dim otrosQuery = repoRangos.GetQueryable().Where(Function(x) x.IdTipo = rango.IdTipo And x.IdRango <> rango.IdRango And x.Activo = True)
                    Dim otros As List(Of Mae_NumeracionRangos)

                    If rango.IdOficina.HasValue Then
                        Dim idOficina = rango.IdOficina.Value
                        otros = Await otrosQuery.Where(Function(x) x.IdOficina.HasValue AndAlso x.IdOficina.Value = idOficina).ToListAsync()
                    Else
                        otros = Await otrosQuery.Where(Function(x) Not x.IdOficina.HasValue).ToListAsync()
                    End If

                    For Each o In otros
                        o.Activo = False
                    Next
                End If

                If esNuevo Then repoRangos.Add(rango)
                Await uow.CommitAsync()

                Toast.Show(Me, "Rango configurado correctamente.", ToastType.Success)
                ModoEdicion(False)
                Await CargarGrillaAsync()
            End Using

        Catch ex As Exception
            Toast.Show(Me, "Error: " & ex.Message, ToastType.Error)
        End Try
    End Sub

    Private Function GenerarNombreRango(ini As Integer, fin As Integer) As String
        Dim tipo As String = cmbTipo.Text?.Trim()
        Dim oficina As String = ObtenerNombreOficinaSeleccionada()
        Dim anio As String = DateTime.Now.Year.ToString()
        Dim rango As String = $"{ini}-{fin}"

        Return $"{tipo} {anio} - {oficina} ({rango})".Trim()
    End Function

    Private Function ObtenerNombreOficinaSeleccionada() As String
        Dim selectedValue = cmbOficina.SelectedValue
        If selectedValue Is Nothing OrElse selectedValue Is DBNull.Value Then
            Return "BANDEJA DE ENTRADA"
        End If

        Dim idOficina As Integer? = Nothing
        Dim parsedId As Integer
        If Integer.TryParse(selectedValue.ToString(), parsedId) Then
            idOficina = parsedId
        Else
            Return "BANDEJA DE ENTRADA"
        End If

        Dim oficina = _oficinas?.FirstOrDefault(Function(o) o.IdOficina.HasValue AndAlso o.IdOficina.Value = idOficina.Value)
        Return If(oficina?.Nombre, "BANDEJA DE ENTRADA")
    End Function

    Private Function EsOficinaGeneralSeleccionada() As Boolean
        Dim selectedValue = cmbOficina.SelectedValue
        Return selectedValue Is Nothing OrElse selectedValue Is DBNull.Value
    End Function

    ' =========================================================================
    ' ACTUALIZACIÓN DE UI: Bloquea campos si es Oficina Específica
    ' =========================================================================
    Private Sub ActualizarModoEntrada()
        Dim esGeneral As Boolean = EsOficinaGeneralSeleccionada()

        ' REGLA DE EXCEPCIÓN:
        ' Si es General -> Usuario puede editar (ReadOnly = False)
        ' Si es Oficina Específica -> Usuario NO puede editar (ReadOnly = True)
        txtInicio.ReadOnly = Not esGeneral
        txtUltimo.ReadOnly = Not esGeneral

        ' Feedback visual
        Dim colorEdicion As Color = If(esGeneral, Color.White, Color.LightGoldenrodYellow)
        txtInicio.BackColor = colorEdicion
        txtUltimo.BackColor = colorEdicion
    End Sub

    Private Sub AplicarSugerencia(sugerido As Integer?)
        If sugerido.HasValue Then
            Dim puedeReemplazarUltimo As Boolean = String.IsNullOrWhiteSpace(txtUltimo.Text)
            If _ultimoUltimoSugerido.HasValue AndAlso txtUltimo.Text.Trim() = _ultimoUltimoSugerido.Value.ToString() Then
                puedeReemplazarUltimo = True
            End If

            _ultimoInicioSugerido = sugerido.Value
            txtInicio.Text = sugerido.Value.ToString()

            Dim ult As Integer = Math.Max(0, sugerido.Value - 1)
            _ultimoUltimoSugerido = ult

            If txtUltimo.ReadOnly OrElse puedeReemplazarUltimo Then
                txtUltimo.Text = ult.ToString()
            End If
        Else
            _ultimoInicioSugerido = Nothing
            _ultimoUltimoSugerido = Nothing
            txtInicio.Clear()
            txtFin.Clear()
            If txtUltimo.ReadOnly Then
                txtUltimo.Text = "0"
            End If
        End If
    End Sub

End Class