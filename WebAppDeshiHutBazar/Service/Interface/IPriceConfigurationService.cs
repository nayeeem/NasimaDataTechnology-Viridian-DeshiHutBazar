
using Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;

namespace WebDeshiHutBazar
{
    public interface IPriceConfigurationService
    {        
        Task<PriceConfig> GetSinglePriceConfig(int packageId, int subCatId);

        Task<PriceConfig> GetSinglePriceConfig(int priceConfigID);

        Task<List<PriceConfig>> GetPriceConfigListByID(int? packageID, int? countryID);

        Task<List<PriceConfig>> GetPriceConfigListByCoyuntryID(int countryID);

        Task<List<PriceConfig>> GetPriceConfigListByPackageID(int packageID);

        Task<List<PriceConfig>> GetAllPriceConfigList();

        Task<PostPriceConfigInformationViewModel> GetInformationViewModel(PostPriceConfigInformationViewModel objInformation);

        Task<PostPriceConfigurationViewModel> GetNewPostPriceConfigViewModel(PriceConfig objConfig);

        Task<PostPriceConfigurationViewModel> GetNewCreatePostConfigurationViewModel();

        Task<bool> DoesPriceConfigExists(EnumCountry? countryId, long? subCategoryID, int? packageID);

        Task<bool> AddPriceConfig(PostPriceConfigurationViewModel objPostPriceConfigurationViewModel, long currentUserID, EnumCountry country);

        Task<PostPriceConfigurationViewModel> GetSinglePriceConfigViewModel(int id);

        Task<bool> UpdatePriceConfig(int id, PostPriceConfigurationViewModel objPriceConfigViewModel, long currentUserID, EnumCountry country);

        Task<bool> DeletePriceConfig(int id, long currentUserID, EnumCountry country);
    }
}
