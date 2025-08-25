using Model;
using Data;
using Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;


namespace WebDeshiHutBazar
{
    public class PaymentOptionService : IPaymentOptionService
    {
        public readonly IPackageConfigRepository _PackageConfigRepository;
        public readonly IPriceConfigRepository _PriceConfigRepository;
        public readonly IPostRepository _PostRepository;
        public readonly IPostQueryRepository _PostQueryRepository;
        public readonly IAccountBillRepository _AccountBillRepository;
        public readonly IUserRepository _UserRepository;
        public readonly IUserPackageRepository _UserPackageRepository;

        public PaymentOptionService(
            IPackageConfigRepository packageConfigRepository,
            IPriceConfigRepository priceConfigRepository,
            IPostRepository postRepository,
            IPostQueryRepository postQueryRepository,
            IAccountBillRepository accountBillRepository,
            IUserRepository userRepository,
            IUserPackageRepository userPackageRepository
            )
        {
            _PackageConfigRepository = packageConfigRepository;
            _PriceConfigRepository = priceConfigRepository;
            _PostRepository = postRepository;
            _PostQueryRepository = postQueryRepository;
            _AccountBillRepository = accountBillRepository;
            _UserRepository = userRepository;
            _UserPackageRepository = userPackageRepository;
        }  
        
        public async Task<List<PackageDetailViewModel>> GetUserActivePackages(long userID)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var userEntity = await _UserRepository.GetSingleUser(userID);
            var userPackages = userEntity.ListUserPackages.Where(a =>
                    a.IsActive &&
                    a.ExpireDate != null &&
                    a.ExpireDate.Value >= BaTime.Date &&
                    a.TotalFreePost > (a.UserFreePostCount + a.UserPremiumPostCount)).ToList();

            List<PackageDetailViewModel> listPackageDetailModel = new List<PackageDetailViewModel>();
            PackageDetailViewModel objPackageDetailsModel;
            foreach (var userPackage in userPackages.ToList())
            {
                objPackageDetailsModel = new PackageDetailViewModel();
                objPackageDetailsModel.UserPackageID = userPackage.UserPackageID;
                objPackageDetailsModel.PackageName = userPackage.PackageName;
                objPackageDetailsModel.TotalFreePost = userPackage.TotalFreePost;
                objPackageDetailsModel.TotalPremiumPost = userPackage.TotalPremiumPost;
                objPackageDetailsModel.Discount = userPackage.Discount;
                objPackageDetailsModel.PackagePrice = userPackage.PackagePrice;
                objPackageDetailsModel.SubscriptionPeriod = userPackage.SubscriptionPeriod;
                objPackageDetailsModel.UserTotalFreePost = userPackage.UserFreePostCount;
                objPackageDetailsModel.UserTotalPremiumPost = userPackage.UserPremiumPostCount;
                listPackageDetailModel.Add(objPackageDetailsModel);
            }
            return listPackageDetailModel.OrderBy(a=>a.PackageName).ToList();
        }

        public async Task<int> GetUserCurrentMonthFreePublishedPostCount(long userID)
        {
            var userPosts = await _PostQueryRepository.GetUserStartupFreeCurrentMonthPosts(userID, EnumCountry.Bangladesh);
            var freePostCount = userPosts != null ? userPosts.ToList().Count : 0;
            return freePostCount;
        }

        public async Task<double> GetUserAccountBalance(long userID, EnumCountry country)
        {
            var userEntity = await _UserRepository.GetSingleUser(userID);
            if (userEntity != null)
                return userEntity.AccountBalance;
            return 0;
        }

        public async Task<bool> IncreaseUserFreePostCount(long userPackageID)
        {
            var result  = await _UserPackageRepository.IncreaseUserPackageFreeCount(userPackageID);
            return true;
        }

        public async Task<bool> IncreaseUserPremiumPostCount(long userPackageID)
        {
            var result = await _UserPackageRepository.IncreaseUserPackagePremiumCount(userPackageID);
            return true;
        }
    }
}
