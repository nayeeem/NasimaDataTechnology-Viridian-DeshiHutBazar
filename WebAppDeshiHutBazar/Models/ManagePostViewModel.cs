using System.Collections.Generic;
using Common;

namespace WebDeshiHutBazar
{
    public class ManagePostViewModel : BaseViewModel
    {
        public ManagePostViewModel()
        {
            ListPostViewModel = new List<PostViewModel>();
        }

        public ManagePostViewModel(EnumCurrency currency) : base(currency)
        {
            ListPostViewModel = new List<PostViewModel>();
        }

        public List<PostViewModel> ListPostViewModel { get; set; }
    }
}
