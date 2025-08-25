using System;
using System.Collections.Generic;
using Common;
namespace WebDeshiHutBazar
{
    public class UserOrderViewModel : UserViewModel
    {
        public UserOrderViewModel()
        {
            ListOrderDetails = new List<UserOrderDetailViewModel>();
        }
        public UserOrderViewModel(EnumCurrency currency) : base(currency)
        {
            ListOrderDetails = new List<UserOrderDetailViewModel>();
        }

        public long UserOrderID { get; set; }
       
        public DateTime OrderDate { get; set; }

        public double? TotalBill { get; set; }

        public EnumPackageOrderStatus OrderStatus { get; set; }

        public List<UserOrderDetailViewModel> ListOrderDetails { get; set; }
    }
}
