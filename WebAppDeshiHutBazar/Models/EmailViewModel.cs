using System;
using Common;
namespace WebDeshiHutBazar
{
    public class EmailViewModel :ContactViewModel
    {
        public EmailViewModel() { }

        public EmailViewModel(string senderEmail,
            string senderPassoword,
            string receiverEmail,
            string advName,
            string DEFAULT_COMPANY_ADDRESS,
                    string DEFAULT_COMPANY_HO_ADDRESS,
                    string DEFAULT_COMPANY_PHONE,
                    string DEFAULT_COMPANY_WEBSITE,
                    string DEFAULT_COMPANY_NAME)
        {
            SenderEmail = senderEmail;
            SenderCompanyEmail = senderEmail;
            SenderEmailPassword = senderPassoword;
            ReceiverEmail = receiverEmail;
            NameAdvertiser = advName;
            SetDefaultData(DEFAULT_COMPANY_NAME,
                DEFAULT_COMPANY_ADDRESS,
                DEFAULT_COMPANY_HO_ADDRESS,
                DEFAULT_COMPANY_PHONE,
                DEFAULT_COMPANY_WEBSITE);
        }

        public void SetDefaultData(string companyName, string companyAddress1, string companyHOAddress2,
            string companyPhone, string companyWebsite)
        {
            SenderCompanyAddress = companyAddress1;
            SenderHQCompanyAddress = companyHOAddress2;
            SenderCompanyWebsite = companyWebsite;
            SenderCompanyPhone = companyPhone;
            SenderCompanyName = companyName;
        }

        public string SenderEmail { get; set; }

        public string SenderEmailPassword { get; set; }

        public string ReceiverEmail { get; set; }

        public string AdminReceiverEmail { get; set; }

        public string PostTitle { get; set; }

        public string SubjectText { get; set; }

        public string SubjectShortText { get; set; }

        public string MessageBodyHTMLText { get; set; }

        public DateTime SendDate { get; set; }

        public string SenderCompanyWebsite { get; set; }

        public string SenderHQCompanyAddress { get; set; }

        public string SenderCompanyAddress { get; set; }

        public string SenderCompanyEmail { get; set; }

        public string SenderCompanyName { get; set; }

        public string SenderCompanyPhone { get; set; }  
        
        public string PhoneNofication { get; set; }

        public string EmailNotification { get; set; }

        public string NameNotification { get; set; }

        public string EmailAdvertiser { get; set; }

        public string PhoneAdvertiser { get; set; }

        public string NameAdvertiser { get; set; }

        public string NewPassword { get; set; }

        public string EmailVerifyUrl { get; set; }

        public EnumReasonForEmail EmailSendReason { get; set; }

        public string Comment { get; set; }
    }
}
