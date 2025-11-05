using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[Route("api/companies/{companyId}/employees")]
public class EmployeControllers : ControllerBase
{
    private readonly IRepositoryManager _repo;
    private readonly IMapper _mapper;

    public EmployeControllers(IRepositoryManager repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
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
    [HttpPost]
    public async Task<ActionResult> CreateEmployeeForCompany(Guid companyID, [FromBody] EmployeeCreationDto employee)
    {
        //  the Route is not caseSensetive
        var companyId = _repo.Company.GetCompany(companyID, false);
        if (companyId == null)
            return NotFound();
        //  i should have map it to the employee
        var employeeEntity = _mapper.Map<Employee>(employee);

        _repo.Employee.CreateEmployee(companyID, employeeEntity);
        _repo.Save();
        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);// output then input

        return CreatedAtRoute("id", new { id = employeeToReturn.Id }, employeeToReturn);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEmployeeForCompany(Guid companyid, Guid id)
    {
        var isUserExist = _repo.Employee.getEmployee(companyid, id, false);
        if (isUserExist == null)
            return NoContent();
        var employeeEntity = _mapper.Map<Employee>(isUserExist);
        _repo.Employee.DeleteEmployee(employeeEntity);
        return Ok("the resource is deleted succesully");
    }
}