Imports System.Data.Entity
Imports System.Threading.Tasks

Public Class UnitOfWork
    Implements IUnitOfWork, IDisposable

    Private ReadOnly _context As New SecretariaDBEntities()
    Private ReadOnly _repos As New Dictionary(Of Type, Object)()

    Public ReadOnly Property Context As DbContext Implements IUnitOfWork.Context
        Get
            Return _context
        End Get
    End Property

    Public Function Repository(Of T As Class)() As IRepository(Of T) Implements IUnitOfWork.Repository
        Dim key = GetType(T)

        If Not _repos.ContainsKey(key) Then
            _repos(key) = New Repository(Of T)(_context)
        End If

        Return CType(_repos(key), IRepository(Of T))
    End Function

    Public Function Commit() As Integer Implements IUnitOfWork.Commit
        Return _context.SaveChanges()
    End Function

    Public Async Function CommitAsync() As Task(Of Integer) Implements IUnitOfWork.CommitAsync
        Return Await _context.SaveChangesAsync().ConfigureAwait(False)
    End Function

    Private _disposed As Boolean = False

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _disposed Then
            If disposing Then
                _context.Dispose()
                _repos.Clear()
            End If
            _disposed = True
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
