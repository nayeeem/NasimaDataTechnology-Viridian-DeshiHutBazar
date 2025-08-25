using Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public interface IPackageConfigRepository
    {
        Task<List<PackageConfig>> GetAllActivePackages();

        Task<PackageConfig> GetSinglePackage(int packageId);

        Task<PackageConfig> GetDefaultPackage();
    }
}