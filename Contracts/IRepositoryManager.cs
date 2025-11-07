public interface IRepositoryManager
{
    ICompanyRepository Company { get; }
    IEmployeeRepository Employee { get; }
    Task SaveAsync();
    //  in sync we can use void Save();
    //  but in async we have to use Task SaveAsync(); because 
    //   Task, for an async method that does not return a value. 

}