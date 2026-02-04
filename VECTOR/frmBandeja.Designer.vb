<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmBandeja
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
        Me.PanelSuperior = New System.Windows.Forms.Panel()
        Me.chkVerDerivados = New System.Windows.Forms.CheckBox()
        Me.btnNuevoIngreso = New System.Windows.Forms.Button()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnDarPase = New System.Windows.Forms.Button()
        Me.PanelInferior = New System.Windows.Forms.Panel()
        Me.btnEditar = New System.Windows.Forms.Button()
        Me.lblContador = New System.Windows.Forms.Label()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.btnVincular = New System.Windows.Forms.Button()
        Me.btnHistorial = New System.Windows.Forms.Button()
        Me.btnRefrescar = New System.Windows.Forms.Button()
        Me.dgvPendientes = New System.Windows.Forms.DataGridView()
        Me.PanelSuperior.SuspendLayout()
        Me.PanelInferior.SuspendLayout()
        CType(Me.dgvPendientes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelSuperior
        '
        Me.PanelSuperior.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelSuperior.Controls.Add(Me.chkVerDerivados)
        Me.PanelSuperior.Controls.Add(Me.btnNuevoIngreso)
        Me.PanelSuperior.Controls.Add(Me.txtBuscar)
        Me.PanelSuperior.Controls.Add(Me.Label1)
        Me.PanelSuperior.Controls.Add(Me.btnDarPase)
        Me.PanelSuperior.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelSuperior.Location = New System.Drawing.Point(0, 0)
        Me.PanelSuperior.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PanelSuperior.Name = "PanelSuperior"
        Me.PanelSuperior.Size = New System.Drawing.Size(1800, 108)
        Me.PanelSuperior.TabIndex = 0
        '
        'chkVerDerivados
        '
        Me.chkVerDerivados.AutoSize = True
        Me.chkVerDerivados.Location = New System.Drawing.Point(528, 38)
        Me.chkVerDerivados.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkVerDerivados.Name = "chkVerDerivados"
        Me.chkVerDerivados.Size = New System.Drawing.Size(195, 24)
        Me.chkVerDerivados.TabIndex = 5
        Me.chkVerDerivados.Text = "Todos los documentos"
        Me.chkVerDerivados.UseVisualStyleBackColor = True
        '
        'btnNuevoIngreso
        '
        Me.btnNuevoIngreso.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevoIngreso.BackColor = System.Drawing.Color.SeaGreen
        Me.btnNuevoIngreso.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNuevoIngreso.ForeColor = System.Drawing.Color.White
        Me.btnNuevoIngreso.Location = New System.Drawing.Point(832, 38)
        Me.btnNuevoIngreso.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnNuevoIngreso.Name = "btnNuevoIngreso"
        Me.btnNuevoIngreso.Size = New System.Drawing.Size(276, 46)
        Me.btnNuevoIngreso.TabIndex = 3
        Me.btnNuevoIngreso.Text = "+ NUEVO INGRESO"
        Me.btnNuevoIngreso.UseVisualStyleBackColor = False
        '
        'txtBuscar
        '
        Me.txtBuscar.Location = New System.Drawing.Point(93, 35)
        Me.txtBuscar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(412, 26)
        Me.txtBuscar.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 42)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Buscar:"
        '
        'btnDarPase
        '
        Me.btnDarPase.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDarPase.BackColor = System.Drawing.Color.ForestGreen
        Me.btnDarPase.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDarPase.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDarPase.ForeColor = System.Drawing.Color.White
        Me.btnDarPase.Location = New System.Drawing.Point(1116, 38)
        Me.btnDarPase.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnDarPase.Name = "btnDarPase"
        Me.btnDarPase.Size = New System.Drawing.Size(276, 46)
        Me.btnDarPase.TabIndex = 1
        Me.btnDarPase.Text = "DAR PASE"
        Me.btnDarPase.UseVisualStyleBackColor = False
        '
        'PanelInferior
        '
        Me.PanelInferior.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelInferior.Controls.Add(Me.btnEditar)
        Me.PanelInferior.Controls.Add(Me.lblContador)
        Me.PanelInferior.Controls.Add(Me.btnEliminar)
        Me.PanelInferior.Controls.Add(Me.btnVincular)
        Me.PanelInferior.Controls.Add(Me.btnHistorial)
        Me.PanelInferior.Controls.Add(Me.btnRefrescar)
        Me.PanelInferior.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelInferior.Location = New System.Drawing.Point(0, 771)
        Me.PanelInferior.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PanelInferior.Name = "PanelInferior"
        Me.PanelInferior.Size = New System.Drawing.Size(1800, 92)
        Me.PanelInferior.TabIndex = 1
        '
        'btnEditar
        '
        Me.btnEditar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditar.BackColor = System.Drawing.Color.Goldenrod
        Me.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEditar.ForeColor = System.Drawing.Color.White
        Me.btnEditar.Location = New System.Drawing.Point(504, 18)
        Me.btnEditar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(162, 55)
        Me.btnEditar.TabIndex = 7
        Me.btnEditar.Text = "✏️ EDITAR"
        Me.btnEditar.UseVisualStyleBackColor = False
        '
        'lblContador
        '
        Me.lblContador.AutoSize = True
        Me.lblContador.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContador.Location = New System.Drawing.Point(158, 35)
        Me.lblContador.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblContador.Name = "lblContador"
        Me.lblContador.Size = New System.Drawing.Size(104, 23)
        Me.lblContador.TabIndex = 6
        Me.lblContador.Text = "Registros: 0"
        '
        'btnEliminar
        '
        Me.btnEliminar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminar.BackColor = System.Drawing.Color.IndianRed
        Me.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEliminar.ForeColor = System.Drawing.Color.White
        Me.btnEliminar.Location = New System.Drawing.Point(675, 18)
        Me.btnEliminar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(162, 55)
        Me.btnEliminar.TabIndex = 5
        Me.btnEliminar.Text = "🗑️ ELIMINAR"
        Me.btnEliminar.UseVisualStyleBackColor = False
        '
        'btnVincular
        '
        Me.btnVincular.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnVincular.BackColor = System.Drawing.Color.SteelBlue
        Me.btnVincular.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnVincular.ForeColor = System.Drawing.Color.White
        Me.btnVincular.Location = New System.Drawing.Point(846, 18)
        Me.btnVincular.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnVincular.Name = "btnVincular"
        Me.btnVincular.Size = New System.Drawing.Size(162, 55)
        Me.btnVincular.TabIndex = 4
        Me.btnVincular.Text = "🔗 VINCULAR"
        Me.btnVincular.UseVisualStyleBackColor = False
        '
        'btnHistorial
        '
        Me.btnHistorial.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnHistorial.BackColor = System.Drawing.Color.SteelBlue
        Me.btnHistorial.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnHistorial.ForeColor = System.Drawing.Color.White
        Me.btnHistorial.Location = New System.Drawing.Point(1017, 18)
        Me.btnHistorial.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnHistorial.Name = "btnHistorial"
        Me.btnHistorial.Size = New System.Drawing.Size(162, 55)
        Me.btnHistorial.TabIndex = 3
        Me.btnHistorial.Text = "Ver Historial"
        Me.btnHistorial.UseVisualStyleBackColor = False
        '
        'btnRefrescar
        '
        Me.btnRefrescar.Location = New System.Drawing.Point(18, 20)
        Me.btnRefrescar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnRefrescar.Name = "btnRefrescar"
        Me.btnRefrescar.Size = New System.Drawing.Size(130, 52)
        Me.btnRefrescar.TabIndex = 0
        Me.btnRefrescar.Text = "Actualizar"
        Me.btnRefrescar.UseVisualStyleBackColor = True
        '
        'dgvPendientes
        '
        Me.dgvPendientes.AllowUserToAddRows = False
        Me.dgvPendientes.AllowUserToDeleteRows = False
        Me.dgvPendientes.AllowUserToResizeColumns = False
        Me.dgvPendientes.AllowUserToResizeRows = False
        Me.dgvPendientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPendientes.BackgroundColor = System.Drawing.Color.WhiteSmoke
        Me.dgvPendientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPendientes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvPendientes.Location = New System.Drawing.Point(0, 108)
        Me.dgvPendientes.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvPendientes.MultiSelect = False
        Me.dgvPendientes.Name = "dgvPendientes"
        Me.dgvPendientes.ReadOnly = True
        Me.dgvPendientes.RowHeadersVisible = False
        Me.dgvPendientes.RowHeadersWidth = 62
        Me.dgvPendientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPendientes.Size = New System.Drawing.Size(1800, 663)
        Me.dgvPendientes.TabIndex = 2
        '
        'frmBandeja
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1800, 863)
        Me.Controls.Add(Me.dgvPendientes)
        Me.Controls.Add(Me.PanelInferior)
        Me.Controls.Add(Me.PanelSuperior)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmBandeja"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "VECTOR - Sistema de Gestión"
        Me.PanelSuperior.ResumeLayout(False)
        Me.PanelSuperior.PerformLayout()
        Me.PanelInferior.ResumeLayout(False)
        Me.PanelInferior.PerformLayout()
        CType(Me.dgvPendientes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelSuperior As Panel
    Friend WithEvents txtBuscar As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents PanelInferior As Panel
    Friend WithEvents btnDarPase As Button
    Friend WithEvents btnRefrescar As Button
    Friend WithEvents dgvPendientes As DataGridView
    Friend WithEvents btnNuevoIngreso As Button
    Friend WithEvents btnEditar As Button
    Friend WithEvents btnHistorial As Button
    Friend WithEvents btnVincular As Button
    Friend WithEvents btnEliminar As Button
    Friend WithEvents chkVerDerivados As CheckBox
    Friend WithEvents lblContador As Label
End Class