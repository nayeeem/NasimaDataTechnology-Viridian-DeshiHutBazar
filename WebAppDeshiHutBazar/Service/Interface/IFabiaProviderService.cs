
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebDeshiHutBazar
{
    public interface IFabiaProviderService
    {
        Task<long> SaveProvider(FabiaProviderViewModel providerObject);

        Task<FabiaProviderViewModel> GetProviderByID(long? providerId);

        Task<List<FabiaProviderViewModel>> GetAllProviderByServiceBy(long fabiaServiceID);

        Task<List<FabiaProviderViewModel>> GetAllProvider(long userId);

        Task<bool> DeleteProvider(long providerId);

        Task<bool> UpdateProvider(FabiaProviderViewModel singleProviderViewModel);
    }
}
