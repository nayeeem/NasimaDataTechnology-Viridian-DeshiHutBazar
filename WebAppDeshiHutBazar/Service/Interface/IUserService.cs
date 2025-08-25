using Model;
using Common;

using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebDeshiHutBazar
{
    public interface IUserService
    {
        Task<bool> UpdateUser(User user);

        Task<User> GetSingleUser(long userId);

        Task<bool> AddUser(User user);

        Task<long> GetAddUserID(User user);
        
        Task<bool> DebitAccountBalance(long userId, double debitAmount, long currentUserID, EnumCountry country);

        Task<bool> CreditAccountBalance(int userId, double creditAmount, long currentUserID, EnumCountry country);

        Task<User> GetAdSpaseAdminUser();

        Task<User> GetCustomerCareAdminUser();

        Task<User> GetSpecificUser(string email);

        Task<User> GetSpecificUser(long userId);

        Task<List<User>> GetAllUser();
    }
}
