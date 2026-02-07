<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGestionRangos
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
        Me.pnlEditor = New System.Windows.Forms.GroupBox()
        Me.chkActivo = New System.Windows.Forms.CheckBox()
        Me.txtUltimo = New System.Windows.Forms.TextBox()
        Me.lblUltimo = New System.Windows.Forms.Label()
        Me.txtFin = New System.Windows.Forms.TextBox()
        Me.lblFin = New System.Windows.Forms.Label()
        Me.numAnio = New System.Windows.Forms.NumericUpDown()
        Me.lblAnio = New System.Windows.Forms.Label()
        Me.txtCantidad = New System.Windows.Forms.TextBox()
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.txtInicio = New System.Windows.Forms.TextBox()
        Me.lblInicio = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.cmbOficina = New System.Windows.Forms.ComboBox()
        Me.lblOficina = New System.Windows.Forms.Label()
        Me.cmbTipo = New System.Windows.Forms.ComboBox()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.lblStockInfo = New System.Windows.Forms.Label()
        Me.btnCupos = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.dgvRangos = New System.Windows.Forms.DataGridView()
        Me.btnNuevo = New System.Windows.Forms.Button()
        Me.btnEditar = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.pnlEditor.SuspendLayout()
        CType(Me.numAnio, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvRangos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlEditor
        '
        Me.pnlEditor.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlEditor.Controls.Add(Me.chkActivo)
        Me.pnlEditor.Controls.Add(Me.txtUltimo)
        Me.pnlEditor.Controls.Add(Me.lblUltimo)
        Me.pnlEditor.Controls.Add(Me.txtFin)
        Me.pnlEditor.Controls.Add(Me.lblFin)
        Me.pnlEditor.Controls.Add(Me.numAnio)
        Me.pnlEditor.Controls.Add(Me.lblAnio)
        Me.pnlEditor.Controls.Add(Me.txtCantidad)
        Me.pnlEditor.Controls.Add(Me.lblCantidad)
        Me.pnlEditor.Controls.Add(Me.txtInicio)
        Me.pnlEditor.Controls.Add(Me.lblInicio)
        Me.pnlEditor.Controls.Add(Me.txtNombre)
        Me.pnlEditor.Controls.Add(Me.lblNombre)
        Me.pnlEditor.Controls.Add(Me.cmbOficina)
        Me.pnlEditor.Controls.Add(Me.lblOficina)
        Me.pnlEditor.Controls.Add(Me.cmbTipo)
        Me.pnlEditor.Controls.Add(Me.lblTipo)
        Me.pnlEditor.Controls.Add(Me.lblStockInfo)
        Me.pnlEditor.Controls.Add(Me.btnCupos)
        Me.pnlEditor.Enabled = False
        Me.pnlEditor.Location = New System.Drawing.Point(18, 18)
        Me.pnlEditor.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlEditor.Name = "pnlEditor"
        Me.pnlEditor.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlEditor.Size = New System.Drawing.Size(908, 231)
        Me.pnlEditor.TabIndex = 0
        Me.pnlEditor.TabStop = False
        Me.pnlEditor.Text = "Detalle del Rango"
        '
        'chkActivo
        '
        Me.chkActivo.AutoSize = True
        Me.chkActivo.Checked = True
        Me.chkActivo.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkActivo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkActivo.ForeColor = System.Drawing.Color.Green
        Me.chkActivo.Location = New System.Drawing.Point(32, 188)
        Me.chkActivo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkActivo.Name = "chkActivo"
        Me.chkActivo.Size = New System.Drawing.Size(186, 24)
        Me.chkActivo.TabIndex = 5
        Me.chkActivo.Text = "RANGO VIGENTE"
        Me.chkActivo.UseVisualStyleBackColor = True
        '
        'txtUltimo
        '
        Me.txtUltimo.Location = New System.Drawing.Point(740, 126)
        Me.txtUltimo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtUltimo.Name = "txtUltimo"
        Me.txtUltimo.Size = New System.Drawing.Size(148, 26)
        Me.txtUltimo.TabIndex = 6
        Me.txtUltimo.TabStop = False
        Me.txtUltimo.Text = "0"
        Me.txtUltimo.Visible = False
        '
        'lblUltimo
        '
        Me.lblUltimo.AutoSize = True
        Me.lblUltimo.Location = New System.Drawing.Point(736, 101)
        Me.lblUltimo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblUltimo.Name = "lblUltimo"
        Me.lblUltimo.Size = New System.Drawing.Size(109, 20)
        Me.lblUltimo.TabIndex = 7
        Me.lblUltimo.Text = "Último Usado:"
        Me.lblUltimo.Visible = False
        '
        'txtFin
        '
        Me.txtFin.Location = New System.Drawing.Point(563, 126)
        Me.txtFin.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtFin.Name = "txtFin"
        Me.txtFin.ReadOnly = True
        Me.txtFin.Size = New System.Drawing.Size(148, 26)
        Me.txtFin.TabIndex = 4
        '
        'lblFin
        '
        Me.lblFin.AutoSize = True
        Me.lblFin.Location = New System.Drawing.Point(559, 101)
        Me.lblFin.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFin.Name = "lblFin"
        Me.lblFin.Size = New System.Drawing.Size(95, 20)
        Me.lblFin.TabIndex = 4
        Me.lblFin.Text = "Número Fin:"
        '
        'numAnio
        '
        Me.numAnio.Location = New System.Drawing.Point(209, 126)
        Me.numAnio.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.numAnio.Maximum = New Decimal(New Integer() {2100, 0, 0, 0})
        Me.numAnio.Minimum = New Decimal(New Integer() {2000, 0, 0, 0})
        Me.numAnio.Name = "numAnio"
        Me.numAnio.Size = New System.Drawing.Size(148, 26)
        Me.numAnio.TabIndex = 8
        Me.numAnio.Value = New Decimal(New Integer() {2000, 0, 0, 0})
        '
        'lblAnio
        '
        Me.lblAnio.AutoSize = True
        Me.lblAnio.Location = New System.Drawing.Point(205, 101)
        Me.lblAnio.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAnio.Name = "lblAnio"
        Me.lblAnio.Size = New System.Drawing.Size(42, 20)
        Me.lblAnio.TabIndex = 8
        Me.lblAnio.Text = "Año:"
        '
        'txtCantidad
        '
        Me.txtCantidad.Location = New System.Drawing.Point(32, 126)
        Me.txtCantidad.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtCantidad.Name = "txtCantidad"
        Me.txtCantidad.Size = New System.Drawing.Size(148, 26)
        Me.txtCantidad.TabIndex = 3
        '
        'lblCantidad
        '
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Location = New System.Drawing.Point(32, 101)
        Me.lblCantidad.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(143, 20)
        Me.lblCantidad.TabIndex = 3
        Me.lblCantidad.Text = "Cantidad números:"
        '
        'txtInicio
        '
        Me.txtInicio.Location = New System.Drawing.Point(386, 126)
        Me.txtInicio.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtInicio.Name = "txtInicio"
        Me.txtInicio.Size = New System.Drawing.Size(148, 26)
        Me.txtInicio.TabIndex = 2
        '
        'lblInicio
        '
        Me.lblInicio.AutoSize = True
        Me.lblInicio.Location = New System.Drawing.Point(382, 101)
        Me.lblInicio.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblInicio.Name = "lblInicio"
        Me.lblInicio.Size = New System.Drawing.Size(110, 20)
        Me.lblInicio.TabIndex = 2
        Me.lblInicio.Text = "Número Inicio:"
        '
        'txtNombre
        '
        Me.txtNombre.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNombre.Location = New System.Drawing.Point(792, 27)
        Me.txtNombre.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(96, 26)
        Me.txtNombre.TabIndex = 99
        Me.txtNombre.TabStop = False
        Me.txtNombre.Visible = False
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Location = New System.Drawing.Point(236, 33)
        Me.lblNombre.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(193, 20)
        Me.lblNombre.TabIndex = 100
        Me.lblNombre.Text = "Nombre (Ej: Oficios 2026):"
        Me.lblNombre.Visible = False
        '
        'cmbOficina
        '
        Me.cmbOficina.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOficina.FormattingEnabled = True
        Me.cmbOficina.Location = New System.Drawing.Point(240, 58)
        Me.cmbOficina.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmbOficina.Name = "cmbOficina"
        Me.cmbOficina.Size = New System.Drawing.Size(648, 28)
        Me.cmbOficina.TabIndex = 1
        '
        'lblOficina
        '
        Me.lblOficina.AutoSize = True
        Me.lblOficina.Location = New System.Drawing.Point(236, 33)
        Me.lblOficina.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOficina.Name = "lblOficina"
        Me.lblOficina.Size = New System.Drawing.Size(62, 20)
        Me.lblOficina.TabIndex = 1
        Me.lblOficina.Text = "Oficina:"
        '
        'cmbTipo
        '
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(30, 58)
        Me.cmbTipo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(178, 28)
        Me.cmbTipo.TabIndex = 0
        '
        'lblTipo
        '
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(26, 33)
        Me.lblTipo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(43, 20)
        Me.lblTipo.TabIndex = 0
        Me.lblTipo.Text = "Tipo:"
        '
        'lblStockInfo
        '
        Me.lblStockInfo.AutoSize = True
        Me.lblStockInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStockInfo.ForeColor = System.Drawing.Color.DimGray
        Me.lblStockInfo.Location = New System.Drawing.Point(30, 165)
        Me.lblStockInfo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStockInfo.Name = "lblStockInfo"
        Me.lblStockInfo.Size = New System.Drawing.Size(85, 20)
        Me.lblStockInfo.TabIndex = 101
        Me.lblStockInfo.Text = "Cupo: ---"
        '
        'btnCupos
        '
        Me.btnCupos.Location = New System.Drawing.Point(646, 160)
        Me.btnCupos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCupos.Name = "btnCupos"
        Me.btnCupos.Size = New System.Drawing.Size(242, 35)
        Me.btnCupos.TabIndex = 102
        Me.btnCupos.Text = "ADMINISTRAR CUPOS"
        Me.btnCupos.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(746, 269)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(180, 54)
        Me.btnCancelar.TabIndex = 11
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SteelBlue
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(564, 269)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(180, 54)
        Me.btnGuardar.TabIndex = 10
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'dgvRangos
        '
        Me.dgvRangos.AllowUserToAddRows = False
        Me.dgvRangos.AllowUserToDeleteRows = False
        Me.dgvRangos.AllowUserToResizeColumns = False
        Me.dgvRangos.AllowUserToResizeRows = False
        Me.dgvRangos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvRangos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvRangos.Location = New System.Drawing.Point(18, 338)
        Me.dgvRangos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvRangos.MultiSelect = False
        Me.dgvRangos.Name = "dgvRangos"
        Me.dgvRangos.ReadOnly = True
        Me.dgvRangos.RowHeadersWidth = 62
        Me.dgvRangos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvRangos.Size = New System.Drawing.Size(908, 222)
        Me.dgvRangos.TabIndex = 1
        '
        'btnNuevo
        '
        Me.btnNuevo.Location = New System.Drawing.Point(18, 269)
        Me.btnNuevo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Size = New System.Drawing.Size(180, 54)
        Me.btnNuevo.TabIndex = 2
        Me.btnNuevo.Text = "+ NUEVO RANGO"
        Me.btnNuevo.UseVisualStyleBackColor = True
        '
        'btnEditar
        '
        Me.btnEditar.Location = New System.Drawing.Point(200, 269)
        Me.btnEditar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(180, 54)
        Me.btnEditar.TabIndex = 3
        Me.btnEditar.Text = "EDITAR / VER"
        Me.btnEditar.UseVisualStyleBackColor = True
        '
        'btnEliminar
        '
        Me.btnEliminar.BackColor = System.Drawing.Color.IndianRed
        Me.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEliminar.ForeColor = System.Drawing.Color.White
        Me.btnEliminar.Location = New System.Drawing.Point(382, 269)
        Me.btnEliminar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(180, 54)
        Me.btnEliminar.TabIndex = 4
        Me.btnEliminar.Text = "ELIMINAR"
        Me.btnEliminar.UseVisualStyleBackColor = False
        '
        'frmGestionRangos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(939, 576)
        Me.Controls.Add(Me.btnEliminar)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnEditar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.btnNuevo)
        Me.Controls.Add(Me.dgvRangos)
        Me.Controls.Add(Me.pnlEditor)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmGestionRangos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de Numeración y Rangos"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlEditor.ResumeLayout(False)
        Me.pnlEditor.PerformLayout()
        CType(Me.numAnio, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvRangos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlEditor As System.Windows.Forms.GroupBox
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents lblNombre As System.Windows.Forms.Label
    Friend WithEvents cmbOficina As System.Windows.Forms.ComboBox
    Friend WithEvents lblOficina As System.Windows.Forms.Label
    Friend WithEvents cmbTipo As System.Windows.Forms.ComboBox
    Friend WithEvents lblTipo As System.Windows.Forms.Label
    Friend WithEvents txtUltimo As System.Windows.Forms.TextBox
    Friend WithEvents lblUltimo As System.Windows.Forms.Label
    Friend WithEvents txtFin As System.Windows.Forms.TextBox
    Friend WithEvents lblFin As System.Windows.Forms.Label
    Friend WithEvents txtCantidad As System.Windows.Forms.TextBox
    Friend WithEvents lblCantidad As System.Windows.Forms.Label
    Friend WithEvents numAnio As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblAnio As System.Windows.Forms.Label
    Friend WithEvents txtInicio As System.Windows.Forms.TextBox
    Friend WithEvents lblInicio As System.Windows.Forms.Label
    Friend WithEvents chkActivo As System.Windows.Forms.CheckBox
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents dgvRangos As System.Windows.Forms.DataGridView
    Friend WithEvents btnNuevo As System.Windows.Forms.Button
    Friend WithEvents btnEditar As System.Windows.Forms.Button
    Friend WithEvents btnEliminar As System.Windows.Forms.Button
    Friend WithEvents lblStockInfo As System.Windows.Forms.Label
    Friend WithEvents btnCupos As System.Windows.Forms.Button
End Class
