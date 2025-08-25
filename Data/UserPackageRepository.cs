using Model;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Data
{
    public class UserPackageRepository : WebBusinessEntityContext, IUserPackageRepository
    {
        public async Task<bool> IncreaseUserPackagePremiumCount(long userPackageID)
        {
            var userPackage = await _Context.UserPackages.FirstOrDefaultAsync(a => a.UserPackageID == userPackageID);
            if (userPackage != null)
            {
                var newCount = userPackage.UserPremiumPostCount + 1;
                userPackage.UserPremiumPostCount = newCount;
                var result = await _Context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> IncreaseUserPackageFreeCount(long userPackageID)
        {
            var userPackage = await _Context.UserPackages.FirstOrDefaultAsync(a => a.UserPackageID == userPackageID);
            if (userPackage != null)
            {
                userPackage.UserFreePostCount = userPackage.UserFreePostCount + 1;
                var result = await _Context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
