using System.Web.Mvc;
using System.Collections.Generic;

namespace WebDeshiHutBazar
{
    public class FabiaInformationViewModel 
    {
        public FabiaInformationViewModel()
        {
            ListPostVM = new List<PostViewModel>();
            ListFabiaServices = new List<SelectListItem>();
            ListAlphabet = new List<SelectListItem>();
            ListFullAlphabet = new List<SelectListItem>();
            ListTopCategories = new List<CategoryButtonViewModel>();
            ListCustomButtonSubCategories = new List<CategoryButtonViewModel>();
            CustomButtonTitle = "SPECIAL";
        }        

        public List<PostViewModel> ListPostVM { get; set; }

        public List<SelectListItem> ListFabiaServices { get; set; }

        public List<SelectListItem> ListAlphabet { get; set; }

        public List<SelectListItem> ListFullAlphabet { get; set; }

        public List<CategoryButtonViewModel> ListTopCategories { get; set; }

        public List<CategoryButtonViewModel> ListCustomButtonSubCategories { get; set; }

        public string CustomButtonTitle { get; set; }
    }
}
