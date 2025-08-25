using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    public class PurchaseOrderItems :  BaseEntity
    {
        public PurchaseOrderItems() { }

        public PurchaseOrderItems(
            Company company,
            User user,
            PurchaseOrder order,
            Post product,
            int totalUnit,
            EnumCountry country)
        {
            CompanyID = company.CompanyID;
            PurchaseOrder = order;
            ProductID = product.PostID;
            ProductName = product.Title;
            UnitPrice = product.UnitPrice;
            UnitDiscountedPrice = product.DiscountedUnitPrice;
            TotalUnits = totalUnit;
            TotalUnitsPrice = totalUnit * product.DiscountedUnitPrice;

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
        }

        [Key]
        public long PurchaseOrderDetailID { get; set; }
        
        public long CompanyID { get; set; }

        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }

        public long OrderID { get; set; }

        [ForeignKey("OrderID")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }

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
    }
}
