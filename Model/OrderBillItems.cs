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
    public class OrderBillItem : BaseEntity
    {
        public OrderBillItem(EnumCountry country) {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
        }

        [Key]
        public long OrderBillItemID { get; set; }
        
        public long CompanyID { get; set; }

        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }

        public long OrderBillID { get; set; }

        [ForeignKey("OrderBillID")]
        public virtual OrderBill OrderBill { get; set; }

        public long? ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Post Product { get; set; }

        [Required]
        public string ProductName { get; set; }
        
        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal UnitDiscountedPrice { get; set; }

        [Required]
        public int TotalUnits { get; set; }

        [Required]
        public decimal TotalUnitsPrice { get; set; }

        [Required]
        public decimal TotalGatewayShareAmount { get; set; }

        [Required]
        public decimal TotalDeshiShareAmount { get; set; }

        [Required]
        public decimal TotalCompanyShareAmount { get; set; }  
        
        [Required]
        public bool BillPaid { get; set; }

        public DateTime? BillPaymentDate { get; set; }
    }
}
