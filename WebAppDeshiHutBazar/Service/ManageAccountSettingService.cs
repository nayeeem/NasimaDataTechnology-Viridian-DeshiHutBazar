using System.Collections.Generic;
using System.Linq;
using Model;
using Common;

using System.Threading.Tasks;
using System;

namespace WebDeshiHutBazar
{
    public class ManageAccountSettingService : IManageAccountSettingService
    {
        private readonly IUserAccountService _UserAccountService;
        private readonly IPostVisitService _PostVisitService;

        public ManageAccountSettingService(IUserAccountService userAccountService,
             IPostVisitService postVisitService) {
            _UserAccountService = userAccountService;
            _PostVisitService = postVisitService;
        }

        public async Task<bool> SetAccountViewModel(long userId, AccountViewModel objAccountViewModel, 
            DateTime bdDate, EnumCountry? enumCountry, EnumCurrency currency)
        {
            var userEntity = await _UserAccountService.GetAuthorizedUser(userId);
            
            if (userEntity != null)
            {
                objAccountViewModel.ListVisitors = await _PostVisitService.GetUserAllPostVisits(userId, EnumPostVisitAction.PostVisit);
                objAccountViewModel.ListVisitorQueries = await _PostVisitService.GetUserAllPostVisits(userId, EnumPostVisitAction.PostContactQueried);
                objAccountViewModel.ListVisitorLikes = await _PostVisitService.GetUserAllPostVisits(userId, EnumPostVisitAction.PostLiked);
                objAccountViewModel.UserID = userEntity.UserID;
                objAccountViewModel.Email = userEntity.Email;
                objAccountViewModel.IsCompanySeller = userEntity.UserAccountType == EnumUserAccountType.Company ? true : false;
                objAccountViewModel.IsPrivateSeller = userEntity.UserAccountType == EnumUserAccountType.IndividualAdvertiser ? true : false;
                objAccountViewModel.Phone = userEntity.Phone;
                objAccountViewModel.ClientName = userEntity.ClientName;
                objAccountViewModel.AccountBalance = userEntity.AccountBalance;
                objAccountViewModel.DisplayCurrency = userEntity.CurrencyLongString;
                objAccountViewModel.ListPackageDetails = LoadPackageDetails(objAccountViewModel, userEntity, bdDate, enumCountry);
                BikashBillTransactonViewModel objBikashTranModel;
                List<BikashBillTransactonViewModel> objBikashTranList = new List<BikashBillTransactonViewModel>();
                foreach (var item in userEntity.ListBikashBills.ToList())
                {
                    objBikashTranModel = new BikashBillTransactonViewModel(currency)
                    {
                        CreatedDate = item.CreatedDate,
                        Status = item.Status,
                        TransactionNumber = item.TransactionNumber,
                        AgentNumber = item.AgentNumber,
                        PaidAmount = item.PaidAmount,
                        DisplayCurrency = "BDT",
                        UserOrderID = item.UserOrderID.HasValue ? item.UserOrderID.Value : 0
                    };


                    UserOrderViewModel objUserOrderModel = null;
                    UserOrderDetailViewModel objUserOrderDetailModel;
                    List<UserOrderDetailViewModel> objListUserOrderDetailModel = new List<UserOrderDetailViewModel>();
                    var userOrderEntity = item.UserOrder;
                    if (userOrderEntity != null)
                    {
                        objUserOrderModel = new UserOrderViewModel(currency);
                        objUserOrderModel.UserOrderID = userOrderEntity.UserOrderID;
                        objUserOrderModel.OrderDate = userOrderEntity.OrderDate;
                        objUserOrderModel.OrderStatus = userOrderEntity.OrderStatus;
                        objUserOrderModel.TotalBill = userOrderEntity.TotalBill;

                        var listDetails = userOrderEntity.ListOrderDetails;
                        if (listDetails != null)
                        {
                            foreach (var itemDet in listDetails)
                            {
                                objUserOrderDetailModel = new UserOrderDetailViewModel(currency);
                                objUserOrderDetailModel.PackageName = itemDet.Package != null ? itemDet.Package.PackageName : "";
                                objUserOrderDetailModel.ItemBillAomunt = itemDet.ItemBillAomunt;
                                objUserOrderDetailModel.TotalFreePost = itemDet.TotalFreePost;
                                objUserOrderDetailModel.TotalPremiumPost = itemDet.TotalPremiumPost;
                                objUserOrderDetailModel.SubscriptionPeriod = itemDet.SubscriptionPeriod;
                                objListUserOrderDetailModel.Add(objUserOrderDetailModel);
                            }
                        }
                        objUserOrderModel.ListOrderDetails = objListUserOrderDetailModel;
                    }

                    objBikashTranModel.UserOrderModel = objUserOrderModel ?? null;
                    objBikashTranList.Add(objBikashTranModel);
                }
                objAccountViewModel.ListBikashBills = objBikashTranList.OrderByDescending(a => a.CreatedDate).ToList();
            }

            return true;
        }

        private List<UserPackage> GetUserActivePackages(List<UserPackage> objListUserPackages, DateTime bdDate)
        {
            if (objListUserPackages == null)
            {
                return new List<UserPackage>();
            }
            return objListUserPackages.ToList().Where(a => a.IsActive &&
                                                           a.ExpireDate >= bdDate ||
                                                           a.ExpireDate == null).OrderBy(a => a.ExpireDate).ToList();
        }

        private List<PackageDetailViewModel> LoadPackageDetails(AccountViewModel objAccountViewModel, User userEntity, DateTime bdDate, EnumCountry? enumCountry)
        {
            PackageDetailViewModel objDetailModel;
            List<PackageDetailViewModel> objListDetailModel = new List<PackageDetailViewModel>();
            foreach (var userPackage in GetUserActivePackages(userEntity.ListUserPackages, bdDate))
            {
                objDetailModel = new PackageDetailViewModel
                {
                    UserPackageID = userPackage.UserPackageID,
                    TotalFreePost = userPackage.TotalFreePost,
                    TotalPremiumPost = userPackage.TotalPremiumPost,
                    UserTotalFreePost = userPackage.UserFreePostCount,
                    UserTotalPremiumPost = userPackage.UserPremiumPostCount,
                    PackagePrice = userPackage.PackagePrice,
                    Discount = userPackage.Discount,
                    PackageName = userPackage.PackageName,
                    Descriptinon = userPackage.Descriptinon,
                    SubscriptionPeriod = userPackage.SubscriptionPeriod,                    
                    DisplayStartDate = userPackage.IssueDate.ToShortDateString(),
                    DisplayExpiryDate = userPackage.ExpireDate.HasValue ? userPackage.ExpireDate.Value.ToShortDateString() : null,
                    CurrencyCode = userPackage.CurrencyLongString                 
                };

                objListDetailModel.Add(objDetailModel);
            }
            return objListDetailModel;
        }
    }
}
