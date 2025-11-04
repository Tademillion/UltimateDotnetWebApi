using Microsoft.AspNetCore.Mvc;

[Route("api/companies/{companyId}/employees")]
public class EmployeControllers : ControllerBase
{
    private readonly IRepositoryManager _repo;

    public EmployeControllers(IRepositoryManager repo)
    {
        _repo = repo;
    }
    [HttpGet]
    public async Task<ActionResult> GetEmployeesForCompany(Guid companyId)
    {
        var company = _repo.Company.GetCompany(companyId, trackChanges: false);
        if (company == null)
            return NotFound();

        var employeesFromDb = _repo.Employee.GetEmployees(companyId, trackChanges: false);
        return Ok(employeesFromDb);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var company = _repo.Company.GetCompany(companyId, trackChanges: false);
        if (company == null)
            return NotFound();

        var employeeFromDb = _repo.Employee.getEmployee(companyId, id, trackChanges: false);
        return Ok(employeeFromDb);
    }
}