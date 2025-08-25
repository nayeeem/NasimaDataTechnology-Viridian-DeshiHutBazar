using System;
using System.ComponentModel.DataAnnotations;
using Common;
namespace Model
{
    public class DonateBikashBillTransacton : BaseEntity
    {
        public DonateBikashBillTransacton() { }

        public DonateBikashBillTransacton(
            string transactionNumber, 
            string agentNumber,
            double? paidAmount,
            EnumCountry country
            )
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            TransactionNumber = transactionNumber;
            AgentNumber = agentNumber;
            PaidAmount = paidAmount.HasValue ? paidAmount.Value : 0;            
            EntryDateTime = BaTime;
        }

        [Key]
        public long DonateBikashBillId { get; set; }

        [Display(Name = "Transaction Number")]
        public string TransactionNumber { get; set; }

        [Display(Name = "Agent Number")]
        public string AgentNumber { get; set; }

        [Display(Name = "Paid Amount")]
        public double PaidAmount { get; set; }

        [Display(Name = "Entry Date & Time")]
        public DateTime EntryDateTime { get; set; }        
    }
}
