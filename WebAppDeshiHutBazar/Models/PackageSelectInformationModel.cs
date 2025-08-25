using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WebDeshiHutBazar
{
    public class PackageSelectInformationViewModel : UserViewModel
    {
        public PackageSelectInformationViewModel() {
            ListPackages = new List<PackageDetailViewModel>();
            ShoppingCart = new List<ShoppingCartViewModel>();
            ContentInfoViewModel = new ContentInfoViewModel();
        }

        public int? PackageID { get; set; }

        [Display(Name = "Subscription Price (Monthly)")]
        public double? MonthlySubscriptionPrice { get; set; }

        [Display(Name = "Subscription Price (Yearly)")]
        public double? YearlySubscriptionPrice { get; set; }

        [Display(Name = "Discount (Monthly Subscription)")]
        public int? DiscountMonthlySubscription { get; set; }

        [Display(Name = "Discount (Yearly Subscription)")]
        public int? DiscountYearlySubscription { get; set; }

        [Display(Name = "Package Name")]
        public string PackageName { get; set; }

        [Display(Name = "Descriptinon")]
        public string Descriptinon { get; set; }

        [Display(Name = "Monthly Total Free")]
        public int? PackageTotalFreePost { get; set; }

        public bool IsMonthlyPaymentSubscription { get; set; }

        public IEnumerable<SelectListItem> AV_Package { get; set; }

        public ContentInfoViewModel ContentInfoViewModel { get; set; }

        public List<PackageDetailViewModel> ListPackages { get; set; }

        public List<ShoppingCartViewModel> ShoppingCart { get; set; }
    }
}
