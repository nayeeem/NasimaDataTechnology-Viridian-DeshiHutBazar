using System.Linq;
using System.Collections.Generic;
using Model;
using Common;
using System;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data
{
    public class PriceConfigRepository : WebBusinessEntityContext, IPriceConfigRepository
    {
        public async Task<PriceConfig> GetSinglePriceConfig(int packageId, int subCatId)
        {
            var singlePriceConfig = await _Context.PostPriceConfigurations.FirstOrDefaultAsync(a =>
                                                            a.PackageConfigID.HasValue &&
                                                            a.PackageConfigID == packageId &&
                                                            a.SubCategoryID.HasValue &&
                                                            a.SubCategoryID == subCatId &&
                                                            a.IsActive);
            return singlePriceConfig;
        }

        public async Task<List<PriceConfig>> GetPriceConfigListByID(int? packageID, int? countryID)
        {
            var listPrices = await _Context.PostPriceConfigurations.Where(a =>
                                                            a.ConfigurationCountry.HasValue &&
                                                            a.ConfigurationCountry == (EnumCountry)countryID &&
                                                            a.PackageConfigID.HasValue &&
                                                            a.PackageConfigID == (int) packageID &&
                                                            a.IsActive).ToListAsync();
            return listPrices.ToList();
        }

        public async Task<List<PriceConfig>> GetPriceConfigListByCoyuntryID(int countryID)
        {
            var listPrices = await _Context.PostPriceConfigurations.Where(a =>
                                                            a.ConfigurationCountry.HasValue &&
                                                            a.ConfigurationCountry == (EnumCountry)countryID &&                                                           
                                                            a.IsActive).ToListAsync();
            return listPrices.ToList();
        }

        public async Task<List<PriceConfig>> GetPriceConfigListByPackageID(int packageID)
        {
            var listPrices = await _Context.PostPriceConfigurations.Where(a =>
                                                            a.PackageConfigID.HasValue &&
                                                            a.PackageConfigID == packageID &&
                                                            a.IsActive).ToListAsync();
            return listPrices.ToList();
        }

        public async Task<List<PriceConfig>> GetAllPriceConfigList()
        {
            var listPrices = await _Context.PostPriceConfigurations.Where(a =>
                                                            a.IsActive).ToListAsync();
            return listPrices.ToList();
        }

        public async Task<PriceConfig> GetSinglePriceConfig(int priceConfigID)
        {
            var priceConfigEntity = await _Context.PostPriceConfigurations.FirstOrDefaultAsync(a => 
                                                                        a.IsActive &&
                                                                        a.PostPriceConfigID == priceConfigID);
            return priceConfigEntity;
        }

        public async Task<bool> DoesPriceConfigExists(EnumCountry? countryId, long? subCategoryID, int? packageID)
        {
            return await _Context.PostPriceConfigurations.AnyAsync(a =>
                                                    a.ConfigurationCountry.HasValue &&
                                                    a.ConfigurationCountry == countryId &&
                                                    a.SubCategoryID.HasValue &&
                                                    a.SubCategoryID == subCategoryID &&
                                                    a.PackageConfigID.HasValue &&
                                                    a.PackageConfigID == packageID &&
                                                    a.IsActive);
        }

        public async Task<bool> AddPriceConfig(PriceConfig objPriceConfig, long currentUserID)
        {
            if (objPriceConfig == null)
                return false;
            objPriceConfig.CreatedBy = currentUserID;
            _Context.PostPriceConfigurations.Add(objPriceConfig);
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePriceConfig(int id, 
            int? noOfPost, 
            double? price, 
            int? discount, 
            long currentUserID,
            EnumCountry country)
        {
            var objConfig = await _Context.PostPriceConfigurations.FirstOrDefaultAsync(a => a.PostPriceConfigID == id);
            objConfig.OfferFreePost = noOfPost;
            objConfig.OfferPrice = price;
            objConfig.OfferDiscount = discount;
            objConfig.UpdateModifiedDate(currentUserID, country);
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePriceConfig(int id, long currentUserID, EnumCountry country)
        {
            var objItem = _Context.PostPriceConfigurations.FirstOrDefault(a => a.PostPriceConfigID == id);
            objItem.IsActive = false;
            objItem.UpdateModifiedDate(currentUserID, country);
            await _Context.SaveChangesAsync();
            return true;
        }
    }
}
