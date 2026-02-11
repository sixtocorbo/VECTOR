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
        Me.lblContador = New System.Windows.Forms.Label()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.btnDarPase = New System.Windows.Forms.Button()
        Me.btnRenovacionesArt120 = New System.Windows.Forms.Button()
        Me.PanelInferior = New System.Windows.Forms.Panel()
        Me.btnDesvincular = New System.Windows.Forms.Button()
        Me.btnEditar = New System.Windows.Forms.Button()
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
        Me.PanelSuperior.Controls.Add(Me.lblContador)
        Me.PanelSuperior.Controls.Add(Me.txtBuscar)
        Me.PanelSuperior.Controls.Add(Me.btnDarPase)
        Me.PanelSuperior.Controls.Add(Me.btnRenovacionesArt120)
        Me.PanelSuperior.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelSuperior.Location = New System.Drawing.Point(0, 0)
        Me.PanelSuperior.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PanelSuperior.Name = "PanelSuperior"
        Me.PanelSuperior.Size = New System.Drawing.Size(1440, 146)
        Me.PanelSuperior.TabIndex = 0
        '
        'chkVerDerivados
        '
        Me.chkVerDerivados.AutoSize = True
        Me.chkVerDerivados.Location = New System.Drawing.Point(13, 14)
        Me.chkVerDerivados.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkVerDerivados.Name = "chkVerDerivados"
        Me.chkVerDerivados.Size = New System.Drawing.Size(195, 24)
        Me.chkVerDerivados.TabIndex = 5
        Me.chkVerDerivados.Text = "Todos los documentos"
        Me.chkVerDerivados.UseVisualStyleBackColor = True
        '
        'btnNuevoIngreso
        '
        Me.btnNuevoIngreso.BackColor = System.Drawing.Color.SeaGreen
        Me.btnNuevoIngreso.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNuevoIngreso.ForeColor = System.Drawing.Color.White
        Me.btnNuevoIngreso.Location = New System.Drawing.Point(349, 14)
        Me.btnNuevoIngreso.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnNuevoIngreso.Name = "btnNuevoIngreso"
        Me.btnNuevoIngreso.Size = New System.Drawing.Size(270, 71)
        Me.btnNuevoIngreso.TabIndex = 3
        Me.btnNuevoIngreso.Text = "+ NUEVO INGRESO"
        Me.btnNuevoIngreso.UseVisualStyleBackColor = False
        '
        'lblContador
        '
        Me.lblContador.AutoSize = True
        Me.lblContador.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContador.ForeColor = System.Drawing.Color.Green
        Me.lblContador.Location = New System.Drawing.Point(7, 97)
        Me.lblContador.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblContador.Name = "lblContador"
        Me.lblContador.Size = New System.Drawing.Size(179, 32)
        Me.lblContador.TabIndex = 6
        Me.lblContador.Text = "Expedientes: 0"
        '
        'txtBuscar
        '
        Me.txtBuscar.Location = New System.Drawing.Point(13, 59)
        Me.txtBuscar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(311, 26)
        Me.txtBuscar.TabIndex = 1
        '
        'btnDarPase
        '
        Me.btnDarPase.BackColor = System.Drawing.Color.ForestGreen
        Me.btnDarPase.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDarPase.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDarPase.ForeColor = System.Drawing.Color.White
        Me.btnDarPase.Location = New System.Drawing.Point(634, 14)
        Me.btnDarPase.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnDarPase.Name = "btnDarPase"
        Me.btnDarPase.Size = New System.Drawing.Size(270, 71)
        Me.btnDarPase.TabIndex = 1
        Me.btnDarPase.Text = "DAR PASE"
        Me.btnDarPase.UseVisualStyleBackColor = False
        '
        'btnRenovacionesArt120
        '
        Me.btnRenovacionesArt120.BackColor = System.Drawing.Color.MediumPurple
        Me.btnRenovacionesArt120.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRenovacionesArt120.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRenovacionesArt120.ForeColor = System.Drawing.Color.White
        Me.btnRenovacionesArt120.Location = New System.Drawing.Point(919, 14)
        Me.btnRenovacionesArt120.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnRenovacionesArt120.Name = "btnRenovacionesArt120"
        Me.btnRenovacionesArt120.Size = New System.Drawing.Size(330, 71)
        Me.btnRenovacionesArt120.TabIndex = 7
        Me.btnRenovacionesArt120.Text = "RENOVACIONES ART.120"
        Me.btnRenovacionesArt120.UseVisualStyleBackColor = False
        '
        'PanelInferior
        '
        Me.PanelInferior.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelInferior.Controls.Add(Me.btnDesvincular)
        Me.PanelInferior.Controls.Add(Me.btnEditar)
        Me.PanelInferior.Controls.Add(Me.btnEliminar)
        Me.PanelInferior.Controls.Add(Me.btnVincular)
        Me.PanelInferior.Controls.Add(Me.btnHistorial)
        Me.PanelInferior.Controls.Add(Me.btnRefrescar)
        Me.PanelInferior.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelInferior.Location = New System.Drawing.Point(0, 393)
        Me.PanelInferior.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PanelInferior.Name = "PanelInferior"
        Me.PanelInferior.Size = New System.Drawing.Size(1440, 131)
        Me.PanelInferior.TabIndex = 1
        '
        'btnDesvincular
        '
        Me.btnDesvincular.BackColor = System.Drawing.Color.SteelBlue
        Me.btnDesvincular.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDesvincular.ForeColor = System.Drawing.Color.White
        Me.btnDesvincular.Location = New System.Drawing.Point(238, 31)
        Me.btnDesvincular.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnDesvincular.Name = "btnDesvincular"
        Me.btnDesvincular.Size = New System.Drawing.Size(180, 80)
        Me.btnDesvincular.TabIndex = 8
        Me.btnDesvincular.Text = "🔗 DESVINCULAR"
        Me.btnDesvincular.UseVisualStyleBackColor = False
        '
        'btnEditar
        '
        Me.btnEditar.BackColor = System.Drawing.Color.Goldenrod
        Me.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEditar.ForeColor = System.Drawing.Color.White
        Me.btnEditar.Location = New System.Drawing.Point(449, 31)
        Me.btnEditar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(180, 80)
        Me.btnEditar.TabIndex = 7
        Me.btnEditar.Text = "✏️ EDITAR"
        Me.btnEditar.UseVisualStyleBackColor = False
        '
        'btnEliminar
        '
        Me.btnEliminar.BackColor = System.Drawing.Color.IndianRed
        Me.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEliminar.ForeColor = System.Drawing.Color.White
        Me.btnEliminar.Location = New System.Drawing.Point(660, 31)
        Me.btnEliminar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(180, 80)
        Me.btnEliminar.TabIndex = 5
        Me.btnEliminar.Text = "🗑️ ELIMINAR"
        Me.btnEliminar.UseVisualStyleBackColor = False
        '
        'btnVincular
        '
        Me.btnVincular.BackColor = System.Drawing.Color.SteelBlue
        Me.btnVincular.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnVincular.ForeColor = System.Drawing.Color.White
        Me.btnVincular.Location = New System.Drawing.Point(871, 31)
        Me.btnVincular.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnVincular.Name = "btnVincular"
        Me.btnVincular.Size = New System.Drawing.Size(180, 80)
        Me.btnVincular.TabIndex = 4
        Me.btnVincular.Text = "🔗 VINCULAR"
        Me.btnVincular.UseVisualStyleBackColor = False
        '
        'btnHistorial
        '
        Me.btnHistorial.BackColor = System.Drawing.Color.SteelBlue
        Me.btnHistorial.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnHistorial.ForeColor = System.Drawing.Color.White
        Me.btnHistorial.Location = New System.Drawing.Point(1082, 31)
        Me.btnHistorial.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnHistorial.Name = "btnHistorial"
        Me.btnHistorial.Size = New System.Drawing.Size(180, 80)
        Me.btnHistorial.TabIndex = 3
        Me.btnHistorial.Text = "Ver Historial"
        Me.btnHistorial.UseVisualStyleBackColor = False
        '
        'btnRefrescar
        '
        Me.btnRefrescar.Location = New System.Drawing.Point(27, 31)
        Me.btnRefrescar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnRefrescar.Name = "btnRefrescar"
        Me.btnRefrescar.Size = New System.Drawing.Size(180, 80)
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
        Me.dgvPendientes.BackgroundColor = System.Drawing.Color.WhiteSmoke
        Me.dgvPendientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPendientes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvPendientes.Location = New System.Drawing.Point(0, 146)
        Me.dgvPendientes.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvPendientes.MultiSelect = False
        Me.dgvPendientes.Name = "dgvPendientes"
        Me.dgvPendientes.ReadOnly = True
        Me.dgvPendientes.RowHeadersWidth = 62
        Me.dgvPendientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPendientes.Size = New System.Drawing.Size(1440, 247)
        Me.dgvPendientes.TabIndex = 2
        '
        'frmBandeja
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1440, 524)
        Me.Controls.Add(Me.dgvPendientes)
        Me.Controls.Add(Me.PanelInferior)
        Me.Controls.Add(Me.PanelSuperior)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmBandeja"
        Me.Text = "VECTOR - Bandeja"
        Me.PanelSuperior.ResumeLayout(False)
        Me.PanelSuperior.PerformLayout()
        Me.PanelInferior.ResumeLayout(False)
        CType(Me.dgvPendientes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelSuperior As System.Windows.Forms.Panel
    Friend WithEvents txtBuscar As System.Windows.Forms.TextBox
    Friend WithEvents PanelInferior As System.Windows.Forms.Panel
    Friend WithEvents btnDarPase As System.Windows.Forms.Button
    Friend WithEvents btnRefrescar As System.Windows.Forms.Button
    Friend WithEvents dgvPendientes As System.Windows.Forms.DataGridView
    Friend WithEvents btnNuevoIngreso As System.Windows.Forms.Button
    Friend WithEvents btnEditar As System.Windows.Forms.Button
    Friend WithEvents btnHistorial As System.Windows.Forms.Button
    Friend WithEvents btnVincular As System.Windows.Forms.Button
    Friend WithEvents btnEliminar As System.Windows.Forms.Button
    Friend WithEvents chkVerDerivados As System.Windows.Forms.CheckBox
    Friend WithEvents lblContador As System.Windows.Forms.Label
    Friend WithEvents btnDesvincular As System.Windows.Forms.Button
    Friend WithEvents btnRenovacionesArt120 As System.Windows.Forms.Button
End Class