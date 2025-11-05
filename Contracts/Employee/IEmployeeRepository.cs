public interface IEmployeeRepository
{
    IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges);
    Employee getEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateEmployee(Guid companyID, Employee employee);
    void DeleteEmployee(Employee employee);


}
