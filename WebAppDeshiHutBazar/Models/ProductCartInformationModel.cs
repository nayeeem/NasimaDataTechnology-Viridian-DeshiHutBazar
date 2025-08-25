using System.Collections.Generic;
namespace WebDeshiHutBazar
{
    public class ProductCartInformationViewModel
    {
        public ProductCartInformationViewModel() {
            ShoppingCart = new List<CartSingleItemViewModel>();
        }        
        
        public List<CartSingleItemViewModel> ShoppingCart { get; set; }

        public decimal TotalBill { get; set; }

        public decimal TransportBill { get; set; }

        public decimal FinalBill
        {
            get
            {
                return TotalBill + TransportBill;
            }
        }
    }
}
