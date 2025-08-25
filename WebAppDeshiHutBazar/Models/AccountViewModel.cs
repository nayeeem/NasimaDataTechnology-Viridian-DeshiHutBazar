using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebDeshiHutBazar
{
    public class AccountViewModel : BaseViewModel
    {
        public AccountViewModel()
        {
            ListBikashBills = new List<BikashBillTransactonViewModel>();
            ListPackageDetails = new List<PackageDetailViewModel>();
            ListVisitors = new List<LogPostVisitViewModel>();
            ListVisitorQueries = new List<LogPostVisitViewModel>();
            ListVisitorLikes = new List<LogPostVisitViewModel>();
        }

        public AccountViewModel(EnumCurrency currency) : base(currency)
        {
            ListBikashBills = new List<BikashBillTransactonViewModel>();
            ListPackageDetails = new List<PackageDetailViewModel>();
            ListVisitors = new List<LogPostVisitViewModel>();
            ListVisitorQueries = new List<LogPostVisitViewModel>();
            ListVisitorLikes = new List<LogPostVisitViewModel>();
        }

        public long UserID { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Re-Password")]
        public string RePassword { get; set; }

        [Display(Name = "Password")]
        public string CurrentPassword { get; set; }

        [Display(Name ="Pin Number")]
        public string PinNumber { get; set; }

        [Display(Name = "Pass Code")]
        public string PassCode { get; set; }

        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        public string Website { get; set; }

        [Display(Name = "Current Subscription (Package)")]
        public string CurrentPackage { get; set; }

        [Display(Name = "Package Description")]
        public string CurrentPackageDescription { get; set; }

        public string DisplayCurrency { get; set; }

        public string FBAttachedEmail { get; set; }
        
        public bool IsPrivateSeller1 { get; set; }

        public bool IsPrivateSeller2 { get; set; }
       
        public bool IsPrivateSeller { get; set; }

        public bool IsCompanySeller { get; set; }

        public bool IsAdminUser { get; private set; }

        public string AdminPersonalEmail { get; set; }

        public string Remarks { get; set; }

        public double AccountBalance { get; set; }

        public string FBUserID { get; set; }
        
        public string UserPortfolioListUrl { get; set; }

        public List<BikashBillTransactonViewModel> ListBikashBills { get; set; }

        public List<PackageDetailViewModel> ListPackageDetails { get; set; }

        public List<LogPostVisitViewModel> ListVisitors { get; set; }

        public List<LogPostVisitViewModel> ListVisitorQueries { get; set; }

        public List<LogPostVisitViewModel> ListVisitorLikes { get; set; }
    }
}
