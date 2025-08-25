using Model;
using Common;


namespace WebDeshiHutBazar
{
    public interface IPostMappingService
    {
        PostViewModel LoadAValues(PostViewModel postViewModel);

        void MapPostEntityToPostViewModelForPostTemplateDisplay(Post postEntity, PostViewModel postVM);

        void MapPostEntityToPostViewModelForEdit(Post post, PostViewModel postVm);
        
        void PostViewModelToPostEntityMappingForNewPostNewUser(PostViewModel objPostVm, Post objPostEntity,EnumCountry country);
        
        void PostViewModelToPostEntityMappingForNewPostExistingUser(PostViewModel objPostVm, Post objPostEntity, User objUser, EnumCountry country);

        void PostViewModelToPostEntityMappingForExistingPost(PostViewModel objPostVm, Post post, EnumCountry country);

        void MapExistingFilesViewModelToFilesEntity(PostViewModel objPostVM, Post postEntity, EnumCountry enumCountry);

        void MapPostEntityToPostViewModelReadonly(Post postEntity, PostViewModel postVM);

        void MapPostEntityToPostViewModelSelectGroupConfig(Post postEntity, PostViewModel postViewModel);

        void MapPostEntityToPostViewModelForPostLimitedDisplay(Post postEntity, PostViewModel postVM);
    }
}
