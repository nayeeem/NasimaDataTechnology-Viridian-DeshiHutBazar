using Model;
using Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Common;

namespace WebDeshiHutBazar
{
    public class UserService : IUserService
    {
        private readonly string ADSPACE_ADMIN_EMAIL = "adspace-admin@seleasy.com";
        private readonly string CUSTOMER_CARE_ADMIN_EMAIL = "customer-care-admin@seleasy.com";
        private readonly IUserRepository _UserRepository;
        

        public UserService(IUserRepository userRepository
            ) {
            _UserRepository = userRepository;
        }

        public async Task<bool> UpdateUser(User user)
        {
            if (user != null)
            {
                await _UserRepository.UpdateUser(user);               
            }
            return true;
        }

        public async Task<User> GetSingleUser(long userId)
        {
            return await _UserRepository.GetSingleUser(userId);
        }

        public async Task<bool> AddUser(User user)
        {
            if (user != null)
            {
                await _UserRepository.AddUser(user);
            }
            return true;
        }

        public async Task<long> GetAddUserID(User user)
        {
            long userID = 0;
            if (user != null)
            {
                userID = await _UserRepository.GetAddedUserID(user);
            }
            return userID;
        }

        public async Task<bool> CreditAccountBalance(int userId, double creditAmount, long currentUserID, EnumCountry country)
        {
            var result = await _UserRepository.CreditUserAccountBalance(userId, creditAmount, currentUserID, country);            
            return result;
        }

        public async Task<bool> DebitAccountBalance(long userId, double debitAmount, long currentUserID, EnumCountry country)
        {
            var result = await _UserRepository.DebitUserAccountBalance(userId, debitAmount, currentUserID, country);
            return result; 
        }

        public async Task<User> GetAdSpaseAdminUser()
        {
            return await _UserRepository.GetSingleUser(ADSPACE_ADMIN_EMAIL);
        }

        public async Task<User> GetCustomerCareAdminUser()
        {
            return await _UserRepository.GetSingleUser(CUSTOMER_CARE_ADMIN_EMAIL);
        }

        public async Task<User> GetSpecificUser(string email)
        {
            return await _UserRepository.GetSingleUser(email);
        }

        public async Task<User> GetSpecificUser(long userID)
        {
            return await _UserRepository.GetSingleUser(userID);
        }

        public async Task<List<User>> GetAllUser()
        {
            return await _UserRepository.GetAllUser();
        }
    }
}
