<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmConfiguracionSistema
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
        Me.grpRenovaciones = New System.Windows.Forms.GroupBox()
        Me.lblAyuda = New System.Windows.Forms.Label()
        Me.nudDiasAlertaRenovaciones = New System.Windows.Forms.NumericUpDown()
        Me.lblDias = New System.Windows.Forms.Label()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.grpRenovaciones.SuspendLayout()
        CType(Me.nudDiasAlertaRenovaciones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(24, 20)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(314, 38)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "Configuración del sistema"
        '
        'grpRenovaciones
        '
        Me.grpRenovaciones.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpRenovaciones.Controls.Add(Me.lblAyuda)
        Me.grpRenovaciones.Controls.Add(Me.nudDiasAlertaRenovaciones)
        Me.grpRenovaciones.Controls.Add(Me.lblDias)
        Me.grpRenovaciones.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.grpRenovaciones.Location = New System.Drawing.Point(30, 72)
        Me.grpRenovaciones.Name = "grpRenovaciones"
        Me.grpRenovaciones.Size = New System.Drawing.Size(754, 161)
        Me.grpRenovaciones.TabIndex = 1
        Me.grpRenovaciones.TabStop = False
        Me.grpRenovaciones.Text = "Renovaciones Art. 120"
        '
        'lblAyuda
        '
        Me.lblAyuda.AutoSize = True
        Me.lblAyuda.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular)
        Me.lblAyuda.Location = New System.Drawing.Point(21, 95)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Size = New System.Drawing.Size(695, 25)
        Me.lblAyuda.TabIndex = 2
        Me.lblAyuda.Text = "Este valor define cuántos días antes del vencimiento se marca una salida en estado ALERTA."
        '
        'nudDiasAlertaRenovaciones
        '
        Me.nudDiasAlertaRenovaciones.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.nudDiasAlertaRenovaciones.Location = New System.Drawing.Point(398, 43)
        Me.nudDiasAlertaRenovaciones.Maximum = New Decimal(New Integer() {365, 0, 0, 0})
        Me.nudDiasAlertaRenovaciones.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudDiasAlertaRenovaciones.Name = "nudDiasAlertaRenovaciones"
        Me.nudDiasAlertaRenovaciones.Size = New System.Drawing.Size(126, 34)
        Me.nudDiasAlertaRenovaciones.TabIndex = 1
        Me.nudDiasAlertaRenovaciones.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.nudDiasAlertaRenovaciones.Value = New Decimal(New Integer() {30, 0, 0, 0})
        '
        'lblDias
        '
        Me.lblDias.AutoSize = True
        Me.lblDias.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular)
        Me.lblDias.Location = New System.Drawing.Point(22, 47)
        Me.lblDias.Name = "lblDias"
        Me.lblDias.Size = New System.Drawing.Size(348, 25)
        Me.lblDias.TabIndex = 0
        Me.lblDias.Text = "Días de anticipación para alerta de vencimiento:"
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.BackColor = System.Drawing.Color.SeaGreen
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(623, 249)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(161, 44)
        Me.btnGuardar.TabIndex = 2
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'frmConfiguracionSistema
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(814, 317)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.grpRenovaciones)
        Me.Controls.Add(Me.lblTitulo)
        Me.Name = "frmConfiguracionSistema"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Configuración del sistema"
        Me.grpRenovaciones.ResumeLayout(False)
        Me.grpRenovaciones.PerformLayout()
        CType(Me.nudDiasAlertaRenovaciones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitulo As Label
    Friend WithEvents grpRenovaciones As GroupBox
    Friend WithEvents lblAyuda As Label
    Friend WithEvents nudDiasAlertaRenovaciones As NumericUpDown
    Friend WithEvents lblDias As Label
    Friend WithEvents btnGuardar As Button
End Class
