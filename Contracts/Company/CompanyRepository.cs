using System.Threading.Tasks;
using MyApp;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
  public CompanyRepository(RepositoryContext repositoryContext)
  : base(repositoryContext)
  {
  }


  //  we can acces the RepositoryBase method in this class
  public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
 FindAll(trackChanges)
   .OrderBy(c => c.Name)
   .ToList();

  public Company GetCompany(Guid companyId, bool trackChanges) =>
     FindByCondition(m => m.Id.Equals(companyId), trackChanges).SingleOrDefault();
  public void CreateCompany(Company company) => Create(company);// 


}