using System.Threading.Tasks;
using Model;

namespace Data
{
    public interface ILogBrowserInfoRepository
    {
        Task<long> LogBrowserInfo(LogBrowserInfo objBrowserLog);
    }
}
