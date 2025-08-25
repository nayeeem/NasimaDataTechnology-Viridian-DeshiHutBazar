using System.Linq;
using System.Collections.Generic;
using Model;
using Common;
using System;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data
{
    public class GroupPanelPostRepository : WebBusinessEntityContext, IGroupPanelPostRepository
    {
        public GroupPanelPostRepository() { }

        public async Task<bool> SaveChanges()
        {
            var result = await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetNextOrderNumber(long groupConfigID)
        {
            var listPosts = await _Context.GroupPanelPosts.Where(a => a.IsActive && 
                                                                      a.GroupPanelConfigID.HasValue &&
                                                                      a.GroupPanelConfigID == groupConfigID).ToListAsync();
            if (listPosts == null)
                return 1;

            return listPosts.Count() + 1;
        }

        public async Task<GroupPanelPost> GetSinglePanelPost(long groupPostID)
        {
            var groupPostEntity = await _Context.GroupPanelPosts.Where(
                                                            a => a.IsActive && 
                                                            a.GroupPostID == groupPostID).FirstOrDefaultAsync();
            return groupPostEntity;
        }

        public async Task<bool> AddNewPanelPost(GroupPanelPost groupPanelPost, long currentUserID)
        {
            var count = _Context.GroupPanelPosts.Count(a =>
                                                        a.GroupPanelConfigID == groupPanelPost.GroupPanelConfigID &&
                                                        a.IsActive);
            groupPanelPost.CreatedBy = currentUserID;
            groupPanelPost.DisplayOrder = count + 1;
            _Context.GroupPanelPosts.Add(groupPanelPost);
            var result = await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<List<GroupPanelPost>> GetAllGroupPosts()
        {
            return await _Context.GroupPanelPosts.Where(a=>a.IsActive).ToListAsync();
        }
    }
}
