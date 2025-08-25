using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    public class Provider : BaseEntity
    {
        public Provider() { }

        public Provider(
            string providerName,
            string providerPhone,
            long fabiaServiceCategoryID,
            string serviceTitle,
            EnumCountry country
            )
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            ProviderName = string.IsNullOrEmpty(providerName) ? "Not Provided" : providerName;
            ProviderPhone = providerPhone;           
            FabiaServiceID = fabiaServiceCategoryID;
            ServiceTitle = serviceTitle;
        }

        [Key]
        public long ProviderID { get; set; }

        public long? FabiaServiceID { get; set; }

        [ForeignKey("FabiaServiceID")]
        public Post FabiaServiceCategory { get; set; }

        public long? UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
       
        public string ServiceTitle { get; set; }

        public byte[] ProfileImage { get; set; }

        public string ProviderName { get; set; }

        public string ProviderPhone { get; set; }
       
        public string ProviderWebsite { get; set; }

        [MaxLength(4000)]
        public string Remarks { get; set; }

        [MaxLength(4000)]
        public string ServiceDescription { get; set; }

        public double ServiceCharge { get; set; }

        public long StateID { get; set; }

        public void SetModifiedDate(EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            ModifiedDate = BaTime;
        }

        public void SetProcessImage(byte[] img)
        {
            ProfileImage = img;
        }
    }
}
