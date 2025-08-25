using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;

using Model;



namespace WebDeshiHutBazar
{
    public class AccountSettingsController : BaseController
    {
        private readonly IRepoDropDownDataList _RepoDropdown;
        private readonly IManageAccountSettingService _ManageAccountSettingService;
        private readonly IUserAccountService _UserAccountService;
        private readonly IPackageConfigurationService _PackageConfigurationService;

        public AccountSettingsController() 
        { }

        public AccountSettingsController(
            IManageAccountSettingService manageAccountSettingService, 
            IUserAccountService userAccountService,
            IPackageConfigurationService packageConfigurationService,
            IRepoDropDownDataList repoDropdown) 
        {
            _RepoDropdown = repoDropdown;
            _ManageAccountSettingService = manageAccountSettingService;
            _UserAccountService = userAccountService;
            _PackageConfigurationService = packageConfigurationService;
        }
       
        [Authorize(Roles = "Company, Advertiser, SuperAdmin")]
        public async Task<ViewResult> ManageAccount()
        {            
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();
            var objAccountViewModel = new AccountViewModel(CURRENCY_CODE);
            await _ManageAccountSettingService.SetAccountViewModel(
                userId, 
                objAccountViewModel, 
                GetBangladeshCurrentDateTime(), 
                (EnumCountry?) COUNTRY_CODE,
                CURRENCY_CODE);
            objAccountViewModel.PageName = "Manage Account Page";
            return View(@"../../Areas/Advertizer/Views/AccountSettings/ManageAccount", objAccountViewModel);
        }

        [Authorize(Roles = "Company, Advertiser, SuperAdmin")]
        public async Task<JsonResult> UpdateAccount(AccountViewModel objAccount)
        {
            var userId = GetSessionUserId();
            var result = await _UserAccountService.UpdateAccount(objAccount);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Company, Advertiser, SuperAdmin")]
        public async Task<JsonResult> UpdateAccountPassword(AccountViewModel objAccount)
        {
            var userId = GetSessionUserId();
            var result = await _UserAccountService.UpdateAccountPassword(objAccount);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Company, Advertiser, SuperAdmin")]
        public async Task<ActionResult> ChangePackage()
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            PackageSelectInformationViewModel objInformationModel = new PackageSelectInformationViewModel()
            {
                AV_Package = await _RepoDropdown.GetPackageList()
            };
            var listPackages = await _PackageConfigurationService.GetAllActivePackages();
            List<PackageDetailViewModel> objListDetailModel = new List<PackageDetailViewModel>();
            SetPackageInformation(listPackages, objListDetailModel);           
            objInformationModel.ListPackages = objListDetailModel;
            objInformationModel.PageName = "Package Change Selection Page";
            return View(@"../../Areas/Advertizer/Views/AccountSettings/ChangePackage", objInformationModel);
        }        

        [Authorize(Roles = "Company, Advertiser, SuperAdmin")]
        private void SetPackageInformation(List<PackageConfig> listPackages, List<PackageDetailViewModel> objListDetailModel)
        {
            PackageDetailViewModel objDetailModel;
            foreach (var item in listPackages)
            {
                objDetailModel = new PackageDetailViewModel();
                objDetailModel.PackageConfigID = item.PackageConfigID;
                objDetailModel.PackageName = item.PackageName;
                objDetailModel.Descriptinon = item.Descriptinon;
                objDetailModel.TotalFreePost = item.TotalFreePost;
                objDetailModel.TotalPremiumPost = item.TotalPremiumPost;
                objDetailModel.PackagePrice = item.PackagePrice;
                objDetailModel.Discount = item.Discount;
                objDetailModel.SubscriptionPeriod = item.SubscriptionPeriod;
                objListDetailModel.Add(objDetailModel);
            }
        }
    }
}