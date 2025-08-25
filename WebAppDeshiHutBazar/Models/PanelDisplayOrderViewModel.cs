using Common;
using System.Collections.Generic;
namespace WebDeshiHutBazar
{
    public class PanelDisplayOrderViewModel :BaseViewModel
    {
        public PanelDisplayOrderViewModel()
        {
            BannerList = new List<int>();
            SubBannerList = new List<int>();
            GroupList = new List<int>();
            SidePanelList = new List<int>();
            AllConfigList = new List<int>();
        }

        public PanelDisplayOrderViewModel(EnumCurrency currency) : base(currency)
        {
            BannerList = new List<int>();
            SubBannerList = new List<int>();
            GroupList = new List<int>();
            SidePanelList = new List<int>();
            AllConfigList = new List<int>();
        }

        public List<int> AllConfigList { get; set; }

        public List<int> BannerList { get; set; }

        public List<int> SubBannerList { get; set; }

        public List<int> GroupList { get; set; }

        public List<int> SidePanelList { get; set; }
    }
}
