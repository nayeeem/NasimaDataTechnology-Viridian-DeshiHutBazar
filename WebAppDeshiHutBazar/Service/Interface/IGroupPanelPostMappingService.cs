using Model;
using Common;


namespace WebDeshiHutBazar
{
    public interface IGroupPanelPostMappingService
    {
        PostViewModel LoadAValues(PostViewModel postViewModel);

        void MapGroupPanelPostEntityToPostViewModelForEdit(GroupPanelPost groupPanelPost, PostViewModel postVm);
        
        void MapGroupPanelPostEntityToPostViewModelReadonly(GroupPanelPost groupPanelPost, PostViewModel postVM);
    }
}
