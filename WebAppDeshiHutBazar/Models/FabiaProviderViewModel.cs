using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Common;

namespace WebDeshiHutBazar
{
    public class FabiaProviderViewModel : BaseViewModel
    {
        public FabiaProviderViewModel()
        {
            ListFabiaServiceCategory = new List<PostViewModel>();
        }

        public long ProviderID { get; set; }

        public long FabiaServiceID { get; set; }

        public long UserID { get; set; }

        public byte[] ProfileImage { get; set; }

        [Required(ErrorMessage = "The field Provider Name is required!")]
        public string ProviderName { get; set; }

        [Required(ErrorMessage = "The field Primary Phone is required!")]
        public string ProviderPhone { get; set; }

        public string Website { get; set; }

        public string Remarks { get; set; }

        public string ServiceTitle { get; set; }
        
        public string ServiceDescription { get; set; }

        public double ServiceCharge { get; set; }

        [Display(Name = "State")]
        [Required]
        public long? StateID { get; set; }
       
        [Display(Name = "State")]
        public string DisplayState { get; set; }

        [Display(Name = "Service Category")]
        public string DisplayServiceCategory { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string RePassword { get; set; }
       
        public bool IsExistingUser { get; set; }

        public IEnumerable<SelectListItem> AV_State { get; set; }

        public IEnumerable<SelectListItem> AV_FabiaServiceCategory { get; set; }

        public List<PostViewModel> ListFabiaServiceCategory { get; set; }
    }
}
