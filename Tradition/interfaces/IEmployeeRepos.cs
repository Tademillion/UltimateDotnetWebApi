

public interface IEmployeeRepo
{
    IEnumerable<Employee> getAllEmployees();
    void CreateEmployee(Employee employee);
}