using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class CompaniesController(IRepositoryManager repo, IMapper mapper) : ControllerBase
{
    private readonly IRepositoryManager _repo = repo;
    private readonly IMapper _mapper = mapper;

    [HttpGet("api/companies")]
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
    [HttpGet("api/companies/{id}")]
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
}