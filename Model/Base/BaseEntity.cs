using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    public class BaseEntity
    {        
        public BaseEntity() {            
            IsActive = true;
        }   
                
        public void UpdateModifiedDate(long userId, EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            ModifiedDate = BaTime;
            ModifiedBy = userId;
        }

        public string GetCountryTimeZoneName(EnumCountry country)
        {
            if(country == EnumCountry.Bangladesh)
            {
                return "Bangladesh Standard Time";
            }
            else if(country == EnumCountry.Nigeria)
            {
                return "W. Central Africa Standard Time";
            }
            else
            {
                return "Bangladesh Standard Time";
            }
        }

        [Required]
        public long CreatedBy { get; set; }
        
        [Required]
        public DateTime CreatedDate { get; set; }
       
        [Required]
        public long ModifiedBy { get; set; }
        
        [Required]
        public DateTime ModifiedDate { get; set; }
        
        public bool IsActive { get; set; }
        
        [Display(Name ="Country")]
        public EnumCountry EnumCountry { get; set; }
        
        public string CountryName
        {
            get
            {
                return LocationRelatedSeed.GetCountryDescription(EnumCountry);
            }
        }
        
        public string CurrencyLongString
        {
            get
            {
                return LocationRelatedSeed.GetCountryCurrencyDescription(EnumCountry);
            }
        }
    }
}
