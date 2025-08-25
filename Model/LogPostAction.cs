using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    public class LogPostAction : BaseEntity
    {
        /// <summary>
        /// For EF
        /// </summary>
        public LogPostAction() {
            
        }       

        /// <summary>
        /// For EF: To Log: StudentMarket, AllItemMarket
        /// </summary>
        public LogPostAction(EnumLogType logType, long subCatId, long catID, EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            LogDateTime = BaTime;
            LogType = logType;
            CatMarketSubCategoryID = subCatId;
            CategoryID = catID;
            DisplayLogType = logType.ToString();
        }

        /// <summary>
        /// For EF: To Log: StudentMarket, AllItemMarket
        /// </summary>
        public LogPostAction(EnumLogType logType, EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
            LogDateTime = BaTime;
            LogType = logType;
            EnumCountry = country;
            DisplayLogType = logType.ToString();
        }

        /// <summary>
        /// For EF, To Log: PostDetailVisit
        /// </summary>
        public LogPostAction(Post post, EnumLogType logType, EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            LogDateTime = BaTime;
            LogType = logType;
            DisplayLogType = logType.ToString();

            if (post != null)
            {
                PostID = post.PostID;
                CategoryID = post.CategoryID;
                SubCategoryID = post.SubCategoryID;
                PostTitle = post.Title;                
            }
        }

        
        public LogPostAction(string searchKey,
            long? categoryID,
            long? subCategoryID,
            long? stateID,
            string areaDescription,
            long? priceLow,
            long? priceHigh,
            bool? isNew,
            bool? isUsed,
            bool? isUrgent,
            bool? isForSell,
            bool? isForRent,
            EnumLogType logType, 
            EnumCountry country)
        {

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            LogDateTime = BaTime;
            LogType = logType;
            DisplayLogType = logType.ToString();
            CategoryID = categoryID;
            SubCategoryID = subCategoryID;
            SearchKey = searchKey;
            StateID = stateID;
            AreaDescription = areaDescription;
            PriceLow = priceLow;
            PriceHigh = priceHigh;
            IsNew = isNew.Value;
            IsUrgent = isUrgent.Value;
            IsUsed = isUsed.Value;
            IsForRent = isForRent.Value;
            IsForSell = isForSell.Value;
        }

        /// <summary>
        /// For EF To Log: SearchMarket
        /// </summary>
        public LogPostAction(EnumLogType logType, string searchKey, EnumCountry country)
        {

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
            LogDateTime = BaTime;
            LogType = logType;
            SearchKey = searchKey;
            DisplayLogType = logType.ToString();
        }        

        /// <summary>
        /// PK
        /// </summary>
        [Key]
        public long LogID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SessionNumber { get; set; }

        #region Post Visit
        /// <summary>
        /// 
        /// </summary>
        public long PostID { get; set; }

        /// <summary>
        /// Post Title
        /// </summary>
        public string PostTitle { get; set; }

        
        /// <summary>
        /// Category ID/ValueID
        /// </summary>
        public long? CategoryID { get; set; }        

        /// <summary>
        /// SubCategoryID
        /// </summary>
        public long? SubCategoryID { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public EnumLogType LogType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayLogType { get; set; }

        /// <summary>
        /// Search keys if Search Market
        /// </summary>
        public string SearchKey { get; set; }

        /// <summary>
        /// Sub Category Id if Category Market
        /// </summary>
        public long CatMarketSubCategoryID { get; set; }

        /// <summary>
        /// Log Time
        /// </summary>
        public DateTime LogDateTime { get; set; }

        public long? PriceLow { get; set; }

        public long? PriceHigh { get; set; }

        public long? StateID { get; set; }

        public string AreaDescription { get; set; }

        public bool IsNew { get; set; }

        public bool IsUsed { get; set; }

        public bool IsUrgent { get; set; }

        public bool IsForSell { get; set; }

        public bool IsForRent { get; set; }

    }
}
