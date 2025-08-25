using System.Threading.Tasks;
using Model;
namespace Data
{
    public interface ICompanyRepository
    {
        Task<Company> GetCompanyByID(long companyID);

        Task<Company> GetCompanyByUserID(long userID);

        Task<bool> SaveCompany(Company company);

        Task<bool> UpdateCompany();
    }
}