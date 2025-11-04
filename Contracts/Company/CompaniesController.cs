using Microsoft.AspNetCore.Mvc;

public class CompaniesController : ControllerBase
{
    private readonly IRepositoryManager _repo;
    public CompaniesController(IRepositoryManager repo)
    {
        _repo = repo;
    }
    [HttpGet("companies")]
    public async Task<ActionResult> getCompanies()
    {
        var companies = _repo.Company.GetAllCompanies(false);
        //  add Dto instead 
        var companiesDto = companies.Select(c => new CompanyDto
        {
            Id = c.Id,
            Name = c.Name,
            FullAddress = string.Join(' ', c.Address, c.Country)
        }).ToList();
        //  in above we use manully mapping so Auto mapper is better
        return Ok(companiesDto);
    }
}