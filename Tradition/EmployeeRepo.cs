

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

    IEnumerable<Employee> IEmployeeRepo.getAllEmployees(int PageNumber, int PageSize)
    {
        return _context.Employees.ToList().Skip(PageNumber - 1 * PageSize).Take(PageSize);
    }


    // public IEnumerable<Employee> getAllEmployees =>



}