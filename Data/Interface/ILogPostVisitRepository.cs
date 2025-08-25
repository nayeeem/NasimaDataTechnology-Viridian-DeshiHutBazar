using Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;

namespace Data
{
    public interface ILogPostVisitRepository
    {
        Task<bool> SaveChanges();

        Task<bool> SavePostVisit(LogPostVisit logPostVisit);

        Task<List<LogPostVisit>> GetAdvertiserVisitedProducts(long advertiserUserID, EnumPostVisitAction visitAction);

        Task<LogPostVisit> GetSinglePostVisit(long postVisitID);

    }
}
