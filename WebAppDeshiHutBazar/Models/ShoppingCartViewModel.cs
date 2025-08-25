using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Common;
namespace WebDeshiHutBazar
{
    public class ShoppingCartViewModel : BaseViewModel
    {
        public ShoppingCartViewModel()
        {
        }
        public ShoppingCartViewModel(EnumCurrency currency) : base(currency)
        {
        }

        public int SlNo { get; set; }

        public int PackageConfigID { get; set; }

        public string PackageName { get; set; }

        public double PackagePrice { get; set; }

        public int Discount { get; set; }

        public double MonthlyFee { get; set; }

        public EnumPackageSubscriptionPeriod SubscriptionPeriod { get; set; }

        public double YearlyFee { get; set; }

        public double TotalFee { get; set; }

        public int NoTotalFree { get; set; }

        public int NoTotalPremiumFree { get; set; }

        public long? UserOrderID { get; set; }
    }
}
