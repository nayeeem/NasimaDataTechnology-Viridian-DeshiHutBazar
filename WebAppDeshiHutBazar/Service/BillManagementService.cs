using Model;
using Data;
using Common;
using System.Threading.Tasks;
using System;

namespace WebDeshiHutBazar
{
    public class BillManagementService : IBillManagementService
    {
        private readonly IPackageConfigRepository _PackageConfigRepository;
        private readonly IPriceConfigRepository _PriceConfigRepository;
        private readonly IPostRepository _PostRepository;
        private readonly IAccountBillRepository _AccountBillRepository;
        private readonly IPackageConfigurationService _PackageConfigurationService;
        private readonly IPostMangementService _PostManagementService;
        private readonly IUserService _UserService;
        private readonly IUserPackageRepository _UserPackageRepository;

        public BillManagementService(
            IPackageConfigRepository packageConfigRepository,
            IPriceConfigRepository priceConfigRepository,

            IPostRepository postRepository,
            IAccountBillRepository accountBillRepository,
            IPackageConfigurationService packageConfigurationService,
            IPostMangementService postManagementService,
            IUserService userService,
            IUserPackageRepository userPackageRepository)
        {
            _PackageConfigRepository = packageConfigRepository;
            _PriceConfigRepository = priceConfigRepository;
            _PostRepository = postRepository;
            _AccountBillRepository = accountBillRepository;
            _PackageConfigurationService = packageConfigurationService;
            _PostManagementService = postManagementService;
            _UserService = userService;
            _UserPackageRepository = userPackageRepository;
        }  

        public async Task<bool> MarkPostFree(long postId, long currentUserID, EnumCountry country)
        {
            var res = await UpdatePostStatus(postId, EnumPostStatus.FreePosted, null, currentUserID, country);
            return true;
        }

        public async Task<bool> MarkPostPayable(long postId, long currentUserID, EnumCountry country)
        {
            var res = await UpdatePostStatus(postId, EnumPostStatus.Payable, null, currentUserID, country);
            return true;
        }

        public async Task<bool> MarkPostCreditPaid(long postId, long currentUserID, EnumCountry country)
        {
            var res = await UpdatePostStatus(postId, EnumPostStatus.PaidPosted, null, currentUserID, country);
            return true;
        }

        public async Task<bool> MarkPostPremiumCreditPaid(long postId, long currentUserID, EnumCountry country)
        {
            var res = await UpdatePostStatus(postId, EnumPostStatus.PremiumPaidPosted, null, currentUserID, country);
            return true;
        }

        public async Task<bool> MarkPostSubscriptionPaid(long postId, int? userPackId, long currentUserID, EnumCountry country)
        {
            var res = await UpdatePostStatus(postId, EnumPostStatus.SubscriptionPosted, userPackId, currentUserID, country);
            return true;
        }

        public async Task<bool> MarkPostPremiumSubscriptionPaid(long postId, 
            int? userPackId, 
            long currentUserID, EnumCountry country)
        {
            var res = await UpdatePostStatus(postId, EnumPostStatus.PremiumSubscriptionPosted, userPackId, currentUserID, country);
            return true;
        }

        public async Task<bool> UpdatePostStatus(long postId, 
            EnumPostStatus postStatus, 
            int? userPackId, 
            long currentUserID, 
            EnumCountry country)
        {
            await _PostRepository.UpdatePostStatus(postId, postStatus, userPackId, currentUserID, country);
            return true;
        }

        public async Task<bool> DebitBalance(long userId, double amount, long currentUserID, EnumCountry country)
        {
            var res = await _UserService.DebitAccountBalance(userId, amount, currentUserID, country);
            return true;
        }

        public async Task<bool> UpdateAccountBillTransactionLog(long postId, 
            double amount, 
            long currentUserID, 
            EnumCountry country)
        {
            UserAccountBillTransaction objBill = new UserAccountBillTransaction(postId, amount, 
                DateTime.Now, EnumTransactionStatus.SystemApproved, country);
            var res = await AddNewBill(objBill, currentUserID);
            return true;
        }

        public async Task<bool> AddNewBill(UserAccountBillTransaction objBill, long currentUserID)
        {
            await _AccountBillRepository.AddNewBill(objBill, currentUserID);
            return true;
        }
    }
}
