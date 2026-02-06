<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLogin
    Inherits System.Windows.Forms.Form

    ' Desecha los componentes del formulario
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

    ' Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    ' NOTA: El siguiente procedimiento es requerido por el Diseñador de Windows Forms
    ' Se puede modificar usando el Diseñador de Windows Forms.  
    ' No lo modifique usando el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblUsuario = New System.Windows.Forms.Label()
        Me.txtUsuario = New System.Windows.Forms.TextBox()
        Me.lblClave = New System.Windows.Forms.Label()
        Me.txtClave = New System.Windows.Forms.TextBox()
        Me.btnIngresar = New System.Windows.Forms.Button()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.pnlFondo = New System.Windows.Forms.Panel()
        Me.pnlFondo.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.ForeColor = System.Drawing.Color.SteelBlue
        Me.lblTitulo.Location = New System.Drawing.Point(128, 31)
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(269, 45)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "VECTOR - LOGIN"
        '
        'lblUsuario
        '
        Me.lblUsuario.AutoSize = True
        Me.lblUsuario.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblUsuario.Location = New System.Drawing.Point(45, 108)
        Me.lblUsuario.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblUsuario.Name = "lblUsuario"
        Me.lblUsuario.Size = New System.Drawing.Size(83, 28)
        Me.lblUsuario.TabIndex = 1
        Me.lblUsuario.Text = "Usuario:"
        '
        'txtUsuario
        '
        Me.txtUsuario.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtUsuario.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtUsuario.Location = New System.Drawing.Point(51, 142)
        Me.txtUsuario.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtUsuario.Name = "txtUsuario"
        Me.txtUsuario.Size = New System.Drawing.Size(433, 34)
        Me.txtUsuario.TabIndex = 0
        '
        'lblClave
        '
        Me.lblClave.AutoSize = True
        Me.lblClave.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblClave.Location = New System.Drawing.Point(45, 200)
        Me.lblClave.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblClave.Name = "lblClave"
        Me.lblClave.Size = New System.Drawing.Size(114, 28)
        Me.lblClave.TabIndex = 3
        Me.lblClave.Text = "Contraseña:"
        '
        'txtClave
        '
        Me.txtClave.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtClave.Location = New System.Drawing.Point(51, 234)
        Me.txtClave.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtClave.Name = "txtClave"
        Me.txtClave.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtClave.Size = New System.Drawing.Size(433, 34)
        Me.txtClave.TabIndex = 1
        '
        'btnIngresar
        '
        Me.btnIngresar.BackColor = System.Drawing.Color.SteelBlue
        Me.btnIngresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnIngresar.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnIngresar.ForeColor = System.Drawing.Color.White
        Me.btnIngresar.Location = New System.Drawing.Point(51, 308)
        Me.btnIngresar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnIngresar.Name = "btnIngresar"
        Me.btnIngresar.Size = New System.Drawing.Size(270, 62)
        Me.btnIngresar.TabIndex = 2
        Me.btnIngresar.Text = "INGRESAR"
        Me.btnIngresar.UseVisualStyleBackColor = False
        '
        'btnSalir
        '
        Me.btnSalir.BackColor = System.Drawing.Color.IndianRed
        Me.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSalir.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnSalir.ForeColor = System.Drawing.Color.White
        Me.btnSalir.Location = New System.Drawing.Point(336, 308)
        Me.btnSalir.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(150, 62)
        Me.btnSalir.TabIndex = 3
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.UseVisualStyleBackColor = False
        '
        'pnlFondo
        '
        Me.pnlFondo.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlFondo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlFondo.Controls.Add(Me.btnSalir)
        Me.pnlFondo.Controls.Add(Me.btnIngresar)
        Me.pnlFondo.Controls.Add(Me.txtClave)
        Me.pnlFondo.Controls.Add(Me.lblClave)
        Me.pnlFondo.Controls.Add(Me.txtUsuario)
        Me.pnlFondo.Controls.Add(Me.lblUsuario)
        Me.pnlFondo.Controls.Add(Me.lblTitulo)
        Me.pnlFondo.Location = New System.Drawing.Point(18, 18)
        Me.pnlFondo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlFondo.Name = "pnlFondo"
        Me.pnlFondo.Size = New System.Drawing.Size(539, 430)
        Me.pnlFondo.TabIndex = 0
        '
        'frmLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(576, 471)
        Me.Controls.Add(Me.pnlFondo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmLogin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "VECTOR - Iniciar Sesión"
        Me.pnlFondo.ResumeLayout(False)
        Me.pnlFondo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlFondo As System.Windows.Forms.Panel
    Friend WithEvents lblTitulo As System.Windows.Forms.Label
    Friend WithEvents lblUsuario As System.Windows.Forms.Label
    Friend WithEvents txtUsuario As System.Windows.Forms.TextBox
    Friend WithEvents lblClave As System.Windows.Forms.Label
    Friend WithEvents txtClave As System.Windows.Forms.TextBox
    Friend WithEvents btnIngresar As System.Windows.Forms.Button
    Friend WithEvents btnSalir As System.Windows.Forms.Button
End Class