<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmHistorial
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PanelHeader = New System.Windows.Forms.Panel()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblAsunto = New System.Windows.Forms.Label()
        Me.lblNumero = New System.Windows.Forms.Label()
        Me.dgvHistoria = New System.Windows.Forms.DataGridView()
        Me.PanelHeader.SuspendLayout()
        CType(Me.dgvHistoria, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelHeader
        '
        Me.PanelHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.PanelHeader.Controls.Add(Me.btnPrint)
        Me.PanelHeader.Controls.Add(Me.btnClose)
        Me.PanelHeader.Controls.Add(Me.lblAsunto)
        Me.PanelHeader.Controls.Add(Me.lblNumero)
        Me.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelHeader.Location = New System.Drawing.Point(0, 0)
        Me.PanelHeader.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PanelHeader.Name = "PanelHeader"
        Me.PanelHeader.Size = New System.Drawing.Size(1200, 123)
        Me.PanelHeader.TabIndex = 0
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.BackColor = System.Drawing.Color.SteelBlue
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.ForeColor = System.Drawing.Color.White
        Me.btnPrint.Location = New System.Drawing.Point(1027, 18)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(95, 35)
        Me.btnPrint.TabIndex = 3
        Me.btnPrint.Text = "Imprimir"
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.IndianRed
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.ForeColor = System.Drawing.Color.White
        Me.btnClose.Location = New System.Drawing.Point(1130, 18)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(52, 35)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "X"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'lblAsunto
        '
        Me.lblAsunto.AutoSize = True
        Me.lblAsunto.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAsunto.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.lblAsunto.Location = New System.Drawing.Point(22, 69)
        Me.lblAsunto.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAsunto.Name = "lblAsunto"
        Me.lblAsunto.Size = New System.Drawing.Size(212, 28)
        Me.lblAsunto.TabIndex = 1
        Me.lblAsunto.Text = "Asunto del documento"
        '
        'lblNumero
        '
        Me.lblNumero.AutoSize = True
        Me.lblNumero.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumero.ForeColor = System.Drawing.Color.White
        Me.lblNumero.Location = New System.Drawing.Point(21, 23)
        Me.lblNumero.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNumero.Name = "lblNumero"
        Me.lblNumero.Size = New System.Drawing.Size(322, 38)
        Me.lblNumero.TabIndex = 0
        Me.lblNumero.Text = "EXP-2026/001 - OFICIO"
        '
        'dgvHistoria
        '
        Me.dgvHistoria.AllowUserToAddRows = False
        Me.dgvHistoria.AllowUserToDeleteRows = False
        Me.dgvHistoria.AllowUserToResizeColumns = False
        Me.dgvHistoria.AllowUserToResizeRows = False
        Me.dgvHistoria.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvHistoria.BackgroundColor = System.Drawing.Color.White
        Me.dgvHistoria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SteelBlue
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvHistoria.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvHistoria.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvHistoria.Location = New System.Drawing.Point(0, 123)
        Me.dgvHistoria.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvHistoria.Name = "dgvHistoria"
        Me.dgvHistoria.ReadOnly = True
        Me.dgvHistoria.RowHeadersVisible = False
        Me.dgvHistoria.RowHeadersWidth = 62
        Me.dgvHistoria.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvHistoria.Size = New System.Drawing.Size(1200, 569)
        Me.dgvHistoria.TabIndex = 1
        '
        'frmHistorial
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1200, 692)
        Me.Controls.Add(Me.dgvHistoria)
        Me.Controls.Add(Me.PanelHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmHistorial"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Historial"
        Me.PanelHeader.ResumeLayout(False)
        Me.PanelHeader.PerformLayout()
        CType(Me.dgvHistoria, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelHeader As Panel
    Friend WithEvents lblAsunto As Label
    Friend WithEvents lblNumero As Label
    Friend WithEvents dgvHistoria As DataGridView
    Friend WithEvents btnClose As Button
    Friend WithEvents btnPrint As Button
End Class
