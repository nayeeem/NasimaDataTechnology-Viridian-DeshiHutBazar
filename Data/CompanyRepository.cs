using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Model;

namespace Data
{
    public class CompanyRepository : WebBusinessEntityContext, ICompanyRepository
    {
        public async Task<Company> GetCompanyByID(long companyID)
        {
            return await _Context.Companies.FirstOrDefaultAsync(a => a.CompanyID == companyID && a.IsActive);
        }

        public async Task<Company> GetCompanyByUserID(long userID)
        {
            return await _Context.Companies.FirstOrDefaultAsync(a => a.UserID == userID && a.IsActive);
        }

        public async Task<bool> SaveCompany(Company company)
        {
            if (company != null)
            {
                _Context.Companies.Add(company);
                await _Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> UpdateCompany()
        {
            await _Context.SaveChangesAsync();
            return true;
        }
    }
}
