using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[Route("api/companies/{companyId}/employees")]
public class EmployeControllers : ControllerBase
{
    private readonly IRepositoryManager _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeControllers> _logger;
 private readonly IDataShaper<EmployeeDto> _dataShaper;
 private readonly  EmployeeLinks _employeeLinks;
    public EmployeControllers(IRepositoryManager repo, IMapper mapper, ILogger<EmployeControllers> logger,IDataShaper<EmployeeDto> datashapper,EmployeeLinks EmployeeLinks)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
        _dataShaper=datashapper;
        _employeeLinks=EmployeeLinks;
    }
    [HttpGet]
    public async Task<ActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters parameters)
    {
        //  valiate the age range 
        if (!parameters.ValidAgeRange)
        {
            _logger.LogError("Max age can't be less than min age.");
            return BadRequest("Max age can't be less than min age.");
        }
        var company = await _repo.Company.GetCompanyAsync(companyId, trackChanges: false);
        if (company == null)
            return NotFound();

        var employeesFromDb = await _repo.Employee.GetEmployeesAsync(companyId, parameters, trackChanges: false);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(employeesFromDb.MetaData));
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
        // return Ok(employeesDto);
        
        return Ok(_dataShaper.ShapeData(employeesDto,parameters.Fields));
    }
    [HttpGet("{id}", Name = "GetEmployeeForCompany")]
    public async Task<ActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
    {
        // 
        var company = await _repo.Company.GetCompanyAsync(companyId, trackChanges: false);
        if (company == null)
            return NotFound();
            
        var employeeFromDb = await _repo.Employee.getEmployeeAsync(companyId, id, trackChanges: false);
        return Ok(employeeFromDb);
    }
    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeCreationDto employee)
    {
        //  the Route is not caseSensetive
        //  we can repllace the model state instead of using the action filter
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }
        var company = await _repo.Company.GetCompanyAsync(companyId, false);
        if (company == null)
            return NotFound();
        //  i should have map it to the employee
        var employeeEntity = _mapper.Map<Employee>(employee);

        _repo.Employee.CreateEmployee(companyId, employeeEntity);
        await _repo.SaveAsync();
        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);// output then input

        // return CreatedAtRoute("GetEmployeeForCompany", new { id = employeeToReturn.Id }, employeeToReturn);
        return Ok("the user is creatde");
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEmployeeForCompany(Guid companyid, Guid id)
    {
        var isUserExist = await _repo.Employee.getEmployeeAsync(companyid, id, false);
        if (isUserExist == null)
            return NoContent();
        var employeeEntity = _mapper.Map<Employee>(isUserExist);
        _repo.Employee.DeleteEmployee(employeeEntity);
        return NoContent();
    }
    //  updadet the employee 

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
    {
        //  
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);

        }
        if (employee == null)
        {
            _logger.LogInformation("the body is required");
            return BadRequest("the body is required");
        }
        //  get company
        var company = await _repo.Company.GetCompanyAsync(companyId, false);
        if (company == null)
        {
            _logger.LogInformation("the company with {companyID} is not exist", companyId);
            return NotFound();
        }
        //  get employee
        var employeeEntity = await _repo.Employee.getEmployeeAsync(companyId, id, trackChanges: true);
        if (employeeEntity == null)
        {
            _logger.LogInformation("the employee  withcompaniId: {companyID}  and employeeid: {id}  is not exist", companyId, id);
            return NotFound();
        }
        // 
        _mapper.Map(employee, employeeEntity);

        await _repo.SaveAsync();
        return NoContent();
    }
    //  the patch request
    [HttpPatch("{id}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
[FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid model state for the patch document");
            return UnprocessableEntity(ModelState);
        }
        if (patchDoc == null)
        {
            _logger.LogError("patchDoc object sent from client is null.");
            return BadRequest("patchDoc object is null");
        }

        var company = await _repo.Company.GetCompanyAsync(companyId, trackChanges: false);
        if (company == null)
        {
            _logger.LogInformation($"Company with id: {companyId} doesn't exist in the database.");
            return NotFound();
        }

        var employeeEntity = await _repo.Employee.getEmployeeAsync(companyId, id, trackChanges:
    true);
        if (employeeEntity == null)
        {
            _logger.LogInformation($"Employee with id: {id} doesn't exist in the database.");
            return NotFound();
        }

        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

        patchDoc.ApplyTo(employeeToPatch);

        _mapper.Map(employeeToPatch, employeeEntity);

        await _repo.SaveAsync();

        return NoContent();
    }
}