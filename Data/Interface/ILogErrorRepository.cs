using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Data
{
    public interface ILogServerErrorRepository
    {
        Task<bool> AddServerErrorLog(Exception exception);

        Task<List<LogServerError>> GetAllServerErrorLog();
    }
}