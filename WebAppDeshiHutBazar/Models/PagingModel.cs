namespace WebDeshiHutBazar
{
    public class PagingModel
    {
        public PagingModel() {
            TotalPageCount = 0;
            PrevPageNumber = 0;
            NextPageNumber = 0;
        }

        public int TotalPageCount { get; set; }

        public int PrevPageNumber { get; set; }

        public int CurrPageNumber { get; set; }

        public int NextPageNumber { get; set; }

        public string TemplateIdAttribute { get; set; }

        public string PrevUrl { get; set; }

        public string NextUrl { get; set; }

        public bool IsNextDisable { get; set; }

        public bool IsPrevDisable { get; set; }
    }
}
