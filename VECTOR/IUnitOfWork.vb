Imports System.Data.Entity
Imports System.Threading.Tasks

Public Interface IUnitOfWork
    Inherits IDisposable
    ReadOnly Property Context As DbContext
    Function Repository(Of T As Class)() As IRepository(Of T)
    Function Commit() As Integer
    Function CommitAsync() As Task(Of Integer)
End Interface
