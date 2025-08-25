using System.Linq;
using System.Collections.Generic;
using Model;
using Common;
using System;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data
{
    public class PackageConfigRepository : WebBusinessEntityContext, IPackageConfigRepository
    {
        public async Task<List<PackageConfig>> GetAllActivePackages()
        {
            var listPackages = await _Context.PostPackageConfigurations.Where(a =>
                                                                        a.PackageStatus == Common.EnumPackageStatus.Enabled &&
                                                                        a.IsActive).ToListAsync();
            return listPackages;
        }
        
        public async Task<PackageConfig> GetSinglePackage(int packageId)
        {
            var objectEntity = await _Context.PostPackageConfigurations.FirstOrDefaultAsync(a => 
                                                                                a.PackageConfigID == packageId && 
                                                                                a.IsActive && 
                                                                                a.PackageStatus == Common.EnumPackageStatus.Enabled);
            return objectEntity;
        }

        public async Task<PackageConfig> GetDefaultPackage()
        {
            return await _Context.PostPackageConfigurations.FirstOrDefaultAsync(a => 
                                                                    a.PackageType == Common.EnumPackageType.StartUpPackage &&
                                                                    a.PackageStatus == Common.EnumPackageStatus.Enabled &&
                                                                    a.IsActive);
        }
    }
}
