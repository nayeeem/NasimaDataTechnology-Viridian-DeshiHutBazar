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
    public class OrderDetail : BaseEntity
    {
        /// <summary>
        /// For EF
        /// </summary>
        public OrderDetail() { }

        public OrderDetail(Post post, int qty, int discount, EnumCountry country) {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            UnitPrice = post.UnitPrice;
            Quantity = 1;
            ProductName = post.Title;
            PercentDiscount = discount;
            TotalProductPrice = Convert.ToInt64(Math.Abs((Quantity * UnitPrice) - (Quantity * UnitPrice * discount)));
        }

        /// <summary>
        /// PK
        /// </summary>
        [Key]
        public long OrderDetailID { get; set; }

        /// <summary>
        /// User
        /// </summary>
        [ForeignKey("PostID")]
        public virtual Post Post { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public long PostID { get; set; }

        /// <summary>
        /// Unit Price
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Qty
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long TotalProductPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double PercentDiscount { get; set; }
    }
}
