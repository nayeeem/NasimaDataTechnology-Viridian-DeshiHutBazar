using Model;
using Common;

using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Data;

namespace WebDeshiHutBazar
{
    public interface ILoggingService
    {
        Task<List<LogServerError>> GetAllServerErrorLog();

        Task<bool> AddServerErrorLog(Exception exception);

        Task<bool> LogPostDetailPageVisit(EnumLogType fabiaLog, long postID, EnumCountry country, string sessionID);

        Task<bool> LogPostDetailPageVisit(long postID, EnumCountry country, string sessionID);

        Task<bool> LogEntirePageVisit(EnumLogType logType, EnumCountry country, string sessionID);

        Task<bool> LogSearchMarketPageVisit(string searchKey, EnumCountry country, string sessionID);

        Task<bool> LogSubCategoryMarketPageVisit(long subCatID, EnumCountry country, string sessionID);

        Task<bool> LogCategoryMarketPageVisit(long catID, EnumCountry country, string sessionID);

        Task<bool> LogSpecialMarketPageVisit(long catID, EnumCountry country, string sessionID);

        Task<long> LogUserSession(LogUserSession objUserSession);

        Task<long> LogBrowserInfo(LogBrowserInfo objBrowserLog);

        Task<bool> LogAdvancedSearch(PostViewModel objSearch, string sessionID);

        Task<bool> LogSimpleSearch(string searchKey, string sessionID);

        List<TempTuple> GetVisitedTopSubCategories(int howManyTake);
    }
}
