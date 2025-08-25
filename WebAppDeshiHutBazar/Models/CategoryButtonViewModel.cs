using Common;

namespace WebDeshiHutBazar
{
    public class CategoryButtonViewModel : BaseViewModel
    {
        public CategoryButtonViewModel() 
        {
        }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

        public long CategoryID { get; set; }

        public long SubCategoryID { get; set; }

        public string CategoryDetailUrl { get; set; }

        public string IconClass { get; set; }

        public EnumCustomButtonItemType CustomButtonItemType { get; set; }
    }
}
