using Model;
using Common;

using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebDeshiHutBazar
{
    public interface IPostVisitService
    {
        Task<bool> SavePostVisit(long postID, string email, string phone, EnumPostVisitAction visitAction);

        Task<List<LogPostVisitViewModel>> GetUserAllPostVisits(long userID, EnumPostVisitAction visitAction);
    }
}
