using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Data
{
    public interface IFileRepository
    {
        Task<List<File>> GetFiles();

        Task<bool> SaveFile(File f);
    }
}