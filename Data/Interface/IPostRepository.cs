using System.Threading.Tasks;
using Common;
using Model;

namespace Data
{
    public interface IPostRepository
    {
        Task<long> SavePost(Post postObject);

        Task<bool> DeletePost(long postId, long currentUserID, EnumCountry country);

        Task<bool> DeletePostImage(long imageId);

        Task<bool> LikeThisPost(long? postId, string actionType);

        Task<bool> UpdatePostStatus(long postId, EnumPostStatus postStatus, int? userPackId, long currentUserID, EnumCountry country);
        
        Task<bool> SaveChanges();

        Task<Post> GetPostByPostID(long postId);

        Task<bool> LikeThisComment(long commentId, string actionType);

        Task<PostService> GetPostServiceByID(long postServiceID);

        Task<PostProcess> GetPostProcessByID(long postProcessID);

        Task<bool> DeletePostService(long postServiceID);

        Task<bool> DeletePostProcess(long postProcessID);
    }
}
