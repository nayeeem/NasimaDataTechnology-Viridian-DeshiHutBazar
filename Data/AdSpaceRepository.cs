using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebBusinessPlatform.Model;

namespace WebBusinessPlatform.Data
{
    public class AdSpaceRepository : WebBusinessEntityContext, IAdSpaceRepository
    {
        public async Task<bool> AddAdSpace(AdsSpaceConfig adSpace)
        {
            _Context.AdSpaces.Add(adSpace);
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAdsSpace(AdsSpaceConfig adSpace)
        {
            var objAdSpace = await _Context.AdSpaces.FirstOrDefaultAsync(a => a.AdSpaceID == adSpace.AdSpaceID);
            objAdSpace = adSpace;
            var result = await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAdsSpace(AdsSpaceConfig adSpace)
        {
            var objAdSpace = await _Context.AdSpaces.FirstOrDefaultAsync(a => a.AdSpaceID == adSpace.AdSpaceID);
            objAdSpace.IsActive = false;
            var result = await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AdsSpaceConfig>> GetAllAdSpace()
        {
            return await _Context.AdSpaces.Where(a => a.IsActive).ToListAsync();
        }

        public async Task<AdsSpaceConfig> GetSingleAdSpace(long id)
        {
            return await _Context.AdSpaces.FirstOrDefaultAsync(a => a.IsActive && a.AdSpaceID == id);
        }

    }
}
