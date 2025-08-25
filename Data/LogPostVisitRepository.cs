using System.Linq;
using System.Collections.Generic;
using Model;
using Common;
using System;
using System.Threading.Tasks;
using System.Data.Entity;
using Data;

namespace Data
{
    public class LogPostVisitRepository : WebBusinessEntityContext, ILogPostVisitRepository
    {
        private string SessionID { get; set; }

        public LogPostVisitRepository()
        {
        }
       
        public async Task<bool> SaveChanges()
        {
            var result = await _Context.SaveChangesAsync();
            return true;
        }
       
        public async Task<bool> SavePostVisit(LogPostVisit logPostVisit)
        {
            if (logPostVisit == null)
                return false;

            _Context.PostVisits.Add(logPostVisit);
            var result = await SaveChanges();
            return true;
        }

        public async Task<List<LogPostVisit>> GetAdvertiserVisitedProducts (long advertiserUserID, EnumPostVisitAction visitAction)
        {
            var listVisits = await _Context.PostVisits.Where(a => a.AdvertiserUserID == advertiserUserID && a.PostVisitAction == visitAction).ToListAsync();
            return listVisits.OrderByDescending(a => a.LogDateTime).ToList();
        }

        public async Task<LogPostVisit> GetSinglePostVisit(long postVisitID)
        {
            var postVisitEntity = await _Context.PostVisits.FirstOrDefaultAsync(a => a.PostVisitLogID == postVisitID);
            return postVisitEntity;
        }
    }
}
