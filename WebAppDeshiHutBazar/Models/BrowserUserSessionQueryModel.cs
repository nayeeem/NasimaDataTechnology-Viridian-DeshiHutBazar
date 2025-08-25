using Common;
using System;
namespace WebDeshiHutBazar
{
    public class BrowserUserSessionQueryModel : BaseViewModel
    {
        public BrowserUserSessionQueryModel()
        {
        }

        public BrowserUserSessionQueryModel(EnumCurrency currency) : base(currency)
        {
        }

        public DateTime? BCreateDate { get; set; }

        public string BSRDate
        {
            get  {
                return BCreateDate.HasValue ? BCreateDate.Value.ToShortDateString() : "";
            }
        }

        public string BSRTime
        {
            get {
                return BCreateDate.HasValue ? BCreateDate.Value.ToShortTimeString() : "";
            }
        }

        public DateTime? UCreateDate { get; set; }

        public string USDate
        {
            get {
                return UCreateDate.HasValue ? UCreateDate.Value.ToShortDateString() : "";
            }
        }

        public string USTime
        {
            get  {
                return UCreateDate.HasValue ? UCreateDate.Value.ToShortTimeString() : "";
            }
        }

        public long? BBrowserLog { get; set; }

        public long? UBrowserLog { get; set; }

        public string CurrentPage { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public string Target { get; set; }

        public string TagID { get; set; }

        public string Class { get; set; }

        public string Tag { get; set; }

        public string BWidth { get; set; }

        public string BHeight { get; set; }

        public string UWidth { get; set; }

        public string UHeight { get; set; }

        public string Country { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }
    }
}
