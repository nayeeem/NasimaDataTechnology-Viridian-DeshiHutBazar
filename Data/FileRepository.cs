using System.Linq;
using System.Collections.Generic;
using Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Data
{
    public class FileRepository : WebBusinessEntityContext, IFileRepository
    {
        public async Task<List<File>> GetFiles()
        {
            return await _Context.Files.Where(a => a.PostID != null).ToListAsync();
        }

        public async Task<bool> SaveFile(File f)
        {
            _Context.Entry<File>(f).State = EntityState.Modified;
            var c = await _Context.SaveChangesAsync();
            return true;
        }
    }
}
