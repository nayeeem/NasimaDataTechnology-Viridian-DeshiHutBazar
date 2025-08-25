using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Model;

namespace Data
{
    public interface IGroupPanelPostRepository
    {
        Task<bool> SaveChanges();

        Task<int> GetNextOrderNumber(long subCateID);

        Task<bool> AddNewPanelPost(GroupPanelPost groupPanelPost, long currentUserID);

        Task<GroupPanelPost> GetSinglePanelPost(long postID);

        Task<List<GroupPanelPost>> GetAllGroupPosts();
    }
}
