using System.Collections.Generic;
using System.Linq;
using Model;
using Data;
using Common;
using System.Threading.Tasks;
using System.Configuration;
using System;
using System.Web.Mvc;

namespace WebDeshiHutBazar
{
    public class PostManagementService : IPostMangementService
    {
        private readonly IPostMappingService _PostMappingService;
        private readonly IPostRepository _PostRepository;
        public readonly IPostQueryRepository _PostQueryRepository;
        private readonly IUserRepository _UserRepository;        
        private readonly IAValueRepository _AvalueRepo;
        private readonly IImageProcessingService _ImageHandler;
        private readonly IPostCommentRepository _CommentRepo;

        public PostManagementService(IPostMappingService postMappingService,
            IPostRepository postRepository,
            IPostQueryRepository postQueryRepository,
            IUserRepository userRepository,
            IAValueRepository avalueRepo,
            IImageProcessingService imageHandler,
            IPostCommentRepository commentRepo)
        {
            _AvalueRepo = avalueRepo;
            _PostMappingService = postMappingService;
            _PostRepository = postRepository;
            _PostQueryRepository = postQueryRepository;
            _UserRepository = userRepository;
            _ImageHandler = imageHandler;
            _CommentRepo = commentRepo;
        }

        public PostViewModel CreateNewPost(EnumCurrency currency)
        {
            PostViewModel objPostVm = new PostViewModel(currency);
            objPostVm = _PostMappingService.LoadAValues(objPostVm);
            objPostVm.OfferType = EnumOfferType.General;
            return objPostVm;
        }        

        public async Task<long> SaveShortContentPost(PostViewModel objPostVm, EnumCountry country, 
            EnumPostType postType, UserModel user)
        {
            var objUser = await _UserRepository.GetSingleUser(ConfigurationManager.AppSettings["DefaultAdminEmail"]);
            Post objPost = new Post(country, postType, objUser, EnumCurrency.USD)
            {
                PosterName = objPostVm.PosterName,
                WebsiteUrl = objPostVm.WebsiteUrl,
                SearchTag = objPostVm.SearchTag,
                UnitPrice = objPostVm.Price,
                Title = objPostVm.Title,
                Description = objPostVm.Description
            };
            objPost.Address = new PostAddress(country)
            {
                StateID = 3,//Default State:Dhaka
                AreaDescription = "Dhaka default area"
            };
            objPostVm.ListImages.ForEach(fileVM =>
            {
                var objFile = new File(fileVM.FileName, fileVM.Image, objPost, country);
                objPost.ImageFiles.Add(objFile);
            });
            var postID = await _PostRepository.SavePost(objPost);
            return postID;
        }

        public async Task<long> SaveProductPost(PostViewModel objPostVM, EnumCountry country)
        {
            Post objPostEntity = new Post(country,EnumCurrency.USD);
            objPostEntity.PostType = EnumPostType.Product;
            objPostEntity.Currency = LocationRelatedSeed.GetCountryCurrency(country);
            objPostEntity.UnitPrice = objPostVM.Price;
            objPostEntity.DiscountedUnitPrice = objPostVM.DiscountedPrice;
            objPostEntity.PaymentGatewayCommissionAmountPercent = objPostVM.PaymentGatewayCommision;
            objPostEntity.DeshiHutBazarCommissionAmount = objPostVM.DeshiHutCommissionAmount;
            objPostEntity.ShopShareAmount = GetShopShareAmount(objPostVM);
            objPostEntity.DeshiHutBazarShareAmount = GetDeshiShareAmount(objPostVM);
            objPostEntity.PortalProductPrice = objPostVM.PortalProductPrice;

            objPostEntity.PostStatus = EnumPostStatus.FreePosted;
            objPostEntity.Title = objPostVM.Title;
            objPostEntity.CategoryID = objPostVM.CategoryID ?? 0;
            objPostEntity.SubCategoryID = objPostVM.SubCategoryID ?? 0;
            objPostEntity.Description = objPostVM.Description;            
            objPostEntity.SearchTag = objPostVM.SearchTag;
            objPostEntity.IsBrandNew = objPostVM.IsBrandNew;
            objPostEntity.IsUsed = objPostVM.IsUsed;
            objPostEntity.IsForRent = false;
            objPostEntity.IsForSell = true;
            objPostEntity.UserID = objPostVM.UserID;
            objPostVM.ListImages.ForEach(fileVM =>
            {
                var objFile = new File(fileVM.FileName, fileVM.Image, objPostEntity, country);
                objFile.EnumPhoto = fileVM.EnumPhoto;
                objPostEntity.ImageFiles.Add(objFile);
            });
            PostAddress objAddress = new PostAddress(EnumCountry.Bangladesh);
            objAddress.StateID = objPostVM.StateID.Value;
            objAddress.AreaDescription = objPostVM.AreaDescription;
            objPostEntity.Address = objAddress;
            var postID = await _PostRepository.SavePost(objPostEntity);
            return postID;
        }

        private decimal GetDeshiShareAmount(PostViewModel objPostVM)
        {
            var deshiAmount = objPostVM.DeshiHutCommissionAmount;          
            return deshiAmount;
        }

        private decimal GetShopShareAmount(PostViewModel objPostVM)
        {
            var productPrice = objPostVM.DiscountedPrice != 0 ? objPostVM.DiscountedPrice : objPostVM.Price;
            var deshiAmount = objPostVM.DeshiHutCommissionAmount;
            var paymentGateCommission = objPostVM.PaymentGatewayCommision;
            var shopShareAmount = productPrice - (productPrice * (decimal)(paymentGateCommission / 100) + deshiAmount);
            return shopShareAmount;
        }

        public async Task<long> SavePost(PostViewModel objPostVm, EnumCountry country, PackageConfig packageD, EnumCurrency currency)
        {
            Post objPost = new Post(country, currency);
            _PostMappingService.PostViewModelToPostEntityMappingForNewPostNewUser(objPostVm, objPost, country);

            var postID =  await _PostRepository.SavePost(objPost);
            objPostVm.UserID = objPost.UserID;

            var userid = objPost.UserID;
            var user = await _UserRepository.GetSingleUser(userid);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(objPostVm.Phone))
                {
                    user.Phone = objPostVm.Phone;
                }
                if (objPostVm.IsPrivateSeller)
                {
                    user.UserAccountType = EnumUserAccountType.IndividualAdvertiser;
                }
                if (objPostVm.IsCompanySeller)
                {
                    user.UserAccountType = EnumUserAccountType.Company;
                }
                var res = await _UserRepository.UpdateUser(user);
            }
            return postID;
        }

        public async Task<long> SavePostForExistingUser(PostViewModel objPostVm, EnumCountry country)
        {
            var objUser = await _UserRepository.GetSingleUser(objPostVm.Email);
            Post objPost = new Post(country, EnumCurrency.USD);
            _PostMappingService.PostViewModelToPostEntityMappingForNewPostExistingUser(objPostVm, objPost, objUser,country);
            var postID = await _PostRepository.SavePost(objPost);
            objPostVm.UserID = objPost.UserID;
            var userid = objPost.UserID;
            var user = await _UserRepository.GetSingleUser(userid);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(objPostVm.Phone))
                {
                    user.Phone = objPostVm.Phone;
                }
                if (objPostVm.IsPrivateSeller)
                {
                    user.UserAccountType = EnumUserAccountType.IndividualAdvertiser;
                }
                if (objPostVm.IsCompanySeller)
                {
                    user.UserAccountType = EnumUserAccountType.Company;
                }
                var res = await _UserRepository.UpdateUser(user);
            }
            return postID;
        }        
       
        public async Task<bool> UpdatePost(PostViewModel objPostVm, long postId, EnumCountry country)
        {           
            var post = await _PostRepository.GetPostByPostID(postId);
            _PostMappingService.PostViewModelToPostEntityMappingForExistingPost(objPostVm, post, country);
            var resultPostUpdate = await _PostRepository.SaveChanges();
            return true;
        }

        private async Task<bool> UpdateUserEntity(PostViewModel objPostVm)
        {
            var userId = objPostVm.UserID;
            var userEntityObject = await _UserRepository.GetSingleUser(userId);
            if (userEntityObject != null)
            {
                if (!string.IsNullOrEmpty(objPostVm.Phone))
                {
                    userEntityObject.Phone = objPostVm.Phone;
                }
                if (objPostVm.IsPrivateSeller)
                {
                    userEntityObject.UserAccountType = EnumUserAccountType.IndividualAdvertiser;
                }                
                if (objPostVm.IsCompanySeller)
                {
                    userEntityObject.UserAccountType = EnumUserAccountType.Company;
                }
                var res = await _UserRepository.UpdateUser(userEntityObject);
            }
            return true;
        }

        public async Task<bool> UpdatePostImages(PostViewModel objPostVm, long postId, EnumCountry country)
        {
            var post = await _PostRepository.GetPostByPostID(postId);
            post.ImageFiles.RemoveAll(a => a.FileID != 0);
            var resultFilesRemoved = await _PostRepository.SaveChanges();
            _PostMappingService.MapExistingFilesViewModelToFilesEntity(objPostVm, post, country);
            var resultFileSave = await _PostRepository.SaveChanges();
            return true;
        }

        public async Task<bool> DeletePostImage(long imageId)
        {
            var resultDelete = await _PostRepository.DeletePostImage(imageId);
            return true;
        }

        public async Task<bool> DeletePost(long postId, long currentUserID, EnumCountry country)
        {
            var resultDelete = await _PostRepository.DeletePost(postId, currentUserID, country);
            return true;
        }

        public async Task<bool> LikeThisPost(long postId, string actionType)
        {
            var res = await _PostRepository.LikeThisPost(postId, actionType);
            return true;
        }

        public async Task<PostDisplayViewModel> GetDisplayPostByID(long postID, EnumCurrency currency)
        {
            var postEntity = await _PostRepository.GetPostByPostID(postID);
            PostDisplayViewModel postViewModel = new PostDisplayViewModel(currency);
            postViewModel.CategoryID = postEntity.CategoryID;
            postViewModel.PostID = postEntity.PostID;
            postViewModel.Title = postEntity.Title;
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
            postViewModel.Currency = EnumCurrency.BDT.ToString();
            postViewModel.Price = postEntity.UnitPrice;
            postViewModel.DiscountedPrice = postEntity.DiscountedUnitPrice;
            postViewModel.FormattedPriceValue = postEntity.GetFormatedPriceValue("BDT");
            postViewModel.PublishDate = postEntity.PublishDate;
            postViewModel.WebsiteUrl = postEntity.WebsiteUrl;
            postViewModel.PostType = postEntity.PostType;
            postViewModel.SubCategoryCSS = BusinessObjectSeed.GetCategoryCSS(postEntity.SubCategoryID);
            postViewModel.CategoryCSS = BusinessObjectSeed.GetCategoryCSS(postEntity.CategoryID);
            postViewModel.LikeCount = postEntity.LikeCount.HasValue ? postEntity.LikeCount.Value : 0;
            postViewModel.CommentsCount = postEntity.ListComments != null && postEntity.ListComments.Count > 0 ? postEntity.ListComments.Count : 0;
            postViewModel.Comment = "";
            postViewModel.SearchTag = postEntity.SearchTag;
            MapImageFilesForDisplay(postViewModel, postEntity);
            MapAddressReadonly(postEntity, postViewModel);
            postViewModel.ListPostComments = GetPostCommentsReadonly(postEntity);
            postViewModel.CategoryID = postEntity.CategoryID;
            postViewModel.SubCategoryID = postEntity.SubCategoryID;
            try
            {
                postViewModel.UserID = postEntity.User.UserID;
                postViewModel.Email = postEntity.User.Email;
            }
            catch { }            
            return postViewModel;
        }

        private List<PostCommentViewModel> GetPostCommentsReadonly(Post postEntity)
        {
            var listComments = postEntity.ListComments.Where(a => a.IsActive).OrderByDescending(a => a.CreatedDate).ToList();
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

        private void MapAddressReadonly(Post postEntity, PostDisplayViewModel postVM)
        {
            var address = postEntity.Address;
            if (address != null)
            {
                postVM.AreaDescription = address.AreaDescription;
                postVM.DisplayState = postEntity.Address != null && DropDownDataList.GetAllStateList().Any(a => a.Value == postEntity.Address.StateID.ToString().Trim())
                                        ? DropDownDataList.GetAllStateList().FirstOrDefault(a => a.Value == postEntity.Address.StateID.ToString().Trim()).Text
                                        : "";
            }
        }

        private void MapImageFilesForDisplay(PostDisplayViewModel postVM, Post postEntity)
        {
            if (postVM == null) throw new ArgumentNullException("postVm");
            if (postEntity == null) throw new ArgumentNullException("post");
            var imageList = postEntity.ImageFiles.ToList();
            if (imageList == null)
            {
                return;
            }
            postVM.ListImages = new List<FileViewModel>();
            FileViewModel objImageVM;
            foreach (var fileEntity in imageList.Where(a => a.IsActive).ToList())
            {
                objImageVM = new FileViewModel();
                objImageVM.Image = fileEntity.Image;
                objImageVM.FileID = fileEntity.FileID;
                objImageVM.FileName = fileEntity.FileName;
                objImageVM.PostID = fileEntity.PostID;
                postVM.ListImages.Add(objImageVM);
            }
            postVM.Image = postVM.ListImages != null && postVM.ListImages.Count > 0 ? postVM.ListImages.FirstOrDefault().Image : null;
        }

        public async Task<PostViewModel> GetPostByPostIDForEdit(long postID, EnumCurrency currency)
        {
            var postEntity = await _PostRepository.GetPostByPostID(postID);
            PostViewModel objModel = new PostViewModel(currency);
            _PostMappingService.MapPostEntityToPostViewModelForEdit(postEntity, objModel);
            return objModel;
        }

        public async Task<PostViewModel> GetPostByPostIDReadonly(long postID, EnumCurrency currency)
        {
            var postEntity = await _PostRepository.GetPostByPostID(postID);
            PostViewModel objModel = new PostViewModel(currency);
            _PostMappingService.MapPostEntityToPostViewModelForEdit(postEntity, objModel);
            return objModel;
        }

        public async Task<List<PostViewModel>> GetAllPublishedPostsByUserID(long userID, EnumCountry country, EnumCurrency currency)
        {
            var listPosts = await _PostQueryRepository.GetAllUserPublishedPosts((int)userID, country);
            List<PostViewModel> objListPostVM = new List<PostViewModel>();
            PostViewModel objModel;
            foreach(var item in listPosts.ToList())
            {
                objModel = new PostViewModel(currency);
                _PostMappingService.MapPostEntityToPostViewModelForEdit(item, objModel);
                objListPostVM.Add(objModel);
            }
            return objListPostVM;
        }

        public async Task<List<PostViewModel>> GetAllUnpaidPostsByUserID(long userID, EnumCountry country, EnumCurrency currency)
        {
            var listPosts = await _PostQueryRepository.GetAllUserUnpaidPosts((int)userID, country);
            List<PostViewModel> objListPostVM = new List<PostViewModel>();
            PostViewModel objModel;
            foreach (var item in listPosts.ToList())
            {
                objModel = new PostViewModel(currency);
                _PostMappingService.MapPostEntityToPostViewModelForEdit(item, objModel);
                objListPostVM.Add(objModel);
            }
            return objListPostVM;
        }

        public async Task<List<PostViewModel>> GetAllPostsBySubCategoryID(long subCategoryID, 
            EnumDeviceType device, EnumCountry country, EnumCurrency currency)
        {
            var listPosts = await _PostQueryRepository.GetAllPostBySubCategoryID(subCategoryID, device, country);
            List<PostViewModel> objListPostVM = new List<PostViewModel>();
            PostViewModel objModel;
            foreach (var item in listPosts.ToList())
            {
                objModel = new PostViewModel(currency);
                _PostMappingService.MapPostEntityToPostViewModelForEdit(item, objModel);
                objListPostVM.Add(objModel);
            }
            return objListPostVM;
        }

        public async Task<List<SelectListItem>> GetAllFabiaPosts(EnumCountry country, EnumPostType postType)
        {
            var listFabiaPosts = await _PostQueryRepository.GetAllPosts(country, postType);
            List<SelectListItem> objListPostVM = new List<SelectListItem>();
            SelectListItem objModel;
            foreach (var item in listFabiaPosts.ToList())
            {
                objModel = new SelectListItem();
                objModel.Value = item.PostID.ToString();
                objModel.Text = item.Title;
                objListPostVM.Add(objModel);
            }
            return objListPostVM;
        }

        public async Task<List<PostViewModel>> GetAllPosts(EnumCountry country, EnumPostType postType, string detailUrl, EnumCurrency currency)
        {
            var listPosts = await _PostQueryRepository.GetAllPosts(country, postType);
            List<PostViewModel> objListPostVM = new List<PostViewModel>();
            PostViewModel objModel;
            foreach (var item in listPosts.ToList())
            {
                objModel = new PostViewModel(currency);
                _PostMappingService.MapPostEntityToPostViewModelForEdit(item, objModel);
                objModel.PostItemDetailViewUrl = detailUrl.Replace("POST_ITEM_ID", objModel.PostID.ToString());
                objListPostVM.Add(objModel);
            }
            return objListPostVM;
        }

        public async Task<List<PostViewModel>> GetMarketAllPosts(EnumCountry country, string detailUrl, EnumCurrency currency)
        {
            var listPosts = await _PostQueryRepository.GetAllPosts(country, EnumPostType.Post);
            List<PostViewModel> objListPostVM = new List<PostViewModel>();
            PostViewModel objModel;
            foreach (var item in listPosts.ToList())
            {
                objModel = new PostViewModel(currency);
                _PostMappingService.MapPostEntityToPostViewModelForPostTemplateDisplay(item, objModel);
                objModel.PostItemDetailViewUrl = detailUrl.Replace("POST_ITEM_ID", objModel.PostID.ToString());
                objListPostVM.Add(objModel);
            }
            return objListPostVM;
        }

        public async Task<List<PostViewModel>> GetMarketAllPosts(EnumCountry country, string detailUrl, int postValidDays, EnumCurrency currency)
        {
            var listPosts = await _PostQueryRepository.GetAllPosts(country, EnumPostType.Post);
            List<PostViewModel> objListPostVM = new List<PostViewModel>();
            PostViewModel objModel;
            foreach (var item in listPosts.ToList())
            {
                objModel = new PostViewModel(currency);
                _PostMappingService.MapPostEntityToPostViewModelForPostTemplateDisplay(item, objModel);
                objModel.PostItemDetailViewUrl = detailUrl.Replace("POST_ITEM_ID", objModel.PostID.ToString());
                objListPostVM.Add(objModel);
            }
            return objListPostVM;
        }

        public async Task<List<PostViewModel>> GetCategoryPosts(EnumCountry country,
            string detailUrl,
            long subCategoryId, EnumCurrency currency)
        {
            var listPosts = await _PostQueryRepository.GetAllPosts(country, EnumPostType.Post);
            List<PostViewModel> objListPostVM = new List<PostViewModel>();
            PostViewModel objModel;
            foreach (var item in listPosts.Where(a => a.CategoryID == subCategoryId).ToList())
            {
                objModel = new PostViewModel(currency);
                _PostMappingService.MapPostEntityToPostViewModelForPostTemplateDisplay(item, objModel);
                objModel.PostItemDetailViewUrl = detailUrl.Replace("POST_ITEM_ID", objModel.PostID.ToString());
                objListPostVM.Add(objModel);
            }
            return objListPostVM.OrderByDescending(a => a.PublishDate).ToList();
        }

        public async Task<List<PostViewModel>> GetCategoryMarketAllPosts(EnumCountry country, 
            string detailUrl,
            long subCategoryId, EnumCurrency currency)
        {
            var listPosts = await _PostQueryRepository.GetAllPosts(country, EnumPostType.Post);
            List<PostViewModel> objListPostVM = new List<PostViewModel>();
            PostViewModel objModel;
            foreach (var item in listPosts.Where(a=>a.SubCategoryID == subCategoryId).ToList())
            {
                objModel = new PostViewModel(currency);
                _PostMappingService.MapPostEntityToPostViewModelForPostTemplateDisplay(item, objModel);
                objModel.PostItemDetailViewUrl = detailUrl.Replace("POST_ITEM_ID", objModel.PostID.ToString());
                objListPostVM.Add(objModel);
            }
            return objListPostVM.OrderByDescending(a=>a.PublishDate).ToList();
        }

        public async Task<int> GetCategoryPostsCount(EnumCountry country, long subcategoryid)
        {
            return await _PostQueryRepository.GetCategoryPostsCount(country, subcategoryid);            
        }

        public async Task<List<PostViewModel>> GetAllModelPosts(EnumCountry country, EnumPostType postType, EnumCurrency currency)
        {
            var listPostEntities = await _PostQueryRepository.GetAllPosts(country, postType);
            List<PostViewModel> objListPostVM = new List<PostViewModel>();
            PostViewModel objModel;
            foreach (var item in listPostEntities.ToList())
            {
                objModel = new PostViewModel(currency);
                _PostMappingService.MapPostEntityToPostViewModelForEdit(item, objModel);
                objListPostVM.Add(objModel);
            }
            return objListPostVM;
        }

        public async Task<bool> AddComments(string comment, int postID, EnumCountry country)
        {
            IPostCommentRepository _CommentRepo = new PostRepository();
            var res = await _CommentRepo.SaveComment(comment, postID, country);
            return true;
        }

        public async Task<bool> LikeThisComment(long commentId, string actionType)
        {
            var res = await _PostRepository.LikeThisComment(commentId, actionType);
            return true;
        }

        public async Task<List<PostViewModel>> GetAllAdminPosts(EnumCountry country, EnumCurrency currency)
        {
            var listPosts = await _PostQueryRepository.GetAllAdminPosts(country);
            List<PostViewModel> objListPostVM = new List<PostViewModel>();
            PostViewModel objModel;
            foreach (var item in listPosts.ToList())
            {
                objModel = new PostViewModel(currency);
                _PostMappingService.MapPostEntityToPostViewModelForEdit(item, objModel);
                objListPostVM.Add(objModel);
            }
            return objListPostVM;
        }

        public async Task<bool> RemovePostService(long postId, long serviceId)
        {
            var post = await _PostRepository.DeletePostService(serviceId);
            return true;
        }

        public async Task<bool> RemovePostProcess(long postId, long processId)
        {
            var post = await _PostRepository.DeletePostProcess(processId);            
            return true;
        }

        public async Task<bool> AddPostService(PostServiceViewModel postServiceViewModel)
        {
            var post = await _PostRepository.GetPostByPostID(postServiceViewModel.PostID);

            if (post.ListPostService == null)
                post.ListPostService = new List<PostService>();

            PostService postService = new PostService(
                EnumCountry.Bangladesh,
                postServiceViewModel.Title,
                post,
                postServiceViewModel.ServicePrice,
                postServiceViewModel.PaidBy,
                postServiceViewModel.Discount,
                postServiceViewModel.ReasonPayment,
                postServiceViewModel.Description,
                postServiceViewModel.ServicePolicy,
                postServiceViewModel.TransportPolicy);
            postService.SetProcessImage(postServiceViewModel.ServiceImage);
            post.ListPostService.Add(postService);
            await _PostRepository.SaveChanges();
            return true;
        }

        public async Task<bool> AddPostProcess(PostProcessViewModel postProcessViewModel)
        {
            var post = await _PostRepository.GetPostByPostID(postProcessViewModel.PostID);

            if (post.ListPostProcess == null)
                post.ListPostProcess = new List<PostProcess>();

            PostProcess postProcess = new PostProcess(
                EnumCountry.Bangladesh,
                postProcessViewModel.StepNo,
                postProcessViewModel.StepName,
                post,
                postProcessViewModel.Price,
                postProcessViewModel.PaidBy,
                postProcessViewModel.AvailabilityDurationHour,
                postProcessViewModel.ReasonPayment,
                postProcessViewModel.Description);
            postProcess.SetProcessImage(postProcessViewModel.StepImage);
            post.ListPostProcess.Add(postProcess);
            await _PostRepository.SaveChanges();
            return true;
        }

        public async Task<bool> UpdatePostProcess(PostProcessViewModel postProcessViewModel)
        {
            var postProcess = await _PostRepository.GetPostProcessByID(postProcessViewModel.PostProcessID);
            if(postProcess == null)
                return false;
            postProcess.StepNo = postProcessViewModel.StepNo;
            postProcess.StepName = postProcessViewModel.StepName;
            postProcess.Price = postProcessViewModel.Price;
            postProcess.PaidBy = postProcessViewModel.PaidBy;
            postProcess.AvailabilityDurationHour = postProcessViewModel.AvailabilityDurationHour;
            postProcess.ReasonPayment = postProcessViewModel.ReasonPayment;
            postProcess.Description = postProcessViewModel.Description;
            postProcess.StepImage = postProcessViewModel.StepImage ?? postProcess.StepImage;
            var r = await _PostRepository.SaveChanges();
            return true;
        }

        public async Task<bool> UpdatePostService(PostServiceViewModel postServiceViewModel)
        {
            var postService = await _PostRepository.GetPostServiceByID(postServiceViewModel.PostServiceID);
            
            if (postService == null)
                return false;
            postService.Title = postServiceViewModel.Title;
            postService.TransportPolicy = postServiceViewModel.TransportPolicy;
            postService.ServicePrice = postServiceViewModel.ServicePrice;
            postService.PaidBy = postServiceViewModel.PaidBy;
            postService.ServicePolicy = postServiceViewModel.ServicePolicy;
            postService.ReasonPayment = postServiceViewModel.ReasonPayment;
            postService.Description = postServiceViewModel.Description;
            postService.Discount = postServiceViewModel.Discount;
            postService.ServiceImage = postServiceViewModel.ServiceImage ?? postService.ServiceImage;
            var r = await _PostRepository.SaveChanges();
            return true;
        }

        public async Task<bool> UpdatePostService(byte[] postServiceViewModel, long id)
        {
            var postService = await _PostRepository.GetPostServiceByID(id);

            if (postService == null)
                return false;
           
            postService.ServiceImage = postServiceViewModel ?? null;
            var r = await _PostRepository.SaveChanges();
            return true;
        }

        public async Task<bool> UpdatePostProcess(byte[] postProcessViewModel, long id)
        {
            var postProcess = await _PostRepository.GetPostProcessByID(id);

            if (postProcess == null)
                return false;

            postProcess.StepImage = postProcessViewModel ?? null;
            var r = await _PostRepository.SaveChanges();
            return true;
        }

        public async Task<List<PostViewModel>> GetAllProductsByUserID(long userID, EnumCountry country, EnumCurrency currency)
        {
            var listPostEntity = await _PostQueryRepository.GetAllProductPostByUserID(userID, country);
            List<PostViewModel> objListPostViewModel = new List<PostViewModel>();
            foreach(var postEntity in listPostEntity.ToList())
            {
                var postViewModel = new PostViewModel(currency);
                _PostMappingService.MapPostEntityToPostViewModelForEdit(postEntity, postViewModel);
                objListPostViewModel.Add(postViewModel);
            }
            return objListPostViewModel.ToList();
        }

        //public async Task<bool> AddFabiaProvider(FabiaServiceProviderViewModel objFabiaProvider, EnumCountry country)
        //{
        //    var postEntity = await _PostRepository.GetPostByPostID(objFabiaProvider.FabiaServiceID);

        //    FabiaServiceProvider objEntityFabiaProvider = new FabiaServiceProvider(objFabiaProvider.Email,
        //        objFabiaProvider.Phone,
        //        objFabiaProvider.ClientName,
        //        EnumCountry.Bangladesh);
        //    if (postEntity.ListFabiaServiceProvider == null)
        //        postEntity.ListFabiaServiceProvider = new List<FabiaServiceProvider>();

        //    postEntity.ListFabiaServiceProvider.Add(objEntityFabiaProvider);
        //    var res = await _PostRepository.SaveChanges();
        //    return true;
        //}
    }
}
