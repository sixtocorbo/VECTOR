Public Class frmSelectorRangoFechas

    Public ReadOnly Property FechaDesde As Date
        Get
            Return dtpDesde.Value.Date
        End Get
    End Property

    Public ReadOnly Property FechaHasta As Date
        Get
            Return dtpHasta.Value.Date
        End Get
    End Property

    Private Sub frmSelectorRangoFechas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Dim hoy = Date.Today
        dtpHasta.Value = hoy
        dtpDesde.Value = hoy.AddDays(-30)
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        If dtpDesde.Value.Date > dtpHasta.Value.Date Then
            Notifier.Warn(Me, "La fecha desde no puede ser mayor a la fecha hasta.")
            Return
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
