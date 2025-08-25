using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
namespace Model
{
    public class UserOrder : BaseEntity
    {
        public UserOrder() {
        }

        public UserOrder(
            User userEntity,
            double? totalBill,
            EnumCountry country
            )
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            UserID = userEntity.UserID;
            TotalBill = totalBill;
            OrderDate = BaTime;
            ListOrderDetails = new List<UserOrderDetail>();
        }

        [Key]
        public long UserOrderID { get; set; }

        public long? UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }        

        public DateTime OrderDate { get; set; }

        public double? TotalBill { get; set; }

        public EnumPackageOrderStatus OrderStatus { get; set; }

        public virtual List<UserOrderDetail> ListOrderDetails { get; set; }

        public void AddOrderDetailList(List<UserOrderDetail> objListOrderDetails)
        {
            if (ListOrderDetails == null)
                ListOrderDetails = new List<UserOrderDetail>();
            ListOrderDetails = objListOrderDetails;
        }
    }
}
