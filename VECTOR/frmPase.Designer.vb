<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPase
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
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.cboDestino = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.numFojasAgregadas = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtObservacion = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnConfirmar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.txtResumen = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.numFojasAgregadas, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.lblTitulo.Size = New System.Drawing.Size(584, 40)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "CONFIRMACIÓN DE PASE (SALIDA)"
        Me.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtBuscar)
        Me.GroupBox1.Controls.Add(Me.cboDestino)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 185)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(560, 146)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Destino"
        '
        'txtBuscar
        '
        Me.txtBuscar.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtBuscar.Location = New System.Drawing.Point(20, 43)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(520, 31)
        Me.txtBuscar.TabIndex = 0
        '
        'cboDestino
        '
        Me.cboDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDestino.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.cboDestino.FormattingEnabled = True
        Me.cboDestino.Location = New System.Drawing.Point(20, 72)
        Me.cboDestino.Name = "cboDestino"
        Me.cboDestino.Size = New System.Drawing.Size(520, 36)
        Me.cboDestino.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(17, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(215, 25)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "¿A qué oficina se envía?"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.numFojasAgregadas)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.txtObservacion)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 337)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(560, 173)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Detalles del Pase"
        '
        'numFojasAgregadas
        '
        Me.numFojasAgregadas.Location = New System.Drawing.Point(286, 130)
        Me.numFojasAgregadas.Name = "numFojasAgregadas"
        Me.numFojasAgregadas.Size = New System.Drawing.Size(60, 31)
        Me.numFojasAgregadas.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 132)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(263, 25)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Fojas agregadas en esta oficina:"
        '
        'txtObservacion
        '
        Me.txtObservacion.Location = New System.Drawing.Point(20, 64)
        Me.txtObservacion.Multiline = True
        Me.txtObservacion.Name = "txtObservacion"
        Me.txtObservacion.Size = New System.Drawing.Size(520, 60)
        Me.txtObservacion.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(17, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(266, 25)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Instrucción / Motivo del pase:"
        '
        'btnConfirmar
        '
        Me.btnConfirmar.BackColor = System.Drawing.Color.ForestGreen
        Me.btnConfirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConfirmar.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnConfirmar.ForeColor = System.Drawing.Color.White
        Me.btnConfirmar.Location = New System.Drawing.Point(392, 540)
        Me.btnConfirmar.Name = "btnConfirmar"
        Me.btnConfirmar.Size = New System.Drawing.Size(180, 40)
        Me.btnConfirmar.TabIndex = 3
        Me.btnConfirmar.Text = "CONFIRMAR ENVÍO"
        Me.btnConfirmar.UseVisualStyleBackColor = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(266, 540)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(110, 40)
        Me.btnCancelar.TabIndex = 4
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'txtResumen
        '
        Me.txtResumen.BackColor = System.Drawing.Color.White
        Me.txtResumen.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtResumen.Location = New System.Drawing.Point(12, 49)
        Me.txtResumen.Multiline = True
        Me.txtResumen.Name = "txtResumen"
        Me.txtResumen.ReadOnly = True
        Me.txtResumen.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtResumen.Size = New System.Drawing.Size(560, 130)
        Me.txtResumen.TabIndex = 5
        Me.txtResumen.TabStop = False
        '
        'frmPase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(584, 592)
        Me.Controls.Add(Me.txtResumen)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnConfirmar)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblTitulo)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPase"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "VECTOR - Movimiento de Expediente"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.numFojasAgregadas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitulo As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtBuscar As TextBox
    Friend WithEvents cboDestino As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents txtObservacion As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnConfirmar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents numFojasAgregadas As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents txtResumen As TextBox
End Class