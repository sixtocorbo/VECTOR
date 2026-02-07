Imports System.Data.Entity
Imports System.Data.Entity.Validation ' Para capturar errores de validación detallados

Public Class frmGestionRangos

    ' Variable para controlar si estamos editando (0 = Nuevo, >0 = ID del rango)
    Private _idEdicion As Integer = 0
    Private _oficinas As List(Of OficinaOption)

    Private Class OficinaOption
        Public Property IdOficina As Integer?
        Public Property Nombre As String
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
            ' para que no aparezca duplicada ni confundamos al usuario.
            ' (Asumiendo que 13 es el ID fijo de Bandeja, ajusta si es otro)
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
            txtFin.Text = "1000"
            txtUltimo.Text = "0"
            cmbTipo.SelectedIndex = -1
            If cmbOficina.Items.Count > 0 Then
                cmbOficina.SelectedIndex = 0
            End If
            chkActivo.Checked = True
            _idEdicion = 0
        End If
    End Sub

    ' --- BOTONES ---

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        ModoEdicion(True)
        cmbTipo.Focus()
        chkActivo.Checked = True
        txtUltimo.Text = "0"
        txtUltimo.Enabled = True ' Permitimos ajustar manualmente el último utilizado al crear.
    End Sub

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvRangos.SelectedRows.Count = 0 Then Return

        _idEdicion = Convert.ToInt32(dgvRangos.SelectedRows(0).Cells("Id").Value)

        Using uow As New UnitOfWork()
            Dim repoRangos = uow.Repository(Of Mae_NumeracionRangos)()
            Dim r = Await repoRangos.GetByIdAsync(_idEdicion)

            If r IsNot Nothing Then
                cmbTipo.SelectedValue = r.IdTipo

                ' --- CORRECCIÓN DEL ERROR ---
                ' No podemos asignar Nothing directamente a SelectedValue.
                ' Lo manejamos manualmente:
                If r.IdOficina.HasValue Then
                    cmbOficina.SelectedValue = r.IdOficina.Value
                Else
                    ' Caso: BANDEJA DE ENTRADA (IdOficina es NULL en base de datos)
                    ' Buscamos manualmente en la lista el ítem que corresponde a Nothing
                    ' (Generalmente es el primero, índice 0, pero lo buscamos por seguridad)
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
                ' -----------------------------

                txtNombre.Text = r.NombreRango
                txtInicio.Text = r.NumeroInicio.ToString()
                txtFin.Text = r.NumeroFin.ToString()
                txtUltimo.Text = r.UltimoUtilizado.ToString()
                chkActivo.Checked = r.Activo

                ' Permitimos editar el último utilizado solo para correcciones manuales
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
        ' A. VALIDACIONES DE INTERFAZ (Igual que antes...)
        If cmbTipo.SelectedIndex = -1 Then
            Toast.Show(Me, "Seleccione el Tipo de Documento.", ToastType.Warning)
            Return
        End If

        Dim ini, fin, ult As Integer
        Integer.TryParse(txtInicio.Text, ini)
        Integer.TryParse(txtFin.Text, fin)
        Integer.TryParse(txtUltimo.Text, ult)

        If ini >= fin Then
            Toast.Show(Me, "El número de Inicio debe ser menor al número Fin.", ToastType.Warning)
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

                ' Traemos TODOS los rangos activos de este tipo (menos yo mismo)
                Dim rangosExistentes = Await repoRangos.GetQueryable().Where(Function(x) x.IdTipo = rango.IdTipo And x.Activo = True And x.IdRango <> rango.IdRango).ToListAsync()

                For Each existente In rangosExistentes
                    Dim existenteEsGeneral As Boolean = Not existente.IdOficina.HasValue

                    ' Verificamos si hay intersección matemática de números
                    Dim hayCruce As Boolean = (rango.NumeroInicio <= existente.NumeroFin) And (rango.NumeroFin >= existente.NumeroInicio)

                    If hayCruce Then
                        ' CASO 1: PELEA DE OFICINAS (Específico vs Específico) -> ERROR
                        If Not soyGeneral AndAlso Not existenteEsGeneral Then

                            ' --- CORRECCIÓN: COMPARACIÓN SEGURA DE NULLABLES ---
                            ' Usamos .HasValue y .Value para evitar que la comparación devuelva 'Nothing'
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
                        ' Aquí es donde permitimos que la Bandeja tenga 1-1000 y la Oficina A tenga 100-200.
                        ' El sistema asumirá que es una "Reserva" dentro del bloque general.
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

                Toast.Show(Me, "Rango configurado correctamente (con reservas permitidas).", ToastType.Success)
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

End Class
