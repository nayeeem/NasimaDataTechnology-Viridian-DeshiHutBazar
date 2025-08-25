using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common;
namespace Model
{        
    public class User : BaseEntity
    {                
        public User() {
        }
        
        public User(string email, 
                    string password,
                    string clientName, 
                    EnumUserAccountType userAccountType,
                    byte[] salt, 
                    EnumCountry country)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password not provided.");
            }
               
            Email = string.IsNullOrEmpty(email)     ? 
                    "fbguest@nasiii.com"            : 
                    email;
            Password = password;
            ClientName = clientName;
            UserAccountType = userAccountType;
            Salt = salt;
            IsUserBlocked = false;
            EnumCountry = country;
            Roles = userAccountType == EnumUserAccountType.IndividualAdvertiser ?
                EnumRoles.Advertiser.ToString() :
                userAccountType == EnumUserAccountType.Company ?
                EnumRoles.Company.ToString() :
                userAccountType == EnumUserAccountType.SuperAdmin ?
                EnumRoles.Admin.ToString() :
                userAccountType == EnumUserAccountType.ContentAdmin ?
                EnumRoles.Admin.ToString() :
                EnumRoles.Advertiser.ToString();
            VerifyCode = GetVerifyCode();
            IsVerifiedAccount = false;
            ListBikashBills = new List<BikashBillTransacton>();
            ListUserOrders = new List<UserOrder>();
            ListUserPackages = new List<UserPackage>();

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
        }

        public User(string email,
                    string password,
                    string passCode,
                    bool isAdmin,
                    string clientName,
                    string remarks,
                    byte[] salt,
                    EnumCountry country)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password not provided.");
            }
            Email = email;
            Password = password;
            ClientName = clientName;
            Remarks = remarks;
            AdminPassCode = passCode;
            Salt = salt;
            UserAccountType = EnumUserAccountType.SuperAdmin;
            EnumCountry = country;
            IsUserBlocked = false;
            Roles = EnumRoles.Admin.ToString();
            VerifyCode = GetVerifyCode();
            IsVerifiedAccount = false;
            ListBikashBills = new List<BikashBillTransacton>();
            ListUserOrders = new List<UserOrder>();
            ListUserPackages = new List<UserPackage>();

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
        }

        [Key]
        public long UserID { get; set; }

        public EnumUserAccountType UserAccountType { get; set; }

        public bool IsUserBlocked { get; set; }

        [Required]
        public string Email { get; private set; }

        public string AdminPersonalEmail { get; set; }

        public string ClientName { get; set; }

        public string Website { get; set; }

        public string Phone { get; set; }

        public string Remarks { get; set; }

        [Required]
        public string Password { get; private set; }

        public byte[] Salt { get; set; }
        
        public string TempAdminPinNumber { get; private set; }
                
        public string AdminPassCode { get; private set; }
                        
        public string Roles { get; set; }                                       

        public bool IsVerifiedAccount { get; set; }

        public string VerifyCode { get; set; }
        
        public double AccountBalance { get; set; }

        public virtual List<BikashBillTransacton> ListBikashBills { get; set; }

        public virtual List<UserPackage> ListUserPackages { get; set; }

        public virtual List<UserOrder> ListUserOrders { get; set; }

        public virtual List<Post> ListPosts { get; set; }

        public virtual List<OrderBillItem> ListProductOrderBills { get; set; }

        public long GetPostCount
        {
            get
            {
                if(ListPosts != null)
                {
                    return ListPosts.Count;
                }
                return 0;
            }
        }

        public void SetPassword(string password, byte[] salt)
        {
            Password = password;
            Salt = salt;
        }

        private string GetVerifyCode()
        {
            Random code = new Random();
            int codeNumber = code.Next(999999);
            var finalVerifyCode = codeNumber.ToString();
            return finalVerifyCode;
        }

        public void SetPhone(string phone)
        {
            this.Phone = phone;
        }

        public void BlockThisUser(bool isBlocked)
        {
            IsUserBlocked = isBlocked;
        }

        public void SetAdminPassCode(string code)
        {
            AdminPassCode = code;
        }

        public void SetAdminPin(string pin)
        {
            TempAdminPinNumber = pin;
        }
    }
}
