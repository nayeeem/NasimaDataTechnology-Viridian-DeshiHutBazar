using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Model;

namespace Data
{
    public interface IPriceConfigRepository
    {
        Task<PriceConfig> GetSinglePriceConfig(int packageId, int subCatId);

        Task<List<PriceConfig>> GetPriceConfigListByID(int? packageID, int? countryID);

        Task<List<PriceConfig>> GetPriceConfigListByCoyuntryID(int countryID);

        Task<List<PriceConfig>> GetPriceConfigListByPackageID(int packageID);

        Task<List<PriceConfig>> GetAllPriceConfigList();

        Task<PriceConfig> GetSinglePriceConfig(int priceConfigID);

        Task<bool> DoesPriceConfigExists(EnumCountry? countryId, long? subCategoryID, int? packageID);

        Task<bool> AddPriceConfig(PriceConfig objPriceConfig, long currentUserID);

        Task<bool> UpdatePriceConfig(int id, int? noOfPost, double? price, int? discount, long currentUserID, EnumCountry country);

        Task<bool> DeletePriceConfig(int id, long currentUserID, EnumCountry country);
    }
}