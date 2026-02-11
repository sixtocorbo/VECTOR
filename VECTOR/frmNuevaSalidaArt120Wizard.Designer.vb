<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNuevaSalidaArt120Wizard
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
        Me.LblPaso = New System.Windows.Forms.Label()
        Me.Tabs = New System.Windows.Forms.TabControl()
        Me.TabRecluso = New System.Windows.Forms.TabPage()
        Me.LblAyudaRecluso = New System.Windows.Forms.Label()
        Me.LblIdRecluso = New System.Windows.Forms.Label()
        Me.BtnBuscarRecluso = New System.Windows.Forms.Button()
        Me.TxtRecluso = New System.Windows.Forms.TextBox()
        Me.LblReclusoTitulo = New System.Windows.Forms.Label()
        Me.LblTituloPaso1 = New System.Windows.Forms.Label()
        Me.TabAutorizacion = New System.Windows.Forms.TabPage()
        Me.DtpNotificacion = New System.Windows.Forms.DateTimePicker()
        Me.LblNotificacion = New System.Windows.Forms.Label()
        Me.DtpVencimiento = New System.Windows.Forms.DateTimePicker()
        Me.LblVencimiento = New System.Windows.Forms.Label()
        Me.DtpInicio = New System.Windows.Forms.DateTimePicker()
        Me.LblInicio = New System.Windows.Forms.Label()
        Me.CboAutorizacion = New System.Windows.Forms.ComboBox()
        Me.LblAutorizacion = New System.Windows.Forms.Label()
        Me.LblTituloPaso2 = New System.Windows.Forms.Label()
        Me.TabDetalle = New System.Windows.Forms.TabPage()
        Me.ChkActivo = New System.Windows.Forms.CheckBox()
        Me.TxtObservaciones = New System.Windows.Forms.TextBox()
        Me.LblObs = New System.Windows.Forms.Label()
        Me.TxtCustodia = New System.Windows.Forms.TextBox()
        Me.LblCustodia = New System.Windows.Forms.Label()
        Me.TxtHorario = New System.Windows.Forms.TextBox()
        Me.LblHorario = New System.Windows.Forms.Label()
        Me.TxtLugarTrabajo = New System.Windows.Forms.TextBox()
        Me.LblLugar = New System.Windows.Forms.Label()
        Me.LblTituloPaso3 = New System.Windows.Forms.Label()
        Me.TabResumen = New System.Windows.Forms.TabPage()
        Me.TxtResumen = New System.Windows.Forms.TextBox()
        Me.LblAyudaResumen = New System.Windows.Forms.Label()
        Me.LblTituloPaso4 = New System.Windows.Forms.Label()
        Me.BtnAtras = New System.Windows.Forms.Button()
        Me.BtnSiguiente = New System.Windows.Forms.Button()
        Me.BtnGuardar = New System.Windows.Forms.Button()
        Me.BtnCancelar = New System.Windows.Forms.Button()
        Me.Tabs.SuspendLayout()
        Me.TabRecluso.SuspendLayout()
        Me.TabAutorizacion.SuspendLayout()
        Me.TabDetalle.SuspendLayout()
        Me.TabResumen.SuspendLayout()
        Me.SuspendLayout()
        '
        'LblPaso
        '
        Me.LblPaso.AutoSize = True
        Me.LblPaso.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.LblPaso.Location = New System.Drawing.Point(16, 16)
        Me.LblPaso.Name = "LblPaso"
        Me.LblPaso.Size = New System.Drawing.Size(90, 19)
        Me.LblPaso.TabIndex = 0
        Me.LblPaso.Text = "Paso 1 de 4"
        '
        'Tabs
        '
        Me.Tabs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.Tabs.Controls.Add(Me.TabRecluso)
        Me.Tabs.Controls.Add(Me.TabAutorizacion)
        Me.Tabs.Controls.Add(Me.TabDetalle)
        Me.Tabs.Controls.Add(Me.TabResumen)
        Me.Tabs.ItemSize = New System.Drawing.Size(1, 1)
        Me.Tabs.Location = New System.Drawing.Point(16, 44)
        Me.Tabs.Name = "Tabs"
        Me.Tabs.SelectedIndex = 0
        Me.Tabs.Size = New System.Drawing.Size(710, 420)
        Me.Tabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.Tabs.TabIndex = 1
        '
        'TabRecluso
        '
        Me.TabRecluso.Controls.Add(Me.LblAyudaRecluso)
        Me.TabRecluso.Controls.Add(Me.LblIdRecluso)
        Me.TabRecluso.Controls.Add(Me.BtnBuscarRecluso)
        Me.TabRecluso.Controls.Add(Me.TxtRecluso)
        Me.TabRecluso.Controls.Add(Me.LblReclusoTitulo)
        Me.TabRecluso.Controls.Add(Me.LblTituloPaso1)
        Me.TabRecluso.Location = New System.Drawing.Point(4, 5)
        Me.TabRecluso.Name = "TabRecluso"
        Me.TabRecluso.Padding = New System.Windows.Forms.Padding(3)
        Me.TabRecluso.Size = New System.Drawing.Size(702, 411)
        Me.TabRecluso.TabIndex = 0
        Me.TabRecluso.Text = "Recluso"
        Me.TabRecluso.UseVisualStyleBackColor = True
        '
        'LblAyudaRecluso
        '
        Me.LblAyudaRecluso.AutoSize = True
        Me.LblAyudaRecluso.ForeColor = System.Drawing.Color.DimGray
        Me.LblAyudaRecluso.Location = New System.Drawing.Point(20, 170)
        Me.LblAyudaRecluso.Name = "LblAyudaRecluso"
        Me.LblAyudaRecluso.Size = New System.Drawing.Size(306, 13)
        Me.LblAyudaRecluso.TabIndex = 5
        Me.LblAyudaRecluso.Text = "Busque y seleccione la persona para asociar la autorización."
        '
        'LblIdRecluso
        '
        Me.LblIdRecluso.AutoSize = True
        Me.LblIdRecluso.Location = New System.Drawing.Point(20, 132)
        Me.LblIdRecluso.Name = "LblIdRecluso"
        Me.LblIdRecluso.Size = New System.Drawing.Size(69, 13)
        Me.LblIdRecluso.TabIndex = 4
        Me.LblIdRecluso.Text = "ID: (ninguno)"
        '
        'BtnBuscarRecluso
        '
        Me.BtnBuscarRecluso.Location = New System.Drawing.Point(540, 92)
        Me.BtnBuscarRecluso.Name = "BtnBuscarRecluso"
        Me.BtnBuscarRecluso.Size = New System.Drawing.Size(110, 28)
        Me.BtnBuscarRecluso.TabIndex = 3
        Me.BtnBuscarRecluso.Text = "Buscar..."
        Me.BtnBuscarRecluso.UseVisualStyleBackColor = True
        '
        'TxtRecluso
        '
        Me.TxtRecluso.Location = New System.Drawing.Point(20, 94)
        Me.TxtRecluso.Name = "TxtRecluso"
        Me.TxtRecluso.ReadOnly = True
        Me.TxtRecluso.Size = New System.Drawing.Size(510, 20)
        Me.TxtRecluso.TabIndex = 2
        '
        'LblReclusoTitulo
        '
        Me.LblReclusoTitulo.AutoSize = True
        Me.LblReclusoTitulo.Location = New System.Drawing.Point(20, 72)
        Me.LblReclusoTitulo.Name = "LblReclusoTitulo"
        Me.LblReclusoTitulo.Size = New System.Drawing.Size(49, 13)
        Me.LblReclusoTitulo.TabIndex = 1
        Me.LblReclusoTitulo.Text = "Recluso:"
        '
        'LblTituloPaso1
        '
        Me.LblTituloPaso1.AutoSize = True
        Me.LblTituloPaso1.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.LblTituloPaso1.Location = New System.Drawing.Point(20, 20)
        Me.LblTituloPaso1.Name = "LblTituloPaso1"
        Me.LblTituloPaso1.Size = New System.Drawing.Size(236, 19)
        Me.LblTituloPaso1.TabIndex = 0
        Me.LblTituloPaso1.Text = "Paso 1: Persona privada de libertad"
        '
        'TabAutorizacion
        '
        Me.TabAutorizacion.Controls.Add(Me.DtpNotificacion)
        Me.TabAutorizacion.Controls.Add(Me.LblNotificacion)
        Me.TabAutorizacion.Controls.Add(Me.DtpVencimiento)
        Me.TabAutorizacion.Controls.Add(Me.LblVencimiento)
        Me.TabAutorizacion.Controls.Add(Me.DtpInicio)
        Me.TabAutorizacion.Controls.Add(Me.LblInicio)
        Me.TabAutorizacion.Controls.Add(Me.CboAutorizacion)
        Me.TabAutorizacion.Controls.Add(Me.LblAutorizacion)
        Me.TabAutorizacion.Controls.Add(Me.LblTituloPaso2)
        Me.TabAutorizacion.Location = New System.Drawing.Point(4, 5)
        Me.TabAutorizacion.Name = "TabAutorizacion"
        Me.TabAutorizacion.Padding = New System.Windows.Forms.Padding(3)
        Me.TabAutorizacion.Size = New System.Drawing.Size(702, 411)
        Me.TabAutorizacion.TabIndex = 1
        Me.TabAutorizacion.Text = "Autorizacion"
        Me.TabAutorizacion.UseVisualStyleBackColor = True
        '
        'DtpNotificacion
        '
        Me.DtpNotificacion.Checked = False
        Me.DtpNotificacion.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.DtpNotificacion.Location = New System.Drawing.Point(20, 228)
        Me.DtpNotificacion.Name = "DtpNotificacion"
        Me.DtpNotificacion.ShowCheckBox = True
        Me.DtpNotificacion.Size = New System.Drawing.Size(200, 20)
        Me.DtpNotificacion.TabIndex = 8
        '
        'LblNotificacion
        '
        Me.LblNotificacion.AutoSize = True
        Me.LblNotificacion.Location = New System.Drawing.Point(20, 206)
        Me.LblNotificacion.Name = "LblNotificacion"
        Me.LblNotificacion.Size = New System.Drawing.Size(183, 13)
        Me.LblNotificacion.TabIndex = 7
        Me.LblNotificacion.Text = "Fecha notificación al juez (opcional):"
        '
        'DtpVencimiento
        '
        Me.DtpVencimiento.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.DtpVencimiento.Location = New System.Drawing.Point(200, 160)
        Me.DtpVencimiento.Name = "DtpVencimiento"
        Me.DtpVencimiento.Size = New System.Drawing.Size(150, 20)
        Me.DtpVencimiento.TabIndex = 6
        '
        'LblVencimiento
        '
        Me.LblVencimiento.AutoSize = True
        Me.LblVencimiento.Location = New System.Drawing.Point(200, 138)
        Me.LblVencimiento.Name = "LblVencimiento"
        Me.LblVencimiento.Size = New System.Drawing.Size(115, 13)
        Me.LblVencimiento.TabIndex = 5
        Me.LblVencimiento.Text = "Fecha de vencimiento:"
        '
        'DtpInicio
        '
        Me.DtpInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.DtpInicio.Location = New System.Drawing.Point(20, 160)
        Me.DtpInicio.Name = "DtpInicio"
        Me.DtpInicio.Size = New System.Drawing.Size(150, 20)
        Me.DtpInicio.TabIndex = 4
        '
        'LblInicio
        '
        Me.LblInicio.AutoSize = True
        Me.LblInicio.Location = New System.Drawing.Point(20, 138)
        Me.LblInicio.Name = "LblInicio"
        Me.LblInicio.Size = New System.Drawing.Size(82, 13)
        Me.LblInicio.TabIndex = 3
        Me.LblInicio.Text = "Fecha de inicio:"
        '
        'CboAutorizacion
        '
        Me.CboAutorizacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboAutorizacion.FormattingEnabled = True
        Me.CboAutorizacion.Location = New System.Drawing.Point(20, 94)
        Me.CboAutorizacion.Name = "CboAutorizacion"
        Me.CboAutorizacion.Size = New System.Drawing.Size(420, 21)
        Me.CboAutorizacion.TabIndex = 2
        '
        'LblAutorizacion
        '
        Me.LblAutorizacion.AutoSize = True
        Me.LblAutorizacion.Location = New System.Drawing.Point(20, 72)
        Me.LblAutorizacion.Name = "LblAutorizacion"
        Me.LblAutorizacion.Size = New System.Drawing.Size(106, 13)
        Me.LblAutorizacion.TabIndex = 1
        Me.LblAutorizacion.Text = "Tipo de autorización:"
        '
        'LblTituloPaso2
        '
        Me.LblTituloPaso2.AutoSize = True
        Me.LblTituloPaso2.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.LblTituloPaso2.Location = New System.Drawing.Point(20, 20)
        Me.LblTituloPaso2.Name = "LblTituloPaso2"
        Me.LblTituloPaso2.Size = New System.Drawing.Size(217, 19)
        Me.LblTituloPaso2.TabIndex = 0
        Me.LblTituloPaso2.Text = "Paso 2: Autorización y vigencia"
        '
        'TabDetalle
        '
        Me.TabDetalle.Controls.Add(Me.ChkActivo)
        Me.TabDetalle.Controls.Add(Me.TxtObservaciones)
        Me.TabDetalle.Controls.Add(Me.LblObs)
        Me.TabDetalle.Controls.Add(Me.TxtCustodia)
        Me.TabDetalle.Controls.Add(Me.LblCustodia)
        Me.TabDetalle.Controls.Add(Me.TxtHorario)
        Me.TabDetalle.Controls.Add(Me.LblHorario)
        Me.TabDetalle.Controls.Add(Me.TxtLugarTrabajo)
        Me.TabDetalle.Controls.Add(Me.LblLugar)
        Me.TabDetalle.Controls.Add(Me.LblTituloPaso3)
        Me.TabDetalle.Location = New System.Drawing.Point(4, 5)
        Me.TabDetalle.Name = "TabDetalle"
        Me.TabDetalle.Size = New System.Drawing.Size(702, 411)
        Me.TabDetalle.TabIndex = 2
        Me.TabDetalle.Text = "Detalle"
        Me.TabDetalle.UseVisualStyleBackColor = True
        '
        'ChkActivo
        '
        Me.ChkActivo.AutoSize = True
        Me.ChkActivo.Checked = True
        Me.ChkActivo.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkActivo.Location = New System.Drawing.Point(20, 380)
        Me.ChkActivo.Name = "ChkActivo"
        Me.ChkActivo.Size = New System.Drawing.Size(98, 17)
        Me.ChkActivo.TabIndex = 9
        Me.ChkActivo.Text = "Registro activo"
        Me.ChkActivo.UseVisualStyleBackColor = True
        '
        'TxtObservaciones
        '
        Me.TxtObservaciones.Location = New System.Drawing.Point(20, 300)
        Me.TxtObservaciones.Multiline = True
        Me.TxtObservaciones.Name = "TxtObservaciones"
        Me.TxtObservaciones.Size = New System.Drawing.Size(630, 70)
        Me.TxtObservaciones.TabIndex = 8
        '
        'LblObs
        '
        Me.LblObs.AutoSize = True
        Me.LblObs.Location = New System.Drawing.Point(20, 278)
        Me.LblObs.Name = "LblObs"
        Me.LblObs.Size = New System.Drawing.Size(136, 13)
        Me.LblObs.TabIndex = 7
        Me.LblObs.Text = "Observaciones adicionales:"
        '
        'TxtCustodia
        '
        Me.TxtCustodia.Location = New System.Drawing.Point(20, 208)
        Me.TxtCustodia.Multiline = True
        Me.TxtCustodia.Name = "TxtCustodia"
        Me.TxtCustodia.Size = New System.Drawing.Size(630, 60)
        Me.TxtCustodia.TabIndex = 6
        '
        'LblCustodia
        '
        Me.LblCustodia.AutoSize = True
        Me.LblCustodia.Location = New System.Drawing.Point(20, 186)
        Me.LblCustodia.Name = "LblCustodia"
        Me.LblCustodia.Size = New System.Drawing.Size(101, 13)
        Me.LblCustodia.TabIndex = 5
        Me.LblCustodia.Text = "Detalle de custodia:"
        '
        'TxtHorario
        '
        Me.TxtHorario.Location = New System.Drawing.Point(20, 146)
        Me.TxtHorario.Name = "TxtHorario"
        Me.TxtHorario.Size = New System.Drawing.Size(300, 20)
        Me.TxtHorario.TabIndex = 4
        '
        'LblHorario
        '
        Me.LblHorario.AutoSize = True
        Me.LblHorario.Location = New System.Drawing.Point(20, 124)
        Me.LblHorario.Name = "LblHorario"
        Me.LblHorario.Size = New System.Drawing.Size(95, 13)
        Me.LblHorario.TabIndex = 3
        Me.LblHorario.Text = "Horario autorizado:"
        '
        'TxtLugarTrabajo
        '
        Me.TxtLugarTrabajo.Location = New System.Drawing.Point(20, 84)
        Me.TxtLugarTrabajo.Name = "TxtLugarTrabajo"
        Me.TxtLugarTrabajo.Size = New System.Drawing.Size(630, 20)
        Me.TxtLugarTrabajo.TabIndex = 2
        '
        'LblLugar
        '
        Me.LblLugar.AutoSize = True
        Me.LblLugar.Location = New System.Drawing.Point(20, 62)
        Me.LblLugar.Name = "LblLugar"
        Me.LblLugar.Size = New System.Drawing.Size(89, 13)
        Me.LblLugar.TabIndex = 1
        Me.LblLugar.Text = "Lugar de trabajo:"
        '
        'LblTituloPaso3
        '
        Me.LblTituloPaso3.AutoSize = True
        Me.LblTituloPaso3.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.LblTituloPaso3.Location = New System.Drawing.Point(20, 20)
        Me.LblTituloPaso3.Name = "LblTituloPaso3"
        Me.LblTituloPaso3.Size = New System.Drawing.Size(222, 19)
        Me.LblTituloPaso3.TabIndex = 0
        Me.LblTituloPaso3.Text = "Paso 3: Detalle de salida laboral"
        '
        'TabResumen
        '
        Me.TabResumen.Controls.Add(Me.TxtResumen)
        Me.TabResumen.Controls.Add(Me.LblAyudaResumen)
        Me.TabResumen.Controls.Add(Me.LblTituloPaso4)
        Me.TabResumen.Location = New System.Drawing.Point(4, 5)
        Me.TabResumen.Name = "TabResumen"
        Me.TabResumen.Size = New System.Drawing.Size(702, 411)
        Me.TabResumen.TabIndex = 3
        Me.TabResumen.Text = "Resumen"
        Me.TabResumen.UseVisualStyleBackColor = True
        '
        'TxtResumen
        '
        Me.TxtResumen.Location = New System.Drawing.Point(20, 84)
        Me.TxtResumen.Multiline = True
        Me.TxtResumen.Name = "TxtResumen"
        Me.TxtResumen.ReadOnly = True
        Me.TxtResumen.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TxtResumen.Size = New System.Drawing.Size(630, 300)
        Me.TxtResumen.TabIndex = 2
        '
        'LblAyudaResumen
        '
        Me.LblAyudaResumen.AutoSize = True
        Me.LblAyudaResumen.ForeColor = System.Drawing.Color.DimGray
        Me.LblAyudaResumen.Location = New System.Drawing.Point(20, 52)
        Me.LblAyudaResumen.Name = "LblAyudaResumen"
        Me.LblAyudaResumen.Size = New System.Drawing.Size(182, 13)
        Me.LblAyudaResumen.TabIndex = 1
        Me.LblAyudaResumen.Text = "Revise los datos antes de registrar."
        '
        'LblTituloPaso4
        '
        Me.LblTituloPaso4.AutoSize = True
        Me.LblTituloPaso4.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.LblTituloPaso4.Location = New System.Drawing.Point(20, 20)
        Me.LblTituloPaso4.Name = "LblTituloPaso4"
        Me.LblTituloPaso4.Size = New System.Drawing.Size(189, 19)
        Me.LblTituloPaso4.TabIndex = 0
        Me.LblTituloPaso4.Text = "Paso 4: Confirmación final"
        '
        'BtnAtras
        '
        Me.BtnAtras.Location = New System.Drawing.Point(302, 476)
        Me.BtnAtras.Name = "BtnAtras"
        Me.BtnAtras.Size = New System.Drawing.Size(100, 34)
        Me.BtnAtras.TabIndex = 2
        Me.BtnAtras.Text = "Atrás"
        Me.BtnAtras.UseVisualStyleBackColor = True
        '
        'BtnSiguiente
        '
        Me.BtnSiguiente.Location = New System.Drawing.Point(410, 476)
        Me.BtnSiguiente.Name = "BtnSiguiente"
        Me.BtnSiguiente.Size = New System.Drawing.Size(100, 34)
        Me.BtnSiguiente.TabIndex = 3
        Me.BtnSiguiente.Text = "Siguiente"
        Me.BtnSiguiente.UseVisualStyleBackColor = True
        '
        'BtnGuardar
        '
        Me.BtnGuardar.BackColor = System.Drawing.Color.ForestGreen
        Me.BtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnGuardar.ForeColor = System.Drawing.Color.White
        Me.BtnGuardar.Location = New System.Drawing.Point(518, 476)
        Me.BtnGuardar.Name = "BtnGuardar"
        Me.BtnGuardar.Size = New System.Drawing.Size(100, 34)
        Me.BtnGuardar.TabIndex = 4
        Me.BtnGuardar.Text = "Registrar"
        Me.BtnGuardar.UseVisualStyleBackColor = False
        '
        'BtnCancelar
        '
        Me.BtnCancelar.Location = New System.Drawing.Point(626, 476)
        Me.BtnCancelar.Name = "BtnCancelar"
        Me.BtnCancelar.Size = New System.Drawing.Size(100, 34)
        Me.BtnCancelar.TabIndex = 5
        Me.BtnCancelar.Text = "Cancelar"
        Me.BtnCancelar.UseVisualStyleBackColor = True
        '
        'frmNuevaSalidaArt120Wizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(760, 560)
        Me.Controls.Add(Me.BtnCancelar)
        Me.Controls.Add(Me.BtnGuardar)
        Me.Controls.Add(Me.BtnSiguiente)
        Me.Controls.Add(Me.BtnAtras)
        Me.Controls.Add(Me.Tabs)
        Me.Controls.Add(Me.LblPaso)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNuevaSalidaArt120Wizard"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Nueva salida laboral - Art. 120"
        Me.Tabs.ResumeLayout(False)
        Me.TabRecluso.ResumeLayout(False)
        Me.TabRecluso.PerformLayout()
        Me.TabAutorizacion.ResumeLayout(False)
        Me.TabAutorizacion.PerformLayout()
        Me.TabDetalle.ResumeLayout(False)
        Me.TabDetalle.PerformLayout()
        Me.TabResumen.ResumeLayout(False)
        Me.TabResumen.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LblPaso As Label
    Friend WithEvents Tabs As TabControl
    Friend WithEvents TabRecluso As TabPage
    Friend WithEvents LblTituloPaso1 As Label
    Friend WithEvents TabAutorizacion As TabPage
    Friend WithEvents TabDetalle As TabPage
    Friend WithEvents TabResumen As TabPage
    Friend WithEvents BtnAtras As Button
    Friend WithEvents BtnSiguiente As Button
    Friend WithEvents BtnGuardar As Button
    Friend WithEvents BtnCancelar As Button
    Friend WithEvents LblAyudaRecluso As Label
    Friend WithEvents LblIdRecluso As Label
    Friend WithEvents BtnBuscarRecluso As Button
    Friend WithEvents TxtRecluso As TextBox
    Friend WithEvents LblReclusoTitulo As Label
    Friend WithEvents LblTituloPaso2 As Label
    Friend WithEvents DtpNotificacion As DateTimePicker
    Friend WithEvents LblNotificacion As Label
    Friend WithEvents DtpVencimiento As DateTimePicker
    Friend WithEvents LblVencimiento As Label
    Friend WithEvents DtpInicio As DateTimePicker
    Friend WithEvents LblInicio As Label
    Friend WithEvents CboAutorizacion As ComboBox
    Friend WithEvents LblAutorizacion As Label
    Friend WithEvents LblTituloPaso3 As Label
    Friend WithEvents ChkActivo As CheckBox
    Friend WithEvents TxtObservaciones As TextBox
    Friend WithEvents LblObs As Label
    Friend WithEvents TxtCustodia As TextBox
    Friend WithEvents LblCustodia As Label
    Friend WithEvents TxtHorario As TextBox
    Friend WithEvents LblHorario As Label
    Friend WithEvents TxtLugarTrabajo As TextBox
    Friend WithEvents LblLugar As Label
    Friend WithEvents TxtResumen As TextBox
    Friend WithEvents LblAyudaResumen As Label
    Friend WithEvents LblTituloPaso4 As Label
End Class