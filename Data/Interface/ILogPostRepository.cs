using Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using System;

namespace Data
{
    public interface ILogPostRepository
    {
        Task<bool> LogPostDetailPageVisit(EnumLogType fabiaLogType, long postId, EnumCountry country, string sessionID);

        Task<bool> LogPostDetailPageVisit(long postId, EnumCountry country, string sessionID);

        Task<bool> LogEntirePageVisit(EnumLogType logType, EnumCountry country, string sessionID);

        Task<bool> LogSearchMarketPageVisit(string searchKey, EnumCountry country, string sessionID);

        Task<bool> LogSubCategoryLinkPageVisit(long subCatId, EnumCountry country, string sessionID);

        Task<bool> LogCategoryLinkPageVisit(long subCatId, EnumCountry country, string sessionID);

        Task<bool> LogSpecialMarketLinkPageVisit(long subCatId, EnumCountry country, string sessionID);

        Task<bool> SaveChanges();

        Task<bool> LogAdvancedSearch(
            long? catID,
            long? subCatID,
            string searchKey,
            long? stateID,
            string areaDesc,
            long? priceLow,
            long? priceHigh,
            bool? isNew,
            bool? isUsed,
            bool? isUrgent,
            bool? isSell,
            bool? isRent,
            EnumCountry country,
            string sessionID);

        List<TempTuple> GetVisitedTopSubCategories(int howManyTake);

        List<LogPostAction> GetFullLogRecordsDateRanged(DateTime startDate, DateTime endDate);
    }
}
