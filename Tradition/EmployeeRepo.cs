

public class EmployeeRepos : IEmployeeRepo
{
    private readonly RepositoryContext _context;
    public EmployeeRepos(RepositoryContext context)
    {
        _context = context;
    }

    public void CreateEmployee(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();// this is waht happening in the back now lets improve this stpes onece again
    }

    IEnumerable<Employee> IEmployeeRepo.getAllEmployees()
    {
        return _context.Employees.ToList();
    }


    // public IEnumerable<Employee> getAllEmployees =>



}