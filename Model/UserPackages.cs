using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
namespace Model
{
    public class UserPackage : BaseEntity
    {
        public UserPackage() { }

        public UserPackage(
            User userEntity,
            PackageConfig packageEntity,
            EnumCountry country
            )
        {
            UserID = userEntity.UserID;
            PackageType = packageEntity.PackageType;
            PackageStatus = packageEntity.PackageStatus;
            PackageName = packageEntity.PackageName;
            Descriptinon = packageEntity.Descriptinon;
            PackageID = packageEntity.PackageConfigID;
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
        }

        public void UpdateExpireDate(EnumPackageSubscriptionPeriod subscriptionPeriod, EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);

            IssueDate = BaTime;
            if (EnumPackageSubscriptionPeriod.Month == subscriptionPeriod)
            {
                ExpireDate = IssueDate.AddDays(30);
            }
            else if (EnumPackageSubscriptionPeriod.Year == subscriptionPeriod)
            {
                ExpireDate = IssueDate.AddDays(365);
            }
            else
            {
                ExpireDate = IssueDate.AddDays(30);
            }
        }

        [Key]
        public long UserPackageID { get; set; }

        public long? UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public int? PackageID { get; set; }

        [ForeignKey("PackageID")]
        public virtual PackageConfig Package { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public string PackageName { get; set; }

        public string Descriptinon { get; set; }

        public double PackagePrice { get; set; }

        public int Discount { get; set; }

        public int TotalFreePost { get; set; }

        public int TotalPremiumPost { get; set; }

        public EnumPackageType PackageType { get; set; }

        public EnumPackageStatus PackageStatus { get; set; }

        public EnumPackageSubscriptionPeriod SubscriptionPeriod { get; set; }

        public int UserPremiumPostCount { get; set; }

        public int UserFreePostCount { get; set; }
    }
}