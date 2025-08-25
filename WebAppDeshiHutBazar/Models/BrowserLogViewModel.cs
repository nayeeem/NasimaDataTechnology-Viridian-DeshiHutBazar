using Common;
using System;
namespace WebDeshiHutBazar
{
    public class BrowserLogViewModel : BaseViewModel
    {
        public BrowserLogViewModel(EnumCurrency currency) : base(currency)
        {
        }
        public BrowserLogViewModel()
        {
        }

        public string Width { get; set; }

        public string Height { get; set; }
        
        public string Country { get; set; }
       
        public string Zip { get; set; }
        
        public string Region { get; set; }
        
        public string City { get; set; }
        
        public DateTime LogDateTime { get; set; }
        
        public string Lon { get; set; }
        
        public string Lat { get; set; }

        public string CountryCode { get; set; }
    }
}
