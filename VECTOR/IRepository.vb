Imports System.Data.Entity
Imports System.Linq.Expressions
Imports System.Threading.Tasks

Public Interface IRepository(Of T As Class)
    Function GetAll() As IQueryable(Of T)
    Function GetAllAsync() As Task(Of List(Of T))

    Function GetById(id As Integer) As T
    Function GetByIdAsync(id As Integer) As Task(Of T)

    Function GetByPredicate(predicate As Expression(Of Func(Of T, Boolean))) As T
    Function GetByPredicateAsync(predicate As Expression(Of Func(Of T, Boolean))) As Task(Of T)

    Function GetAllByPredicate(predicate As Expression(Of Func(Of T, Boolean))) As IEnumerable(Of T)
    Function GetAllByPredicateAsync(predicate As Expression(Of Func(Of T, Boolean))) As Task(Of List(Of T))

    Sub Add(entity As T)
    Sub AddRange(entities As IEnumerable(Of T))
    Sub Remove(entity As T)
    Sub RemoveRange(entities As IEnumerable(Of T))
    Sub RemoveById(id As Integer)
    Sub RemoveByPredicate(predicate As Expression(Of Func(Of T, Boolean)))
    Sub Clear()

    Sub SaveChanges()
    Function SaveChangesAsync() As Task

    Sub Update(entity As T)
    Sub UpdateRange(entities As IEnumerable(Of T))

    Function Find(predicate As Expression(Of Func(Of T, Boolean))) As IEnumerable(Of T)
    Function FindAsync(predicate As Expression(Of Func(Of T, Boolean))) As Task(Of List(Of T))

    Function Count() As Integer
    Function CountAsync() As Task(Of Integer)

    Function Any() As Boolean
    Function AnyAsync() As Task(Of Boolean)

    Function Any(predicate As Expression(Of Func(Of T, Boolean))) As Boolean
    Function AnyAsync(predicate As Expression(Of Func(Of T, Boolean))) As Task(Of Boolean)

    Function GetQueryable() As IQueryable(Of T)
    Function GetQueryable(Optional includeProperties As String = "", Optional tracking As Boolean = False) As IQueryable(Of T)
End Interface
