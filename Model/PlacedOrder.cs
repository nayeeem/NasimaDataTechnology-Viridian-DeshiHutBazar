using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model;

namespace Model.Order
{
    public class PlacedOrder : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public PlacedOrder() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        public PlacedOrder(EnumCountry country) {
            ListOrderItems = new List<OrderDetail>();
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
        }

        /// <summary>
        /// PK
        /// </summary>
        [Key]
        public long PlacedOrderID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<OrderDetail> ListOrderItems { get; }

        /// <summary>
        /// User
        /// </summary>
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// Address Object 
        /// </summary>
        [ForeignKey("ShippingAddressID")]
        public virtual ShippingAddress ShippingAddress { get; set; }

        /// <summary>
        /// FK
        /// </summary>
        public long ShippingAddressID { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public long TotalPrice { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public EnumCurrency Currency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOrderDelivered { get; set; }
    }
}
