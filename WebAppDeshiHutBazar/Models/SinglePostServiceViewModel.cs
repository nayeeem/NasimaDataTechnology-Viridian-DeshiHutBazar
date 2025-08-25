using Common;
using System.Collections.Generic;
namespace WebDeshiHutBazar
{    
    public class SinglePostServiceViewModel : BaseViewModel
    {
        public SinglePostServiceViewModel()
        {
            PostViewModel = new PostViewModel();
            ListProcess = new List<PostProcessViewModel>();
            ListService = new List<PostServiceViewModel>();
        }
        public SinglePostServiceViewModel(EnumCurrency currency) : base(currency)
        {
            PostViewModel = new PostViewModel(currency);
            ListProcess = new List<PostProcessViewModel>();
            ListService = new List<PostServiceViewModel>();
        }
       
        public PostViewModel PostViewModel { get; set; }

        public long PostID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PosterName { get; set; }

        public string WebsiteUrl { get; set; }

        public string SearchKey { get; set; }

        public List<PostProcessViewModel> ListProcess { get; set; }

        public List<PostServiceViewModel> ListService { get; set; }
    }
}
