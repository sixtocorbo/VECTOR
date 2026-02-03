Imports System.Data.Entity
Imports System.Data.Entity.Validation ' Para capturar errores de validación detallados

Public Class frmGestionRangos

    ' Variable para controlar si estamos editando (0 = Nuevo, >0 = ID del rango)
    Private _idEdicion As Integer = 0

    Private Sub frmGestionRangos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarTipos()
        CargarGrilla()
        ModoEdicion(False)
    End Sub

    ' 1. CARGA EL COMBO DE TIPOS DE DOCUMENTO (Desde Cat_TipoDocumento)
    Private Sub CargarTipos()
        Using db As New SecretariaDBEntities()
            ' Filtramos para no numerar cosas raras si es necesario
            cmbTipo.DataSource = db.Cat_TipoDocumento.OrderBy(Function(t) t.Nombre).ToList()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "IdTipo"
        End Using
    End Sub

    ' 2. CARGA LA GRILLA DE RANGOS (Desde Mae_NumeracionRangos)
    Private Sub CargarGrilla()
        Using db As New SecretariaDBEntities()
            ' Hacemos un Select anónimo para mostrar nombres bonitos en la grilla
            Dim lista = db.Mae_NumeracionRangos.Include("Cat_TipoDocumento") _
                          .Select(Function(r) New With {
                              .Id = r.IdRango,
                              .Tipo = r.Cat_TipoDocumento.Codigo & " - " & r.Cat_TipoDocumento.Nombre,
                              .Nombre = r.NombreRango,
                              .Inicio = r.NumeroInicio,
                              .Fin = r.NumeroFin,
                              .Actual = r.UltimoUtilizado,
                              .Vigente = r.Activo
                          }).OrderByDescending(Function(r) r.Id).ToList()

            dgvRangos.DataSource = lista

            ' Ocultamos el ID y ajustamos columnas
            If dgvRangos.Columns.Count > 0 Then
                dgvRangos.Columns("Id").Visible = False
                dgvRangos.Columns("Tipo").Width = 150
                dgvRangos.Columns("Nombre").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                dgvRangos.Columns("Inicio").Width = 80
                dgvRangos.Columns("Fin").Width = 80
                dgvRangos.Columns("Actual").Width = 80
                dgvRangos.Columns("Vigente").Width = 60
            End If
        End Using
    End Sub

    ' 3. CONTROLA EL ESTADO DE LOS BOTONES (Habilitar/Deshabilitar)
    Private Sub ModoEdicion(habilitar As Boolean)
        pnlEditor.Enabled = habilitar
        btnNuevo.Enabled = Not habilitar
        btnEditar.Enabled = Not habilitar
        dgvRangos.Enabled = Not habilitar

        If Not habilitar Then
            ' Limpiar campos al salir del modo edición
            txtNombre.Clear()
            txtInicio.Text = "1"
            txtFin.Text = "1000"
            txtUltimo.Text = "0"
            cmbTipo.SelectedIndex = -1
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
        txtUltimo.Enabled = False ' En uno nuevo, el último siempre es 0 (o inicio - 1)
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvRangos.SelectedRows.Count = 0 Then Return

        _idEdicion = Convert.ToInt32(dgvRangos.SelectedRows(0).Cells("Id").Value)

        Using db As New SecretariaDBEntities()
            Dim r = db.Mae_NumeracionRangos.Find(_idEdicion)
            If r IsNot Nothing Then
                cmbTipo.SelectedValue = r.IdTipo
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

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' A. VALIDACIONES DE INTERFAZ
        If cmbTipo.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione el Tipo de Documento.")
            Return
        End If
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            MessageBox.Show("Falta el nombre del rango (Ej: 'Resoluciones 2026').")
            Return
        End If

        Dim ini, fin, ult As Integer
        Integer.TryParse(txtInicio.Text, ini)
        Integer.TryParse(txtFin.Text, fin)
        Integer.TryParse(txtUltimo.Text, ult)

        If ini >= fin Then
            MessageBox.Show("El número de Inicio debe ser menor al número Fin.")
            Return
        End If
        If ult < (ini - 1) Or ult > fin Then
            MessageBox.Show("El 'Último Utilizado' es incoherente con el rango Inicio-Fin.")
            Return
        End If

        ' B. GUARDADO EN BASE DE DATOS
        Try
            Using db As New SecretariaDBEntities()
                Dim rango As Mae_NumeracionRangos
                Dim esNuevo As Boolean = (_idEdicion = 0)

                ' Determinar si es Nuevo o Edición
                If esNuevo Then
                    rango = New Mae_NumeracionRangos()
                    db.Mae_NumeracionRangos.Add(rango)
                Else
                    rango = db.Mae_NumeracionRangos.Find(_idEdicion)
                End If

                ' Asignar valores
                rango.IdTipo = Convert.ToInt32(cmbTipo.SelectedValue)
                rango.NombreRango = txtNombre.Text.Trim()
                rango.NumeroInicio = ini
                rango.NumeroFin = fin
                rango.UltimoUtilizado = ult
                rango.Activo = chkActivo.Checked

                ' Si es nuevo, ponemos fecha
                If _idEdicion = 0 Then rango.FechaCreacion = DateTime.Now

                ' REGLA DE NEGOCIO:
                ' Si activo este rango, debo desactivar cualquier otro rango ACTIVO del mismo tipo
                ' para que el sistema no se confunda sobre cuál usar.
                If rango.Activo Then
                    Dim otros = db.Mae_NumeracionRangos.Where(Function(x) x.IdTipo = rango.IdTipo And
                                                                          x.IdRango <> rango.IdRango And
                                                                          x.Activo = True).ToList()
                    For Each o In otros
                        o.Activo = False
                    Next
                End If

                db.SaveChanges()

                Dim accion As String = If(esNuevo, "Creación", "Edición")
                AuditoriaSistema.RegistrarEvento($"{accion} de rango {rango.NombreRango} ({rango.NumeroInicio}-{rango.NumeroFin}) para tipo {cmbTipo.Text}. Activo: {rango.Activo}.", "RANGOS")
                MessageBox.Show("Rango guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ModoEdicion(False)
                CargarGrilla()
            End Using

            ' C. MANEJO DE ERRORES DE VALIDACIÓN (ENTITY FRAMEWORK)
        Catch dbEx As DbEntityValidationException
            Dim mensaje As String = ""
            For Each valResult In dbEx.EntityValidationErrors
                For Each validationError In valResult.ValidationErrors
                    mensaje &= "- " & validationError.ErrorMessage & vbCrLf
                Next
            Next
            MessageBox.Show("Error de validación de datos:" & vbCrLf & mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error inesperado: " & ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class
