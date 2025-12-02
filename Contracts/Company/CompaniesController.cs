using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// [ApiVersion("1.0",Deprecated =true)]
[Route("api/companies")]
public class CompaniesController : ControllerBase
{
    private readonly IRepositoryManager _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<CompaniesController> _logger;
    public CompaniesController(ILogger<CompaniesController> logger, IRepositoryManager repo, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult> getCompanies()
    {
        var companies = await _repo.Company.GetAllCompaniesAsync(false);
        //  add Dto instead 
        // var companiesDto = companies.Select(c => new CompanyDto
        // {
        //     Id = c.Id,
        //     Name = c.Name,
        //     FullAddress = string.Join(' ', c.Address, c.Country)
        // }).ToList();
        //  in above we use manully mapping so Auto mapper is better
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        // throw new Exception(); to simulate  the exceptions
        return Ok(companiesDto);

    }
    [HttpGet("{id}", Name = "GetCompanyById")]// this should adde in order to get  CreatedAtRoute with correct http response
    public async Task<ActionResult> getSingleCompany(Guid id)
    {
        var company = await _repo.Company.GetCompanyAsync(id, false);
        if (company == null)
            return NotFound();
        else
        {
            var companyDto = _mapper.Map<CompanyDto>(company);
            return Ok(companyDto);
        }
    }
    //  create the company
    [HttpPost]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult> createCompany([FromBody] CompanyForCreationDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);// convert the company to Company
        _repo.Company.CreateCompany(companyEntity);
        await _repo.SaveAsync();
        var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

        return CreatedAtRoute("GetCompanyById", new { id = companyToReturn.Id }, companyToReturn);
    }
    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    // public IActionResult GetCompanyCollection(IEnumerable<Guid> ids)
    public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        Console.WriteLine($"ids is: {string.Join(", ", ids)}");

        if (ids == null)
        {
            return BadRequest("Parameter ids is null");
        }
        var companyEntities = await _repo.Company.GetByIdsAsync(ids, trackChanges: false);
        if (ids.Count() != companyEntities.Count())
        {
            return NotFound();
        }
        var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        return Ok(companiesToReturn);
    }
    // create collection of companies 
    [HttpPost("collection"),MapToApiVersion("1.0")]
    public async Task<IActionResult> CreateCompanyCollection([FromBody]
IEnumerable<CompanyForCreationDto> companyCollection)
    {
        if (companyCollection == null)
        {
            return BadRequest("Company collection is null");
        }
        var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach (var company in companyEntities)
        {
            _repo.Company.CreateCompany(company);
        }
        await _repo.SaveAsync();
        var companyCollectionToReturn =
        _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
        return CreatedAtRoute("CompanyCollection", new { ids },
        companyCollectionToReturn);
    }
    //  delete the  company
    [HttpDelete("{id}")]
    public async Task<ActionResult> deleteCompany(Guid id)
    {
        //  find the company
        var company = await _repo.Company.GetCompanyAsync(id, false);
        _logger.LogInformation("The deleted company will be {@company}", JsonSerializer.Serialize(company));


        if (company == null)
        {
            _logger.LogInformation($"Company with id: {id} doesn't exist in the database.");
            return NotFound();
        }

        _repo.Company.DeleteCompany(company);
        await _repo.SaveAsync();
        return NoContent();
    }

}