using Microsoft.AspNetCore.Mvc;

public interface IEmployeeRepository
{
    Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, [FromQuery] EmployeeParameters parameters, bool trackChanges);
    Task<Employee> getEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateEmployee(Guid companyID, Employee employee);
    void DeleteEmployee(Employee employee);


}
