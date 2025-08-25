using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CartSingleItemModel
    {
        public CartSingleItemModel()
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
