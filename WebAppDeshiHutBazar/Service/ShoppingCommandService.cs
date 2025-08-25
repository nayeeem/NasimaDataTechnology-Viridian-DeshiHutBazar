using Model;
using Data;
using Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;


namespace WebDeshiHutBazar
{
    public class ShoppingCommandService : IShoppingCommandService
    {        
        public readonly IShoppingCommand _IShoppingCommandService;

        public ShoppingCommandService(
            IShoppingCommand iShoppingCommand
            )
        {
            _IShoppingCommandService = iShoppingCommand;
        }  
        
        public async Task<bool> ExecuteShopCompletedCommand(List<CartSingleItemViewModel> ListCartItems, 
            ShippingAddressViewModel shippingAddressViewModel,
            string bKashTransactionNumber,
            bool billPaid,
            EnumCountry country)
        {            
            CartSingleItemModel objModel;
            List<CartSingleItemModel> listSingleItems = new List<CartSingleItemModel>();
            foreach(var item in ListCartItems)
            {
                objModel = new CartSingleItemModel();
                objModel.PostID = item.PostID;
                objModel.ProductName = item.ProductName;
                objModel.Quantity = item.Quantity;
                objModel.SlNo = item.SlNo;
                objModel.UnitDiscountedPrice = item.UnitDiscountedPrice;
                objModel.UnitPrice = item.UnitPrice;
                listSingleItems.Add(objModel);
            }
            ShippingAddress objShippingAddress = new ShippingAddress();
            objShippingAddress.AddressDetails = shippingAddressViewModel.AddressDetails;
            objShippingAddress.ApartmentNo = shippingAddressViewModel.ApartmentNo;
            objShippingAddress.Area = shippingAddressViewModel.Area;
            objShippingAddress.Block = shippingAddressViewModel.Block;
            objShippingAddress.City = shippingAddressViewModel.City;
            objShippingAddress.CustomerEmail = shippingAddressViewModel.CustomerEmail;
            objShippingAddress.CustomerName = shippingAddressViewModel.CustomerName;
            objShippingAddress.CustomerPhone = shippingAddressViewModel.CustomerPhone;
            objShippingAddress.EnumCountry = country;
            objShippingAddress.HouseNo = shippingAddressViewModel.HouseNo;
            objShippingAddress.IsActive = true;
            objShippingAddress.LandMark = shippingAddressViewModel.LandMark;
            objShippingAddress.RoadNo = shippingAddressViewModel.RoadNo;
            objShippingAddress.ZipCode = shippingAddressViewModel.ZipCode;
            var result = await _IShoppingCommandService.ExecuteShopCompletedCommand(
                listSingleItems,
                objShippingAddress,
                bKashTransactionNumber,
                billPaid,
                country);
            return result;
        }
    }
}
