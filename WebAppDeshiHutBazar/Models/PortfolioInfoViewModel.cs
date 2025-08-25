using Common;
using System.Collections.Generic;
namespace WebDeshiHutBazar
{
    public class PortfolioInfoViewModel : BaseViewModel
    {
        public PortfolioInfoViewModel()
        {
            PageingModelAll = new PagingModel();
            ListUserAll = new List<UserViewModel>();
        }
        public PortfolioInfoViewModel(EnumCurrency currency) : base(currency)
        {
            PageingModelAll = new PagingModel();
            ListUserAll = new List<UserViewModel>();
        }
      
        public List<UserViewModel> ListUserAll { get; set; }

        public PostViewModel PostViewModel { get; set; }
    }
}
