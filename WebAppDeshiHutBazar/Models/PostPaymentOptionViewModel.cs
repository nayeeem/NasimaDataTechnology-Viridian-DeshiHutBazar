using System.Collections.Generic;
using Common;
using System.ComponentModel.DataAnnotations;

namespace WebDeshiHutBazar
{
    public class PostPaymentOptionViewModel : BaseViewModel
    {
        public PostPaymentOptionViewModel()
        {
            ListActiveUserPackages = new List<PackageDetailViewModel>();
            ContentInfoViewModel = new ContentInfoViewModel();
        }
        public PostPaymentOptionViewModel(EnumCurrency currency) : base(currency)
        {
            ListActiveUserPackages = new List<PackageDetailViewModel>();
            ContentInfoViewModel = new ContentInfoViewModel();
        }

        [Display(Name ="Currency")]
        public string DisplayCurrency { get; set; }

        [Display(Name = "Post Category")]
        public long? SubCategoryID { get; set; }

        [Display(Name = "Post Category")]
        public string DisplaySubCategory { get; set; }

        [Display(Name = "Post Price")]
        public double? PostPrice { get; set; }

        [Display(Name = "Premium Post Price")]
        public double? PremiumPostPrice { get; set; }

        [Display(Name = "Free Quota Balance")]
        public int FreeQuotaBalance { get; set; }

        public List<PackageDetailViewModel> ListActiveUserPackages { get; set; }

        public bool HasFreeQuota { get; set; }

        public bool HasCreditBalance { get; set; }

        public bool HasPremiumCreditBalance { get; set; }

        public bool HasSubscription { get; set; }

        public bool HasPremiumSubscription { get; set; }

        public string FreePostURL { get; set; }

        public string PaidPostURL { get; set; }

        public string PremiumPaidPostURL { get; set; }        

        [Display(Name = "Discount")]
        public int OfferDiscount { get; set; }

        [Display(Name = "Offer Type")]
        public EnumOfferType OfferType { get; set; }

        [Display(Name = "Offer Type")]
        public string DisplayOfferType { get; set; }

        public long PostId { get; set; }

        public ContentInfoViewModel ContentInfoViewModel { get; set; }
    }
}
