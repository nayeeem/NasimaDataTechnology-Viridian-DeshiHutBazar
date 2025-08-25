using System.Linq;
using System.Collections.Generic;
using Model;
using Common;
using System.Threading.Tasks;

namespace Data
{
    public class LogUserSessionRepository : WebBusinessEntityContext, ILogUserSessionRepository
    {        
        public LogUserSessionRepository()
        {
        }       
       
        public async Task<long> LogUserSession(LogUserSession objUserSession)
        {
            if (objUserSession == null)
                return 0;
            _Context.UserSessions.Add(objUserSession);
            await _Context.SaveChangesAsync();
            return objUserSession.UserSessionId;
        }
    }
}
