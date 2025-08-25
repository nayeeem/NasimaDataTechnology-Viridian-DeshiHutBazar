using System.Threading.Tasks;
using System.Collections.Generic;
using Common;

namespace WebDeshiHutBazar
{
    public interface IShoppingCommandService
    {
        Task<bool> ExecuteShopCompletedCommand(List<CartSingleItemViewModel> ListCartItems,
            ShippingAddressViewModel shippingAddressViewModel,
            string bKashTransactionNumber,
            bool billPaid, 
            EnumCountry country);
    }
}
