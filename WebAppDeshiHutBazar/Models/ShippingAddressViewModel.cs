using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebDeshiHutBazar
{
    public class ShippingAddressViewModel : BaseViewModel
    {
        public ShippingAddressViewModel() {
        }

        public int ShippingAddressID { get; set; }
        
        [Required(ErrorMessage ="Your Name is required!")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Your Email is required!")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Your Phone Number is required!")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "State/Region is required!")]
        public int StateID { get; set; }

        public IEnumerable<SelectListItem> AV_State { get; set; }

        [Required(ErrorMessage = "City is required!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Area is required!")]
        public string Area { get; set; }

        [Required(ErrorMessage = "Zip Code is required!")]
        public string ZipCode { get; set; }

        public string HouseNo { get; set; }

        public string RoadNo { get; set; }

        public string Block { get; set; }

        public string ApartmentNo { get; set; }

        public string AddressDetails { get; set; }

        public string LandMark { get; set; }
    }
}
