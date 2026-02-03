Imports System.Data.SqlClient ' Necesario para SqlParameter
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

        ' Estilos
        dgvOficinas.AllowUserToAddRows = False
        dgvOficinas.RowHeadersVisible = False
        dgvOficinas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    ' 2. CARGAR DATOS USANDO LINQ (ENTITY FRAMEWORK)
    Private Sub CargarOficinas(filtro As String)
        Try
            ' REEMPLAZA 'SecretariaEntities' POR EL NOMBRE DE TU CONTEXTO
            Using db As New SecretariaDBEntities()

                ' Consulta LINQ básica
                Dim query = From o In db.Cat_Oficina
                            Select o.IdOficina, o.Nombre
                            Order By Nombre Ascending

                ' Aplicar filtro si existe
                If Not String.IsNullOrEmpty(filtro) Then
                    query = query.Where(Function(x) x.Nombre.Contains(filtro))
                End If

                ' Convertir a lista y asignar
                ' ToList() ejecuta la consulta en la BD
                dgvOficinas.DataSource = query.ToList()

                ' Ajustes visuales de columnas (si se generaron automáticamente)
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

        ' Validaciones
        If String.IsNullOrWhiteSpace(txtNombreOficial.Text) Then
            MessageBox.Show("Por favor, escribe el NOMBRE OFICIAL (Destino).", "Falta Nombre", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Recolectar IDs seleccionados
        Dim idsParaBorrar As New List(Of Integer)()
        Dim nombresMuestra As String = ""

        For Each row As DataGridViewRow In dgvOficinas.Rows
            If Convert.ToBoolean(row.Cells("colSeleccionar").Value) Then
                idsParaBorrar.Add(Convert.ToInt32(row.Cells("IdOficina").Value))

                ' Guardar algunos nombres para el mensaje de alerta
                If idsParaBorrar.Count <= 3 Then
                    nombresMuestra &= "- " & row.Cells("Nombre").Value.ToString() & vbCrLf
                End If
            End If
        Next

        If idsParaBorrar.Count = 0 Then
            MessageBox.Show("Selecciona al menos una oficina para unificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Confirmación
        Dim msg As String = $"Vas a fusionar {idsParaBorrar.Count} oficinas en:" & vbCrLf &
                            $"'{txtNombreOficial.Text.ToUpper()}'" & vbCrLf & vbCrLf &
                            $"Ejemplos a borrar:" & vbCrLf & nombresMuestra & "..." & vbCrLf &
                            "¿Confirmar operación?"

        If MessageBox.Show(msg, "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            EjecutarFusionEF(txtNombreOficial.Text.ToUpper().Trim(), idsParaBorrar)
        End If
    End Sub

    ' 5. EJECUTAR EL STORED PROCEDURE VÍA ENTITY FRAMEWORK
    Private Sub EjecutarFusionEF(nombreDestino As String, listaIds As List(Of Integer))
        Try
            Dim idsString As String = String.Join(",", listaIds)

            ' REEMPLAZA 'SecretariaEntities' POR EL NOMBRE DE TU CONTEXTO
            Using db As New SecretariaDBEntities()

                ' Parámetros SQL
                Dim pNombre As New SqlParameter("@NombreDestino", nombreDestino)
                Dim pLista As New SqlParameter("@ListaIdsBorrar", idsString)

                ' Ejecutamos el SP usando SqlQuery para capturar el mensaje de retorno (SELECT 1, 'Mensaje')
                ' Creamos una clase temporal interna para recibir el resultado
                Dim resultado = db.Database.SqlQuery(Of ResultadoSP)(
                    "EXEC sp_UnificarOficinas @NombreDestino, @ListaIdsBorrar", pNombre, pLista).FirstOrDefault()

                If resultado IsNot Nothing Then
                    If resultado.Resultado = 1 Then
                        MessageBox.Show(resultado.Mensaje & " 🚀", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' Limpiar interfaz
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

    ' CLASE AUXILIAR PARA LEER LA RESPUESTA DEL SP
    Private Class ResultadoSP
        Public Property Resultado As Integer
        Public Property Mensaje As String
    End Class

    ' Evento click en celda para marcar checkbox fácil
    Private Sub dgvOficinas_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvOficinas.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = -1 Then Return
        If e.RowIndex >= 0 Then
            Dim celda As DataGridViewCheckBoxCell = CType(dgvOficinas.Rows(e.RowIndex).Cells("colSeleccionar"), DataGridViewCheckBoxCell)
            celda.Value = Not Convert.ToBoolean(celda.Value)
        End If
    End Sub

End Class