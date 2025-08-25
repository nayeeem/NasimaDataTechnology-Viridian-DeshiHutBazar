using System;
using Common;
using System.ComponentModel.DataAnnotations;
namespace WebDeshiHutBazar
{
    public class PackageDetailViewModel : BaseViewModel
    {
        public PackageDetailViewModel()
        {
        }

        public long UserPackageID { get; set; }

        [Display(Name = "Package Price")]
        public double PackagePrice { get; set; } 

        [Display(Name = "Discount")]
        public int Discount { get; set; }
        
        [Display(Name = "Package Name")]
        public string PackageName { get; set; }

        [Display(Name = "Descriptinon")]
        public string Descriptinon { get; set; }
        
        [Display(Name = "Total Free Posts")]
        public int TotalFreePost { get; set; }

        [Display(Name = "Total Premium Posts")]
        public int TotalPremiumPost { get; set; }

        [Display(Name = "Used Free Posts")]
        public int UserTotalFreePost { get; set; }

        [Display(Name = "Used Premium Posts")]
        public int UserTotalPremiumPost { get; set; }
        
        public int PackageConfigID { get; set; }

        [Display(Name = "Subscription Start Date")]
        public string DisplayStartDate { get; set; }

        [Display(Name = "Subscription End Date")]
        public string DisplayExpiryDate { get; set; }

        public string SubscriptionPostURL { get; set; }

        public string PremiumSubscriptionPostURL { get; set; }

        [Display(Name = "Subscription Period")]
        public EnumPackageSubscriptionPeriod SubscriptionPeriod { get; set; }

        [Display(Name = "Package Price (Month)")]
        public double MonthlyPackagePrice
        {
            get
            {
                if (SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    return PackagePrice;
                }
                else if (SubscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
                {
                    return Math.Abs(PackagePrice / 12);
                }
                else
                {
                    return PackagePrice;
                }
            }
        }

        [Display(Name = "Package Price (Year)")]
        public double YearlyPackagePrice
        {
            get
            {
                if (SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    return PackagePrice * 12;
                }
                else if (SubscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
                {
                    return PackagePrice;
                }
                else
                {
                    return PackagePrice;
                }
            }
        }

        [Display(Name = "Total Premium Posts (Month)")]
        public int MonthlyTotalPremiumPost
        {
            get
            {
                if (SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    return TotalPremiumPost;
                }
                else if (SubscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
                {
                    return Math.Abs(TotalPremiumPost / 12);
                }
                else
                {
                    return TotalPremiumPost;
                }
            }
        }

        [Display(Name = "Total Premium Posts (Year)")]
        public int YearlyTotalPremiumPost
        {
            get
            {
                if (SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    return TotalPremiumPost * 12;
                }
                else if (SubscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
                {
                    return TotalPremiumPost;
                }
                else
                {
                    return TotalPremiumPost;
                }
            }
        }

        [Display(Name = "Total Free Posts (Month)")]
        public int MonthlyTotalFreePost
        {
            get
            {
                if (SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    return TotalFreePost;
                }
                else if (SubscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
                {
                    return Math.Abs(TotalFreePost / 12);
                }
                else
                {
                    return TotalFreePost;
                }
            }
        }

        [Display(Name = "Total Free Posts (Year)")]
        public int YearlyTotalFreePost
        {
            get
            {
                if (SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    return TotalFreePost * 12;
                }
                else if (SubscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
                {
                    return TotalFreePost;
                }
                else
                {
                    return TotalFreePost;
                }
            }
        }
    }
}
