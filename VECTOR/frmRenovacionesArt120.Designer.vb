<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRenovacionesArt120
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
        Me.PanelFiltros = New System.Windows.Forms.Panel()
        Me.btnReactivar = New System.Windows.Forms.Button()
        Me.btnDesactivar = New System.Windows.Forms.Button()
        Me.btnEditar = New System.Windows.Forms.Button()
        Me.btnNueva = New System.Windows.Forms.Button()
        Me.btnAbrirDocumento = New System.Windows.Forms.Button()
        Me.btnConfigurarRenovaciones = New System.Windows.Forms.Button()
        Me.chkSoloActivas = New System.Windows.Forms.CheckBox()
        Me.nudDiasAlerta = New System.Windows.Forms.NumericUpDown()
        Me.lblDiasAlerta = New System.Windows.Forms.Label()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblResumen = New System.Windows.Forms.Label()
        Me.dgvSalidas = New System.Windows.Forms.DataGridView()
        Me.PanelEditor = New System.Windows.Forms.Panel()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.txtObservaciones = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.btnQuitarDocumento = New System.Windows.Forms.Button()
        Me.lstDocumentosRespaldo = New System.Windows.Forms.ListBox()
        Me.btnAgregarDocumento = New System.Windows.Forms.Button()
        Me.cboDocumentoRespaldo = New System.Windows.Forms.ComboBox()
        Me.btnRefrescarDocumentos = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.chkActivoRegistro = New System.Windows.Forms.CheckBox()
        Me.dtpVencimiento = New System.Windows.Forms.DateTimePicker()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.dtpFechaNotificacion = New System.Windows.Forms.DateTimePicker()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.dtpInicio = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtLugarTrabajo = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtHorario = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtCustodia = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblIdRecluso = New System.Windows.Forms.Label()
        Me.btnBuscarRecluso = New System.Windows.Forms.Button()
        Me.txtRecluso = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PanelFiltros.SuspendLayout()
        CType(Me.nudDiasAlerta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvSalidas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelEditor.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelFiltros
        '
        Me.PanelFiltros.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelFiltros.Controls.Add(Me.btnReactivar)
        Me.PanelFiltros.Controls.Add(Me.btnDesactivar)
        Me.PanelFiltros.Controls.Add(Me.btnEditar)
        Me.PanelFiltros.Controls.Add(Me.btnNueva)
        Me.PanelFiltros.Controls.Add(Me.btnAbrirDocumento)
        Me.PanelFiltros.Controls.Add(Me.btnConfigurarRenovaciones)
        Me.PanelFiltros.Controls.Add(Me.chkSoloActivas)
        Me.PanelFiltros.Controls.Add(Me.nudDiasAlerta)
        Me.PanelFiltros.Controls.Add(Me.lblDiasAlerta)
        Me.PanelFiltros.Controls.Add(Me.txtBuscar)
        Me.PanelFiltros.Controls.Add(Me.Label1)
        Me.PanelFiltros.Controls.Add(Me.lblResumen)
        Me.PanelFiltros.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelFiltros.Location = New System.Drawing.Point(0, 0)
        Me.PanelFiltros.Name = "PanelFiltros"
        Me.PanelFiltros.Size = New System.Drawing.Size(1650, 89)
        Me.PanelFiltros.TabIndex = 0
        '
        'btnReactivar
        '
        Me.btnReactivar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReactivar.BackColor = System.Drawing.Color.DarkCyan
        Me.btnReactivar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReactivar.ForeColor = System.Drawing.Color.White
        Me.btnReactivar.Location = New System.Drawing.Point(1494, 12)
        Me.btnReactivar.Name = "btnReactivar"
        Me.btnReactivar.Size = New System.Drawing.Size(144, 32)
        Me.btnReactivar.TabIndex = 8
        Me.btnReactivar.Text = "Reactivar"
        Me.btnReactivar.UseVisualStyleBackColor = False
        '
        'btnDesactivar
        '
        Me.btnDesactivar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDesactivar.BackColor = System.Drawing.Color.IndianRed
        Me.btnDesactivar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDesactivar.ForeColor = System.Drawing.Color.White
        Me.btnDesactivar.Location = New System.Drawing.Point(1344, 12)
        Me.btnDesactivar.Name = "btnDesactivar"
        Me.btnDesactivar.Size = New System.Drawing.Size(144, 32)
        Me.btnDesactivar.TabIndex = 7
        Me.btnDesactivar.Text = "Desactivar"
        Me.btnDesactivar.UseVisualStyleBackColor = False
        '
        'btnEditar
        '
        Me.btnEditar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditar.BackColor = System.Drawing.Color.Goldenrod
        Me.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEditar.ForeColor = System.Drawing.Color.White
        Me.btnEditar.Location = New System.Drawing.Point(1194, 12)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(144, 32)
        Me.btnEditar.TabIndex = 6
        Me.btnEditar.Text = "Editar"
        Me.btnEditar.UseVisualStyleBackColor = False
        '
        'btnNueva
        '
        Me.btnNueva.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNueva.BackColor = System.Drawing.Color.ForestGreen
        Me.btnNueva.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNueva.ForeColor = System.Drawing.Color.White
        Me.btnNueva.Location = New System.Drawing.Point(1044, 12)
        Me.btnNueva.Name = "btnNueva"
        Me.btnNueva.Size = New System.Drawing.Size(144, 32)
        Me.btnNueva.TabIndex = 5
        Me.btnNueva.Text = "Nueva salida"
        Me.btnNueva.UseVisualStyleBackColor = False
        '
        'btnAbrirDocumento
        '
        Me.btnAbrirDocumento.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAbrirDocumento.BackColor = System.Drawing.Color.SteelBlue
        Me.btnAbrirDocumento.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAbrirDocumento.ForeColor = System.Drawing.Color.White
        Me.btnAbrirDocumento.Location = New System.Drawing.Point(1494, 50)
        Me.btnAbrirDocumento.Name = "btnAbrirDocumento"
        Me.btnAbrirDocumento.Size = New System.Drawing.Size(144, 32)
        Me.btnAbrirDocumento.TabIndex = 4
        Me.btnAbrirDocumento.Text = "Documentación"
        Me.btnAbrirDocumento.UseVisualStyleBackColor = False
        '
        'btnConfigurarRenovaciones
        '
        Me.btnConfigurarRenovaciones.BackColor = System.Drawing.Color.MediumSlateBlue
        Me.btnConfigurarRenovaciones.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConfigurarRenovaciones.ForeColor = System.Drawing.Color.White
        Me.btnConfigurarRenovaciones.Location = New System.Drawing.Point(620, 48)
        Me.btnConfigurarRenovaciones.Name = "btnConfigurarRenovaciones"
        Me.btnConfigurarRenovaciones.Size = New System.Drawing.Size(205, 32)
        Me.btnConfigurarRenovaciones.TabIndex = 10
        Me.btnConfigurarRenovaciones.Text = "Configurar..."
        Me.btnConfigurarRenovaciones.UseVisualStyleBackColor = False
        '
        'chkSoloActivas
        '
        Me.chkSoloActivas.AutoSize = True
        Me.chkSoloActivas.Checked = True
        Me.chkSoloActivas.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSoloActivas.Location = New System.Drawing.Point(15, 16)
        Me.chkSoloActivas.Name = "chkSoloActivas"
        Me.chkSoloActivas.Size = New System.Drawing.Size(120, 24)
        Me.chkSoloActivas.TabIndex = 3
        Me.chkSoloActivas.Text = "Solo activas"
        Me.chkSoloActivas.UseVisualStyleBackColor = True
        '
        'nudDiasAlerta
        '
        Me.nudDiasAlerta.Location = New System.Drawing.Point(313, 16)
        Me.nudDiasAlerta.Maximum = New Decimal(New Integer() {365, 0, 0, 0})
        Me.nudDiasAlerta.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudDiasAlerta.Name = "nudDiasAlerta"
        Me.nudDiasAlerta.Size = New System.Drawing.Size(74, 26)
        Me.nudDiasAlerta.TabIndex = 5
        Me.nudDiasAlerta.Value = New Decimal(New Integer() {30, 0, 0, 0})
        '
        'lblDiasAlerta
        '
        Me.lblDiasAlerta.AutoSize = True
        Me.lblDiasAlerta.Location = New System.Drawing.Point(195, 18)
        Me.lblDiasAlerta.Name = "lblDiasAlerta"
        Me.lblDiasAlerta.Size = New System.Drawing.Size(111, 20)
        Me.lblDiasAlerta.TabIndex = 4
        Me.lblDiasAlerta.Text = "Días de alerta:"
        '
        'txtBuscar
        '
        Me.txtBuscar.Location = New System.Drawing.Point(71, 52)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(543, 26)
        Me.txtBuscar.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Buscar"
        '
        'lblResumen
        '
        Me.lblResumen.AutoSize = True
        Me.lblResumen.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblResumen.ForeColor = System.Drawing.Color.Firebrick
        Me.lblResumen.Location = New System.Drawing.Point(393, 14)
        Me.lblResumen.Name = "lblResumen"
        Me.lblResumen.Size = New System.Drawing.Size(155, 28)
        Me.lblResumen.TabIndex = 0
        Me.lblResumen.Text = "Sin datos aún..."
        '
        'dgvSalidas
        '
        Me.dgvSalidas.AllowUserToAddRows = False
        Me.dgvSalidas.AllowUserToDeleteRows = False
        Me.dgvSalidas.AllowUserToResizeRows = False
        Me.dgvSalidas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvSalidas.BackgroundColor = System.Drawing.Color.White
        Me.dgvSalidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSalidas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSalidas.Location = New System.Drawing.Point(0, 89)
        Me.dgvSalidas.MultiSelect = False
        Me.dgvSalidas.Name = "dgvSalidas"
        Me.dgvSalidas.ReadOnly = True
        Me.dgvSalidas.RowHeadersVisible = False
        Me.dgvSalidas.RowHeadersWidth = 62
        Me.dgvSalidas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvSalidas.Size = New System.Drawing.Size(1650, 487)
        Me.dgvSalidas.TabIndex = 1
        '
        'PanelEditor
        '
        Me.PanelEditor.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelEditor.Controls.Add(Me.btnCancelar)
        Me.PanelEditor.Controls.Add(Me.btnGuardar)
        Me.PanelEditor.Controls.Add(Me.txtObservaciones)
        Me.PanelEditor.Controls.Add(Me.Label8)
        Me.PanelEditor.Controls.Add(Me.btnQuitarDocumento)
        Me.PanelEditor.Controls.Add(Me.lstDocumentosRespaldo)
        Me.PanelEditor.Controls.Add(Me.btnAgregarDocumento)
        Me.PanelEditor.Controls.Add(Me.cboDocumentoRespaldo)
        Me.PanelEditor.Controls.Add(Me.btnRefrescarDocumentos)
        Me.PanelEditor.Controls.Add(Me.Label7)
        Me.PanelEditor.Controls.Add(Me.chkActivoRegistro)
        Me.PanelEditor.Controls.Add(Me.dtpVencimiento)
        Me.PanelEditor.Controls.Add(Me.Label6)
        Me.PanelEditor.Controls.Add(Me.dtpFechaNotificacion)
        Me.PanelEditor.Controls.Add(Me.Label11)
        Me.PanelEditor.Controls.Add(Me.dtpInicio)
        Me.PanelEditor.Controls.Add(Me.Label5)
        Me.PanelEditor.Controls.Add(Me.txtLugarTrabajo)
        Me.PanelEditor.Controls.Add(Me.Label4)
        Me.PanelEditor.Controls.Add(Me.txtHorario)
        Me.PanelEditor.Controls.Add(Me.Label9)
        Me.PanelEditor.Controls.Add(Me.txtCustodia)
        Me.PanelEditor.Controls.Add(Me.Label10)
        Me.PanelEditor.Controls.Add(Me.lblIdRecluso)
        Me.PanelEditor.Controls.Add(Me.btnBuscarRecluso)
        Me.PanelEditor.Controls.Add(Me.txtRecluso)
        Me.PanelEditor.Controls.Add(Me.Label3)
        Me.PanelEditor.Controls.Add(Me.Label2)
        Me.PanelEditor.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelEditor.Location = New System.Drawing.Point(0, 576)
        Me.PanelEditor.Name = "PanelEditor"
        Me.PanelEditor.Size = New System.Drawing.Size(1650, 274)
        Me.PanelEditor.TabIndex = 2
        '
        'btnCancelar
        '
        Me.btnCancelar.BackColor = System.Drawing.Color.DimGray
        Me.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancelar.ForeColor = System.Drawing.Color.White
        Me.btnCancelar.Location = New System.Drawing.Point(1453, 224)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(185, 36)
        Me.btnCancelar.TabIndex = 16
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = False
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SeaGreen
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(1262, 224)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(185, 36)
        Me.btnGuardar.TabIndex = 15
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'txtObservaciones
        '
        Me.txtObservaciones.Location = New System.Drawing.Point(1262, 36)
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtObservaciones.Size = New System.Drawing.Size(376, 182)
        Me.txtObservaciones.TabIndex = 14
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(1258, 13)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(114, 20)
        Me.Label8.TabIndex = 13
        Me.Label8.Text = "Observaciones"
        '
        'btnQuitarDocumento
        '
        Me.btnQuitarDocumento.Enabled = False
        Me.btnQuitarDocumento.Location = New System.Drawing.Point(1188, 69)
        Me.btnQuitarDocumento.Name = "btnQuitarDocumento"
        Me.btnQuitarDocumento.Size = New System.Drawing.Size(54, 31)
        Me.btnQuitarDocumento.TabIndex = 15
        Me.btnQuitarDocumento.Text = "-"
        Me.btnQuitarDocumento.UseVisualStyleBackColor = True
        '
        'lstDocumentosRespaldo
        '
        Me.lstDocumentosRespaldo.FormattingEnabled = True
        Me.lstDocumentosRespaldo.ItemHeight = 20
        Me.lstDocumentosRespaldo.Location = New System.Drawing.Point(861, 70)
        Me.lstDocumentosRespaldo.Name = "lstDocumentosRespaldo"
        Me.lstDocumentosRespaldo.Size = New System.Drawing.Size(321, 84)
        Me.lstDocumentosRespaldo.TabIndex = 14
        '
        'btnAgregarDocumento
        '
        Me.btnAgregarDocumento.Enabled = False
        Me.btnAgregarDocumento.Location = New System.Drawing.Point(1188, 35)
        Me.btnAgregarDocumento.Name = "btnAgregarDocumento"
        Me.btnAgregarDocumento.Size = New System.Drawing.Size(54, 31)
        Me.btnAgregarDocumento.TabIndex = 13
        Me.btnAgregarDocumento.Text = "+"
        Me.btnAgregarDocumento.UseVisualStyleBackColor = True
        '
        'cboDocumentoRespaldo
        '
        Me.cboDocumentoRespaldo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDocumentoRespaldo.FormattingEnabled = True
        Me.cboDocumentoRespaldo.Location = New System.Drawing.Point(861, 36)
        Me.cboDocumentoRespaldo.Name = "cboDocumentoRespaldo"
        Me.cboDocumentoRespaldo.Size = New System.Drawing.Size(321, 28)
        Me.cboDocumentoRespaldo.TabIndex = 12
        '
        'btnRefrescarDocumentos
        '
        Me.btnRefrescarDocumentos.Location = New System.Drawing.Point(1188, 123)
        Me.btnRefrescarDocumentos.Name = "btnRefrescarDocumentos"
        Me.btnRefrescarDocumentos.Size = New System.Drawing.Size(54, 31)
        Me.btnRefrescarDocumentos.TabIndex = 16
        Me.btnRefrescarDocumentos.Text = "↻"
        Me.btnRefrescarDocumentos.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(857, 13)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(322, 20)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Expediente / Documento respaldo (opcional)"
        '
        'chkActivoRegistro
        '
        Me.chkActivoRegistro.AutoSize = True
        Me.chkActivoRegistro.Checked = True
        Me.chkActivoRegistro.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkActivoRegistro.Location = New System.Drawing.Point(462, 203)
        Me.chkActivoRegistro.Name = "chkActivoRegistro"
        Me.chkActivoRegistro.Size = New System.Drawing.Size(140, 24)
        Me.chkActivoRegistro.TabIndex = 17
        Me.chkActivoRegistro.Text = "Registro activo"
        Me.chkActivoRegistro.UseVisualStyleBackColor = True
        '
        'dtpVencimiento
        '
        Me.dtpVencimiento.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpVencimiento.Location = New System.Drawing.Point(660, 171)
        Me.dtpVencimiento.Name = "dtpVencimiento"
        Me.dtpVencimiento.Size = New System.Drawing.Size(185, 26)
        Me.dtpVencimiento.TabIndex = 10
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(656, 148)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(97, 20)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "Vencimiento"
        '
        'dtpFechaNotificacion
        '
        Me.dtpFechaNotificacion.Checked = False
        Me.dtpFechaNotificacion.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaNotificacion.Location = New System.Drawing.Point(462, 105)
        Me.dtpFechaNotificacion.Name = "dtpFechaNotificacion"
        Me.dtpFechaNotificacion.ShowCheckBox = True
        Me.dtpFechaNotificacion.Size = New System.Drawing.Size(185, 26)
        Me.dtpFechaNotificacion.TabIndex = 9
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(458, 82)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(152, 20)
        Me.Label11.TabIndex = 20
        Me.Label11.Text = "Notif. juez (opcional)"
        '
        'dtpInicio
        '
        Me.dtpInicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpInicio.Location = New System.Drawing.Point(462, 171)
        Me.dtpInicio.Name = "dtpInicio"
        Me.dtpInicio.Size = New System.Drawing.Size(185, 26)
        Me.dtpInicio.TabIndex = 8
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(458, 148)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(46, 20)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Inicio"
        '
        'txtLugarTrabajo
        '
        Me.txtLugarTrabajo.Location = New System.Drawing.Point(15, 105)
        Me.txtLugarTrabajo.Name = "txtLugarTrabajo"
        Me.txtLugarTrabajo.Size = New System.Drawing.Size(432, 26)
        Me.txtLugarTrabajo.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 82)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(125, 20)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Lugar de trabajo"
        '
        'txtHorario
        '
        Me.txtHorario.Location = New System.Drawing.Point(462, 36)
        Me.txtHorario.Name = "txtHorario"
        Me.txtHorario.Size = New System.Drawing.Size(383, 26)
        Me.txtHorario.TabIndex = 4
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(458, 13)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(67, 20)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Horario*"
        '
        'txtCustodia
        '
        Me.txtCustodia.Location = New System.Drawing.Point(15, 171)
        Me.txtCustodia.Name = "txtCustodia"
        Me.txtCustodia.Size = New System.Drawing.Size(432, 26)
        Me.txtCustodia.TabIndex = 7
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(11, 148)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(78, 20)
        Me.Label10.TabIndex = 19
        Me.Label10.Text = "Custodia*"
        '
        'lblIdRecluso
        '
        Me.lblIdRecluso.AutoSize = True
        Me.lblIdRecluso.ForeColor = System.Drawing.Color.DimGray
        Me.lblIdRecluso.Location = New System.Drawing.Point(660, 108)
        Me.lblIdRecluso.Name = "lblIdRecluso"
        Me.lblIdRecluso.Size = New System.Drawing.Size(101, 20)
        Me.lblIdRecluso.TabIndex = 4
        Me.lblIdRecluso.Text = "ID: (ninguno)"
        '
        'btnBuscarRecluso
        '
        Me.btnBuscarRecluso.Location = New System.Drawing.Point(402, 35)
        Me.btnBuscarRecluso.Name = "btnBuscarRecluso"
        Me.btnBuscarRecluso.Size = New System.Drawing.Size(45, 31)
        Me.btnBuscarRecluso.TabIndex = 3
        Me.btnBuscarRecluso.Text = "..."
        Me.btnBuscarRecluso.UseVisualStyleBackColor = True
        '
        'txtRecluso
        '
        Me.txtRecluso.Location = New System.Drawing.Point(15, 36)
        Me.txtRecluso.Name = "txtRecluso"
        Me.txtRecluso.ReadOnly = True
        Me.txtRecluso.Size = New System.Drawing.Size(381, 26)
        Me.txtRecluso.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(201, 20)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Persona privada de libertad"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Label2.Location = New System.Drawing.Point(11, 236)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(962, 28)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Registro Decreto 434/013: 1) persona 2) trabajo/horario/custodia 3) fechas/notifi" &
    "cación 4) respaldo"
        '
        'frmRenovacionesArt120
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1650, 850)
        Me.Controls.Add(Me.dgvSalidas)
        Me.Controls.Add(Me.PanelEditor)
        Me.Controls.Add(Me.PanelFiltros)
        Me.Name = "frmRenovacionesArt120"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Renovaciones de salidas laborales - Art. 120"
        Me.PanelFiltros.ResumeLayout(False)
        Me.PanelFiltros.PerformLayout()
        CType(Me.nudDiasAlerta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvSalidas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelEditor.ResumeLayout(False)
        Me.PanelEditor.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelFiltros As Panel
    Friend WithEvents lblResumen As Label
    Friend WithEvents dgvSalidas As DataGridView
    Friend WithEvents txtBuscar As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents chkSoloActivas As CheckBox
    Friend WithEvents nudDiasAlerta As NumericUpDown
    Friend WithEvents lblDiasAlerta As Label
    Friend WithEvents btnAbrirDocumento As Button
    Friend WithEvents btnConfigurarRenovaciones As Button
    Friend WithEvents btnNueva As Button
    Friend WithEvents btnEditar As Button
    Friend WithEvents btnDesactivar As Button
    Friend WithEvents btnReactivar As Button
    Friend WithEvents PanelEditor As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents txtRecluso As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents btnBuscarRecluso As Button
    Friend WithEvents lblIdRecluso As Label
    Friend WithEvents txtLugarTrabajo As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtHorario As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtCustodia As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents dtpInicio As DateTimePicker
    Friend WithEvents Label5 As Label
    Friend WithEvents dtpVencimiento As DateTimePicker
    Friend WithEvents Label6 As Label
    Friend WithEvents dtpFechaNotificacion As DateTimePicker
    Friend WithEvents Label11 As Label
    Friend WithEvents cboDocumentoRespaldo As ComboBox
    Friend WithEvents btnAgregarDocumento As Button
    Friend WithEvents lstDocumentosRespaldo As ListBox
    Friend WithEvents btnQuitarDocumento As Button
    Friend WithEvents btnRefrescarDocumentos As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents chkActivoRegistro As CheckBox
    Friend WithEvents txtObservaciones As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
End Class
