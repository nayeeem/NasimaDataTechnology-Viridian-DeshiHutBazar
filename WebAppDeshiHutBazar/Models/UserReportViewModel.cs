using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WebDeshiHutBazar
{    
    public class UserReportViewModel : BaseViewModel
    {
        public UserReportViewModel()
        {
        }

        public UserReportViewModel(EnumCurrency currency) : base(currency)
        {           
        }

        public long UserID { get; set; }

        public string FBUserID { get; set; }

        public string ClientName { get; set; }

        public string UserName { get; set; }
        
        public string Email { get; set; }
               
        public string Phone { get; set; }

        public string Website { get; set; }

        public string AdminPersonalEmail { get; set; }

        public bool IsPrivateSeller { get; set; }
       
        public bool IsCompanySeller { get; set; }        
       
        public bool IsAdminUser { get; set; }                           
        
        public long PostCount { get; set; }

        public string UserCreateDate { get; set; }

        public string UserType
        {
            get
            {
                if (IsPrivateSeller)
                {
                    return "Individual User";
                }
                else if (IsCompanySeller)
                {
                    return "Company User";
                }
                else if (IsAdminUser)
                {
                    return "Admin User";
                }
                else
                {
                    return "N/A";
                }
            }
        }
    }
}
