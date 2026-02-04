Imports System.Data.Entity

Public Class frmAuditoria

    Private Sub frmAuditoria_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InicializarFechas()
        CargarCombos()
        CargarAuditoria()
        CargarTransacciones()
    End Sub

    Private Sub InicializarFechas()
        Dim hoy = Date.Today
        dtpAuditoriaDesde.Value = hoy.AddDays(-7)
        dtpAuditoriaHasta.Value = hoy
        dtpTransaccionesDesde.Value = hoy.AddDays(-7)
        dtpTransaccionesHasta.Value = hoy
    End Sub

    Private Sub CargarCombos()
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
    End Sub

    Private Sub CargarAuditoria()
        Try
            Using db As New SecretariaDBEntities()
                Dim desde = dtpAuditoriaDesde.Value.Date
                Dim hasta = dtpAuditoriaHasta.Value.Date.AddDays(1).AddTicks(-1)

                ' --- CORRECCIÓN: Conversión segura de ID ---
                Dim usuarioId As Integer = 0
                Integer.TryParse(Convert.ToString(cmbAuditoriaUsuario.SelectedValue), usuarioId)
                ' -------------------------------------------

                Dim modulo As String = If(cmbAuditoriaModulo.SelectedItem Is Nothing, "Todos", cmbAuditoriaModulo.SelectedItem.ToString())
                Dim texto = txtAuditoriaBuscar.Text.Trim()

                Dim query = db.EventosSistema.Include("Cat_Usuario").AsQueryable()
                query = query.Where(Function(e) e.FechaEvento >= desde AndAlso e.FechaEvento <= hasta)

                If usuarioId > 0 Then
                    query = query.Where(Function(e) e.UsuarioId = usuarioId)
                End If

                If Not String.Equals(modulo, "Todos", StringComparison.OrdinalIgnoreCase) Then
                    query = query.Where(Function(e) e.Modulo = modulo)
                End If

                If texto <> String.Empty Then
                    query = query.Where(Function(e) e.Descripcion.Contains(texto))
                End If

                Dim datos = query.OrderByDescending(Function(e) e.FechaEvento).Select(Function(e) New With {
                    .IdEvento = e.IdEvento,
                    .Fecha = e.FechaEvento,
                    .Usuario = e.Cat_Usuario.UsuarioLogin,
                    .Modulo = e.Modulo,
                    .Descripcion = e.Descripcion
                }).ToList()

                dgvAuditoria.DataSource = datos
                ConfigurarGridAuditoria()
                MostrarDetalleAuditoria()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar la auditoría: " & ex.Message, "Auditoría", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CargarTransacciones()
        Try
            Using db As New SecretariaDBEntities()
                Dim desde = dtpTransaccionesDesde.Value.Date
                Dim hasta = dtpTransaccionesHasta.Value.Date.AddDays(1).AddTicks(-1)

                ' --- CORRECCIÓN: Conversión segura de IDs ---
                Dim usuarioId As Integer = 0
                Integer.TryParse(Convert.ToString(cmbTransaccionesUsuario.SelectedValue), usuarioId)

                Dim origenId As Integer = 0
                Integer.TryParse(Convert.ToString(cmbTransaccionesOrigen.SelectedValue), origenId)

                Dim destinoId As Integer = 0
                Integer.TryParse(Convert.ToString(cmbTransaccionesDestino.SelectedValue), destinoId)
                ' -------------------------------------------

                Dim texto = txtTransaccionesBuscar.Text.Trim()

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

                If texto <> String.Empty Then
                    query = query.Where(Function(m) m.Mae_Documento.NumeroOficial.Contains(texto) OrElse m.Mae_Documento.NumeroInterno.Contains(texto) OrElse m.Mae_Documento.Asunto.Contains(texto))
                End If

                Dim datos = query.OrderByDescending(Function(m) m.FechaMovimiento).Select(Function(m) New With {
                    .IdMovimiento = m.IdMovimiento,
                    .Fecha = m.FechaMovimiento,
                    .Documento = m.Mae_Documento.Cat_TipoDocumento.Codigo & " " & m.Mae_Documento.NumeroOficial,
                    .Origen = m.Cat_Oficina.Nombre,
                    .Destino = m.Cat_Oficina1.Nombre,
                    .Estado = m.Cat_Estado.Nombre,
                    .Responsable = If(m.Cat_Usuario Is Nothing, "", m.Cat_Usuario.UsuarioLogin),
                    .Observacion = m.ObservacionPase
                }).ToList()

                dgvTransacciones.DataSource = datos
                ConfigurarGridTransacciones()
                MostrarDetalleTransaccion()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar transacciones: " & ex.Message, "Transacciones", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ConfigurarGridAuditoria()
        If dgvAuditoria.Columns.Count = 0 Then
            Return
        End If

        If dgvAuditoria.Columns.Contains("IdEvento") Then
            dgvAuditoria.Columns("IdEvento").Visible = False
        End If

        If dgvAuditoria.Columns.Contains("Fecha") Then
            dgvAuditoria.Columns("Fecha").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            dgvAuditoria.Columns("Fecha").Width = 140
        End If

        If dgvAuditoria.Columns.Contains("Usuario") Then
            dgvAuditoria.Columns("Usuario").Width = 160
        End If

        If dgvAuditoria.Columns.Contains("Modulo") Then
            dgvAuditoria.Columns("Modulo").Width = 120
        End If

        If dgvAuditoria.Columns.Contains("Descripcion") Then
            dgvAuditoria.Columns("Descripcion").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
        dgvAuditoria.ClearSelection()
    End Sub

    Private Sub ConfigurarGridTransacciones()
        ' 1. Validación inicial de seguridad
        If dgvTransacciones Is Nothing OrElse dgvTransacciones.Columns.Count = 0 Then
            Return
        End If

        Try
            ' --- CORRECCIÓN: Columna ID ---
            Dim colId = ObtenerColumnaTransaccion("IdMovimiento")
            If colId IsNot Nothing Then
                colId.Visible = False
            End If

            ' --- CORRECCIÓN CRÍTICA: Columna Fecha ---
            Dim colFecha = ObtenerColumnaTransaccion("Fecha")
            If colFecha IsNot Nothing Then
                ' IMPORTANTE: Primero desactivar AutoSize antes de cambiar el Width para evitar el crash
                colFecha.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                colFecha.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
                colFecha.Width = 140
            End If

            ' --- Resto de columnas ---
            Dim colDocumento = ObtenerColumnaTransaccion("Documento")
            If colDocumento IsNot Nothing Then
                colDocumento.Width = 160
            End If

            Dim colOrigen = ObtenerColumnaTransaccion("Origen")
            If colOrigen IsNot Nothing Then
                colOrigen.Width = 160
            End If

            Dim colDestino = ObtenerColumnaTransaccion("Destino")
            If colDestino IsNot Nothing Then
                colDestino.Width = 160
            End If

            Dim colEstado = ObtenerColumnaTransaccion("Estado")
            If colEstado IsNot Nothing Then
                colEstado.Width = 120
            End If

            Dim colResponsable = ObtenerColumnaTransaccion("Responsable")
            If colResponsable IsNot Nothing Then
                colResponsable.Width = 120
            End If

            ' La columna de Observación SI puede ser Fill, pero no le asignes Width fijo después
            Dim colObservacion = ObtenerColumnaTransaccion("Observacion")
            If colObservacion IsNot Nothing Then
                colObservacion.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If

            dgvTransacciones.ClearSelection()

        Catch ex As Exception
            ' Este Try-Catch evita que un error visual detenga toda la aplicación
            Console.WriteLine("Error configurando grid: " & ex.Message)
        End Try
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

    Private Sub btnAuditoriaBuscar_Click(sender As Object, e As EventArgs) Handles btnAuditoriaBuscar.Click
        CargarAuditoria()
    End Sub

    Private Sub btnAuditoriaLimpiar_Click(sender As Object, e As EventArgs) Handles btnAuditoriaLimpiar.Click
        InicializarFechas()
        txtAuditoriaBuscar.Clear()
        cmbAuditoriaUsuario.SelectedIndex = 0
        cmbAuditoriaModulo.SelectedIndex = 0
        CargarAuditoria()
    End Sub

    Private Sub btnTransaccionesBuscar_Click(sender As Object, e As EventArgs) Handles btnTransaccionesBuscar.Click
        CargarTransacciones()
    End Sub

    Private Sub btnTransaccionesLimpiar_Click(sender As Object, e As EventArgs) Handles btnTransaccionesLimpiar.Click
        InicializarFechas()
        txtTransaccionesBuscar.Clear()
        cmbTransaccionesUsuario.SelectedIndex = 0
        cmbTransaccionesOrigen.SelectedIndex = 0
        cmbTransaccionesDestino.SelectedIndex = 0
        CargarTransacciones()
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
        lblDetalleTransaccionFecha.Text = "Fecha: " & Convert.ToDateTime(fila.Cells("Fecha").Value).ToString("dd/MM/yyyy HH:mm")
        lblDetalleTransaccionDocumento.Text = "Documento: " & Convert.ToString(fila.Cells("Documento").Value)
        lblDetalleTransaccionOrigen.Text = "Origen: " & Convert.ToString(fila.Cells("Origen").Value)
        lblDetalleTransaccionDestino.Text = "Destino: " & Convert.ToString(fila.Cells("Destino").Value)
        lblDetalleTransaccionEstado.Text = "Estado: " & Convert.ToString(fila.Cells("Estado").Value)
        lblDetalleTransaccionResponsable.Text = "Responsable: " & Convert.ToString(fila.Cells("Responsable").Value)
        txtDetalleTransaccionObservacion.Text = Convert.ToString(fila.Cells("Observacion").Value)
    End Sub

End Class
