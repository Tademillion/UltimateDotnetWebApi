using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class CompaniesController : ControllerBase
{
    private readonly IRepositoryManager _repo;
    private readonly IMapper _mapper;
    public CompaniesController(IRepositoryManager repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    [HttpGet("companies")]
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
        return Ok(companiesDto);

    }
}