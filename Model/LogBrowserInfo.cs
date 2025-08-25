using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model;

namespace Model
{
    public class LogBrowserInfo : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public LogBrowserInfo() { }

        /// <summary>
        /// For EF
        /// </summary>
        public LogBrowserInfo(EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;           
        }

        
        /// <summary>
        /// PK
        /// </summary>
        [Key]
        public long BrowserLogID { get; set; }

        
        /// <summary>
        /// 
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Height { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Country { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Log Time
        /// </summary>
        public DateTime LogDateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Lon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Lat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CountryCode { get; set; }

    }
}
