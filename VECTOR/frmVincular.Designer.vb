<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmVincular
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
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.grpDatos = New System.Windows.Forms.GroupBox()
        Me.dgvPadres = New System.Windows.Forms.DataGridView()
        Me.lblHintPadre = New System.Windows.Forms.Label()
        Me.txtBuscarPadre = New System.Windows.Forms.TextBox()
        Me.lblBuscarPadre = New System.Windows.Forms.Label()
        Me.lblDetalleHijo = New System.Windows.Forms.Label()
        Me.txtIdPadre = New System.Windows.Forms.TextBox()
        Me.lblPadre = New System.Windows.Forms.Label()
        Me.txtIdHijo = New System.Windows.Forms.TextBox()
        Me.lblHijo = New System.Windows.Forms.Label()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnConfirmar = New System.Windows.Forms.Button()
        Me.grpDatos.SuspendLayout()
        CType(Me.dgvPadres, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitulo
        '
        Me.lblTitulo.BackColor = System.Drawing.Color.ForestGreen
        Me.lblTitulo.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitulo.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitulo.ForeColor = System.Drawing.Color.White
        Me.lblTitulo.Location = New System.Drawing.Point(0, 0)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.lblTitulo.Size = New System.Drawing.Size(384, 40)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "Vincular Expedientes"
        Me.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpDatos
        '
        Me.grpDatos.Controls.Add(Me.dgvPadres)
        Me.grpDatos.Controls.Add(Me.lblHintPadre)
        Me.grpDatos.Controls.Add(Me.txtBuscarPadre)
        Me.grpDatos.Controls.Add(Me.lblBuscarPadre)
        Me.grpDatos.Controls.Add(Me.lblDetalleHijo)
        Me.grpDatos.Controls.Add(Me.txtIdPadre)
        Me.grpDatos.Controls.Add(Me.lblPadre)
        Me.grpDatos.Controls.Add(Me.txtIdHijo)
        Me.grpDatos.Controls.Add(Me.lblHijo)
        Me.grpDatos.Location = New System.Drawing.Point(12, 53)
        Me.grpDatos.Name = "grpDatos"
        Me.grpDatos.Size = New System.Drawing.Size(944, 494)
        Me.grpDatos.TabIndex = 1
        Me.grpDatos.TabStop = False
        '
        'dgvPadres
        '
        Me.dgvPadres.AllowUserToAddRows = False
        Me.dgvPadres.AllowUserToDeleteRows = False
        Me.dgvPadres.AllowUserToResizeRows = False
        Me.dgvPadres.BackgroundColor = System.Drawing.Color.White
        Me.dgvPadres.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPadres.Location = New System.Drawing.Point(20, 261)
        Me.dgvPadres.MultiSelect = False
        Me.dgvPadres.Name = "dgvPadres"
        Me.dgvPadres.ReadOnly = True
        Me.dgvPadres.RowHeadersVisible = False
        Me.dgvPadres.RowHeadersWidth = 62
        Me.dgvPadres.RowTemplate.Height = 28
        Me.dgvPadres.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPadres.Size = New System.Drawing.Size(905, 217)
        Me.dgvPadres.TabIndex = 8
        '
        'lblHintPadre
        '
        Me.lblHintPadre.AutoSize = True
        Me.lblHintPadre.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic)
        Me.lblHintPadre.ForeColor = System.Drawing.Color.DimGray
        Me.lblHintPadre.Location = New System.Drawing.Point(20, 230)
        Me.lblHintPadre.Name = "lblHintPadre"
        Me.lblHintPadre.Size = New System.Drawing.Size(554, 21)
        Me.lblHintPadre.TabIndex = 7
        Me.lblHintPadre.Text = "Escriba criterios como en Bandeja de Entrada y seleccione un registro en la grilla."
        '
        'txtBuscarPadre
        '
        Me.txtBuscarPadre.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtBuscarPadre.Location = New System.Drawing.Point(20, 192)
        Me.txtBuscarPadre.Name = "txtBuscarPadre"
        Me.txtBuscarPadre.Size = New System.Drawing.Size(905, 34)
        Me.txtBuscarPadre.TabIndex = 6
        '
        'lblBuscarPadre
        '
        Me.lblBuscarPadre.AutoSize = True
        Me.lblBuscarPadre.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblBuscarPadre.Location = New System.Drawing.Point(16, 168)
        Me.lblBuscarPadre.Name = "lblBuscarPadre"
        Me.lblBuscarPadre.Size = New System.Drawing.Size(229, 25)
        Me.lblBuscarPadre.TabIndex = 5
        Me.lblBuscarPadre.Text = "Buscar Documento Padre"
        '
        'lblDetalleHijo
        '
        Me.lblDetalleHijo.BackColor = System.Drawing.Color.Honeydew
        Me.lblDetalleHijo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblDetalleHijo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblDetalleHijo.ForeColor = System.Drawing.Color.DarkGreen
        Me.lblDetalleHijo.Location = New System.Drawing.Point(20, 76)
        Me.lblDetalleHijo.Name = "lblDetalleHijo"
        Me.lblDetalleHijo.Padding = New System.Windows.Forms.Padding(8, 6, 8, 6)
        Me.lblDetalleHijo.Size = New System.Drawing.Size(905, 76)
        Me.lblDetalleHijo.TabIndex = 4
        Me.lblDetalleHijo.Text = "Detalle del documento hijo seleccionado"
        '
        'txtIdPadre
        '
        Me.txtIdPadre.BackColor = System.Drawing.Color.White
        Me.txtIdPadre.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtIdPadre.Location = New System.Drawing.Point(630, 38)
        Me.txtIdPadre.Name = "txtIdPadre"
        Me.txtIdPadre.ReadOnly = True
        Me.txtIdPadre.Size = New System.Drawing.Size(295, 37)
        Me.txtIdPadre.TabIndex = 3
        '
        'lblPadre
        '
        Me.lblPadre.AutoSize = True
        Me.lblPadre.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblPadre.Location = New System.Drawing.Point(626, 20)
        Me.lblPadre.Name = "lblPadre"
        Me.lblPadre.Size = New System.Drawing.Size(238, 25)
        Me.lblPadre.TabIndex = 2
        Me.lblPadre.Text = "ID del PADRE seleccionado"
        '
        'txtIdHijo
        '
        Me.txtIdHijo.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtIdHijo.Location = New System.Drawing.Point(20, 38)
        Me.txtIdHijo.Name = "txtIdHijo"
        Me.txtIdHijo.Size = New System.Drawing.Size(295, 37)
        Me.txtIdHijo.TabIndex = 1
        '
        'lblHijo
        '
        Me.lblHijo.AutoSize = True
        Me.lblHijo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblHijo.Location = New System.Drawing.Point(16, 20)
        Me.lblHijo.Name = "lblHijo"
        Me.lblHijo.Size = New System.Drawing.Size(265, 25)
        Me.lblHijo.TabIndex = 0
        Me.lblHijo.Text = "ID del HIJO (El que se mueve)"
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(851, 562)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(105, 35)
        Me.btnCancelar.TabIndex = 3
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnConfirmar
        '
        Me.btnConfirmar.BackColor = System.Drawing.Color.ForestGreen
        Me.btnConfirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConfirmar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnConfirmar.ForeColor = System.Drawing.Color.White
        Me.btnConfirmar.Location = New System.Drawing.Point(730, 562)
        Me.btnConfirmar.Name = "btnConfirmar"
        Me.btnConfirmar.Size = New System.Drawing.Size(115, 35)
        Me.btnConfirmar.TabIndex = 2
        Me.btnConfirmar.Text = "VINCULAR"
        Me.btnConfirmar.UseVisualStyleBackColor = False
        '
        'frmVincular
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(968, 609)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnConfirmar)
        Me.Controls.Add(Me.grpDatos)
        Me.Controls.Add(Me.lblTitulo)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmVincular"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Gestión de Vinculaciones"
        Me.grpDatos.ResumeLayout(False)
        Me.grpDatos.PerformLayout()
        CType(Me.dgvPadres, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lblTitulo As Label
    Friend WithEvents grpDatos As GroupBox
    Friend WithEvents dgvPadres As DataGridView
    Friend WithEvents lblHintPadre As Label
    Friend WithEvents txtBuscarPadre As TextBox
    Friend WithEvents lblBuscarPadre As Label
    Friend WithEvents lblDetalleHijo As Label
    Friend WithEvents txtIdPadre As TextBox
    Friend WithEvents lblPadre As Label
    Friend WithEvents txtIdHijo As TextBox
    Friend WithEvents lblHijo As Label
    Friend WithEvents btnCancelar As Button
    Friend WithEvents btnConfirmar As Button
End Class
