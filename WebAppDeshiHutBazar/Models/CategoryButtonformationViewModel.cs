using Common;
using System.Collections.Generic;

namespace WebDeshiHutBazar
{
    public class CategoryButtonformationViewModel : BaseViewModel
    {
        public CategoryButtonformationViewModel()
        {
            ListTopCategories = new List<CategoryButtonViewModel>();
        }
        public CategoryButtonformationViewModel(EnumCurrency currency) : base(currency)
        {
            ListTopCategories = new List<CategoryButtonViewModel>();
        }

        public List<CategoryButtonViewModel> ListTopCategories { get; set; }        
    }
}
