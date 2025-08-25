using System.Linq;
using System.Collections.Generic;
using Model;
using Common;
using System.Threading.Tasks;
using System;
using System.Dynamic;

namespace Data
{
    public class TempTuple
    {
        public TempTuple()
        {
        }

        public long SubCategoryID { get; set; }
        public string Count { get; set; }
        public long CategoryID { get; set; }
    }

    public class LogPostRepository : WebBusinessEntityContext, ILogPostRepository
    {
        private string SessionID { get; set; }

        public LogPostRepository()
        {
        }

        public async Task<bool> SaveChanges()
        {
            var result = await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LogPostDetailPageVisit(EnumLogType fabiaLogType, long postId, EnumCountry country, string sessionID)
        {
            var post = _Context.Posts.FirstOrDefault(a => a.PostID == postId);
            if (post == null)
                return false;

            _Context.LogOfPosts.Add(new LogPostAction(post, fabiaLogType, country) { SessionNumber = sessionID });
            var result = await SaveChanges();
            return true;
        }

        public async Task<bool> LogPostDetailPageVisit(long postId, EnumCountry country, string sessionID)
        {
            var post = _Context.Posts.FirstOrDefault(a => a.PostID == postId);
            if (post == null)
                return false;

            _Context.LogOfPosts.Add(new LogPostAction(post, EnumLogType.PostDetailLink, country) { SessionNumber = sessionID });
            var result = await SaveChanges();
            return true;
        }

        public async Task<bool> LogEntirePageVisit(EnumLogType logType, EnumCountry country, string sessionID)
        {
            if (logType == EnumLogType.AllItemMarketLink)
                _Context.LogOfPosts.Add(new LogPostAction(EnumLogType.AllItemMarketLink, country) { SessionNumber = sessionID });           
            else if (logType == EnumLogType.HomePageLink)
                _Context.LogOfPosts.Add(new LogPostAction(EnumLogType.HomePageLink, country) { SessionNumber = sessionID });
            await SaveChanges();
            return true;
        }

        public async Task<bool> LogSearchMarketPageVisit(string searchKey, EnumCountry country, string sessionID)
        {
            _Context.LogOfPosts.Add(new LogPostAction(EnumLogType.SearchMarketLink, searchKey, country) { SessionNumber = sessionID });
            var result = await SaveChanges();
            return true;
        }

        public async Task<bool> LogSubCategoryLinkPageVisit(long subCatId, EnumCountry country, string sessionID)
        {
            _Context.LogOfPosts.Add(new LogPostAction(EnumLogType.SubCategoryMarketLink, subCatId, 0, country) { SessionNumber = sessionID });
            var result = await SaveChanges();
            return true;
        }

        public async Task<bool> LogSpecialMarketLinkPageVisit(long subCatId, EnumCountry country, string sessionID)
        {
            _Context.LogOfPosts.Add(new LogPostAction(EnumLogType.SpecialMarketLink, subCatId, 0, country) { SessionNumber = sessionID });
            var result = await SaveChanges();
            return true;
        }

        public async Task<bool> LogCategoryLinkPageVisit(long subCatId, EnumCountry country, string sessionID)
        {
            _Context.LogOfPosts.Add(new LogPostAction(EnumLogType.CategoryMarketLink, 0, subCatId, country) { SessionNumber = sessionID });
            var result = await SaveChanges();
            return true;
        }

        public async Task<bool> LogAdvancedSearch(
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
            string sessionID)
        {
            _Context.LogOfPosts.Add(new LogPostAction(searchKey,
                catID,
                subCatID,
                stateID,
                areaDesc,
                priceLow,
                priceHigh,
                isNew, isUsed, isUrgent, isSell, isRent,
                EnumLogType.AdvancedSearchLink,
                EnumCountry.Bangladesh)
            {
                SessionNumber = sessionID
            });
            var result = await SaveChanges();
            return true;
        }

        public List<TempTuple> GetVisitedTopSubCategories(int howManyTake)
        {
            return _Context.LogOfPosts.Where(a=>a.CategoryID.HasValue).GroupBy(info => info.CategoryID)
                .Select(group => new TempTuple
                {
                    SubCategoryID = group.Key.Value,
                    CategoryID = group.Key.Value,
                    Count = group.Count().ToString()
                }).ToList();
        }

        public List<LogPostAction> GetFullLogRecordsDateRanged(DateTime startDate, DateTime endDate)
        {
            return _Context.LogOfPosts.Where(a =>
                                    a.CreatedDate >= startDate &&
                                    a.CreatedDate <= endDate)
                                     .OrderByDescending(a => a.CreatedDate).ToList();
        }
    }    
}
