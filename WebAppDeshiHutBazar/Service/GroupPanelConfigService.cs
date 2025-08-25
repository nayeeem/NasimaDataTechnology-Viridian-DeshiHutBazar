using System.Collections.Generic;
using System.Linq;
using Model;
using Data;
using Common;

using System.Threading.Tasks;

using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using System;
namespace WebDeshiHutBazar
{
    public class GroupPanelConfigService : IGroupPanelConfigService
    {
        private readonly IGroupPanelConfigRepository _GroupPanelConfigRepository;
        private readonly IGroupPanelPostRepository _PanelPostRepository;
        private readonly IPostRepository _PostRepository;
        private readonly IPostQueryRepository _PostQueryRepository;
        private readonly IPostMappingService _PostMappingService;
        private readonly IPostMangementService _PostManagementService;
        private readonly IImageProcessingService _ImageProcessingService;
        private readonly IRepoDropDownDataList _IRepoDropDownDataList;

        public GroupPanelConfigService(
            IGroupPanelConfigRepository groupPanelConfigRepository,
            IPostMappingService postMappingService,
            IGroupPanelPostRepository panelPostRepository,
            IPostRepository postRepository,
            IPostQueryRepository postQueryRepository,
            IPostMangementService postManagementService,
            IImageProcessingService imageProcessingService,
            IRepoDropDownDataList repoDropDownDataList
            )
        {
            _GroupPanelConfigRepository = groupPanelConfigRepository;
            _PostRepository = postRepository;
            _PostQueryRepository = postQueryRepository;
            _PostMappingService = postMappingService;
            _PanelPostRepository = panelPostRepository;
            _PostManagementService = postManagementService;
            _ImageProcessingService = imageProcessingService;
            _IRepoDropDownDataList = repoDropDownDataList;
        }

        private bool ShoudShowCartButton(User user)
        {
            if (user != null)
            {
                if (user.UserAccountType == EnumUserAccountType.Company)
                {
                    return true;
                }
            }
            return false;
        }

        private byte[] GetImage(GroupPanelPost postPanelEntity, Post postEntity)
        {
            if (postPanelEntity.PrimaryImage != null && postPanelEntity.PrimaryImage.Image != null)
            {
                return postPanelEntity.PrimaryImage.Image;
            }
            else if (postEntity.ImageFiles != null && postEntity.ImageFiles.Any(a => a.Image != null))
            {
                return postEntity.ImageFiles.Where(a => a.Image != null).FirstOrDefault().Image;
            }
            return null;
        }

        private byte[] GetImage(Post postEntity)
        {
            if (postEntity.ImageFiles != null && postEntity.ImageFiles.Any(a => a.Image != null))
            {
                return postEntity.ImageFiles.Where(a => a.Image != null).FirstOrDefault().Image;
            }
            return null;
        }

        private string GetClientName(Post postEntity)
        {
            var userEntity = postEntity.User;
            if (userEntity != null)
            {
                return userEntity.ClientName;
            }
            return "";
        }

        private string GetClientEmail(Post postEntity)
        {
            var userEntity = postEntity.User;
            if (userEntity != null)
            {
                return userEntity.Email;
            }
            return "";
        }

        private string GetClientState(Post postEntity)
        {
            var address = postEntity.Address;
            if (address != null)
            {
                return postEntity.Address != null && DropDownDataList.GetAllStateList().Any(a => a.Value == postEntity.Address.StateID.ToString().Trim())
                 ? DropDownDataList.GetAllStateList().FirstOrDefault(a => a.Value == postEntity.Address.StateID.ToString().Trim()).Text
                 : "";
            }
            return "";
        }

        private List<PostCommentViewModel> GetCommentList(List<PostComment> listCommentEntity)
        {
            if (listCommentEntity != null && listCommentEntity.Count > 0)
            {
                List<PostCommentViewModel> objCommentListViewModel = new List<PostCommentViewModel>();
                PostCommentViewModel objCommentViewModel;
                foreach (var commentEntity in listCommentEntity.Where(a => a.IsActive && !string.IsNullOrEmpty(a.Comment)).ToList())
                {
                    objCommentViewModel = new PostCommentViewModel();
                    objCommentViewModel.PostID = commentEntity.PostID;
                    objCommentViewModel.Comment = commentEntity.Comment;
                    objCommentViewModel.CommentID = commentEntity.CommentID;
                    objCommentViewModel.CommentDate = commentEntity.CreatedDate.ToShortDateString();
                    objCommentListViewModel.Add(objCommentViewModel);
                }
                return objCommentListViewModel;
            }
            return new List<PostCommentViewModel>();
        }

        public async Task<List<GroupPanelTemplateDisplayViewModel>>
            GetAllPageGroupPanelConfigurations(
                    EnumPublicPage pageName,
                    string viewMoreUrl,
                    string viewPostDetailsUrl,
                    EnumCountry country,
                    int dayTimeSlot,
                    EnumCurrency currency)
        {
            var listGroupPanelConfigs = await _GroupPanelConfigRepository.GetAllPublishedGroupPanelConfig(country, pageName, null);
            if (listGroupPanelConfigs == null)
                return new List<GroupPanelTemplateDisplayViewModel>();
            GroupPanelTemplateDisplayViewModel objGroupPanelConfigurationVM;
            List<GroupPanelTemplateDisplayViewModel> objListGroupPanelConfigurationVM = new List<GroupPanelTemplateDisplayViewModel>();
            foreach (var singleConfigEntity in listGroupPanelConfigs.ToList())
            {
                objGroupPanelConfigurationVM = new GroupPanelTemplateDisplayViewModel()
                {
                    GroupPanelConfigID = singleConfigEntity.GroupPanelConfigID,
                    ShowOrHide = singleConfigEntity.ShowOrHide,
                    Order = singleConfigEntity.Order,
                    Device = singleConfigEntity.Device,
                    EnumPanelDisplayStyle = singleConfigEntity.EnumPanelDisplayStyle,
                    GroupPanelTitle = singleConfigEntity.GroupPanelTitle,
                    EnumPublicPage = singleConfigEntity.EnumPublicPage.Value,
                    PublishStatus = singleConfigEntity.EnumGroupPanelStatus
                };

                if (singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.MarketPanel
                    || singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.SimilarItemsMarketPanel)
                {
                    objGroupPanelConfigurationVM.HasToShowPostsOrOrder = false;
                }
                else if (singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.AllFixedButtons)
                {
                    objGroupPanelConfigurationVM.FabiaInformationViewModel = await LoadFabiaButtonInformation();
                    objGroupPanelConfigurationVM.HasToShowPostsOrOrder = false;
                }
                else if (singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.QuardMarketPanel &&
                    !string.IsNullOrEmpty(objGroupPanelConfigurationVM.CategoryID))
                {
                    objGroupPanelConfigurationVM.HasToShowPostsOrOrder = false;
                    List<TemplateDisplayViewModel> objListTemplatePosts = new List<TemplateDisplayViewModel>();
                    TemplateDisplayViewModel objTemplatePostViewModel;
                    var listPostEntities = await _PostQueryRepository.GetAllPosts(EnumCountry.Bangladesh, EnumPostType.Post);

                    if (!string.IsNullOrEmpty(objGroupPanelConfigurationVM.CategoryID))
                    {
                        listPostEntities = listPostEntities.Where(a =>
                                                     a.CategoryID == Convert.ToInt64(objGroupPanelConfigurationVM.CategoryID)).OrderByDescending(a => a.PublishDate).ToList();
                    }
                    else
                    {
                        listPostEntities = listPostEntities.Take(4).ToList();
                    }
                    if (listPostEntities != null)
                    {
                        foreach (var postEntity in listPostEntities.Take(4))
                        {
                            objTemplatePostViewModel = new TemplateDisplayViewModel(currency);
                            objTemplatePostViewModel.PostID = postEntity.PostID;
                            objTemplatePostViewModel.Title = postEntity.Title;
                            objTemplatePostViewModel.Price = postEntity.UnitPrice;
                            objTemplatePostViewModel.FormattedPriceValue = postEntity.GetFormatedPriceValue("BDT");
                            objTemplatePostViewModel.PublishDate = postEntity.GetPublishDate;
                            objTemplatePostViewModel.LikeCount = postEntity.GetLikeCount;
                            objTemplatePostViewModel.CommentsCount = postEntity.GetCommentCount;
                            objTemplatePostViewModel.WebsiteUrl = postEntity.WebsiteUrl;
                            objTemplatePostViewModel.PostType = postEntity.GetpostType;
                            objTemplatePostViewModel.ListPostComments = GetCommentList(postEntity.ListComments);
                            objTemplatePostViewModel.Image = GetImage(postEntity);
                            objTemplatePostViewModel.PostItemDetailViewUrl = postEntity.GetItemDetailsURL(viewPostDetailsUrl);
                            objTemplatePostViewModel.CategoryID = postEntity.CategoryID;
                            objTemplatePostViewModel.SubCategoryID = postEntity.SubCategoryID;
                            objTemplatePostViewModel.DisplayCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.CategoryID);
                            objTemplatePostViewModel.DisplaySubCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.SubCategoryID);
                            objTemplatePostViewModel.PosterName = postEntity.PosterName;
                            objTemplatePostViewModel.ClientName = GetClientName(postEntity);
                            objTemplatePostViewModel.PosterContactNumber = postEntity.PosterContactNumber;
                            objTemplatePostViewModel.Email = GetClientEmail(postEntity);
                            objTemplatePostViewModel.DisplayState = GetClientState(postEntity);
                            objTemplatePostViewModel.IsBrandNew = postEntity.IsBrandNew;
                            objTemplatePostViewModel.IsUrgent = postEntity.IsUrgent;
                            objTemplatePostViewModel.IsForRent = postEntity.IsForRent;
                            objTemplatePostViewModel.IsForSell = postEntity.IsForSell;
                            objTemplatePostViewModel.IsPrivateSeller = postEntity.User != null ? postEntity.User.UserAccountType == EnumUserAccountType.IndividualAdvertiser ? true : false : false;
                            objTemplatePostViewModel.IsCompanySeller = postEntity.User != null ? postEntity.User.UserAccountType == EnumUserAccountType.Company ? true : false : false;
                            objTemplatePostViewModel.Title = postEntity.Title;
                            objTemplatePostViewModel.ShowCartButton = ShoudShowCartButton(postEntity.User);
                            objListTemplatePosts.Add(objTemplatePostViewModel);
                        }
                        objGroupPanelConfigurationVM.ListGroupPost = objListTemplatePosts;
                    }
                    objGroupPanelConfigurationVM.GroupPanelTitle = "Our Latest " + objGroupPanelConfigurationVM.DisplayCategory;
                }
                else if(singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.MostPopularProduct ||
                    singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.PopularCategory ||
                    singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.StarPanel ||
                    singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.DoubleBlockSquarePanel ||                    
                    singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.TrianglePanel)
                {
                    objGroupPanelConfigurationVM.HasToShowPostsOrOrder = true;
                    var listGroupPanelPostEntity = singleConfigEntity.ListPanelPost.Where(a => a.IsActive).ToList();
                    List<TemplateDisplayViewModel> objListTemplatePosts = new List<TemplateDisplayViewModel>();
                    TemplateDisplayViewModel objTemplatePostViewModel;
                    if (listGroupPanelPostEntity != null)
                    {
                        foreach (var postPanelEntity in listGroupPanelPostEntity)
                        {
                            objTemplatePostViewModel = new TemplateDisplayViewModel(currency);
                            if (postPanelEntity != null && postPanelEntity.PostID.HasValue)
                            {
                                var postEntity = await _PostRepository.GetPostByPostID(postPanelEntity.PostID.Value);
                                if (postEntity != null)
                                {
                                    objTemplatePostViewModel.PostID = postEntity.PostID;
                                    objTemplatePostViewModel.Title = postEntity.Title;
                                    objTemplatePostViewModel.Price = postEntity.UnitPrice;
                                    objTemplatePostViewModel.FormattedPriceValue = postEntity.GetFormatedPriceValue("BDT");
                                    objTemplatePostViewModel.PublishDate = postEntity.GetPublishDate;
                                    objTemplatePostViewModel.LikeCount = postEntity.GetLikeCount;
                                    objTemplatePostViewModel.CommentsCount = postEntity.GetCommentCount;
                                    objTemplatePostViewModel.WebsiteUrl = postEntity.WebsiteUrl;
                                    objTemplatePostViewModel.PostType = postEntity.GetpostType;
                                    objTemplatePostViewModel.ListPostComments = GetCommentList(postEntity.ListComments);
                                    objTemplatePostViewModel.Image = GetImage(postPanelEntity, postEntity);
                                    objTemplatePostViewModel.PostItemDetailViewUrl = postEntity.GetItemDetailsURL(viewPostDetailsUrl);
                                    objTemplatePostViewModel.ViewMoreUrl = postEntity.GetViewMoreURL(viewMoreUrl, null);
                                    objTemplatePostViewModel.GroupPostID = postPanelEntity.GroupPostID;
                                    objTemplatePostViewModel.Description = postEntity.Description;
                                    objTemplatePostViewModel.PosterName = postEntity.PosterName;
                                    objTemplatePostViewModel.ShowCartButton = ShoudShowCartButton(postEntity.User);
                                    objTemplatePostViewModel.CategoryID = postEntity.CategoryID;
                                    objTemplatePostViewModel.SubCategoryID = postEntity.SubCategoryID;
                                    objTemplatePostViewModel.DisplayCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.CategoryID);
                                    objTemplatePostViewModel.DisplaySubCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.SubCategoryID);
                                    objListTemplatePosts.Add(objTemplatePostViewModel);
                                }
                            }
                        }
                        objGroupPanelConfigurationVM.ListGroupPost = objListTemplatePosts;
                    }
                }
                else
                {
                    objGroupPanelConfigurationVM.HasToShowPostsOrOrder = true;
                    var listGroupPanelPostEntity = singleConfigEntity.ListPanelPost.Where(a => a.IsActive).ToList();
                    List<TemplateDisplayViewModel> objListTemplatePosts = new List<TemplateDisplayViewModel>();
                    TemplateDisplayViewModel objTemplatePostViewModel;
                    if (listGroupPanelPostEntity != null)
                    {
                        foreach (var postPanelEntity in listGroupPanelPostEntity)
                        {
                            objTemplatePostViewModel = new TemplateDisplayViewModel(currency);
                            if (postPanelEntity != null && postPanelEntity.PostID.HasValue)
                            {
                                var postEntity = await _PostRepository.GetPostByPostID(postPanelEntity.PostID.Value);
                                if (postEntity != null)
                                {
                                    objTemplatePostViewModel.PostID = postEntity.PostID;
                                    objTemplatePostViewModel.Title = postEntity.Title;
                                    objTemplatePostViewModel.Price = postEntity.UnitPrice;
                                    objTemplatePostViewModel.FormattedPriceValue = postEntity.GetFormatedPriceValue("BDT");
                                    objTemplatePostViewModel.PublishDate = postEntity.GetPublishDate;
                                    objTemplatePostViewModel.LikeCount = postEntity.GetLikeCount;
                                    objTemplatePostViewModel.CommentsCount = postEntity.GetCommentCount;
                                    objTemplatePostViewModel.WebsiteUrl = postEntity.WebsiteUrl;
                                    objTemplatePostViewModel.PostType = postEntity.GetpostType;
                                    objTemplatePostViewModel.ListPostComments = GetCommentList(postEntity.ListComments);
                                    objTemplatePostViewModel.Image = GetImage(postPanelEntity, postEntity);
                                    objTemplatePostViewModel.PostItemDetailViewUrl = postEntity.GetItemDetailsURL(viewPostDetailsUrl);
                                    objTemplatePostViewModel.CategoryID = postEntity.CategoryID;
                                    objTemplatePostViewModel.SubCategoryID = postEntity.SubCategoryID;
                                    objTemplatePostViewModel.DisplayCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.CategoryID);
                                    objTemplatePostViewModel.DisplaySubCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.SubCategoryID);
                                    objTemplatePostViewModel.GroupPostID = postPanelEntity.GroupPostID;
                                    objTemplatePostViewModel.Description = postEntity.Description;
                                    objTemplatePostViewModel.PosterName = postEntity.PosterName;
                                    objTemplatePostViewModel.ShowCartButton = ShoudShowCartButton(postEntity.User);
                                    objListTemplatePosts.Add(objTemplatePostViewModel);
                                }
                            }
                        }
                        objGroupPanelConfigurationVM.ListGroupPost = objListTemplatePosts;
                    }
                }
                objListGroupPanelConfigurationVM.Add(objGroupPanelConfigurationVM);
            }
            return objListGroupPanelConfigurationVM;
        }        

        public async Task<List<GroupPanelTemplateDisplayViewModel>>
            GetAllPageGroupPanelConfigurations(
                    EnumPublicPage pageName,
                    string viewMoreUrl,
                    string viewPostDetailsUrl,
                    EnumCountry country,
                    int dayTimeSlot,
                    EnumMarketType? typeMarket,
                    long typeMarketCategoryID,
                    int? pageNumber,
                    int pageSize,
                    decimal? price,
                    EnumCurrency currency
                    )
        {
            var listGroupPanelConfigs = await _GroupPanelConfigRepository.GetAllPublishedGroupPanelConfig(country, pageName, EnumGroupPanelStatus.Published);
            if (listGroupPanelConfigs != null)
                new List<GroupPanelTemplateDisplayViewModel>();
            GroupPanelTemplateDisplayViewModel objGroupPanelConfigurationVM;
            List<GroupPanelTemplateDisplayViewModel> objListGroupPanelConfigurationVM = new List<GroupPanelTemplateDisplayViewModel>();
            foreach (var singleConfigEntity in listGroupPanelConfigs.ToList())
            {
                objGroupPanelConfigurationVM = new GroupPanelTemplateDisplayViewModel()
                {
                    GroupPanelConfigID = singleConfigEntity.GroupPanelConfigID,
                    ShowOrHide = singleConfigEntity.ShowOrHide,
                    Order = singleConfigEntity.Order,
                    Device = singleConfigEntity.Device,
                    EnumPanelDisplayStyle = singleConfigEntity.EnumPanelDisplayStyle,
                    GroupPanelTitle = singleConfigEntity.GroupPanelTitle,
                    EnumPublicPage = singleConfigEntity.EnumPublicPage                    
                };
                if (singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.AllFixedButtons)
                {
                    objGroupPanelConfigurationVM.FabiaInformationViewModel = await LoadFabiaButtonInformation();
                }
                else if (typeMarket.HasValue && singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.MarketPanel)
                {
                    List<TemplateDisplayViewModel> objListTemplatePosts = new List<TemplateDisplayViewModel>();
                    TemplateDisplayViewModel objTemplatePostViewModel;
                    var listPostEntities = await _PostQueryRepository.GetAllPosts(EnumCountry.Bangladesh, EnumPostType.Post);
                    if (typeMarket == EnumMarketType.Category)
                    {
                        listPostEntities = listPostEntities.Where(a => a.CategoryID == typeMarketCategoryID).OrderByDescending(a => a.PublishDate).ToList();
                        objGroupPanelConfigurationVM.TotalPostCount = listPostEntities.Count;
                        int skipCount = (pageNumber.Value - 1) * pageSize;
                        listPostEntities = listPostEntities.Skip(skipCount).Take(pageSize).ToList();
                        objGroupPanelConfigurationVM.GroupPanelTitle = "Today's " + BusinessObjectSeed.GetCateSubCategoryItemText(typeMarketCategoryID) + " Market";
                    }
                    else if (typeMarket == EnumMarketType.SubCategory)
                    {
                        listPostEntities = listPostEntities.Where(a => a.SubCategoryID == typeMarketCategoryID).OrderByDescending(a => a.PublishDate).ToList();
                        objGroupPanelConfigurationVM.TotalPostCount = listPostEntities.Count;
                        int skipCount = (pageNumber.Value - 1) * pageSize;
                        listPostEntities = listPostEntities.Skip(skipCount).Take(pageSize).ToList();
                        objGroupPanelConfigurationVM.GroupPanelTitle = "Today's " + BusinessObjectSeed.GetCateSubCategoryItemText(typeMarketCategoryID) + " Market";
                    }
                    else
                    {
                        objGroupPanelConfigurationVM.TotalPostCount = listPostEntities.Count;
                        int skipCount = (pageNumber.Value - 1) * pageSize;
                        listPostEntities = listPostEntities.Skip(skipCount).Take(pageSize).ToList();
                        objGroupPanelConfigurationVM.GroupPanelTitle = "Today's Latest Market";
                    }
                    foreach (var postEntity in listPostEntities.ToList())
                    {
                        objTemplatePostViewModel = new TemplateDisplayViewModel(currency);
                        if (postEntity != null)
                        {
                            objTemplatePostViewModel.PostID = postEntity.PostID;
                            objTemplatePostViewModel.Title = postEntity.Title;
                            objTemplatePostViewModel.Price = postEntity.UnitPrice;
                            objTemplatePostViewModel.FormattedPriceValue = postEntity.GetFormatedPriceValue("BDT");
                            objTemplatePostViewModel.PublishDate = postEntity.GetPublishDate;
                            objTemplatePostViewModel.LikeCount = postEntity.GetLikeCount;
                            objTemplatePostViewModel.CommentsCount = postEntity.GetCommentCount;
                            objTemplatePostViewModel.WebsiteUrl = postEntity.WebsiteUrl;
                            objTemplatePostViewModel.PostType = postEntity.GetpostType;
                            objTemplatePostViewModel.ListPostComments = GetCommentList(postEntity.ListComments);
                            objTemplatePostViewModel.Image = GetImage(postEntity);
                            objTemplatePostViewModel.PostItemDetailViewUrl = postEntity.GetItemDetailsURL(viewPostDetailsUrl);
                            objTemplatePostViewModel.PosterName = postEntity.PosterName;
                            objTemplatePostViewModel.ClientName = GetClientName(postEntity);
                            objTemplatePostViewModel.PosterContactNumber = postEntity.PosterContactNumber;
                            objTemplatePostViewModel.Email = GetClientEmail(postEntity);
                            objTemplatePostViewModel.DisplayState = GetClientState(postEntity);
                            objTemplatePostViewModel.IsBrandNew = postEntity.IsBrandNew;
                            objTemplatePostViewModel.IsUrgent = postEntity.IsUrgent;
                            objTemplatePostViewModel.IsForRent = postEntity.IsForRent;
                            objTemplatePostViewModel.IsForSell = postEntity.IsForSell;
                            objTemplatePostViewModel.IsPrivateSeller = GetPrivateUserType(postEntity.User);
                            objTemplatePostViewModel.IsCompanySeller = GetCompanyUserType(postEntity.User); 
                            objTemplatePostViewModel.Title = postEntity.Title;
                            objTemplatePostViewModel.ShowCartButton = ShoudShowCartButton(postEntity.User);
                            objTemplatePostViewModel.CategoryID = postEntity.CategoryID;
                            objTemplatePostViewModel.SubCategoryID = postEntity.SubCategoryID;
                            objTemplatePostViewModel.DisplayCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.CategoryID);
                            objTemplatePostViewModel.DisplaySubCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.SubCategoryID);
                            objListTemplatePosts.Add(objTemplatePostViewModel);
                        }
                    }
                    objGroupPanelConfigurationVM.ListGroupPost = objListTemplatePosts;
                }
                else if (singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.SimilarItemsMarketPanel
                    && typeMarketCategoryID != 0)
                {
                    objGroupPanelConfigurationVM.GroupPanelTitle = "Similar Product Ads, you may like!";
                    List<TemplateDisplayViewModel> objListTemplatePosts = new List<TemplateDisplayViewModel>();
                    TemplateDisplayViewModel objTemplatePostViewModel;
                    var listPostEntities = await _PostQueryRepository.GetAllPosts(EnumCountry.Bangladesh, EnumPostType.Post);
                    if (typeMarket == EnumMarketType.SimilarItems && price != 0)
                    {
                        listPostEntities = listPostEntities.Where(a =>
                        a.SubCategoryID == typeMarketCategoryID &&
                        a.UnitPrice >= (price - 5000) &&
                        a.UnitPrice <= (price + 5000)).OrderByDescending(a => a.PublishDate).ToList();
                        listPostEntities = listPostEntities.Take(4).ToList();
                    }
                    else
                    {
                        listPostEntities = listPostEntities.Where(a =>
                        a.SubCategoryID == typeMarketCategoryID).OrderByDescending(a => a.PublishDate).ToList();
                        listPostEntities = listPostEntities.Take(4).ToList();
                    }
                    foreach (var postEntity in listPostEntities.ToList())
                    {
                        objTemplatePostViewModel = new TemplateDisplayViewModel(currency);
                        if (postEntity != null)
                        {
                            objTemplatePostViewModel.PostID = postEntity.PostID;
                            objTemplatePostViewModel.Title = postEntity.Title;
                            objTemplatePostViewModel.Price = postEntity.UnitPrice;
                            objTemplatePostViewModel.FormattedPriceValue = postEntity.GetFormatedPriceValue("BDT");
                            objTemplatePostViewModel.PublishDate = postEntity.GetPublishDate;
                            objTemplatePostViewModel.LikeCount = postEntity.GetLikeCount;
                            objTemplatePostViewModel.CommentsCount = postEntity.GetCommentCount;
                            objTemplatePostViewModel.WebsiteUrl = postEntity.WebsiteUrl;
                            objTemplatePostViewModel.PostType = postEntity.GetpostType;
                            objTemplatePostViewModel.ListPostComments = GetCommentList(postEntity.ListComments);
                            objTemplatePostViewModel.Image = GetImage(postEntity);
                            objTemplatePostViewModel.PostItemDetailViewUrl = postEntity.GetItemDetailsURL(viewPostDetailsUrl);
                            objTemplatePostViewModel.PosterName = postEntity.PosterName;
                            objTemplatePostViewModel.ClientName = GetClientName(postEntity);
                            objTemplatePostViewModel.PosterContactNumber = postEntity.PosterContactNumber;
                            objTemplatePostViewModel.Email = GetClientEmail(postEntity);
                            objTemplatePostViewModel.DisplayState = GetClientState(postEntity);
                            objTemplatePostViewModel.IsBrandNew = postEntity.IsBrandNew;
                            objTemplatePostViewModel.IsUrgent = postEntity.IsUrgent;
                            objTemplatePostViewModel.IsForRent = postEntity.IsForRent;
                            objTemplatePostViewModel.IsForSell = postEntity.IsForSell;
                            objTemplatePostViewModel.IsPrivateSeller = GetPrivateUserType(postEntity.User);
                            objTemplatePostViewModel.IsCompanySeller = GetCompanyUserType(postEntity.User);
                            objTemplatePostViewModel.Title = postEntity.Title;
                            objTemplatePostViewModel.ShowCartButton = ShoudShowCartButton(postEntity.User);
                            objTemplatePostViewModel.CategoryID = postEntity.CategoryID;
                            objTemplatePostViewModel.SubCategoryID = postEntity.SubCategoryID;
                            objTemplatePostViewModel.DisplayCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.CategoryID);
                            objTemplatePostViewModel.DisplaySubCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.SubCategoryID);
                            objListTemplatePosts.Add(objTemplatePostViewModel);
                        }
                    }
                    objGroupPanelConfigurationVM.ListGroupPost = objListTemplatePosts;
                }
                else if (singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.QuardMarketPanel &&
                    !string.IsNullOrEmpty(objGroupPanelConfigurationVM.CategoryID))
                {

                    List<TemplateDisplayViewModel> objListTemplatePosts = new List<TemplateDisplayViewModel>();
                    TemplateDisplayViewModel objTemplatePostViewModel;
                    var listPostEntities = await _PostQueryRepository.GetAllPosts(EnumCountry.Bangladesh, EnumPostType.Post);

                    if (!string.IsNullOrEmpty(objGroupPanelConfigurationVM.CategoryID))
                    {
                        listPostEntities = listPostEntities.Where(a =>
                                                     a.CategoryID == Convert.ToInt64(objGroupPanelConfigurationVM.CategoryID)).OrderByDescending(a => a.PublishDate).ToList();
                        listPostEntities = listPostEntities.Take(4).ToList();
                    }
                    if (listPostEntities != null)
                    {
                        foreach (var postEntity in listPostEntities.ToList())
                        {
                            objTemplatePostViewModel = new TemplateDisplayViewModel(currency);
                            if (postEntity != null)
                            {
                                objTemplatePostViewModel.PostID = postEntity.PostID;
                                objTemplatePostViewModel.Title = postEntity.Title;
                                objTemplatePostViewModel.Price = postEntity.UnitPrice;
                                objTemplatePostViewModel.FormattedPriceValue = postEntity.GetFormatedPriceValue("BDT");
                                objTemplatePostViewModel.PublishDate = postEntity.GetPublishDate;
                                objTemplatePostViewModel.LikeCount = postEntity.GetLikeCount;
                                objTemplatePostViewModel.CommentsCount = postEntity.GetCommentCount;
                                objTemplatePostViewModel.WebsiteUrl = postEntity.WebsiteUrl;
                                objTemplatePostViewModel.PostType = postEntity.GetpostType;
                                objTemplatePostViewModel.ListPostComments = GetCommentList(postEntity.ListComments);
                                objTemplatePostViewModel.Image = GetImage(postEntity);
                                objTemplatePostViewModel.PostItemDetailViewUrl = postEntity.GetItemDetailsURL(viewPostDetailsUrl);
                                objTemplatePostViewModel.PosterName = postEntity.PosterName;
                                objTemplatePostViewModel.ClientName = GetClientName(postEntity);
                                objTemplatePostViewModel.PosterContactNumber = postEntity.PosterContactNumber;
                                objTemplatePostViewModel.Email = GetClientEmail(postEntity);
                                objTemplatePostViewModel.DisplayState = GetClientState(postEntity);
                                objTemplatePostViewModel.IsBrandNew = postEntity.IsBrandNew;
                                objTemplatePostViewModel.IsUrgent = postEntity.IsUrgent;
                                objTemplatePostViewModel.IsForRent = postEntity.IsForRent;
                                objTemplatePostViewModel.IsForSell = postEntity.IsForSell;
                                objTemplatePostViewModel.IsPrivateSeller = postEntity.User != null ? postEntity.User.UserAccountType == EnumUserAccountType.IndividualAdvertiser ? true : false : false;
                                objTemplatePostViewModel.IsCompanySeller = postEntity.User != null ? postEntity.User.UserAccountType == EnumUserAccountType.Company ? true : false : false;
                                objTemplatePostViewModel.Title = postEntity.Title;
                                objTemplatePostViewModel.ShowCartButton = ShoudShowCartButton(postEntity.User);
                                objTemplatePostViewModel.CategoryID = postEntity.CategoryID;
                                objTemplatePostViewModel.SubCategoryID = postEntity.SubCategoryID;
                                objTemplatePostViewModel.DisplayCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.CategoryID);
                                objTemplatePostViewModel.DisplaySubCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.SubCategoryID);
                                objListTemplatePosts.Add(objTemplatePostViewModel);
                            }
                        }
                        objGroupPanelConfigurationVM.ListGroupPost = objListTemplatePosts;
                    }
                    objGroupPanelConfigurationVM.GroupPanelTitle = "Our Latest " + BusinessObjectSeed.GetCateSubCategoryItemText(Convert.ToInt64(objGroupPanelConfigurationVM.CategoryID));
                }
                else
                {
                    var listGroupPanelPostEntity = singleConfigEntity.ListPanelPost.Where(a => a.IsActive).ToList(); //GetGroupPanelPosts(singleConfigEntity, dayTimeSlot).ToList();
                    List<TemplateDisplayViewModel> objListTemplatePosts = new List<TemplateDisplayViewModel>();
                    TemplateDisplayViewModel objTemplatePostViewModel;
                    if (listGroupPanelPostEntity != null)
                    {
                        foreach (var postPanelEntity in listGroupPanelPostEntity)
                        {
                            objTemplatePostViewModel = new TemplateDisplayViewModel(currency);
                            if (postPanelEntity != null && postPanelEntity.PostID.HasValue)
                            {
                                var postEntity = await _PostRepository.GetPostByPostID(postPanelEntity.PostID.Value);
                                if (postEntity != null)
                                {
                                    objTemplatePostViewModel.PostID = postEntity.PostID;
                                    objTemplatePostViewModel.Title = postEntity.Title;
                                    objTemplatePostViewModel.Price = postEntity.UnitPrice;
                                    objTemplatePostViewModel.FormattedPriceValue = postEntity.GetFormatedPriceValue("BDT");
                                    objTemplatePostViewModel.PublishDate = postEntity.GetPublishDate;
                                    objTemplatePostViewModel.LikeCount = postEntity.GetLikeCount;
                                    objTemplatePostViewModel.CommentsCount = postEntity.GetCommentCount;
                                    objTemplatePostViewModel.WebsiteUrl = postEntity.WebsiteUrl;
                                    objTemplatePostViewModel.PostType = postEntity.GetpostType;
                                    objTemplatePostViewModel.ListPostComments = GetCommentList(postEntity.ListComments);
                                    objTemplatePostViewModel.Image = GetImage(postPanelEntity, postEntity);
                                    objTemplatePostViewModel.PostItemDetailViewUrl = postEntity.GetItemDetailsURL(viewPostDetailsUrl);
                                    objTemplatePostViewModel.GroupPostID = postPanelEntity.GroupPostID;
                                    objTemplatePostViewModel.Description = postEntity.Description;
                                    objTemplatePostViewModel.PosterName = postEntity.PosterName;
                                    objTemplatePostViewModel.ShowCartButton = ShoudShowCartButton(postEntity.User);
                                    objTemplatePostViewModel.CategoryID = postEntity.CategoryID;
                                    objTemplatePostViewModel.SubCategoryID = postEntity.SubCategoryID;
                                    objTemplatePostViewModel.DisplayCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.CategoryID);
                                    objTemplatePostViewModel.DisplaySubCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.SubCategoryID);
                                    objListTemplatePosts.Add(objTemplatePostViewModel);
                                }
                            }
                        }
                        objGroupPanelConfigurationVM.ListGroupPost = objListTemplatePosts;
                    }
                }
                objListGroupPanelConfigurationVM.Add(objGroupPanelConfigurationVM);
            }
            return objListGroupPanelConfigurationVM;
        }        

        private bool GetPrivateUserType(User user)
        {
            if(user != null)
            {
                if (user.UserAccountType == EnumUserAccountType.IndividualAdvertiser)
                    return true;                
            }
            return false;
        }

        private bool GetCompanyUserType(User user)
        {
            if (user != null)
            {
                if (user.UserAccountType == EnumUserAccountType.Company)
                    return true;
            }
            return false;
        }

        public async Task<GroupPanelConfigurationViewModel> GetSingleGroupPanelConfig(int panelConfigID, EnumCountry country)
        {
            var listAllConfigEntities = await _GroupPanelConfigRepository.GetAllGroupPanelConfig(country);
            var singleConfigEntity = listAllConfigEntities.FirstOrDefault(a => a.GroupPanelConfigID == panelConfigID);
            GroupPanelConfigurationViewModel objGroupPanelConfigurationVM;
            objGroupPanelConfigurationVM = new GroupPanelConfigurationViewModel
            {
                GroupPanelConfigID = singleConfigEntity.GroupPanelConfigID,
                ShowOrHide = singleConfigEntity.ShowOrHide,
                Order = singleConfigEntity.Order,
                Device = singleConfigEntity.Device,
                EnumPanelDisplayStyle = singleConfigEntity.EnumPanelDisplayStyle,
                GroupPanelTitle = singleConfigEntity.GroupPanelTitle,
                AV_Device = DropDownDataList.GetDeviceTypeList(),
                AV_ShowHide = DropDownDataList.GetShowHideList(),
                AV_EnumPanelDisplayStyle = DropDownDataList.GetPanelDisplayPositionList(),
                AV_EnumPublicPage = DropDownDataList.GetPageList(),
                PublicPage = singleConfigEntity.EnumPublicPage,
                AV_Users = await _IRepoDropDownDataList.GetUsersList(),
                PanelConfigUserID = singleConfigEntity.PanelConfigUserID
            };
            return objGroupPanelConfigurationVM;
        }

        public async Task<GroupPanelConfigurationViewModel> GetSingleGroupConfigPosts(int panelConfigID, 
            EnumCountry country, EnumCurrency currency)
        {
            var singleConfigEntity = await _GroupPanelConfigRepository.GetSingleGroupPanelConfig(country, panelConfigID);
            if(singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.AllFixedButtons ||
                singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.MarketPanel ||
                singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.QuardMarketPanel ||
                singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.SimilarItemsMarketPanel)
            {
                return new GroupPanelConfigurationViewModel();
            }

            GroupPanelConfigurationViewModel objGroupPanelConfigurationVM;
            objGroupPanelConfigurationVM = new GroupPanelConfigurationViewModel
            {
                GroupPanelConfigID = singleConfigEntity.GroupPanelConfigID,
                GroupPanelTitle = singleConfigEntity.GroupPanelTitle,
                PublicPage = singleConfigEntity.EnumPublicPage,
                EnumPanelDisplayStyle = singleConfigEntity.EnumPanelDisplayStyle
            };

            var listGroupPanelPostEntity = singleConfigEntity.ListPanelPost.Where(a=>a.IsActive);
            List<TemplateDisplayViewModel> objListTemplatePosts = new List<TemplateDisplayViewModel>();
            TemplateDisplayViewModel objTemplatePostViewModel;
            if (listGroupPanelPostEntity != null)
            {
                foreach (var postPanelEntity in listGroupPanelPostEntity)
                {
                    objTemplatePostViewModel = new TemplateDisplayViewModel(currency);
                    if (postPanelEntity != null && postPanelEntity.PostID.HasValue)
                    {
                        var postEntity = await _PostRepository.GetPostByPostID(postPanelEntity.PostID.Value);
                        if (postEntity != null)
                        {
                            objTemplatePostViewModel.PostID = postEntity.PostID;
                            objTemplatePostViewModel.Title = postEntity.Title;
                            objTemplatePostViewModel.Price = postEntity.UnitPrice;
                            objTemplatePostViewModel.FormattedPriceValue = postEntity.GetFormatedPriceValue("BDT");
                            objTemplatePostViewModel.PublishDate = postEntity.GetPublishDate;
                            objTemplatePostViewModel.LikeCount = postEntity.GetLikeCount;
                            objTemplatePostViewModel.CommentsCount = postEntity.GetCommentCount;
                            objTemplatePostViewModel.WebsiteUrl = postEntity.WebsiteUrl;
                            objTemplatePostViewModel.PostType = postEntity.GetpostType;
                            objTemplatePostViewModel.ListPostComments = GetCommentList(postEntity.ListComments);
                            objTemplatePostViewModel.Image = GetImage(postPanelEntity, postEntity);
                            objTemplatePostViewModel.GroupPostID = postPanelEntity.GroupPostID;
                            objTemplatePostViewModel.Description = postEntity.Description;
                            objTemplatePostViewModel.PosterName = postEntity.PosterName;
                            objTemplatePostViewModel.ShowCartButton = ShoudShowCartButton(postEntity.User);
                            objTemplatePostViewModel.CategoryID = postEntity.CategoryID;
                            objTemplatePostViewModel.SubCategoryID = postEntity.SubCategoryID;
                            objTemplatePostViewModel.DisplayCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.CategoryID);
                            objTemplatePostViewModel.DisplaySubCategory = BusinessObjectSeed.GetCateSubCategoryItemText(postEntity.SubCategoryID);
                            objListTemplatePosts.Add(objTemplatePostViewModel);
                        }
                    }
                }
                objGroupPanelConfigurationVM.ListGroupTemplatePost = objListTemplatePosts;
            }
            return objGroupPanelConfigurationVM;
        }        

        private List<PostViewModel> GetSelectListPostViewModel(List<Post> listPostEntities, 
            int groupConfigID, EnumCurrency currency)
        {
            List<PostViewModel> objListPostVM = new List<PostViewModel>();
            PostViewModel objPostVM;
            foreach (var postEntity in listPostEntities.ToList())
            {
                objPostVM = new PostViewModel(currency);
                _PostMappingService.MapPostEntityToPostViewModelSelectGroupConfig(postEntity, objPostVM);
                objPostVM.GroupPanelConfigID = groupConfigID;
                objListPostVM.Add(objPostVM);
            }
            return objListPostVM.ToList();
        }

        private List<PostViewModel> GetListPostViewModel(List<Post> listPostEntities, int groupConfigID,
            EnumCurrency currency)
        {
            List<PostViewModel> objListPostVM = new List<PostViewModel>();
            PostViewModel objPostVM;
            foreach (var postEntity in listPostEntities.ToList())
            {
                objPostVM = new PostViewModel(currency);
                _PostMappingService.MapPostEntityToPostViewModelForEdit(postEntity, objPostVM);
                objPostVM.GroupPanelConfigID = groupConfigID;
                objListPostVM.Add(objPostVM);
            }
            return objListPostVM.ToList();
        } 

        //private List<GroupPanelPost> GetGroupPanelPosts(GroupPanelConfig singleConfigEntity, int timeSlot)
        //{
        //    var listGroupPostEntity = singleConfigEntity.ListPanelPost.Where(a => a.IsActive).ToList();
        //    if (singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.VideoComment ||
        //        singleConfigEntity.EnumPanelDisplayStyle == EnumPanelDisplayStyle.VideoPost)
        //    {
        //        return listGroupPostEntity.OrderBy(a => a.DisplayOrder).ToList();
        //    }
        //    int colCount = GetColumnCount(singleConfigEntity.Column);
        //    int? rowCount = singleConfigEntity.NoOfRows;
        //    int totalPosts = colCount * (rowCount.HasValue ? rowCount.Value : 1);
        //    int skipCount = GetSkipCount(timeSlot, colCount);
        //    int length = listGroupPostEntity.ToList().Count;
        //    int startPosition = skipCount == 0 ? 0 : skipCount;
        //    if (colCount + startPosition < length)
        //    {
        //        return listGroupPostEntity.GetRange(startPosition, colCount).OrderBy(a => a.DisplayOrder).ToList();
        //    }
        //    else if (colCount <= length)
        //    {
        //        startPosition = length - colCount;
        //        return listGroupPostEntity.GetRange(startPosition, colCount).OrderBy(a => a.DisplayOrder).ToList();
        //    }
        //    else
        //    {
        //        return listGroupPostEntity.Where(a => a.IsActive).OrderBy(a => a.DisplayOrder).ToList();
        //    }
        //}

        private int GetSkipCount(int timeSlot, int colCount)
        {
            return (colCount * (timeSlot - 1));
        }

        private int GetColumnCount(EnumColumn? column)
        {
            if (!column.HasValue)
                return 1;
            if (column == EnumColumn.One)
            {
                return 1;
            }
            else if (column == EnumColumn.Two)
            {
                return 2;
            }
            else if (column == EnumColumn.Three)
            {
                return 3;
            }
            else if (column == EnumColumn.Four)
            {
                return 4;
            }
            return 1;
        }

        public async Task<bool> AddGroupPanelConfig(GroupPanelConfigurationViewModel objGroupPanelVM, 
            long currentUserID, EnumCountry country)
        {
            if (objGroupPanelVM == null)
                return false;
            GroupPanelConfig objGroupEntity = new GroupPanelConfig(
                objGroupPanelVM.Order,
                objGroupPanelVM.ShowOrHide,
                objGroupPanelVM.Device,
                EnumGroupPanelStatus.Saved,
                currentUserID,
                country)
            {
                EnumPanelDisplayStyle = objGroupPanelVM.EnumPanelDisplayStyle,
                GroupPanelTitle = objGroupPanelVM.GroupPanelTitle,
                GroupPanelTitleBangla = objGroupPanelVM.GroupPanelTitleBangla,
                EnumPublicPage = objGroupPanelVM.PublicPage,
                PanelConfigUserID = objGroupPanelVM.PanelConfigUserID
            };

            if (objGroupPanelVM.EnumPanelDisplayStyle == EnumPanelDisplayStyle.AllFixedButtons ||
                objGroupPanelVM.EnumPanelDisplayStyle == EnumPanelDisplayStyle.MarketPanel)
            {               
                objGroupEntity.PanelConfigUserID = null;
            }
           
            var result = await _GroupPanelConfigRepository.AddGroupPanelConfig(objGroupEntity, currentUserID, country);
            return true;
        }

        public async Task<bool> UpdateGroupPanelConfig(GroupPanelConfigurationViewModel objGroupPanelVM, 
            EnumCountry country, 
            long currentUserID)
        {
            var listAllConfigEntities = await _GroupPanelConfigRepository.GetAllGroupPanelConfig(country);
            var singleConfigEntity = listAllConfigEntities.FirstOrDefault(a => a.GroupPanelConfigID == objGroupPanelVM.GroupPanelConfigID);
            if(objGroupPanelVM.EnumPanelDisplayStyle == EnumPanelDisplayStyle.AllFixedButtons ||
                objGroupPanelVM.EnumPanelDisplayStyle == EnumPanelDisplayStyle.MarketPanel)
            {
                singleConfigEntity.ShowOrHide = (EnumShowOrHide)objGroupPanelVM.ShowOrHide;
                singleConfigEntity.Device = (EnumDeviceType)objGroupPanelVM.Device;
                singleConfigEntity.EnumPanelDisplayStyle = objGroupPanelVM.EnumPanelDisplayStyle;
                singleConfigEntity.GroupPanelTitle = objGroupPanelVM.GroupPanelTitle;
                singleConfigEntity.EnumPublicPage = objGroupPanelVM.PublicPage;
            }
            else
            {
                singleConfigEntity.ShowOrHide = (EnumShowOrHide)objGroupPanelVM.ShowOrHide;
                singleConfigEntity.Device = (EnumDeviceType)objGroupPanelVM.Device;
                singleConfigEntity.EnumPanelDisplayStyle = objGroupPanelVM.EnumPanelDisplayStyle;
                singleConfigEntity.GroupPanelTitle = objGroupPanelVM.GroupPanelTitle;
                singleConfigEntity.EnumPublicPage = objGroupPanelVM.PublicPage;
            }
            var result = await _GroupPanelConfigRepository.UpdateGroupPanelConfig(singleConfigEntity, 
                currentUserID, 
                country);
            return true;
        }

        public async Task<bool> DeleteGroupPanelConfig(int id, long currentUserID, EnumCountry country)
        {
            var result = await _GroupPanelConfigRepository.DeleteGroupPanelConfig(id, currentUserID, country);
            return result;
        }

        public async Task<bool> AddSelectedPost(int postID, int groupConfigID, long fileId, EnumCountry country, long currentUserID)
        {
            var listAllConfigEntities = await _GroupPanelConfigRepository.GetAllGroupPanelConfig(country);
            var singleConfigEntity = listAllConfigEntities.FirstOrDefault(a => a.GroupPanelConfigID == groupConfigID);
            var singlePostEntity = await _PostRepository.GetPostByPostID(postID);
            File objFileEntity = null;
            if (fileId != 0)
            {
                objFileEntity = singlePostEntity.ImageFiles.FirstOrDefault(a => a.FileID == fileId);
                if (objFileEntity == null)
                {
                    objFileEntity = singlePostEntity.ImageFiles.FirstOrDefault(a => a.IsActive && a.Image != null);
                }
            }
            else
            {
                objFileEntity = singlePostEntity.ImageFiles.FirstOrDefault(a => a.IsActive && a.Image != null);
            }
            if (singlePostEntity != null && singleConfigEntity != null)
            {
                var orderNumber = await _PanelPostRepository.GetNextOrderNumber(singleConfigEntity.GroupPanelConfigID);
                GroupPanelPost objGroupPanelPost = new GroupPanelPost(
                                                        orderNumber,
                                                        singleConfigEntity,
                                                        singlePostEntity,
                                                        objFileEntity,
                                                        EnumSelectionType.AdminSelected, currentUserID, country);
                objGroupPanelPost.SetSelectionDate(country);
                var saveResult = _PanelPostRepository.AddNewPanelPost(objGroupPanelPost, currentUserID);
            }
            return true;
        }

        public async Task<bool> RemoveSelectedPost(int groupPostID, long currentUserID, EnumCountry country)
        {
            var panelPostEntity = await _PanelPostRepository.GetSinglePanelPost(groupPostID);
            if (panelPostEntity != null)
            {
                panelPostEntity.SetRemovalDate(country);
                panelPostEntity.MarkPostRemoved();
                panelPostEntity.UpdateModifiedDate(currentUserID, country);
                var result = await _PanelPostRepository.SaveChanges();
            }
            return true;
        }

        public async Task<bool> PublishAllGroupPanelConfig(EnumDeviceType device, 
            EnumPublicPage page, 
            long currentUserID,
            EnumCountry country)
        {
            var result = await _GroupPanelConfigRepository.PublishGroupPanelConfig(device, page, currentUserID, country);
            return true;
        }

        public async Task<bool> UpdateDisplayOrder(PanelDisplayOrderViewModel objNewDisplayOrder, 
            EnumCountry country, 
            long currentUserID)
        {
            var listConfigs = await _GroupPanelConfigRepository.GetAllGroupPanelConfig(country);
            MapForConfigDispalyOrderUpdate(listConfigs, objNewDisplayOrder.GroupList, 5000, currentUserID, country);
            MapForConfigDispalyOrderUpdate(listConfigs, objNewDisplayOrder.AllConfigList, 1, currentUserID, country);

            var result = await _GroupPanelConfigRepository.SaveChanges();
            return true;
        }

        public async Task<bool> UpdatePostDisplayOrder(List<int> listGroupPosts, 
            long currentUserID,
            EnumCountry country)
        {
            var listConfigs = await _PanelPostRepository.GetAllGroupPosts();
            int i = 1;
            foreach (var item in listGroupPosts)
            {
                var groupPost = listConfigs.FirstOrDefault(a => a.GroupPostID == item);
                if (groupPost != null)
                {
                    groupPost.DisplayOrder = i;
                    groupPost.UpdateModifiedDate(currentUserID, country);
                    i++;
                }
            }

            var result = await _PanelPostRepository.SaveChanges();
            return true;
        }

        private void MapForConfigDispalyOrderUpdate(List<GroupPanelConfig> listConfigs, 
            List<int> allConfigOrderList, 
            int startOrder, 
            long currentUserID,
            EnumCountry country)
        {
            int i = startOrder;
            if (allConfigOrderList != null && allConfigOrderList.Count > 0)
            {
                foreach (var item in allConfigOrderList.ToList())
                {
                    var config = listConfigs.FirstOrDefault(a => a.GroupPanelConfigID == item);
                    if (config != null)
                    {
                        config.Order = i;
                        config.UpdateModifiedDate(currentUserID, country);
                        i++;
                    }
                }
            }
        }

        public async Task<FabiaInformationViewModel> LoadFabiaButtonInformation()
        {
            FabiaInformationViewModel objModel = new FabiaInformationViewModel();
            var listFabiaServices = await _PostManagementService.GetAllFabiaPosts(EnumCountry.Bangladesh, EnumPostType.FabiaService);
            var listAlphabets = listFabiaServices
                .DistinctBy(e => e.Text.Substring(0, 1).ToUpper())
                .ToList()
                .Select(a => new SelectListItem
                {
                    Text = a.Text.Substring(0, 1).ToUpper(),
                    Value = a.Value.ToString()
                }).ToList();
            objModel.ListAlphabet = listAlphabets;
            objModel.ListFabiaServices = listFabiaServices;
            objModel.ListFullAlphabet = GetFullAlphabetSet();
            List<CategoryButtonViewModel> listTopCategories = new List<CategoryButtonViewModel>();
            CategoryButtonViewModel objButton;
            foreach (var item in BusinessObjectSeed.GetAllCategoryList())
            {
                objButton = new CategoryButtonViewModel();
                objButton.CategoryID = item.ValueID;
                objButton.CategoryName = BusinessObjectSeed.GetCateSubCategoryItemText(item.ValueID);
                objButton.IconClass = BusinessObjectSeed.GetCategoryCSS(item.ValueID);
                listTopCategories.Add(objButton);
            }
            objModel.ListTopCategories = listTopCategories;
            var subCategoryList = BusinessObjectSeed.GetAllSubCategoryList();
            objModel.ListCustomButtonSubCategories = GetSpecialButtonList(subCategoryList);
            return objModel;
        }

        private List<CategoryButtonViewModel> GetSpecialButtonList(List<AValueModel> subCategoryList)
        {
            List<CategoryButtonViewModel> listTopCategories = new List<CategoryButtonViewModel>();
            CategoryButtonViewModel objButton;
            foreach (var item in subCategoryList)
            {
                if (item.ValueID == (long)EnumSpecialMarket.WomenDresses ||
                    item.ValueID == (long)EnumSpecialMarket.WomenFashionAccessories ||
                    item.ValueID == (long)EnumSpecialMarket.WomenShoes ||
                    item.ValueID == (long)EnumSpecialMarket.WomenWatches ||
                    item.ValueID == (long)EnumSpecialMarket.WomenGolds ||
                    item.ValueID == (long)EnumSpecialMarket.WomenSpecticals ||
                    item.ValueID == (long)EnumSpecialMarket.WomensSportswear)
                {

                    objButton = new CategoryButtonViewModel();
                    objButton.SubCategoryID = item.ValueID;
                    objButton.SubCategoryName = item.Text;
                    objButton.IconClass = BusinessObjectSeed.GetCategoryCSS(item.ParentValueID);
                    objButton.CustomButtonItemType = EnumCustomButtonItemType.Women;
                    listTopCategories.Add(objButton);
                }
                else if (item.ValueID == (long)EnumSpecialMarket.MenDresses ||
                    item.ValueID == (long)EnumSpecialMarket.MenFashionAccessories ||
                    item.ValueID == (long)EnumSpecialMarket.MenShoes ||
                    item.ValueID == (long)EnumSpecialMarket.MensSportswear ||
                    item.ValueID == (long)EnumSpecialMarket.MenWaletBags ||
                    item.ValueID == (long)EnumSpecialMarket.MenWatches)
                {

                    objButton = new CategoryButtonViewModel();
                    objButton.SubCategoryID = item.ValueID;
                    objButton.SubCategoryName = item.Text;
                    objButton.IconClass = BusinessObjectSeed.GetCategoryCSS(item.ParentValueID);
                    objButton.CustomButtonItemType = EnumCustomButtonItemType.Men;
                    listTopCategories.Add(objButton);
                }
                else if (item.ValueID == (long)EnumSpecialMarket.BabyDiapers ||
                    item.ValueID == (long)EnumSpecialMarket.BabyGears ||
                    item.ValueID == (long)EnumSpecialMarket.ToysGames
                )
                {

                    objButton = new CategoryButtonViewModel();
                    objButton.SubCategoryID = item.ValueID;
                    objButton.SubCategoryName = item.Text;
                    objButton.IconClass = BusinessObjectSeed.GetCategoryCSS(item.ParentValueID);
                    objButton.CustomButtonItemType = EnumCustomButtonItemType.Kids;
                    listTopCategories.Add(objButton);
                }
            }

            return listTopCategories;
        }        

        private List<SelectListItem> GetFullAlphabetSet()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            objList.Add(new SelectListItem() { Value = "1", Text = "A" });
            objList.Add(new SelectListItem() { Value = "2", Text = "B" });
            objList.Add(new SelectListItem() { Value = "3", Text = "C" });
            objList.Add(new SelectListItem() { Value = "4", Text = "D" });
            objList.Add(new SelectListItem() { Value = "5", Text = "E" });
            objList.Add(new SelectListItem() { Value = "6", Text = "F" });
            objList.Add(new SelectListItem() { Value = "7", Text = "G" });
            objList.Add(new SelectListItem() { Value = "8", Text = "H" });
            objList.Add(new SelectListItem() { Value = "9", Text = "I" });
            objList.Add(new SelectListItem() { Value = "10", Text = "J" });
            objList.Add(new SelectListItem() { Value = "11", Text = "K" });
            objList.Add(new SelectListItem() { Value = "12", Text = "L" });
            objList.Add(new SelectListItem() { Value = "13", Text = "M" });
            objList.Add(new SelectListItem() { Value = "14", Text = "N" });
            objList.Add(new SelectListItem() { Value = "15", Text = "O" });
            objList.Add(new SelectListItem() { Value = "16", Text = "P" });
            objList.Add(new SelectListItem() { Value = "17", Text = "Q" });
            objList.Add(new SelectListItem() { Value = "18", Text = "R" });
            objList.Add(new SelectListItem() { Value = "19", Text = "S" });
            objList.Add(new SelectListItem() { Value = "20", Text = "T" });
            objList.Add(new SelectListItem() { Value = "21", Text = "U" });
            objList.Add(new SelectListItem() { Value = "22", Text = "V" });
            objList.Add(new SelectListItem() { Value = "23", Text = "W" });
            objList.Add(new SelectListItem() { Value = "24", Text = "X" });
            objList.Add(new SelectListItem() { Value = "25", Text = "Y" });
            objList.Add(new SelectListItem() { Value = "26", Text = "Z" });
            return objList;
        }
    }
}
