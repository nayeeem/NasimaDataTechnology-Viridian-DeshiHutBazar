using System.Collections.Generic;
using Model;
using Data;
using Common;
using System.Threading.Tasks;
using System;
using System.Linq;

using System.Web.Mvc;

namespace WebDeshiHutBazar
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogServerErrorRepository _LogServerErrorRepo;
        private readonly ILogPostRepository _LogPostRepo;
        private readonly ILogBrowserInfoRepository _LogBrowserInfoRepo;
        private readonly ILogUserSessionRepository _LogUserSessionRepo;

        public LoggingService(
            ILogServerErrorRepository logServerErrorRepo,
            ILogPostRepository logPostRepo,
            ILogBrowserInfoRepository logBrowserInfoRepo,
            ILogUserSessionRepository logUserSessionRepo)
        {
            _LogServerErrorRepo = logServerErrorRepo;
            _LogPostRepo = logPostRepo;
            _LogBrowserInfoRepo = logBrowserInfoRepo;
            _LogUserSessionRepo = logUserSessionRepo;
        }

        public LoggingService()
        {
            _LogServerErrorRepo = new LogServerErrorRepository();
            _LogPostRepo = new LogPostRepository();
            _LogBrowserInfoRepo = new LogBrowserInfoRepository();
            _LogUserSessionRepo = new LogUserSessionRepository();
        }

        public async Task<bool> AddServerErrorLog(Exception exception)
        {
            var result = await _LogServerErrorRepo.AddServerErrorLog(exception);
            return result;
        }

        public async Task<List<LogServerError>> GetAllServerErrorLog()
        {
            var listResult =  await _LogServerErrorRepo.GetAllServerErrorLog();
            return listResult;
        }

        public async Task<bool> LogPostDetailPageVisit(long postID, EnumCountry country, string sessionID)
        {
            var result = await _LogPostRepo.LogPostDetailPageVisit(postID, country, sessionID);            
            return result;
        }

        public async Task<bool> LogPostDetailPageVisit(EnumLogType fabiaLog, long postID, EnumCountry country, string sessionID)
        {
            var result = await _LogPostRepo.LogPostDetailPageVisit(postID, country, sessionID);
            return result;
        }

        public async Task<bool> LogEntirePageVisit(EnumLogType logType, EnumCountry country, string sessionID)
        {
            var result = await _LogPostRepo.LogEntirePageVisit(logType, country, sessionID);
            return result;
        }

        public async Task<bool> LogSearchMarketPageVisit(string searchKey, EnumCountry country, string sessionID)
        {
            var result = await _LogPostRepo.LogSearchMarketPageVisit(searchKey, country, sessionID);
            return result;            
        }

        public async Task<bool> LogSubCategoryMarketPageVisit(long subCatID, EnumCountry country, string sessionID)
        {
            var result = await _LogPostRepo.LogSubCategoryLinkPageVisit(subCatID, country, sessionID);
            return result;          
        }

        public async Task<bool> LogCategoryMarketPageVisit(long catID, EnumCountry country, string sessionID)
        {
            var result = await _LogPostRepo.LogCategoryLinkPageVisit(catID, country, sessionID);
            return result;
        }

        public async Task<bool> LogSpecialMarketPageVisit(long catID, EnumCountry country, string sessionID)
        {
            var result = await _LogPostRepo.LogSpecialMarketLinkPageVisit(catID, country, sessionID);
            return result;
        }

        public async Task<long> LogUserSession(LogUserSession objUserSession)
        {
            var result = await _LogUserSessionRepo.LogUserSession(objUserSession);
            return result;
        }

        public async Task<long> LogBrowserInfo(LogBrowserInfo objBrowserLog)
        {
            var result = await _LogBrowserInfoRepo.LogBrowserInfo(objBrowserLog);
            return result;
        }

        public async Task<bool> LogAdvancedSearch(PostViewModel objSearch, string sessionID)
        {
            var result = await _LogPostRepo.LogAdvancedSearch(objSearch.CategoryID,
                objSearch.SubCategoryID,
                objSearch.SearchKey,
                objSearch.StateID,
                objSearch.AreaDescription,
                objSearch.PriceLow,
                objSearch.PriceHigh,
                objSearch.IsBrandNew,
                objSearch.IsUsed,
                objSearch.IsUrgent,
                objSearch.IsForSell,
                objSearch.IsForRent,
                EnumCountry.Bangladesh,
                sessionID);
            return result;
        }

        public async Task<bool> LogSimpleSearch(string searchKey, string sessionID)
        {
            var result = await _LogPostRepo.LogAdvancedSearch(null,
                null,
                searchKey,
                null,
                null,
                null,
                null, null, null, null, null, null,
                EnumCountry.Bangladesh,
                sessionID);
            return result;
        }

        public List<TempTuple> GetVisitedTopSubCategories(int howManyTake)
        {
            return _LogPostRepo.GetVisitedTopSubCategories(5);
        }
    }
}
