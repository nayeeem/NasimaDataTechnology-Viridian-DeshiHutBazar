using Model;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using System;
using Common;

namespace Data
{
    public class ShoppingCommand : WebBusinessEntityContext, IShoppingCommand
    {
        public async Task<bool> ExecuteShopCompletedCommand(List<CartSingleItemModel> ListCartItems, 
            ShippingAddress shippingAddressEntity,
            string bKashTransactionNumber,
            bool billPaid,
            EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);

            using (var dbContextTransaction = _Context.Database.BeginTransaction())
            {
                try
                {
                    _Context.ShippingAddresss.Add(shippingAddressEntity);
                    await _Context.SaveChangesAsync();

                    var totalPayment = GetTotalPayAmount(ListCartItems);
                    PurchaseOrder objOrder = new PurchaseOrder(country)
                    {
                        EnumCountry = EnumCountry.Bangladesh,
                        OrderConfirmed = true,
                        OrderDate = BaTime,
                        OrderDelivered = false,
                        OrderTotalPaymentAmount = totalPayment,
                        IsActive = true
                    };
                    foreach (var item in ListCartItems)
                    {
                        var post = await _Context.Posts.FirstOrDefaultAsync(a => a.PostID == item.PostID);
                        var user = await _Context.Users.FirstOrDefaultAsync(a => a.UserID == post.UserID);
                        var company = await _Context.Companies.FirstOrDefaultAsync(a => a.UserID == user.UserID);
                        //var companyShopAddress = company.ShopAddress;
                        PurchaseOrderItems objItemOrder = new PurchaseOrderItems(
                            company, user, objOrder, post,
                            item.Quantity, EnumCountry.Bangladesh);
                        objOrder.ListOrderedItems.Add(objItemOrder);                        
                    }
                    _Context.PurchaseOrders.Add(objOrder);
                    await _Context.SaveChangesAsync();

                    OrderBill objOrderBill = new OrderBill(country);
                    objOrderBill.EnumCountry = EnumCountry.Bangladesh;
                    objOrderBill.BillingDate = BaTime;
                    objOrderBill.BillPaid = billPaid;
                    objOrderBill.BillPaidDate = billPaid ? BaTime : (DateTime?)null;
                    objOrderBill.EnumCountry = EnumCountry.Bangladesh;
                    objOrderBill.IsActive = true;
                    objOrderBill.PaymentMethod = billPaid ? EnumBillPaymentMethod.BkashPayment : EnumBillPaymentMethod.CashAtHand;
                    objOrderBill.PurchaseOrder = objOrder;
                    objOrderBill.ShippingAddress = shippingAddressEntity;
                    objOrderBill.TotalPayableAmount = totalPayment;
                    objOrderBill.BkashTransactionNumber = bKashTransactionNumber;
                    foreach (var item in ListCartItems)
                    {
                        var post = await _Context.Posts.FirstOrDefaultAsync(a => a.PostID == item.PostID);
                        var user = await _Context.Users.FirstOrDefaultAsync(a => a.UserID == post.UserID);
                        var company = await _Context.Companies.FirstOrDefaultAsync(a => a.UserID == user.UserID);
                        //var companyShopAddress = company.ShopAddress;
                        OrderBillItem objItemOrder = new OrderBillItem(country)
                        {
                            BillPaid = billPaid,
                            BillPaymentDate = billPaid ? BaTime : (DateTime?)null,
                            CompanyID = company.CompanyID,
                            OrderBill = objOrderBill,
                            ProductID = post.PostID,
                            ProductName = post.Title,
                            TotalUnits = item.Quantity,
                            UnitDiscountedPrice = item.UnitDiscountedPrice,
                            UnitPrice = item.UnitPrice
                        };
                        objOrderBill.ListBillItems.Add(objItemOrder);
                    }
                    _Context.OrderBills.Add(objOrderBill);                    
                    await _Context.SaveChangesAsync();

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    dbContextTransaction.Rollback();
                    throw exception;
                }                
            }
        }

        private decimal GetTotalPayAmount(List<CartSingleItemModel> listCartItems)
        {
            decimal sumAmount = 0;
            foreach(var item in listCartItems)
            {
                sumAmount += item.TotalUnitsPrice;
            }
            return sumAmount;
        }
    }
}
