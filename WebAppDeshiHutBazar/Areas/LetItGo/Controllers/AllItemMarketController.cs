using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using System;
using System.Net.Mail;
using System.Net;

namespace WebDeshiHutBazar
{
    public class AllItemMarketController : PageingController
    {
        List<PostViewModel> listPostVM;
        private readonly IGroupPanelConfigService _GroupPanelConfigService;
        private readonly ILoggingService _LoggingService;
        private readonly IPostMangementService _PostMangementService;
        private readonly IPostMappingService _PostMappingService;
        private readonly IPostVisitService _PostVisitService;

        public AllItemMarketController()
        { }

        public AllItemMarketController(
            ILoggingService loggingService,
            IGroupPanelConfigService groupPanelConfigService,
            IPostMangementService postMangementService,
            IPostMappingService postMappingService,
            IPostVisitService postVisitService,
            IEmailNotificationService emailService
            )
        {
            _LoggingService = loggingService;
            _GroupPanelConfigService = groupPanelConfigService;
            _PostMangementService = postMangementService;
            _PostMappingService = postMappingService;
            listPostVM = new List<PostViewModel>();
            _PostVisitService = postVisitService;
            _EmailService = emailService;
        }

        [OutputCache(CacheProfile = "Cache60Mins")]
        public async Task<ViewResult> Market(int? pageNumber)
        {
            if (!pageNumber.HasValue)
                pageNumber = 1;
            var res = _LoggingService.LogEntirePageVisit(EnumLogType.AllItemMarketLink, COUNTRY_CODE, HttpContext.Session.SessionID);
            MarketInfoViewModel objMarketInfoModel = new MarketInfoViewModel(CURRENCY_CODE)
            {
                PostViewModel = new PostViewModel(CURRENCY_CODE)
                {
                    AV_State = DropDownDataList.GetAllStateList(),
                    AV_Category = DropDownDataList.GetCategoryList(),
                    AV_SubCategory = DropDownDataList.GetSubCategoryList()
                },
                CategorySearchInfoModel = new CategorySearchInfoModel()
                {
                    PageLocation = "AllMarket"
                },
                PageName = "Today's Latest Ads"
            };
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
            var viewMoreUrl = Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" });
            objMarketInfoModel.ContentInfoViewModel.ListGroupPanelConfiguration =
                                     await _GroupPanelConfigService.GetAllPageGroupPanelConfigurations(
                                         EnumPublicPage.AllMarket,
                                         viewMoreUrl,
                                         viewPostDetUrl,
                                         COUNTRY_CODE,
                                         CURRENT_TIME_SLOT,
                                         EnumMarketType.AllItems, 
                                         0, pageNumber, MARKET_PAGE_SIZE,null,
                                         CURRENCY_CODE);
            var singleMarketConfigList = objMarketInfoModel.ContentInfoViewModel.ListGroupPanelConfiguration.Where(a => a.EnumPanelDisplayStyle == EnumPanelDisplayStyle.MarketPanel).ToList();
            if (singleMarketConfigList != null)
            {
                foreach (var listItem in singleMarketConfigList)
                {
                    if (listItem.EnumPublicPage == EnumPublicPage.AllMarket)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber.Value, MARKET_PAGE_SIZE, "Market", "AllItemMarket", null, null);
                    }
                    else if (listItem.EnumPublicPage == EnumPublicPage.CategoryButtonMarket)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber.Value, MARKET_PAGE_SIZE, "CatMarket", "CategoryMarket", null, null);
                    }
                    else if (listItem.EnumPublicPage == EnumPublicPage.SpecialMarketButton)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber.Value, MARKET_PAGE_SIZE, "CustomMarket", "CategoryMarket", null, null);
                    }
                    else if (listItem.EnumPublicPage == EnumPublicPage.SubCategoryDropdownMarket)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber.Value, MARKET_PAGE_SIZE, "Market", "CategoryMarket", null, null);
                    }
                }
            }            
            return View(@"../../Areas/LetItGo/Views/AllItemMarket/Market", objMarketInfoModel);
        }
               
        [OutputCache(CacheProfile = "Cache30MinsItemDetail")]
        public async Task<ViewResult> ViewItemDetail(long postId)
        {
            var res = await _LoggingService.LogPostDetailPageVisit(postId, COUNTRY_CODE, HttpContext.Session.SessionID);
            var postDisplayViewModel = await _PostMangementService.GetDisplayPostByID(postId, CURRENCY_CODE);
            postDisplayViewModel.PageName = "Item Details Page";
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
            var viewMoreUrl = Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" });
            postDisplayViewModel.ContentInfoViewModel.ListGroupPanelConfiguration =
                                     await _GroupPanelConfigService.GetAllPageGroupPanelConfigurations(
                                         EnumPublicPage.AdsDetail,
                                         viewMoreUrl,
                                         viewPostDetUrl,
                                         COUNTRY_CODE,
                                         CURRENT_TIME_SLOT,EnumMarketType.SimilarItems, 
                                         postDisplayViewModel.SubCategoryID, 
                                         0, 
                                         MARKET_PAGE_SIZE, postDisplayViewModel.Price, CURRENCY_CODE);
            try
            {
                EmailViewModel objEmailViewModel = _EmailService.GetPostViewedViewModel(postDisplayViewModel, postId);
                objEmailViewModel.MessageBodyHTMLText = RenderPartialToString(this, "_AdBrowsed", objEmailViewModel, ViewData, TempData);
                objEmailViewModel.ReceiverEmail = postDisplayViewModel.Email;
                _EmailService.SendCommentEmail(objEmailViewModel);
            }
            catch { }
            return View(@"../../Areas/LetItGo/Views/AllItemMarket/ViewItemDetail", postDisplayViewModel);
        }        
        
        [HttpGet]
        public async Task<JsonResult> AddComments(string comment, int postID)
        {
            var resultAddComments = await _PostMangementService.AddComments(comment, postID, COUNTRY_CODE);
            try
            {
                var postVM = await _PostMangementService.GetPostByPostIDForEdit(postID, CURRENCY_CODE);
                EmailViewModel objEmailViewModel = _EmailService.GetCommentViewModel(postVM, postID);
                objEmailViewModel.MessageBodyHTMLText = RenderPartialToString(
                    this, 
                    "_UserCommented", 
                    objEmailViewModel, 
                    ViewData, 
                    TempData);
                objEmailViewModel.ReceiverEmail = postVM.Email;
                _EmailService.SendCommentEmail(objEmailViewModel);
            }
            catch {
            }
            return Json(GetBangladeshCurrentDateTime().ToShortDateString(), JsonRequestBehavior.AllowGet);
        }
        
        private int GetOrderValue(long? val, string[] arr)
        {
            if(arr == null || arr.Length == 0)
            {
                return 1;
            }

            try
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (val.HasValue && arr[i] != null && val.Value == Convert.ToInt64(arr[i]))
                    {
                        return i;
                    }
                }
                return 99999;
            }
            catch
            {
                return 99999;
            }
        }

        private async Task<GroupPanelTemplateDisplayViewModel> LoadPageingSpecificPosts(GroupPanelTemplateDisplayViewModel objModel, 
            int pageNumber, 
            string action, 
            string controller, 
            string interests)
        {
            interests = interests == null ? "" : interests;
            var viewPostDetUrl = Url.Action(action, controller, new { postid = "POST_ITEM_ID" });
            listPostVM = await _PostMangementService.GetMarketAllPosts(COUNTRY_CODE, viewPostDetUrl, POST_VALID_DAYS, CURRENCY_CODE);
            listPostVM = listPostVM.OrderBy(a => GetOrderValue(a.CategoryID, interests.Split('a'))).ToList();

            objModel.PageingModelAll = SetPageingModel(
                objModel.PageingModelAll,
                objModel.ListGroupPost.Count,
                pageNumber,
                MARKET_PAGE_SIZE,
                "Market",
                "AllItemMarket",
                interests,
                null);
            return objModel;
        }

        [HttpGet]
        public async Task<JsonResult> LogPostVisit(string email, string phone, int postID, string visitaction)
        {
            if(visitaction == "PostVisit")
            {
                var resultAddComments = await _PostVisitService.SavePostVisit(postID, email, phone, EnumPostVisitAction.PostVisit);
            }
            else if (visitaction == "PostLiked")
            {
                var resultAddComments = await _PostVisitService.SavePostVisit(postID, email, phone, EnumPostVisitAction.PostLiked);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> LikeThisComment(long commentID, string actionType)
        {
            var res = await _PostMangementService.LikeThisComment(commentID, actionType);
            return Json(true, JsonRequestBehavior.AllowGet);
        }       
    }
}