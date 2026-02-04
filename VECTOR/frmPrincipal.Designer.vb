<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPrincipal
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.SistemaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SalirToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GestiónToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BandejaDeEntradaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GestionRangosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GestionUsuariosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AuditoriaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UnificarOficinasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem() ' <--- NUEVO
        Me.VentanasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblEstadoUsuario = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblEstadoOficina = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SistemaToolStripMenuItem, Me.GestiónToolStripMenuItem, Me.VentanasToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.MdiWindowListItem = Me.VentanasToolStripMenuItem
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(9, 3, 0, 3)
        Me.MenuStrip1.Size = New System.Drawing.Size(1512, 35)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'SistemaToolStripMenuItem
        '
        Me.SistemaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SalirToolStripMenuItem})
        Me.SistemaToolStripMenuItem.Name = "SistemaToolStripMenuItem"
        Me.SistemaToolStripMenuItem.Size = New System.Drawing.Size(90, 29)
        Me.SistemaToolStripMenuItem.Text = "Sistema"
        '
        'SalirToolStripMenuItem
        '
        Me.SalirToolStripMenuItem.Name = "SalirToolStripMenuItem"
        Me.SalirToolStripMenuItem.Size = New System.Drawing.Size(147, 34)
        Me.SalirToolStripMenuItem.Text = "Salir"
        '
        'GestiónToolStripMenuItem
        '
        Me.GestiónToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BandejaDeEntradaToolStripMenuItem, Me.GestionRangosToolStripMenuItem, Me.GestionUsuariosToolStripMenuItem, Me.AuditoriaToolStripMenuItem, Me.UnificarOficinasToolStripMenuItem})
        Me.GestiónToolStripMenuItem.Name = "GestiónToolStripMenuItem"
        Me.GestiónToolStripMenuItem.Size = New System.Drawing.Size(88, 29)
        Me.GestiónToolStripMenuItem.Text = "Gestión"
        '
        'BandejaDeEntradaToolStripMenuItem
        '
        Me.BandejaDeEntradaToolStripMenuItem.Name = "BandejaDeEntradaToolStripMenuItem"
        Me.BandejaDeEntradaToolStripMenuItem.Size = New System.Drawing.Size(345, 34)
        Me.BandejaDeEntradaToolStripMenuItem.Text = "Bandeja de Entrada"
        '
        'GestionRangosToolStripMenuItem
        '
        Me.GestionRangosToolStripMenuItem.Name = "GestionRangosToolStripMenuItem"
        Me.GestionRangosToolStripMenuItem.Size = New System.Drawing.Size(345, 34)
        Me.GestionRangosToolStripMenuItem.Text = "Numeración y Rangos"
        '
        'GestionUsuariosToolStripMenuItem
        '
        Me.GestionUsuariosToolStripMenuItem.Name = "GestionUsuariosToolStripMenuItem"
        Me.GestionUsuariosToolStripMenuItem.Size = New System.Drawing.Size(345, 34)
        Me.GestionUsuariosToolStripMenuItem.Text = "Gestión de Usuarios"
        '
        'AuditoriaToolStripMenuItem
        '
        Me.AuditoriaToolStripMenuItem.Name = "AuditoriaToolStripMenuItem"
        Me.AuditoriaToolStripMenuItem.Size = New System.Drawing.Size(345, 34)
        Me.AuditoriaToolStripMenuItem.Text = "Auditoría"
        '
        'UnificarOficinasToolStripMenuItem
        '
        Me.UnificarOficinasToolStripMenuItem.Name = "UnificarOficinasToolStripMenuItem"
        Me.UnificarOficinasToolStripMenuItem.Size = New System.Drawing.Size(345, 34)
        Me.UnificarOficinasToolStripMenuItem.Text = "Herramienta Unificar Oficinas"
        '
        'VentanasToolStripMenuItem
        '
        Me.VentanasToolStripMenuItem.Name = "VentanasToolStripMenuItem"
        Me.VentanasToolStripMenuItem.Size = New System.Drawing.Size(99, 29)
        Me.VentanasToolStripMenuItem.Text = "Ventanas"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblEstadoUsuario, Me.lblEstadoOficina})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 1018)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(2, 0, 21, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(1512, 32)
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lblEstadoUsuario
        '
        Me.lblEstadoUsuario.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblEstadoUsuario.Name = "lblEstadoUsuario"
        Me.lblEstadoUsuario.Size = New System.Drawing.Size(87, 25)
        Me.lblEstadoUsuario.Text = "Usuario: "
        '
        'lblEstadoOficina
        '
        Me.lblEstadoOficina.Name = "lblEstadoOficina"
        Me.lblEstadoOficina.Size = New System.Drawing.Size(85, 25)
        Me.lblEstadoOficina.Text = "| Oficina: "
        '
        'frmPrincipal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1512, 1050)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmPrincipal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "VECTOR - Sistema de Gestión de Expedientes"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents SistemaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SalirToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GestiónToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BandejaDeEntradaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GestionRangosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GestionUsuariosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AuditoriaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UnificarOficinasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem ' <--- NUEVO
    Friend WithEvents VentanasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents lblEstadoUsuario As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblEstadoOficina As System.Windows.Forms.ToolStripStatusLabel
End Class
