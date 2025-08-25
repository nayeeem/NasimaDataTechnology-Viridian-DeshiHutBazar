using Model;
using Data;
using System.Threading.Tasks;

namespace WebDeshiHutBazar
{
    public class UserOrderService : IUserOrderService
    {
        public readonly IUserOrderRepository _UserOrderRepository;

        public UserOrderService(IUserOrderRepository userOrderRepository)
        {
            _UserOrderRepository = userOrderRepository;
        }

        public async Task<bool> AddUserOrder(UserOrder userOrder)
        {
            if (userOrder != null)
            {
                await _UserOrderRepository.AddUserOrder(userOrder);
            }
            return true;
        }
        
        public async Task<bool> UpdateUserOrder(UserOrder userOrder)
        {
            if (userOrder != null)
            {
                await _UserOrderRepository.UpdateUserOrder(userOrder);
            }
            return true;
        }

        public async Task<UserOrder> GetSingleUserOrder(long userOrderID)
        {
            return await _UserOrderRepository.GetSingleUserOrder(userOrderID);
        }

        public async Task<bool> DeleteUserOrder(long userOrderID)
        {
            var result = await _UserOrderRepository.DeleteUserOrder(userOrderID);
            return result;
        }        
    }
}
