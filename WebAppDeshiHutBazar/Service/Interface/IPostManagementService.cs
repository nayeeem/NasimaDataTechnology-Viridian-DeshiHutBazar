using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;
using Model;

namespace WebDeshiHutBazar
{
    public interface IPostMangementService
    {
        Task<PostDisplayViewModel> GetDisplayPostByID(long postID, EnumCurrency currency);

        Task<bool> DeletePostImage(long imageId);

        Task<List<PostViewModel>> GetMarketAllPosts(EnumCountry country, string detailUrl, EnumCurrency currency);

        PostViewModel CreateNewPost(EnumCurrency currency);

        Task<long> SaveProductPost(PostViewModel objPostVM, EnumCountry country);

        Task<long> SavePost(PostViewModel objPostVm, EnumCountry country, PackageConfig package, EnumCurrency currency);

        Task<long> SaveShortContentPost(PostViewModel objPostVm, EnumCountry country, 
            EnumPostType postType, UserModel user);

        Task<long> SavePostForExistingUser(PostViewModel objPostVm, EnumCountry country);

        Task<bool> UpdatePost(PostViewModel objPostVm, long postId, EnumCountry country);

        Task<bool> DeletePost(long postId, long currentUserID, EnumCountry country);

        Task<bool> LikeThisPost(long postId, string actionType);

        Task<List<PostViewModel>> GetAllProductsByUserID(long userID, EnumCountry country, EnumCurrency currency);

        Task<List<PostViewModel>> GetAllPublishedPostsByUserID(long userID, EnumCountry country, EnumCurrency currency);

        Task<List<PostViewModel>> GetAllUnpaidPostsByUserID(long userID, EnumCountry country, EnumCurrency currency);

        Task<List<PostViewModel>> GetAllPostsBySubCategoryID(long subCategoryID, EnumDeviceType device, 
            EnumCountry country, EnumCurrency currency);

        Task<List<PostViewModel>> GetAllPosts(EnumCountry country, EnumPostType postType, string detailUrl, EnumCurrency currency);

        Task<List<PostViewModel>> GetAllModelPosts(EnumCountry country, EnumPostType postType, EnumCurrency currency);

        Task<int> GetCategoryPostsCount(EnumCountry country, long subcategoryid);

        Task<bool> UpdatePostImages(PostViewModel objPostVm, long postId, EnumCountry country);

        Task<PostViewModel> GetPostByPostIDReadonly(long postID, EnumCurrency currency);

        Task<PostViewModel> GetPostByPostIDForEdit(long postID, EnumCurrency currency);

        Task<bool> AddComments(string comment, int postID, EnumCountry country);

        Task<List<PostViewModel>> GetCategoryMarketAllPosts(EnumCountry country, string detailUrl, 
            long subCategoryId, EnumCurrency currency);

        Task<List<PostViewModel>> GetMarketAllPosts(EnumCountry country, string detailUrl, 
            int postValidDays, EnumCurrency currency);

        Task<bool> LikeThisComment(long commentId, string actionType);

        Task<List<PostViewModel>> GetAllAdminPosts(EnumCountry country, EnumCurrency currency);

        Task<bool> RemovePostService(long postId, long serviceId);

        Task<bool> RemovePostProcess(long postId, long processId);

        Task<bool> AddPostService(PostServiceViewModel postServiceViewModel);

        Task<bool> AddPostProcess(PostProcessViewModel postProcessViewModel);

        Task<bool> UpdatePostService(PostServiceViewModel postServiceViewModel);

        Task<bool> UpdatePostProcess(PostProcessViewModel postProcessViewModel);

        Task<bool> UpdatePostService(byte[] postServiceViewModel, long id);

        Task<bool> UpdatePostProcess(byte[] postProcessViewModel, long id);

        Task<List<PostViewModel>> GetCategoryPosts(EnumCountry country, string detailUrl, 
            long subCategoryId, EnumCurrency currency);

        Task<List<SelectListItem>> GetAllFabiaPosts(EnumCountry country, EnumPostType postType);
    }
}
