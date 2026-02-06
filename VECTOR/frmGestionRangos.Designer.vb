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
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.txtUltimo = New System.Windows.Forms.TextBox()
        Me.lblUltimo = New System.Windows.Forms.Label()
        Me.txtFin = New System.Windows.Forms.TextBox()
        Me.lblFin = New System.Windows.Forms.Label()
        Me.txtInicio = New System.Windows.Forms.TextBox()
        Me.lblInicio = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.cmbTipo = New System.Windows.Forms.ComboBox()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.dgvRangos = New System.Windows.Forms.DataGridView()
        Me.btnNuevo = New System.Windows.Forms.Button()
        Me.btnEditar = New System.Windows.Forms.Button()
        Me.pnlEditor.SuspendLayout()
        CType(Me.dgvRangos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlEditor
        '
        Me.pnlEditor.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlEditor.Controls.Add(Me.chkActivo)
        Me.pnlEditor.Controls.Add(Me.btnCancelar)
        Me.pnlEditor.Controls.Add(Me.btnGuardar)
        Me.pnlEditor.Controls.Add(Me.txtUltimo)
        Me.pnlEditor.Controls.Add(Me.lblUltimo)
        Me.pnlEditor.Controls.Add(Me.txtFin)
        Me.pnlEditor.Controls.Add(Me.lblFin)
        Me.pnlEditor.Controls.Add(Me.txtInicio)
        Me.pnlEditor.Controls.Add(Me.lblInicio)
        Me.pnlEditor.Controls.Add(Me.txtNombre)
        Me.pnlEditor.Controls.Add(Me.lblNombre)
        Me.pnlEditor.Controls.Add(Me.cmbTipo)
        Me.pnlEditor.Controls.Add(Me.lblTipo)
        Me.pnlEditor.Enabled = False
        Me.pnlEditor.Location = New System.Drawing.Point(18, 18)
        Me.pnlEditor.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlEditor.Name = "pnlEditor"
        Me.pnlEditor.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlEditor.Size = New System.Drawing.Size(1140, 231)
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
        Me.chkActivo.Location = New System.Drawing.Point(825, 46)
        Me.chkActivo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkActivo.Name = "chkActivo"
        Me.chkActivo.Size = New System.Drawing.Size(186, 24)
        Me.chkActivo.TabIndex = 12
        Me.chkActivo.Text = "RANGO VIGENTE"
        Me.chkActivo.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(975, 154)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(135, 54)
        Me.btnCancelar.TabIndex = 11
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SteelBlue
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(810, 154)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(150, 54)
        Me.btnGuardar.TabIndex = 10
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'txtUltimo
        '
        Me.txtUltimo.Location = New System.Drawing.Point(600, 166)
        Me.txtUltimo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtUltimo.Name = "txtUltimo"
        Me.txtUltimo.Size = New System.Drawing.Size(148, 26)
        Me.txtUltimo.TabIndex = 9
        Me.txtUltimo.Text = "0"
        '
        'lblUltimo
        '
        Me.lblUltimo.AutoSize = True
        Me.lblUltimo.Location = New System.Drawing.Point(596, 142)
        Me.lblUltimo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblUltimo.Name = "lblUltimo"
        Me.lblUltimo.Size = New System.Drawing.Size(109, 20)
        Me.lblUltimo.TabIndex = 8
        Me.lblUltimo.Text = "Último Usado:"
        '
        'txtFin
        '
        Me.txtFin.Location = New System.Drawing.Point(420, 166)
        Me.txtFin.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtFin.Name = "txtFin"
        Me.txtFin.Size = New System.Drawing.Size(148, 26)
        Me.txtFin.TabIndex = 7
        '
        'lblFin
        '
        Me.lblFin.AutoSize = True
        Me.lblFin.Location = New System.Drawing.Point(416, 142)
        Me.lblFin.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFin.Name = "lblFin"
        Me.lblFin.Size = New System.Drawing.Size(95, 20)
        Me.lblFin.TabIndex = 6
        Me.lblFin.Text = "Número Fin:"
        '
        'txtInicio
        '
        Me.txtInicio.Location = New System.Drawing.Point(240, 166)
        Me.txtInicio.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtInicio.Name = "txtInicio"
        Me.txtInicio.Size = New System.Drawing.Size(148, 26)
        Me.txtInicio.TabIndex = 5
        '
        'lblInicio
        '
        Me.lblInicio.AutoSize = True
        Me.lblInicio.Location = New System.Drawing.Point(236, 142)
        Me.lblInicio.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblInicio.Name = "lblInicio"
        Me.lblInicio.Size = New System.Drawing.Size(110, 20)
        Me.lblInicio.TabIndex = 4
        Me.lblInicio.Text = "Número Inicio:"
        '
        'txtNombre
        '
        Me.txtNombre.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNombre.Location = New System.Drawing.Point(240, 77)
        Me.txtNombre.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(508, 26)
        Me.txtNombre.TabIndex = 3
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Location = New System.Drawing.Point(236, 52)
        Me.lblNombre.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(193, 20)
        Me.lblNombre.TabIndex = 2
        Me.lblNombre.Text = "Nombre (Ej: Oficios 2026):"
        '
        'cmbTipo
        '
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(30, 77)
        Me.cmbTipo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(178, 28)
        Me.cmbTipo.TabIndex = 1
        '
        'lblTipo
        '
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(26, 52)
        Me.lblTipo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(43, 20)
        Me.lblTipo.TabIndex = 0
        Me.lblTipo.Text = "Tipo:"
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
        Me.dgvRangos.Size = New System.Drawing.Size(1140, 354)
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
        Me.btnEditar.Location = New System.Drawing.Point(210, 269)
        Me.btnEditar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(180, 54)
        Me.btnEditar.TabIndex = 3
        Me.btnEditar.Text = "EDITAR / VER"
        Me.btnEditar.UseVisualStyleBackColor = True
        '
        'frmGestionRangos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1176, 709)
        Me.Controls.Add(Me.btnEditar)
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
        CType(Me.dgvRangos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlEditor As System.Windows.Forms.GroupBox
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents lblNombre As System.Windows.Forms.Label
    Friend WithEvents cmbTipo As System.Windows.Forms.ComboBox
    Friend WithEvents lblTipo As System.Windows.Forms.Label
    Friend WithEvents txtUltimo As System.Windows.Forms.TextBox
    Friend WithEvents lblUltimo As System.Windows.Forms.Label
    Friend WithEvents txtFin As System.Windows.Forms.TextBox
    Friend WithEvents lblFin As System.Windows.Forms.Label
    Friend WithEvents txtInicio As System.Windows.Forms.TextBox
    Friend WithEvents lblInicio As System.Windows.Forms.Label
    Friend WithEvents chkActivo As System.Windows.Forms.CheckBox
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents dgvRangos As System.Windows.Forms.DataGridView
    Friend WithEvents btnNuevo As System.Windows.Forms.Button
    Friend WithEvents btnEditar As System.Windows.Forms.Button
End Class