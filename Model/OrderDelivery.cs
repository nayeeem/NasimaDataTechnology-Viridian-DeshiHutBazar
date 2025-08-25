using Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
namespace Model
{
    public class OrderDelivery : BaseEntity
    {
        public OrderDelivery() { }

        public OrderDelivery(
            CompanyAddress shopAddress,
            ShippingAddress shipAddress,
            Post product,
            PurchaseOrder purchaseOrder,
            OrderBill orderBill,
            OrderBillItem billItem,
            EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            Product = product;
            PurchaseOrder = purchaseOrder;
            OrderBill = orderBill;
            BillItem = billItem;
            ShopAddress = shopAddress;
            ShippingAddress = shipAddress;
            EnumCountry = country;
            ProductDelivered = false;
        }

        [Key]
        public long OrderDeliveryID { get; set; }

        public long? PurchaseOrderID { get; set; }

        [ForeignKey("PurchaseOrderID")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public long? OrderItemID { get; set; }

        [ForeignKey("OrderItemID")]
        public virtual PurchaseOrderItems OrderItem { get; set; }

        public long? OrderBillID { get; set; }

        [ForeignKey("OrderBillID")]
        public virtual OrderBill OrderBill { get; set; }

        public long? BillItemID { get; set; }

        [ForeignKey("BillItemID")]
        public virtual OrderBillItem BillItem { get; set; }

        public long ShippingAddressID { get; set; }

        [ForeignKey("ShippingAddressID")]
        public virtual ShippingAddress ShippingAddress { get; set; }

        public long? ShopAddressID { get; set; }

        [ForeignKey("ShopAddressID")]
        public virtual CompanyAddress ShopAddress { get; set; }

        public long ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Post Product { get; set; }

        public EnumBillPaymentMethod BillPayMethod { get; set; }

        public bool BillPaid { get; set; }

        public decimal ItemCollectionAmount { get; set; }

        public string DeliveryPerson { get; set; }

        public DateTime DeliveryDate { get; set; }

        public DateTime DeliveryLastDate { get; set; }

        public bool BillCollected { get; set; }

        public bool ProductDelivered { get; set; }
    }
}
