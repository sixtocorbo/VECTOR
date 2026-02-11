<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMesaEntrada
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
        Me.grpOrigen = New System.Windows.Forms.GroupBox()
        Me.dtpFechaRecepcion = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtBuscarOrigen = New System.Windows.Forms.TextBox()
        Me.cboOrigen = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpDetalles = New System.Windows.Forms.GroupBox()
        Me.numFojas = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtNumeroRef = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cboTipo = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.grpContenido = New System.Windows.Forms.GroupBox()
        Me.btnBuscarPPL = New System.Windows.Forms.Button()
        Me.txtDescripcion = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtAsunto = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.grpAdjuntos = New System.Windows.Forms.GroupBox()
        Me.lblAdjuntosInfo = New System.Windows.Forms.Label()
        Me.btnQuitarAdjunto = New System.Windows.Forms.Button()
        Me.btnAbrirAdjunto = New System.Windows.Forms.Button()
        Me.btnAdjuntar = New System.Windows.Forms.Button()
        Me.lstAdjuntos = New System.Windows.Forms.ListBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.grpOrigen.SuspendLayout()
        Me.grpDetalles.SuspendLayout()
        CType(Me.numFojas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpContenido.SuspendLayout()
        Me.grpAdjuntos.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpOrigen
        '
        ' IMPORTANTE: Anclar a Izquierda, Arriba y Derecha para que se estire
        Me.grpOrigen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpOrigen.Controls.Add(Me.dtpFechaRecepcion)
        Me.grpOrigen.Controls.Add(Me.Label2)
        Me.grpOrigen.Controls.Add(Me.txtBuscarOrigen)
        Me.grpOrigen.Controls.Add(Me.cboOrigen)
        Me.grpOrigen.Controls.Add(Me.Label1)
        Me.grpOrigen.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpOrigen.Location = New System.Drawing.Point(18, 18)
        Me.grpOrigen.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpOrigen.Name = "grpOrigen"
        Me.grpOrigen.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpOrigen.Size = New System.Drawing.Size(840, 131)
        Me.grpOrigen.TabIndex = 0
        Me.grpOrigen.TabStop = False
        Me.grpOrigen.Text = "1. Origen del Documento"
        '
        'dtpFechaRecepcion
        '
        Me.dtpFechaRecepcion.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaRecepcion.Location = New System.Drawing.Point(612, 69)
        Me.dtpFechaRecepcion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpFechaRecepcion.Name = "dtpFechaRecepcion"
        Me.dtpFechaRecepcion.Size = New System.Drawing.Size(199, 31)
        Me.dtpFechaRecepcion.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(608, 42)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(146, 25)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Fecha Recepción:"
        '
        'txtBuscarOrigen
        '
        Me.txtBuscarOrigen.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBuscarOrigen.Location = New System.Drawing.Point(28, 69)
        Me.txtBuscarOrigen.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtBuscarOrigen.Name = "txtBuscarOrigen"
        Me.txtBuscarOrigen.Size = New System.Drawing.Size(550, 31)
        Me.txtBuscarOrigen.TabIndex = 1
        '
        'cboOrigen
        '
        Me.cboOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOrigen.FormattingEnabled = True
        Me.cboOrigen.Location = New System.Drawing.Point(28, 69)
        Me.cboOrigen.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cboOrigen.Name = "cboOrigen"
        Me.cboOrigen.Size = New System.Drawing.Size(550, 33)
        Me.cboOrigen.TabIndex = 99
        Me.cboOrigen.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 42)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(177, 25)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Organismo / Oficina:"
        '
        'grpDetalles
        '
        ' IMPORTANTE: Anclar
        Me.grpDetalles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDetalles.Controls.Add(Me.numFojas)
        Me.grpDetalles.Controls.Add(Me.Label5)
        Me.grpDetalles.Controls.Add(Me.txtNumeroRef)
        Me.grpDetalles.Controls.Add(Me.Label4)
        Me.grpDetalles.Controls.Add(Me.cboTipo)
        Me.grpDetalles.Controls.Add(Me.Label3)
        Me.grpDetalles.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpDetalles.Location = New System.Drawing.Point(18, 174)
        Me.grpDetalles.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpDetalles.Name = "grpDetalles"
        Me.grpDetalles.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpDetalles.Size = New System.Drawing.Size(840, 138)
        Me.grpDetalles.TabIndex = 1
        Me.grpDetalles.TabStop = False
        Me.grpDetalles.Text = "2. Identificación"
        '
        'numFojas
        '
        Me.numFojas.Location = New System.Drawing.Point(694, 74)
        Me.numFojas.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.numFojas.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.numFojas.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numFojas.Name = "numFojas"
        Me.numFojas.Size = New System.Drawing.Size(118, 31)
        Me.numFojas.TabIndex = 5
        Me.numFojas.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(690, 45)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(98, 25)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Cant. Fojas"
        '
        'txtNumeroRef
        '
        Me.txtNumeroRef.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNumeroRef.Location = New System.Drawing.Point(339, 72)
        Me.txtNumeroRef.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtNumeroRef.Name = "txtNumeroRef"
        Me.txtNumeroRef.Size = New System.Drawing.Size(320, 31)
        Me.txtNumeroRef.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(334, 45)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(130, 25)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "N° (Ej.: 542/26)"
        '
        'cboTipo
        '
        Me.cboTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTipo.FormattingEnabled = True
        Me.cboTipo.Location = New System.Drawing.Point(28, 72)
        Me.cboTipo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cboTipo.Name = "cboTipo"
        Me.cboTipo.Size = New System.Drawing.Size(280, 33)
        Me.cboTipo.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(24, 45)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(150, 25)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Tipo Documento:"
        '
        'grpContenido
        '
        ' IMPORTANTE: Anclar
        Me.grpContenido.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpContenido.Controls.Add(Me.btnBuscarPPL)
        Me.grpContenido.Controls.Add(Me.txtDescripcion)
        Me.grpContenido.Controls.Add(Me.Label7)
        Me.grpContenido.Controls.Add(Me.txtAsunto)
        Me.grpContenido.Controls.Add(Me.Label6)
        Me.grpContenido.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpContenido.Location = New System.Drawing.Point(18, 337)
        Me.grpContenido.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpContenido.Name = "grpContenido"
        Me.grpContenido.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpContenido.Size = New System.Drawing.Size(840, 285)
        Me.grpContenido.TabIndex = 2
        Me.grpContenido.TabStop = False
        Me.grpContenido.Text = "3. Contenido"
        '
        'btnBuscarPPL
        '
        Me.btnBuscarPPL.BackColor = System.Drawing.Color.LightGray
        Me.btnBuscarPPL.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnBuscarPPL.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnBuscarPPL.Font = New System.Drawing.Font("Segoe UI Emoji", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBuscarPPL.Location = New System.Drawing.Point(753, 66)
        Me.btnBuscarPPL.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnBuscarPPL.Name = "btnBuscarPPL"
        Me.btnBuscarPPL.Size = New System.Drawing.Size(60, 37)
        Me.btnBuscarPPL.TabIndex = 2
        Me.btnBuscarPPL.Text = "🔍"
        Me.btnBuscarPPL.UseVisualStyleBackColor = False
        '
        'txtDescripcion
        '
        ' IMPORTANTE: Anclar el texto para que crezca con el grupo
        Me.txtDescripcion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescripcion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDescripcion.Location = New System.Drawing.Point(28, 145)
        Me.txtDescripcion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtDescripcion.Multiline = True
        Me.txtDescripcion.Name = "txtDescripcion"
        Me.txtDescripcion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescripcion.Size = New System.Drawing.Size(782, 115)
        Me.txtDescripcion.TabIndex = 4
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(24, 117)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(235, 25)
        Me.Label7.TabIndex = 3
        Me.Label7.Text = "Descripción Detallada (Obs):"
        '
        'txtAsunto
        '
        ' IMPORTANTE: Anclar el asunto
        Me.txtAsunto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAsunto.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAsunto.Location = New System.Drawing.Point(28, 68)
        Me.txtAsunto.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtAsunto.Name = "txtAsunto"
        Me.txtAsunto.Size = New System.Drawing.Size(714, 31)
        Me.txtAsunto.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(24, 40)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(181, 25)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Asunto (Título breve):"
        '
        'grpAdjuntos
        '
        ' IMPORTANTE: Anclar
        Me.grpAdjuntos.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAdjuntos.Controls.Add(Me.lblAdjuntosInfo)
        Me.grpAdjuntos.Controls.Add(Me.btnQuitarAdjunto)
        Me.grpAdjuntos.Controls.Add(Me.btnAbrirAdjunto)
        Me.grpAdjuntos.Controls.Add(Me.btnAdjuntar)
        Me.grpAdjuntos.Controls.Add(Me.lstAdjuntos)
        Me.grpAdjuntos.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpAdjuntos.Location = New System.Drawing.Point(18, 640)
        Me.grpAdjuntos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpAdjuntos.Name = "grpAdjuntos"
        Me.grpAdjuntos.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpAdjuntos.Size = New System.Drawing.Size(840, 160)
        Me.grpAdjuntos.TabIndex = 3
        Me.grpAdjuntos.TabStop = False
        Me.grpAdjuntos.Text = "4. Adjuntos Digitales"
        '
        'lblAdjuntosInfo
        '
        Me.lblAdjuntosInfo.AutoSize = True
        Me.lblAdjuntosInfo.Location = New System.Drawing.Point(24, 131)
        Me.lblAdjuntosInfo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAdjuntosInfo.Name = "lblAdjuntosInfo"
        Me.lblAdjuntosInfo.Size = New System.Drawing.Size(102, 25)
        Me.lblAdjuntosInfo.TabIndex = 4
        Me.lblAdjuntosInfo.Text = "0 archivo(s)"
        '
        'btnQuitarAdjunto
        '
        Me.btnQuitarAdjunto.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnQuitarAdjunto.BackColor = System.Drawing.Color.LightGray
        Me.btnQuitarAdjunto.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnQuitarAdjunto.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnQuitarAdjunto.Location = New System.Drawing.Point(610, 107)
        Me.btnQuitarAdjunto.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnQuitarAdjunto.Name = "btnQuitarAdjunto"
        Me.btnQuitarAdjunto.Size = New System.Drawing.Size(202, 34)
        Me.btnQuitarAdjunto.TabIndex = 3
        Me.btnQuitarAdjunto.Text = "Quitar"
        Me.btnQuitarAdjunto.UseVisualStyleBackColor = False
        '
        'btnAbrirAdjunto
        '
        Me.btnAbrirAdjunto.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAbrirAdjunto.BackColor = System.Drawing.Color.LightGray
        Me.btnAbrirAdjunto.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAbrirAdjunto.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnAbrirAdjunto.Location = New System.Drawing.Point(610, 70)
        Me.btnAbrirAdjunto.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAbrirAdjunto.Name = "btnAbrirAdjunto"
        Me.btnAbrirAdjunto.Size = New System.Drawing.Size(202, 34)
        Me.btnAbrirAdjunto.TabIndex = 2
        Me.btnAbrirAdjunto.Text = "Abrir"
        Me.btnAbrirAdjunto.UseVisualStyleBackColor = False
        '
        'btnAdjuntar
        '
        Me.btnAdjuntar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdjuntar.BackColor = System.Drawing.Color.LightGray
        Me.btnAdjuntar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAdjuntar.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnAdjuntar.Location = New System.Drawing.Point(610, 32)
        Me.btnAdjuntar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAdjuntar.Name = "btnAdjuntar"
        Me.btnAdjuntar.Size = New System.Drawing.Size(202, 34)
        Me.btnAdjuntar.TabIndex = 1
        Me.btnAdjuntar.Text = "Adjuntar..."
        Me.btnAdjuntar.UseVisualStyleBackColor = False
        '
        'lstAdjuntos
        '
        Me.lstAdjuntos.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstAdjuntos.FormattingEnabled = True
        Me.lstAdjuntos.ItemHeight = 25
        Me.lstAdjuntos.Location = New System.Drawing.Point(28, 32)
        Me.lstAdjuntos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lstAdjuntos.Name = "lstAdjuntos"
        Me.lstAdjuntos.Size = New System.Drawing.Size(560, 104)
        Me.lstAdjuntos.TabIndex = 0
        '
        'btnGuardar
        '
        ' IMPORTANTE: Anclar ABAJO y DERECHA para que no se oculte al redimensionar
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.BackColor = System.Drawing.Color.ForestGreen
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(592, 812)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(266, 58)
        Me.btnGuardar.TabIndex = 4
        Me.btnGuardar.Text = "REGISTRAR ENTRADA"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'btnCancelar
        '
        ' IMPORTANTE: Anclar ABAJO y DERECHA
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancelar.Location = New System.Drawing.Point(402, 812)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(165, 58)
        Me.btnCancelar.TabIndex = 5
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmMesaEntrada
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(886, 900)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.grpAdjuntos)
        Me.Controls.Add(Me.grpContenido)
        Me.Controls.Add(Me.grpDetalles)
        Me.Controls.Add(Me.grpOrigen)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmMesaEntrada"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "VECTOR - Mesa de Entrada"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpOrigen.ResumeLayout(False)
        Me.grpOrigen.PerformLayout()
        Me.grpDetalles.ResumeLayout(False)
        Me.grpDetalles.PerformLayout()
        CType(Me.numFojas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpContenido.ResumeLayout(False)
        Me.grpContenido.PerformLayout()
        Me.grpAdjuntos.ResumeLayout(False)
        Me.grpAdjuntos.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpOrigen As GroupBox
    Friend WithEvents dtpFechaRecepcion As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents txtBuscarOrigen As TextBox
    Friend WithEvents cboOrigen As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents grpDetalles As GroupBox
    Friend WithEvents numFojas As NumericUpDown
    Friend WithEvents Label5 As Label
    Friend WithEvents txtNumeroRef As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents cboTipo As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents grpContenido As GroupBox
    Friend WithEvents btnBuscarPPL As Button
    Friend WithEvents txtDescripcion As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtAsunto As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents grpAdjuntos As GroupBox
    Friend WithEvents lblAdjuntosInfo As Label
    Friend WithEvents btnQuitarAdjunto As Button
    Friend WithEvents btnAbrirAdjunto As Button
    Friend WithEvents btnAdjuntar As Button
    Friend WithEvents lstAdjuntos As ListBox
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
End Class