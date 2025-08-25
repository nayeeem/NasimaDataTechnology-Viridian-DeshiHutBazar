using Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Data
{
    public interface IShoppingCommand
    {
        Task<bool> ExecuteShopCompletedCommand(List<CartSingleItemModel> ListCartItems,
            ShippingAddress shippingAddressEntity,
            string bKashTransactionNumber,
            bool billPaid,
            EnumCountry country);
    }
}
