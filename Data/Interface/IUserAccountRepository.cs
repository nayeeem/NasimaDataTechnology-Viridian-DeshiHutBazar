using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Data
{
    public interface IUserAccountRepository
    {
        Task<bool> IsThisIsAValidAuthenticationForAdminLoginGateway(string pin, string passcode);
        
        Task<bool> IsThisIsAValidAuthenticationForAdminUser(string email, string password);

        Task<bool> IsThisIsAValidAuthenticationForUser(string email, string password);

        Task<User> GetSingleAuthorizedUser(string email);

        Task<User> GetSingleAuthorizedUser(long userid);

        Task<bool> IsAccountRegistered(string email);
    }
}
