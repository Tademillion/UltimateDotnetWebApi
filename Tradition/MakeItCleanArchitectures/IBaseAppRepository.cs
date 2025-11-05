public interface IBaseAppRepository<T>
{
    //  create basic of the crud operations
    IEnumerable<T> FindAll();
    void Create(T enitity);

}