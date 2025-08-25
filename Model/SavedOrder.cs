using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
using Model;
namespace Model.Order
{
    public class SavedOrder : BaseEntity
    {
        public SavedOrder() { }

        public SavedOrder(EnumCountry country) {
            ListOrderItems = new List<OrderDetail>();

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
        }

        [Key]
        public long SavedOrderID { get; set; }

        public List<OrderDetail> ListOrderItems { get; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public long UserID { get; set; }

        [ForeignKey("ShippingAddressID")]
        public virtual ShippingAddress ShippingAddress { get; set; }

        public long ShippingAddressID { get; set; }

        public long TotalPrice { get; set; }

        public EnumCurrency Currency { get; set; }
    }
}
