public interface IEmployeeCleanRepo
{
    IEnumerable<Employee> AllEmployees();
    void CreateEmployees(Employee employee);
}