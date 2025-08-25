using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Common;
using System.ComponentModel.DataAnnotations;
using Common.Language;

namespace WebDeshiHutBazar
{
    public class BikashBillTransactonViewModel : BaseViewModel
    {
        public BikashBillTransactonViewModel()
        {
            ContentInfoViewModel = new ContentInfoViewModel();
            MenuObjectModel = new MenuObjectModel();
            MenuObjectModel.AV_State = DropDownDataList.GetAllStateList();
            MenuObjectModel.AV_Category = DropDownDataList.GetCategoryList();
            MenuObjectModel.AV_SubCategory = DropDownDataList.GetSubCategoryList();
        }

        public BikashBillTransactonViewModel(EnumCurrency currency) : base(currency)
        {
            UserOrderModel = new UserOrderViewModel(currency);
            ContentInfoViewModel = new ContentInfoViewModel();
            MenuObjectModel = new MenuObjectModel();
            MenuObjectModel.AV_State = DropDownDataList.GetAllStateList();
            MenuObjectModel.AV_Category = DropDownDataList.GetCategoryList();
            MenuObjectModel.AV_SubCategory = DropDownDataList.GetSubCategoryList();
        }

        public long UserOrderID { get; set; }

        public long UserCreditOrderID { get; set; }

        public long BikashBillId { get; set; }

        [Display(Name = "PayableAmount", ResourceType = typeof(LanguageDefault))]
        public string PayableAmount { get; set; }

        [Display(Name = "MonthlyFee", ResourceType = typeof(LanguageDefault))]
        public double? MonthlyFee { get; set; }

        [Display(Name = "YearlyFee", ResourceType = typeof(LanguageDefault))]
        public double? YearlyFee { get; set; }        

        [Display(Name = "DisplayCurrency", ResourceType = typeof(LanguageDefault))]
        public string DisplayCurrency { get; set; }

        [Display(Name = "DisplaySubCategory", ResourceType = typeof(LanguageDefault))]
        public string DisplaySubCategory { get; set; }

        [Required]
        [Display(Name = "TransactionNumber", ResourceType = typeof(LanguageDefault))]
        public string TransactionNumber { get; set; }

        [Display(Name = "AgentNumber", ResourceType = typeof(LanguageDefault))]
        public string AgentNumber { get; set; }

        public IEnumerable<SelectListItem> AV_PaymentTime { get; set; }

        public string Paymentway { get; set; }

        [Required]
        [Display(Name = "PaidAmount", ResourceType = typeof(LanguageDefault))]
        public double? PaidAmount { get; set; }
        
        public string FormattedPaidAmountValue { get; set; }

        [Display(Name = "Entry Date:")]
        public DateTime EntryDateTime { get; set; }

        public EnumTransactionStatus AdminApprovalStatus { get; set; }

        [Display(Name = "Status", ResourceType = typeof(LanguageDefault))]
        public string Status { get; set; }

        public int UserId { get; set; }

        public int? PostId { get; set; }

        public int? SubCatId { get; set; }    
        
        public UserOrderViewModel UserOrderModel { get; set; }

        public bool ShowRechargeUserMessage { get; set; }

        public double? PostPrice { get; set; }

        public double? PremiumPostPrice { get; set; }

        public ContentInfoViewModel ContentInfoViewModel { get; set; }

        public decimal ProductCartFinalTotalBillAmount { get; set; }
    }
}
