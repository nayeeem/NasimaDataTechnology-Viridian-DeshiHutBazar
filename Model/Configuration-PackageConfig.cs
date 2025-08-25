using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common;
using Model;

namespace Model
{
    public class PackageConfig : BaseEntity
    {
        public PackageConfig() { }

        public PackageConfig(
            EnumPackageStatus packageStatus,
            EnumPackageType packageType,
            EnumPackageSubscriptionPeriod subscriptionPeriod,
            string packageName,
            string description,
            int packageTotalFreePost,
            int packageToalPremiumPost,
            double purchasePrice,
            int discount,
            EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            PackageType = packageType;
            PackageStatus = packageStatus;
            SubscriptionPeriod = subscriptionPeriod;
            PackageName = packageName;
            Descriptinon = description;
            TotalFreePost = packageTotalFreePost;
            TotalPremiumPost = packageToalPremiumPost;
            PackagePrice = purchasePrice;
            Discount = discount;
        }

        [Key]
        public int PackageConfigID { get; set; }

        public string PackageName { get; set; }

        public string Descriptinon { get; set; }

        public int PackageTotalAllowedPost { get; set; }

        public int TotalFreePost { get; set; }

        public int TotalPremiumPost { get; set; }

        public EnumPackageType PackageType { get; set; }

        public EnumPackageStatus PackageStatus { get; set; }
        
        public double PackagePrice { get; set; }

        public int Discount { get; set; }

        public EnumPackageSubscriptionPeriod SubscriptionPeriod { get; set; }

        public virtual List<PriceConfig> ListPostPriceConfigs { get; set; }

        public virtual List<User> ListUsers { get; set; }

        public virtual List<UserOrderDetail> ListOrderDetails { get; set; }
    }
}
