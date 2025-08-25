using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
namespace Model
{
    public class UserOrderDetail : BaseEntity
    {
        public UserOrderDetail() { }

        public UserOrderDetail(PackageConfig packageEntity,
            UserOrder userOrderEntity,
            EnumPackageSubscriptionPeriod subsPeriod,
            EnumCountry country
            )
        {
            PackageConfigID = packageEntity.PackageConfigID;
            UserOrderID = userOrderEntity.UserOrderID;
            TotalAllowedPost = GetTotalFreePost(subsPeriod, packageEntity);
            TotalFreePost = GetTotalFreePost(subsPeriod, packageEntity);
            TotalPremiumPost = GetTotalPremiumPost(subsPeriod, packageEntity);
            PackageType = packageEntity.PackageType;
            PackageStatus = packageEntity.PackageStatus;
            SubscriptionPeriod = subsPeriod;
            PackagePrice = GetPackagePrice(subsPeriod, packageEntity);
            ItemBillAomunt = GetPackagePrice(subsPeriod, packageEntity);
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
        }

        private double GetPackagePrice(EnumPackageSubscriptionPeriod subsPeriod, PackageConfig packageEntity)
        {
            if (subsPeriod == EnumPackageSubscriptionPeriod.Month)
            {
                if (packageEntity.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    return packageEntity.PackagePrice;
                }
                else if (packageEntity.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
                {
                    return Math.Abs(packageEntity.PackagePrice / 12);
                }
            }
            else if (subsPeriod == EnumPackageSubscriptionPeriod.Year)
            {
                if (packageEntity.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    return packageEntity.PackagePrice * 12;
                }
                else if (packageEntity.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
                {
                    return packageEntity.PackagePrice;
                }
            }
            return packageEntity.PackagePrice;
        }

        private int GetTotalPremiumPost(EnumPackageSubscriptionPeriod subsPeriod, PackageConfig packageEntity)
        {
            if (subsPeriod == EnumPackageSubscriptionPeriod.Month)
            {
                if (packageEntity.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    return packageEntity.TotalPremiumPost;
                }
                else if (packageEntity.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
                {
                    return Math.Abs(packageEntity.TotalPremiumPost / 12);
                }
            }
            else if (subsPeriod == EnumPackageSubscriptionPeriod.Year)
            {
                if (packageEntity.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    return packageEntity.TotalPremiumPost * 12;
                }
                else if (packageEntity.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
                {
                    return packageEntity.TotalPremiumPost;
                }
            }
            return packageEntity.TotalPremiumPost;
        }

        private int GetTotalFreePost(EnumPackageSubscriptionPeriod subsPeriod, PackageConfig packageEntity)
        {
            if (subsPeriod == EnumPackageSubscriptionPeriod.Month)
            {
                if(packageEntity.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    return packageEntity.TotalFreePost;
                }
                else if(packageEntity.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
                {
                    return Math.Abs(packageEntity.TotalFreePost / 12);
                }
            }
            else if (subsPeriod == EnumPackageSubscriptionPeriod.Year) {
                if (packageEntity.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Month)
                {
                    return packageEntity.TotalFreePost * 12;
                }
                else if (packageEntity.SubscriptionPeriod == EnumPackageSubscriptionPeriod.Year)
                {
                    return packageEntity.TotalFreePost;
                }
            }
            return packageEntity.TotalFreePost;
        }

        [Key]
        public int OrderDetailID { get; set; }

        [ForeignKey("UserOrderID")]
        public virtual UserOrder UserOrder { get; set; }

        public long? UserOrderID { get; set; }

        [ForeignKey("PackageConfigID")]
        public virtual PackageConfig Package { get; set; }

        public int? PackageConfigID { get; set; }

        public int TotalAllowedPost { get; set; }

        public int TotalFreePost { get; set; }

        public int TotalPremiumPost { get; set; }

        public double PackagePrice { get; set; }

        public int PackageDiscount { get; set; }

        public EnumPackageType PackageType { get; set; }

        public EnumPackageStatus PackageStatus { get; set; }

        public EnumPackageSubscriptionPeriod SubscriptionPeriod { get; set; }        

        public double ItemBillAomunt { get; set; }
    }
}
