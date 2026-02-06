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
        Me.grpNuevo.Location = New System.Drawing.Point(18, 18)
        Me.grpNuevo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpNuevo.Name = "grpNuevo"
        Me.grpNuevo.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpNuevo.Size = New System.Drawing.Size(375, 566)
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
        Me.btnGuardar.Location = New System.Drawing.Point(27, 392)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(322, 62)
        Me.btnGuardar.TabIndex = 9
        Me.btnGuardar.Text = "GUARDAR USUARIO"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'cmbRol
        '
        Me.cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRol.FormattingEnabled = True
        Me.cmbRol.Items.AddRange(New Object() {"OPERADOR", "ADMINISTRADOR", "CONSULTA"})
        Me.cmbRol.Location = New System.Drawing.Point(27, 331)
        Me.cmbRol.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmbRol.Name = "cmbRol"
        Me.cmbRol.Size = New System.Drawing.Size(320, 33)
        Me.cmbRol.TabIndex = 8
        '
        'lblRol
        '
        Me.lblRol.AutoSize = True
        Me.lblRol.Location = New System.Drawing.Point(22, 303)
        Me.lblRol.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRol.Name = "lblRol"
        Me.lblRol.Size = New System.Drawing.Size(41, 25)
        Me.lblRol.TabIndex = 7
        Me.lblRol.Text = "Rol:"
        '
        'txtClave
        '
        Me.txtClave.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtClave.Location = New System.Drawing.Point(27, 246)
        Me.txtClave.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtClave.Name = "txtClave"
        Me.txtClave.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtClave.Size = New System.Drawing.Size(320, 31)
        Me.txtClave.TabIndex = 6
        '
        'lblClave
        '
        Me.lblClave.AutoSize = True
        Me.lblClave.Location = New System.Drawing.Point(22, 218)
        Me.lblClave.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblClave.Name = "lblClave"
        Me.lblClave.Size = New System.Drawing.Size(105, 25)
        Me.lblClave.TabIndex = 5
        Me.lblClave.Text = "Contraseña:"
        '
        'txtLogin
        '
        Me.txtLogin.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLogin.Location = New System.Drawing.Point(27, 162)
        Me.txtLogin.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtLogin.Name = "txtLogin"
        Me.txtLogin.Size = New System.Drawing.Size(320, 31)
        Me.txtLogin.TabIndex = 4
        '
        'lblLogin
        '
        Me.lblLogin.AutoSize = True
        Me.lblLogin.Location = New System.Drawing.Point(22, 134)
        Me.lblLogin.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblLogin.Name = "lblLogin"
        Me.lblLogin.Size = New System.Drawing.Size(137, 25)
        Me.lblLogin.TabIndex = 3
        Me.lblLogin.Text = "Usuario / Login:"
        '
        'txtNombre
        '
        Me.txtNombre.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNombre.Location = New System.Drawing.Point(27, 77)
        Me.txtNombre.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(320, 31)
        Me.txtNombre.TabIndex = 2
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Location = New System.Drawing.Point(22, 49)
        Me.lblNombre.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(166, 25)
        Me.lblNombre.TabIndex = 1
        Me.lblNombre.Text = "Nombre Completo:"
        '
        'lblModoEdicion
        '
        Me.lblModoEdicion.AutoSize = True
        Me.lblModoEdicion.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic)
        Me.lblModoEdicion.Location = New System.Drawing.Point(27, 462)
        Me.lblModoEdicion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblModoEdicion.Name = "lblModoEdicion"
        Me.lblModoEdicion.Size = New System.Drawing.Size(85, 21)
        Me.lblModoEdicion.TabIndex = 11
        Me.lblModoEdicion.Text = "Modo: Alta"
        '
        'btnCancelar
        '
        Me.btnCancelar.BackColor = System.Drawing.Color.DimGray
        Me.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancelar.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.ForeColor = System.Drawing.Color.White
        Me.btnCancelar.Location = New System.Drawing.Point(27, 489)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(322, 46)
        Me.btnCancelar.TabIndex = 10
        Me.btnCancelar.Text = "CANCELAR EDICIÓN"
        Me.btnCancelar.UseVisualStyleBackColor = False
        '
        'dgvUsuarios
        '
        Me.dgvUsuarios.AllowUserToAddRows = False
        Me.dgvUsuarios.AllowUserToDeleteRows = False
        Me.dgvUsuarios.AllowUserToResizeColumns = False
        Me.dgvUsuarios.AllowUserToResizeRows = False
        Me.dgvUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvUsuarios.BackgroundColor = System.Drawing.Color.White
        Me.dgvUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUsuarios.Location = New System.Drawing.Point(420, 77)
        Me.dgvUsuarios.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvUsuarios.Name = "dgvUsuarios"
        Me.dgvUsuarios.ReadOnly = True
        Me.dgvUsuarios.RowHeadersWidth = 62
        Me.dgvUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvUsuarios.Size = New System.Drawing.Size(645, 354)
        Me.dgvUsuarios.TabIndex = 1
        '
        'btnEliminar
        '
        Me.btnEliminar.BackColor = System.Drawing.Color.IndianRed
        Me.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEliminar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnEliminar.ForeColor = System.Drawing.Color.White
        Me.btnEliminar.Location = New System.Drawing.Point(840, 443)
        Me.btnEliminar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(225, 52)
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
        Me.btnEditar.Location = New System.Drawing.Point(600, 443)
        Me.btnEditar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(225, 52)
        Me.btnEditar.TabIndex = 4
        Me.btnEditar.Text = "Editar Seleccionado"
        Me.btnEditar.UseVisualStyleBackColor = False
        '
        'lblTituloLista
        '
        Me.lblTituloLista.AutoSize = True
        Me.lblTituloLista.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTituloLista.Location = New System.Drawing.Point(414, 40)
        Me.lblTituloLista.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTituloLista.Name = "lblTituloLista"
        Me.lblTituloLista.Size = New System.Drawing.Size(180, 28)
        Me.lblTituloLista.TabIndex = 2
        Me.lblTituloLista.Text = "Usuarios Actuales"
        '
        'frmUsuarios
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1095, 600)
        Me.Controls.Add(Me.btnEditar)
        Me.Controls.Add(Me.btnEliminar)
        Me.Controls.Add(Me.lblTituloLista)
        Me.Controls.Add(Me.dgvUsuarios)
        Me.Controls.Add(Me.grpNuevo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
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
