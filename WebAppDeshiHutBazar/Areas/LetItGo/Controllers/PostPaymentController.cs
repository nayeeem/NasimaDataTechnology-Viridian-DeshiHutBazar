using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Model;


namespace WebDeshiHutBazar
{
    public partial class PostController : BaseController
    {
        public async Task<ActionResult> PaymentOptions(long? subCategoryId, long postId)
        {
            PostPaymentOptionViewModel objPaymentOptionViewModel = new PostPaymentOptionViewModel(CURRENCY_CODE);
            var currentUserID = GetSessionUserId();
            var defaultPackage = await _PackageConfigurationService.GetDefaultStartupPackage();
            var priceConfigList = defaultPackage.ListPostPriceConfigs.Where(a => a.SubCategoryID == subCategoryId.Value).ToList();
            var result = await GetPaymentOptionViewModel(priceConfigList, subCategoryId, postId, objPaymentOptionViewModel, currentUserID);
            if (!HasToRecharge(objPaymentOptionViewModel))
            {
                //var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
                //var viewMoreUrl = Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" });
                //objPaymentOptionViewModel.ContentInfoViewModel.ListGroupPanelConfiguration =
                //                         await _GroupPanelConfigService.GetAllPageGroupPanelConfig(EnumPublicPage.PaymentOption, viewMoreUrl, viewPostDetUrl, COUNTRY_CODE);                
                return View(@"../../Areas/LetItGo/Views/Post/PaymentOptions", objPaymentOptionViewModel);
            }
            else
            {
                BikashBillTransactonViewModel objModelBikashBillTranVM = new BikashBillTransactonViewModel(CURRENCY_CODE);
                SetSessionRechargeSubCategoryID((int?)subCategoryId);
                GetBillTransactionViewModel(priceConfigList, subCategoryId, postId, objModelBikashBillTranVM, currentUserID);

                //var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
                //var viewMoreUrl = Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" });
                //objModelBikashBillTranVM.ContentInfoViewModel.ListGroupPanelConfiguration =
                //                         await _GroupPanelConfigService.GetAllPageGroupPanelConfig(EnumPublicPage.RechargeNotification, viewMoreUrl, viewPostDetUrl, COUNTRY_CODE);
                return View(@"../../Areas/LetItGo/Views/Post/RechargeNotification", objModelBikashBillTranVM);
            }
        }

        private void GetBillTransactionViewModel(List<PriceConfig> priceConfigList, long? subCategoryId, long postId, BikashBillTransactonViewModel objModelBikashBillTranVM, long currentUserID)
        {
            objModelBikashBillTranVM.PostId = (int?)postId;
            objModelBikashBillTranVM.PayableAmount = GetGeneralPrice(priceConfigList).ToString();
            objModelBikashBillTranVM.PostPrice = GetGeneralPrice(priceConfigList);
            objModelBikashBillTranVM.PremiumPostPrice = GetPremiumPrice(priceConfigList);
            objModelBikashBillTranVM.Currency = CURRENCY_CODE;
            objModelBikashBillTranVM.DisplayCurrency = LocationRelatedSeed.GetCountryCurrencyDescription(EnumCountry.Bangladesh);
            objModelBikashBillTranVM.SubCatId = (int?)subCategoryId;
            objModelBikashBillTranVM.DisplaySubCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId((long)objModelBikashBillTranVM.SubCatId);
            objModelBikashBillTranVM.PageName = "Recharge Notification Page";
        }

        private async Task<bool> GetPaymentOptionViewModel(List<PriceConfig> priceConfigList, long? subCategoryId, long postId, PostPaymentOptionViewModel objPaymentOptionViewModel, long currentUserID)
        {
            if (priceConfigList != null)
            {
                objPaymentOptionViewModel.PostPrice = GetGeneralPrice(priceConfigList);
                objPaymentOptionViewModel.PremiumPostPrice = GetPremiumPrice(priceConfigList);
                objPaymentOptionViewModel.Currency = CURRENCY_CODE;
            }
            objPaymentOptionViewModel.HasFreeQuota = await DoesUserHasFreeQuota(currentUserID);
            objPaymentOptionViewModel.HasCreditBalance = await DoesUserHasCreditBalance(objPaymentOptionViewModel.PostPrice, currentUserID);
            objPaymentOptionViewModel.HasPremiumCreditBalance = await DoesUserHasPremiumCreditBalance(objPaymentOptionViewModel.PremiumPostPrice, currentUserID);
            objPaymentOptionViewModel.ListActiveUserPackages = await GetListActiveUserPackages(currentUserID);
            objPaymentOptionViewModel.HasSubscription = await HasSubscription(currentUserID);
            objPaymentOptionViewModel.HasPremiumSubscription = await HasPremiumSubscription(currentUserID);
            objPaymentOptionViewModel.PostId = postId;
            objPaymentOptionViewModel.SubCategoryID = subCategoryId;
            objPaymentOptionViewModel.DisplaySubCategory = BusinessObjectSeed.GetCateSubCategoryItemText(subCategoryId);
            objPaymentOptionViewModel.PageName = "Payment Option Page";
            objPaymentOptionViewModel.FreePostURL = Url.Action("PublishFree", "Post", new { postId = objPaymentOptionViewModel.PostId });
            objPaymentOptionViewModel.PaidPostURL = Url.Action("PublishCreditPaid", "Post", new { postId = objPaymentOptionViewModel.PostId, price = objPaymentOptionViewModel.PostPrice });
            objPaymentOptionViewModel.PremiumPaidPostURL = Url.Action("PublishPremiumCreditPaid", "Post", new { postId = objPaymentOptionViewModel.PostId, price = objPaymentOptionViewModel.PremiumPostPrice });
            foreach (var item in objPaymentOptionViewModel.ListActiveUserPackages.ToList())
            {
                item.SubscriptionPostURL = Url.Action("PublishSubscription", "Post", new { postId = objPaymentOptionViewModel.PostId, userPackageId = item.UserPackageID });
                item.PremiumSubscriptionPostURL = Url.Action("PublishPremiumSubscription", "Post", new { postId = objPaymentOptionViewModel.PostId, userPackageId = item.UserPackageID });
            }
            return true;
        }

        private double? GetPremiumPrice(List<PriceConfig> priceConfigList)
        {
            var price = priceConfigList.FirstOrDefault(a => a.OfferType == EnumOfferType.Premium);
            if (price != null)
                return price.OfferPrice;
            return 0;
        }

        private double? GetGeneralPrice(List<PriceConfig> priceConfigList)
        {
            var price = priceConfigList.FirstOrDefault(a => a.OfferType == EnumOfferType.General);
            if (price != null)
                return price.OfferPrice;
            return 0;
        }

        private async Task<bool> HasSubscription(long currentUserID)
        {
            var listPackages = await _PaymentOptionService.GetUserActivePackages(currentUserID);
            return listPackages.Count > 0;
        }

        private async Task<bool> HasPremiumSubscription(long currentUserID)
        {
            var listPackages = await _PaymentOptionService.GetUserActivePackages(currentUserID);
            return listPackages.Any(a => a.TotalPremiumPost > a.UserTotalPremiumPost);
        }

        private async Task<bool> DoesUserHasFreeQuota(long currentUserID)
        {
            var userTotalPublishCurrentMonth = await _PaymentOptionService.GetUserCurrentMonthFreePublishedPostCount(currentUserID);
            var freeQuotaCount = Convert.ToInt32(ConfigurationManager.AppSettings["MonthlyFreePostQuota"]);
            if (userTotalPublishCurrentMonth >= freeQuotaCount)
                return false;
            return true;
        }

        private async Task<List<PackageDetailViewModel>> GetListActiveUserPackages(long currentUserID)
        {
            var listPackages = await _PaymentOptionService.GetUserActivePackages(currentUserID);
            return listPackages;
        }

        private async Task<bool> DoesUserHasPremiumCreditBalance(double? premiumPrice, long currentUserID)
        {
            if (!premiumPrice.HasValue)
                return true;
            var currentAccountBalance = await _PaymentOptionService.GetUserAccountBalance(currentUserID, EnumCountry.Bangladesh);
            return currentAccountBalance >= premiumPrice;
        }
        
        private async Task<bool> DoesUserHasCreditBalance(double? price, long currentUserID)
        {
            if (!price.HasValue)
                return true;
            var currentAccountBalance = await _PaymentOptionService.GetUserAccountBalance(currentUserID, EnumCountry.Bangladesh);
            return currentAccountBalance >= price;
        }

        private bool HasToRecharge(PostPaymentOptionViewModel objPaymentOptionViewModel)
        {
            if (!objPaymentOptionViewModel.HasFreeQuota &&
                !objPaymentOptionViewModel.HasCreditBalance &&
                !objPaymentOptionViewModel.HasPremiumCreditBalance &&
                !objPaymentOptionViewModel.HasSubscription &&
                !objPaymentOptionViewModel.HasPremiumSubscription)
                return true;
            return false;
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> PublishFree(long postId)
        {
            var markFree = await _BillManagementService.MarkPostFree(postId, GetSessionUserId(), COUNTRY_CODE);
            return RedirectToAction("OnlyPublishedPosts", "ManagePost");
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> PublishPremiumSubscription(long postId, long userPackageId)
        {
            var result1 = await _BillManagementService.MarkPostPremiumSubscriptionPaid(postId, (int?) userPackageId, GetSessionUserId(), COUNTRY_CODE);
            var result3 = await _PaymentOptionService.IncreaseUserPremiumPostCount(userPackageId);
            return RedirectToAction("OnlyPublishedPosts", "ManagePost");
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> PublishSubscription(long postId, long userPackageId)
        {
            var result1 = await _BillManagementService.MarkPostSubscriptionPaid(postId, (int?)userPackageId, 
                GetSessionUserId(),
                COUNTRY_CODE);
            var result3 = await _PaymentOptionService.IncreaseUserFreePostCount(userPackageId);
            return RedirectToAction("OnlyPublishedPosts", "ManagePost");
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> PublishPremiumCreditPaid(long postId, double price)
        {
            var result1 = await _BillManagementService.MarkPostPremiumCreditPaid(postId, GetSessionUserId(),COUNTRY_CODE);
            var result3 = await _BillManagementService.DebitBalance(GetSessionUserId(), price, GetSessionUserId(), COUNTRY_CODE);
            var result4 = await _BillManagementService.UpdateAccountBillTransactionLog(postId, price, GetSessionUserId(), COUNTRY_CODE);
            return RedirectToAction("OnlyPublishedPosts", "ManagePost");
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> PublishCreditPaid(long postId, double price)
        {
            var result1 = await _BillManagementService.MarkPostCreditPaid(postId, GetSessionUserId(), COUNTRY_CODE);
            var result2 = await _BillManagementService.DebitBalance(GetSessionUserId(), price, GetSessionUserId(), COUNTRY_CODE);
            var result3 = await _BillManagementService.UpdateAccountBillTransactionLog(postId, price, GetSessionUserId(),COUNTRY_CODE);
            return RedirectToAction("OnlyPublishedPosts", "ManagePost");
        }
    }
}