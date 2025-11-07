using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp;

public class getEmployees : ControllerBase
{
    private readonly RepositoryContext _Context;
    private readonly IEmployeeRepo _repo;
    public getEmployees(RepositoryContext context, IEmployeeRepo repo)
    {
        _Context = context;
        _repo = repo;
    }
    [HttpGet("employeesadd")]
    public async Task<IActionResult> AddEmployees([FromBody] Employee employee)
    {
        try
        {
            var newEmployee = new Employee
            {
                Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                Name = "Sam Raiden",
                Age = 26,
                Position = "Software developer",
                CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
            };
            var addemloyee = _Context.Employees.Add(newEmployee);
            await _Context.SaveChangesAsync();
            return Ok(addemloyee);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "internal Server Error happen");
        }
    }
    [HttpGet("employees")]
    public async Task<IActionResult> GetCompanies(int PageNumber = 1, int PageSize = 10)
    {
        try
        {
            // var employees = await _Context.Employees.ToListAsync();
            var employees = _repo.getAllEmployees(PageNumber, PageSize);
            return Ok(employees);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "Internal server error");
        }
    }

}