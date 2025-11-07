using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
  public EmployeeRepository(RepositoryContext repositoryContext)
  : base(repositoryContext)
  {
  }
  public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, [FromQuery] EmployeeParameters parameters, bool trackChanges) =>
                await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                  .OrderBy(e => e.Name).Skip((parameters.PageNumber - 1) * parameters.PageSize)
                  .Take(parameters.PageSize)
                  .ToListAsync();
  public async Task<Employee> getEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges) =>
       await FindByCondition(e => e.CompanyId.Equals(companyId)
       && e.Id.Equals(employeeId), trackChanges).SingleOrDefaultAsync();
  public void CreateEmployee(Guid companyID, Employee employee) { employee.CompanyId = companyID; Create(employee); }

  public void DeleteEmployee(Employee employee) => Delete(employee);


}



