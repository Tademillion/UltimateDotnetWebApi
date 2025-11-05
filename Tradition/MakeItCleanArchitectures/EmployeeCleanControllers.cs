using Microsoft.AspNetCore.Mvc;

[Route("api/clean/employee")]
public class EmployeCleanControllers : ControllerBase
{
    IEmployeeCleanRepo _emp;
    public EmployeCleanControllers(IEmployeeCleanRepo repo)
    {
        _emp = repo;
    }
    [HttpGet]
    public async Task<IActionResult> getEmp()
    {
        return Ok(_emp.AllEmployees());
    }

}