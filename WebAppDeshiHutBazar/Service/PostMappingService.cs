using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Data;
using Common;

namespace WebDeshiHutBazar
{
    public class PostMappingService : IPostMappingService
    {
        private readonly IAValueRepository _AValueRepo;
        private readonly HashingCryptographyService _HashingService;

        public PostMappingService(
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

        public void MapPostEntityToPostViewModelForEdit(Post postEntity, PostViewModel postViewModel)
        {
            postViewModel.PostID = postEntity.PostID;
            postViewModel.CreatedDate = postEntity.CreatedDate;
            postViewModel.Title = postEntity.Title;
            postViewModel.CategoryID = postEntity.CategoryID;
            postViewModel.SubCategoryID = postEntity.SubCategoryID;
            postViewModel.DisplayCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId(postEntity.CategoryID);
            postViewModel.DisplaySubCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId(postEntity.SubCategoryID);
            postViewModel.Description = postEntity.Description;
            postViewModel.IsBrandNew = postEntity.IsBrandNew;
            postViewModel.IsUsed = postEntity.IsUsed;
            postViewModel.IsUrgent = postEntity.IsUrgent;
            postViewModel.IsForRent = postEntity.IsForRent;
            postViewModel.IsForSell = postEntity.IsForSell;
            postViewModel.PosterContactNumber = postEntity.PosterContactNumber;
            postViewModel.PosterName = postEntity.PosterName;
            postViewModel.Currency = postEntity.Currency;
            postViewModel.Price = postEntity.UnitPrice;
            postViewModel.FormattedPriceValue = postViewModel.GetFormatedPriceValue(postViewModel.Price.ToString());
            postViewModel.PublishDate = postEntity.PublishDate;
            postViewModel.WebsiteUrl = postEntity.WebsiteUrl;
            postViewModel.PostType = postEntity.PostType;
            postViewModel.SubCategoryCSS = BusinessObjectSeed.GetCategoryCSS(postEntity.SubCategoryID);
            postViewModel.CategoryCSS = BusinessObjectSeed.GetCategoryCSS(postEntity.CategoryID);
            postViewModel.LikeCount = postEntity.LikeCount.HasValue ? postEntity.LikeCount.Value : 0;
            postViewModel.CommentsCount = postEntity.ListComments != null && postEntity.ListComments.Count > 0 ? postEntity.ListComments.Count : 0;
            postViewModel.Comment = "";
            postViewModel.SearchTag = postEntity.SearchTag;
            postViewModel.DiscountedPrice = postEntity.DiscountedUnitPrice;
            postViewModel.Price = postEntity.UnitPrice;
            postViewModel.DeshiHutCommissionAmount = postEntity.DeshiHutBazarCommissionAmount;
            postViewModel.ShopShareAmount = postEntity.ShopShareAmount;
            postViewModel.PaymentGatewayCommision = postEntity.PaymentGatewayCommissionAmountPercent;
            postViewModel.PortalProductPrice = postEntity.PortalProductPrice;
            MapImageFilesForDisplay(postViewModel, postEntity);
            MapUserReadonly(postEntity, postViewModel);
            MapAddressReadonly(postEntity, postViewModel);
            postViewModel.ListPostComments = GetPostCommentsReadonly(postEntity);
            postViewModel.ListPostProcess = GetPostProcess(postEntity);
            postViewModel.ListPostService = GetPostService(postEntity);
            LoadAValues(postViewModel);

            var user = postEntity.User;
            if (user != null)
            {
                postViewModel.UserID = user.UserID;
                postViewModel.Email = user.Email;
            }
        }

        private List<PostServiceViewModel> GetPostService(Post postEntity)
        {
            if (postEntity.ListPostService == null)
                return new List<PostServiceViewModel>();

            var listPostService = postEntity.ListPostService.Where(a => a.IsActive).OrderBy(a => a.Title).ToList();
           
            List<PostServiceViewModel> objListPostServiceViewModel = new List<PostServiceViewModel>();
            foreach (var item in listPostService)
            {
                PostServiceViewModel objItem = new PostServiceViewModel();
                objItem.PostServiceID = item.PostServiceID;
                objItem.PostID = item.PostID.Value;
                objItem.Description = item.Description;
                objItem.ServicePrice = item.ServicePrice;
                objItem.ServicePolicy = item.ServicePolicy;
                objItem.TransportPolicy = item.TransportPolicy;
                objItem.ReasonPayment = item.ReasonPayment;
                objItem.ServiceImage = item.ServiceImage;
                objItem.PaidBy = item.PaidBy;
                objItem.Discount = item.Discount;
                objItem.CreatedDate = item.CreatedDate;
                objItem.Title = item.Title;
                objItem.AV_PaidBy = DropDownDataList.GetPaidByList();
                objListPostServiceViewModel.Add(objItem);
            }
            return objListPostServiceViewModel.ToList();
        }

        private List<PostProcessViewModel> GetPostProcess(Post postEntity)
        {
            if (postEntity.ListPostProcess == null)
                return new List<PostProcessViewModel>();

            var listPostProcess = postEntity.ListPostProcess.Where(a => a.IsActive).ToList();

            List<PostProcessViewModel> objListPostProcessViewModel = new List<PostProcessViewModel>();
            foreach (var item in listPostProcess)
            {
                PostProcessViewModel objItem = new PostProcessViewModel();
                objItem.PostProcessID = item.PostProcessID;
                objItem.PostID = item.PostID.Value;
                objItem.Description = item.Description;
                objItem.Price = item.Price;
                objItem.StepNo = item.StepNo;
                objItem.StepName = item.StepName;
                objItem.ReasonPayment = item.ReasonPayment;
                objItem.StepImage = item.StepImage;
                objItem.PaidBy = item.PaidBy;
                objItem.AvailabilityDurationHour = item.AvailabilityDurationHour;
                objItem.CreatedDate = item.CreatedDate;
                objItem.AV_StepNo = DropDownDataList.GetStepsList();
                objItem.AV_PaidBy = DropDownDataList.GetPaidByList();
                objListPostProcessViewModel.Add(objItem);
            }
            return objListPostProcessViewModel.ToList();
        }

        public void MapPostEntityToPostViewModelSelectGroupConfig(Post postEntity, PostViewModel postViewModel)
        {
            postViewModel.PostID = postEntity.PostID;
            postViewModel.CreatedDate = postEntity.CreatedDate;
            postViewModel.Title = postEntity.Title;
            postViewModel.CategoryID = postEntity.CategoryID;
            postViewModel.SubCategoryID = postEntity.SubCategoryID;
            postViewModel.DisplayCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId(postEntity.CategoryID);
            postViewModel.DisplaySubCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId(postEntity.SubCategoryID);
            postViewModel.Description = postEntity.Description;
            postViewModel.IsBrandNew = postEntity.IsBrandNew;
            postViewModel.IsUsed = postEntity.IsUsed;
            postViewModel.IsUrgent = postEntity.IsUrgent;
            postViewModel.IsForRent = postEntity.IsForRent;
            postViewModel.IsForSell = postEntity.IsForSell;
            postViewModel.PosterContactNumber = postEntity.PosterContactNumber;
            postViewModel.PosterName = postEntity.PosterName;
            postViewModel.Currency = postEntity.Currency;
            postViewModel.Price = postEntity.UnitPrice;
            postViewModel.FormattedPriceValue = postViewModel.GetFormatedPriceValue(postViewModel.Price.ToString());
            postViewModel.PublishDate = postEntity.PublishDate;
            postViewModel.WebsiteUrl = postEntity.WebsiteUrl;
            postViewModel.PostType = postEntity.PostType.HasValue ? postEntity.PostType.Value : EnumPostType.Post;
            postViewModel.SubCategoryCSS = BusinessObjectSeed.GetCategoryCSS(postEntity.SubCategoryID);
            postViewModel.CategoryCSS = BusinessObjectSeed.GetCategoryCSS(postEntity.CategoryID);
            postViewModel.LikeCount = postEntity.LikeCount.HasValue ? postEntity.LikeCount.Value : 0;
            postViewModel.CommentsCount = postEntity.ListComments != null && postEntity.ListComments.Count > 0 ? postEntity.ListComments.Count : 0;
            postViewModel.Comment = "";
            postViewModel.SearchTag = postEntity.SearchTag;
            MapImageFilesForDisplay(postViewModel, postEntity);
            MapUserReadonly(postEntity, postViewModel);
            MapAddressReadonly(postEntity, postViewModel);           
        }


        public void MapPostEntityToPostViewModelReadonly(Post postEntity, PostViewModel postVM)
        {
            postVM.PostID = postEntity.PostID;
            postVM.CreatedDate = postEntity.CreatedDate;
            postVM.Title = postEntity.Title;
            postVM.CategoryID = postEntity.CategoryID;
            postVM.SubCategoryID = postEntity.SubCategoryID;
            postVM.DisplayCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId(postEntity.CategoryID);
            postVM.DisplaySubCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId(postEntity.SubCategoryID);
            postVM.Description = postEntity.Description;
            postVM.IsBrandNew = postEntity.IsBrandNew;
            postVM.IsUsed = postEntity.IsUsed;
            postVM.IsUrgent = postEntity.IsUrgent;
            postVM.IsForRent = postEntity.IsForRent;
            postVM.IsForSell = postEntity.IsForSell;
            postVM.PosterContactNumber = postEntity.PosterContactNumber;
            postVM.PosterName = postEntity.PosterName;
            postVM.Currency = postEntity.Currency;
            postVM.Price = postEntity.UnitPrice;
            postVM.FormattedPriceValue = postVM.GetFormatedPriceValue(postVM.Price.ToString());
            postVM.PublishDate = postEntity.PublishDate;
            postVM.WebsiteUrl = postEntity.WebsiteUrl;
            postVM.PostType = postEntity.PostType;
            postVM.SubCategoryCSS = BusinessObjectSeed.GetCategoryCSS(postEntity.SubCategoryID);
            postVM.CategoryCSS = BusinessObjectSeed.GetCategoryCSS(postEntity.CategoryID);
            postVM.LikeCount = postEntity.LikeCount.HasValue ? postEntity.LikeCount.Value : 0;
            postVM.CommentsCount = postEntity.ListComments != null && postEntity.ListComments.Count > 0 ? postEntity.ListComments.Count : 0;
            postVM.Comment = "";
            postVM.SearchTag = postEntity.SearchTag;
            MapImageFilesForDisplay(postVM, postEntity);
            MapUserReadonly(postEntity, postVM);
            MapAddressReadonly(postEntity, postVM);
            postVM.ListPostComments = GetPostCommentsReadonly(postEntity);
        }

        public void MapPostEntityToPostViewModelForPostTemplateDisplay(Post postEntity, PostViewModel postVM)
        {
            postVM.CategoryID = postEntity.CategoryID;
            postVM.SubCategoryID = postEntity.SubCategoryID;
            postVM.PostID = postEntity.PostID;
            postVM.CreatedDate = postEntity.PublishDate.HasValue ? postEntity.PublishDate.Value : postEntity.CreatedDate;
            postVM.Title = postEntity.Title;            
            postVM.IsBrandNew = postEntity.IsBrandNew;
            postVM.IsUsed = postEntity.IsUsed;
            postVM.IsUrgent = postEntity.IsUrgent;
            postVM.IsForRent = postEntity.IsForRent;
            postVM.IsForSell = postEntity.IsForSell;
            postVM.PosterName = postEntity.PosterName;
            postVM.Currency = postEntity.Currency;
            postVM.Price = postEntity.UnitPrice;
            postVM.FormattedPriceValue = postVM.GetFormatedPriceValue(postVM.Price.ToString());
            postVM.PublishDate = postEntity.PublishDate;
            postVM.PosterName = postEntity.PosterName;
            postVM.LikeCount = postEntity.LikeCount.HasValue ? postEntity.LikeCount.Value : 0;
            postVM.CommentsCount = GetCommentsCount(postEntity);
            postVM.Image = GetSingleDisplayImage(postEntity);            
            SetUserInformationReadonly(postEntity, postVM);
            SetAddressInformationReadonly(postEntity, postVM);
            postVM.WebsiteUrl = postEntity.WebsiteUrl;
            postVM.PostType = postEntity.PostType;

            PostCommentViewModel objPostComment;
            List<PostCommentViewModel> objPostCommentList = new List<PostCommentViewModel>();
            foreach (var comment in postEntity.ListComments)
            {
                objPostComment = new PostCommentViewModel();
                objPostComment.Comment = comment.Comment;
                objPostComment.CommentID = comment.CommentID;
                objPostComment.PostID = comment.PostID;
                objPostComment.CommentDate = comment.CreatedDate.ToString();
                objPostCommentList.Add(objPostComment);
            }
            postVM.ListPostComments = objPostCommentList;
            postVM.CommentsCount = objPostCommentList.Count;
        }

        public void MapPostEntityToPostViewModelForPostLimitedDisplay(Post postEntity, PostViewModel postVM)
        {            
            postVM.PostID = postEntity.PostID;
            postVM.CategoryID = postEntity.CategoryID;
            postVM.SubCategoryID = postEntity.SubCategoryID;
            postVM.DisplayCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId(postEntity.CategoryID);
            postVM.DisplaySubCategory = BusinessObjectSeed.GetCatSubCategoryItemTextForId(postEntity.SubCategoryID);
            postVM.Title = postEntity.Title;           
            postVM.Currency = postEntity.Currency;
            postVM.Price = postEntity.UnitPrice;
            postVM.FormattedPriceValue = postVM.GetFormatedPriceValue(postVM.Price.ToString());
            postVM.PublishDate = postEntity.PublishDate;
            postVM.LikeCount = postEntity.LikeCount.HasValue ? postEntity.LikeCount.Value : 0;
            postVM.CommentsCount = GetCommentsCount(postEntity);           
            postVM.WebsiteUrl = postEntity.WebsiteUrl;
            postVM.PostType = postEntity.PostType;
            postVM.Description = postEntity.Description;

            PostCommentViewModel objPostComment;
            List<PostCommentViewModel> objPostCommentList = new List<PostCommentViewModel>();
            foreach (var comment in postEntity.ListComments.OrderByDescending(a=>a.CreatedDate).ToList())
            {
                objPostComment = new PostCommentViewModel();
                objPostComment.Comment = comment.Comment;
                objPostComment.CommentID = comment.CommentID;
                objPostComment.PostID = comment.PostID;
                objPostComment.Like = comment.Like;
                objPostComment.CommentDate = comment.CreatedDate.ToString();
                objPostCommentList.Add(objPostComment);
            }
            postVM.ListPostComments = objPostCommentList;
            postVM.ListPostProcess = GetPostProcess(postEntity);
            postVM.ListPostService = GetPostService(postEntity);
        }

        public void PostViewModelToPostEntityMappingForNewPostNewUser(PostViewModel objPostVM, Post objPostEntity, EnumCountry country)
        {
            objPostEntity.PostStatus = EnumPostStatus.Payable;
            objPostEntity.Title = objPostVM.Title;
            objPostEntity.CategoryID = objPostVM.CategoryID ?? 0;
            objPostEntity.SubCategoryID = objPostVM.SubCategoryID ?? 0;
            objPostEntity.Description = objPostVM.Description;
            objPostEntity.UnitPrice = objPostVM.Price;
            objPostEntity.Currency = LocationRelatedSeed.GetCountryCurrency(country);
            objPostEntity.SearchTag = objPostVM.SearchTag;
            objPostEntity.IsRecent = true;
            objPostEntity.IsBrandNew = objPostVM.IsBrandNew;
            objPostEntity.IsUsed= objPostVM.IsUsed;
            objPostEntity.IsUrgent = objPostVM.IsUrgent;
            objPostEntity.IsForRent = false;
            objPostEntity.IsForSell = true;
            objPostEntity.PosterName = objPostVM.ClientName;
            objPostEntity.PosterContactNumber = objPostVM.Phone;
            objPostEntity.Address = CreateNewAddress(country, objPostVM);
            objPostEntity.User = CreateNewUser(country, objPostVM);
            CreateNewFiles(country, objPostVM, objPostEntity);
        }

        public void PostViewModelToPostEntityMappingForNewPostExistingUser(PostViewModel objPostVM,
            Post objPostEntity,
            User objUserEntity,
            EnumCountry enumCountry)
        {
            objPostEntity.UserID = objUserEntity.UserID;
            objPostEntity.PostStatus = EnumPostStatus.Payable;
            objPostEntity.Title = objPostVM.Title;
            objPostEntity.CategoryID = objPostVM.CategoryID.HasValue ? objPostVM.CategoryID.Value : 0;
            objPostEntity.SubCategoryID = objPostVM.SubCategoryID.HasValue ? objPostVM.SubCategoryID.Value : 0;
            objPostEntity.Description = objPostVM.Description;
            objPostEntity.UnitPrice = objPostVM.Price;
            objPostEntity.Currency = LocationRelatedSeed.GetCountryCurrency(enumCountry);
            objPostEntity.SearchTag = objPostVM.SearchTag;
            objPostEntity.IsRecent = true;
            objPostEntity.IsBrandNew = objPostVM.IsBrandNew;
            objPostEntity.IsUsed = objPostVM.IsUsed;
            objPostEntity.IsUrgent = objPostVM.IsUrgent;
            objPostEntity.IsForRent = false;
            objPostEntity.IsForSell = true;
            objPostEntity.PosterContactNumber = objPostVM.Phone;
            objPostEntity.PosterName = objPostVM.ClientName;
            objPostEntity.Address = CreateNewAddress(enumCountry, objPostVM);
            CreateNewFiles(enumCountry, objPostVM, objPostEntity);
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
                postVM.DisplayState = postEntity.Address != null && DropDownDataList.GetAllStateList().Any(a => a.Value == postEntity.Address.StateID.ToString().Trim())
                ? DropDownDataList.GetAllStateList().FirstOrDefault(a => a.Value == postEntity.Address.StateID.ToString().Trim()).Text
                : "";
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
            return fileObj != null && fileObj.Image != null ? fileObj.Image : null;
        }

        private void MapUserReadonly(Post postEntity, PostViewModel postVM)
        {
            var userEntity = postEntity.User;
            if (userEntity != null)
            {
                postVM.UserID = userEntity.UserID;
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
                postVM.DisplayState = postEntity.Address != null && DropDownDataList.GetAllStateList().Any(a => a.Value == postEntity.Address.StateID.ToString().Trim())
                ? DropDownDataList.GetAllStateList().FirstOrDefault(a => a.Value == postEntity.Address.StateID.ToString().Trim()).Text
                : "";
            }
        }

        private List<PostCommentViewModel> GetPostCommentsReadonly(Post postEntity)
        {
            var listComments = postEntity.ListComments.Where(a => a.IsActive).OrderByDescending(a=>a.CreatedDate).ToList();
            if (listComments.Count == 0 || listComments == null)
                return new List<PostCommentViewModel>();

            List<PostCommentViewModel> objListPostComments = new List<PostCommentViewModel>();
            foreach (var item in listComments)
            {
                PostCommentViewModel objItem = new PostCommentViewModel();
                objItem.CommentID = item.CommentID;
                objItem.Comment = item.Comment;
                objItem.CommentDate = item.CreatedDate.ToShortDateString();
                objItem.Like = item.Like;
                objListPostComments.Add(objItem);
            }
            return objListPostComments.ToList();
        }

        private void MapImageFilesForDisplay(PostViewModel postVM, Post postEntity)
        {
            if (postVM == null) throw new ArgumentNullException("postVm");
            if (postEntity == null) throw new ArgumentNullException("post");
            var imageList = postEntity.ImageFiles.ToList();
            if (imageList == null)
            {
                return;
            }
            FileViewModel objImageVM;
            foreach (var fileEntity in imageList.Where(a => a.IsActive).ToList())
            {
                objImageVM = new FileViewModel();
                objImageVM.Image = fileEntity.Image;
                objImageVM.FileID = fileEntity.FileID;
                objImageVM.FileName = fileEntity.FileName;
                objImageVM.PostID = fileEntity.PostID;
                objImageVM.EnumPhoto = fileEntity.EnumPhoto;
                postVM.ListImages.Add(objImageVM);
            }
            postVM.Image = postVM.ListImages != null && postVM.ListImages.Count > 0 ? postVM.ListImages.FirstOrDefault().Image : null;
        }

        public void PostViewModelToPostEntityMappingForExistingPost(PostViewModel objPostVM, Post postEntity, EnumCountry enumCountry)
        {
            if(postEntity.PostType == EnumPostType.Product)
            {
                postEntity.CategoryID = objPostVM.CategoryID.Value;
                postEntity.SubCategoryID = objPostVM.SubCategoryID.Value;                     
            }
            postEntity.Title = objPostVM.Title;
            postEntity.Description = objPostVM.Description;
            postEntity.UnitPrice = objPostVM.Price;
            postEntity.SearchTag = objPostVM.SearchTag;
            postEntity.Currency = LocationRelatedSeed.GetCountryCurrency(enumCountry);
            postEntity.IsRecent = true;
            postEntity.IsBrandNew = objPostVM.IsBrandNew;
            postEntity.IsUsed = objPostVM.IsBrandNew == true ? false : true;
            postEntity.IsUrgent = objPostVM.IsUrgent;
            postEntity.IsForRent = objPostVM.IsForRent;
            postEntity.IsForSell = objPostVM.IsForSell;
            postEntity.EnumCountry = enumCountry;
            postEntity.PosterContactNumber = objPostVM.PosterContactNumber;
            postEntity.PosterName = objPostVM.PosterName;
            postEntity.WebsiteUrl = objPostVM.WebsiteUrl;
            postEntity.DiscountedUnitPrice = objPostVM.DiscountedPrice;
            postEntity.UnitPrice = objPostVM.Price;
            postEntity.DeshiHutBazarCommissionAmount = objPostVM.DeshiHutCommissionAmount;
            postEntity.ShopShareAmount = objPostVM.ShopShareAmount;
            postEntity.PaymentGatewayCommissionAmountPercent= objPostVM.PaymentGatewayCommision;
            postEntity.PortalProductPrice = objPostVM.PortalProductPrice;
            MapExistingViewModelAddressToExistingEntityAddress(postEntity, objPostVM, enumCountry);
            MapExitingImageFilesToEntityImageFiles(postEntity, objPostVM, enumCountry);
        }

        private void MapExitingImageFilesToEntityImageFiles(Post postEntity,
            PostViewModel objPostVM,
            EnumCountry enumCountry)
        {
            if (postEntity.ImageFiles == null && postEntity.ImageFiles.Count == 0)
            {
                postEntity.ImageFiles = new List<File>();
            }
            objPostVM.ListImages.ForEach(fileVM => {
                var objFile = new File(fileVM.FileName, fileVM.Image, postEntity, enumCountry);
                objFile.EnumPhoto = fileVM.EnumPhoto;
                postEntity.ImageFiles.Add(objFile);
            });
        }

        private void MapExistingViewModelAddressToExistingEntityAddress(Post postEntity,
            PostViewModel objPostVM,
            EnumCountry country)
        {
            if (postEntity.Address == null)
            {
                postEntity.Address = new PostAddress(country);
            }
            postEntity.Address.StateID = objPostVM.StateID ?? 0;
            postEntity.Address.AreaDescription = objPostVM.AreaDescription;
        }

        public void MapExistingFilesViewModelToFilesEntity(PostViewModel objPostVM,
            Post postEntity,
            EnumCountry enumCountry)
        {
            postEntity.ImageFiles = new List<File>();
            objPostVM.ListImages.ForEach(fileVM => {
                var objFile = new File(fileVM.FileName, fileVM.Image, postEntity, enumCountry);
                postEntity.ImageFiles.Add(objFile);
            });
        }

        private User CreateNewUser(EnumCountry country, PostViewModel objPostVM)
        {
            var passwordVM = _HashingService.GetMessageDigest(objPostVM.Password);
            EnumUserAccountType userType = objPostVM.IsCompanySeller == true ? EnumUserAccountType.Company : EnumUserAccountType.IndividualAdvertiser;
            var objUser = new User(objPostVM.Email, passwordVM.Digest, objPostVM.ClientName, userType, passwordVM.Salt, country)
            {
                Phone = objPostVM.Phone
            };
            return objUser;
        }

        private void CreateNewFiles(EnumCountry country, PostViewModel objPostVM, Post objPostEntity)
        {
            objPostVM.ListImages.ForEach(fileVM => {
                var objFile = new File(fileVM.FileName, fileVM.Image, objPostEntity, country);
                objPostEntity.ImageFiles.Add(objFile);
            });
        }

        private PostAddress CreateNewAddress(EnumCountry country, PostViewModel objPostVM)
        {
            return new PostAddress(country)
            {
                StateID = objPostVM.StateID ?? 0,
                AreaDescription = objPostVM.AreaDescription
            };
        }
    }
}
