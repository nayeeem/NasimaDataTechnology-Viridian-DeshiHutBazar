using Common;
using System.Collections.Generic;
namespace WebDeshiHutBazar
{
    public class UserSessionViewModel : BaseViewModel
    {
        public UserSessionViewModel()
        {
            ListMousePosition = new List<string>();
        }
        public UserSessionViewModel(EnumCurrency currency) : base(currency)
        {
            ListMousePosition = new List<string>();
        }

        public List<string> ListMousePosition { get; set; }
        public string ActiveUrl { get; set; }
        public string ElementId { get; set; }
        public string ElementClass { get; set; }
        public string TargetUrl { get; set; }
        public string ElementTagName { get; set; }
        public string BrowserWidth { get; set; }
        public string BrowserHeight { get; set; }
    }
}