using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MyApp;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected RepositoryContext RepositoryContext;
    public RepositoryBase(RepositoryContext repositoryContext)
    {
        RepositoryContext = repositoryContext;
    }

    //  abstract class is not instantiated be instantiated on its own. 
    // Its primary purpose is to serve as a base class for other classes, providing a common foundation,
    //  shared functionality, and enforcing a specific structure or contract on its derived classes
    //  all method should not have implemnted
    public IQueryable<T> FindAll(bool trackChanges) =>
    !trackChanges ?
  RepositoryContext.Set<T>()
  .AsNoTracking() :
  RepositoryContext.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
   bool trackChanges) =>
   !trackChanges ?
   RepositoryContext.Set<T>()
   .Where(expression)
   .AsNoTracking() :
   RepositoryContext.Set<T>()
   .Where(expression);
    public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
    public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
    public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
}
