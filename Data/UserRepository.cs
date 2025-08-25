using Model;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace Data
{
    public class UserRepository : WebBusinessEntityContext, IUserRepository
    {
        public async Task<long> GetAddedUserID(User user)
        {
            if (user != null)
            {
                _Context.Users.Add(user);
                await _Context.SaveChangesAsync();
            }
            return user.UserID;
        }

        public async Task<User> GetSingleUser(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;
            return await _Context.Users.FirstOrDefaultAsync(a => a.Email == email &&
                                                        !a.IsUserBlocked &&
                                                        a.IsActive);
        }

        public async Task<User> GetVerifyUser(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;
            return await _Context.Users.FirstOrDefaultAsync(a => a.VerifyCode == code &&
                                                        !a.IsUserBlocked &&
                                                        a.IsActive);
        }

        public async Task<User> GetSingleUser(long userID)
        {
            var userEntity = await _Context.Users.FirstOrDefaultAsync(a =>
                                    a.UserID == userID
                                    && !a.IsUserBlocked
                                    && a.IsActive);
            return userEntity;
        }

        public async Task<bool> DoesUserExistsByCryteria(string email, string password)
        {
            return await _Context.Users.AnyAsync(a => a.Email.Trim() == email.Trim() && a.Password.Trim() == password.Trim() && !a.IsUserBlocked);
        }

        public async Task<bool> DoesUserExistsByCryteria(string email, string password, bool isAdmin)
        {
            return await _Context.Users.AnyAsync(a => a.Email.Trim() == email.Trim() && a.Password.Trim() == password.Trim() && a.UserAccountType == Common.EnumUserAccountType.SuperAdmin && !a.IsUserBlocked);
        }

        public async Task<bool> DoesUserExistsByCryteria(bool isAdmin, string pin, string passcode)
        {
            return await _Context.Users.AnyAsync(a => a.UserAccountType == Common.EnumUserAccountType.SuperAdmin && a.TempAdminPinNumber.Equals(pin) && a.AdminPassCode.Equals(passcode) && !a.IsUserBlocked);
        }

        public async Task<bool> DoesUserEmailExists(string email)
        {
            return await _Context.Users.AnyAsync(a => a.Email.Trim() == email.Trim() && !a.IsUserBlocked);
        }    

        public async Task<bool> UpdateUser(User user)
        {
            if (user != null)
            {
                _Context.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;
                await _Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> AddUser(User user)
        {
            if (user != null)
            {
                _Context.Users.Add(user);
                await _Context.SaveChangesAsync();
            }
            return true;
        }
        
        public async Task<bool> CreditUserAccountBalance(int userId, double creditAmount, long currentUserID, EnumCountry country)
        {
            var user = await _Context.Users.FirstOrDefaultAsync(a => a.UserID == userId);
            var curBalance = user.AccountBalance;
            var newBalance = curBalance + creditAmount;
            user.AccountBalance = newBalance;
            user.UpdateModifiedDate(currentUserID, country);
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DebitUserAccountBalance(long userId, double debitAmount, long currentUserID, EnumCountry country)
        {
            var user = await _Context.Users.FirstOrDefaultAsync(a => a.UserID == userId);
            var curBalance = user.AccountBalance;
            var newBalance = curBalance - debitAmount;
            user.AccountBalance = newBalance;
            user.UpdateModifiedDate(currentUserID, country);
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddPackageToUser(int userId, PackageConfig objPackage, EnumCountry country)
        {
            var user = await _Context.Users.FirstOrDefaultAsync(a => a.UserID == userId && a.IsActive);
            if (user == null)
                return false;            
            await _Context.SaveChangesAsync();
            _Context.UserPackageHistorys.Add(new UserPackageHistory(userId, objPackage.PackageConfigID, country));
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAllUser()
        {
            var userList = await _Context.Users.Where(a => a.IsActive && !a.IsUserBlocked).ToListAsync();
            return userList;
        }

        public async Task<List<User>> GetAllUser(bool allTypesOfUser)
        {
            var userList = await _Context.Users
                .OrderBy(a=>a.IsActive)
                .ThenBy(a=>a.UserAccountType)
                .ToListAsync();
            return userList;
        }
    }
}
