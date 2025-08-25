using System.Linq;
using System.Collections.Generic;
using Model;
using Common;
using System;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data
{
    public class FabiaProviderRepository : WebBusinessEntityContext, IFabiaProviderRepository
    {
        public async Task<bool> SaveChanges()
        {
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProvider(long providerId)
        {
            try
            {
                var singleProvider = _Context.Providers.FirstOrDefault(a => a.ProviderID == providerId);
                if (singleProvider != null)
                {
                    singleProvider.IsActive = false;
                    var result = await _Context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<List<Provider>> GetAllProvider(long userId)
        {
            try
            {
                var objListProviders = await _Context.Providers.Where(a => a.UserID == userId && a.IsActive).ToListAsync();
                if (objListProviders != null)
                {
                    return objListProviders;
                }
                return new List<Provider>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Provider>> GetAllProviderByServiceID(long serviceID)
        {
            try
            {
                var objListProviders = await _Context.Providers.Where(a => a.FabiaServiceID == serviceID && a.IsActive).ToListAsync();
                if (objListProviders != null)
                {
                    return objListProviders;
                }
                return new List<Provider>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Provider> GetProviderByID(long? providerId)
        {
            try
            {
                var singleProvider = await _Context.Providers.FirstOrDefaultAsync(a => a.ProviderID == providerId);
                if (singleProvider != null)
                {
                    return singleProvider;
                }
                return new Provider();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveProvider(Provider providerObject)
        {
            if (providerObject != null)
            {
                _Context.Providers.Add(providerObject);
                var result = await _Context.SaveChangesAsync();
            }
            return true;
        }
    }
}
