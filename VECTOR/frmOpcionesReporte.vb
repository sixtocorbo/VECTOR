Public Class frmOpcionesReporte
    ' Propiedades para devolver los valores seleccionados
    Public Property ModoSeleccionUnica As Boolean = True
    Public Property FechaDesde As Date
    Public Property FechaHasta As Date
    ' NUEVA PROPIEDAD: Indica si el usuario eligió PDF en lugar de Imprimir
    Public Property EsExportacionPDF As Boolean = False

    ' Constructor que recibe si hay algo seleccionado en la grilla
    Public Sub New(haySeleccion As Boolean)
        InitializeComponent()

        ' Si no hay nada seleccionado en la grilla principal, forzamos modo Rango
        If Not haySeleccion Then
            rbSeleccionado.Enabled = False
            rbRangoFechas.Checked = True
        End If

        ConfigurarControles()
    End Sub

    Private Sub ConfigurarControles()
        Dim esRango As Boolean = rbRangoFechas.Checked
        dtpDesde.Enabled = esRango
        dtpHasta.Enabled = esRango
    End Sub

    Private Sub rbRangoFechas_CheckedChanged(sender As Object, e As EventArgs) Handles rbRangoFechas.CheckedChanged
        ConfigurarControles()
    End Sub

    Private Sub rbSeleccionado_CheckedChanged(sender As Object, e As EventArgs) Handles rbSeleccionado.CheckedChanged
        ConfigurarControles()
    End Sub

    ' Función auxiliar para validar antes de cerrar
    Private Function ValidarYGuardarDatos() As Boolean
        ModoSeleccionUnica = rbSeleccionado.Checked
        FechaDesde = dtpDesde.Value.Date
        FechaHasta = dtpHasta.Value.Date

        If Not ModoSeleccionUnica AndAlso FechaDesde > FechaHasta Then
            MessageBox.Show("La fecha 'Desde' no puede ser mayor a 'Hasta'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function

    ' Botón IMPRIMIR (Azul) - Vista Previa
    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        If ValidarYGuardarDatos() Then
            EsExportacionPDF = False ' Quiere imprimir/vista previa
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    ' Botón PDF (Rojo) - Exportar
    Private Sub btnPDF_Click(sender As Object, e As EventArgs) Handles btnPDF.Click
        If ValidarYGuardarDatos() Then
            EsExportacionPDF = True ' Quiere generar PDF
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class