using Common;

namespace WebDeshiHutBazar
{
    public class CartSingleItemViewModel : BaseViewModel
    {
        public CartSingleItemViewModel()
        {
        }
        public CartSingleItemViewModel(EnumCurrency currency) : base(currency)
        {
        }

        public int SlNo { get; set; }

        public long PostID { get; set; }

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal UnitDiscountedPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalUnitsPrice
        {
            get
            {
                return UnitDiscountedPrice * Quantity;
            }
        }
    }
}
