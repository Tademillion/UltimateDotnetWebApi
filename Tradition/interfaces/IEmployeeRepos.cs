

public interface IEmployeeRepo
{
    IEnumerable<Employee> getAllEmployees(int PageNumber, int PageSize);
    void CreateEmployee(Employee employee);

}