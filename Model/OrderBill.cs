using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model;

namespace Model
{
    public class OrderBill : BaseEntity
    {
        public OrderBill(EnumCountry country) {
            ListBillItems = new List<OrderBillItem>();
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
            BillingDate = BaTime;
        }

        [Key]
        public long BillID { get; set; }

        public long PurchaseOrderID { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public DateTime BillingDate { get; set; }
        
        public EnumBillPaymentMethod PaymentMethod { get; set; }

        public bool BillPaid { get; set; }

        public DateTime? BillPaidDate { get; set; }

        public decimal TotalPayableAmount { get; set; }

        public decimal TransportPayableAmount { get; set; }

        public virtual List<OrderBillItem> ListBillItems { get; set; }
        
        [ForeignKey("ShippingAddressID")]
        public virtual ShippingAddress ShippingAddress { get; set; }
        
        public long ShippingAddressID { get; set; }

        public string BkashTransactionNumber { get; set; }
    }
}
