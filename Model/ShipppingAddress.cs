using System;
using System.ComponentModel.DataAnnotations;
using Common;
namespace Model
{
    public class ShippingAddress : BaseEntity
    {
        public ShippingAddress()
        {
        }

        public ShippingAddress(EnumCountry country, int stateID, string city, string zipCode)
        {
            EnumCountry = country;
            StateID = stateID;
            City = city;
            ZipCode = zipCode;

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
        }

        [Key]
        public long ShippingAddressID { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string CustomerEmail { get; set; }

        [Required]
        public string CustomerPhone { get; set; }

        [Required]
        public int StateID { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Area { get; set; }

        [Required]
        public string ZipCode { get; set; }

        public string HouseNo { get; set; }

        public string RoadNo { get; set; }

        public string Block { get; set; }
                                
        public string ApartmentNo { get; set; }

        public string AddressDetails { get; set; }

        public string LandMark { get; set; }
    }
}
