using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Model;

namespace Data
{
    public interface IUserRepository
    {
        Task<bool> AddUser(User user);

        Task<bool> UpdateUser(User user);

        Task<bool> CreditUserAccountBalance(int userId, double creditAmount, long currentUserID, EnumCountry country);

        Task<bool> DebitUserAccountBalance(long userId, double debitAmount, long currentUserID, EnumCountry country);

        Task<User> GetSingleUser(string email);

        Task<User> GetVerifyUser(string code);

        Task<User> GetSingleUser(long userId);

        Task<bool> DoesUserExistsByCryteria(string email, string password);

        Task<bool> DoesUserExistsByCryteria(string email, string password, bool isAdmin);

        Task<bool> DoesUserExistsByCryteria(bool isAdmin, string pin, string passcode);

        Task<bool> DoesUserEmailExists(string email);

        Task<long> GetAddedUserID(User user);

        Task<List<User>> GetAllUser();

        Task<List<User>> GetAllUser(bool allTypesOfUser);

    }
}
