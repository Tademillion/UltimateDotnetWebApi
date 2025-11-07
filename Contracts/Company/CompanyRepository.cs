using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyApp;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
  public CompanyRepository(RepositoryContext repositoryContext)
  : base(repositoryContext)
  {
  }


  //  we can acces the RepositoryBase method in this class
  public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) =>
     await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();


  public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges) =>
  await FindByCondition(m => m.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();
  public void CreateCompany(Company company) => Create(company);// 

  public async Task<IEnumerable<Company>> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
  await FindByCondition(x => ids.Contains(x.Id), trackChanges)
   .ToListAsync();

  public void DeleteCompany(Company company) => Delete(company);

  public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
  await FindByCondition(x => ids.Contains(x.Id), trackChanges)
      .ToListAsync();
}