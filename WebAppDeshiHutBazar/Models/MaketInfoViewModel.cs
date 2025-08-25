using Common;
using System.Collections.Generic;
namespace WebDeshiHutBazar
{
    public class MarketInfoViewModel : BaseViewModel
    {
        public MarketInfoViewModel()
        {
            PageingModelAll = new PagingModel();
            ListPostsAll = new List<PostViewModel>();
            ContentInfoViewModel = new ContentInfoViewModel();
        }
        public MarketInfoViewModel(EnumCurrency currency) : base(currency)
        {
            PageingModelAll = new PagingModel();
            ListPostsAll = new List<PostViewModel>();
            ContentInfoViewModel = new ContentInfoViewModel();
        }
      
        public List<PostViewModel> ListPostsAll { get; set; }

        public List<PostViewModel> UrgentPanelPostList { get; set; }

        public long? SubCategoryID { get; set; }

        public long? CategoryID { get; set; }

        public PostViewModel PostViewModel { get; set; }

        public string DisplaySubCategory { get; set; }

        public ContentInfoViewModel ContentInfoViewModel { get; set; }        
    }
}
