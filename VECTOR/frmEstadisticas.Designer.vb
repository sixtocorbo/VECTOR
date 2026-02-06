<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmEstadisticas
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
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPageResumen = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelResumen = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBoxTotales = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanelTotales = New System.Windows.Forms.TableLayoutPanel()
        Me.lblTotalAdjuntos = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lblTotalRangos = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblTotalEstados = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblTotalTipos = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblTotalOficinas = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblTotalUsuarios = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblTotalReclusos = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblTotalMovimientos = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblTotalDocumentos = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBoxDocumentosRecientes = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanelDocumentos = New System.Windows.Forms.TableLayoutPanel()
        Me.lblDocumentos30 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lblDocumentos7 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblDocumentosHoy = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.GroupBoxMovimientosRecientes = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanelMovimientos = New System.Windows.Forms.TableLayoutPanel()
        Me.lblMovimientos30 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lblMovimientos7 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblMovimientosHoy = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.GroupBoxUsuariosOficinas = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanelUsuarios = New System.Windows.Forms.TableLayoutPanel()
        Me.lblOficinasExternas = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.lblOficinasInternas = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.lblUsuariosInactivos = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lblUsuariosActivos = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnActualizar = New System.Windows.Forms.Button()
        Me.TabPageDocumentos = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelDocumentosTab = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBoxDocumentosEstado = New System.Windows.Forms.GroupBox()
        Me.dgvDocumentosPorEstado = New System.Windows.Forms.DataGridView()
        Me.GroupBoxDocumentosTipo = New System.Windows.Forms.GroupBox()
        Me.dgvDocumentosPorTipo = New System.Windows.Forms.DataGridView()
        Me.TabPageMovimientos = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelMovimientosTab = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBoxMovimientosOficina = New System.Windows.Forms.GroupBox()
        Me.dgvMovimientosPorOficina = New System.Windows.Forms.DataGridView()
        Me.GroupBoxMovimientosMes = New System.Windows.Forms.GroupBox()
        Me.dgvMovimientosPorMes = New System.Windows.Forms.DataGridView()
        Me.TabControl1.SuspendLayout()
        Me.TabPageResumen.SuspendLayout()
        Me.TableLayoutPanelResumen.SuspendLayout()
        Me.GroupBoxTotales.SuspendLayout()
        Me.TableLayoutPanelTotales.SuspendLayout()
        Me.GroupBoxDocumentosRecientes.SuspendLayout()
        Me.TableLayoutPanelDocumentos.SuspendLayout()
        Me.GroupBoxMovimientosRecientes.SuspendLayout()
        Me.TableLayoutPanelMovimientos.SuspendLayout()
        Me.GroupBoxUsuariosOficinas.SuspendLayout()
        Me.TableLayoutPanelUsuarios.SuspendLayout()
        Me.TabPageDocumentos.SuspendLayout()
        Me.TableLayoutPanelDocumentosTab.SuspendLayout()
        Me.GroupBoxDocumentosEstado.SuspendLayout()
        CType(Me.dgvDocumentosPorEstado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxDocumentosTipo.SuspendLayout()
        CType(Me.dgvDocumentosPorTipo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageMovimientos.SuspendLayout()
        Me.TableLayoutPanelMovimientosTab.SuspendLayout()
        Me.GroupBoxMovimientosOficina.SuspendLayout()
        CType(Me.dgvMovimientosPorOficina, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxMovimientosMes.SuspendLayout()
        CType(Me.dgvMovimientosPorMes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPageResumen)
        Me.TabControl1.Controls.Add(Me.TabPageDocumentos)
        Me.TabControl1.Controls.Add(Me.TabPageMovimientos)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1400, 900)
        Me.TabControl1.TabIndex = 0
        '
        'TabPageResumen
        '
        Me.TabPageResumen.Controls.Add(Me.TableLayoutPanelResumen)
        Me.TabPageResumen.Location = New System.Drawing.Point(4, 29)
        Me.TabPageResumen.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPageResumen.Name = "TabPageResumen"
        Me.TabPageResumen.Padding = New System.Windows.Forms.Padding(9)
        Me.TabPageResumen.Size = New System.Drawing.Size(1392, 867)
        Me.TabPageResumen.TabIndex = 0
        Me.TabPageResumen.Text = "Resumen"
        Me.TabPageResumen.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelResumen
        '
        Me.TableLayoutPanelResumen.ColumnCount = 2
        Me.TableLayoutPanelResumen.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelResumen.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelResumen.Controls.Add(Me.GroupBoxTotales, 0, 0)
        Me.TableLayoutPanelResumen.Controls.Add(Me.GroupBoxDocumentosRecientes, 1, 0)
        Me.TableLayoutPanelResumen.Controls.Add(Me.GroupBoxMovimientosRecientes, 0, 1)
        Me.TableLayoutPanelResumen.Controls.Add(Me.GroupBoxUsuariosOficinas, 1, 1)
        Me.TableLayoutPanelResumen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelResumen.Location = New System.Drawing.Point(9, 9)
        Me.TableLayoutPanelResumen.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TableLayoutPanelResumen.Name = "TableLayoutPanelResumen"
        Me.TableLayoutPanelResumen.RowCount = 2
        Me.TableLayoutPanelResumen.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.TableLayoutPanelResumen.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanelResumen.Size = New System.Drawing.Size(1374, 849)
        Me.TableLayoutPanelResumen.TabIndex = 0
        '
        'GroupBoxTotales
        '
        Me.GroupBoxTotales.Controls.Add(Me.TableLayoutPanelTotales)
        Me.GroupBoxTotales.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxTotales.Location = New System.Drawing.Point(4, 5)
        Me.GroupBoxTotales.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxTotales.Name = "GroupBoxTotales"
        Me.GroupBoxTotales.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxTotales.Size = New System.Drawing.Size(679, 499)
        Me.GroupBoxTotales.TabIndex = 0
        Me.GroupBoxTotales.TabStop = False
        Me.GroupBoxTotales.Text = "Totales generales"
        '
        'TableLayoutPanelTotales
        '
        Me.TableLayoutPanelTotales.ColumnCount = 2
        Me.TableLayoutPanelTotales.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.0!))
        Me.TableLayoutPanelTotales.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.0!))
        Me.TableLayoutPanelTotales.Controls.Add(Me.lblTotalAdjuntos, 1, 8)
        Me.TableLayoutPanelTotales.Controls.Add(Me.Label9, 0, 8)
        Me.TableLayoutPanelTotales.Controls.Add(Me.lblTotalRangos, 1, 7)
        Me.TableLayoutPanelTotales.Controls.Add(Me.Label8, 0, 7)
        Me.TableLayoutPanelTotales.Controls.Add(Me.lblTotalEstados, 1, 6)
        Me.TableLayoutPanelTotales.Controls.Add(Me.Label7, 0, 6)
        Me.TableLayoutPanelTotales.Controls.Add(Me.lblTotalTipos, 1, 5)
        Me.TableLayoutPanelTotales.Controls.Add(Me.Label6, 0, 5)
        Me.TableLayoutPanelTotales.Controls.Add(Me.lblTotalOficinas, 1, 4)
        Me.TableLayoutPanelTotales.Controls.Add(Me.Label5, 0, 4)
        Me.TableLayoutPanelTotales.Controls.Add(Me.lblTotalUsuarios, 1, 3)
        Me.TableLayoutPanelTotales.Controls.Add(Me.Label4, 0, 3)
        Me.TableLayoutPanelTotales.Controls.Add(Me.lblTotalReclusos, 1, 2)
        Me.TableLayoutPanelTotales.Controls.Add(Me.Label3, 0, 2)
        Me.TableLayoutPanelTotales.Controls.Add(Me.lblTotalMovimientos, 1, 1)
        Me.TableLayoutPanelTotales.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanelTotales.Controls.Add(Me.lblTotalDocumentos, 1, 0)
        Me.TableLayoutPanelTotales.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanelTotales.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelTotales.Location = New System.Drawing.Point(4, 24)
        Me.TableLayoutPanelTotales.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TableLayoutPanelTotales.Name = "TableLayoutPanelTotales"
        Me.TableLayoutPanelTotales.RowCount = 9
        Me.TableLayoutPanelTotales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanelTotales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanelTotales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanelTotales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanelTotales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanelTotales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanelTotales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanelTotales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanelTotales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanelTotales.Size = New System.Drawing.Size(671, 470)
        Me.TableLayoutPanelTotales.TabIndex = 0
        '
        'lblTotalAdjuntos
        '
        Me.lblTotalAdjuntos.AutoSize = True
        Me.lblTotalAdjuntos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTotalAdjuntos.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalAdjuntos.Location = New System.Drawing.Point(439, 416)
        Me.lblTotalAdjuntos.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalAdjuntos.Name = "lblTotalAdjuntos"
        Me.lblTotalAdjuntos.Size = New System.Drawing.Size(228, 54)
        Me.lblTotalAdjuntos.TabIndex = 17
        Me.lblTotalAdjuntos.Text = "0"
        Me.lblTotalAdjuntos.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label9.Location = New System.Drawing.Point(4, 416)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(427, 54)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Total adjuntos"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalRangos
        '
        Me.lblTotalRangos.AutoSize = True
        Me.lblTotalRangos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTotalRangos.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalRangos.Location = New System.Drawing.Point(439, 364)
        Me.lblTotalRangos.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalRangos.Name = "lblTotalRangos"
        Me.lblTotalRangos.Size = New System.Drawing.Size(228, 52)
        Me.lblTotalRangos.TabIndex = 15
        Me.lblTotalRangos.Text = "0"
        Me.lblTotalRangos.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label8.Location = New System.Drawing.Point(4, 364)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(427, 52)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Total rangos de numeración"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalEstados
        '
        Me.lblTotalEstados.AutoSize = True
        Me.lblTotalEstados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTotalEstados.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalEstados.Location = New System.Drawing.Point(439, 312)
        Me.lblTotalEstados.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalEstados.Name = "lblTotalEstados"
        Me.lblTotalEstados.Size = New System.Drawing.Size(228, 52)
        Me.lblTotalEstados.TabIndex = 13
        Me.lblTotalEstados.Text = "0"
        Me.lblTotalEstados.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label7.Location = New System.Drawing.Point(4, 312)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(427, 52)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Total estados"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalTipos
        '
        Me.lblTotalTipos.AutoSize = True
        Me.lblTotalTipos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTotalTipos.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalTipos.Location = New System.Drawing.Point(439, 260)
        Me.lblTotalTipos.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalTipos.Name = "lblTotalTipos"
        Me.lblTotalTipos.Size = New System.Drawing.Size(228, 52)
        Me.lblTotalTipos.TabIndex = 11
        Me.lblTotalTipos.Text = "0"
        Me.lblTotalTipos.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label6.Location = New System.Drawing.Point(4, 260)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(427, 52)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Total tipos de documento"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalOficinas
        '
        Me.lblTotalOficinas.AutoSize = True
        Me.lblTotalOficinas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTotalOficinas.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalOficinas.Location = New System.Drawing.Point(439, 208)
        Me.lblTotalOficinas.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalOficinas.Name = "lblTotalOficinas"
        Me.lblTotalOficinas.Size = New System.Drawing.Size(228, 52)
        Me.lblTotalOficinas.TabIndex = 9
        Me.lblTotalOficinas.Text = "0"
        Me.lblTotalOficinas.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label5.Location = New System.Drawing.Point(4, 208)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(427, 52)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Total oficinas"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalUsuarios
        '
        Me.lblTotalUsuarios.AutoSize = True
        Me.lblTotalUsuarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTotalUsuarios.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalUsuarios.Location = New System.Drawing.Point(439, 156)
        Me.lblTotalUsuarios.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalUsuarios.Name = "lblTotalUsuarios"
        Me.lblTotalUsuarios.Size = New System.Drawing.Size(228, 52)
        Me.lblTotalUsuarios.TabIndex = 7
        Me.lblTotalUsuarios.Text = "0"
        Me.lblTotalUsuarios.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label4.Location = New System.Drawing.Point(4, 156)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(427, 52)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Total usuarios"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalReclusos
        '
        Me.lblTotalReclusos.AutoSize = True
        Me.lblTotalReclusos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTotalReclusos.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalReclusos.Location = New System.Drawing.Point(439, 104)
        Me.lblTotalReclusos.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalReclusos.Name = "lblTotalReclusos"
        Me.lblTotalReclusos.Size = New System.Drawing.Size(228, 52)
        Me.lblTotalReclusos.TabIndex = 5
        Me.lblTotalReclusos.Text = "0"
        Me.lblTotalReclusos.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label3.Location = New System.Drawing.Point(4, 104)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(427, 52)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Total reclusos"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalMovimientos
        '
        Me.lblTotalMovimientos.AutoSize = True
        Me.lblTotalMovimientos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTotalMovimientos.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalMovimientos.Location = New System.Drawing.Point(439, 52)
        Me.lblTotalMovimientos.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalMovimientos.Name = "lblTotalMovimientos"
        Me.lblTotalMovimientos.Size = New System.Drawing.Size(228, 52)
        Me.lblTotalMovimientos.TabIndex = 3
        Me.lblTotalMovimientos.Text = "0"
        Me.lblTotalMovimientos.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Location = New System.Drawing.Point(4, 52)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(427, 52)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Total movimientos"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotalDocumentos
        '
        Me.lblTotalDocumentos.AutoSize = True
        Me.lblTotalDocumentos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTotalDocumentos.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalDocumentos.Location = New System.Drawing.Point(439, 0)
        Me.lblTotalDocumentos.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalDocumentos.Name = "lblTotalDocumentos"
        Me.lblTotalDocumentos.Size = New System.Drawing.Size(228, 52)
        Me.lblTotalDocumentos.TabIndex = 1
        Me.lblTotalDocumentos.Text = "0"
        Me.lblTotalDocumentos.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(4, 0)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(427, 52)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Total documentos"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBoxDocumentosRecientes
        '
        Me.GroupBoxDocumentosRecientes.Controls.Add(Me.TableLayoutPanelDocumentos)
        Me.GroupBoxDocumentosRecientes.Controls.Add(Me.btnActualizar)
        Me.GroupBoxDocumentosRecientes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxDocumentosRecientes.Location = New System.Drawing.Point(691, 5)
        Me.GroupBoxDocumentosRecientes.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxDocumentosRecientes.Name = "GroupBoxDocumentosRecientes"
        Me.GroupBoxDocumentosRecientes.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxDocumentosRecientes.Size = New System.Drawing.Size(679, 499)
        Me.GroupBoxDocumentosRecientes.TabIndex = 1
        Me.GroupBoxDocumentosRecientes.TabStop = False
        Me.GroupBoxDocumentosRecientes.Text = "Documentos recientes"
        '
        'TableLayoutPanelDocumentos
        '
        Me.TableLayoutPanelDocumentos.ColumnCount = 2
        Me.TableLayoutPanelDocumentos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.0!))
        Me.TableLayoutPanelDocumentos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.0!))
        Me.TableLayoutPanelDocumentos.Controls.Add(Me.lblDocumentos30, 1, 2)
        Me.TableLayoutPanelDocumentos.Controls.Add(Me.Label12, 0, 2)
        Me.TableLayoutPanelDocumentos.Controls.Add(Me.lblDocumentos7, 1, 1)
        Me.TableLayoutPanelDocumentos.Controls.Add(Me.Label11, 0, 1)
        Me.TableLayoutPanelDocumentos.Controls.Add(Me.lblDocumentosHoy, 1, 0)
        Me.TableLayoutPanelDocumentos.Controls.Add(Me.Label10, 0, 0)
        Me.TableLayoutPanelDocumentos.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanelDocumentos.Location = New System.Drawing.Point(4, 24)
        Me.TableLayoutPanelDocumentos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TableLayoutPanelDocumentos.Name = "TableLayoutPanelDocumentos"
        Me.TableLayoutPanelDocumentos.RowCount = 3
        Me.TableLayoutPanelDocumentos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelDocumentos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelDocumentos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelDocumentos.Size = New System.Drawing.Size(671, 200)
        Me.TableLayoutPanelDocumentos.TabIndex = 0
        '
        'lblDocumentos30
        '
        Me.lblDocumentos30.AutoSize = True
        Me.lblDocumentos30.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDocumentos30.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblDocumentos30.Location = New System.Drawing.Point(439, 132)
        Me.lblDocumentos30.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDocumentos30.Name = "lblDocumentos30"
        Me.lblDocumentos30.Size = New System.Drawing.Size(228, 68)
        Me.lblDocumentos30.TabIndex = 5
        Me.lblDocumentos30.Text = "0"
        Me.lblDocumentos30.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label12.Location = New System.Drawing.Point(4, 132)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(427, 68)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "Creados últimos 30 días"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDocumentos7
        '
        Me.lblDocumentos7.AutoSize = True
        Me.lblDocumentos7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDocumentos7.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblDocumentos7.Location = New System.Drawing.Point(439, 66)
        Me.lblDocumentos7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDocumentos7.Name = "lblDocumentos7"
        Me.lblDocumentos7.Size = New System.Drawing.Size(228, 66)
        Me.lblDocumentos7.TabIndex = 3
        Me.lblDocumentos7.Text = "0"
        Me.lblDocumentos7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label11.Location = New System.Drawing.Point(4, 66)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(427, 66)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Creados últimos 7 días"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDocumentosHoy
        '
        Me.lblDocumentosHoy.AutoSize = True
        Me.lblDocumentosHoy.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDocumentosHoy.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblDocumentosHoy.Location = New System.Drawing.Point(439, 0)
        Me.lblDocumentosHoy.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDocumentosHoy.Name = "lblDocumentosHoy"
        Me.lblDocumentosHoy.Size = New System.Drawing.Size(228, 66)
        Me.lblDocumentosHoy.TabIndex = 1
        Me.lblDocumentosHoy.Text = "0"
        Me.lblDocumentosHoy.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label10.Location = New System.Drawing.Point(4, 0)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(427, 66)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Creados hoy"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBoxMovimientosRecientes
        '
        Me.GroupBoxMovimientosRecientes.Controls.Add(Me.TableLayoutPanelMovimientos)
        Me.GroupBoxMovimientosRecientes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxMovimientosRecientes.Location = New System.Drawing.Point(4, 514)
        Me.GroupBoxMovimientosRecientes.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxMovimientosRecientes.Name = "GroupBoxMovimientosRecientes"
        Me.GroupBoxMovimientosRecientes.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxMovimientosRecientes.Size = New System.Drawing.Size(679, 330)
        Me.GroupBoxMovimientosRecientes.TabIndex = 2
        Me.GroupBoxMovimientosRecientes.TabStop = False
        Me.GroupBoxMovimientosRecientes.Text = "Movimientos recientes"
        '
        'TableLayoutPanelMovimientos
        '
        Me.TableLayoutPanelMovimientos.ColumnCount = 2
        Me.TableLayoutPanelMovimientos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.0!))
        Me.TableLayoutPanelMovimientos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.0!))
        Me.TableLayoutPanelMovimientos.Controls.Add(Me.lblMovimientos30, 1, 2)
        Me.TableLayoutPanelMovimientos.Controls.Add(Me.Label15, 0, 2)
        Me.TableLayoutPanelMovimientos.Controls.Add(Me.lblMovimientos7, 1, 1)
        Me.TableLayoutPanelMovimientos.Controls.Add(Me.Label14, 0, 1)
        Me.TableLayoutPanelMovimientos.Controls.Add(Me.lblMovimientosHoy, 1, 0)
        Me.TableLayoutPanelMovimientos.Controls.Add(Me.Label13, 0, 0)
        Me.TableLayoutPanelMovimientos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMovimientos.Location = New System.Drawing.Point(4, 24)
        Me.TableLayoutPanelMovimientos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TableLayoutPanelMovimientos.Name = "TableLayoutPanelMovimientos"
        Me.TableLayoutPanelMovimientos.RowCount = 3
        Me.TableLayoutPanelMovimientos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelMovimientos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelMovimientos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelMovimientos.Size = New System.Drawing.Size(671, 301)
        Me.TableLayoutPanelMovimientos.TabIndex = 0
        '
        'lblMovimientos30
        '
        Me.lblMovimientos30.AutoSize = True
        Me.lblMovimientos30.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblMovimientos30.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblMovimientos30.Location = New System.Drawing.Point(439, 200)
        Me.lblMovimientos30.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMovimientos30.Name = "lblMovimientos30"
        Me.lblMovimientos30.Size = New System.Drawing.Size(228, 101)
        Me.lblMovimientos30.TabIndex = 5
        Me.lblMovimientos30.Text = "0"
        Me.lblMovimientos30.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label15.Location = New System.Drawing.Point(4, 200)
        Me.Label15.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(427, 101)
        Me.Label15.TabIndex = 4
        Me.Label15.Text = "Movimientos últimos 30 días"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblMovimientos7
        '
        Me.lblMovimientos7.AutoSize = True
        Me.lblMovimientos7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblMovimientos7.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblMovimientos7.Location = New System.Drawing.Point(439, 100)
        Me.lblMovimientos7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMovimientos7.Name = "lblMovimientos7"
        Me.lblMovimientos7.Size = New System.Drawing.Size(228, 100)
        Me.lblMovimientos7.TabIndex = 3
        Me.lblMovimientos7.Text = "0"
        Me.lblMovimientos7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label14.Location = New System.Drawing.Point(4, 100)
        Me.Label14.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(427, 100)
        Me.Label14.TabIndex = 2
        Me.Label14.Text = "Movimientos últimos 7 días"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblMovimientosHoy
        '
        Me.lblMovimientosHoy.AutoSize = True
        Me.lblMovimientosHoy.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblMovimientosHoy.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblMovimientosHoy.Location = New System.Drawing.Point(439, 0)
        Me.lblMovimientosHoy.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMovimientosHoy.Name = "lblMovimientosHoy"
        Me.lblMovimientosHoy.Size = New System.Drawing.Size(228, 100)
        Me.lblMovimientosHoy.TabIndex = 1
        Me.lblMovimientosHoy.Text = "0"
        Me.lblMovimientosHoy.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label13.Location = New System.Drawing.Point(4, 0)
        Me.Label13.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(427, 100)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Movimientos hoy"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBoxUsuariosOficinas
        '
        Me.GroupBoxUsuariosOficinas.Controls.Add(Me.TableLayoutPanelUsuarios)
        Me.GroupBoxUsuariosOficinas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxUsuariosOficinas.Location = New System.Drawing.Point(691, 514)
        Me.GroupBoxUsuariosOficinas.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxUsuariosOficinas.Name = "GroupBoxUsuariosOficinas"
        Me.GroupBoxUsuariosOficinas.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxUsuariosOficinas.Size = New System.Drawing.Size(679, 330)
        Me.GroupBoxUsuariosOficinas.TabIndex = 3
        Me.GroupBoxUsuariosOficinas.TabStop = False
        Me.GroupBoxUsuariosOficinas.Text = "Usuarios y oficinas"
        '
        'TableLayoutPanelUsuarios
        '
        Me.TableLayoutPanelUsuarios.ColumnCount = 2
        Me.TableLayoutPanelUsuarios.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.0!))
        Me.TableLayoutPanelUsuarios.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.0!))
        Me.TableLayoutPanelUsuarios.Controls.Add(Me.lblOficinasExternas, 1, 3)
        Me.TableLayoutPanelUsuarios.Controls.Add(Me.Label19, 0, 3)
        Me.TableLayoutPanelUsuarios.Controls.Add(Me.lblOficinasInternas, 1, 2)
        Me.TableLayoutPanelUsuarios.Controls.Add(Me.Label18, 0, 2)
        Me.TableLayoutPanelUsuarios.Controls.Add(Me.lblUsuariosInactivos, 1, 1)
        Me.TableLayoutPanelUsuarios.Controls.Add(Me.Label17, 0, 1)
        Me.TableLayoutPanelUsuarios.Controls.Add(Me.lblUsuariosActivos, 1, 0)
        Me.TableLayoutPanelUsuarios.Controls.Add(Me.Label16, 0, 0)
        Me.TableLayoutPanelUsuarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelUsuarios.Location = New System.Drawing.Point(4, 24)
        Me.TableLayoutPanelUsuarios.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TableLayoutPanelUsuarios.Name = "TableLayoutPanelUsuarios"
        Me.TableLayoutPanelUsuarios.RowCount = 4
        Me.TableLayoutPanelUsuarios.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanelUsuarios.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanelUsuarios.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanelUsuarios.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanelUsuarios.Size = New System.Drawing.Size(671, 301)
        Me.TableLayoutPanelUsuarios.TabIndex = 0
        '
        'lblOficinasExternas
        '
        Me.lblOficinasExternas.AutoSize = True
        Me.lblOficinasExternas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblOficinasExternas.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblOficinasExternas.Location = New System.Drawing.Point(439, 225)
        Me.lblOficinasExternas.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOficinasExternas.Name = "lblOficinasExternas"
        Me.lblOficinasExternas.Size = New System.Drawing.Size(228, 76)
        Me.lblOficinasExternas.TabIndex = 7
        Me.lblOficinasExternas.Text = "0"
        Me.lblOficinasExternas.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label19.Location = New System.Drawing.Point(4, 225)
        Me.Label19.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(427, 76)
        Me.Label19.TabIndex = 6
        Me.Label19.Text = "Oficinas externas"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblOficinasInternas
        '
        Me.lblOficinasInternas.AutoSize = True
        Me.lblOficinasInternas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblOficinasInternas.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblOficinasInternas.Location = New System.Drawing.Point(439, 150)
        Me.lblOficinasInternas.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOficinasInternas.Name = "lblOficinasInternas"
        Me.lblOficinasInternas.Size = New System.Drawing.Size(228, 75)
        Me.lblOficinasInternas.TabIndex = 5
        Me.lblOficinasInternas.Text = "0"
        Me.lblOficinasInternas.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label18.Location = New System.Drawing.Point(4, 150)
        Me.Label18.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(427, 75)
        Me.Label18.TabIndex = 4
        Me.Label18.Text = "Oficinas internas"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblUsuariosInactivos
        '
        Me.lblUsuariosInactivos.AutoSize = True
        Me.lblUsuariosInactivos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblUsuariosInactivos.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblUsuariosInactivos.Location = New System.Drawing.Point(439, 75)
        Me.lblUsuariosInactivos.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblUsuariosInactivos.Name = "lblUsuariosInactivos"
        Me.lblUsuariosInactivos.Size = New System.Drawing.Size(228, 75)
        Me.lblUsuariosInactivos.TabIndex = 3
        Me.lblUsuariosInactivos.Text = "0"
        Me.lblUsuariosInactivos.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label17.Location = New System.Drawing.Point(4, 75)
        Me.Label17.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(427, 75)
        Me.Label17.TabIndex = 2
        Me.Label17.Text = "Usuarios inactivos"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblUsuariosActivos
        '
        Me.lblUsuariosActivos.AutoSize = True
        Me.lblUsuariosActivos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblUsuariosActivos.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblUsuariosActivos.Location = New System.Drawing.Point(439, 0)
        Me.lblUsuariosActivos.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblUsuariosActivos.Name = "lblUsuariosActivos"
        Me.lblUsuariosActivos.Size = New System.Drawing.Size(228, 75)
        Me.lblUsuariosActivos.TabIndex = 1
        Me.lblUsuariosActivos.Text = "0"
        Me.lblUsuariosActivos.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label16.Location = New System.Drawing.Point(4, 0)
        Me.Label16.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(427, 75)
        Me.Label16.TabIndex = 0
        Me.Label16.Text = "Usuarios activos"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnActualizar
        '
        Me.btnActualizar.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnActualizar.Location = New System.Drawing.Point(272, 250)
        Me.btnActualizar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnActualizar.Name = "btnActualizar"
        Me.btnActualizar.Size = New System.Drawing.Size(135, 35)
        Me.btnActualizar.TabIndex = 1
        Me.btnActualizar.Text = "Actualizar"
        Me.btnActualizar.UseVisualStyleBackColor = True
        '
        'TabPageDocumentos
        '
        Me.TabPageDocumentos.Controls.Add(Me.TableLayoutPanelDocumentosTab)
        Me.TabPageDocumentos.Location = New System.Drawing.Point(4, 29)
        Me.TabPageDocumentos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPageDocumentos.Name = "TabPageDocumentos"
        Me.TabPageDocumentos.Padding = New System.Windows.Forms.Padding(9)
        Me.TabPageDocumentos.Size = New System.Drawing.Size(1392, 867)
        Me.TabPageDocumentos.TabIndex = 1
        Me.TabPageDocumentos.Text = "Documentos"
        Me.TabPageDocumentos.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelDocumentosTab
        '
        Me.TableLayoutPanelDocumentosTab.ColumnCount = 1
        Me.TableLayoutPanelDocumentosTab.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelDocumentosTab.Controls.Add(Me.GroupBoxDocumentosEstado, 0, 0)
        Me.TableLayoutPanelDocumentosTab.Controls.Add(Me.GroupBoxDocumentosTipo, 0, 1)
        Me.TableLayoutPanelDocumentosTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelDocumentosTab.Location = New System.Drawing.Point(9, 9)
        Me.TableLayoutPanelDocumentosTab.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TableLayoutPanelDocumentosTab.Name = "TableLayoutPanelDocumentosTab"
        Me.TableLayoutPanelDocumentosTab.RowCount = 2
        Me.TableLayoutPanelDocumentosTab.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelDocumentosTab.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelDocumentosTab.Size = New System.Drawing.Size(1374, 849)
        Me.TableLayoutPanelDocumentosTab.TabIndex = 0
        '
        'GroupBoxDocumentosEstado
        '
        Me.GroupBoxDocumentosEstado.Controls.Add(Me.dgvDocumentosPorEstado)
        Me.GroupBoxDocumentosEstado.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxDocumentosEstado.Location = New System.Drawing.Point(4, 5)
        Me.GroupBoxDocumentosEstado.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxDocumentosEstado.Name = "GroupBoxDocumentosEstado"
        Me.GroupBoxDocumentosEstado.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxDocumentosEstado.Size = New System.Drawing.Size(1366, 414)
        Me.GroupBoxDocumentosEstado.TabIndex = 0
        Me.GroupBoxDocumentosEstado.TabStop = False
        Me.GroupBoxDocumentosEstado.Text = "Documentos por estado"
        '
        'dgvDocumentosPorEstado
        '
        Me.dgvDocumentosPorEstado.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDocumentosPorEstado.Location = New System.Drawing.Point(4, 24)
        Me.dgvDocumentosPorEstado.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvDocumentosPorEstado.Name = "dgvDocumentosPorEstado"
        Me.dgvDocumentosPorEstado.RowHeadersWidth = 62
        Me.dgvDocumentosPorEstado.Size = New System.Drawing.Size(1358, 385)
        Me.dgvDocumentosPorEstado.TabIndex = 0
        '
        'GroupBoxDocumentosTipo
        '
        Me.GroupBoxDocumentosTipo.Controls.Add(Me.dgvDocumentosPorTipo)
        Me.GroupBoxDocumentosTipo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxDocumentosTipo.Location = New System.Drawing.Point(4, 429)
        Me.GroupBoxDocumentosTipo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxDocumentosTipo.Name = "GroupBoxDocumentosTipo"
        Me.GroupBoxDocumentosTipo.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxDocumentosTipo.Size = New System.Drawing.Size(1366, 415)
        Me.GroupBoxDocumentosTipo.TabIndex = 1
        Me.GroupBoxDocumentosTipo.TabStop = False
        Me.GroupBoxDocumentosTipo.Text = "Documentos por tipo"
        '
        'dgvDocumentosPorTipo
        '
        Me.dgvDocumentosPorTipo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDocumentosPorTipo.Location = New System.Drawing.Point(4, 24)
        Me.dgvDocumentosPorTipo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvDocumentosPorTipo.Name = "dgvDocumentosPorTipo"
        Me.dgvDocumentosPorTipo.RowHeadersWidth = 62
        Me.dgvDocumentosPorTipo.Size = New System.Drawing.Size(1358, 386)
        Me.dgvDocumentosPorTipo.TabIndex = 0
        '
        'TabPageMovimientos
        '
        Me.TabPageMovimientos.Controls.Add(Me.TableLayoutPanelMovimientosTab)
        Me.TabPageMovimientos.Location = New System.Drawing.Point(4, 29)
        Me.TabPageMovimientos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPageMovimientos.Name = "TabPageMovimientos"
        Me.TabPageMovimientos.Padding = New System.Windows.Forms.Padding(9)
        Me.TabPageMovimientos.Size = New System.Drawing.Size(1392, 867)
        Me.TabPageMovimientos.TabIndex = 2
        Me.TabPageMovimientos.Text = "Movimientos"
        Me.TabPageMovimientos.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelMovimientosTab
        '
        Me.TableLayoutPanelMovimientosTab.ColumnCount = 1
        Me.TableLayoutPanelMovimientosTab.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelMovimientosTab.Controls.Add(Me.GroupBoxMovimientosOficina, 0, 0)
        Me.TableLayoutPanelMovimientosTab.Controls.Add(Me.GroupBoxMovimientosMes, 0, 1)
        Me.TableLayoutPanelMovimientosTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMovimientosTab.Location = New System.Drawing.Point(9, 9)
        Me.TableLayoutPanelMovimientosTab.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TableLayoutPanelMovimientosTab.Name = "TableLayoutPanelMovimientosTab"
        Me.TableLayoutPanelMovimientosTab.RowCount = 2
        Me.TableLayoutPanelMovimientosTab.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelMovimientosTab.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelMovimientosTab.Size = New System.Drawing.Size(1374, 849)
        Me.TableLayoutPanelMovimientosTab.TabIndex = 0
        '
        'GroupBoxMovimientosOficina
        '
        Me.GroupBoxMovimientosOficina.Controls.Add(Me.dgvMovimientosPorOficina)
        Me.GroupBoxMovimientosOficina.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxMovimientosOficina.Location = New System.Drawing.Point(4, 5)
        Me.GroupBoxMovimientosOficina.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxMovimientosOficina.Name = "GroupBoxMovimientosOficina"
        Me.GroupBoxMovimientosOficina.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxMovimientosOficina.Size = New System.Drawing.Size(1366, 414)
        Me.GroupBoxMovimientosOficina.TabIndex = 0
        Me.GroupBoxMovimientosOficina.TabStop = False
        Me.GroupBoxMovimientosOficina.Text = "Movimientos por oficina destino"
        '
        'dgvMovimientosPorOficina
        '
        Me.dgvMovimientosPorOficina.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvMovimientosPorOficina.Location = New System.Drawing.Point(4, 24)
        Me.dgvMovimientosPorOficina.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvMovimientosPorOficina.Name = "dgvMovimientosPorOficina"
        Me.dgvMovimientosPorOficina.RowHeadersWidth = 62
        Me.dgvMovimientosPorOficina.Size = New System.Drawing.Size(1358, 385)
        Me.dgvMovimientosPorOficina.TabIndex = 0
        '
        'GroupBoxMovimientosMes
        '
        Me.GroupBoxMovimientosMes.Controls.Add(Me.dgvMovimientosPorMes)
        Me.GroupBoxMovimientosMes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxMovimientosMes.Location = New System.Drawing.Point(4, 429)
        Me.GroupBoxMovimientosMes.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxMovimientosMes.Name = "GroupBoxMovimientosMes"
        Me.GroupBoxMovimientosMes.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBoxMovimientosMes.Size = New System.Drawing.Size(1366, 415)
        Me.GroupBoxMovimientosMes.TabIndex = 1
        Me.GroupBoxMovimientosMes.TabStop = False
        Me.GroupBoxMovimientosMes.Text = "Movimientos por mes (últimos 12)"
        '
        'dgvMovimientosPorMes
        '
        Me.dgvMovimientosPorMes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvMovimientosPorMes.Location = New System.Drawing.Point(4, 24)
        Me.dgvMovimientosPorMes.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvMovimientosPorMes.Name = "dgvMovimientosPorMes"
        Me.dgvMovimientosPorMes.RowHeadersWidth = 62
        Me.dgvMovimientosPorMes.Size = New System.Drawing.Size(1358, 386)
        Me.dgvMovimientosPorMes.TabIndex = 0
        '
        'frmEstadisticas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1400, 900)
        Me.Controls.Add(Me.TabControl1)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmEstadisticas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Estadísticas Generales"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageResumen.ResumeLayout(False)
        Me.TableLayoutPanelResumen.ResumeLayout(False)
        Me.GroupBoxTotales.ResumeLayout(False)
        Me.TableLayoutPanelTotales.ResumeLayout(False)
        Me.TableLayoutPanelTotales.PerformLayout()
        Me.GroupBoxDocumentosRecientes.ResumeLayout(False)
        Me.TableLayoutPanelDocumentos.ResumeLayout(False)
        Me.TableLayoutPanelDocumentos.PerformLayout()
        Me.GroupBoxMovimientosRecientes.ResumeLayout(False)
        Me.TableLayoutPanelMovimientos.ResumeLayout(False)
        Me.TableLayoutPanelMovimientos.PerformLayout()
        Me.GroupBoxUsuariosOficinas.ResumeLayout(False)
        Me.TableLayoutPanelUsuarios.ResumeLayout(False)
        Me.TableLayoutPanelUsuarios.PerformLayout()
        Me.TabPageDocumentos.ResumeLayout(False)
        Me.TableLayoutPanelDocumentosTab.ResumeLayout(False)
        Me.GroupBoxDocumentosEstado.ResumeLayout(False)
        CType(Me.dgvDocumentosPorEstado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxDocumentosTipo.ResumeLayout(False)
        CType(Me.dgvDocumentosPorTipo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageMovimientos.ResumeLayout(False)
        Me.TableLayoutPanelMovimientosTab.ResumeLayout(False)
        Me.GroupBoxMovimientosOficina.ResumeLayout(False)
        CType(Me.dgvMovimientosPorOficina, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxMovimientosMes.ResumeLayout(False)
        CType(Me.dgvMovimientosPorMes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPageResumen As System.Windows.Forms.TabPage
    Friend WithEvents TabPageDocumentos As System.Windows.Forms.TabPage
    Friend WithEvents TabPageMovimientos As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanelResumen As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents GroupBoxTotales As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanelTotales As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblTotalDocumentos As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblTotalMovimientos As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblTotalReclusos As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblTotalUsuarios As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblTotalOficinas As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblTotalTipos As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblTotalEstados As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblTotalRangos As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblTotalAdjuntos As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents GroupBoxDocumentosRecientes As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanelDocumentos As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblDocumentosHoy As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblDocumentos7 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblDocumentos30 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents GroupBoxMovimientosRecientes As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanelMovimientos As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblMovimientosHoy As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents lblMovimientos7 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lblMovimientos30 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents GroupBoxUsuariosOficinas As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanelUsuarios As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblUsuariosActivos As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents lblUsuariosInactivos As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lblOficinasInternas As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents lblOficinasExternas As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents btnActualizar As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanelDocumentosTab As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents GroupBoxDocumentosEstado As System.Windows.Forms.GroupBox
    Friend WithEvents dgvDocumentosPorEstado As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBoxDocumentosTipo As System.Windows.Forms.GroupBox
    Friend WithEvents dgvDocumentosPorTipo As System.Windows.Forms.DataGridView
    Friend WithEvents TableLayoutPanelMovimientosTab As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents GroupBoxMovimientosOficina As System.Windows.Forms.GroupBox
    Friend WithEvents dgvMovimientosPorOficina As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBoxMovimientosMes As System.Windows.Forms.GroupBox
    Friend WithEvents dgvMovimientosPorMes As System.Windows.Forms.DataGridView
End Class
