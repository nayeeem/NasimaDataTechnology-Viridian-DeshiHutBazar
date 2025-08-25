using Model;
using Common;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace WebDeshiHutBazar
{
    public interface IPaymentOptionService
    {
        Task<List<PackageDetailViewModel>> GetUserActivePackages(long userID);

        Task<int> GetUserCurrentMonthFreePublishedPostCount(long userID);

        Task<double> GetUserAccountBalance(long userID, EnumCountry country);

        Task<bool> IncreaseUserFreePostCount(long userPackageID);

        Task<bool> IncreaseUserPremiumPostCount(long userPackageID);
    }
}
