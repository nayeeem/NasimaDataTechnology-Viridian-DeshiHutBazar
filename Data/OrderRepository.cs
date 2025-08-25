using System.Linq;
using System.Collections.Generic;
using WebBusinessPlatform.Model;
using Model.Order;

namespace WebBusinessPlatform.Data
{
    public class OrderRepository : WebBusinessEntityContext, IOrderRepositry
    {
        //private WebBusinessPlatformEntityContext _context = new WebBusinessPlatformEntityContext();

        
        public void SavePlacedOrder(PlacedOrder orderObject)
        {
            _context.PlacedOrders.Add(orderObject);
            _context.SaveChanges();
        }

        public void DeletePlacedOrder(long orderId)
        {
            var order = _context.PlacedOrders.ToList().SingleOrDefault(a => a.PlacedOrderID == orderId);
            if (order != null)
                order.IsActive = false;

            _context.SaveChanges();
        }

        public void SaveSavedOrder(SavedOrder orderObject)
        {
            _context.SavedOrders.Add(orderObject);
            _context.SaveChanges();
        }

        public void DeleteSavedOrder(long orderId)
        {
            var order = _context.SavedOrders.ToList().SingleOrDefault(a => a.SavedOrderID == orderId);
            if (order != null)
                order.IsActive = false;

            _context.SaveChanges();
        }

        public List<PlacedOrder> GetAllPlacedOrdersByUserID(long userId)
        {
            return _context.PlacedOrders.Where(a => a.UserID == userId && a.IsActive && !a.User.IsUserBlocked).ToList();
        }

        public List<SavedOrder> GetAllSavedOrdersByUserID(long userId)
        {
            return _context.SavedOrders.Where(a => a.UserID == userId && a.IsActive && !a.User.IsUserBlocked).ToList();
        }

        public List<PlacedOrder> GetAllPlacedOrders()
        {
            return _context.PlacedOrders.Where(a=>a.IsActive && !a.User.IsUserBlocked).ToList();
        }

        public PlacedOrder GetPlacedOrderByOrderID(long? orderId)
        {
            if (!orderId.HasValue)
                return null;
            return _context.PlacedOrders.SingleOrDefault(a => a.PlacedOrderID == orderId.Value && !a.User.IsUserBlocked);
        }

        public SavedOrder GetSavedOrderByOrderID(long? orderId)
        {
            if (!orderId.HasValue)
                return null;
            return _context.SavedOrders.SingleOrDefault(a => a.SavedOrderID == orderId.Value && !a.User.IsUserBlocked);
        }
      

    }
}
