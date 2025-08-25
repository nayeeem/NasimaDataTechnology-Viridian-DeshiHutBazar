using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WebDeshiHutBazar
{    
    public class UserViewModel : BaseViewModel
    {
        public UserViewModel()
        {
            ListVisitors = new List<LogPostVisitViewModel>();
            ListVisitorQueries = new List<LogPostVisitViewModel>();
            ListVisitorLikes = new List<LogPostVisitViewModel>();
        }

        public UserViewModel(EnumCurrency currency) : base(currency)
        {
            ListVisitors = new List<LogPostVisitViewModel>();
            ListVisitorQueries = new List<LogPostVisitViewModel>();
            ListVisitorLikes = new List<LogPostVisitViewModel>();
        }

        public long UserID { get; set; }

        public string FBUserID { get; set; }

        [Display(Name = "Client Name:")]
        public string ClientName { get; set; }

        [Display(Name = "User Name:")]
        public string UserName { get; set; }
        
        [Display(Name = "Password:")]
        public string Password { get; set; }
     
        [Display(Name = "Email:")]
        public string Email { get; set; }
               
        [Display(Name = "Phone Number:")]
        public string Phone { get; set; }

        public string Website { get; set; }

        public string AdminPersonalEmail { get; set; }

        public bool IsPrivateSeller { get; set; }
       
        public bool IsCompanySeller { get; set; }        
       
        public bool IsAdminUser { get; private set; }                           
        
        public double AccountBalance { get; set; }

        public string Remarks { get; set; }

        public string UserPortfolioListUrl { get; set; }

        public List<LogPostVisitViewModel> ListVisitors { get; set; }

        public List<LogPostVisitViewModel> ListVisitorQueries { get; set; }

        public List<LogPostVisitViewModel> ListVisitorLikes { get; set; }
    }
}
