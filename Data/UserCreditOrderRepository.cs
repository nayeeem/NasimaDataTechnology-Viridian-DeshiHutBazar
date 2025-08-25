using Model;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Data
{
    public class UserCreditOrderRepository : WebBusinessEntityContext, IUserCreditOrderRepository
    {
        public async Task<bool> AddUserCreditOrder(UserCreditOrder userCreditOrder)
        {
            if (userCreditOrder != null)
            {
                _Context.UserCreditOrders.Add(userCreditOrder);
                await _Context.SaveChangesAsync();
            }
            return true;
        }

        //public async Task<bool> UpdateUserOrder(UserOrder userOrder)
        //{
        //    if (userOrder != null)
        //    {
        //        var userOrderEntity = await _context.UserOrders.FirstOrDefaultAsync(a => a.UserOrderID == userOrder.UserOrderID);
        //        userOrderEntity = userOrder;
        //        _context.Entry<UserOrder>(userOrderEntity).State = System.Data.Entity.EntityState.Modified


        //        var listOrderDetls = userOrderEntity.ListOrderDetails;
        //        foreach(var item in listOrderDetls)
        //        {
        //            _context.Entry<UserOrderDetail>(item).State = System.Data.Entity.EntityState.Added;
        //        }
        //        await _context.SaveChangesAsync();
        //    }
        //    return true;
        //}

        public async Task<UserCreditOrder> GetSingleUserCreditOrder(long userCreditOrderID)
        {
            var userCreditOrderObjectEntity = await _Context.UserCreditOrders.FirstOrDefaultAsync(a => a.UserCreditOrderID == userCreditOrderID && a.IsActive);
            return userCreditOrderObjectEntity;
        }

        //public async Task<long> GetAddUserID(User user)
        //{
        //    if (user != null)
        //    {
        //        _context.Users.Add(user);
        //        await _context.SaveChangesAsync();
        //    }
        //    return user.UserID;
        //}

        //public async Task<User> GetSingleUser(string email)
        //{
        //    if (string.IsNullOrEmpty(email))
        //        return null;
        //    return await _context.Users.FirstOrDefaultAsync(a => a.Email == email && !a.IsUserBlocked);
        //}



        //public async Task<User> GetSingleFBUser(string fbUserId)
        //{
        //    var result = await _context.Users.FirstOrDefaultAsync(a => a.FBUserID!=null && a.FBUserID.Trim() == fbUserId.Trim() && !a.IsUserBlocked);
        //    return result;
        //}

        //public async Task<bool> DoesUserExistsByCryteria(string email, string password)
        //{
        //    return await _context.Users.AnyAsync(a => a.Email.Trim() == email.Trim() && a.Password.Trim() == password.Trim() && !a.IsUserBlocked);
        //}

        //public async Task<bool> DoesUserExistsByCryteria(string email, string password, bool isAdmin)
        //{
        //    return await _context.Users.AnyAsync(a => a.Email.Trim() == email.Trim() && a.Password.Trim() == password.Trim() && a.IsAdminUser==true && !a.IsUserBlocked);
        //}

        //public async Task<bool> DoesUserExistsByCryteria(bool isAdmin, string pin, string passcode)
        //{
        //    return await _context.Users.AnyAsync(a => a.IsAdminUser == true && a.TempAdminPinNumber.Equals(pin) && a.AdminPassCode.Equals(passcode) && !a.IsUserBlocked);
        //}

        //public async Task<bool> DoesUserEmailExists(string email)
        //{
        //    return await _context.Users.AnyAsync(a => a.Email.Trim() == email.Trim() && !a.IsUserBlocked);
        //}

        //public async Task<int> GetUserPackageId(int userId)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(a => a.UserID == userId && a.IsActive);
        //    if (user != null)
        //        return -1;/// user.PackageConfigID.Value;
        //    return 0;
        //}

        //public async Task<bool> CreditAccountBalance(int userId, double creditAmount)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(a => a.UserID == userId);
        //    var curBalance = user.AccountBalance;
        //    var newBalance = curBalance + creditAmount;
        //    user.AccountBalance = newBalance;
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> DebitAccountBalance(int userId, double debitAmount)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(a => a.UserID == userId);
        //    var curBalance = user.AccountBalance;
        //    var newBalance = curBalance - debitAmount;
        //    user.AccountBalance = newBalance;
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> AddPackageToUser(int userId, PostPackageConfiguration objPackage)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(a => a.UserID == userId && a.IsActive);
        //    if (user == null)
        //        return false;
        //    ////user.Package = objPackage;
        //    //user.PackageConfigID = objPackage.PackageConfigID;
        //    await _context.SaveChangesAsync();
        //    _context.UserPackageHistorys.Add(new UserPackageHistory(userId, objPackage.PackageConfigID));
        //    await _context.SaveChangesAsync();
        //    return true;
        //}
    }
}
