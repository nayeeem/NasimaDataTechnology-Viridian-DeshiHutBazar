using System.Collections.Generic;
using System.Linq;
using Model;
using Data;
using Common;
using System.Threading.Tasks;

namespace WebDeshiHutBazar
{
    public class PriceConfigurationService : IPriceConfigurationService
    {
        public readonly IPackageConfigRepository _PackageConfigRepository;
        public readonly IPriceConfigRepository _PriceConfigRepository;
        public readonly IPostRepository _PostRepository;
        public readonly IAccountBillRepository _AccountBillRepository;
        public readonly IRepoDropDownDataList _DropDownDataList;

        public PriceConfigurationService(
            IPackageConfigRepository packageConfigRepository,
            IPriceConfigRepository priceConfigRepository,
            IPostRepository postRepository,
            IAccountBillRepository accountBillRepository,
            IRepoDropDownDataList dropDownDataList
            )
        {
            _PackageConfigRepository = packageConfigRepository;
            _PriceConfigRepository = priceConfigRepository;
            _PostRepository = postRepository;
            _AccountBillRepository = accountBillRepository;
            _DropDownDataList = dropDownDataList;
        }
        
        public async Task<PriceConfig> GetSinglePriceConfig(int packageId, int subCatId)
        {
            var singlePriceConfig = await _PriceConfigRepository.GetSinglePriceConfig(packageId, subCatId);
            return singlePriceConfig;
        }

        public async Task<PriceConfig> GetSinglePriceConfig(int priceConfigID)
        {
            var singlePriceConfig = await _PriceConfigRepository.GetSinglePriceConfig(priceConfigID);
            return singlePriceConfig;
        }

        public async Task<List<PriceConfig>> GetPriceConfigListByID(int? packageID, int? countryID)
        {
            var listPrices = await _PriceConfigRepository.GetPriceConfigListByID(packageID, countryID);
            return listPrices;
        }

        public async Task<List<PriceConfig>> GetPriceConfigListByCoyuntryID(int countryID)
        {
            var listPrices = await _PriceConfigRepository.GetPriceConfigListByCoyuntryID(countryID);
            return listPrices.ToList();
        }

        public async Task<List<PriceConfig>> GetPriceConfigListByPackageID(int packageID)
        {
            var listPrices = await _PriceConfigRepository.GetPriceConfigListByPackageID(packageID);
            return listPrices;
        }

        public async Task<List<PriceConfig>> GetAllPriceConfigList()
        {
            var listPrices = await _PriceConfigRepository.GetAllPriceConfigList();
            return listPrices;
        }

        public async Task<PostPriceConfigInformationViewModel> GetInformationViewModel(PostPriceConfigInformationViewModel objInformation)
        {
            PostPriceConfigInformationViewModel objInformationModel = new PostPriceConfigInformationViewModel();
            List<PriceConfig> objListPostPriceConfig;
            if (objInformation != null && objInformation.CountryID.HasValue && objInformation.PackageID.HasValue)
            {
                objListPostPriceConfig = await GetPriceConfigListByID((int)objInformation.PackageID, (int)objInformation.CountryID);
            }
            else if (objInformation != null && objInformation.CountryID.HasValue)
            {
                objListPostPriceConfig = await GetPriceConfigListByCoyuntryID((int)objInformation.CountryID);
            }
            else if (objInformation != null && objInformation.PackageID.HasValue)
            {
                objListPostPriceConfig = await GetPriceConfigListByPackageID((int)objInformation.PackageID);
            }
            else
            {
                objListPostPriceConfig = await GetAllPriceConfigList();
            }


            List<PostPriceConfigurationViewModel> objList = new List<PostPriceConfigurationViewModel>();
            PostPriceConfigurationViewModel objSingleViewModel;

            foreach (var item in objListPostPriceConfig)
            {
                objSingleViewModel = new PostPriceConfigurationViewModel();
                objSingleViewModel.PostPriceConfigID = item.PostPriceConfigID;
                objSingleViewModel.OfferName = item.OfferName;
                objSingleViewModel.OfferType = item.OfferType;
                objSingleViewModel.OfferDiscount = item.OfferDiscount;
                objSingleViewModel.CountryId = item.ConfigurationCountry;
                objSingleViewModel.NoFreePost = item.OfferFreePost;
                objSingleViewModel.Currency = item.CountryCurrency;
                objSingleViewModel.Price = item.OfferPrice;
                objSingleViewModel.FormattedPriceValue = objSingleViewModel.GetFormatedPriceValue(objSingleViewModel.Price.HasValue ? objSingleViewModel.Price.Value.ToString() : "0 BDT");
                objSingleViewModel.SubCategoryID = item.SubCategoryID;
                objSingleViewModel.PackageID = item.PackageConfigID.Value;
                objSingleViewModel.DisplayPackageName = await _DropDownDataList.GetPackageNameText(item.PackageConfigID.Value);
                objSingleViewModel.DisplayCountryName = LocationRelatedSeed.GetCountryDescription((EnumCountry)item.ConfigurationCountry);
                objSingleViewModel.DisplayCurrency = LocationRelatedSeed.GetCurrencyDescription((EnumCurrency)item.CountryCurrency);
                objSingleViewModel.DisplaySubCategory = BusinessObjectSeed.GetCateSubCategoryItemText(item.SubCategoryID);
                objList.Add(objSingleViewModel);
            }

            objInformationModel.ListPriceConfig = objList;
            objInformationModel.AV_Country = DropDownDataList.GetCountryList();
            objInformationModel.AV_Package = await _DropDownDataList.GetPackageList();
            objInformationModel.PackageID = 0;
            objInformationModel.CountryID = 0;

            return objInformationModel;
        }

        public async Task<PostPriceConfigurationViewModel> GetNewPostPriceConfigViewModel(PriceConfig objConfig)
        {
            var objSingleViewModel = new PostPriceConfigurationViewModel
            {
                OfferType = objConfig.OfferType,
                DisplayOfferType = ((EnumOfferType) objConfig.OfferType).ToString(),
                OfferName = objConfig.OfferName,
                OfferDiscount = objConfig.OfferDiscount,
                PostPriceConfigID = objConfig.PostPriceConfigID,
                CountryId = objConfig.ConfigurationCountry,
                NoFreePost = objConfig.OfferFreePost,
                Price = objConfig.OfferPrice,
                Currency = objConfig.CountryCurrency,
                SubCategoryID = objConfig.SubCategoryID,
                PackageID = objConfig.PackageConfigID.Value,
                DisplayPackageName = await _DropDownDataList.GetPackageNameText(objConfig.PackageConfigID.Value),
                DisplayCountryName = LocationRelatedSeed.GetCountryDescription((EnumCountry)objConfig.ConfigurationCountry),
                DisplayCurrency = LocationRelatedSeed.GetCurrencyDescription((EnumCurrency)objConfig.CountryCurrency),
                DisplaySubCategory = BusinessObjectSeed.GetCateSubCategoryItemText((long?)objConfig.SubCategoryID),
                PageName = "Price Config Details Page",
            };
            objSingleViewModel.FormattedPriceValue = objSingleViewModel.GetFormatedPriceValue(objSingleViewModel.Price.HasValue ? objSingleViewModel.Price.Value.ToString() : "");
            return objSingleViewModel;
        }

        public async Task<PostPriceConfigurationViewModel> GetNewCreatePostConfigurationViewModel()
        {
            var objSingleViewModel = new PostPriceConfigurationViewModel
            {
                AV_Currency = DropDownDataList.GetCurrencyList(),
                AV_Country = DropDownDataList.GetCountryList(),
                AV_SubCategory = DropDownDataList.GetSubCategoryList(),
                AV_Package = await _DropDownDataList.GetPackageList(),
                AV_OfferType = DropDownDataList.GetOfferTypeList(),
                PageName = "Create Price Config Page"
            };
            return objSingleViewModel;
        }

        public async Task<bool> DoesPriceConfigExists(EnumCountry? countryId, long? subCategoryID, int? packageID)
        {
            return await _PriceConfigRepository.DoesPriceConfigExists(countryId, subCategoryID, packageID);
        }

        public async Task<bool> AddPriceConfig(PostPriceConfigurationViewModel objPostPriceConfigurationViewModel, 
            long currentUserID, 
            EnumCountry country)
        {
            var package = await _PackageConfigRepository.GetSinglePackage((int)objPostPriceConfigurationViewModel.PackageID);
            PriceConfig objEntity = new PriceConfig(package, 
                objPostPriceConfigurationViewModel.CountryId, 
                objPostPriceConfigurationViewModel.Currency, 
                objPostPriceConfigurationViewModel.SubCategoryID, 
                objPostPriceConfigurationViewModel.Price, 
                objPostPriceConfigurationViewModel.NoFreePost, 
                objPostPriceConfigurationViewModel.OfferDiscount,
                country);
            objEntity.OfferName = objPostPriceConfigurationViewModel.OfferName;
            objEntity.OfferType = objPostPriceConfigurationViewModel.OfferType;
            objEntity.CreatedBy = currentUserID;
            await _PriceConfigRepository.AddPriceConfig(objEntity, currentUserID);
            return true;
        }

        public async Task<PostPriceConfigurationViewModel> GetSinglePriceConfigViewModel(int id)
        {
            var objConfig = await GetSinglePriceConfig(id);
            var objSingleViewModel = new PostPriceConfigurationViewModel
            {
                PostPriceConfigID = objConfig.PostPriceConfigID,
                CountryId = objConfig.ConfigurationCountry,
                NoFreePost = objConfig.OfferFreePost,
                Price = objConfig.OfferPrice,
                SubCategoryID = objConfig.SubCategoryID,
                Currency = objConfig.CountryCurrency,
                PackageID = objConfig.PackageConfigID.Value,
                DisplayPackageName = await _DropDownDataList.GetPackageNameText(objConfig.PackageConfigID.Value),
                DisplayCountryName = LocationRelatedSeed.GetCountryDescription((EnumCountry)objConfig.ConfigurationCountry),
                DisplayCurrency = LocationRelatedSeed.GetCurrencyDescription((EnumCurrency)objConfig.CountryCurrency),
                DisplaySubCategory =  BusinessObjectSeed.GetCateSubCategoryItemText(objConfig.SubCategoryID),
                AV_Currency = DropDownDataList.GetCurrencyList(),
                AV_Country = DropDownDataList.GetCountryList(),
                AV_SubCategory =  DropDownDataList.GetSubCategoryList(),
                AV_Package = await _DropDownDataList.GetPackageList(),
                AV_OfferType = DropDownDataList.GetOfferTypeList(),
                PageName = "Edit Price Config Page",
                OfferType = objConfig.OfferType,
                DisplayOfferType = objConfig.OfferType.ToString(),
                OfferName = objConfig.OfferName,
                OfferDiscount = objConfig.OfferDiscount
            };
            objSingleViewModel.FormattedPriceValue = objSingleViewModel.GetFormatedPriceValue(objSingleViewModel.Price.HasValue ? objSingleViewModel.Price.Value.ToString() : "");
            return objSingleViewModel;
        }

        public async Task<bool> UpdatePriceConfig(int id, 
            PostPriceConfigurationViewModel objPriceConfigViewModel, 
            long currentUserID,
            EnumCountry country)
        {
            var result = await _PriceConfigRepository.UpdatePriceConfig(
                id, 
                objPriceConfigViewModel.NoFreePost, 
                objPriceConfigViewModel.Price,
                objPriceConfigViewModel.OfferDiscount,
                currentUserID,
                country);
            return true;
        }

        public async Task<bool> DeletePriceConfig(int id, long currentUserID, EnumCountry country)
        {
            var result = await _PriceConfigRepository.DeletePriceConfig(id, currentUserID, country);
            return result;
        }
    }
}
