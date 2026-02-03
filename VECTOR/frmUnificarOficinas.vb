Imports System.Data.SqlClient ' Para SqlParameter
Imports System.Linq

Public Class frmUnificarOficinas

    Private Sub frmUnificarOficinas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarGrilla()
        CargarOficinas("")
    End Sub

    ' 1. CONFIGURAR LA GRILLA
    Private Sub ConfigurarGrilla()
        dgvOficinas.Columns.Clear()

        ' Columna de Checkbox
        Dim colCheck As New DataGridViewCheckBoxColumn()
        colCheck.HeaderText = "Sel"
        colCheck.Name = "colSeleccionar"
        colCheck.Width = 40
        dgvOficinas.Columns.Add(colCheck)

        ' Estilos visuales
        dgvOficinas.AllowUserToAddRows = False
        dgvOficinas.RowHeadersVisible = False
        dgvOficinas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    ' 2. CARGAR DATOS (CON FILTRO DE SEGURIDAD)
    Private Sub CargarOficinas(filtro As String)
        Try
            Using db As New SecretariaDBEntities()

                ' --- ZONA DE SEGURIDAD ---
                ' IDs que el sistema usa internamente y NO se deben tocar.
                ' 1 = MESA DE ENTRADA, 13 = BANDEJA DE ENTRADA
                Dim idsIntocables As Integer() = {1, 13}

                ' Consulta LINQ
                ' Filtramos:
                ' 1. Que no esté en la lista de IDs prohibidos
                ' 2. Que el nombre no contenga "ARCHIVO" (para proteger el histórico)
                Dim query = From o In db.Cat_Oficina
                            Where Not idsIntocables.Contains(o.IdOficina) AndAlso
                                  Not o.Nombre.Contains("ARCHIVO")
                            Select o.IdOficina, o.Nombre
                            Order By Nombre Ascending

                ' Aplicar filtro de búsqueda del usuario
                If Not String.IsNullOrEmpty(filtro) Then
                    query = query.Where(Function(x) x.Nombre.Contains(filtro))
                End If

                ' Ejecutar y mostrar
                dgvOficinas.DataSource = query.ToList()

                ' Ajustes estéticos
                If dgvOficinas.Columns("IdOficina") IsNot Nothing Then dgvOficinas.Columns("IdOficina").Width = 60
                If dgvOficinas.Columns("Nombre") IsNot Nothing Then dgvOficinas.Columns("Nombre").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End Using

        Catch ex As Exception
            MessageBox.Show("Error al cargar oficinas: " & ex.Message)
        End Try
    End Sub

    ' 3. FILTRO EN TIEMPO REAL
    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        CargarOficinas(txtBuscar.Text.Trim())
    End Sub

    ' 4. BOTÓN UNIFICAR
    Private Sub btnUnificar_Click(sender As Object, e As EventArgs) Handles btnUnificar.Click

        ' Validaciones básicas
        If String.IsNullOrWhiteSpace(txtNombreOficial.Text) Then
            MessageBox.Show("Por favor, escribe el NOMBRE OFICIAL (Destino).", "Falta Nombre", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNombreOficial.Focus()
            Return
        End If

        ' Recolectar seleccionados
        Dim idsParaBorrar As New List(Of Integer)()
        Dim nombresMuestra As String = ""

        For Each row As DataGridViewRow In dgvOficinas.Rows
            If Convert.ToBoolean(row.Cells("colSeleccionar").Value) Then
                idsParaBorrar.Add(Convert.ToInt32(row.Cells("IdOficina").Value))

                ' Guardamos muestra para la alerta
                If idsParaBorrar.Count <= 3 Then
                    nombresMuestra &= "- " & row.Cells("Nombre").Value.ToString() & vbCrLf
                End If
            End If
        Next

        If idsParaBorrar.Count = 0 Then
            MessageBox.Show("Selecciona al menos una oficina para unificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Confirmación Crítica
        Dim msg As String = $"Vas a fusionar {idsParaBorrar.Count} oficinas en:" & vbCrLf & vbCrLf &
                            $"DIR FINAL: '{txtNombreOficial.Text.ToUpper()}'" & vbCrLf & vbCrLf &
                            $"Se eliminarán:" & vbCrLf & nombresMuestra & "..." & vbCrLf & vbCrLf &
                            "¿Confirmar operación?"

        If MessageBox.Show(msg, "CONFIRMAR FUSIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            EjecutarFusionEF(txtNombreOficial.Text.ToUpper().Trim(), idsParaBorrar)
        End If
    End Sub

    ' 5. EJECUTAR FUSIÓN (STORED PROCEDURE)
    Private Sub EjecutarFusionEF(nombreDestino As String, listaIds As List(Of Integer))
        Try
            Dim idsString As String = String.Join(",", listaIds)

            Using db As New SecretariaDBEntities()

                ' Parámetros para SQL
                Dim pNombre As New SqlParameter("@NombreDestino", nombreDestino)
                Dim pLista As New SqlParameter("@ListaIdsBorrar", idsString)

                ' Llamada segura al SP esperando respuesta
                Dim resultado = db.Database.SqlQuery(Of ResultadoSP)(
                    "EXEC sp_UnificarOficinas @NombreDestino, @ListaIdsBorrar", pNombre, pLista).FirstOrDefault()

                If resultado IsNot Nothing Then
                    If resultado.Resultado = 1 Then
                        AuditoriaSistema.RegistrarEvento($"Unificación de {listaIds.Count} oficina(s) en '{nombreDestino}'.", "OFICINAS")
                        MessageBox.Show(resultado.Mensaje & " 🚀", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' Limpiar todo
                        txtBuscar.Text = ""
                        txtNombreOficial.Text = ""
                        CargarOficinas("")
                    Else
                        MessageBox.Show("Error SQL: " & resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show("Error crítico: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Clase auxiliar para leer el SELECT del SP
    Private Class ResultadoSP
        Public Property Resultado As Integer
        Public Property Mensaje As String
    End Class

    ' Mejora de usabilidad: Click en celda = Check
    Private Sub dgvOficinas_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvOficinas.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex <> -1 Then
            ' Solo invertimos si no se hizo clic directo en el checkbox (para evitar doble acción)
            If dgvOficinas.Columns(e.ColumnIndex).Name <> "colSeleccionar" Then
                Dim celda As DataGridViewCheckBoxCell = CType(dgvOficinas.Rows(e.RowIndex).Cells("colSeleccionar"), DataGridViewCheckBoxCell)
                celda.Value = Not Convert.ToBoolean(celda.Value)
            End If
        End If
    End Sub

End Class
