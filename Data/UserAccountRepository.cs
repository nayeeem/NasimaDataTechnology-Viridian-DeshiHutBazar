using System.Linq;
using System.Collections.Generic;
using Model;
using System.Threading.Tasks;

namespace Data
{
    public class UserAccountRepository : UserRepository, IUserAccountRepository
    {
        public async Task<bool> IsThisIsAValidAuthenticationForAdminLoginGateway(string pin, string passcode)
        {
            if (string.IsNullOrEmpty(pin) || string.IsNullOrEmpty(passcode))
                return false;
            return await this.DoesUserExistsByCryteria(true,pin, passcode);
        }

        public async Task<bool> IsThisIsAValidAuthenticationForAdminUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return false;
            return await this.DoesUserExistsByCryteria(email, password,true);
        }

        public async Task<bool> IsThisIsAValidAuthenticationForUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return false;
            return await this.DoesUserExistsByCryteria(email, password);
        }

        public async Task<User> GetSingleAuthorizedUser(string email)
        {
            return await GetSingleUser(email);
        }

        public async Task<User> GetSingleAuthorizedUser(long userid)
        {
            return await GetSingleUser(userid);
        }

        public async Task<bool> IsAccountRegistered(string email)
        {            
            return await DoesUserEmailExists(email);
        }

    }
}
