using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Model;

namespace WebDeshiHutBazar
{
    public class ProductShoppingCartController : BaseController
    {
        private readonly IBikashBillTransactionService _BikashBillTransactionService;
        private readonly IShoppingCommandService _IShoppingCommandService;
        private readonly IPostMangementService _PostMangementService;

        public ProductShoppingCartController() 
        { }

        public ProductShoppingCartController(IBikashBillTransactionService bikashBillTransactionService,
                                  IShoppingCommandService iShoppingCommandService,
                                  IPostMangementService postMangementService)
        {
            _BikashBillTransactionService = bikashBillTransactionService;
            _IShoppingCommandService = iShoppingCommandService;
            _PostMangementService = postMangementService;
        }

        public ActionResult GetShoppingCart()
        {
            ProductCartInformationViewModel objInformationModel = new ProductCartInformationViewModel();
            objInformationModel.ShoppingCart = GetProductShoppingCart();
            objInformationModel.TotalBill = GetTotalBill();
            return View(@"../../Areas/Company/Views/ProductShoppingCart/ShoppingCart", objInformationModel);
        }

        public ActionResult ReturnShippingAddress(ShippingAddressViewModel objShipAddressViewModel)
        {
            objShipAddressViewModel = (ShippingAddressViewModel)Session["ShippingAddress"];
            if (objShipAddressViewModel == null)
            {
                objShipAddressViewModel = new ShippingAddressViewModel();
            }
            objShipAddressViewModel.AV_State = DropDownDataList.GetAllStateList();
            return View(@"../../Areas/Company/Views/ProductShoppingCart/ShippingAddress", objShipAddressViewModel);
        }

        public ActionResult ShippingAddress(ShippingAddressViewModel objShipAddressViewModel)
        {           
            if (!ModelState.IsValid)
            {
                objShipAddressViewModel.AV_State = DropDownDataList.GetAllStateList();
                return View(@"../../Areas/Company/Views/ProductShoppingCart/ShippingAddress", objShipAddressViewModel);
            }
            if (objShipAddressViewModel != null && !string.IsNullOrEmpty(objShipAddressViewModel.CustomerEmail))
            {
                Session["ShippingAddress"] = objShipAddressViewModel;
                ShoppingCartSummaryInformationViewModel objModelBikashBillTranVM = 
                    new ShoppingCartSummaryInformationViewModel(CURRENCY_CODE);
                objModelBikashBillTranVM.ProductCartInformationViewModel = new ProductCartInformationViewModel()
                {
                    ShoppingCart = GetProductShoppingCart(),
                    TotalBill = GetTotalBill(),
                    TransportBill = 100
                };
                objModelBikashBillTranVM.ShipAddress = (ShippingAddressViewModel)Session["ShippingAddress"];                
                return View(@"../../Areas/Company/Views/ProductShoppingCart/Summary", objModelBikashBillTranVM);
            }

            if (objShipAddressViewModel == null)
            {
                objShipAddressViewModel = new ShippingAddressViewModel();
            }
            objShipAddressViewModel.AV_State = DropDownDataList.GetAllStateList();
            return View(@"../../Areas/Company/Views/ProductShoppingCart/ShippingAddress", objShipAddressViewModel);
        }

        public ActionResult ReturnSummary(ShoppingCartSummaryInformationViewModel objInformation)
        {
            ShoppingCartSummaryInformationViewModel objModelBikashBillTranVM = 
                new ShoppingCartSummaryInformationViewModel(CURRENCY_CODE);
            objModelBikashBillTranVM.ProductCartInformationViewModel = new ProductCartInformationViewModel()
            {
                ShoppingCart = GetProductShoppingCart(),
                TotalBill = GetTotalBill(),
                TransportBill = 100
            };
            if (Session["ShippingAddress"] != null)
            {
                objModelBikashBillTranVM.ShipAddress = (ShippingAddressViewModel)Session["ShippingAddress"];
            }
            return View(@"../../Areas/Company/Views/ProductShoppingCart/Summary", objModelBikashBillTranVM);
        }

        public async Task<ActionResult> Summary(ShoppingCartSummaryInformationViewModel objInformation)
        {           
            if (!objInformation.PayOnDelivery)
            {
                //Pay by Bkash online
                var objModelBikashBillTranVM = new BikashBillTransactonViewModel(CURRENCY_CODE);
                objModelBikashBillTranVM.ProductCartFinalTotalBillAmount = GetTotalBill();
                objModelBikashBillTranVM.AgentNumber = PRODUCT_CART_AGENT_PHONE_NUMBER;
                return View(@"../../Areas/Company/Views/ProductShoppingCart/Recharge", objModelBikashBillTranVM);
            }
            else
            {
                //Pay cash on delivery
                ShippingAddressViewModel shippingVM = (ShippingAddressViewModel)Session["ShippingAddress"];
                if (shippingVM != null)
                {
                    var result = await _IShoppingCommandService.ExecuteShopCompletedCommand(
                        GetProductShoppingCart(),
                        shippingVM,
                        "",
                        false,
                        COUNTRY_CODE);
                }
                var objModelBikashBillTranVM = new BikashBillTransactonViewModel(CURRENCY_CODE);
                return View(@"../../Areas/Company/Views/ProductShoppingCart/RechargeNotification", objModelBikashBillTranVM);
            }
        }

        public async Task<ActionResult> Recharge(BikashBillTransactonViewModel objModelBikashBillTranVM)
        {
            if (objModelBikashBillTranVM.TransactionNumber != null && 
                objModelBikashBillTranVM.PaidAmount != null && 
                objModelBikashBillTranVM.PaidAmount != 0)
            {
                ShippingAddressViewModel shippingVM = (ShippingAddressViewModel) Session["ShippingAddress"];
                if(shippingVM != null)
                {
                    var result = await _IShoppingCommandService.ExecuteShopCompletedCommand(
                        GetProductShoppingCart(),
                        shippingVM,
                        objModelBikashBillTranVM.TransactionNumber,
                        true,
                        COUNTRY_CODE);
                }
                var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
                DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
                BikashBillTransacton objBillEntityObject = new BikashBillTransacton()
                {
                    AgentNumber = objModelBikashBillTranVM.AgentNumber,
                    TransactionNumber = objModelBikashBillTranVM.TransactionNumber,
                    PaidAmount = objModelBikashBillTranVM.PaidAmount.Value,
                    EntryDateTime = BaTime
                };
                var status = await _BikashBillTransactionService.AddNewBill(objBillEntityObject);
                objModelBikashBillTranVM.PageName = "Notification Page";
                return View(@"../../Areas/Company/Views/ProductShoppingCart/RechargeNotification", objModelBikashBillTranVM);
            }
            objModelBikashBillTranVM.PageName = "Recharge Page";
            return View(@"../../Areas/Company/Views/ProductShoppingCart/Recharge", objModelBikashBillTranVM);
        }

        [HttpGet]
        public JsonResult PlusCartItem(long serialNo)
        {
            var cartItems = GetProductShoppingCart();
            if (cartItems == null || cartItems.Count == 0)
                return Json(false, JsonRequestBehavior.AllowGet);
            var singleItem = cartItems.FirstOrDefault(a => a.SlNo == serialNo);
            if (singleItem != null)
            {
                var qty = singleItem.Quantity;
                singleItem.Quantity = qty + 1;
            }
            SetProductShoppingCart(cartItems);
            return Json(GetTotalBill(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MinusCartItem(long serialNo)
        {
            var cartItems = GetProductShoppingCart();
            if (cartItems == null || cartItems.Count == 0)
                return Json(false, JsonRequestBehavior.AllowGet);
            var singleItem = cartItems.FirstOrDefault(a => a.SlNo == serialNo);
            if (singleItem != null)
            {
                var qty = singleItem.Quantity;
                if (qty > 1)
                {
                    singleItem.Quantity = qty - 1;
                }
                else if (qty <= 1)
                {
                    cartItems.Remove(singleItem);
                    for (var i = 0; i < cartItems.Count; i++)
                    {
                        cartItems[i].SlNo = i + 1;
                    }
                }
            }
            SetProductShoppingCart(cartItems);
            return Json(GetTotalBill(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RemoveCartItem(long serialNo)
        {
            var cartItems = GetProductShoppingCart();
            if (cartItems == null || cartItems.Count == 0)
                return Json(false, JsonRequestBehavior.AllowGet);
            var singleItem = cartItems.FirstOrDefault(a => a.SlNo == serialNo);
            if (singleItem != null)
                cartItems.Remove(singleItem);
            for (var i = 0; i < cartItems.Count; i++)
            {
                cartItems[i].SlNo = i + 1;
            }
            SetProductShoppingCart(cartItems);
            return Json(GetTotalBill(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCartTotalPrice()
        {
            var cartItems = GetProductShoppingCart();
            if (cartItems == null || cartItems.Count == 0)
                return Json(0, JsonRequestBehavior.AllowGet);
            decimal sum = 0;
            foreach (var item in cartItems)
            {
                sum += item.TotalUnitsPrice;
            }
            SetProductShoppingCart(cartItems);
            return Json(sum, JsonRequestBehavior.AllowGet);
        }

        private decimal GetTotalBill()
        {
            var cartItems = GetProductShoppingCart();
            decimal sum = 0;
            if (cartItems == null)
                return 0;
            foreach (var item in cartItems)
            {
                sum += item.TotalUnitsPrice;
            }
            return sum;
        }

        [HttpGet]
        public async Task<JsonResult> AddCartItem(int postID)
        {            
            List<CartSingleItemViewModel> cartItems = GetProductShoppingCart();
            if (cartItems == null)
            {
                cartItems = new List<CartSingleItemViewModel>();
            }
            else
            {
                CartSingleItemViewModel objCartModel = cartItems.FirstOrDefault(a => a.PostID == postID);
                if (objCartModel != null)
                {
                    var quantity = objCartModel.Quantity;
                    objCartModel.Quantity = quantity + 1;
                    SetProductShoppingCart(cartItems);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            var postObject = await _PostMangementService.GetDisplayPostByID(postID, CURRENCY_CODE);
            if (postObject != null)
            {
                CartSingleItemViewModel objCartModel = new CartSingleItemViewModel(CURRENCY_CODE);
                objCartModel.PostID = postObject.PostID;
                objCartModel.ProductName = postObject.Title;
                objCartModel.UnitPrice = postObject.Price;
                objCartModel.UnitDiscountedPrice = postObject.DiscountedPrice;
                objCartModel.Quantity = 1;
                var count = cartItems.Count;
                objCartModel.SlNo = count + 1;
                cartItems.Add(objCartModel);
            }
            SetProductShoppingCart(cartItems);
            return Json(true, JsonRequestBehavior.AllowGet);
        }        
    }
}