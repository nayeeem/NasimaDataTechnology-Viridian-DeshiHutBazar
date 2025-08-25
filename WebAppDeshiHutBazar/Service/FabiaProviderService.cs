using System.Linq;
using System.Collections.Generic;
using Common;
using System;
using System.Threading.Tasks;
using System.Data.Entity;
using Data;
using Model;

namespace WebDeshiHutBazar
{
    public class FabiaProviderServService : IFabiaProviderService
    {
        private readonly IFabiaProviderRepository _FabiaProviderRepo;
        private readonly HashingCryptographyService _HashingService;
        private readonly IRepoDropDownDataList _DropDownRepository;

        public FabiaProviderServService(IFabiaProviderRepository fabiaProviderRepo,            
            HashingCryptographyService hashingService,
            IRepoDropDownDataList dropdownRepo)
        {
            _FabiaProviderRepo = fabiaProviderRepo;
            _HashingService = hashingService;
            _DropDownRepository = dropdownRepo;
        }

        public async Task<bool> DeleteProvider(long providerId)
        {
            try
            {
                var singleProvider = await _FabiaProviderRepo.DeleteProvider(providerId);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<List<FabiaProviderViewModel>> GetAllProviderByServiceBy(long fabiaServiceID)
        {
            try
            {
                var listProviders = await _FabiaProviderRepo.GetAllProviderByServiceID(fabiaServiceID);
                FabiaProviderViewModel objProviderViewModel;
                List<FabiaProviderViewModel> objListVM = new List<FabiaProviderViewModel>();
                foreach (var providerEntity in listProviders.OrderBy(a => a.ProviderName).ThenBy(a => a.ServiceTitle).ToList())
                {
                    objProviderViewModel = new FabiaProviderViewModel();
                    objProviderViewModel.FabiaServiceID = providerEntity.FabiaServiceID.Value;
                    objProviderViewModel.ProviderID = providerEntity.ProviderID;
                    objProviderViewModel.ServiceTitle = providerEntity.ServiceTitle;
                    objProviderViewModel.ProviderName = providerEntity.ProviderName;
                    objProviderViewModel.ServiceDescription = providerEntity.ServiceDescription;
                    objProviderViewModel.ServiceCharge = providerEntity.ServiceCharge;
                    objProviderViewModel.ProviderPhone = providerEntity.ProviderPhone;
                    objProviderViewModel.StateID = providerEntity.StateID;
                    objProviderViewModel.ProfileImage = providerEntity.ProfileImage;
                    objProviderViewModel.DisplayState = DropDownDataList.GetAllStateList().Any(a => a.Value == providerEntity.StateID.ToString().Trim())
                                            ? DropDownDataList.GetAllStateList().FirstOrDefault(a => a.Value == providerEntity.StateID.ToString().Trim()).Text
                                            : "";
                    objListVM.Add(objProviderViewModel);
                }
                return objListVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FabiaProviderViewModel>> GetAllProvider(long userId)
        {
            try
            {
                var objProviderServiceCateList = await _DropDownRepository.GetFabiaServiceCategoryList();
                var listProviders = await _FabiaProviderRepo.GetAllProvider(userId);
                FabiaProviderViewModel objProviderViewModel;
                List<FabiaProviderViewModel> objListVM = new List<FabiaProviderViewModel>();
                foreach (var providerEntity in listProviders.OrderBy(a => a.ProviderName).ThenBy(a => a.ServiceTitle).ToList())
                {
                    objProviderViewModel = new FabiaProviderViewModel();                  
                    objProviderViewModel.FabiaServiceID = providerEntity.FabiaServiceID.Value;
                    objProviderViewModel.ProviderID = providerEntity.ProviderID;                   
                    objProviderViewModel.ServiceTitle = providerEntity.ServiceTitle;
                    objProviderViewModel.ProviderName = providerEntity.ProviderName;
                    objProviderViewModel.ServiceDescription = providerEntity.ServiceDescription;
                    objProviderViewModel.ServiceCharge = providerEntity.ServiceCharge;
                    objProviderViewModel.ProviderPhone = providerEntity.ProviderPhone;
                    objProviderViewModel.StateID = providerEntity.StateID;
                    objProviderViewModel.ProfileImage = providerEntity.ProfileImage;
                    objProviderViewModel.DisplayState = DropDownDataList.GetAllStateList().Any(a => a.Value == providerEntity.StateID.ToString().Trim())
                                            ? DropDownDataList.GetAllStateList().FirstOrDefault(a => a.Value == providerEntity.StateID.ToString().Trim()).Text
                                            : "";
                    objProviderViewModel.DisplayServiceCategory = objProviderServiceCateList.Any(a => a.Value == providerEntity.FabiaServiceID.ToString())
                                                    ? objProviderServiceCateList.FirstOrDefault(a => a.Value == providerEntity.FabiaServiceID.ToString()).Text
                                                    : "";
                    objListVM.Add(objProviderViewModel);
                }
                return objListVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FabiaProviderViewModel> GetProviderByID(long? providerId)
        {
            try
            {
                var singleProviderObject = await _FabiaProviderRepo.GetProviderByID(providerId);
                if (singleProviderObject != null)
                {
                    FabiaProviderViewModel objModel = new FabiaProviderViewModel();
                    objModel.ProfileImage = singleProviderObject.ProfileImage;
                    objModel.FabiaServiceID = singleProviderObject.FabiaServiceID.Value;
                    objModel.ProviderID = singleProviderObject.ProviderID;                   
                    objModel.ServiceTitle = singleProviderObject.ServiceTitle;                    
                    objModel.ProviderPhone = singleProviderObject.ProviderPhone;
                    objModel.Website = singleProviderObject.ProviderWebsite;
                    objModel.ServiceDescription = singleProviderObject.ServiceDescription;
                    objModel.ProviderName = singleProviderObject.ProviderName;
                    objModel.ServiceCharge = singleProviderObject.ServiceCharge;
                    objModel.StateID = singleProviderObject.StateID;
                    objModel.DisplayState = DropDownDataList.GetAllStateList().Any(a => a.Value == singleProviderObject.StateID.ToString().Trim())
                                            ? DropDownDataList.GetAllStateList().FirstOrDefault(a => a.Value == singleProviderObject.StateID.ToString().Trim()).Text
                                            : "";
                    var userEntity = singleProviderObject.User;
                    if (userEntity != null)
                    {
                        objModel.Email = userEntity.Email;
                    }
                    return objModel;
                }
                return new FabiaProviderViewModel();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private User CreateNewUser(EnumCountry country, FabiaProviderViewModel objPostVM)
        {
            var passwordVM = _HashingService.GetMessageDigest(objPostVM.Password);
            var objUser = new User(objPostVM.Email, passwordVM.Digest, objPostVM.ProviderName, EnumUserAccountType.IndividualAdvertiser, passwordVM.Salt, country)
            {
                Phone = objPostVM.ProviderPhone
            };
            return objUser;
        }

        public async Task<long> SaveProvider(FabiaProviderViewModel singleProviderViewModel)
        {
            try
            {
                if (singleProviderViewModel != null)
                {
                    Provider objProviderEntity = new Provider(
                        singleProviderViewModel.ProviderName,
                        singleProviderViewModel.ProviderPhone,
                        singleProviderViewModel.FabiaServiceID,
                        singleProviderViewModel.ServiceTitle,
                        EnumCountry.Bangladesh
                        );
                    
                    if (!singleProviderViewModel.IsExistingUser) {
                        objProviderEntity.User = CreateNewUser(EnumCountry.Bangladesh, singleProviderViewModel);
                    }
                    else  {
                        objProviderEntity.UserID = singleProviderViewModel.UserID;
                    }
                    objProviderEntity.ProfileImage = singleProviderViewModel.ProfileImage;
                    objProviderEntity.ProviderWebsite = singleProviderViewModel.Website;
                    objProviderEntity.ServiceCharge = singleProviderViewModel.ServiceCharge;
                    objProviderEntity.ServiceDescription = singleProviderViewModel.ServiceDescription;
                    objProviderEntity.ProviderWebsite = singleProviderViewModel.Website;
                    objProviderEntity.StateID = singleProviderViewModel.StateID.Value;
                    var result = await _FabiaProviderRepo.SaveProvider(objProviderEntity);
                    return objProviderEntity.UserID.Value;
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateProvider(FabiaProviderViewModel singleProviderViewModel)
        {
            try
            {
                if (singleProviderViewModel != null)
                {
                    var objProviderEntity = await _FabiaProviderRepo.GetProviderByID(singleProviderViewModel.ProviderID);
                    objProviderEntity.ProviderName = singleProviderViewModel.ProviderName;
                    objProviderEntity.ProfileImage = singleProviderViewModel.ProfileImage;
                    objProviderEntity.ProviderWebsite = singleProviderViewModel.Website;
                    objProviderEntity.ServiceCharge = singleProviderViewModel.ServiceCharge;
                    objProviderEntity.ServiceDescription = singleProviderViewModel.ServiceDescription;
                    objProviderEntity.ProviderWebsite = singleProviderViewModel.Website;
                    objProviderEntity.StateID = singleProviderViewModel.StateID.Value;
                    objProviderEntity.ProviderPhone = singleProviderViewModel.ProviderPhone;
                    objProviderEntity.ServiceTitle = singleProviderViewModel.ServiceTitle;
                    var result = await _FabiaProviderRepo.SaveChanges();
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
