public class EmployeeCleanRepo : BaseAppRepository<Employee>, IEmployeeCleanRepo
{
    public EmployeeCleanRepo(RepositoryContext context) : base(context)
    {
        // initializing the base repo constructors base is must because the base class has paramtrized constructors
    }
    public IEnumerable<Employee> AllEmployees() => FindAll().ToList();

    public void CreateEmployees(Employee employee) =>
        Create(employee);
}
