using System.Linq;
using System.Collections.Generic;
using Model;
using Common;
using System;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data
{
    public class PostRepository : WebBusinessEntityContext, IPostRepository, IPostCommentRepository
    {
        public async Task<bool> SaveChanges()
        {
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostImage(long imageId)
        {
            var image = await _Context.Files.FirstOrDefaultAsync(a => a.FileID == imageId);
            var res = _Context.Files.Remove(image);
            var sve = await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostProcess(long postProcessID)
        {
            if (postProcessID == 0)
                return false;
            var postProcess = await _Context.PostProcesses.FirstOrDefaultAsync(
                                                a =>
                                                a.PostProcessID == postProcessID
                                                && a.IsActive);
            _Context.PostProcesses.Remove(postProcess);
            var res = await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostService(long postServiceID)
        {
            if (postServiceID == 0)
                return false;
            var postService = await _Context.PostServices.FirstOrDefaultAsync(
                                                a =>
                                                a.PostServiceID == postServiceID
                                                && a.IsActive);
            _Context.PostServices.Remove(postService);
            var res = await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<PostProcess> GetPostProcessByID(long postProcessID)
        {
            if (postProcessID == 0)
                return null;
            var postProcess = await _Context.PostProcesses.FirstOrDefaultAsync(
                                                a =>
                                                a.PostProcessID == postProcessID
                                                && a.IsActive);
            return postProcess;
        }

        public async Task<PostService> GetPostServiceByID(long postServiceID)
        {
            if (postServiceID == 0)
                return null;
            var postService = await _Context.PostServices.FirstOrDefaultAsync(
                                                a =>
                                                a.PostServiceID == postServiceID                                                
                                                && a.IsActive);
            return postService;
        }

        public async Task<Post> GetPostByPostID(long postId)
        {            
            var post = await _Context.Posts.FirstOrDefaultAsync(a =>
                                                a.PostID == postId
                                                && a.IsActive);
            return post;
        }

        public async Task<long> SavePost(Post postObject)
        {
            _Context.Posts.Add(postObject);
            await _Context.SaveChangesAsync();
            return postObject.PostID;
        }

        public async Task<bool> DeletePost(long postId, long currentUserID, EnumCountry country)
        {
            var post = _Context.Posts.ToList().SingleOrDefault(a => a.PostID == postId);
            if (post != null)
            {
                post.UpdateModifiedDate(currentUserID, country);
                post.IsActive = false;
                await _Context.SaveChangesAsync();
            }
            return true;
        }
       
        public async Task<bool> LikeThisPost(long? postId, string actionType)
        {
            if (!postId.HasValue)
                return false;
            
            var post = await _Context.Posts.FirstOrDefaultAsync(a =>
                                                a.PostID == postId.Value
                                                && a.IsActive);
            if (post != null)
            {
                var like = post.LikeCount;
                if (like.HasValue)
                {
                    if (actionType == "Plus")
                    {
                        like = like + 1;
                    }                    
                    else if(actionType == "Minus")
                    {
                        like = like - 1;
                    }
                }
                else
                {
                    if (actionType == "Plus")
                    {
                        like = 1;
                    }
                    else if (actionType == "Minus")
                    {
                        like = 0;
                    }
                }
                post.LikeCount = like;                
                var res = await _Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> SaveComment(string comment, long postId, EnumCountry country)
        {
            var post = await _Context.Posts.FirstOrDefaultAsync(a => a.PostID == postId);
            if(post!=null)
            {
                PostComment objComment = new PostComment(comment, post, country);
                _Context.PostComments.Add(objComment);
                var result = await SaveChanges();
            }
            return true;
        }
        
        public async Task<bool> UpdatePostStatus(long postId, 
            EnumPostStatus postStatus, 
            int? userPackId, 
            long currentUserID,
            EnumCountry country)
        {
            var post = await _Context.Posts.FirstOrDefaultAsync(a => a.PostID == postId);
            post.PostStatus = postStatus;
            post.UserPackageID = userPackId;
            post.UpdateModifiedDate(currentUserID, country);
            if(
                postStatus != EnumPostStatus.Payable
                )
            {
                post.SetPublishDate();
            }
            var result = await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LikeThisComment(long commentId, string actionType)
        {
            var comment = await _Context.PostComments.FirstOrDefaultAsync(a =>
                                                a.CommentID == commentId
                                                && a.IsActive);
            if (comment != null)
            {
                var like = comment.Like;

                if (actionType == "Plus")
                {
                    like = like + 1;
                }
                else if (actionType == "Minus")
                {
                    like = like - 1;
                }
                comment.Like = like;
                var res = await _Context.SaveChangesAsync();
            }
            return true;
        }
    }
}
