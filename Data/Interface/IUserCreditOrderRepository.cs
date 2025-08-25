using System.Threading.Tasks;
using Model;

namespace Data
{
    public interface IUserCreditOrderRepository
    {
        Task<bool> AddUserCreditOrder(UserCreditOrder userCreditOrder);

        //Task<bool> UpdateUserOrder(UserOrder userOrder);

        Task<UserCreditOrder> GetSingleUserCreditOrder(long userCreditOrderID);

        //Task<User> GetSingleUser(string email);

        //Task<User> GetSingleUser(long userId);

        //Task<bool> DoesUserExistsByCryteria(string email, string password);

        //Task<bool> DoesUserExistsByCryteria(string email, string password, bool isAdmin);

        //Task<bool> DoesUserExistsByCryteria(bool isAdmin, string pin, string passcode);

        //Task<bool> DoesUserEmailExists(string email);

        //Task<int> GetUserPackageId(int userId);

        //Task<bool> CreditAccountBalance(int userId, double creditAmount);

        //Task<bool> DebitAccountBalance(int userId, double debitAmount);

        //Task<long> GetAddUserID(User user);

        //Task<User> GetSingleFBUser(string fbUserId);
    }
}
