using System.ComponentModel.DataAnnotations;
namespace WebDeshiHutBazar
{
    public class SignUpViewModel
    {
        public SignUpViewModel() {
        }

        [Display(Name = "Client Name:")]
        [Required(ErrorMessage = "Client Name is required!")]
        public string ClientName { get; set; }

        [Required(ErrorMessage ="Re-Password is required!")]
        [Display(Name = "Re-Password")]
        public string RePassword { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }

        [Display(Name = "Phone Number:")]
        [Required(ErrorMessage = "Phone is required!")]
        public string Phone { get; set; }

        public bool IsPrivateSeller { get; set; }

        public bool IsCompanySeller { get; set; }

        public string PageName { get; set; }
    }
}
