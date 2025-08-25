using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Common;
using System.Threading.Tasks;
using Data;

namespace WebDeshiHutBazar
{
    public partial class CategoryMarketController : PageingController
    {
        private readonly IGroupPanelConfigService _GroupPanelConfigService;
        private readonly ILoggingService _LoggingService;
        private readonly IPostMangementService _PostMangementService;
        private readonly ILogPostRepository _ILogPostRepository;
        public CategoryMarketController() 
        { }

        public CategoryMarketController(ILoggingService loggingService,
            IGroupPanelConfigService groupPanelConfigService,
            IPostMangementService postMangementService,
            ILogPostRepository logPost,
            IEmailNotificationService emailService)
        {
            _LoggingService = loggingService;
            _GroupPanelConfigService = groupPanelConfigService;
            _PostMangementService = postMangementService;
            _ILogPostRepository = logPost;
            _EmailService = emailService;
        }

        [OutputCache(CacheProfile = "Cache30Mins")]
        public async Task<ViewResult> Market(long subcategoryid, int pageNumber = 1)
        {
            var res = await _LoggingService.LogSubCategoryMarketPageVisit(subcategoryid, COUNTRY_CODE, HttpContext.Session.SessionID);
            var specificMarket = BusinessObjectSeed.GetCatSubCategoryItemTextForId(subcategoryid);
            ViewBag.Title = specificMarket;
            MarketInfoViewModel objMarketInfoModel = new MarketInfoViewModel(CURRENCY_CODE)
            {
                PostViewModel = new PostViewModel(CURRENCY_CODE)
                {
                    AV_State = DropDownDataList.GetAllStateList(),
                    AV_Category = DropDownDataList.GetCategoryList(),
                    AV_SubCategory = DropDownDataList.GetSubCategoryList()
                },              
                SubCategoryID = subcategoryid,
                CategoryID = BusinessObjectSeed.GetCategoryIDForSubCategoryID(subcategoryid),
                PageName = "Today's " + specificMarket + " Market",
                DisplaySubCategory = specificMarket               
            };            
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
            var viewMoreUrl = Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" });
            objMarketInfoModel.ContentInfoViewModel.ListGroupPanelConfiguration =
                                    await _GroupPanelConfigService.GetAllPageGroupPanelConfigurations(
                                        EnumPublicPage.SubCategoryDropdownMarket,
                                        viewMoreUrl,
                                        viewPostDetUrl,
                                        COUNTRY_CODE,
                                        CURRENT_TIME_SLOT,
                                        EnumMarketType.SubCategory, 
                                        subcategoryid, pageNumber, 
                                        MARKET_PAGE_SIZE,null, CURRENCY_CODE);
            var singleMarketConfigList = objMarketInfoModel.ContentInfoViewModel.ListGroupPanelConfiguration.Where(a => a.EnumPanelDisplayStyle == EnumPanelDisplayStyle.MarketPanel).ToList();
            if (singleMarketConfigList != null)
            {
                foreach(var listItem in singleMarketConfigList)
                {
                    if (listItem.EnumPublicPage == EnumPublicPage.AllMarket)
                    {
                        listItem.PageingModelAll = SetPageingModel( listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber, MARKET_PAGE_SIZE, "Market", "AllItemMarket", null, null);
                    }
                    else if (listItem.EnumPublicPage == EnumPublicPage.CategoryButtonMarket)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber, MARKET_PAGE_SIZE, "CatMarket", "CategoryMarket", null, subcategoryid);
                    }
                    else if (listItem.EnumPublicPage == EnumPublicPage.SpecialMarketButton)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber, MARKET_PAGE_SIZE, "CustomMarket", "CategoryMarket", null, subcategoryid);
                    }
                    else if (listItem.EnumPublicPage == EnumPublicPage.SubCategoryDropdownMarket)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber, MARKET_PAGE_SIZE, "Market", "CategoryMarket", null, subcategoryid);
                    }                    
                }
            }            
            objMarketInfoModel.ContentInfoViewModel.CategoryID = objMarketInfoModel.CategoryID;
            return View(@"../../Areas/LetItGo/Views/CategoryMarket/Market", objMarketInfoModel);
        }

        [OutputCache(CacheProfile = "Cache30Mins")]
        public async Task<ViewResult> CatMarket(long subcategoryid, int pageNumber = 1)
        {
            var res = await _LoggingService.LogCategoryMarketPageVisit(subcategoryid, EnumCountry.Bangladesh, HttpContext.Session.SessionID);
            var specificMarket = BusinessObjectSeed.GetCatSubCategoryItemTextForId(subcategoryid);
            ViewBag.Title = specificMarket;
            MarketInfoViewModel objMarketInfoModel = new MarketInfoViewModel(CURRENCY_CODE)
            {
                PostViewModel = new PostViewModel(CURRENCY_CODE)
                {
                    AV_State = DropDownDataList.GetAllStateList(),
                    AV_Category = DropDownDataList.GetCategoryList(),
                    AV_SubCategory = DropDownDataList.GetSubCategoryList()
                },
                SubCategoryID = subcategoryid,
                CategoryID = BusinessObjectSeed.GetCategoryIDForSubCategoryID(subcategoryid),
                PageName = "Today's " + specificMarket + " Market",
                DisplaySubCategory = specificMarket,
                ListPostsAll = await GetCategoryBasedPostList(subcategoryid,
                "ViewItemDetail",
                "AllItemMarket")
            };            
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
            var viewMoreUrl = Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" });
            objMarketInfoModel.ContentInfoViewModel.ListGroupPanelConfiguration =
                                   await _GroupPanelConfigService.GetAllPageGroupPanelConfigurations(
                                       EnumPublicPage.CategoryButtonMarket,
                                       viewMoreUrl,
                                       viewPostDetUrl,
                                       COUNTRY_CODE,
                                       CURRENT_TIME_SLOT,
                                       EnumMarketType.Category, 
                                       subcategoryid, pageNumber, 
                                       MARKET_PAGE_SIZE,null, CURRENCY_CODE);
            var singleMarketConfigList = objMarketInfoModel.ContentInfoViewModel.ListGroupPanelConfiguration.Where(a => a.EnumPanelDisplayStyle == EnumPanelDisplayStyle.MarketPanel).ToList();
            if (singleMarketConfigList != null)
            {
                foreach (var listItem in singleMarketConfigList)
                {
                    if (listItem.EnumPublicPage == EnumPublicPage.AllMarket)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber, MARKET_PAGE_SIZE, "Market", "AllItemMarket", null, null);
                    }
                    else if (listItem.EnumPublicPage == EnumPublicPage.CategoryButtonMarket)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber, MARKET_PAGE_SIZE, "CatMarket", "CategoryMarket", null, subcategoryid);
                    }
                    else if (listItem.EnumPublicPage == EnumPublicPage.SpecialMarketButton)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber, MARKET_PAGE_SIZE, "CustomMarket", "CategoryMarket", null, subcategoryid);
                    }
                    else if (listItem.EnumPublicPage == EnumPublicPage.SubCategoryDropdownMarket)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber, MARKET_PAGE_SIZE, "Market", "CategoryMarket", null, subcategoryid);
                    }
                }
            }
            objMarketInfoModel.ContentInfoViewModel.CategoryID = objMarketInfoModel.CategoryID;
            return View(@"../../Areas/LetItGo/Views/CategoryMarket/CatMarket", objMarketInfoModel);
        }
        
        private async Task<List<PostViewModel>> GetCategoryBasedPostList(long subcategoryid, string action, string controller)
        {
            var viewPostDetUrl = Url.Action(action, controller, new { postid = "POST_ITEM_ID" });
            var listAllMarketPosts = await _PostMangementService.GetCategoryPosts(
                COUNTRY_CODE, 
                viewPostDetUrl, 
                subcategoryid,
                CURRENCY_CODE);
            return listAllMarketPosts;
        }

        private async Task<List<PostViewModel>> GetSubCategoryBasedPostList(long subcategoryid, string action, string controller)
        {
            var viewPostDetUrl = Url.Action(action, controller, new { postid = "POST_ITEM_ID" });
            var listAllMarketPosts = await _PostMangementService.GetCategoryMarketAllPosts(
                COUNTRY_CODE, viewPostDetUrl, subcategoryid, CURRENCY_CODE);
            return listAllMarketPosts;
        }
        
        private async Task<int> GetSubCategoryBasedPostListCount(long subcategoryid)
        {
            return await _PostMangementService.GetCategoryPostsCount(COUNTRY_CODE, subcategoryid);
        }
        
        public async Task<ViewResult> ViewItemDetail(long postId)
        {
            var res = await _LoggingService.LogPostDetailPageVisit(postId, COUNTRY_CODE, HttpContext.Session.SessionID);
            var postvm = await _PostMangementService.GetDisplayPostByID(postId, CURRENCY_CODE);
            postvm.PageName = "Item Details Page";
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
            var viewMoreUrl = Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" });
            postvm.ContentInfoViewModel.ListGroupPanelConfiguration =
                                     await _GroupPanelConfigService.GetAllPageGroupPanelConfigurations(
                                         EnumPublicPage.AdsDetail,
                                         viewMoreUrl,
                                         viewPostDetUrl,
                                         COUNTRY_CODE,
                                         CURRENT_TIME_SLOT, EnumMarketType.SimilarItems,
                                         postvm.SubCategoryID,
                                         0,
                                         MARKET_PAGE_SIZE, 
                                         postvm.Price,
                                         CURRENCY_CODE);
            try
            {
                EmailViewModel objEmailViewModel = _EmailService.GetPostViewedViewModel(postvm, postId);
                objEmailViewModel.MessageBodyHTMLText = RenderPartialToString(this, "_AdBrowsed", objEmailViewModel, ViewData, TempData);
                objEmailViewModel.ReceiverEmail = postvm.Email;
                _EmailService.SendCommentEmail(objEmailViewModel);
            }
            catch { }
            return View(@"../../Areas/LetItGo/Views/AllItemMarket/ViewItemDetail", postvm);
        }             
    }
}