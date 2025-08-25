using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;

using Model;

namespace WebDeshiHutBazar
{
    public class ShoppingController : BaseController
    {
        private readonly IRepoDropDownDataList _RepoDropdown;
        private readonly IBikashBillTransactionService _BikashBillTransactionService;
        private readonly IPackageConfigurationService _PackageConfigurationService;
        private readonly IUserService _UserService;
        private readonly IUserOrderService _UserOrderService;
        private readonly IUserCreditOrderService _UserCreditOrderService;
        private readonly IGroupPanelConfigService _GroupPanelConfigService;

        public ShoppingController() 
        { }

        public ShoppingController(IBikashBillTransactionService bikashBillTransactionService,
                                  IPackageConfigurationService packageConfigurationService,
                                  IUserService userService,
                                  IUserOrderService userOrderService,
                                  IUserCreditOrderService userCreditOrderService,
                                  IRepoDropDownDataList repoDropdown,
                                  IGroupPanelConfigService groupPanelConfigService)
        {
            _RepoDropdown = repoDropdown;
            _BikashBillTransactionService = bikashBillTransactionService;
            _PackageConfigurationService = packageConfigurationService;
            _UserService = userService;
            _UserOrderService = userOrderService;
            _UserCreditOrderService = userCreditOrderService;
            _GroupPanelConfigService = groupPanelConfigService;
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> ChangePackage()
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            PackageSelectInformationViewModel objInformationModel = new PackageSelectInformationViewModel();
            await SetPackageSelectInformationModel(objInformationModel, (EnumCountry?) COUNTRY_CODE);
            objInformationModel.ShoppingCart = GetPackageShoppingCart();
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
            var viewMoreUrl = Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" });
            return View(@"../../Areas/Advertizer/Views/Shopping/ChangePackage", objInformationModel);
        }

        [Authorize(Roles = "Company, Advertiser")]
        private async Task<bool> SetPackageSelectInformationModel(PackageSelectInformationViewModel objInformationModel, EnumCountry? enumCountry)
        {
            objInformationModel.AV_Package = await _RepoDropdown.GetPackageList();
            objInformationModel.PageName = "Package Change Selection Page";
            var listActivePackageEntities = await _PackageConfigurationService.GetAllActivePackages();
            PackageDetailViewModel objDetailModel;
            List<PackageDetailViewModel> objListDetailModel = new List<PackageDetailViewModel>();
            foreach (var packageEntity in listActivePackageEntities)
            {
                objDetailModel = new PackageDetailViewModel()
                {
                    PackageConfigID = packageEntity.PackageConfigID,
                    PackageName = packageEntity.PackageName,
                    Descriptinon = packageEntity.Descriptinon,
                    TotalFreePost = packageEntity.TotalFreePost,
                    TotalPremiumPost = packageEntity.TotalPremiumPost,
                    PackagePrice = packageEntity.PackagePrice,
                    Discount = packageEntity.Discount                   
                };
                List<PostPriceConfigurationViewModel> objListPriceConfig = new List<PostPriceConfigurationViewModel>();                
                objListDetailModel.Add(objDetailModel);
            }
            objInformationModel.ListPackages = objListDetailModel;
            return true;
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> Recharge(BikashBillTransactonViewModel objModelBikashBillTranVM)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var vSubCatID = GetSessionRechargeSubCategoryID();
            vSubCatID = vSubCatID ?? -1;
            var defaultPackage = await _PackageConfigurationService.GetDefaultStartupPackage();
            var listPrices = defaultPackage.ListPostPriceConfigs;
            double POST_ITEM_PRICE = GetCatItemPrice(listPrices, vSubCatID);
            objModelBikashBillTranVM.UserId = (int) userId;
            objModelBikashBillTranVM.AgentNumber = "01765805853 or 01742600276";
            objModelBikashBillTranVM.PostId = objModelBikashBillTranVM == null || objModelBikashBillTranVM.PostId == 0 || objModelBikashBillTranVM.PostId == null ? null : (int?)objModelBikashBillTranVM.PostId;
            objModelBikashBillTranVM.PayableAmount = POST_ITEM_PRICE.ToString() == "0" ? "Any amount" : POST_ITEM_PRICE.ToString();
            objModelBikashBillTranVM.Currency = CURRENCY_CODE;
            objModelBikashBillTranVM.DisplayCurrency = LocationRelatedSeed.GetCountryCurrencyDescription(EnumCountry.Bangladesh);
            objModelBikashBillTranVM.SubCatId = vSubCatID;
            objModelBikashBillTranVM.DisplaySubCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId((long)vSubCatID);
            if (objModelBikashBillTranVM.TransactionNumber != null && objModelBikashBillTranVM.PaidAmount != null && objModelBikashBillTranVM.PaidAmount != 0)
            {
                var objUserEntity = await _UserService.GetSingleUser((int)GetSessionUserId());
                UserCreditOrder objUserCreditOrderEntity = new UserCreditOrder(objUserEntity, objModelBikashBillTranVM.PaidAmount, COUNTRY_CODE);
                var resultAdd = await _UserCreditOrderService.AddUserCreditOrder(objUserCreditOrderEntity);
                BikashBillTransacton objBillEntityObject = _BikashBillTransactionService.CreateNewBikashTranEntityObject(objModelBikashBillTranVM, objUserCreditOrderEntity, COUNTRY_CODE);
                var status = await _BikashBillTransactionService.AddNewBill(objBillEntityObject);
                ClearSessionRechargeSubCategoryID();
                return RedirectToAction("ManageAccount", "AccountSettings");
            }
            objModelBikashBillTranVM.PageName = "Recharge Page";
            return View(@"../../Areas/Advertizer/Views/Shopping/Recharge", objModelBikashBillTranVM);
        }

        [Authorize(Roles = "Company, Advertiser")]
        private double GetCatItemPrice(List<PriceConfig> listPrices, int? vSubCatID)
        {
            double price = 0;
            if (listPrices != null)
            {
                var priceObjEntity = listPrices.ToList().FirstOrDefault(a => a.SubCategoryID == vSubCatID);
                if (priceObjEntity != null)
                {
                    price = priceObjEntity.OfferPrice ?? 0;
                }
                else
                {
                    price = 0;
                }
            }
            return price;
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> PackageRecharge(BikashBillTransactonViewModel objModel)
        {   
            var currentUserID = GetSessionUserId();
            if (currentUserID == -1)
                CheckLogoutRequirements();

            User userObjectEntity = await _UserService.GetSingleUser(Convert.ToInt64(currentUserID));

            var objUserOrderObjectEntity = await _UserOrderService.GetSingleUserOrder(objModel.UserOrderID);
            objModel.UserId = (int) userObjectEntity.UserID;
            objModel.AgentNumber = "01765805853";
            objModel.PayableAmount = objUserOrderObjectEntity.TotalBill.Value.ToString();
            objModel.Currency = CURRENCY_CODE;
            objModel.DisplayCurrency = LocationRelatedSeed.GetCountryCurrencyDescription(EnumCountry.Bangladesh);
            if (objModel.TransactionNumber != null && objModel.PaidAmount != null && objModel.PaidAmount.Value != 0)
            {
                BikashBillTransacton objBill = new BikashBillTransacton
                            (objUserOrderObjectEntity,
                            userObjectEntity,
                            objModel.TransactionNumber,
                            objModel.AgentNumber,
                            objModel.PaidAmount, COUNTRY_CODE)
                {
                    EnumCountry = COUNTRY_CODE                    
                };                
                var status = await _BikashBillTransactionService.AddNewBill(objBill);
                ClearShoppingCartSession();
                ClearSessionRechargeSubCategoryID();
                return RedirectToAction("ManageAccount", "AccountSettings");
            }
            objModel.PageName = "Package Recharge Page";
            objModel.MenuObjectModel = new MenuObjectModel();
            objModel.MenuObjectModel.AV_State = DropDownDataList.GetAllStateList();
            objModel.MenuObjectModel.AV_Category = DropDownDataList.GetCategoryList();
            objModel.MenuObjectModel.AV_SubCategory = DropDownDataList.GetSubCategoryList();

            return View(@"../../Areas/Advertizer/Views/Shopping/PackageRecharge", objModel);
        }

        [HttpGet]
        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> Checkout()
        {
            var currentUserID = GetSessionUserId();
            if (currentUserID == -1)
                CheckLogoutRequirements();

            User userObjectEntity = await _UserService.GetSingleUser(Convert.ToInt64(currentUserID));
            var cartListItems = GetPackageShoppingCart();
            double totalBill = GetTotalBill(cartListItems);
            var currentOrderID = GetUserOrderID();
            if (currentOrderID.HasValue)
            {
                var resultDelete = await _UserOrderService.DeleteUserOrder(currentOrderID.Value);
            }
            UserOrder objUserOrderObjectEntity = new UserOrder(userObjectEntity, totalBill, COUNTRY_CODE)
            {
                OrderStatus = EnumPackageOrderStatus.Saved
            };
            var result = await _UserOrderService.AddUserOrder(objUserOrderObjectEntity);
            List<UserOrderDetail> objListUserOrderDetails = new List<UserOrderDetail>();
            UserOrderDetail objUserOrderDetailObjectEntity;
            foreach (var cartItem in cartListItems.ToList())
            {
                var packageID = cartItem.PackageConfigID;
                var packageObjectEntity = await _PackageConfigurationService.GetSinglePackage((int)packageID);
                objUserOrderDetailObjectEntity = new UserOrderDetail(
                    packageObjectEntity,
                    objUserOrderObjectEntity,
                    cartItem.SubscriptionPeriod, COUNTRY_CODE
                )
                {
                    EnumCountry = EnumCountry.Bangladesh,
                    IsActive = true
                };
                objListUserOrderDetails.Add(objUserOrderDetailObjectEntity);
            }
            objUserOrderObjectEntity.AddOrderDetailList(objListUserOrderDetails);
            var resultUpdate = await _UserOrderService.UpdateUserOrder(objUserOrderObjectEntity);
            BikashBillTransactonViewModel objModelLocal = new BikashBillTransactonViewModel(CURRENCY_CODE);
            objModelLocal.UserOrderID = objUserOrderObjectEntity.UserOrderID;
            SetUserOrderID((long?)objUserOrderObjectEntity.UserOrderID);
            return RedirectToAction("PackageRecharge", objModelLocal);
        }

        [HttpGet]
        [Authorize(Roles = "Company, Advertiser")]
        public async Task<PartialViewResult> GetCartItem(int packageId)
        {
            ShoppingCartViewModel objCartModel = new ShoppingCartViewModel(CURRENCY_CODE);
            List<ShoppingCartViewModel> cartItems = GetPackageShoppingCart();
            if (cartItems == null)
                cartItems = new List<ShoppingCartViewModel>();
            var packageConfig = await _PackageConfigurationService.GetSinglePackage(packageId);
            if (packageConfig != null)
            {
                objCartModel.PackageConfigID = packageConfig.PackageConfigID;
                objCartModel.PackageName = packageConfig.PackageName;
                objCartModel.PackagePrice =  packageConfig.PackagePrice;
                objCartModel.MonthlyFee = GetMonthlyFee(packageConfig.SubscriptionPeriod, packageConfig.PackagePrice);
                objCartModel.YearlyFee = GetYearlyFee(packageConfig.SubscriptionPeriod, packageConfig.PackagePrice);
                objCartModel.NoTotalFree = packageConfig.TotalFreePost;
                objCartModel.NoTotalPremiumFree = packageConfig.TotalPremiumPost;
                objCartModel.SubscriptionPeriod = packageConfig.SubscriptionPeriod;
                var count = cartItems.Count;
                objCartModel.SlNo = count + 1;
                cartItems.Add(objCartModel);
            }
            SetPackageShoppingCart(cartItems);
            return PartialView(@"../../Areas/Advertizer/Views/Shopping/_SingleItemCart", objCartModel);
        }

        private double GetYearlyFee(EnumPackageSubscriptionPeriod subscriptionPeriod, double packagePrice)
        {
            if(subscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
            {
                return packagePrice * 12;
            }
            else if(subscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
            {
                return packagePrice;
            }
            return packagePrice;
        }

        private double GetMonthlyFee(EnumPackageSubscriptionPeriod subscriptionPeriod, double packagePrice)
        {
            if (subscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
            {
                return packagePrice;
            }
            else if (subscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
            {
                return packagePrice/12;
            }
            return packagePrice;
        }

        [HttpGet]
        [Authorize(Roles = "Company, Advertiser")]
        public async Task<PartialViewResult> GetCartItemMobile(int packageId)
        {
            ShoppingCartViewModel objCartModel = new ShoppingCartViewModel(CURRENCY_CODE);
            List<ShoppingCartViewModel> cartItems = GetPackageShoppingCart();
            if (cartItems == null)
                cartItems = new List<ShoppingCartViewModel>();
            var packageConfig = await _PackageConfigurationService.GetSinglePackage(packageId);
            if (packageConfig != null)
            {
                objCartModel.PackageConfigID = packageConfig.PackageConfigID;
                objCartModel.PackageName = packageConfig.PackageName;
                objCartModel.YearlyFee = GetYearlyFee(packageConfig.SubscriptionPeriod, packageConfig.PackagePrice);
                objCartModel.MonthlyFee = GetMonthlyFee(packageConfig.SubscriptionPeriod, packageConfig.PackagePrice);
                objCartModel.NoTotalFree = packageConfig.TotalFreePost;
                objCartModel.NoTotalPremiumFree = packageConfig.TotalPremiumPost;
                objCartModel.SubscriptionPeriod = packageConfig.SubscriptionPeriod;
                var count = cartItems.Count;
                objCartModel.SlNo = count + 1;
                cartItems.Add(objCartModel);
            }
            SetPackageShoppingCart(cartItems);
            return PartialView(@"../../Areas/Advertizer/Views/Shopping/_SingleItemCartMobile", objCartModel);
        }

        [HttpGet]
        [Authorize(Roles = "Company, Advertiser")]
        public JsonResult UpdatePayCycle(long serialNo, int payCycle)
        {
            var cartItems = GetPackageShoppingCart();
            if (cartItems == null || cartItems.Count == 0)
                return Json(false, JsonRequestBehavior.AllowGet);
            var singleItem = cartItems.FirstOrDefault(a => a.SlNo == serialNo);
            if (singleItem != null)
                singleItem.SubscriptionPeriod = payCycle == 1 ? EnumPackageSubscriptionPeriod.Month : EnumPackageSubscriptionPeriod.Year;
            SetPackageShoppingCart(cartItems);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Company, Advertiser")]
        public JsonResult RemoveCartItem(long serialNo)
        {
            var cartItems = GetPackageShoppingCart();
            if (cartItems == null || cartItems.Count == 0)
                return Json(false, JsonRequestBehavior.AllowGet);
            var singleItem = cartItems.FirstOrDefault(a => a.SlNo == serialNo);
            if (singleItem != null)
                cartItems.Remove(singleItem);
            SetPackageShoppingCart(cartItems);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Company, Advertiser")]
        public JsonResult GetCartTotalPrice()
        {
            var cartItems = GetPackageShoppingCart();
            if (cartItems == null || cartItems.Count == 0)
                return Json(0, JsonRequestBehavior.AllowGet);
            double sum = 0;
            foreach(var item in cartItems)
            {
                if (item.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    sum += item.MonthlyFee;
                }
                else
                {
                    sum += item.YearlyFee;
                }
            }
            SetPackageShoppingCart(cartItems);
            return Json(sum, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Company, Advertiser")]
        private double GetTotalBill(List<ShoppingCartViewModel> cartListItems)
        {
            double sum = 0;
            foreach(var item in cartListItems)
            {
                if (item.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    sum += item.MonthlyFee;
                }
                else
                {
                    sum += item.YearlyFee;
                }
            }
            return sum;
        }        
    }
}