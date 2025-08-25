using System.Threading.Tasks;
using Model;

namespace Data
{
    public interface ILogUserSessionRepository
    {
        Task<long> LogUserSession(LogUserSession objUserSession);
    }
}
