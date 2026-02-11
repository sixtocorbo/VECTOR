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
        Me.MenuInicio = New System.Windows.Forms.ToolStripMenuItem()
        Me.BandejaDeEntradaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BuscadorReclusosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.SalirToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuControl = New System.Windows.Forms.ToolStripMenuItem()
        Me.EstadisticasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AuditoriaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuAdministracion = New System.Windows.Forms.ToolStripMenuItem()
        Me.GestionUsuariosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GestionRangosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UnificarOficinasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSistema = New System.Windows.Forms.ToolStripMenuItem()
        Me.GestionTiposDocumentoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GestionTiemposToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfiguracionSistemaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblEstadoUsuario = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblEstadoOficina = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.GripMargin = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuInicio, Me.MenuControl, Me.MenuAdministracion, Me.MenuSistema})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1512, 35)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'MenuInicio
        '
        Me.MenuInicio.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BandejaDeEntradaToolStripMenuItem, Me.BuscadorReclusosToolStripMenuItem, Me.ToolStripSeparator1, Me.SalirToolStripMenuItem})
        Me.MenuInicio.Name = "MenuInicio"
        Me.MenuInicio.Size = New System.Drawing.Size(70, 29)
        Me.MenuInicio.Text = "Inicio"
        '
        'BandejaDeEntradaToolStripMenuItem
        '
        Me.BandejaDeEntradaToolStripMenuItem.Name = "BandejaDeEntradaToolStripMenuItem"
        Me.BandejaDeEntradaToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B), System.Windows.Forms.Keys)
        Me.BandejaDeEntradaToolStripMenuItem.Size = New System.Drawing.Size(327, 34)
        Me.BandejaDeEntradaToolStripMenuItem.Text = "Bandeja de Entrada"
        '
        'BuscadorReclusosToolStripMenuItem
        '
        Me.BuscadorReclusosToolStripMenuItem.Name = "BuscadorReclusosToolStripMenuItem"
        Me.BuscadorReclusosToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.BuscadorReclusosToolStripMenuItem.Size = New System.Drawing.Size(327, 34)
        Me.BuscadorReclusosToolStripMenuItem.Text = "Buscar Reclusos"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(324, 6)
        '
        'SalirToolStripMenuItem
        '
        Me.SalirToolStripMenuItem.Name = "SalirToolStripMenuItem"
        Me.SalirToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.SalirToolStripMenuItem.Size = New System.Drawing.Size(327, 34)
        Me.SalirToolStripMenuItem.Text = "Salir"
        '
        'MenuControl
        '
        Me.MenuControl.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EstadisticasToolStripMenuItem, Me.AuditoriaToolStripMenuItem})
        Me.MenuControl.Name = "MenuControl"
        Me.MenuControl.Size = New System.Drawing.Size(87, 29)
        Me.MenuControl.Text = "Control"
        '
        'EstadisticasToolStripMenuItem
        '
        Me.EstadisticasToolStripMenuItem.Name = "EstadisticasToolStripMenuItem"
        Me.EstadisticasToolStripMenuItem.Size = New System.Drawing.Size(279, 34)
        Me.EstadisticasToolStripMenuItem.Text = "Estadísticas"
        '
        'AuditoriaToolStripMenuItem
        '
        Me.AuditoriaToolStripMenuItem.Name = "AuditoriaToolStripMenuItem"
        Me.AuditoriaToolStripMenuItem.Size = New System.Drawing.Size(279, 34)
        Me.AuditoriaToolStripMenuItem.Text = "Auditoría de Eventos"
        '
        'MenuAdministracion
        '
        Me.MenuAdministracion.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GestionUsuariosToolStripMenuItem, Me.GestionRangosToolStripMenuItem, Me.UnificarOficinasToolStripMenuItem})
        Me.MenuAdministracion.Name = "MenuAdministracion"
        Me.MenuAdministracion.Size = New System.Drawing.Size(147, 29)
        Me.MenuAdministracion.Text = "Administración"
        '
        'GestionUsuariosToolStripMenuItem
        '
        Me.GestionUsuariosToolStripMenuItem.Name = "GestionUsuariosToolStripMenuItem"
        Me.GestionUsuariosToolStripMenuItem.Size = New System.Drawing.Size(270, 34)
        Me.GestionUsuariosToolStripMenuItem.Text = "Usuarios"
        '
        'GestionRangosToolStripMenuItem
        '
        Me.GestionRangosToolStripMenuItem.Name = "GestionRangosToolStripMenuItem"
        Me.GestionRangosToolStripMenuItem.Size = New System.Drawing.Size(343, 34)
        Me.GestionRangosToolStripMenuItem.Text = "Rangos Documentos"
        '
        'UnificarOficinasToolStripMenuItem
        '
        Me.UnificarOficinasToolStripMenuItem.Name = "UnificarOficinasToolStripMenuItem"
        Me.UnificarOficinasToolStripMenuItem.Size = New System.Drawing.Size(343, 34)
        Me.UnificarOficinasToolStripMenuItem.Text = "Herramienta Unificar Oficinas"
        '
        'MenuSistema
        '
        Me.MenuSistema.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GestionTiposDocumentoToolStripMenuItem, Me.GestionTiemposToolStripMenuItem, Me.ConfiguracionSistemaToolStripMenuItem})
        Me.MenuSistema.Name = "MenuSistema"
        Me.MenuSistema.Size = New System.Drawing.Size(90, 29)
        Me.MenuSistema.Text = "Sistema"
        '
        'GestionTiposDocumentoToolStripMenuItem
        '
        Me.GestionTiposDocumentoToolStripMenuItem.Name = "GestionTiposDocumentoToolStripMenuItem"
        Me.GestionTiposDocumentoToolStripMenuItem.Size = New System.Drawing.Size(323, 34)
        Me.GestionTiposDocumentoToolStripMenuItem.Text = "Tipos de Documento"
        '
        'GestionTiemposToolStripMenuItem
        '
        Me.GestionTiemposToolStripMenuItem.Name = "GestionTiemposToolStripMenuItem"
        Me.GestionTiemposToolStripMenuItem.Size = New System.Drawing.Size(281, 34)
        Me.GestionTiemposToolStripMenuItem.Text = "Plazos documentos"
        '
        'ConfiguracionSistemaToolStripMenuItem
        '
        Me.ConfiguracionSistemaToolStripMenuItem.Name = "ConfiguracionSistemaToolStripMenuItem"
        Me.ConfiguracionSistemaToolStripMenuItem.Size = New System.Drawing.Size(281, 34)
        Me.ConfiguracionSistemaToolStripMenuItem.Text = "Alertas Art. 120"
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
        Me.lblEstadoUsuario.Name = "lblEstadoUsuario"
        Me.lblEstadoUsuario.Size = New System.Drawing.Size(76, 25)
        Me.lblEstadoUsuario.Text = "Usuario:"
        '
        'lblEstadoOficina
        '
        Me.lblEstadoOficina.Name = "lblEstadoOficina"
        Me.lblEstadoOficina.Size = New System.Drawing.Size(71, 25)
        Me.lblEstadoOficina.Text = "Oficina:"
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
        Me.Text = "VECTOR - Sistema de Gestión"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    ' Componentes
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents lblEstadoUsuario As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblEstadoOficina As System.Windows.Forms.ToolStripStatusLabel

    ' Niveles Superiores
    Friend WithEvents MenuInicio As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuControl As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuAdministracion As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuSistema As System.Windows.Forms.ToolStripMenuItem

    ' Sub-Items (Manteniendo nombres originales para compatibilidad)
    Friend WithEvents BandejaDeEntradaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BuscadorReclusosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SalirToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

    Friend WithEvents EstadisticasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AuditoriaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

    Friend WithEvents GestionUsuariosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GestionRangosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UnificarOficinasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

    Friend WithEvents GestionTiposDocumentoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GestionTiemposToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConfiguracionSistemaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class