<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUnificarOficinas
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: El Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.dgvOficinas = New System.Windows.Forms.DataGridView()
        Me.lblFiltro = New System.Windows.Forms.Label()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.lblDestino = New System.Windows.Forms.Label()
        Me.txtNombreOficial = New System.Windows.Forms.TextBox()
        Me.btnUnificar = New System.Windows.Forms.Button()
        Me.PanelSuperior = New System.Windows.Forms.Panel()
        Me.PanelInferior = New System.Windows.Forms.Panel()
        CType(Me.dgvOficinas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelSuperior.SuspendLayout()
        Me.PanelInferior.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvOficinas
        '
        Me.dgvOficinas.AllowUserToAddRows = False
        Me.dgvOficinas.AllowUserToDeleteRows = False
        Me.dgvOficinas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvOficinas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvOficinas.Location = New System.Drawing.Point(12, 60)
        Me.dgvOficinas.Name = "dgvOficinas"
        Me.dgvOficinas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvOficinas.Size = New System.Drawing.Size(760, 380)
        Me.dgvOficinas.TabIndex = 2
        '
        'lblFiltro
        '
        Me.lblFiltro.AutoSize = True
        Me.lblFiltro.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFiltro.Location = New System.Drawing.Point(9, 15)
        Me.lblFiltro.Name = "lblFiltro"
        Me.lblFiltro.Size = New System.Drawing.Size(203, 16)
        Me.lblFiltro.TabIndex = 0
        Me.lblFiltro.Text = "1. Buscar Oficina (Ej: 'SDO'):"
        '
        'txtBuscar
        '
        Me.txtBuscar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBuscar.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBuscar.Location = New System.Drawing.Point(218, 10)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(539, 24)
        Me.txtBuscar.TabIndex = 1
        '
        'lblDestino
        '
        Me.lblDestino.AutoSize = True
        Me.lblDestino.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDestino.Location = New System.Drawing.Point(9, 15)
        Me.lblDestino.Name = "lblDestino"
        Me.lblDestino.Size = New System.Drawing.Size(271, 16)
        Me.lblDestino.TabIndex = 3
        Me.lblDestino.Text = "2. Nombre CORRECTO (Destino Final):"
        '
        'txtNombreOficial
        '
        Me.txtNombreOficial.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreOficial.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombreOficial.Location = New System.Drawing.Point(12, 34)
        Me.txtNombreOficial.Name = "txtNombreOficial"
        Me.txtNombreOficial.Size = New System.Drawing.Size(580, 24)
        Me.txtNombreOficial.TabIndex = 4
        '
        'btnUnificar
        '
        Me.btnUnificar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUnificar.BackColor = System.Drawing.Color.ForestGreen
        Me.btnUnificar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUnificar.ForeColor = System.Drawing.Color.White
        Me.btnUnificar.Location = New System.Drawing.Point(608, 15)
        Me.btnUnificar.Name = "btnUnificar"
        Me.btnUnificar.Size = New System.Drawing.Size(149, 45)
        Me.btnUnificar.TabIndex = 5
        Me.btnUnificar.Text = "UNIFICAR SELECCIONADOS"
        Me.btnUnificar.UseVisualStyleBackColor = False
        '
        'PanelSuperior
        '
        Me.PanelSuperior.Controls.Add(Me.lblFiltro)
        Me.PanelSuperior.Controls.Add(Me.txtBuscar)
        Me.PanelSuperior.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelSuperior.Location = New System.Drawing.Point(0, 0)
        Me.PanelSuperior.Name = "PanelSuperior"
        Me.PanelSuperior.Size = New System.Drawing.Size(784, 54)
        Me.PanelSuperior.TabIndex = 6
        '
        'PanelInferior
        '
        Me.PanelInferior.Controls.Add(Me.lblDestino)
        Me.PanelInferior.Controls.Add(Me.btnUnificar)
        Me.PanelInferior.Controls.Add(Me.txtNombreOficial)
        Me.PanelInferior.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelInferior.Location = New System.Drawing.Point(0, 446)
        Me.PanelInferior.Name = "PanelInferior"
        Me.PanelInferior.Size = New System.Drawing.Size(784, 75)
        Me.PanelInferior.TabIndex = 7
        '
        'frmUnificarOficinas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 521)
        Me.Controls.Add(Me.dgvOficinas)
        Me.Controls.Add(Me.PanelInferior)
        Me.Controls.Add(Me.PanelSuperior)
        Me.Name = "frmUnificarOficinas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Herramienta de Unificación de Oficinas (VECTOR)"
        CType(Me.dgvOficinas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelSuperior.ResumeLayout(False)
        Me.PanelSuperior.PerformLayout()
        Me.PanelInferior.ResumeLayout(False)
        Me.PanelInferior.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvOficinas As DataGridView
    Friend WithEvents lblFiltro As Label
    Friend WithEvents txtBuscar As TextBox
    Friend WithEvents lblDestino As Label
    Friend WithEvents txtNombreOficial As TextBox
    Friend WithEvents btnUnificar As Button
    Friend WithEvents PanelSuperior As Panel
    Friend WithEvents PanelInferior As Panel
End Class