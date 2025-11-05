using MyApp;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
  public EmployeeRepository(RepositoryContext repositoryContext)
  : base(repositoryContext)
  {
  }

  public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges) =>
                  FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                  .OrderBy(e => e.Name);

  public Employee getEmployee(Guid companyId, Guid employeeId, bool trackChanges) =>
        FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId),
      trackChanges).SingleOrDefault();

  public void CreateEmployee(Employee employee) => Create(employee);
}



