using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

[Route("api/companies/{companyId}/employees")]
public class EmployeControllers : ControllerBase
{
    private readonly IRepositoryManager _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeControllers> _logger;

    public EmployeControllers(IRepositoryManager repo, IMapper mapper, ILogger<EmployeControllers> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
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
    [HttpGet("{id}", Name = "GetEmployeeForCompany")]
    public async Task<ActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var company = _repo.Company.GetCompany(companyId, trackChanges: false);
        if (company == null)
            return NotFound();

        var employeeFromDb = _repo.Employee.getEmployee(companyId, id, trackChanges: false);
        return Ok(employeeFromDb);
    }
    [HttpPost]
    public async Task<ActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeCreationDto employee)
    {
        //  the Route is not caseSensetive
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var company = _repo.Company.GetCompany(companyId, false);
        if (company == null)
            return NotFound();
        //  i should have map it to the employee
        var employeeEntity = _mapper.Map<Employee>(employee);

        _repo.Employee.CreateEmployee(companyId, employeeEntity);
        _repo.Save();
        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);// output then input

        // return CreatedAtRoute("GetEmployeeForCompany", new { id = employeeToReturn.Id }, employeeToReturn);
        return Ok("the user is creatde");
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEmployeeForCompany(Guid companyid, Guid id)
    {
        var isUserExist = _repo.Employee.getEmployee(companyid, id, false);
        if (isUserExist == null)
            return NoContent();
        var employeeEntity = _mapper.Map<Employee>(isUserExist);
        _repo.Employee.DeleteEmployee(employeeEntity);
        return NoContent();
    }
    //  updadet the employee 

    [HttpPut("{id}")]
    public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
    {
        //  
        if (employee == null)
        {
            _logger.LogInformation("the body is required");
            return BadRequest("the body is required");
        }
        //  get company
        var company = _repo.Company.GetCompany(companyId, false);
        if (company == null)
        {
            _logger.LogInformation("the company with {companyID} is not exist", companyId);
            return NotFound();
        }
        //  get employee
        var employeeEntity = _repo.Employee.getEmployee(companyId, id, trackChanges: true);
        if (employeeEntity == null)
        {
            _logger.LogInformation("the employee  withcompaniId: {companyID}  and employeeid: {id}  is not exist", companyId, id);
            return NotFound();
        }
        // 
        _mapper.Map(employee, employeeEntity);

        _repo.Save();
        return NoContent();
    }
    //  the patch request
    [HttpPatch("{id}")]
    public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
[FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if (patchDoc == null)
        {
            _logger.LogError("patchDoc object sent from client is null.");
            return BadRequest("patchDoc object is null");
        }

        var company = _repo.Company.GetCompany(companyId, trackChanges: false);
        if (company == null)
        {
            _logger.LogInformation($"Company with id: {companyId} doesn't exist in the database.");
            return NotFound();
        }

        var employeeEntity = _repo.Employee.getEmployee(companyId, id, trackChanges:
    true);
        if (employeeEntity == null)
        {
            _logger.LogInformation($"Employee with id: {id} doesn't exist in the database.");
            return NotFound();
        }

        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

        patchDoc.ApplyTo(employeeToPatch);

        _mapper.Map(employeeToPatch, employeeEntity);

        _repo.Save();

        return NoContent();
    }
}