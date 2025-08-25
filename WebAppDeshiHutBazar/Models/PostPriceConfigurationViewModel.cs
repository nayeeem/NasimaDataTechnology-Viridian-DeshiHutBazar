using System.Web.Mvc;
using System.Collections.Generic;
using Common;
using System.ComponentModel.DataAnnotations;
namespace WebDeshiHutBazar
{
    public class PostPriceConfigurationViewModel : BaseViewModel
    {
        public PostPriceConfigurationViewModel()
        {
        }

        public int PostPriceConfigID { get; set; }

        public long UserPackPriceID { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "The field Country is required.")]
        public EnumCountry? CountryId { get; set; }

        [Display(Name = "Country")]
        public string DisplayCountryName { get; set; }
        
        [Display(Name ="Currency")]
        public string DisplayCurrency { get; set; }

        [Display(Name = "Product Category")]
        [Required(ErrorMessage = "The field Sub Category is required.")]
        public long? SubCategoryID { get; set; }

        [Display(Name = "Product Category")]
        public string DisplaySubCategory { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field Price is required.")]
        public double? Price { get; set; }

        [Display(Name = "Price")]
        public string FormattedPriceValue { get; set; }

        [Display(Name = "No of Free Post")]
        [Required(ErrorMessage = "The field No of Free Post is required.")]
        public int? NoFreePost { get; set; }

        [Display(Name = "Package Name")]
        [Required(ErrorMessage ="The field Package Name is required.")]
        public int? PackageID { get; set; }

        [Display(Name = "Package Name")]
        public string DisplayPackageName { get; set; }

        [Display(Name = "Offer Discount")]
        public int? OfferDiscount { get; set; }

        [Display(Name = "Offer Type")]
        [Required(ErrorMessage = "The field Offer Type is required.")]
        public EnumOfferType OfferType { get; set; }

        [Display(Name = "Offer Type")]
        public string DisplayOfferType { get; set; }

        [Display(Name = "Offer Name")]
        [Required(ErrorMessage = "The field Offer Name is required.")]
        public string OfferName { get; set; }

        public IEnumerable<SelectListItem> AV_Country { get; set; }

        public IEnumerable<SelectListItem> AV_Currency { get; set; }

        public IEnumerable<SelectListItem> AV_SubCategory { get; set; }

        public IEnumerable<SelectListItem> AV_Package { get; set; }

        public IEnumerable<SelectListItem> AV_OfferType { get; set; }       
    }
}
