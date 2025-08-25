using System.Web.Mvc;
using System.Collections.Generic;
using Common;
using System.ComponentModel.DataAnnotations;

namespace WebDeshiHutBazar
{
    public class PostPackageConfigurationViewModel : BaseViewModel
    {
        public PostPackageConfigurationViewModel()
        {
        }

        public int PackageConfigID { get; set; }

        [Display(Name ="Package Name")]
        [Required(ErrorMessage = "The field Package Name is required.", AllowEmptyStrings =false)]
        public string PackageName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Total Allowed Posts")]
        [Required(ErrorMessage = "The field Total AllowedPost is required.")]
        public int TotalAllowedPost { get; set; }

        [Display(Name = "Total Free Posts")]
        [Required(ErrorMessage = "The field Total Free Post is required.")]
        public int TotalFreePost { get; set; }

        [Display(Name = "Monthly Premium Posts")]
        [Required(ErrorMessage = "The field Total Premium Post is required.")]
        public int TotalPremiumPost { get; set; }

        [Display(Name = "Package Type")]
        [Required(ErrorMessage = "The field Package Type is required.")]
        public EnumPackageType PackageType { get; set; }

        [Display(Name = "Package Status")]
        [Required(ErrorMessage = "The field Package Status is required.")]
        public EnumPackageStatus PackageStatus { get; set; }

        [Display(Name = "Subscription Period")]
        [Required(ErrorMessage = "The field Subscription Period is required.")]
        public EnumPackageSubscriptionPeriod SubscriptionPeriod { get; set; }

        [Display(Name = "Package Type")]
        public string DisplayPackageType { get; set; }

        [Display(Name = "Package Status")]
        public string DisplayPackageStatus { get; set; }

        [Display(Name = "Subscription Period")]
        public string DisplaySubscriptionPeriod { get; set; }

        [Display(Name = "Package Price")]
        [Required(ErrorMessage = "The field Package Price is required.")]
        public double PackagePrice { get; set; }

        [Display(Name = "Discount")]
        public int Discount { get; set; }

        public IEnumerable<SelectListItem> AV_PackageType { get; set; }

        public IEnumerable<SelectListItem> AV_PackageStatus { get; set; }

        public IEnumerable<SelectListItem> AV_SubscriptionPeriod { get; set; }            
    }
}
