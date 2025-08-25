using System.Threading.Tasks;

namespace WebDeshiHutBazar
{
    public interface ICompanyService
    {
        Task<CompanyAccountViewModel> GetCompanyByID(long companyID);

        Task<CompanyAccountViewModel> GetCompanyByUserID(long userID);

        Task<bool> SaveCompany(CompanyAccountViewModel companyViewModel);

        Task<bool> UpdateCompany(CompanyAccountViewModel companyViewModel);

        Task<bool> UpdateAccountPassword(CompanyAccountViewModel objCompanyAccount);
    }
}
