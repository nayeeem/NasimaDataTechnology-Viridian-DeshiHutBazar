using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    public class LogUserSession : BaseEntity
    {
        public LogUserSession(EnumCountry country) {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
        }

        [Key]
        public int UserSessionId { get; set; }

        public string ActiveUrl { get; set; }

        public string ElementId { get; set; }

        public string ElementClass { get; set; }

        public string TargetUrl { get; set; }

        public string ElementTagName { get; set; }

        public virtual List<LogMousePosition> ListMousePosition { get; set; }

        public void AddPositions(List<string> listMousePosition)
        {
            LogMousePosition objPosition;
            foreach (var item in listMousePosition)
            {
                objPosition = new LogMousePosition();
                objPosition.Position = item;
                ListMousePosition.Add(objPosition);
            }            
        }

        public string BrowserWidth { get; set; }

        public string BrowserHeight { get; set; }

        public long? BrowserLogId { get; set; }
    }
}
