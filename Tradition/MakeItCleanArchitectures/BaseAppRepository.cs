public abstract class BaseAppRepository<T> : IBaseAppRepository<T> where T : class
{
    private readonly RepositoryContext _repoContext;
    public BaseAppRepository(RepositoryContext context)
    {
        _repoContext = context;
    }

    public void Create(T enitity)
    {
        _repoContext.Set<T>().Add(enitity);
    }

    public IEnumerable<T> FindAll()
    {
        return _repoContext.Set<T>();
    }
}