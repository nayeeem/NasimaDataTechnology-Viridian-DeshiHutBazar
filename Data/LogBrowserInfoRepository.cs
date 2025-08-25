using System.Linq;
using System.Collections.Generic;
using Model;
using Common;
using System.Threading.Tasks;

namespace Data
{
    public class LogBrowserInfoRepository : WebBusinessEntityContext, ILogBrowserInfoRepository
    {
        private string SessionID { get; set; }

        public LogBrowserInfoRepository()
        {
        }
       
        public async Task<bool> SaveChanges()
        {
            await _Context.SaveChangesAsync();
            return true;
        }
       
        public async Task<long>  LogBrowserInfo(LogBrowserInfo objBrowserLog)
        {
            if (objBrowserLog == null)
                return 0;
            _Context.LogOfBrowsers.Add(objBrowserLog);
            await _Context.SaveChangesAsync();
            return objBrowserLog.BrowserLogID;
        }
    }
}
