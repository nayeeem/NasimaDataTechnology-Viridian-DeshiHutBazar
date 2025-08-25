using System.Collections.Generic;
using Model;
using System;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data
{
    public class LogServerErrorRepository : WebBusinessEntityContext, ILogServerErrorRepository
    {
        public LogServerErrorRepository(){
        }        
        
        public async Task<bool> AddServerErrorLog(Exception exception)
        {
            LogServerError objErrorLog = new LogServerError(exception, null, null);
            _Context.ServerErrorLogs.Add(objErrorLog);
            await _Context.SaveChangesAsync();
            return true;
        }
        
        public async Task<List<LogServerError>> GetAllServerErrorLog()
        {
            var listResult= await _Context.ServerErrorLogs.ToListAsync();
            return listResult;
        }
    }
}
