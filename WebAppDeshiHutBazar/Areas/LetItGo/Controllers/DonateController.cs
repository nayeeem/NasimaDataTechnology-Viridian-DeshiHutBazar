using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;

using Model;

namespace WebDeshiHutBazar
{
    public class DonateController : BaseController
    {
        private readonly IRepoDropDownDataList _RepoDropdown;
        private readonly IBikashBillTransactionService _BikashBillTransactionService;
        private readonly IPackageConfigurationService _PackageConfigurationService;
        private readonly IUserService _UserService;
        private readonly IUserOrderService _UserOrderService;
        private readonly IUserCreditOrderService _UserCreditOrderService;
        private readonly IGroupPanelConfigService _GroupPanelConfigService;

        public DonateController() 
        { }

        public DonateController(IBikashBillTransactionService bikashBillTransactionService,
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

        public async Task<ActionResult> Donate(BikashBillTransactonViewModel objModelBikashBillTranVM)
        {                      
            objModelBikashBillTranVM.PostId = objModelBikashBillTranVM == null || objModelBikashBillTranVM.PostId == 0 || objModelBikashBillTranVM.PostId == null ? null : (int?)objModelBikashBillTranVM.PostId;
            objModelBikashBillTranVM.Currency = EnumCurrency.BDT;
            objModelBikashBillTranVM.DisplayCurrency = LocationRelatedSeed.GetCountryCurrencyDescription(EnumCountry.Bangladesh);
            if (objModelBikashBillTranVM.TransactionNumber != null && objModelBikashBillTranVM.PaidAmount != null && objModelBikashBillTranVM.PaidAmount != 0)
            {
                DonateBikashBillTransacton objBillEntityObject = _BikashBillTransactionService.CreateNewDonateBikashTranEntityObject(objModelBikashBillTranVM, COUNTRY_CODE);
                var status = await _BikashBillTransactionService.AddNewDonationBill(objBillEntityObject);
                return View(@"../../Areas/LetItGo/Views/Donate/DonateSuccess", objModelBikashBillTranVM);
            }
            objModelBikashBillTranVM.PageName = "Donate Page";
            return View(@"../../Areas/LetItGo/Views/Donate/Donate", objModelBikashBillTranVM);
        }
    }
}