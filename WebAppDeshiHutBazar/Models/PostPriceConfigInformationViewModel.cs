using System.Web.Mvc;
using System.Collections.Generic;
using Common;
using System.ComponentModel.DataAnnotations;
namespace WebDeshiHutBazar
{
    public class PostPriceConfigInformationViewModel : BaseViewModel
    {
        public PostPriceConfigInformationViewModel()
        {
            ListPriceConfig = new List<PostPriceConfigurationViewModel>();
            ListConfig = new List<PostPackageConfigurationViewModel>();
            ListBikasdhBill = new List<BikashBillTransactonViewModel>();
        }

        public PostPriceConfigInformationViewModel(EnumCurrency currency)
        {
            ListPriceConfig = new List<PostPriceConfigurationViewModel>();
            ListConfig = new List<PostPackageConfigurationViewModel>();
            ListBikasdhBill = new List<BikashBillTransactonViewModel>();
        }

        public EnumCountry? CountryID { get; set; }

        public int? PackageID { get; set; }
   
        [Display(Name = "Country")]
        public string DisplayCountryName { get; set; }

        [Display(Name = "Currency")]
        public string DisplayCurrency { get; set; }

        [Display(Name = "Product Category")]
        public string DisplaySubCategory { get; set; }

        [Display(Name = "Price")]
        public int? Price { get; set; }

        public string FormattedPriceValue { get; set; }

        [Display(Name = "No of Free Post")]
        public int? NoFreePost { get; set; }

        [Display(Name = "Package Name")]
        public string DisplayPackageName { get; set; }

        [Display(Name = "Offer Name")]
        public string DisplayOfferName { get; set; }

        [Display(Name = "Discount")]
        public int OfferDiscount { get; set; }

        public List<PostPriceConfigurationViewModel> ListPriceConfig { get; set; }

        public List<PostPackageConfigurationViewModel> ListConfig { get; set; }

        public List<BikashBillTransactonViewModel> ListBikasdhBill { get; set; }

        public IEnumerable<SelectListItem> AV_Country { get; set; }

        public IEnumerable<SelectListItem> AV_Package { get; set; }
    }
}
