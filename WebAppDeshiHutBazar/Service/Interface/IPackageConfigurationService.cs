
using Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;

namespace WebDeshiHutBazar
{
    public interface IPackageConfigurationService
    {
        Task<PackageConfig> GetDefaultStartupPackage();

        Task<List<PackageConfig>> GetAllActivePackages();

        Task<PackageConfig> GetSinglePackage(int packageId);
    }
}
