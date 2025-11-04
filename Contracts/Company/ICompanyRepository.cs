public interface ICompanyRepository
{
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
}