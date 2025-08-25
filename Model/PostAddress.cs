using System;
using System.ComponentModel.DataAnnotations;
using Common;
namespace Model
{
    public class PostAddress : BaseEntity
    {
        public PostAddress() {
        }

        public PostAddress(EnumCountry country) {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
        }

        [Key]
        public long AddressID { get; set; }       

        public long StateID { get; set; }

        public string AreaDescription { get; set; }       
    }
}
