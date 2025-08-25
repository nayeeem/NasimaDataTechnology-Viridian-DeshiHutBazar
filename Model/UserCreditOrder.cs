using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
namespace Model
{
    public class UserCreditOrder : BaseEntity
    {
        public UserCreditOrder() { }

        public UserCreditOrder(
            User userEntity,
            double? totalBill,
            EnumCountry country
            )
        {
            UserID = userEntity.UserID;
            BillAmount = totalBill;
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
            OrderDate = BaTime;
            OrderStatus = EnumPackageOrderStatus.Saved;
        }

        [Key]
        public long UserCreditOrderID { get; set; }

        public long? UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }        
        
        public DateTime OrderDate { get; set; }

        public double? BillAmount { get; set; }

        public EnumPackageOrderStatus OrderStatus { get; set; }
    }
}
