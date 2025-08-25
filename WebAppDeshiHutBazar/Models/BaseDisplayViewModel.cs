using Common;

namespace WebDeshiHutBazar
{
    public class BaseDisplayViewModel : BaseViewModel
    {

        public BaseDisplayViewModel()
        {
            MenuObjectModel = new MenuObjectModel();
        }

        public BaseDisplayViewModel(EnumCurrency currency) : base(currency)
        {
            MenuObjectModel = new MenuObjectModel();
        }     
        
        //public MenuObjectModel MenuObjectModel { get; set; }
    }
}