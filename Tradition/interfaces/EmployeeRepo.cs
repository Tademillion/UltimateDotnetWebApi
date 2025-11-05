
using System.Threading.Tasks;

namespace MyApp;

public class EmployeeRepos : IEmployeeRepo
{
    private readonly RepositoryContext _context;
    public EmployeeRepos(RepositoryContext context)
    {
        _context = context;
    }


    IEnumerable<Employee> getAllEmployees()
    {
        return _context.Employees.ToList();
    }

    IEnumerable<Employee> IEmployeeRepo.getAllEmployees()
    {
        return getAllEmployees();
    }
}