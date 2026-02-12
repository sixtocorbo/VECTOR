<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmOpcionesReporte
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.btnPDF = New System.Windows.Forms.Button() ' <--- NUEVO
        Me.grpOpciones = New System.Windows.Forms.GroupBox()
        Me.lblHasta = New System.Windows.Forms.Label()
        Me.lblDesde = New System.Windows.Forms.Label()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.rbRangoFechas = New System.Windows.Forms.RadioButton()
        Me.rbSeleccionado = New System.Windows.Forms.RadioButton()
        Me.grpOpciones.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(234, 186)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(95, 35)
        Me.btnCancelar.TabIndex = 5
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnImprimir
        '
        Me.btnImprimir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImprimir.BackColor = System.Drawing.Color.SteelBlue
        Me.btnImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImprimir.ForeColor = System.Drawing.Color.White
        Me.btnImprimir.Location = New System.Drawing.Point(118, 186)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(110, 35)
        Me.btnImprimir.TabIndex = 4
        Me.btnImprimir.Text = "IMPRIMIR"
        Me.btnImprimir.UseVisualStyleBackColor = False
        '
        'btnPDF (NUEVO BOTÓN)
        '
        Me.btnPDF.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPDF.BackColor = System.Drawing.Color.IndianRed
        Me.btnPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPDF.ForeColor = System.Drawing.Color.White
        Me.btnPDF.Location = New System.Drawing.Point(12, 186)
        Me.btnPDF.Name = "btnPDF"
        Me.btnPDF.Size = New System.Drawing.Size(100, 35)
        Me.btnPDF.TabIndex = 6
        Me.btnPDF.Text = "EXPORTAR PDF"
        Me.btnPDF.UseVisualStyleBackColor = False
        '
        'grpOpciones
        '
        Me.grpOpciones.Controls.Add(Me.lblHasta)
        Me.grpOpciones.Controls.Add(Me.lblDesde)
        Me.grpOpciones.Controls.Add(Me.dtpHasta)
        Me.grpOpciones.Controls.Add(Me.dtpDesde)
        Me.grpOpciones.Controls.Add(Me.rbRangoFechas)
        Me.grpOpciones.Controls.Add(Me.rbSeleccionado)
        Me.grpOpciones.Location = New System.Drawing.Point(12, 12)
        Me.grpOpciones.Name = "grpOpciones"
        Me.grpOpciones.Size = New System.Drawing.Size(317, 159)
        Me.grpOpciones.TabIndex = 6
        Me.grpOpciones.TabStop = False
        Me.grpOpciones.Text = "Tipo de Reporte"
        '
        'lblHasta
        '
        Me.lblHasta.AutoSize = True
        Me.lblHasta.Location = New System.Drawing.Point(167, 98)
        Me.lblHasta.Name = "lblHasta"
        Me.lblHasta.Size = New System.Drawing.Size(38, 13)
        Me.lblHasta.TabIndex = 5
        Me.lblHasta.Text = "Hasta:"
        '
        'lblDesde
        '
        Me.lblDesde.AutoSize = True
        Me.lblDesde.Location = New System.Drawing.Point(37, 98)
        Me.lblDesde.Name = "lblDesde"
        Me.lblDesde.Size = New System.Drawing.Size(41, 13)
        Me.lblDesde.TabIndex = 4
        Me.lblDesde.Text = "Desde:"
        '
        'dtpHasta
        '
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpHasta.Location = New System.Drawing.Point(170, 114)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(111, 20)
        Me.dtpHasta.TabIndex = 3
        '
        'dtpDesde
        '
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDesde.Location = New System.Drawing.Point(40, 114)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(111, 20)
        Me.dtpDesde.TabIndex = 2
        '
        'rbRangoFechas
        '
        Me.rbRangoFechas.AutoSize = True
        Me.rbRangoFechas.Location = New System.Drawing.Point(20, 65)
        Me.rbRangoFechas.Name = "rbRangoFechas"
        Me.rbRangoFechas.Size = New System.Drawing.Size(155, 17)
        Me.rbRangoFechas.TabIndex = 1
        Me.rbRangoFechas.TabStop = True
        Me.rbRangoFechas.Text = "Reporte General por Fechas"
        Me.rbRangoFechas.UseVisualStyleBackColor = True
        '
        'rbSeleccionado
        '
        Me.rbSeleccionado.AutoSize = True
        Me.rbSeleccionado.Checked = True
        Me.rbSeleccionado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbSeleccionado.Location = New System.Drawing.Point(20, 30)
        Me.rbSeleccionado.Name = "rbSeleccionado"
        Me.rbSeleccionado.Size = New System.Drawing.Size(225, 17)
        Me.rbSeleccionado.TabIndex = 0
        Me.rbSeleccionado.TabStop = True
        Me.rbSeleccionado.Text = "Historial Completo del Seleccionado"
        Me.rbSeleccionado.UseVisualStyleBackColor = True
        '
        'frmOpcionesReporte
        '
        Me.AcceptButton = Me.btnImprimir
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(341, 233)
        Me.Controls.Add(Me.grpOpciones)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.btnPDF) ' <--- AGREGADO A LA COLECCIÓN
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOpcionesReporte"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Opciones de Impresión"
        Me.grpOpciones.ResumeLayout(False)
        Me.grpOpciones.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnCancelar As Button
    Friend WithEvents btnImprimir As Button
    Friend WithEvents btnPDF As Button ' <--- DECLARACIÓN
    Friend WithEvents grpOpciones As GroupBox
    Friend WithEvents lblHasta As Label
    Friend WithEvents lblDesde As Label
    Friend WithEvents dtpHasta As DateTimePicker
    Friend WithEvents dtpDesde As DateTimePicker
    Friend WithEvents rbRangoFechas As RadioButton
    Friend WithEvents rbSeleccionado As RadioButton
End Class