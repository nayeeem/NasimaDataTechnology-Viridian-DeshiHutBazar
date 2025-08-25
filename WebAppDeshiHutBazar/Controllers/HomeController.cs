using System.Web.Mvc;
using System.Collections.Generic;
using Common;
using System.Net.Mail;
using System.Net;
using System;
using System.Threading.Tasks;
using Model;
using Microsoft.Ajax.Utilities;
using System.Linq;
using Data;

namespace WebDeshiHutBazar
{    
    public class HomeController : BaseController
    {
        private readonly IGroupPanelConfigService _GroupPanelConfigService;
        private readonly ILoggingService _LoggingService;
        private readonly IPostMangementService _PostMangementService;
        private readonly IPostMappingService _PostMappingService;
        private readonly ILoggingService _ILogPostService;

        public HomeController(
            IGroupPanelConfigService groupPanelConfigService,
            ILoggingService loggingService,
            IPostMangementService postMangementService,
            IPostMappingService postMappingService,
            ILoggingService logPostService,
            IEmailNotificationService emailNotification
            ) 
        {
            _GroupPanelConfigService = groupPanelConfigService;
            _LoggingService = loggingService;
            _PostMangementService = postMangementService;
            _PostMappingService = postMappingService;
            _ILogPostService = logPostService;
            _EmailService = emailNotification;
        }

        public HomeController()  { }

        [OutputCache(CacheProfile = "CacheHomePage")]
        public async Task<ActionResult> Index(bool? isLogout)
        {
            var res = await _LoggingService.LogEntirePageVisit(EnumLogType.HomePageLink, COUNTRY_CODE, HttpContext.Session.SessionID);
            CheckLogout(isLogout);
            var resultConfigList = await _GroupPanelConfigService
                                                        .GetAllPageGroupPanelConfigurations(
                                                        EnumPublicPage.Home,
                                                        Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" }),
                                                        Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" }),
                                                        COUNTRY_CODE,
                                                        CURRENT_TIME_SLOT,
                                                        CURRENCY_CODE);
            HomeViewModel objHomeModel = new HomeViewModel()
            {
                ContentInfoViewModel = new ContentInfoViewModel()
                {
                    ListGroupPanelConfiguration = resultConfigList.Where(a =>
                    a.PublishStatus.HasValue &&
                    a.PublishStatus.Value == EnumGroupPanelStatus.Published &&
                    a.ShowOrHide.HasValue &&
                    a.ShowOrHide == EnumShowOrHide.Yes).ToList()
                },
            };
            objHomeModel.PageName = "Home Page";
            return View(objHomeModel);
        }

        public async Task<ActionResult> Notice(bool? isLogout)
        {
            var res = await _LoggingService.LogEntirePageVisit(EnumLogType.NoticePage, COUNTRY_CODE, HttpContext.Session.SessionID);
            CheckLogout(isLogout);
            var resultConfigList = await _GroupPanelConfigService
                                                        .GetAllPageGroupPanelConfigurations(
                                                        EnumPublicPage.NoticeAndNews,
                                                        Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" }),
                                                        Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" }),
                                                        COUNTRY_CODE,
                                                        CURRENT_TIME_SLOT,
                                                        CURRENCY_CODE);
            HomeViewModel objHomeModel = new HomeViewModel()
            {
                ContentInfoViewModel = new ContentInfoViewModel()
                {
                    ListGroupPanelConfiguration = resultConfigList.Where(a =>
                    a.PublishStatus.HasValue &&
                    a.PublishStatus.Value == EnumGroupPanelStatus.Published &&
                    a.ShowOrHide.HasValue &&
                    a.ShowOrHide == EnumShowOrHide.Yes).ToList()
                },
            };
            objHomeModel.PageName = "Notice Page";
            return View(objHomeModel);
        }

        private void CheckLogout(bool? isLogout)
        {
            if (isLogout.HasValue && isLogout.Value)
            {
                ClearSessionUser();
                ViewBag.IsLogout = true;
            }
            else
            {
                ViewBag.IsLogout = false;
            }
        }
       
        [OutputCache(CacheProfile = "Cache1day")]
        public ActionResult About()
        {
            HomeViewModel objHomeModel = new HomeViewModel();
            ViewBag.NevigationText = "Home  >  About";
            return View(objHomeModel);
        }

        [OutputCache(CacheProfile = "Cache1day")]
        public ActionResult Contact()
        {
            ContactViewModel objHomeModel = new ContactViewModel();
            ViewBag.NevigationText = "Home  >  Contact";
            objHomeModel.AV_MessageTypeList = DropDownDataList.GetAllContactMessageType();
            objHomeModel.PageName = "Contact Us Page";
            return View(objHomeModel);
        }

        [OutputCache(CacheProfile = "Cache1day")]
        public ActionResult FAQ()
        {
            HomeViewModel objHomeModel = new HomeViewModel();
            ViewBag.NevigationText = "Home  >  FAQ";
            objHomeModel.PageName = "FAQ Page";
            return View(objHomeModel);
        }

        [OutputCache(CacheProfile = "Cache1day")]
        public ActionResult Privacy()
        {
            HomeViewModel objHomeModel = new HomeViewModel();
            ViewBag.NevigationText = "Home  >  Privacy";
            objHomeModel.PageName = "Privacy Page";
            return View(objHomeModel);
        }

        [OutputCache(CacheProfile = "Cache1day")]
        public ActionResult Terms()
        {
            HomeViewModel objHomeModel = new HomeViewModel();
            ViewBag.NevigationText = "Home  >  Terms";
            objHomeModel.PageName = "Terms Page";
            return View(objHomeModel);
        }

        [OutputCache(CacheProfile = "Cache1day")]
        public ActionResult OurServices()
        {
            HomeViewModel objHomeModel = new HomeViewModel();
            ViewBag.NevigationText = "Home  >  Our Services";
            return View(objHomeModel);
        }

        [OutputCache(CacheProfile = "Cache1day")]
        public async Task<ActionResult> Resource()
        {
            var resultConfigList = await _GroupPanelConfigService
                                                        .GetAllPageGroupPanelConfigurations(
                                                        EnumPublicPage.Resources,
                                                        Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" }),
                                                        Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" }),
                                                        COUNTRY_CODE,
                                                        CURRENT_TIME_SLOT,
                                                        CURRENCY_CODE);
            HomeViewModel objHomeModel = new HomeViewModel()
            {
                ContentInfoViewModel = new ContentInfoViewModel()
                {
                    ListGroupPanelConfiguration = resultConfigList.Where(a =>
                    a.PublishStatus.HasValue &&
                    a.PublishStatus.Value == EnumGroupPanelStatus.Published &&
                    a.ShowOrHide.HasValue &&
                    a.ShowOrHide == EnumShowOrHide.Yes).ToList()
                },
            };
            ViewBag.NevigationText = "Home  >  Resources";
            return View(objHomeModel);
        }

        [HttpPost]
        public JsonResult SaveContactMessage(ContactViewModel objContact)
        {
            var isValid = ValidationService.IsValidEmail(objContact.Email);
            if (!isValid)
                return Json("EmailInvalid");
            try
            {
                var objEmailViewModel = _EmailService.GetContactUsViewModel(objContact);  
                objEmailViewModel.MessageBodyHTMLText = RenderPartialToString(this, "_ContactUs", objEmailViewModel, ViewData, TempData);
                _EmailService.SendContactUsEmail(objEmailViewModel);
                return Json("Success");
            }
            catch
            {
                return Json("SendFailed");
            }
        }
      
        [HttpPost]
        public async Task<JsonResult> BrowserInfo(BrowserLogViewModel objBrowserLog)
        {
            LogBrowserInfo objLog = new LogBrowserInfo()
            {
                Country = objBrowserLog.Country,
                CountryCode = objBrowserLog.CountryCode,
                City = objBrowserLog.City,
                Region = objBrowserLog.Region,
                Lon = objBrowserLog.Lon,
                Lat = objBrowserLog.Lat,
                Width = objBrowserLog.Width,
                Height = objBrowserLog.Height,
                LogDateTime = DateTime.Now,
                Zip = objBrowserLog.Zip
            };
            SetBrowserId(await _LoggingService.LogBrowserInfo(objLog));
            return Json("Success");
        }

        [HttpGet]
        public async Task<JsonResult> AddComments(string comment, int postID)
        {
            var resultAddComments = await _PostMangementService.AddComments(comment, postID, COUNTRY_CODE);
            return Json(GetBangladeshCurrentDateTime().ToShortDateString(), JsonRequestBehavior.AllowGet);
        }
    }
}
