using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Model;

namespace Data
{
    public interface IPostQueryRepository
    {
        Task<List<Post>> GetAllAdminPosts(EnumCountry country);

        Task<List<Post>> GetAllPostByUserID(long userId);

        Task<List<Post>> GetAllPosts(EnumCountry country, EnumPostType postType);

        Task<List<Post>> GetAllUserPublishedPosts(long userId, EnumCountry country);

        Task<List<Post>> GetAllUserUnpaidPosts(long userId, EnumCountry country);

        Task<List<Post>> GetAllUserFreeCurrentMonthPosts(long userId, long subCatId, EnumCountry country);

        Task<List<Post>> GetAllUserPayablePosts(long userId, EnumCountry country);       

        Task<int> GetUserFreePostCount(long userId, EnumCountry country);        

        Task<int> GetUserFreeCategoryPostCount(long userId, long subCatId, EnumCountry country);

        Task<List<Post>> GetUserStartupFreeCurrentMonthPosts(long userId, EnumCountry country);

        Task<int> GetUserStartupFreePostCount(long userId, EnumCountry country);

        Task<int> GetUserStartupCateFreePostCount(long userId, long subCateId, EnumCountry country);

        Task<List<Post>> GetUserStartupCateFreeCurrentMonthPosts(long userId, long subCateId, EnumCountry country);

        Task<List<Post>> GetAllPostBySubCategoryID(long? subCategoryID, EnumDeviceType device, EnumCountry country);

        Task<int> GetCategoryPostsCount(EnumCountry country, long subcategoryid);

        Task<List<PostComment>> GetAllCommentsByPostId(long postId);

        Task<List<Post>> GetAllPostBySubCategoryID(long subCategoryID, EnumCountry country);

        Task<List<Post>> GetAllProductPostByUserID(long userID, EnumCountry country);
    }
}
