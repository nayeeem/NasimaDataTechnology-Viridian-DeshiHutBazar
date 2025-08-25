using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WebDeshiHutBazar
{
    public class CompanyAccountViewModel : BaseViewModel
    {
        public CompanyAccountViewModel()
        {
            ListBikashBills = new List<BikashBillTransactonViewModel>();
            ListVisitors = new List<LogPostVisitViewModel>();
            ListVisitorQueries = new List<LogPostVisitViewModel>();
            ListVisitorLikes = new List<LogPostVisitViewModel>();
            CompanyAddress = new CompanyAddressViewModel();
            CompanyBankInformation = new CompanyBankInformation();
            CompanyShopInformation = new CompanyShopInformation();
        }

        public CompanyAccountViewModel(EnumCurrency currency) : base(currency)
        {
            ListBikashBills = new List<BikashBillTransactonViewModel>();
            ListVisitors = new List<LogPostVisitViewModel>();
            ListVisitorQueries = new List<LogPostVisitViewModel>();
            ListVisitorLikes = new List<LogPostVisitViewModel>();
            CompanyAddress = new CompanyAddressViewModel();
            CompanyBankInformation = new CompanyBankInformation();
            CompanyShopInformation = new CompanyShopInformation();
        }

        public bool Agreement { get; set; }

        public long UserID { get; set; }

        public long CompanyID { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public string ClientName { get; set; }

        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        public string Website { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Re-Password")]
        public string RePassword { get; set; }

        [Display(Name = "Password")]
        public string CurrentPassword { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public string CompanyWebsite { get; set; }

        [Required]
        public string CompanyPhone { get; set; }

        [Required]
        public string CompanyEmail { get; set; }

        [Required]
        public string ShopContactEmail { get; set; }

        [Required]
        public string ShopContactName { get; set; }

        [Required]
        public string ShopContactPhoneNumber { get; set; }

        public string Remarks { get; set; }

        public long? CompanyAddressID { get; set; }

        public CompanyAddressViewModel CompanyAddress { get; set; }

        public long? ShopAddressID { get; set; }

        public CompanyAddressViewModel ShopAddress { get; set; }

        public string BankName { get; set; }

        public string BranchName { get; set; }

        public string AccountHolderName { get; set; }

        public string AccountNumber { get; set; }

        public string ACHolderContactNumber { get; set; }

        public string BkashAccountNumber { get; set; }

        public string OwnerName { get; set; }

        public string OwnerPhoneNumber { get; set; }

        public string OwnerEmail { get; set; }

        public byte[] OwnerNIDFile { get; set; }

        public byte[] TradeLicenseFile { get; set; }

        public List<BikashBillTransactonViewModel> ListBikashBills { get; set; }

        public List<LogPostVisitViewModel> ListVisitors { get; set; }

        public List<LogPostVisitViewModel> ListVisitorQueries { get; set; }

        public List<LogPostVisitViewModel> ListVisitorLikes { get; set; }

        public CompanyBankInformation CompanyBankInformation { get; set; }

        public CompanyShopInformation CompanyShopInformation { get; set; }
    }
}
