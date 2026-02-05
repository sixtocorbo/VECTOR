<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAuditoria
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.TabControlAuditoria = New System.Windows.Forms.TabControl()
        Me.TabPageAuditoria = New System.Windows.Forms.TabPage()
        Me.SplitAuditoria = New System.Windows.Forms.SplitContainer()
        Me.dgvAuditoria = New System.Windows.Forms.DataGridView()
        Me.grpDetalleAuditoria = New System.Windows.Forms.GroupBox()
        Me.txtDetalleAuditoriaDescripcion = New System.Windows.Forms.TextBox()
        Me.lblDetalleAuditoriaModulo = New System.Windows.Forms.Label()
        Me.lblDetalleAuditoriaUsuario = New System.Windows.Forms.Label()
        Me.lblDetalleAuditoriaFecha = New System.Windows.Forms.Label()
        Me.PanelAuditoriaFiltros = New System.Windows.Forms.Panel()
        Me.btnAuditoriaLimpiar = New System.Windows.Forms.Button()
        Me.btnAuditoriaBuscar = New System.Windows.Forms.Button()
        Me.txtAuditoriaBuscar = New System.Windows.Forms.TextBox()
        Me.lblAuditoriaBuscar = New System.Windows.Forms.Label()
        Me.cmbAuditoriaModulo = New System.Windows.Forms.ComboBox()
        Me.lblAuditoriaModulo = New System.Windows.Forms.Label()
        Me.cmbAuditoriaUsuario = New System.Windows.Forms.ComboBox()
        Me.lblAuditoriaUsuario = New System.Windows.Forms.Label()
        Me.dtpAuditoriaHasta = New System.Windows.Forms.DateTimePicker()
        Me.lblAuditoriaHasta = New System.Windows.Forms.Label()
        Me.dtpAuditoriaDesde = New System.Windows.Forms.DateTimePicker()
        Me.lblAuditoriaDesde = New System.Windows.Forms.Label()
        Me.TabPageTransacciones = New System.Windows.Forms.TabPage()
        Me.SplitTransacciones = New System.Windows.Forms.SplitContainer()
        Me.dgvTransacciones = New System.Windows.Forms.DataGridView()
        Me.grpDetalleTransaccion = New System.Windows.Forms.GroupBox()
        Me.txtDetalleTransaccionObservacion = New System.Windows.Forms.TextBox()
        Me.lblDetalleTransaccionResponsable = New System.Windows.Forms.Label()
        Me.lblDetalleTransaccionEstado = New System.Windows.Forms.Label()
        Me.lblDetalleTransaccionFecha = New System.Windows.Forms.Label()
        Me.PanelTransaccionesFiltros = New System.Windows.Forms.Panel()
        Me.btnTransaccionesLimpiar = New System.Windows.Forms.Button()
        Me.btnTransaccionesBuscar = New System.Windows.Forms.Button()
        Me.txtTransaccionesBuscar = New System.Windows.Forms.TextBox()
        Me.lblTransaccionesBuscar = New System.Windows.Forms.Label()
        Me.cmbTransaccionesDestino = New System.Windows.Forms.ComboBox()
        Me.lblTransaccionesDestino = New System.Windows.Forms.Label()
        Me.cmbTransaccionesOrigen = New System.Windows.Forms.ComboBox()
        Me.lblTransaccionesOrigen = New System.Windows.Forms.Label()
        Me.cmbTransaccionesUsuario = New System.Windows.Forms.ComboBox()
        Me.lblTransaccionesUsuario = New System.Windows.Forms.Label()
        Me.dtpTransaccionesHasta = New System.Windows.Forms.DateTimePicker()
        Me.lblTransaccionesHasta = New System.Windows.Forms.Label()
        Me.dtpTransaccionesDesde = New System.Windows.Forms.DateTimePicker()
        Me.lblTransaccionesDesde = New System.Windows.Forms.Label()
        Me.TabControlAuditoria.SuspendLayout()
        Me.TabPageAuditoria.SuspendLayout()
        CType(Me.SplitAuditoria, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitAuditoria.Panel1.SuspendLayout()
        Me.SplitAuditoria.Panel2.SuspendLayout()
        Me.SplitAuditoria.SuspendLayout()
        CType(Me.dgvAuditoria, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDetalleAuditoria.SuspendLayout()
        Me.PanelAuditoriaFiltros.SuspendLayout()
        Me.TabPageTransacciones.SuspendLayout()
        CType(Me.SplitTransacciones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitTransacciones.Panel1.SuspendLayout()
        Me.SplitTransacciones.Panel2.SuspendLayout()
        Me.SplitTransacciones.SuspendLayout()
        CType(Me.dgvTransacciones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDetalleTransaccion.SuspendLayout()
        Me.PanelTransaccionesFiltros.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControlAuditoria
        '
        Me.TabControlAuditoria.Controls.Add(Me.TabPageAuditoria)
        Me.TabControlAuditoria.Controls.Add(Me.TabPageTransacciones)
        Me.TabControlAuditoria.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlAuditoria.Location = New System.Drawing.Point(0, 0)
        Me.TabControlAuditoria.Name = "TabControlAuditoria"
        Me.TabControlAuditoria.SelectedIndex = 0
        Me.TabControlAuditoria.Size = New System.Drawing.Size(1200, 700)
        Me.TabControlAuditoria.TabIndex = 0
        '
        'TabPageAuditoria
        '
        Me.TabPageAuditoria.Controls.Add(Me.SplitAuditoria)
        Me.TabPageAuditoria.Controls.Add(Me.PanelAuditoriaFiltros)
        Me.TabPageAuditoria.Location = New System.Drawing.Point(4, 29)
        Me.TabPageAuditoria.Name = "TabPageAuditoria"
        Me.TabPageAuditoria.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageAuditoria.Size = New System.Drawing.Size(1192, 667)
        Me.TabPageAuditoria.TabIndex = 0
        Me.TabPageAuditoria.Text = "Auditoría"
        Me.TabPageAuditoria.UseVisualStyleBackColor = True
        '
        'SplitAuditoria
        '
        Me.SplitAuditoria.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitAuditoria.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitAuditoria.Location = New System.Drawing.Point(3, 95)
        Me.SplitAuditoria.Name = "SplitAuditoria"
        Me.SplitAuditoria.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitAuditoria.Panel1
        '
        Me.SplitAuditoria.Panel1.Controls.Add(Me.dgvAuditoria)
        '
        'SplitAuditoria.Panel2
        '
        Me.SplitAuditoria.Panel2.Controls.Add(Me.grpDetalleAuditoria)
        Me.SplitAuditoria.Panel2.Padding = New System.Windows.Forms.Padding(0, 0, 0, 8)
        Me.SplitAuditoria.Panel2MinSize = 185
        Me.SplitAuditoria.Size = New System.Drawing.Size(1186, 569)
        Me.SplitAuditoria.SplitterDistance = 380
        Me.SplitAuditoria.TabIndex = 1
        '
        'dgvAuditoria
        '
        Me.dgvAuditoria.AllowUserToAddRows = False
        Me.dgvAuditoria.AllowUserToDeleteRows = False
        Me.dgvAuditoria.AllowUserToResizeColumns = False
        Me.dgvAuditoria.AllowUserToResizeRows = False
        Me.dgvAuditoria.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvAuditoria.BackgroundColor = System.Drawing.Color.White
        Me.dgvAuditoria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAuditoria.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAuditoria.Location = New System.Drawing.Point(0, 0)
        Me.dgvAuditoria.MultiSelect = False
        Me.dgvAuditoria.Name = "dgvAuditoria"
        Me.dgvAuditoria.ReadOnly = True
        Me.dgvAuditoria.RowHeadersVisible = False
        Me.dgvAuditoria.RowHeadersWidth = 62
        Me.dgvAuditoria.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAuditoria.Size = New System.Drawing.Size(1186, 380)
        Me.dgvAuditoria.TabIndex = 0
        '
        'grpDetalleAuditoria
        '
        Me.grpDetalleAuditoria.Controls.Add(Me.txtDetalleAuditoriaDescripcion)
        Me.grpDetalleAuditoria.Controls.Add(Me.lblDetalleAuditoriaModulo)
        Me.grpDetalleAuditoria.Controls.Add(Me.lblDetalleAuditoriaUsuario)
        Me.grpDetalleAuditoria.Controls.Add(Me.lblDetalleAuditoriaFecha)
        Me.grpDetalleAuditoria.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpDetalleAuditoria.Location = New System.Drawing.Point(0, 0)
        Me.grpDetalleAuditoria.Name = "grpDetalleAuditoria"
        Me.grpDetalleAuditoria.Size = New System.Drawing.Size(1186, 177)
        Me.grpDetalleAuditoria.TabIndex = 0
        Me.grpDetalleAuditoria.TabStop = False
        Me.grpDetalleAuditoria.Text = "Detalle de auditoría"
        '
        'txtDetalleAuditoriaDescripcion
        '
        Me.txtDetalleAuditoriaDescripcion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDetalleAuditoriaDescripcion.Location = New System.Drawing.Point(16, 75)
        Me.txtDetalleAuditoriaDescripcion.Multiline = True
        Me.txtDetalleAuditoriaDescripcion.Name = "txtDetalleAuditoriaDescripcion"
        Me.txtDetalleAuditoriaDescripcion.ReadOnly = True
        Me.txtDetalleAuditoriaDescripcion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDetalleAuditoriaDescripcion.Size = New System.Drawing.Size(1154, 87)
        Me.txtDetalleAuditoriaDescripcion.TabIndex = 3
        '
        'lblDetalleAuditoriaModulo
        '
        Me.lblDetalleAuditoriaModulo.AutoSize = True
        Me.lblDetalleAuditoriaModulo.Location = New System.Drawing.Point(12, 50)
        Me.lblDetalleAuditoriaModulo.Name = "lblDetalleAuditoriaModulo"
        Me.lblDetalleAuditoriaModulo.Size = New System.Drawing.Size(65, 20)
        Me.lblDetalleAuditoriaModulo.TabIndex = 2
        Me.lblDetalleAuditoriaModulo.Text = "Módulo:"
        '
        'lblDetalleAuditoriaUsuario
        '
        Me.lblDetalleAuditoriaUsuario.AutoSize = True
        Me.lblDetalleAuditoriaUsuario.Location = New System.Drawing.Point(270, 25)
        Me.lblDetalleAuditoriaUsuario.Name = "lblDetalleAuditoriaUsuario"
        Me.lblDetalleAuditoriaUsuario.Size = New System.Drawing.Size(68, 20)
        Me.lblDetalleAuditoriaUsuario.TabIndex = 1
        Me.lblDetalleAuditoriaUsuario.Text = "Usuario:"
        '
        'lblDetalleAuditoriaFecha
        '
        Me.lblDetalleAuditoriaFecha.AutoSize = True
        Me.lblDetalleAuditoriaFecha.Location = New System.Drawing.Point(12, 25)
        Me.lblDetalleAuditoriaFecha.Name = "lblDetalleAuditoriaFecha"
        Me.lblDetalleAuditoriaFecha.Size = New System.Drawing.Size(58, 20)
        Me.lblDetalleAuditoriaFecha.TabIndex = 0
        Me.lblDetalleAuditoriaFecha.Text = "Fecha:"
        '
        'PanelAuditoriaFiltros
        '
        Me.PanelAuditoriaFiltros.Controls.Add(Me.btnAuditoriaLimpiar)
        Me.PanelAuditoriaFiltros.Controls.Add(Me.btnAuditoriaBuscar)
        Me.PanelAuditoriaFiltros.Controls.Add(Me.txtAuditoriaBuscar)
        Me.PanelAuditoriaFiltros.Controls.Add(Me.lblAuditoriaBuscar)
        Me.PanelAuditoriaFiltros.Controls.Add(Me.cmbAuditoriaModulo)
        Me.PanelAuditoriaFiltros.Controls.Add(Me.lblAuditoriaModulo)
        Me.PanelAuditoriaFiltros.Controls.Add(Me.cmbAuditoriaUsuario)
        Me.PanelAuditoriaFiltros.Controls.Add(Me.lblAuditoriaUsuario)
        Me.PanelAuditoriaFiltros.Controls.Add(Me.dtpAuditoriaHasta)
        Me.PanelAuditoriaFiltros.Controls.Add(Me.lblAuditoriaHasta)
        Me.PanelAuditoriaFiltros.Controls.Add(Me.dtpAuditoriaDesde)
        Me.PanelAuditoriaFiltros.Controls.Add(Me.lblAuditoriaDesde)
        Me.PanelAuditoriaFiltros.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelAuditoriaFiltros.Location = New System.Drawing.Point(3, 3)
        Me.PanelAuditoriaFiltros.Name = "PanelAuditoriaFiltros"
        Me.PanelAuditoriaFiltros.Size = New System.Drawing.Size(1186, 92)
        Me.PanelAuditoriaFiltros.TabIndex = 0
        '
        'btnAuditoriaLimpiar
        '
        Me.btnAuditoriaLimpiar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAuditoriaLimpiar.Location = New System.Drawing.Point(1088, 50)
        Me.btnAuditoriaLimpiar.Name = "btnAuditoriaLimpiar"
        Me.btnAuditoriaLimpiar.Size = New System.Drawing.Size(86, 30)
        Me.btnAuditoriaLimpiar.TabIndex = 11
        Me.btnAuditoriaLimpiar.Text = "Limpiar"
        Me.btnAuditoriaLimpiar.UseVisualStyleBackColor = True
        '
        'btnAuditoriaBuscar
        '
        Me.btnAuditoriaBuscar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAuditoriaBuscar.Location = New System.Drawing.Point(1088, 12)
        Me.btnAuditoriaBuscar.Name = "btnAuditoriaBuscar"
        Me.btnAuditoriaBuscar.Size = New System.Drawing.Size(86, 30)
        Me.btnAuditoriaBuscar.TabIndex = 10
        Me.btnAuditoriaBuscar.Text = "Buscar"
        Me.btnAuditoriaBuscar.UseVisualStyleBackColor = True
        '
        'txtAuditoriaBuscar
        '
        Me.txtAuditoriaBuscar.Location = New System.Drawing.Point(498, 50)
        Me.txtAuditoriaBuscar.Name = "txtAuditoriaBuscar"
        Me.txtAuditoriaBuscar.Size = New System.Drawing.Size(300, 26)
        Me.txtAuditoriaBuscar.TabIndex = 9
        '
        'lblAuditoriaBuscar
        '
        Me.lblAuditoriaBuscar.AutoSize = True
        Me.lblAuditoriaBuscar.Location = New System.Drawing.Point(426, 53)
        Me.lblAuditoriaBuscar.Name = "lblAuditoriaBuscar"
        Me.lblAuditoriaBuscar.Size = New System.Drawing.Size(63, 20)
        Me.lblAuditoriaBuscar.TabIndex = 8
        Me.lblAuditoriaBuscar.Text = "Buscar:"
        '
        'cmbAuditoriaModulo
        '
        Me.cmbAuditoriaModulo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAuditoriaModulo.FormattingEnabled = True
        Me.cmbAuditoriaModulo.Location = New System.Drawing.Point(498, 12)
        Me.cmbAuditoriaModulo.Name = "cmbAuditoriaModulo"
        Me.cmbAuditoriaModulo.Size = New System.Drawing.Size(300, 28)
        Me.cmbAuditoriaModulo.TabIndex = 7
        '
        'lblAuditoriaModulo
        '
        Me.lblAuditoriaModulo.AutoSize = True
        Me.lblAuditoriaModulo.Location = New System.Drawing.Point(426, 16)
        Me.lblAuditoriaModulo.Name = "lblAuditoriaModulo"
        Me.lblAuditoriaModulo.Size = New System.Drawing.Size(65, 20)
        Me.lblAuditoriaModulo.TabIndex = 6
        Me.lblAuditoriaModulo.Text = "Módulo:"
        '
        'cmbAuditoriaUsuario
        '
        Me.cmbAuditoriaUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAuditoriaUsuario.FormattingEnabled = True
        Me.cmbAuditoriaUsuario.Location = New System.Drawing.Point(98, 50)
        Me.cmbAuditoriaUsuario.Name = "cmbAuditoriaUsuario"
        Me.cmbAuditoriaUsuario.Size = New System.Drawing.Size(300, 28)
        Me.cmbAuditoriaUsuario.TabIndex = 5
        '
        'lblAuditoriaUsuario
        '
        Me.lblAuditoriaUsuario.AutoSize = True
        Me.lblAuditoriaUsuario.Location = New System.Drawing.Point(12, 53)
        Me.lblAuditoriaUsuario.Name = "lblAuditoriaUsuario"
        Me.lblAuditoriaUsuario.Size = New System.Drawing.Size(68, 20)
        Me.lblAuditoriaUsuario.TabIndex = 4
        Me.lblAuditoriaUsuario.Text = "Usuario:"
        '
        'dtpAuditoriaHasta
        '
        Me.dtpAuditoriaHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpAuditoriaHasta.Location = New System.Drawing.Point(274, 12)
        Me.dtpAuditoriaHasta.Name = "dtpAuditoriaHasta"
        Me.dtpAuditoriaHasta.Size = New System.Drawing.Size(124, 26)
        Me.dtpAuditoriaHasta.TabIndex = 3
        '
        'lblAuditoriaHasta
        '
        Me.lblAuditoriaHasta.AutoSize = True
        Me.lblAuditoriaHasta.Location = New System.Drawing.Point(220, 16)
        Me.lblAuditoriaHasta.Name = "lblAuditoriaHasta"
        Me.lblAuditoriaHasta.Size = New System.Drawing.Size(56, 20)
        Me.lblAuditoriaHasta.TabIndex = 2
        Me.lblAuditoriaHasta.Text = "Hasta:"
        '
        'dtpAuditoriaDesde
        '
        Me.dtpAuditoriaDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpAuditoriaDesde.Location = New System.Drawing.Point(98, 12)
        Me.dtpAuditoriaDesde.Name = "dtpAuditoriaDesde"
        Me.dtpAuditoriaDesde.Size = New System.Drawing.Size(118, 26)
        Me.dtpAuditoriaDesde.TabIndex = 1
        '
        'lblAuditoriaDesde
        '
        Me.lblAuditoriaDesde.AutoSize = True
        Me.lblAuditoriaDesde.Location = New System.Drawing.Point(12, 16)
        Me.lblAuditoriaDesde.Name = "lblAuditoriaDesde"
        Me.lblAuditoriaDesde.Size = New System.Drawing.Size(60, 20)
        Me.lblAuditoriaDesde.TabIndex = 0
        Me.lblAuditoriaDesde.Text = "Desde:"
        '
        'TabPageTransacciones
        '
        Me.TabPageTransacciones.Controls.Add(Me.SplitTransacciones)
        Me.TabPageTransacciones.Controls.Add(Me.PanelTransaccionesFiltros)
        Me.TabPageTransacciones.Location = New System.Drawing.Point(4, 29)
        Me.TabPageTransacciones.Name = "TabPageTransacciones"
        Me.TabPageTransacciones.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageTransacciones.Size = New System.Drawing.Size(1192, 667)
        Me.TabPageTransacciones.TabIndex = 1
        Me.TabPageTransacciones.Text = "Transacciones"
        Me.TabPageTransacciones.UseVisualStyleBackColor = True
        '
        'SplitTransacciones
        '
        Me.SplitTransacciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitTransacciones.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitTransacciones.Location = New System.Drawing.Point(3, 95)
        Me.SplitTransacciones.Name = "SplitTransacciones"
        Me.SplitTransacciones.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitTransacciones.Panel1
        '
        Me.SplitTransacciones.Panel1.Controls.Add(Me.dgvTransacciones)
        '
        'SplitTransacciones.Panel2
        '
        Me.SplitTransacciones.Panel2.Controls.Add(Me.grpDetalleTransaccion)
        Me.SplitTransacciones.Panel2.Padding = New System.Windows.Forms.Padding(0, 0, 0, 8)
        Me.SplitTransacciones.Panel2MinSize = 185
        Me.SplitTransacciones.Size = New System.Drawing.Size(1186, 569)
        Me.SplitTransacciones.SplitterDistance = 380
        Me.SplitTransacciones.TabIndex = 1
        '
        'dgvTransacciones
        '
        Me.dgvTransacciones.AllowUserToAddRows = False
        Me.dgvTransacciones.AllowUserToDeleteRows = False
        Me.dgvTransacciones.AllowUserToResizeColumns = False
        Me.dgvTransacciones.AllowUserToResizeRows = False
        Me.dgvTransacciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvTransacciones.BackgroundColor = System.Drawing.Color.White
        Me.dgvTransacciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTransacciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvTransacciones.Location = New System.Drawing.Point(0, 0)
        Me.dgvTransacciones.MultiSelect = False
        Me.dgvTransacciones.Name = "dgvTransacciones"
        Me.dgvTransacciones.ReadOnly = True
        Me.dgvTransacciones.RowHeadersVisible = False
        Me.dgvTransacciones.RowHeadersWidth = 62
        Me.dgvTransacciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTransacciones.Size = New System.Drawing.Size(1186, 380)
        Me.dgvTransacciones.TabIndex = 0
        '
        'grpDetalleTransaccion
        '
        Me.grpDetalleTransaccion.Controls.Add(Me.txtDetalleTransaccionObservacion)
        Me.grpDetalleTransaccion.Controls.Add(Me.lblDetalleTransaccionResponsable)
        Me.grpDetalleTransaccion.Controls.Add(Me.lblDetalleTransaccionEstado)
        Me.grpDetalleTransaccion.Controls.Add(Me.lblDetalleTransaccionFecha)
        Me.grpDetalleTransaccion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpDetalleTransaccion.Location = New System.Drawing.Point(0, 0)
        Me.grpDetalleTransaccion.Name = "grpDetalleTransaccion"
        Me.grpDetalleTransaccion.Size = New System.Drawing.Size(1186, 177)
        Me.grpDetalleTransaccion.TabIndex = 0
        Me.grpDetalleTransaccion.TabStop = False
        Me.grpDetalleTransaccion.Text = "Detalle de transacción"
        '
        'txtDetalleTransaccionObservacion
        '
        Me.txtDetalleTransaccionObservacion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDetalleTransaccionObservacion.Location = New System.Drawing.Point(16, 48)
        Me.txtDetalleTransaccionObservacion.Multiline = True
        Me.txtDetalleTransaccionObservacion.Name = "txtDetalleTransaccionObservacion"
        Me.txtDetalleTransaccionObservacion.ReadOnly = True
        Me.txtDetalleTransaccionObservacion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDetalleTransaccionObservacion.Size = New System.Drawing.Size(1154, 114)
        Me.txtDetalleTransaccionObservacion.TabIndex = 6
        '
        'lblDetalleTransaccionResponsable
        '
        Me.lblDetalleTransaccionResponsable.AutoSize = True
        Me.lblDetalleTransaccionResponsable.Location = New System.Drawing.Point(582, 25)
        Me.lblDetalleTransaccionResponsable.Name = "lblDetalleTransaccionResponsable"
        Me.lblDetalleTransaccionResponsable.Size = New System.Drawing.Size(107, 20)
        Me.lblDetalleTransaccionResponsable.TabIndex = 5
        Me.lblDetalleTransaccionResponsable.Text = "Responsable:"
        '
        'lblDetalleTransaccionEstado
        '
        Me.lblDetalleTransaccionEstado.AutoSize = True
        Me.lblDetalleTransaccionEstado.Location = New System.Drawing.Point(297, 25)
        Me.lblDetalleTransaccionEstado.Name = "lblDetalleTransaccionEstado"
        Me.lblDetalleTransaccionEstado.Size = New System.Drawing.Size(64, 20)
        Me.lblDetalleTransaccionEstado.TabIndex = 4
        Me.lblDetalleTransaccionEstado.Text = "Estado:"
        '
        'lblDetalleTransaccionFecha
        '
        Me.lblDetalleTransaccionFecha.AutoSize = True
        Me.lblDetalleTransaccionFecha.Location = New System.Drawing.Point(12, 25)
        Me.lblDetalleTransaccionFecha.Name = "lblDetalleTransaccionFecha"
        Me.lblDetalleTransaccionFecha.Size = New System.Drawing.Size(58, 20)
        Me.lblDetalleTransaccionFecha.TabIndex = 0
        Me.lblDetalleTransaccionFecha.Text = "Fecha:"
        '
        'PanelTransaccionesFiltros
        '
        Me.PanelTransaccionesFiltros.Controls.Add(Me.btnTransaccionesLimpiar)
        Me.PanelTransaccionesFiltros.Controls.Add(Me.btnTransaccionesBuscar)
        Me.PanelTransaccionesFiltros.Controls.Add(Me.txtTransaccionesBuscar)
        Me.PanelTransaccionesFiltros.Controls.Add(Me.lblTransaccionesBuscar)
        Me.PanelTransaccionesFiltros.Controls.Add(Me.cmbTransaccionesDestino)
        Me.PanelTransaccionesFiltros.Controls.Add(Me.lblTransaccionesDestino)
        Me.PanelTransaccionesFiltros.Controls.Add(Me.cmbTransaccionesOrigen)
        Me.PanelTransaccionesFiltros.Controls.Add(Me.lblTransaccionesOrigen)
        Me.PanelTransaccionesFiltros.Controls.Add(Me.cmbTransaccionesUsuario)
        Me.PanelTransaccionesFiltros.Controls.Add(Me.lblTransaccionesUsuario)
        Me.PanelTransaccionesFiltros.Controls.Add(Me.dtpTransaccionesHasta)
        Me.PanelTransaccionesFiltros.Controls.Add(Me.lblTransaccionesHasta)
        Me.PanelTransaccionesFiltros.Controls.Add(Me.dtpTransaccionesDesde)
        Me.PanelTransaccionesFiltros.Controls.Add(Me.lblTransaccionesDesde)
        Me.PanelTransaccionesFiltros.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelTransaccionesFiltros.Location = New System.Drawing.Point(3, 3)
        Me.PanelTransaccionesFiltros.Name = "PanelTransaccionesFiltros"
        Me.PanelTransaccionesFiltros.Size = New System.Drawing.Size(1186, 92)
        Me.PanelTransaccionesFiltros.TabIndex = 0
        '
        'btnTransaccionesLimpiar
        '
        Me.btnTransaccionesLimpiar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTransaccionesLimpiar.Location = New System.Drawing.Point(1088, 50)
        Me.btnTransaccionesLimpiar.Name = "btnTransaccionesLimpiar"
        Me.btnTransaccionesLimpiar.Size = New System.Drawing.Size(86, 30)
        Me.btnTransaccionesLimpiar.TabIndex = 13
        Me.btnTransaccionesLimpiar.Text = "Limpiar"
        Me.btnTransaccionesLimpiar.UseVisualStyleBackColor = True
        '
        'btnTransaccionesBuscar
        '
        Me.btnTransaccionesBuscar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTransaccionesBuscar.Location = New System.Drawing.Point(1088, 12)
        Me.btnTransaccionesBuscar.Name = "btnTransaccionesBuscar"
        Me.btnTransaccionesBuscar.Size = New System.Drawing.Size(86, 30)
        Me.btnTransaccionesBuscar.TabIndex = 12
        Me.btnTransaccionesBuscar.Text = "Buscar"
        Me.btnTransaccionesBuscar.UseVisualStyleBackColor = True
        '
        'txtTransaccionesBuscar
        '
        Me.txtTransaccionesBuscar.Location = New System.Drawing.Point(494, 50)
        Me.txtTransaccionesBuscar.Name = "txtTransaccionesBuscar"
        Me.txtTransaccionesBuscar.Size = New System.Drawing.Size(304, 26)
        Me.txtTransaccionesBuscar.TabIndex = 11
        '
        'lblTransaccionesBuscar
        '
        Me.lblTransaccionesBuscar.AutoSize = True
        Me.lblTransaccionesBuscar.Location = New System.Drawing.Point(426, 53)
        Me.lblTransaccionesBuscar.Name = "lblTransaccionesBuscar"
        Me.lblTransaccionesBuscar.Size = New System.Drawing.Size(63, 20)
        Me.lblTransaccionesBuscar.TabIndex = 10
        Me.lblTransaccionesBuscar.Text = "Buscar:"
        '
        'cmbTransaccionesDestino
        '
        Me.cmbTransaccionesDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTransaccionesDestino.FormattingEnabled = True
        Me.cmbTransaccionesDestino.Location = New System.Drawing.Point(494, 12)
        Me.cmbTransaccionesDestino.Name = "cmbTransaccionesDestino"
        Me.cmbTransaccionesDestino.Size = New System.Drawing.Size(304, 28)
        Me.cmbTransaccionesDestino.TabIndex = 9
        '
        'lblTransaccionesDestino
        '
        Me.lblTransaccionesDestino.AutoSize = True
        Me.lblTransaccionesDestino.Location = New System.Drawing.Point(426, 16)
        Me.lblTransaccionesDestino.Name = "lblTransaccionesDestino"
        Me.lblTransaccionesDestino.Size = New System.Drawing.Size(68, 20)
        Me.lblTransaccionesDestino.TabIndex = 8
        Me.lblTransaccionesDestino.Text = "Destino:"
        '
        'cmbTransaccionesOrigen
        '
        Me.cmbTransaccionesOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTransaccionesOrigen.FormattingEnabled = True
        Me.cmbTransaccionesOrigen.Location = New System.Drawing.Point(98, 50)
        Me.cmbTransaccionesOrigen.Name = "cmbTransaccionesOrigen"
        Me.cmbTransaccionesOrigen.Size = New System.Drawing.Size(300, 28)
        Me.cmbTransaccionesOrigen.TabIndex = 7
        '
        'lblTransaccionesOrigen
        '
        Me.lblTransaccionesOrigen.AutoSize = True
        Me.lblTransaccionesOrigen.Location = New System.Drawing.Point(12, 53)
        Me.lblTransaccionesOrigen.Name = "lblTransaccionesOrigen"
        Me.lblTransaccionesOrigen.Size = New System.Drawing.Size(60, 20)
        Me.lblTransaccionesOrigen.TabIndex = 6
        Me.lblTransaccionesOrigen.Text = "Origen:"
        '
        'cmbTransaccionesUsuario
        '
        Me.cmbTransaccionesUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTransaccionesUsuario.FormattingEnabled = True
        Me.cmbTransaccionesUsuario.Location = New System.Drawing.Point(98, 12)
        Me.cmbTransaccionesUsuario.Name = "cmbTransaccionesUsuario"
        Me.cmbTransaccionesUsuario.Size = New System.Drawing.Size(300, 28)
        Me.cmbTransaccionesUsuario.TabIndex = 5
        '
        'lblTransaccionesUsuario
        '
        Me.lblTransaccionesUsuario.AutoSize = True
        Me.lblTransaccionesUsuario.Location = New System.Drawing.Point(12, 16)
        Me.lblTransaccionesUsuario.Name = "lblTransaccionesUsuario"
        Me.lblTransaccionesUsuario.Size = New System.Drawing.Size(68, 20)
        Me.lblTransaccionesUsuario.TabIndex = 4
        Me.lblTransaccionesUsuario.Text = "Usuario:"
        '
        'dtpTransaccionesHasta
        '
        Me.dtpTransaccionesHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTransaccionesHasta.Location = New System.Drawing.Point(878, 50)
        Me.dtpTransaccionesHasta.Name = "dtpTransaccionesHasta"
        Me.dtpTransaccionesHasta.Size = New System.Drawing.Size(124, 26)
        Me.dtpTransaccionesHasta.TabIndex = 3
        '
        'lblTransaccionesHasta
        '
        Me.lblTransaccionesHasta.AutoSize = True
        Me.lblTransaccionesHasta.Location = New System.Drawing.Point(824, 53)
        Me.lblTransaccionesHasta.Name = "lblTransaccionesHasta"
        Me.lblTransaccionesHasta.Size = New System.Drawing.Size(56, 20)
        Me.lblTransaccionesHasta.TabIndex = 2
        Me.lblTransaccionesHasta.Text = "Hasta:"
        '
        'dtpTransaccionesDesde
        '
        Me.dtpTransaccionesDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTransaccionesDesde.Location = New System.Drawing.Point(878, 12)
        Me.dtpTransaccionesDesde.Name = "dtpTransaccionesDesde"
        Me.dtpTransaccionesDesde.Size = New System.Drawing.Size(124, 26)
        Me.dtpTransaccionesDesde.TabIndex = 1
        '
        'lblTransaccionesDesde
        '
        Me.lblTransaccionesDesde.AutoSize = True
        Me.lblTransaccionesDesde.Location = New System.Drawing.Point(824, 16)
        Me.lblTransaccionesDesde.Name = "lblTransaccionesDesde"
        Me.lblTransaccionesDesde.Size = New System.Drawing.Size(60, 20)
        Me.lblTransaccionesDesde.TabIndex = 0
        Me.lblTransaccionesDesde.Text = "Desde:"
        '
        'frmAuditoria
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1200, 700)
        Me.Controls.Add(Me.TabControlAuditoria)
        Me.Name = "frmAuditoria"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Auditoría y Transacciones"
        Me.TabControlAuditoria.ResumeLayout(False)
        Me.TabPageAuditoria.ResumeLayout(False)
        Me.SplitAuditoria.Panel1.ResumeLayout(False)
        Me.SplitAuditoria.Panel2.ResumeLayout(False)
        CType(Me.SplitAuditoria, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitAuditoria.ResumeLayout(False)
        CType(Me.dgvAuditoria, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDetalleAuditoria.ResumeLayout(False)
        Me.grpDetalleAuditoria.PerformLayout()
        Me.PanelAuditoriaFiltros.ResumeLayout(False)
        Me.PanelAuditoriaFiltros.PerformLayout()
        Me.TabPageTransacciones.ResumeLayout(False)
        Me.SplitTransacciones.Panel1.ResumeLayout(False)
        Me.SplitTransacciones.Panel2.ResumeLayout(False)
        CType(Me.SplitTransacciones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitTransacciones.ResumeLayout(False)
        CType(Me.dgvTransacciones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDetalleTransaccion.ResumeLayout(False)
        Me.grpDetalleTransaccion.PerformLayout()
        Me.PanelTransaccionesFiltros.ResumeLayout(False)
        Me.PanelTransaccionesFiltros.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControlAuditoria As TabControl
    Friend WithEvents TabPageAuditoria As TabPage
    Friend WithEvents TabPageTransacciones As TabPage
    Friend WithEvents PanelAuditoriaFiltros As Panel
    Friend WithEvents SplitAuditoria As SplitContainer
    Friend WithEvents dgvAuditoria As DataGridView
    Friend WithEvents grpDetalleAuditoria As GroupBox
    Friend WithEvents txtDetalleAuditoriaDescripcion As TextBox
    Friend WithEvents lblDetalleAuditoriaModulo As Label
    Friend WithEvents lblDetalleAuditoriaUsuario As Label
    Friend WithEvents lblDetalleAuditoriaFecha As Label
    Friend WithEvents btnAuditoriaLimpiar As Button
    Friend WithEvents btnAuditoriaBuscar As Button
    Friend WithEvents txtAuditoriaBuscar As TextBox
    Friend WithEvents lblAuditoriaBuscar As Label
    Friend WithEvents cmbAuditoriaModulo As ComboBox
    Friend WithEvents lblAuditoriaModulo As Label
    Friend WithEvents cmbAuditoriaUsuario As ComboBox
    Friend WithEvents lblAuditoriaUsuario As Label
    Friend WithEvents dtpAuditoriaHasta As DateTimePicker
    Friend WithEvents lblAuditoriaHasta As Label
    Friend WithEvents dtpAuditoriaDesde As DateTimePicker
    Friend WithEvents lblAuditoriaDesde As Label
    Friend WithEvents PanelTransaccionesFiltros As Panel
    Friend WithEvents SplitTransacciones As SplitContainer
    Friend WithEvents dgvTransacciones As DataGridView
    Friend WithEvents grpDetalleTransaccion As GroupBox
    Friend WithEvents txtDetalleTransaccionObservacion As TextBox
    Friend WithEvents lblDetalleTransaccionResponsable As Label
    Friend WithEvents lblDetalleTransaccionEstado As Label
    Friend WithEvents lblDetalleTransaccionFecha As Label
    Friend WithEvents btnTransaccionesLimpiar As Button
    Friend WithEvents btnTransaccionesBuscar As Button
    Friend WithEvents txtTransaccionesBuscar As TextBox
    Friend WithEvents lblTransaccionesBuscar As Label
    Friend WithEvents cmbTransaccionesDestino As ComboBox
    Friend WithEvents lblTransaccionesDestino As Label
    Friend WithEvents cmbTransaccionesOrigen As ComboBox
    Friend WithEvents lblTransaccionesOrigen As Label
    Friend WithEvents cmbTransaccionesUsuario As ComboBox
    Friend WithEvents lblTransaccionesUsuario As Label
    Friend WithEvents dtpTransaccionesHasta As DateTimePicker
    Friend WithEvents lblTransaccionesHasta As Label
    Friend WithEvents dtpTransaccionesDesde As DateTimePicker
    Friend WithEvents lblTransaccionesDesde As Label
End Class
