using System;
using Common;
using System.ComponentModel.DataAnnotations;
namespace WebDeshiHutBazar
{
    public class UserOrderDetailViewModel : UserViewModel
    {
        public UserOrderDetailViewModel()
        {
        }
        public UserOrderDetailViewModel(EnumCurrency currency) : base(currency)
        {
        }

        public int OrderDetailID { get; set; }
        
        public long? UserOrderID { get; set; }

        public string PackageName { get; set; }

        public int TotalFreePost { get; set; }

        public double ItemBillAomunt { get; set; }

        public int? Discount { get; set; }

        public EnumPackageType? PackageType { get; set; }

        public EnumPackageStatus? PackageStatus { get; set; }

        public EnumPackageSubscriptionPeriod SubscriptionPeriod { get; set; }

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

        public int TotalPremiumPost { get; set; }

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

        public double PackagePrice { get; set; }

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
    }
}
