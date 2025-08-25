using System.Linq;
using System.Collections.Generic;
using Model;
using Common;
using System;
using System.Threading.Tasks;
using System.Data.Entity;
using Data;

namespace Data
{
    public class PostQueryRepository : WebBusinessEntityContext, IPostQueryRepository
    {
        public async Task<List<Post>> GetAllAdminPosts(EnumCountry country)
        {
            return await _Context.Posts.Where(
                                    a => a.IsActive && 
                                         a.EnumCountry == country &&
                                         a.PostType != EnumPostType.Post)
                                          .OrderBy(a => a.PostType)
                                          .ToListAsync();
        }

        public async Task<List<Post>> GetAllPosts(EnumCountry country, EnumPostType postType)
        {
            if (postType == EnumPostType.Post || postType == EnumPostType.Product)
            {
                return await _Context.Posts.Where(
                                    a =>
                                    a.IsActive &&
                                    a.EnumCountry == country &&
                                    a.PublishDate.HasValue &&
                                    a.PostType == EnumPostType.Post ||
                                    a.PostType == EnumPostType.Product)
                                        .OrderByDescending(a => a.PublishDate)
                                        .ToListAsync();
            }
            else if (postType == EnumPostType.AdSpace) {
                return await _Context.Posts.Where(
                                    a =>
                                    a.IsActive &&
                                    a.EnumCountry == country &&
                                    a.PostType == postType)
                                        .OrderBy(a => a.Title)
                                        .ToListAsync();
            }
            else if (postType == EnumPostType.ShortNote)
            {
                return await _Context.Posts.Where(
                                    a =>
                                    a.IsActive &&
                                    a.EnumCountry == country &&
                                    a.PostType == postType)
                                        .OrderByDescending(a => a.CreatedDate)
                                        .ToListAsync();
            }
            else if (postType == EnumPostType.ShortVideo)
            {
                return await _Context.Posts.Where(
                                    a =>
                                    a.IsActive &&
                                    a.EnumCountry == country &&
                                    a.PostType == postType)
                                        .OrderByDescending(a => a.CreatedDate)
                                        .ToListAsync();
            }
            else if (postType == EnumPostType.FabiaService)
            {
                return await _Context.Posts.Where(
                                    a =>
                                    a.IsActive &&
                                    a.EnumCountry == country &&
                                    a.PostType == postType)
                                        .OrderByDescending(a => a.CreatedDate)
                                        .ToListAsync();
            }
            else
            {
                // Return all posts
                return await _Context.Posts.Where(
                                    a =>
                                    a.IsActive &&
                                    a.EnumCountry == country)
                                        .OrderByDescending(a => a.CreatedDate)
                                        .ToListAsync();
            }            
        }

        public async Task<List<Post>> GetAllPostByUserID(long userId)
        {
            return await _Context.Posts.Where(a => a.UserID == userId &&
                                                    a.IsActive &&
                                                    !a.User.IsUserBlocked &&
                                                    a.PostType == EnumPostType.Post
                                                    ).OrderByDescending(a => a.CreatedDate)
                                                     .ToListAsync();
        }
        
        public async Task<int> GetCategoryPostsCount(EnumCountry country, long subcategoryid)
        {
            return await _Context.Posts.CountAsync(
                                a =>
                                a.IsActive &&
                                a.PostType == EnumPostType.Post &&
                                a.PublishDate.HasValue &&
                                a.EnumCountry == country &&
                                a.SubCategoryID == subcategoryid );
        }

        

        public async Task<List<PostComment>> GetAllCommentsByPostId(long postId)
        {
            var listComments = await _Context.PostComments.Where(a => a.PostID == postId && a.IsActive).ToListAsync();
            return listComments;
        }
        
        public async Task<List<Post>> GetAllUserFreeCurrentMonthPosts(long userId, EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            DateTime dayFrom = FirstDayOfMonthFromDateTime(BaTime.Date);
            DateTime dayTo = LastDayOfMonthFromDateTime(BaTime.Date);
            var postList = await _Context.Posts.Where(a => a.UserID == userId &&
                                                        a.PublishDate.HasValue &&
                                                        a.PublishDate >= dayFrom &&
                                                        a.PublishDate <= dayTo &&
                                                        a.PostStatus == EnumPostStatus.FreePosted &&
                                                        a.EnumCountry == country).ToListAsync();
            return postList;
        }

        public async Task<List<Post>> GetUserStartupFreeCurrentMonthPosts(long userId, EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            DateTime dayFrom = FirstDayOfMonthFromDateTime(BaTime.Date);
            DateTime dayTo = LastDayOfMonthFromDateTime(BaTime.Date);
            var postList = await _Context.Posts.Where(a =>
                                a.UserID == userId &&
                                a.PublishDate.HasValue &&
                                a.PublishDate >= dayFrom &&
                                a.PublishDate <= dayTo &&
                                a.PostType == EnumPostType.Post &&
                                a.PostStatus == EnumPostStatus.FreePosted &&
                                !a.UserPackageID.HasValue &&
                                a.EnumCountry == country).ToListAsync();
            return postList;
        }

        public async Task<List<Post>> GetUserStartupCateFreeCurrentMonthPosts(long userId, long subCateId, EnumCountry country)
        {
            DateTime dayFrom = FirstDayOfMonthFromDateTime(DateTime.Today);
            DateTime dayTo = LastDayOfMonthFromDateTime(DateTime.Today);
            var postList = await _Context.Posts.Where(a => a.UserID == userId &&
                                                            a.SubCategoryID == subCateId &&
                                                            a.PublishDate.HasValue &&
                                                            a.PublishDate >= dayFrom &&
                                                            a.PublishDate <= dayTo &&
                                                            a.PostStatus == EnumPostStatus.FreePosted &&
                                                            a.UserPackageID == null &&
                                                            a.EnumCountry == country).ToListAsync();
            return postList;
        }

        public async Task<int> GetUserStartupFreePostCount(long userId, EnumCountry country)
        {
            var list = await GetUserStartupFreeCurrentMonthPosts(userId, country);
            return list.Count;
        }

        public async Task<int> GetUserStartupCateFreePostCount(long userId, long subCateId, EnumCountry country)
        {
            var list = await GetUserStartupCateFreeCurrentMonthPosts(userId, subCateId, country);
            return list.Count;
        }

        public async Task<int> GetUserFreePostCount(long userId, EnumCountry country)
        {
            var list = await GetAllUserFreeCurrentMonthPosts(userId, country);
            return list.Count;
        }

        public async Task<List<Post>> GetAllUserFreeCurrentMonthPosts(long userId, long subCatId, EnumCountry country)
        {
            DateTime dayFrom = FirstDayOfMonthFromDateTime(DateTime.Today);
            DateTime dayTo = LastDayOfMonthFromDateTime(DateTime.Today);
            var postList = await _Context.Posts.Where(a => a.UserID == userId
                                            && a.PublishDate.HasValue
                                            && a.PublishDate >= dayFrom
                                            && a.PublishDate <= dayTo
                                            && a.PostStatus == EnumPostStatus.FreePosted
                                            && a.SubCategoryID == subCatId
                                            && a.EnumCountry == country).ToListAsync();
            return postList;
        }

        public async Task<int> GetUserFreeCategoryPostCount(long userId, long subCatId, EnumCountry country)
        {
            var list = await GetAllUserFreeCurrentMonthPosts(userId, subCatId, country);
            return list.Count;
        }

        public async Task<List<Post>> GetAllUserPayablePosts(long userId, EnumCountry country)
        {
            var postList = await _Context.Posts.Where(a => a.UserID == userId
                                            && a.PostStatus == EnumPostStatus.Payable
                                            && a.IsActive
                                            && !a.PublishDate.HasValue
                                            && a.EnumCountry == country).ToListAsync();
            return postList.OrderByDescending(a => a.CreatedDate).ToList();
        }

        public async Task<List<Post>> GetAllUserPublishedPosts(long userId, EnumCountry country)
        {
            var postList = await _Context.Posts.Where(a => a.UserID == (long)userId &&
                            a.PostStatus != EnumPostStatus.Payable &&
                            a.PostType == EnumPostType.Post &&
                            a.PublishDate.HasValue &&
                            a.EnumCountry == country
                            ).ToListAsync();
            return postList.OrderByDescending(a => a.PublishDate)
                            .ThenBy(a => a.CategoryID)
                            .ThenBy(a => a.SubCategoryID)
                            .ToList();
        }

        public async Task<List<Post>> GetAllUserUnpaidPosts(long userId, EnumCountry country)
        {
            var postList = await _Context.Posts.Where(a => a.UserID == userId
                                            && a.PublishDate == null
                                            && a.PostStatus == EnumPostStatus.Payable
                                            && a.IsActive
                                            && a.PostType == EnumPostType.Post
                                            && a.EnumCountry == country).ToListAsync();
            return postList.OrderByDescending(a => a.CreatedDate).ToList(); ;
        }

        public async Task<List<Post>> GetAllPostBySubCategoryID(long subCategoryID, EnumCountry country)
        {
            var listPosts = await _Context.Posts.Where(p =>
                                       p.SubCategoryID == subCategoryID &&
                                       p.IsActive &&
                                       p.PublishDate.HasValue &&
                                       p.EnumCountry == country).ToListAsync();
            return listPosts.ToList().OrderByDescending(a => a.PublishDate).ToList();
        }

        public async Task<List<Post>> GetAllPostBySubCategoryID(long? subCategoryID, EnumDeviceType device, EnumCountry country)
        {
            List<long?> listIds = new List<long?>();
            List<GroupPanelPost> listPostsAvoid = await _Context.GroupPanelPosts.Where(a => a.IsActive &&
                                    a.Post.SubCategoryID == subCategoryID &&
                                    a.Post.IsActive &&
                                    a.GroupPanelConfig.Device == device &&
                                    a.EnumCountry == country
                                    ).ToListAsync();

            foreach (var item in listPostsAvoid.ToList())
            {
                listIds.Add(item.PostID);
            }

            var listPosts = await _Context.Posts.Where(p =>
                                       p.SubCategoryID == subCategoryID &&
                                       p.IsActive &&
                                       p.PublishDate.HasValue &&
                                       p.EnumCountry == country).ToListAsync();
            return listPosts.ToList().OrderByDescending(a => a.PublishDate).ToList();
        }

        private DateTime FirstDayOfMonthFromDateTime(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        private DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        public async Task<List<Post>> GetAllProductPostByUserID(long userID, EnumCountry country)
        {
            return await _Context.Posts.Where(
                a => a.UserID == userID &&
                a.EnumCountry == country &&
                a.PostType == EnumPostType.Product &&
                a.IsActive).OrderByDescending(a => a.CreatedDate).ThenBy(a => a.Title).ToListAsync();
        }
    }
}
