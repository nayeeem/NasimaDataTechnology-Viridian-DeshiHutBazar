using System;
using System.ComponentModel.DataAnnotations;
using Common;
namespace Model
{
    public class UserAccountBillTransaction : BaseEntity
    {        
        public UserAccountBillTransaction() { }

        public UserAccountBillTransaction(long postId, 
            double amount, 
            DateTime timeTransaction, 
            EnumTransactionStatus tranStatus, 
            EnumCountry country) 
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            PaidAmount = amount;
            EntryDateTime = BaTime;
            TransactionApprovalStatus = tranStatus;
            PostId = (int) postId;
        }

        [Key]
        public long BillId { get; set; }
        
        public double PaidAmount { get; set; }

        public DateTime EntryDateTime { get; set; }

        public EnumTransactionStatus TransactionApprovalStatus { get; set; }

        public int PostId { get; set; }        
    }
}
