using System;
using Model;
using Data;
using Common;

using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace WebDeshiHutBazar
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _UserAccountRepository;
        private readonly IUserRepository _UserRepository;
        
        private readonly HashingCryptographyService _cryptographyService;

        public UserAccountService(
            IUserAccountRepository userAccountRepository,
            IUserRepository userRepository
            )
        {
            _UserAccountRepository = userAccountRepository;
            _UserRepository = userRepository;
            _cryptographyService = new HashingCryptographyService();
            
        }

        public async Task<EnumAccountCredential> ValidateUserCredential(string email, string password)
        {
            if(string.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
                return EnumAccountCredential.Invalid;

            var user = await _UserRepository.GetSingleUser(email);
            MessageDigestModel passwordModel = null;
            if (user != null)
            {
                passwordModel = _cryptographyService.GetMessageDigest(password, user.Salt);
            }

            if(passwordModel ==null)
                return EnumAccountCredential.Invalid;

            if (await _UserAccountRepository.IsThisIsAValidAuthenticationForUser(email, passwordModel.Digest))
            {
                return EnumAccountCredential.Valid;
            }
            return  EnumAccountCredential.Invalid;
        }

        public async Task<EnumAccountCredential> ValidateAdminUserCredential(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
                return EnumAccountCredential.Invalid;
            
            var user = await _UserRepository.GetSingleUser(email);
            MessageDigestModel passwordModel = null;

            if (user != null)
            {
                passwordModel = _cryptographyService.GetMessageDigest(password, user.Salt);
            }


            if (passwordModel !=null && await _UserAccountRepository.IsThisIsAValidAuthenticationForAdminUser(email, passwordModel.Digest))
            {
                return EnumAccountCredential.Valid;
            }
            return EnumAccountCredential.Invalid;
        }

        public async Task<EnumAccountCredential> ValidateAdminLoginGatewayCredential(string pin, string passcode)
        {
            EnumAccountCredential result = EnumAccountCredential.Invalid;
            if (string.IsNullOrEmpty(pin) || String.IsNullOrEmpty(passcode))
                result = EnumAccountCredential.Invalid;

            if (await _UserAccountRepository.IsThisIsAValidAuthenticationForAdminLoginGateway(pin, passcode))
                result = EnumAccountCredential.Valid;
            else
                result = EnumAccountCredential.Invalid;
            
            return result;
        }

        public async Task<User> GetAuthorizedUser(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;
            try
            {
                return await _UserAccountRepository.GetSingleAuthorizedUser(email);
            }
            catch (Exception exception)
            {
                throw new Exception("Error generated from GetAuthorizedUser from Service Layer.", exception.InnerException);
            }
        }

        public async Task<bool> GetVerifyUser(string code)
        {
            var user = await _UserRepository.GetVerifyUser(code);
            if (user.IsVerifiedAccount)
            {
                return false;
            }

            user.IsVerifiedAccount = true;
            var res = await _UserRepository.UpdateUser(user);
            return true;
        }

        public async Task<User> GetAuthorizedUser(long userid)
        {
            if (userid == default(long))
                return null;
            try
            {
                return await _UserAccountRepository.GetSingleAuthorizedUser(userid);
            }
            catch (Exception exception)
            {
                throw new Exception("Error generated from GetAuthorizedUser from Service Layer.", exception.InnerException);
            }
        }

        public async Task<bool> IsAccountRegistered(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            try
            {
                return await _UserAccountRepository.IsAccountRegistered(email);
            }
            catch (Exception exception)
            {
                throw new Exception("Error generated from IsAccountRegistered from Service Layer.", exception.InnerException);
            }
        }

        public async Task<bool> UpdateAccount(AccountViewModel account){
            var user = await _UserRepository.GetSingleUser(account.UserID);           
            user.Phone = account.Phone;
            user.ClientName = account.ClientName;
            var res = await _UserRepository.UpdateUser(user);
            return true;
        }

        public async Task<bool> UpdateAccountPassword(AccountViewModel account)
        {
            var user = await _UserRepository.GetSingleUser(account.UserID);
            var currrentpassword = _cryptographyService.GetMessageDigest(account.CurrentPassword, user.Salt);
            if (account.Password == account.RePassword && currrentpassword.Digest == user.Password)
            {
                var newpass = _cryptographyService.GetMessageDigest(account.Password);
                user.SetPassword(newpass.Digest,newpass.Salt);
                var resUpdate = await _UserRepository.UpdateUser(user);
                return true;
            }
            return false;
        }

        public async Task<string> UpdateVerifyCode(long userId)
        {
            var user = await _UserRepository.GetSingleUser(userId);
            Random code = new Random();
            var finalCode = code.Next(99999);
            user.VerifyCode = finalCode.ToString();
            user.IsVerifiedAccount = false;
            var resUpdate = await _UserRepository.UpdateUser(user);            
            return finalCode.ToString();
        }

        public async Task<string> UpdateResetPassword(string email)
        {
            var user = await _UserRepository.GetSingleUser(email);
            var newPassword = Guid.NewGuid().ToString();
            if (user != null)
            {
                var newpass = _cryptographyService.GetMessageDigest(newPassword);
                user.SetPassword(newpass.Digest, newpass.Salt);
                try
                {
                    var resUser = await _UserRepository.UpdateUser(user);
                    return newPassword;
                }
                catch
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public async Task<bool> UpdateAdminUserLoginGatewayPin(long userId, string pin)
        {
            var user = await _UserRepository.GetSingleUser(userId);
            if (user != null)
            {
                user.SetAdminPin(pin);
                var res = await _UserRepository.UpdateUser(user);
                return true;
            }
            return false;
        }

        public async Task<bool> IsUserEmailAlreadyCreated(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;
            var user = await _UserRepository.GetSingleUser(email);
            if (user != null)
                return true;
            return false;
        }

        public async Task<List<UserReportViewModel>> GetAllUsers(bool allUserTypes)
        {
            UserReportViewModel objUserReportVM;
            List<UserReportViewModel> objListUserReportVM = new List<UserReportViewModel>();
            var listUsers = await _UserRepository.GetAllUser(true);
            foreach(var userItem in listUsers.ToList())
            {
                objUserReportVM = new UserReportViewModel();
                objUserReportVM.ClientName = userItem.ClientName;
                objUserReportVM.UserName = userItem.Email;
                objUserReportVM.Email = userItem.Email;
                objUserReportVM.Phone = userItem.Phone;
                objUserReportVM.Website = userItem.Website;
                objUserReportVM.PostCount = userItem.GetPostCount;
                objUserReportVM.IsAdminUser = userItem.UserAccountType == EnumUserAccountType.SuperAdmin ? true : false;
                objUserReportVM.IsPrivateSeller = userItem.UserAccountType == EnumUserAccountType.IndividualAdvertiser ? true : false;
                objUserReportVM.IsCompanySeller = userItem.UserAccountType == EnumUserAccountType.Company ? true : false;
                objUserReportVM.CreatedDate = userItem.CreatedDate;
                objListUserReportVM.Add(objUserReportVM);
            }
            return objListUserReportVM.ToList();
        }
    }
}
