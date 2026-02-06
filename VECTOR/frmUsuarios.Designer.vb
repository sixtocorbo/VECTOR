<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUsuarios
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
        Me.grpNuevo = New System.Windows.Forms.GroupBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.cmbRol = New System.Windows.Forms.ComboBox()
        Me.lblRol = New System.Windows.Forms.Label()
        Me.txtClave = New System.Windows.Forms.TextBox()
        Me.lblClave = New System.Windows.Forms.Label()
        Me.txtLogin = New System.Windows.Forms.TextBox()
        Me.lblLogin = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.lblModoEdicion = New System.Windows.Forms.Label()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.dgvUsuarios = New System.Windows.Forms.DataGridView()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.btnEditar = New System.Windows.Forms.Button()
        Me.lblTituloLista = New System.Windows.Forms.Label()
        Me.grpNuevo.SuspendLayout()
        CType(Me.dgvUsuarios, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpNuevo
        '
        Me.grpNuevo.Controls.Add(Me.btnGuardar)
        Me.grpNuevo.Controls.Add(Me.cmbRol)
        Me.grpNuevo.Controls.Add(Me.lblRol)
        Me.grpNuevo.Controls.Add(Me.txtClave)
        Me.grpNuevo.Controls.Add(Me.lblClave)
        Me.grpNuevo.Controls.Add(Me.txtLogin)
        Me.grpNuevo.Controls.Add(Me.lblLogin)
        Me.grpNuevo.Controls.Add(Me.txtNombre)
        Me.grpNuevo.Controls.Add(Me.lblNombre)
        Me.grpNuevo.Controls.Add(Me.lblModoEdicion)
        Me.grpNuevo.Controls.Add(Me.btnCancelar)
        Me.grpNuevo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.grpNuevo.Location = New System.Drawing.Point(12, 12)
        Me.grpNuevo.Name = "grpNuevo"
        Me.grpNuevo.Size = New System.Drawing.Size(250, 368)
        Me.grpNuevo.TabIndex = 0
        Me.grpNuevo.TabStop = False
        Me.grpNuevo.Text = "Datos de Usuario"
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SeaGreen
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(18, 255)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(215, 40)
        Me.btnGuardar.TabIndex = 9
        Me.btnGuardar.Text = "GUARDAR USUARIO"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'cmbRol
        '
        Me.cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRol.FormattingEnabled = True
        Me.cmbRol.Items.AddRange(New Object() {"OPERADOR", "ADMINISTRADOR", "CONSULTA"})
        Me.cmbRol.Location = New System.Drawing.Point(18, 215)
        Me.cmbRol.Name = "cmbRol"
        Me.cmbRol.Size = New System.Drawing.Size(215, 23)
        Me.cmbRol.TabIndex = 8
        '
        'lblRol
        '
        Me.lblRol.AutoSize = True
        Me.lblRol.Location = New System.Drawing.Point(15, 197)
        Me.lblRol.Name = "lblRol"
        Me.lblRol.Size = New System.Drawing.Size(27, 15)
        Me.lblRol.TabIndex = 7
        Me.lblRol.Text = "Rol:"
        '
        'txtClave
        '
        Me.txtClave.Location = New System.Drawing.Point(18, 160)
        Me.txtClave.Name = "txtClave"
        Me.txtClave.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtClave.Size = New System.Drawing.Size(215, 23)
        Me.txtClave.TabIndex = 6
        '
        'lblClave
        '
        Me.lblClave.AutoSize = True
        Me.lblClave.Location = New System.Drawing.Point(15, 142)
        Me.lblClave.Name = "lblClave"
        Me.lblClave.Size = New System.Drawing.Size(70, 15)
        Me.lblClave.TabIndex = 5
        Me.lblClave.Text = "Contraseña:"
        '
        'txtLogin
        '
        Me.txtLogin.Location = New System.Drawing.Point(18, 105)
        Me.txtLogin.Name = "txtLogin"
        Me.txtLogin.Size = New System.Drawing.Size(215, 23)
        Me.txtLogin.TabIndex = 4
        '
        'lblLogin
        '
        Me.lblLogin.AutoSize = True
        Me.lblLogin.Location = New System.Drawing.Point(15, 87)
        Me.lblLogin.Name = "lblLogin"
        Me.lblLogin.Size = New System.Drawing.Size(95, 15)
        Me.lblLogin.TabIndex = 3
        Me.lblLogin.Text = "Usuario / Login:"
        '
        'txtNombre
        '
        Me.txtNombre.Location = New System.Drawing.Point(18, 50)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(215, 23)
        Me.txtNombre.TabIndex = 2
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Location = New System.Drawing.Point(15, 32)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(108, 15)
        Me.lblNombre.TabIndex = 1
        Me.lblNombre.Text = "Nombre Completo:"
        '
        'lblModoEdicion
        '
        Me.lblModoEdicion.AutoSize = True
        Me.lblModoEdicion.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic)
        Me.lblModoEdicion.Location = New System.Drawing.Point(18, 300)
        Me.lblModoEdicion.Name = "lblModoEdicion"
        Me.lblModoEdicion.Size = New System.Drawing.Size(66, 13)
        Me.lblModoEdicion.TabIndex = 11
        Me.lblModoEdicion.Text = "Modo: Alta"
        '
        'btnCancelar
        '
        Me.btnCancelar.BackColor = System.Drawing.Color.DimGray
        Me.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancelar.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.ForeColor = System.Drawing.Color.White
        Me.btnCancelar.Location = New System.Drawing.Point(18, 318)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(215, 30)
        Me.btnCancelar.TabIndex = 10
        Me.btnCancelar.Text = "CANCELAR EDICIÓN"
        Me.btnCancelar.UseVisualStyleBackColor = False
        '
        'dgvUsuarios
        '
        Me.dgvUsuarios.AllowUserToAddRows = False
        Me.dgvUsuarios.AllowUserToDeleteRows = False
        Me.dgvUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvUsuarios.BackgroundColor = System.Drawing.Color.White
        Me.dgvUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUsuarios.Location = New System.Drawing.Point(280, 50)
        Me.dgvUsuarios.Name = "dgvUsuarios"
        Me.dgvUsuarios.ReadOnly = True
        Me.dgvUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvUsuarios.Size = New System.Drawing.Size(430, 230)
        Me.dgvUsuarios.TabIndex = 1
        '
        'btnEliminar
        '
        Me.btnEliminar.BackColor = System.Drawing.Color.IndianRed
        Me.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEliminar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnEliminar.ForeColor = System.Drawing.Color.White
        Me.btnEliminar.Location = New System.Drawing.Point(560, 288)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(150, 34)
        Me.btnEliminar.TabIndex = 3
        Me.btnEliminar.Text = "Eliminar Seleccionado"
        Me.btnEliminar.UseVisualStyleBackColor = False
        '
        'btnEditar
        '
        Me.btnEditar.BackColor = System.Drawing.Color.SteelBlue
        Me.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEditar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnEditar.ForeColor = System.Drawing.Color.White
        Me.btnEditar.Location = New System.Drawing.Point(400, 288)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(150, 34)
        Me.btnEditar.TabIndex = 4
        Me.btnEditar.Text = "Editar Seleccionado"
        Me.btnEditar.UseVisualStyleBackColor = False
        '
        'lblTituloLista
        '
        Me.lblTituloLista.AutoSize = True
        Me.lblTituloLista.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTituloLista.Location = New System.Drawing.Point(276, 26)
        Me.lblTituloLista.Name = "lblTituloLista"
        Me.lblTituloLista.Size = New System.Drawing.Size(127, 19)
        Me.lblTituloLista.TabIndex = 2
        Me.lblTituloLista.Text = "Usuarios Actuales"
        '
        'frmUsuarios
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(730, 390)
        Me.Controls.Add(Me.btnEditar)
        Me.Controls.Add(Me.btnEliminar)
        Me.Controls.Add(Me.lblTituloLista)
        Me.Controls.Add(Me.dgvUsuarios)
        Me.Controls.Add(Me.grpNuevo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmUsuarios"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de Usuarios y Seguridad - VECTOR"
        Me.grpNuevo.ResumeLayout(False)
        Me.grpNuevo.PerformLayout()
        CType(Me.dgvUsuarios, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents grpNuevo As System.Windows.Forms.GroupBox
    Friend WithEvents lblNombre As System.Windows.Forms.Label
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents lblLogin As System.Windows.Forms.Label
    Friend WithEvents txtLogin As System.Windows.Forms.TextBox
    Friend WithEvents lblClave As System.Windows.Forms.Label
    Friend WithEvents txtClave As System.Windows.Forms.TextBox
    Friend WithEvents lblRol As System.Windows.Forms.Label
    Friend WithEvents cmbRol As System.Windows.Forms.ComboBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents lblModoEdicion As System.Windows.Forms.Label
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents dgvUsuarios As System.Windows.Forms.DataGridView
    Friend WithEvents lblTituloLista As System.Windows.Forms.Label
    Friend WithEvents btnEliminar As System.Windows.Forms.Button
    Friend WithEvents btnEditar As System.Windows.Forms.Button
End Class
