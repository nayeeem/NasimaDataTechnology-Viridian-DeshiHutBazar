using Model;
using Common;

using System.Threading.Tasks;

namespace WebDeshiHutBazar
{
    public interface IUserCreditOrderService
    {
        Task<bool> AddUserCreditOrder(UserCreditOrder userCreditOrder);

        Task<UserCreditOrder> GetSingleUserCreditOrder(long userCreditOrderID);       
    }
}
