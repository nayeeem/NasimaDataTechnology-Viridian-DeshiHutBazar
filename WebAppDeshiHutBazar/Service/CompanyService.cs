using Model;
using Data;
using Common;
using System.Threading.Tasks;
using System.Linq;

namespace WebDeshiHutBazar
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _ICompanyRepository;
        private readonly IUserAccountService _IUserAccountService;

        public CompanyService(
            ICompanyRepository companyRepository,
            IUserAccountService userAccountService)
        {
            _ICompanyRepository = companyRepository;
            _IUserAccountService = userAccountService;
        }

        public async Task<CompanyAccountViewModel> GetCompanyByID(long companyID)
        {
            var companyEntity = await _ICompanyRepository.GetCompanyByID(companyID);
            CompanyAccountViewModel objCompanyVM = new CompanyAccountViewModel();
            objCompanyVM.Agreement = companyEntity.Agreement;
            objCompanyVM.CompanyName = companyEntity.CompanyName;
            objCompanyVM.CompanyPhone = companyEntity.CompanyPhone;
            objCompanyVM.CompanyEmail = companyEntity.CompanyEmail;
            objCompanyVM.CompanyWebsite = companyEntity.CompanyWebsite;
            objCompanyVM.AccountHolderName = companyEntity.AccountHolderName;
            objCompanyVM.AccountNumber = companyEntity.AccountHolderName;
            objCompanyVM.ACHolderContactNumber = companyEntity.AccountHolderName;
            objCompanyVM.BankName = companyEntity.AccountHolderName;
            objCompanyVM.BkashAccountNumber = companyEntity.AccountHolderName;
            objCompanyVM.BranchName = companyEntity.AccountHolderName;            
            objCompanyVM.ShopContactEmail = companyEntity.ShopContactEmail;
            objCompanyVM.ShopContactName = companyEntity.ShopContactName;
            objCompanyVM.ShopContactPhoneNumber = companyEntity.ShopContactPhoneNumber;
            objCompanyVM.TradeLicenseFile = companyEntity.TradeLicenseFile;
            objCompanyVM.OwnerNIDFile = companyEntity.OwnerNIDFile;
            objCompanyVM.OwnerName = companyEntity.OwnerName;
            objCompanyVM.OwnerEmail = companyEntity.OwnerEmail;
            objCompanyVM.OwnerPhoneNumber = companyEntity.OwnerPhoneNumber;

            var shopAddress = companyEntity.ShopAddress;
            objCompanyVM.ShopAddress = new CompanyAddressViewModel();
            objCompanyVM.ShopAddress.AddressDetails = shopAddress.AddressDetails;
            objCompanyVM.ShopAddress.ApartmentNo = shopAddress.ApartmentNo;
            objCompanyVM.ShopAddress.Area = shopAddress.Area;
            objCompanyVM.ShopAddress.Block = shopAddress.Block;
            objCompanyVM.ShopAddress.City = shopAddress.City;
            objCompanyVM.ShopAddress.CompanyAddressID = shopAddress.CompanyAddressID;
            objCompanyVM.ShopAddress.DisplayState = GetStateName(shopAddress.StateID);
            objCompanyVM.ShopAddress.HouseNo = shopAddress.HouseNo;
            objCompanyVM.ShopAddress.LandMark = shopAddress.LandMark;
            objCompanyVM.ShopAddress.RoadNo = shopAddress.RoadNo;
            objCompanyVM.ShopAddress.StateID = shopAddress.StateID;
            objCompanyVM.ShopAddress.ZipCode = shopAddress.ZipCode;

            var companyAddress = companyEntity.CompanyAddress;
            objCompanyVM.CompanyAddress = new CompanyAddressViewModel();
            objCompanyVM.CompanyAddress.AddressDetails = companyAddress.AddressDetails;
            objCompanyVM.CompanyAddress.ApartmentNo = companyAddress.ApartmentNo;
            objCompanyVM.CompanyAddress.Area = companyAddress.Area;
            objCompanyVM.CompanyAddress.Block = companyAddress.Block;
            objCompanyVM.CompanyAddress.City = companyAddress.City;
            objCompanyVM.CompanyAddress.CompanyAddressID = companyAddress.CompanyAddressID;
            objCompanyVM.CompanyAddress.DisplayState = GetStateName(companyAddress.StateID);
            objCompanyVM.CompanyAddress.HouseNo = companyAddress.HouseNo;
            objCompanyVM.CompanyAddress.LandMark = companyAddress.LandMark;
            objCompanyVM.CompanyAddress.RoadNo = companyAddress.RoadNo;
            objCompanyVM.CompanyAddress.StateID = companyAddress.StateID;
            objCompanyVM.CompanyAddress.ZipCode = companyAddress.ZipCode;
            
            var userEntity = companyEntity.User;
            if(userEntity != null)
            {
                objCompanyVM.UserID = userEntity.UserID;
                objCompanyVM.Website = userEntity.Website;
                objCompanyVM.ClientName = userEntity.ClientName;
            }

            return objCompanyVM;
        }

        private string GetStateName(int stateID)
        {
            var state = DropDownDataList.GetAllStateList().FirstOrDefault(a => a.Value == stateID.ToString());
            if (state != null)
                return state.Text;
            return "";
        }

        public async Task<CompanyAccountViewModel> GetCompanyByUserID(long userID)
        {
            CompanyAccountViewModel objCompanyVM = new CompanyAccountViewModel();
            var companyEntity = await _ICompanyRepository.GetCompanyByUserID(userID);
            if(companyEntity == null)
            {
                User userEntityObj = await _IUserAccountService.GetAuthorizedUser(userID);
                if (userEntityObj != null)
                {
                    objCompanyVM.UserID = userEntityObj.UserID;
                    objCompanyVM.Website = userEntityObj.Website;
                    objCompanyVM.ClientName = userEntityObj.ClientName;
                    objCompanyVM.Email = userEntityObj.Email;
                }
                objCompanyVM.ShopAddress = new CompanyAddressViewModel();
                objCompanyVM.ShopAddress.AV_State = DropDownDataList.GetAllStateList();
                objCompanyVM.CompanyAddress = new CompanyAddressViewModel();
                objCompanyVM.CompanyAddress.AV_State = DropDownDataList.GetAllStateList();
                return objCompanyVM;
            }
            
            objCompanyVM.CompanyID = companyEntity.CompanyID;
            objCompanyVM.Agreement = companyEntity.Agreement;
            objCompanyVM.CompanyName = companyEntity.CompanyName;
            objCompanyVM.CompanyPhone = companyEntity.CompanyPhone;
            objCompanyVM.CompanyEmail = companyEntity.CompanyEmail;
            objCompanyVM.CompanyWebsite = companyEntity.CompanyWebsite;
            objCompanyVM.AccountHolderName = companyEntity.AccountHolderName;
            objCompanyVM.AccountNumber = companyEntity.AccountHolderName;
            objCompanyVM.ACHolderContactNumber = companyEntity.AccountHolderName;
            objCompanyVM.BankName = companyEntity.AccountHolderName;
            objCompanyVM.BkashAccountNumber = companyEntity.AccountHolderName;
            objCompanyVM.BranchName = companyEntity.AccountHolderName;
            objCompanyVM.ShopContactEmail = companyEntity.ShopContactEmail;
            objCompanyVM.ShopContactName = companyEntity.ShopContactName;
            objCompanyVM.ShopContactPhoneNumber = companyEntity.ShopContactPhoneNumber;
            objCompanyVM.TradeLicenseFile = companyEntity.TradeLicenseFile;
            objCompanyVM.OwnerNIDFile = companyEntity.OwnerNIDFile;
            objCompanyVM.OwnerName = companyEntity.OwnerName;
            objCompanyVM.OwnerEmail = companyEntity.OwnerEmail;
            objCompanyVM.OwnerPhoneNumber = companyEntity.OwnerPhoneNumber;

            var shopAddress = companyEntity.ShopAddress;
            objCompanyVM.ShopAddress = new CompanyAddressViewModel();
            objCompanyVM.ShopAddress.CompanyAddressID = shopAddress.CompanyAddressID;
            objCompanyVM.ShopAddress.AddressDetails = shopAddress.AddressDetails;
            objCompanyVM.ShopAddress.ApartmentNo = shopAddress.ApartmentNo;
            objCompanyVM.ShopAddress.Area = shopAddress.Area;
            objCompanyVM.ShopAddress.Block = shopAddress.Block;
            objCompanyVM.ShopAddress.City = shopAddress.City;
            objCompanyVM.ShopAddress.CompanyAddressID = shopAddress.CompanyAddressID;
            objCompanyVM.ShopAddress.DisplayState = GetStateName(shopAddress.StateID);
            objCompanyVM.ShopAddress.HouseNo = shopAddress.HouseNo;
            objCompanyVM.ShopAddress.LandMark = shopAddress.LandMark;
            objCompanyVM.ShopAddress.RoadNo = shopAddress.RoadNo;
            objCompanyVM.ShopAddress.StateID = shopAddress.StateID;
            objCompanyVM.ShopAddress.ZipCode = shopAddress.ZipCode;
            objCompanyVM.ShopAddress.AV_State = DropDownDataList.GetAllStateList();

            var companyAddress = companyEntity.CompanyAddress;
            objCompanyVM.CompanyAddress = new CompanyAddressViewModel();
            objCompanyVM.CompanyAddress.CompanyAddressID = companyAddress.CompanyAddressID;
            objCompanyVM.CompanyAddress.AddressDetails = companyAddress.AddressDetails;
            objCompanyVM.CompanyAddress.ApartmentNo = companyAddress.ApartmentNo;
            objCompanyVM.CompanyAddress.Area = companyAddress.Area;
            objCompanyVM.CompanyAddress.Block = companyAddress.Block;
            objCompanyVM.CompanyAddress.City = companyAddress.City;
            objCompanyVM.CompanyAddress.CompanyAddressID = companyAddress.CompanyAddressID;
            objCompanyVM.CompanyAddress.DisplayState = GetStateName(companyAddress.StateID);
            objCompanyVM.CompanyAddress.HouseNo = companyAddress.HouseNo;
            objCompanyVM.CompanyAddress.LandMark = companyAddress.LandMark;
            objCompanyVM.CompanyAddress.RoadNo = companyAddress.RoadNo;
            objCompanyVM.CompanyAddress.StateID = companyAddress.StateID;
            objCompanyVM.CompanyAddress.ZipCode = companyAddress.ZipCode;
            objCompanyVM.CompanyAddress.AV_State = DropDownDataList.GetAllStateList();

            User userEntity = await _IUserAccountService.GetAuthorizedUser(userID);
            if (userEntity != null)
            {
                objCompanyVM.UserID = userEntity.UserID;
                objCompanyVM.Website = userEntity.Website;
                objCompanyVM.ClientName = userEntity.ClientName;
                objCompanyVM.Email = userEntity.Email;
            }

            return objCompanyVM;
        }

        public async Task<bool> SaveCompany(CompanyAccountViewModel companyVM)
        {
            var companyEntity = new Company()
            {
                UserID = companyVM.UserID
            };
            companyEntity.Agreement = companyVM.Agreement;
            companyEntity.OwnerName = companyVM.OwnerName;
            companyEntity.OwnerEmail = companyVM.OwnerEmail;
            companyEntity.OwnerPhoneNumber = companyVM.OwnerPhoneNumber;
            companyEntity.CompanyName = companyVM.CompanyName;
            companyEntity.CompanyPhone = companyVM.CompanyPhone;
            companyEntity.CompanyEmail = companyVM.CompanyEmail;
            companyEntity.CompanyWebsite = companyVM.CompanyWebsite;
            companyEntity.AccountHolderName = companyVM.AccountHolderName;
            companyEntity.AccountNumber = companyVM.AccountNumber;
            companyEntity.ACHolderContactNumber = companyVM.ACHolderContactNumber;
            companyEntity.BankName = companyVM.BankName;
            companyEntity.BkashAccountNumber = companyVM.BkashAccountNumber;
            companyEntity.BranchName = companyVM.BranchName;
            companyEntity.ShopContactEmail = companyVM.ShopContactEmail;
            companyEntity.ShopContactName = companyVM.ShopContactName;
            companyEntity.ShopContactPhoneNumber = companyVM.ShopContactPhoneNumber;
            companyEntity.TradeLicenseFile = companyVM.TradeLicenseFile;
            companyEntity.OwnerNIDFile = companyVM.OwnerNIDFile;            
            companyEntity.ShopAddress = GetNewShopAddress(companyVM);
            companyEntity.CompanyAddress = GetNewCompanyAddress(companyVM);
            var result = await _ICompanyRepository.SaveCompany(companyEntity);
            return true;
        }

        private CompanyAddress GetNewShopAddress(CompanyAccountViewModel companyVM)
        {
            if (companyVM.ShopAddress == null)
                return new CompanyAddress();

            var shopAddress = new CompanyAddress(EnumCountry.Bangladesh,
                companyVM.ShopAddress.StateID,
                companyVM.ShopAddress.City,
                companyVM.ShopAddress.ZipCode)
            {
                AddressDetails = companyVM.ShopAddress.AddressDetails,
                ApartmentNo = companyVM.ShopAddress.ApartmentNo,
                Area = companyVM.ShopAddress.Area,
                Block = companyVM.ShopAddress.Block,
                CompanyAddressID = companyVM.ShopAddress.CompanyAddressID,
                HouseNo = companyVM.ShopAddress.HouseNo,
                LandMark = companyVM.ShopAddress.LandMark,
                RoadNo = companyVM.ShopAddress.RoadNo
            };
            return shopAddress;
        }

        private CompanyAddress GetNewCompanyAddress(CompanyAccountViewModel companyVM)
        {
            if (companyVM.CompanyAddress == null)
                return new CompanyAddress();

            var companyAddress = new CompanyAddress(
            EnumCountry.Bangladesh,
            companyVM.CompanyAddress.StateID,
            companyVM.CompanyAddress.City,
            companyVM.CompanyAddress.ZipCode)
            {
                AddressDetails = companyVM.CompanyAddress.AddressDetails,
                ApartmentNo = companyVM.CompanyAddress.ApartmentNo,
                Area = companyVM.CompanyAddress.Area,
                Block = companyVM.CompanyAddress.Block,
                CompanyAddressID = companyVM.CompanyAddress.CompanyAddressID,
                HouseNo = companyVM.CompanyAddress.HouseNo,
                LandMark = companyVM.CompanyAddress.LandMark,
                RoadNo = companyVM.CompanyAddress.RoadNo
            };
            return companyAddress;
        }

        public async Task<bool> UpdateCompany(CompanyAccountViewModel companyVM)
        {
            var companyEntity = await _ICompanyRepository.GetCompanyByUserID(companyVM.UserID);
            companyEntity.Agreement = companyVM.Agreement;
            companyEntity.OwnerName = companyVM.OwnerName;
            companyEntity.OwnerEmail = companyVM.OwnerEmail;
            companyEntity.OwnerPhoneNumber = companyVM.OwnerPhoneNumber;
            companyEntity.CompanyName = companyVM.CompanyName;
            companyEntity.CompanyPhone = companyVM.CompanyPhone;
            companyEntity.CompanyEmail = companyVM.CompanyEmail;
            companyEntity.CompanyWebsite = companyVM.CompanyWebsite;
            companyEntity.AccountHolderName = companyVM.AccountHolderName;
            companyEntity.AccountNumber = companyVM.AccountNumber;
            companyEntity.ACHolderContactNumber = companyVM.ACHolderContactNumber;
            companyEntity.BankName = companyVM.BankName;
            companyEntity.BkashAccountNumber = companyVM.BkashAccountNumber;
            companyEntity.BranchName = companyVM.BranchName;
            companyEntity.ShopContactEmail = companyVM.ShopContactEmail;
            companyEntity.ShopContactName = companyVM.ShopContactName;
            companyEntity.ShopContactPhoneNumber = companyVM.ShopContactPhoneNumber;
            companyEntity.TradeLicenseFile = companyVM.TradeLicenseFile;
            companyEntity.OwnerNIDFile = companyVM.OwnerNIDFile;
            if(companyEntity.ShopAddress == null)
            {
                companyEntity.ShopAddress = GetNewShopAddress(companyVM);
            }
            else
            {
                UpdateShopAddress(companyEntity.ShopAddress, companyVM);
            }
            if (companyEntity.CompanyAddress == null)
            {
                companyEntity.CompanyAddress = GetNewCompanyAddress(companyVM);
            }
            else
            {
                UpdateCompanyAddress(companyEntity.CompanyAddress, companyVM);
            }
            var result = await _ICompanyRepository.UpdateCompany();
            return true;
        }

        private void UpdateShopAddress(CompanyAddress shopAddress, CompanyAccountViewModel companyVM)
        {
            shopAddress.StateID = companyVM.ShopAddress.StateID;
            shopAddress.ZipCode = companyVM.ShopAddress.ZipCode;
            shopAddress.City = companyVM.ShopAddress.City;
            shopAddress.ApartmentNo = companyVM.ShopAddress.ApartmentNo;
            shopAddress.Area = companyVM.ShopAddress.Area;
            shopAddress.Block = companyVM.ShopAddress.Block;
            shopAddress.HouseNo = companyVM.ShopAddress.HouseNo;
            shopAddress.LandMark = companyVM.ShopAddress.LandMark;
            shopAddress.RoadNo = companyVM.ShopAddress.RoadNo;
        }

        private void UpdateCompanyAddress(CompanyAddress companyAddress, CompanyAccountViewModel companyVM)
        {
            companyAddress.StateID = companyVM.CompanyAddress.StateID;
            companyAddress.ZipCode = companyVM.CompanyAddress.ZipCode;
            companyAddress.City = companyVM.CompanyAddress.City;
            companyAddress.ApartmentNo = companyVM.CompanyAddress.ApartmentNo;
            companyAddress.Area = companyVM.CompanyAddress.Area;
            companyAddress.Block = companyVM.CompanyAddress.Block;
            companyAddress.HouseNo = companyVM.CompanyAddress.HouseNo;
            companyAddress.LandMark = companyVM.CompanyAddress.LandMark;
            companyAddress.RoadNo = companyVM.CompanyAddress.RoadNo;
        }

        public async Task<bool> UpdateAccountPassword(CompanyAccountViewModel objCompanyAccount)
        {
            if (!string.IsNullOrEmpty(objCompanyAccount.Password) &&
               !string.IsNullOrEmpty(objCompanyAccount.RePassword) &&
               !string.IsNullOrEmpty(objCompanyAccount.CurrentPassword))
            {

                AccountViewModel objAccountVM = new AccountViewModel();
                objAccountVM.UserID = objCompanyAccount.UserID;
                objAccountVM.Password = objCompanyAccount.Password;
                objAccountVM.CurrentPassword = objCompanyAccount.CurrentPassword;
                objAccountVM.RePassword = objCompanyAccount.RePassword;
                var result = await _IUserAccountService.UpdateAccountPassword(objAccountVM);
            }
            return true;
        }
    }
}
