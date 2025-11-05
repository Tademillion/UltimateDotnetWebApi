using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[Route("api/companies")]
public class CompaniesController(IRepositoryManager repo, IMapper mapper) : ControllerBase
{
    private readonly IRepositoryManager _repo = repo;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult> getCompanies()
    {
        var companies = _repo.Company.GetAllCompanies(false);
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
    [HttpGet("{id}")]// this should adde in order to get  CreatedAtRoute with correct http response
    public async Task<ActionResult> getSingleCompany(Guid id)
    {
        var company = _repo.Company.GetCompany(id, false);
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
    public async Task<ActionResult> createCompany([FromBody] CompanyForCreationDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);// convert the company to Company
        _repo.Company.CreateCompany(companyEntity);
        _repo.Save();
        var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

        return CreatedAtRoute("id", new { id = companyToReturn.Id }, companyToReturn);
    }
    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    // public IActionResult GetCompanyCollection(IEnumerable<Guid> ids)
    public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        Console.WriteLine($"ids is: {string.Join(", ", ids)}");

        if (ids == null)
        {
            return BadRequest("Parameter ids is null");
        }
        var companyEntities = _repo.Company.GetByIds(ids, trackChanges: false);
        if (ids.Count() != companyEntities.Count())
        {
            return NotFound();
        }
        var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        return Ok(companiesToReturn);
    }
    // create collection of companies 
    [HttpPost("collection")]
    public IActionResult CreateCompanyCollection([FromBody]
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
        _repo.Save();
        var companyCollectionToReturn =
        _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
        return CreatedAtRoute("CompanyCollection", new { ids },
        companyCollectionToReturn);
    }
}