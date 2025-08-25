using System;
using System.ComponentModel.DataAnnotations;
using Common;

namespace Model
{
    public class LogServerError : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public LogServerError(Exception ex, string action, string controller) {
            Message = ex != null && ex.Message != null ? ex.Message : "";
            Source = ex.Source != null ? ex.Source : "";
            InnerExceptionMessage = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : "";
            MethodName = ex.TargetSite != null && ex.TargetSite.Name != null ? ex.TargetSite.Name : "";
            
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(EnumCountry.Bangladesh));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            ErrorLogTime = BaTime;
            EnumCountry = EnumCountry.Bangladesh;
        }

        /// <summary>
        /// For EF
        /// </summary>
        public LogServerError()
        {            
        }

        
        /// <summary>
        /// PK
        /// </summary>
        [Key]
        public long ServerErrorLogID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ErrorLogTime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InnerExceptionMessage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Controller { get; set; }
    }
}
