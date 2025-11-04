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
        return Ok(companies);
    }
}