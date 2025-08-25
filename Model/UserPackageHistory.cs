using System;
using System.ComponentModel.DataAnnotations;
using Common;
namespace Model
{
    public class UserPackageHistory : BaseEntity
    {
        public UserPackageHistory() { }

        public UserPackageHistory(int packageId, int userId, EnumCountry country) {
            PackageId = packageId;
            UserId = userId;
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
        }

        [Key]
        public long UserPacHistoryId { get; set; }

        public int PackageId { get; set; }

        public int UserId { get; set; }
    }
}
