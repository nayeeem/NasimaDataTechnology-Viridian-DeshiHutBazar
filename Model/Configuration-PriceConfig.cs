using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model;

namespace Model
{
    public class PriceConfig : BaseEntity
    {
        public PriceConfig() { }

        public PriceConfig(PackageConfig package,
                                        EnumCountry? configCountry,
                                        EnumCurrency? countryCurrency,
                                        long? subCategoryID,
                                        double? offerPrice,
                                        int? offerFreePost,
                                        int? offerDiscount,
                                        EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            ConfigurationCountry = configCountry;
            CountryCurrency = countryCurrency;
            SubCategoryID = subCategoryID;
            OfferPrice = offerPrice;
            OfferFreePost = offerFreePost;
            PackageConfigID = package.PackageConfigID;
            OfferDiscount = offerDiscount;
        }

        [Key]
        public int PostPriceConfigID { get; set; }

        public string OfferName { get; set; }

        public EnumCountry? ConfigurationCountry { get; set; }

        public EnumCurrency? CountryCurrency { get; set; }

        public long? SubCategoryID { get; set; }

        public EnumOfferType OfferType { get; set; }

        public double? OfferPrice { get; set; }

        public int? OfferDiscount { get; set; }

        public int? OfferFreePost { get; set; }
       
        [ForeignKey("PackageConfigID")]
        public PackageConfig PostPackageConfig { get; set; }

        public int? PackageConfigID { get; set; }
    }
}
