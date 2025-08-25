using Common;
using System.Collections.Generic;
namespace WebDeshiHutBazar
{
    public class ShoppingCartSummaryInformationViewModel : UserViewModel
    {
        public ShoppingCartSummaryInformationViewModel()
        {
            ShipAddress = new ShippingAddressViewModel();
            ProductCartInformationViewModel = new ProductCartInformationViewModel();
            PayOnDelivery = false;
        }
        public ShoppingCartSummaryInformationViewModel(EnumCurrency currency) : base(currency)
        {
            ShipAddress = new ShippingAddressViewModel();
            ProductCartInformationViewModel = new ProductCartInformationViewModel();
            PayOnDelivery = false;
        }

        public ProductCartInformationViewModel ProductCartInformationViewModel { get; set; }

        public decimal TransportPayableAmount { get; set; }

        public decimal ItemsPayableAmount { get; set; }

        public decimal TotalPayable { get; set; }

        public ShippingAddressViewModel ShipAddress { get; set; }

        public bool PayOnDelivery { get; set; }
        
        public int MinimumDeliveryDays { get; set; }
    }
}
