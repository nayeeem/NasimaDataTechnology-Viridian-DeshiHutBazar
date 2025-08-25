namespace Common
{
    public class CategorySearchInfoModel
    {
        public CategorySearchInfoModel()
        {
            SearchModel = new SearchModel();
        }

        public SearchModel SearchModel { get; set; }

        public string PageLocation { get; set; }
    }
}
