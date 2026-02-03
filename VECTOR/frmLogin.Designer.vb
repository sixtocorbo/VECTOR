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
        Me.pnlFondo.Location = New System.Drawing.Point(12, 12)
        Me.pnlFondo.Name = "pnlFondo"
        Me.pnlFondo.Size = New System.Drawing.Size(360, 280)
        Me.pnlFondo.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.ForeColor = System.Drawing.Color.SteelBlue
        Me.lblTitulo.Location = New System.Drawing.Point(85, 20)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(190, 30)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "VECTOR - LOGIN"
        '
        'lblUsuario
        '
        Me.lblUsuario.AutoSize = True
        Me.lblUsuario.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblUsuario.Location = New System.Drawing.Point(30, 70)
        Me.lblUsuario.Name = "lblUsuario"
        Me.lblUsuario.Size = New System.Drawing.Size(59, 19)
        Me.lblUsuario.TabIndex = 1
        Me.lblUsuario.Text = "Usuario:"
        '
        'txtUsuario
        '
        Me.txtUsuario.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtUsuario.Location = New System.Drawing.Point(34, 92)
        Me.txtUsuario.Name = "txtUsuario"
        Me.txtUsuario.Size = New System.Drawing.Size(290, 25)
        Me.txtUsuario.TabIndex = 0
        '
        'lblClave
        '
        Me.lblClave.AutoSize = True
        Me.lblClave.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblClave.Location = New System.Drawing.Point(30, 130)
        Me.lblClave.Name = "lblClave"
        Me.lblClave.Size = New System.Drawing.Size(82, 19)
        Me.lblClave.TabIndex = 3
        Me.lblClave.Text = "Contraseña:"
        '
        'txtClave
        '
        Me.txtClave.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtClave.Location = New System.Drawing.Point(34, 152)
        Me.txtClave.Name = "txtClave"
        Me.txtClave.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtClave.Size = New System.Drawing.Size(290, 25)
        Me.txtClave.TabIndex = 1
        '
        'btnIngresar
        '
        Me.btnIngresar.BackColor = System.Drawing.Color.SteelBlue
        Me.btnIngresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnIngresar.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnIngresar.ForeColor = System.Drawing.Color.White
        Me.btnIngresar.Location = New System.Drawing.Point(34, 200)
        Me.btnIngresar.Name = "btnIngresar"
        Me.btnIngresar.Size = New System.Drawing.Size(180, 40)
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
        Me.btnSalir.Location = New System.Drawing.Point(224, 200)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(100, 40)
        Me.btnSalir.TabIndex = 3
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.UseVisualStyleBackColor = False
        '
        'frmLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(384, 306)
        Me.Controls.Add(Me.pnlFondo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
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