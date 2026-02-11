<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDashboard
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
        Me.components = New System.ComponentModel.Container()
        Me.panelNavegacion = New System.Windows.Forms.Panel()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.btnTiempos = New System.Windows.Forms.Button()
        Me.btnTiposDoc = New System.Windows.Forms.Button()
        Me.btnConfiguracion = New System.Windows.Forms.Button()
        Me.btnUnificar = New System.Windows.Forms.Button()
        Me.btnRangos = New System.Windows.Forms.Button()
        Me.btnUsuarios = New System.Windows.Forms.Button()
        Me.btnAuditoria = New System.Windows.Forms.Button()
        Me.btnEstadisticas = New System.Windows.Forms.Button()
        Me.btnReclusos = New System.Windows.Forms.Button()
        Me.btnBandeja = New System.Windows.Forms.Button()
        Me.panelLogo = New System.Windows.Forms.Panel()
        Me.lblSemanaActual = New System.Windows.Forms.Label()
        Me.lblAppName = New System.Windows.Forms.Label()
        Me.panelContenido = New System.Windows.Forms.Panel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.panelNavegacion.SuspendLayout()
        Me.panelLogo.SuspendLayout()
        Me.SuspendLayout()
        '
        'panelNavegacion
        '
        Me.panelNavegacion.AutoScroll = True
        Me.panelNavegacion.BackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(51, Byte), Integer), CType(CType(76, Byte), Integer))
        Me.panelNavegacion.Controls.Add(Me.btnSalir)
        Me.panelNavegacion.Controls.Add(Me.btnTiempos)
        Me.panelNavegacion.Controls.Add(Me.btnTiposDoc)
        Me.panelNavegacion.Controls.Add(Me.btnConfiguracion)
        Me.panelNavegacion.Controls.Add(Me.btnUnificar)
        Me.panelNavegacion.Controls.Add(Me.btnRangos)
        Me.panelNavegacion.Controls.Add(Me.btnUsuarios)
        Me.panelNavegacion.Controls.Add(Me.btnAuditoria)
        Me.panelNavegacion.Controls.Add(Me.btnEstadisticas)
        Me.panelNavegacion.Controls.Add(Me.btnReclusos)
        Me.panelNavegacion.Controls.Add(Me.btnBandeja)
        Me.panelNavegacion.Controls.Add(Me.panelLogo)
        Me.panelNavegacion.Dock = System.Windows.Forms.DockStyle.Left
        Me.panelNavegacion.Location = New System.Drawing.Point(0, 0)
        Me.panelNavegacion.Name = "panelNavegacion"
        Me.panelNavegacion.Size = New System.Drawing.Size(330, 900)
        Me.panelNavegacion.TabIndex = 0
        '
        'btnSalir
        '
        Me.btnSalir.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnSalir.FlatAppearance.BorderSize = 0
        Me.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSalir.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSalir.ForeColor = System.Drawing.Color.LightCoral
        Me.btnSalir.Location = New System.Drawing.Point(0, 830)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnSalir.Size = New System.Drawing.Size(330, 70)
        Me.btnSalir.TabIndex = 11
        Me.btnSalir.Tag = "🚪 Salir del Sistema"
        Me.btnSalir.Text = "   🚪 Salir del Sistema"
        Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSalir.UseVisualStyleBackColor = True
        '
        'btnTiempos
        '
        Me.btnTiempos.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnTiempos.FlatAppearance.BorderSize = 0
        Me.btnTiempos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTiempos.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTiempos.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnTiempos.Location = New System.Drawing.Point(0, 740)
        Me.btnTiempos.Name = "btnTiempos"
        Me.btnTiempos.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnTiempos.Size = New System.Drawing.Size(330, 68)
        Me.btnTiempos.TabIndex = 10
        Me.btnTiempos.Tag = "⏱️ Tiempos Respuesta"
        Me.btnTiempos.Text = "   ⏱️ Tiempos Respuesta"
        Me.btnTiempos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnTiempos.UseVisualStyleBackColor = True
        '
        'btnTiposDoc
        '
        Me.btnTiposDoc.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnTiposDoc.FlatAppearance.BorderSize = 0
        Me.btnTiposDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTiposDoc.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTiposDoc.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnTiposDoc.Location = New System.Drawing.Point(0, 672)
        Me.btnTiposDoc.Name = "btnTiposDoc"
        Me.btnTiposDoc.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnTiposDoc.Size = New System.Drawing.Size(330, 68)
        Me.btnTiposDoc.TabIndex = 9
        Me.btnTiposDoc.Tag = "📄 Tipos Documento"
        Me.btnTiposDoc.Text = "   📄 Tipos Documento"
        Me.btnTiposDoc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnTiposDoc.UseVisualStyleBackColor = True
        '
        'btnConfiguracion
        '
        Me.btnConfiguracion.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnConfiguracion.FlatAppearance.BorderSize = 0
        Me.btnConfiguracion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConfiguracion.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnConfiguracion.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnConfiguracion.Location = New System.Drawing.Point(0, 604)
        Me.btnConfiguracion.Name = "btnConfiguracion"
        Me.btnConfiguracion.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnConfiguracion.Size = New System.Drawing.Size(330, 68)
        Me.btnConfiguracion.TabIndex = 8
        Me.btnConfiguracion.Tag = "⚙️ Configuración"
        Me.btnConfiguracion.Text = "   ⚙️ Configuración"
        Me.btnConfiguracion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnConfiguracion.UseVisualStyleBackColor = True
        '
        'btnUnificar
        '
        Me.btnUnificar.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnUnificar.FlatAppearance.BorderSize = 0
        Me.btnUnificar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUnificar.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUnificar.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnUnificar.Location = New System.Drawing.Point(0, 536)
        Me.btnUnificar.Name = "btnUnificar"
        Me.btnUnificar.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnUnificar.Size = New System.Drawing.Size(330, 68)
        Me.btnUnificar.TabIndex = 7
        Me.btnUnificar.Tag = "🏢 Unificar Oficinas"
        Me.btnUnificar.Text = "   🏢 Unificar Oficinas"
        Me.btnUnificar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUnificar.UseVisualStyleBackColor = True
        '
        'btnRangos
        '
        Me.btnRangos.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnRangos.FlatAppearance.BorderSize = 0
        Me.btnRangos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRangos.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRangos.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnRangos.Location = New System.Drawing.Point(0, 468)
        Me.btnRangos.Name = "btnRangos"
        Me.btnRangos.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnRangos.Size = New System.Drawing.Size(330, 68)
        Me.btnRangos.TabIndex = 6
        Me.btnRangos.Tag = "👮 Gestión Rangos"
        Me.btnRangos.Text = "   👮 Gestión Rangos"
        Me.btnRangos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRangos.UseVisualStyleBackColor = True
        '
        'btnUsuarios
        '
        Me.btnUsuarios.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnUsuarios.FlatAppearance.BorderSize = 0
        Me.btnUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUsuarios.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUsuarios.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnUsuarios.Location = New System.Drawing.Point(0, 400)
        Me.btnUsuarios.Name = "btnUsuarios"
        Me.btnUsuarios.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnUsuarios.Size = New System.Drawing.Size(330, 68)
        Me.btnUsuarios.TabIndex = 5
        Me.btnUsuarios.Tag = "👥 Gestión Usuarios"
        Me.btnUsuarios.Text = "   👥 Gestión Usuarios"
        Me.btnUsuarios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUsuarios.UseVisualStyleBackColor = True
        '
        'btnAuditoria
        '
        Me.btnAuditoria.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnAuditoria.FlatAppearance.BorderSize = 0
        Me.btnAuditoria.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAuditoria.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAuditoria.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnAuditoria.Location = New System.Drawing.Point(0, 332)
        Me.btnAuditoria.Name = "btnAuditoria"
        Me.btnAuditoria.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnAuditoria.Size = New System.Drawing.Size(330, 68)
        Me.btnAuditoria.TabIndex = 4
        Me.btnAuditoria.Tag = "🛡️ Auditoría"
        Me.btnAuditoria.Text = "   🛡️ Auditoría"
        Me.btnAuditoria.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAuditoria.UseVisualStyleBackColor = True
        '
        'btnEstadisticas
        '
        Me.btnEstadisticas.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnEstadisticas.FlatAppearance.BorderSize = 0
        Me.btnEstadisticas.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEstadisticas.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEstadisticas.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnEstadisticas.Location = New System.Drawing.Point(0, 264)
        Me.btnEstadisticas.Name = "btnEstadisticas"
        Me.btnEstadisticas.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnEstadisticas.Size = New System.Drawing.Size(330, 68)
        Me.btnEstadisticas.TabIndex = 3
        Me.btnEstadisticas.Tag = "📊 Estadísticas"
        Me.btnEstadisticas.Text = "   📊 Estadísticas"
        Me.btnEstadisticas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEstadisticas.UseVisualStyleBackColor = True
        '
        'btnReclusos
        '
        Me.btnReclusos.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnReclusos.FlatAppearance.BorderSize = 0
        Me.btnReclusos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReclusos.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReclusos.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnReclusos.Location = New System.Drawing.Point(0, 196)
        Me.btnReclusos.Name = "btnReclusos"
        Me.btnReclusos.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnReclusos.Size = New System.Drawing.Size(330, 68)
        Me.btnReclusos.TabIndex = 2
        Me.btnReclusos.Tag = "🔒 Buscador Reclusos"
        Me.btnReclusos.Text = "   🔒 Buscador Reclusos"
        Me.btnReclusos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReclusos.UseVisualStyleBackColor = True
        '
        'btnBandeja
        '
        Me.btnBandeja.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnBandeja.FlatAppearance.BorderSize = 0
        Me.btnBandeja.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBandeja.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBandeja.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnBandeja.Location = New System.Drawing.Point(0, 128)
        Me.btnBandeja.Name = "btnBandeja"
        Me.btnBandeja.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnBandeja.Size = New System.Drawing.Size(330, 68)
        Me.btnBandeja.TabIndex = 1
        Me.btnBandeja.Tag = "📥 Bandeja de Entrada"
        Me.btnBandeja.Text = "   📥 Bandeja de Entrada"
        Me.btnBandeja.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBandeja.UseVisualStyleBackColor = True
        '
        'panelLogo
        '
        Me.panelLogo.BackColor = System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(39, Byte), Integer), CType(CType(58, Byte), Integer))
        Me.panelLogo.Controls.Add(Me.lblSemanaActual)
        Me.panelLogo.Controls.Add(Me.lblAppName)
        Me.panelLogo.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelLogo.Location = New System.Drawing.Point(0, 0)
        Me.panelLogo.Name = "panelLogo"
        Me.panelLogo.Size = New System.Drawing.Size(330, 128)
        Me.panelLogo.TabIndex = 0
        '
        'lblSemanaActual
        '
        Me.lblSemanaActual.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblSemanaActual.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSemanaActual.ForeColor = System.Drawing.Color.Gold
        Me.lblSemanaActual.Location = New System.Drawing.Point(0, 83)
        Me.lblSemanaActual.Name = "lblSemanaActual"
        Me.lblSemanaActual.Size = New System.Drawing.Size(330, 45)
        Me.lblSemanaActual.TabIndex = 1
        Me.lblSemanaActual.Text = "Usuario"
        Me.lblSemanaActual.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblAppName
        '
        Me.lblAppName.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblAppName.Font = New System.Drawing.Font("Segoe UI Semibold", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAppName.ForeColor = System.Drawing.Color.White
        Me.lblAppName.Location = New System.Drawing.Point(0, 0)
        Me.lblAppName.Name = "lblAppName"
        Me.lblAppName.Size = New System.Drawing.Size(330, 83)
        Me.lblAppName.TabIndex = 0
        Me.lblAppName.Text = "VECTOR"
        Me.lblAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'panelContenido
        '
        Me.panelContenido.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelContenido.Location = New System.Drawing.Point(330, 0)
        Me.panelContenido.Name = "panelContenido"
        Me.panelContenido.Size = New System.Drawing.Size(954, 900)
        Me.panelContenido.TabIndex = 1
        '
        'frmDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1284, 900)
        Me.Controls.Add(Me.panelContenido)
        Me.Controls.Add(Me.panelNavegacion)
        Me.MinimumSize = New System.Drawing.Size(900, 600)
        Me.Name = "frmDashboard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "VECTOR - Sistema de Gestión"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.panelNavegacion.ResumeLayout(False)
        Me.panelLogo.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents panelNavegacion As System.Windows.Forms.Panel
    Friend WithEvents panelLogo As System.Windows.Forms.Panel
    Friend WithEvents lblAppName As System.Windows.Forms.Label
    Friend WithEvents panelContenido As System.Windows.Forms.Panel
    Friend WithEvents btnBandeja As System.Windows.Forms.Button
    Friend WithEvents btnReclusos As System.Windows.Forms.Button
    Friend WithEvents btnEstadisticas As System.Windows.Forms.Button
    Friend WithEvents btnAuditoria As System.Windows.Forms.Button
    Friend WithEvents btnUsuarios As System.Windows.Forms.Button
    Friend WithEvents btnRangos As System.Windows.Forms.Button
    Friend WithEvents btnUnificar As System.Windows.Forms.Button
    Friend WithEvents btnConfiguracion As System.Windows.Forms.Button
    Friend WithEvents btnTiposDoc As System.Windows.Forms.Button
    Friend WithEvents btnTiempos As System.Windows.Forms.Button
    Friend WithEvents btnSalir As System.Windows.Forms.Button
    Friend WithEvents lblSemanaActual As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class