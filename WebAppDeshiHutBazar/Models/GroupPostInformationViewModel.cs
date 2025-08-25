using System.Web.Mvc;
using System.Collections.Generic;
using Common;

namespace WebDeshiHutBazar
{
    public class GroupPostInformationViewModel : BaseViewModel
    {
        public GroupPostInformationViewModel()
        {
            ListPost = new List<PostViewModel>();
        }

        public GroupPostInformationViewModel(EnumCurrency currency) : base(currency)
        {
            ListPost = new List<PostViewModel>();
        }

        public GroupPostInformationViewModel(int groupConfigID, EnumCurrency currency) : base(currency)
        {
            ListPost = new List<PostViewModel>();
            GroupConfigID = groupConfigID;
        }

        public List<PostViewModel> ListPost { get; set; }

        public long? CategoryID { get; set; } 

        public long? SubCategoryID { get; set; }

        public int GroupConfigID { get; set; }
    }
}
