using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Data
{
    public interface IFabiaProviderRepository
    {
        Task<bool> SaveProvider(Provider providerObject);

        Task<List<Provider>> GetAllProviderByServiceID(long serviceID);

        Task<Provider> GetProviderByID(long? providerId);

        Task<List<Provider>> GetAllProvider(long userId);

        Task<bool> DeleteProvider(long providerId);

        Task<bool> SaveChanges();
    }
}
