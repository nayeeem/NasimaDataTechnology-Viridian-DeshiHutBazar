using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    public class BikashBillTransacton : BaseEntity
    {
        /// <summary>
        /// For EF
        /// </summary>
        public BikashBillTransacton() { }

        public BikashBillTransacton(
            UserCreditOrder userCreditOrder,
            long userId, 
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

            UserCreditOrderID = userCreditOrder.UserCreditOrderID;
            TransactionNumber = transactionNumber;
            AgentNumber = agentNumber;
            PaidAmount = paidAmount.HasValue ? paidAmount.Value : 0;            
            EntryDateTime = BaTime;
            AdminApprovalStatus = EnumTransactionStatus.AdminCheckPending;
            UserId = userId;
        }

        public BikashBillTransacton(
            UserOrder userOrder,
            User user, 
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

            UserId = user.UserID;
            UserOrderID = userOrder.UserOrderID;
            TransactionNumber = transactionNumber;
            AgentNumber = agentNumber;
            PaidAmount = paidAmount ?? 0;            
            EntryDateTime = BaTime;
            AdminApprovalStatus = EnumTransactionStatus.AdminCheckPending;            
        }

        [Key]
        public long BikashBillId { get; set; }

        [Display(Name = "Transaction Number")]
        public string TransactionNumber { get; set; }

        [Display(Name = "Agent Number")]
        public string AgentNumber { get; set; }

        [Display(Name = "Paid Amount")]
        public double PaidAmount { get; set; }

        [Display(Name = "Entry Date & Time")]
        public DateTime EntryDateTime { get; set; }

        [Display(Name = "Admin Approval Status")]
        public EnumTransactionStatus AdminApprovalStatus { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        public long? UserId { get; set; }

        public bool IsReadOnly
        {
            get
            {
                if (AdminApprovalStatus == EnumTransactionStatus.AdminApproved)
                    return true;
                return false;
            }
        }

        public string Status
        {
            get
            {
                if (AdminApprovalStatus == EnumTransactionStatus.AdminApproved)
                    return "Approved";
                return "Approval Pending";
            }
        }

        [ForeignKey("UserOrderID")]
        public virtual UserOrder UserOrder { get; set; }

        public long? UserOrderID { get; set; }

        [ForeignKey("UserCreditOrderID")]
        public virtual UserCreditOrder UserCreditOrder { get; set; }

        public long? UserCreditOrderID { get; set; }
    }
}
