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
        Me.pnlEditor.Location = New System.Drawing.Point(12, 12)
        Me.pnlEditor.Name = "pnlEditor"
        Me.pnlEditor.Size = New System.Drawing.Size(760, 150)
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
        Me.chkActivo.Location = New System.Drawing.Point(550, 30)
        Me.chkActivo.Name = "chkActivo"
        Me.chkActivo.Size = New System.Drawing.Size(134, 17)
        Me.chkActivo.TabIndex = 12
        Me.chkActivo.Text = "RANGO VIGENTE"
        Me.chkActivo.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(650, 100)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(90, 35)
        Me.btnCancelar.TabIndex = 11
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SteelBlue
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(540, 100)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(100, 35)
        Me.btnGuardar.TabIndex = 10
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'txtUltimo
        '
        Me.txtUltimo.Location = New System.Drawing.Point(400, 108)
        Me.txtUltimo.Name = "txtUltimo"
        Me.txtUltimo.Size = New System.Drawing.Size(100, 20)
        Me.txtUltimo.TabIndex = 9
        Me.txtUltimo.Text = "0"
        '
        'lblUltimo
        '
        Me.lblUltimo.AutoSize = True
        Me.lblUltimo.Location = New System.Drawing.Point(397, 92)
        Me.lblUltimo.Name = "lblUltimo"
        Me.lblUltimo.Size = New System.Drawing.Size(83, 13)
        Me.lblUltimo.TabIndex = 8
        Me.lblUltimo.Text = "Último Usado:"
        '
        'txtFin
        '
        Me.txtFin.Location = New System.Drawing.Point(280, 108)
        Me.txtFin.Name = "txtFin"
        Me.txtFin.Size = New System.Drawing.Size(100, 20)
        Me.txtFin.TabIndex = 7
        '
        'lblFin
        '
        Me.lblFin.AutoSize = True
        Me.lblFin.Location = New System.Drawing.Point(277, 92)
        Me.lblFin.Name = "lblFin"
        Me.lblFin.Size = New System.Drawing.Size(65, 13)
        Me.lblFin.TabIndex = 6
        Me.lblFin.Text = "Número Fin:"
        '
        'txtInicio
        '
        Me.txtInicio.Location = New System.Drawing.Point(160, 108)
        Me.txtInicio.Name = "txtInicio"
        Me.txtInicio.Size = New System.Drawing.Size(100, 20)
        Me.txtInicio.TabIndex = 5
        '
        'lblInicio
        '
        Me.lblInicio.AutoSize = True
        Me.lblInicio.Location = New System.Drawing.Point(157, 92)
        Me.lblInicio.Name = "lblInicio"
        Me.lblInicio.Size = New System.Drawing.Size(76, 13)
        Me.lblInicio.TabIndex = 4
        Me.lblInicio.Text = "Número Inicio:"
        '
        'txtNombre
        '
        Me.txtNombre.Location = New System.Drawing.Point(160, 50)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(340, 20)
        Me.txtNombre.TabIndex = 3
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Location = New System.Drawing.Point(157, 34)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(130, 13)
        Me.lblNombre.TabIndex = 2
        Me.lblNombre.Text = "Nombre (Ej: Oficios 2026):"
        '
        'cmbTipo
        '
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(20, 50)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(120, 21)
        Me.cmbTipo.TabIndex = 1
        '
        'lblTipo
        '
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(17, 34)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(31, 13)
        Me.lblTipo.TabIndex = 0
        Me.lblTipo.Text = "Tipo:"
        '
        'dgvRangos
        '
        Me.dgvRangos.AllowUserToAddRows = False
        Me.dgvRangos.AllowUserToDeleteRows = False
        Me.dgvRangos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvRangos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvRangos.Location = New System.Drawing.Point(12, 220)
        Me.dgvRangos.MultiSelect = False
        Me.dgvRangos.Name = "dgvRangos"
        Me.dgvRangos.ReadOnly = True
        Me.dgvRangos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvRangos.Size = New System.Drawing.Size(760, 230)
        Me.dgvRangos.TabIndex = 1
        '
        'btnNuevo
        '
        Me.btnNuevo.Location = New System.Drawing.Point(12, 175)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Size = New System.Drawing.Size(120, 35)
        Me.btnNuevo.TabIndex = 2
        Me.btnNuevo.Text = "+ NUEVO RANGO"
        Me.btnNuevo.UseVisualStyleBackColor = True
        '
        'btnEditar
        '
        Me.btnEditar.Location = New System.Drawing.Point(140, 175)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(120, 35)
        Me.btnEditar.TabIndex = 3
        Me.btnEditar.Text = "EDITAR / VER"
        Me.btnEditar.UseVisualStyleBackColor = True
        '
        'frmGestionRangos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 461)
        Me.Controls.Add(Me.btnEditar)
        Me.Controls.Add(Me.btnNuevo)
        Me.Controls.Add(Me.dgvRangos)
        Me.Controls.Add(Me.pnlEditor)
        Me.Name = "frmGestionRangos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de Numeración y Rangos"
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