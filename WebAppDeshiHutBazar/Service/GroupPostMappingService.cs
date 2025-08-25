using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Data;
using Common;

namespace WebDeshiHutBazar
{
    public class GroupPostMappingService : IGroupPanelPostMappingService
    {
        private readonly IAValueRepository _AValueRepo;
        private readonly HashingCryptographyService _HashingService;

        public GroupPostMappingService(
            IAValueRepository aValueRepo
            )
        {
            _AValueRepo = aValueRepo;
            _HashingService = new HashingCryptographyService();
        }

        public PostViewModel LoadAValues(PostViewModel postViewModel)
        {
            postViewModel.AV_State = DropDownDataList.GetAllStateList();
            postViewModel.AV_Category = DropDownDataList.GetCategoryList();
            postViewModel.AV_SubCategory = DropDownDataList.GetSubCategoryList();
            return postViewModel;
        }

        public void MapGroupPanelPostEntityToPostViewModelForEdit(GroupPanelPost groupPostEntity, PostViewModel postViewModel)
        {
            postViewModel.PostID = groupPostEntity.Post.PostID;
            postViewModel.CreatedDate = groupPostEntity.Post.CreatedDate;
            postViewModel.Title = groupPostEntity.Post.Title;
            postViewModel.CategoryID = groupPostEntity.Post.CategoryID;
            postViewModel.SubCategoryID = groupPostEntity.Post.SubCategoryID;
            postViewModel.DisplayCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId(groupPostEntity.Post.CategoryID);
            postViewModel.DisplaySubCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId(groupPostEntity.Post.SubCategoryID);
            postViewModel.Description = groupPostEntity.Post.Description;
            postViewModel.IsBrandNew = groupPostEntity.Post.IsBrandNew;
            postViewModel.IsUsed = groupPostEntity.Post.IsUsed;
            postViewModel.IsUrgent = groupPostEntity.Post.IsUrgent;
            postViewModel.IsForRent = groupPostEntity.Post.IsForRent;
            postViewModel.IsForSell = groupPostEntity.Post.IsForSell;
            postViewModel.PosterContactNumber = groupPostEntity.Post.PosterContactNumber;
            postViewModel.PosterName = groupPostEntity.Post.PosterName;
            postViewModel.Currency = groupPostEntity.Post.Currency;
            postViewModel.Price = groupPostEntity.Post.UnitPrice;
            postViewModel.FormattedPriceValue = postViewModel.GetFormatedPriceValue(postViewModel.Price.ToString());
            postViewModel.PublishDate = groupPostEntity.Post.PublishDate;
            postViewModel.WebsiteUrl = groupPostEntity.Post.WebsiteUrl;
            postViewModel.PostType = groupPostEntity.Post.PostType;
            postViewModel.SubCategoryCSS = BusinessObjectSeed.GetCategoryCSS(groupPostEntity.Post.SubCategoryID);
            postViewModel.CategoryCSS = BusinessObjectSeed.GetCategoryCSS(groupPostEntity.Post.CategoryID);
            postViewModel.LikeCount = groupPostEntity.Post.LikeCount.HasValue ? groupPostEntity.Post.LikeCount.Value : 0;
            postViewModel.CommentsCount = groupPostEntity.Post.ListComments != null && groupPostEntity.Post.ListComments.Count > 0 ? groupPostEntity.Post.ListComments.Count : 0;
            postViewModel.Comment = "";
            postViewModel.SearchTag = groupPostEntity.Post.SearchTag;
            MapImageFilesForDisplay(postViewModel, groupPostEntity.Post);
            MapUserReadonly(groupPostEntity.Post, postViewModel);
            MapAddressReadonly(groupPostEntity.Post, postViewModel);
            postViewModel.ListPostComments = GetPostCommentsReadonly(groupPostEntity.Post);
            LoadAValues(postViewModel);
        }

        public void MapGroupPanelPostEntityToPostViewModelReadonly(GroupPanelPost groupPostEntity, PostViewModel postVM)
        {
            postVM.PostID = groupPostEntity.Post.PostID;
            postVM.CreatedDate = groupPostEntity.Post.CreatedDate;
            postVM.Title = groupPostEntity.Post.Title;
            postVM.CategoryID = groupPostEntity.Post.CategoryID;
            postVM.SubCategoryID = groupPostEntity.Post.SubCategoryID;
            postVM.DisplayCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId(groupPostEntity.Post.CategoryID);
            postVM.DisplaySubCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId(groupPostEntity.Post.SubCategoryID);
            postVM.Description = groupPostEntity.Post.Description;
            postVM.IsBrandNew = groupPostEntity.Post.IsBrandNew;
            postVM.IsUsed = groupPostEntity.Post.IsUsed;
            postVM.IsUrgent = groupPostEntity.Post.IsUrgent;
            postVM.IsForRent = groupPostEntity.Post.IsForRent;
            postVM.IsForSell = groupPostEntity.Post.IsForSell;
            postVM.PosterContactNumber = groupPostEntity.Post.PosterContactNumber;
            postVM.PosterName = groupPostEntity.Post.PosterName;
            postVM.Currency = groupPostEntity.Post.Currency;
            postVM.Price = groupPostEntity.Post.UnitPrice;
            postVM.FormattedPriceValue = postVM.GetFormatedPriceValue(postVM.Price.ToString());
            postVM.PublishDate = groupPostEntity.Post.PublishDate;
            postVM.WebsiteUrl = groupPostEntity.Post.WebsiteUrl;
            postVM.PostType = groupPostEntity.Post.PostType;
            postVM.SubCategoryCSS = BusinessObjectSeed.GetCategoryCSS(groupPostEntity.Post.SubCategoryID);
            postVM.CategoryCSS = BusinessObjectSeed.GetCategoryCSS(groupPostEntity.Post.CategoryID);
            postVM.LikeCount = groupPostEntity.Post.LikeCount.HasValue ? groupPostEntity.Post.LikeCount.Value : 0;
            postVM.CommentsCount = groupPostEntity.Post.ListComments != null && groupPostEntity.Post.ListComments.Count > 0 ? groupPostEntity.Post.ListComments.Count : 0;
            postVM.Comment = "";
            postVM.SearchTag = groupPostEntity.Post.SearchTag;
            MapImageFilesForDisplay(postVM, groupPostEntity.Post);
            MapUserReadonly(groupPostEntity.Post, postVM);
            MapAddressReadonly(groupPostEntity.Post, postVM);
            postVM.ListPostComments = GetPostCommentsReadonly(groupPostEntity.Post);
        }        

        private long GetCommentsCount(Post postEntity)
        {
            if (postEntity.ListComments == null || postEntity.ListComments.Count == 0)
                return 0;
            return postEntity.ListComments.Count;
        }

        private void SetAddressInformationReadonly(Post postEntity, PostViewModel postVM)
        {
            var address = postEntity.Address;
            if (address != null)
            {
                postVM.AreaDescription = address.AreaDescription;
                postVM.StateID = address.StateID;
                postVM.DisplayState = LocationRelatedSeed.GetStateDescription((EnumState)postVM.StateID);
            }
        }

        private void SetUserInformationReadonly(Post postEntity, PostViewModel postVM)
        {
            var userEntity = postEntity.User;
            if (userEntity != null)
            {
                postVM.ClientName = userEntity.ClientName;
                postVM.IsCompanySeller = userEntity.UserAccountType == EnumUserAccountType.Company ? true : false;
                postVM.IsPrivateSeller = userEntity.UserAccountType == EnumUserAccountType.IndividualAdvertiser ? true : false;
                postVM.UserID = userEntity.UserID;
            }
        }

        private byte[] GetSingleDisplayImage(Post postEntity)
        {
            var fileObj = postEntity.ImageFiles.Where(a => a.Image != null).FirstOrDefault();
            return fileObj == null ? fileObj.Image : new byte[1];
        }

        private void MapUserReadonly(Post postEntity, PostViewModel postVM)
        {
            var userEntity = postEntity.User;
            if (userEntity != null)
            {
                postVM.Email = userEntity.Email;
                postVM.ClientName = userEntity.ClientName;
                postVM.UserID = userEntity.UserID;
                postVM.IsPrivateSeller = userEntity.UserAccountType == EnumUserAccountType.IndividualAdvertiser ? true : false;
                postVM.IsCompanySeller = userEntity.UserAccountType == EnumUserAccountType.Company ? true : false;
            }
        }

        private void MapAddressReadonly(Post postEntity, PostViewModel postVM)
        {
            var address = postEntity.Address;
            if (address != null)
            {
                postVM.AreaDescription = address.AreaDescription;
                postVM.StateID = address.StateID;
                postVM.DisplayState = LocationRelatedSeed.GetStateDescription((EnumState)postVM.StateID);
            }
        }

        private List<PostCommentViewModel> GetPostCommentsReadonly(Post postEntity)
        {
            var listComments = postEntity.ListComments.Where(a => a.IsActive).ToList();
            if (listComments.Count == 0 || listComments == null)
                return new List<PostCommentViewModel>();

            List<PostCommentViewModel> objListPostComments = new List<PostCommentViewModel>();
            foreach (var item in listComments)
            {
                PostCommentViewModel objItem = new PostCommentViewModel();
                objItem.CommentID = item.CommentID;
                objItem.Comment = item.Comment;
                objItem.CommentDate = item.CreatedDate.ToShortDateString();
                objListPostComments.Add(objItem);
            }
            return objListPostComments.ToList();
        }

        private void MapImageFilesForDisplay(PostViewModel postVM, Post postEntity)
        {
            if (postVM == null) throw new ArgumentNullException("postVm");
            if (postEntity == null) throw new ArgumentNullException("post");
            List<File> imageList = new List<File>();
            if (postEntity.ImageFiles != null)
            {
                imageList = postEntity.ImageFiles.Where(a => a.IsActive).ToList();
            }

            FileViewModel objImageVM;
            foreach (var fileEntity in imageList.ToList())
            {
                objImageVM = new FileViewModel();
                objImageVM.Image = fileEntity.Image;
                objImageVM.FileID = fileEntity.FileID;
                objImageVM.FileName = fileEntity.FileName;
                objImageVM.PostID = fileEntity.PostID;
                objImageVM.IsNewItem = false;
                postVM.ListImages.Add(objImageVM);
            }
            var imageVM = postVM.ListImages.FirstOrDefault();
            postVM.Image = imageVM?.Image;
        }
    }
}
