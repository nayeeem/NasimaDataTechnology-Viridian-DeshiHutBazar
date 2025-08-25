using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PurchaseOrder : BaseEntity
    {
        public PurchaseOrder(EnumCountry country) {
            ListOrderedItems = new List<PurchaseOrderItems>();

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
            OrderDate = BaTime;
        }

        [Key]
        public long PurchaseOrderID { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal OrderTotalPaymentAmount { get; set; }

        public bool OrderConfirmed { get; set; }

        public bool OrderDelivered { get; set; }

        public virtual List<PurchaseOrderItems> ListOrderedItems { get; set; }
    }
}
