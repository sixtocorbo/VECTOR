Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Linq

Public Class frmUnificarOficinas

    Private Async Sub frmUnificarOficinas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        Await CargarOficinasAsync("")
    End Sub

    Private Sub ConfigurarGrilla()
        dgvOficinas.Columns.Clear()

        Dim colCheck As New DataGridViewCheckBoxColumn()
        colCheck.HeaderText = "Sel"
        colCheck.Name = "colSeleccionar"
        colCheck.Width = 40
        dgvOficinas.Columns.Add(colCheck)

        dgvOficinas.AllowUserToAddRows = False
        dgvOficinas.RowHeadersVisible = False
        dgvOficinas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    Private Async Function CargarOficinasAsync(filtro As String) As Task
        Try
            Using uow As New UnitOfWork()
                Dim repoOficinas = uow.Repository(Of Cat_Oficina)()
                Dim idsIntocables As Integer() = {1, 13}

                Dim query = repoOficinas.GetQueryable().
                    Where(Function(o) Not idsIntocables.Contains(o.IdOficina) AndAlso Not o.Nombre.Contains("ARCHIVO")).
                    Select(Function(o) New With {o.IdOficina, o.Nombre})

                If Not String.IsNullOrEmpty(filtro) Then
                    query = query.Where(Function(x) x.Nombre.Contains(filtro))
                End If

                dgvOficinas.DataSource = Await query.OrderBy(Function(x) x.Nombre).ToListAsync()

                If dgvOficinas.Columns("IdOficina") IsNot Nothing Then dgvOficinas.Columns("IdOficina").Width = 60
                If dgvOficinas.Columns("Nombre") IsNot Nothing Then dgvOficinas.Columns("Nombre").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End Using

        Catch ex As Exception
            Toast.Show(Me, "Error al cargar oficinas: " & ex.Message, ToastType.Error)
        End Try
    End Function

    Private Async Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Await CargarOficinasAsync(txtBuscar.Text.Trim())
    End Sub

    Private Async Sub btnUnificar_Click(sender As Object, e As EventArgs) Handles btnUnificar.Click
        If String.IsNullOrWhiteSpace(txtNombreOficial.Text) Then
            Toast.Show(Me, "Por favor, escribe el NOMBRE OFICIAL (Destino).", ToastType.Warning)
            txtNombreOficial.Focus()
            Return
        End If

        Dim idsParaBorrar As New List(Of Integer)()
        Dim nombresMuestra As String = ""

        For Each row As DataGridViewRow In dgvOficinas.Rows
            If Convert.ToBoolean(row.Cells("colSeleccionar").Value) Then
                idsParaBorrar.Add(Convert.ToInt32(row.Cells("IdOficina").Value))
                If idsParaBorrar.Count <= 3 Then
                    nombresMuestra &= "- " & row.Cells("Nombre").Value.ToString() & vbCrLf
                End If
            End If
        Next

        If idsParaBorrar.Count = 0 Then
            Toast.Show(Me, "Selecciona al menos una oficina para unificar.", ToastType.Warning)
            Return
        End If

        Dim msg As String = $"Vas a fusionar {idsParaBorrar.Count} oficinas en:" & vbCrLf & vbCrLf &
                            $"DIR FINAL: '{txtNombreOficial.Text.ToUpper()}'" & vbCrLf & vbCrLf &
                            $"Se eliminarán:" & vbCrLf & nombresMuestra & "..." & vbCrLf & vbCrLf &
                            "¿Confirmar operación?"

        If MessageBox.Show(msg, "CONFIRMAR FUSIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Await EjecutarFusionEFAsync(txtNombreOficial.Text.ToUpper().Trim(), idsParaBorrar)
        End If
    End Sub

    Private Async Function EjecutarFusionEFAsync(nombreDestino As String, listaIds As List(Of Integer)) As Task
        Try
            Dim idsString As String = String.Join(",", listaIds)

            Using uow As New UnitOfWork()
                Dim pNombre As New SqlParameter("@NombreDestino", nombreDestino)
                Dim pLista As New SqlParameter("@ListaIdsBorrar", idsString)

                Dim resultado = Await uow.Context.Database.SqlQuery(Of ResultadoSP)(
                    "EXEC sp_UnificarOficinas @NombreDestino, @ListaIdsBorrar", pNombre, pLista).FirstOrDefaultAsync()

                If resultado IsNot Nothing Then
                    If resultado.Resultado = 1 Then
                        AuditoriaSistema.RegistrarEvento($"Unificación de {listaIds.Count} oficina(s) en '{nombreDestino}'.", "OFICINAS")
                        Toast.Show(Me, resultado.Mensaje & " 🚀", ToastType.Success)

                        txtBuscar.Text = ""
                        txtNombreOficial.Text = ""
                        Await CargarOficinasAsync("")
                    Else
                        Toast.Show(Me, "Error SQL: " & resultado.Mensaje, ToastType.Error)
                    End If
                End If
            End Using

        Catch ex As Exception
            Toast.Show(Me, "Error crítico: " & ex.Message, ToastType.Error)
        End Try
    End Function

    Private Class ResultadoSP
        Public Property Resultado As Integer
        Public Property Mensaje As String
    End Class

    Private Sub dgvOficinas_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvOficinas.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex <> -1 Then
            If dgvOficinas.Columns(e.ColumnIndex).Name <> "colSeleccionar" Then
                Dim celda As DataGridViewCheckBoxCell = CType(dgvOficinas.Rows(e.RowIndex).Cells("colSeleccionar"), DataGridViewCheckBoxCell)
                celda.Value = Not Convert.ToBoolean(celda.Value)
            End If
        End If
    End Sub

End Class
