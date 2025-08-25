using System.Threading.Tasks;
using Model;

namespace Data
{
    public interface IUserPackageRepository
    {
        Task<bool> IncreaseUserPackagePremiumCount(long userPackageID);

        Task<bool> IncreaseUserPackageFreeCount(long userPackageID);
    }
}
