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
    public async Task<ActionResult> CreateEmployeeForCompany([FromBody] EmployeeCreationDto employeedto)
    {
        var companyId = _repo.Company.GetCompany(employeedto.CompanyId, false);
        if (companyId == null)
            return NotFound();
        //  i should have mapp it to the employee
        var employeeEntity = _mapper.Map<Employee>(employeedto);

        _repo.Employee.CreateEmployee(employeeEntity);
        _repo.Save();
        var employeeToReturn = _mapper.Map<EmployeeCreationDto>(employeeEntity);

        return CreatedAtRoute("id", new { id = employeeToReturn.CompanyId }, employeeToReturn);
    }
}