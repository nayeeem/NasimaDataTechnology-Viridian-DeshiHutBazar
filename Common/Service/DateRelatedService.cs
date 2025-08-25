using System;
namespace Common
{
    public static class DateRelatedService
    {        
        public static string GetDateString(DateTime date)
        {
            var day = date.Day.ToString();
            var month = date.Month;
            var year = date.Year.ToString();
            var dateString = string.Format("{0}-{1}-{2}", day, GetMonthString(month), year);
            return dateString;
        }

        private static string GetMonthString(int month)
        {
            if (month < 1 && month > 12)
                return "";
            if (month == 1)
                return "Jan";
            if (month == 2)
                return "Feb";
            if (month == 3)
                return "Mar";
            if (month == 4)
                return "Apr";
            if (month == 5)
                return "May";
            if (month == 6)
                return "Jun";
            if (month == 7)
                return "July";
            if (month == 8)
                return "Aug";
            if (month == 9)
                return "Sep";
            if (month == 10)
                return "Oct";
            if (month == 11)
                return "Nov";
            if (month == 12)
                return "Dec";
            return "";
        }

        public static DateTime GetBangladeshCurrentDateTime()
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            return BaTime;
        }
    }
}
