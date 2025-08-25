using Model;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Data
{
    public class UserOrderRepository : WebBusinessEntityContext, IUserOrderRepository
    {
        public async Task<bool> AddUserOrder(UserOrder userOrder)
        {
            if (userOrder != null)
            {
                _Context.UserOrders.Add(userOrder);
                await _Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> UpdateUserOrder(UserOrder userOrder)
        {
            if (userOrder != null)
            {
                var userOrderEntity = await _Context.UserOrders.FirstOrDefaultAsync(a => a.UserOrderID == userOrder.UserOrderID);
                userOrderEntity = userOrder;
                _Context.Entry<UserOrder>(userOrderEntity).State = System.Data.Entity.EntityState.Modified;
                var listOrderDetls = userOrderEntity.ListOrderDetails;
                foreach(var item in listOrderDetls)
                {
                    _Context.Entry<UserOrderDetail>(item).State = System.Data.Entity.EntityState.Added;
                }
                await _Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<UserOrder> GetSingleUserOrder(long userOrderID)
        {
            var userOrderObjectEntity = await _Context.UserOrders.FirstOrDefaultAsync(a => a.UserOrderID == userOrderID && a.IsActive);
            return userOrderObjectEntity;
        }

        public async Task<bool> DeleteUserOrder(long userOrderID)
        {
           var userOrder = await _Context.UserOrders.FirstOrDefaultAsync(a=>a.UserOrderID == userOrderID);
            if (userOrder != null)
            {
                userOrder.IsActive = false;
                await _Context.SaveChangesAsync();
            }
            return true;
        }        
    }
}
