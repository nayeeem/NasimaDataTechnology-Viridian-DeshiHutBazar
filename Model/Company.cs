using Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model
{        
    public class Company : BaseEntity
    {                
        public Company() {
        }

        [Key]
        public long CompanyID { get; set; }

        public bool Agreement { get; set; }

        public long? UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }        

        public string CompanyName { get; set; }

        public string CompanyWebsite { get; set; }

        public string CompanyPhone { get; set; }

        public string CompanyEmail { get; set; }

        public string ShopContactEmail { get; set; }

        public string ShopContactName { get; set; }

        public string ShopContactPhoneNumber { get; set; }

        public string Remarks { get; set; }

        public long? CompanyAddressID { get; set; }

        [ForeignKey("CompanyAddressID")]
        public virtual CompanyAddress CompanyAddress { get; set; }

        public long? ShopAddressID { get; set; }

        [ForeignKey("ShopAddressID")]
        public virtual CompanyAddress ShopAddress { get; set; }

        public virtual List<OrderBillItem> ListOrderBills { get; set; }

        public string BankName { get; set; }

        public string BranchName { get; set; }

        public string AccountHolderName { get; set; }

        public string AccountNumber { get; set; }

        public string ACHolderContactNumber { get; set; }

        public string BkashAccountNumber { get; set; }

        public string OwnerName { get; set; }

        public string OwnerPhoneNumber { get; set; }

        public string OwnerEmail { get; set; }

        public byte[] OwnerNIDFile { get; set; }

        public byte[] TradeLicenseFile { get; set; }
    }
}
