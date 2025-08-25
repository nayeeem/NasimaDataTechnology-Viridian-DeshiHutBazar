using System.Collections.Generic;
using Data;
using System.Web.Mvc;
using System;
using Common;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.IO;
using System.Text;
using System.Web.UI;

namespace WebDeshiHutBazar
{
    public partial class BaseController : Controller
    {
        public IEmailNotificationService _EmailService;
        public readonly KeyValuePair<int, EnumPublicPage> CurrentPage;
        public int MARKET_PAGE_SIZE = Convert.ToInt32(ConfigurationManager.AppSettings["MarketPageSize"]);
        public int MANAGE_POST_PAGE_SIZE = Convert.ToInt32(ConfigurationManager.AppSettings["ManagePostPageSize"]);
        public int POST_VALID_DAYS = Convert.ToInt32(ConfigurationManager.AppSettings["PostValidDays"]);
        public int POST_IMAGE_SIZE = Convert.ToInt32(ConfigurationManager.AppSettings["PostImageSize"]);
        public int TIME_SLOT_SELECT_STYLE = Convert.ToInt32(ConfigurationManager.AppSettings["TimeSlotStyle"]);
        public int CURRENT_TIME_SLOT = 1;
        public string SERVER_URL = ConfigurationManager.AppSettings["ServerUrl"];
        public EnumCountry COUNTRY_CODE = (EnumCountry)Convert.ToInt32(ConfigurationManager.AppSettings["CountryCode"]);
        public EnumCurrency CURRENCY_CODE = (EnumCurrency)Convert.ToInt32(ConfigurationManager.AppSettings["CurrencyCode"]);
        public string PRODUCT_CART_AGENT_PHONE_NUMBER = ConfigurationManager.AppSettings["ProductCartAgentPhoneNumber"];
        public double BKASH_TRANSECTION_COMMISSION_PERCENT = Convert.ToDouble(ConfigurationManager.AppSettings["BkashPaymentGatewayCommissionPercent"]);

        public BaseController()
        {
            if (TIME_SLOT_SELECT_STYLE == 1)
            {
                CURRENT_TIME_SLOT = GetCurrentTimeBasedTimeSlot();
            }
            else if (TIME_SLOT_SELECT_STYLE == 2)
            {
                CURRENT_TIME_SLOT = GetRandomNumberBasedTimeSlot();
            }
        }

        public DateTime GetBangladeshCurrentDateTime()
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            return BaTime;
        }

        public int GetRandomNumberBasedTimeSlot()
        {
            Random random = new Random();
            return random.Next(1, 4);
        }

        public int GetCurrentTimeBasedTimeSlot()
        {
            var currentTime = GetBangladeshCurrentDateTime();
            DateTime now = GetBangladeshCurrentDateTime();
            DateTime baseDate = GetBangladeshCurrentDateTime();

            TimeSpan ts = new TimeSpan(00, 00, 0);
            var slot4 = baseDate.Date + ts;
            ts = new TimeSpan(00, 59, 0);
            var slot5 = baseDate.Date + ts;
            if (now >= slot4 && now <= slot5)
                return 1;

            ts = new TimeSpan(01, 00, 0);
            var slot6 = baseDate.Date + ts;
            ts = new TimeSpan(01, 59, 0);
            var slot7 = baseDate.Date + ts;
            if (now >= slot6 && now <= slot7)
                return 2;

            ts = new TimeSpan(02, 00, 0);
            var slot8 = baseDate.Date + ts;
            ts = new TimeSpan(02, 59, 0);
            var slot9 = baseDate.Date + ts;
            if (now >= slot8 && now <= slot9)
                return 3;

            ts = new TimeSpan(03, 00, 0);
            var slot10 = baseDate.Date + ts;
            ts = new TimeSpan(03, 59, 0);
            var slot11 = baseDate.Date + ts;
            if (now >= slot10 && now <= slot11)
                return 4;

            ts = new TimeSpan(04, 00, 0);
            var slot12 = baseDate.Date + ts;
            ts = new TimeSpan(04, 59, 0);
            var slot13 = baseDate.Date + ts;
            if (now >= slot12 && now <= slot13)
                return 5;

            ts = new TimeSpan(05, 00, 0);
            var slot14 = baseDate.Date + ts;
            ts = new TimeSpan(05, 59, 0);
            var slot15 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 6;

            ts = new TimeSpan(06, 00, 0);
            var slot16 = baseDate.Date + ts;
            ts = new TimeSpan(06, 59, 0);
            var slot17 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 1;

            ts = new TimeSpan(07, 00, 0);
            var slot18 = baseDate.Date + ts;
            ts = new TimeSpan(07, 59, 0);
            var slot19 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 2;

            ts = new TimeSpan(08, 00, 0);
            var slot20 = baseDate.Date + ts;
            ts = new TimeSpan(08, 59, 0);
            var slot21 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 3;

            ts = new TimeSpan(09, 00, 0);
            var slot22 = baseDate.Date + ts;
            ts = new TimeSpan(09, 59, 0);
            var slot23 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 4;

            ts = new TimeSpan(10, 00, 0);
            var slot24 = baseDate.Date + ts;
            ts = new TimeSpan(10, 59, 0);
            var slot25 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 5;

            ts = new TimeSpan(11, 00, 0);
            var slot26 = baseDate.Date + ts;
            ts = new TimeSpan(11, 59, 0);
            var slot27 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 6;

            ts = new TimeSpan(12, 00, 0);
            var slot28 = baseDate.Date + ts;
            ts = new TimeSpan(12, 59, 0);
            var slot29 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 1;

            ts = new TimeSpan(13, 00, 0);
            var slot30 = baseDate.Date + ts;
            ts = new TimeSpan(13, 59, 0);
            var slot31 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 2;

            ts = new TimeSpan(14, 00, 0);
            var slot32 = baseDate.Date + ts;
            ts = new TimeSpan(14, 59, 0);
            var slot33 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 3;

            ts = new TimeSpan(15, 00, 0);
            var slot34 = baseDate.Date + ts;
            ts = new TimeSpan(15, 59, 0);
            var slot35 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 4;

            ts = new TimeSpan(16, 00, 0);
            var slot36 = baseDate.Date + ts;
            ts = new TimeSpan(16, 59, 0);
            var slot37 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 5;

            ts = new TimeSpan(17, 00, 0);
            var slot38 = baseDate.Date + ts;
            ts = new TimeSpan(17, 59, 0);
            var slot39 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 6;

            ts = new TimeSpan(18, 00, 0);
            var slot40 = baseDate.Date + ts;
            ts = new TimeSpan(18, 59, 0);
            var slot41 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 1;

            ts = new TimeSpan(19, 00, 0);
            var slot42 = baseDate.Date + ts;
            ts = new TimeSpan(19, 59, 0);
            var slot43 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 2;

            ts = new TimeSpan(20, 00, 0);
            var slot44 = baseDate.Date + ts;
            ts = new TimeSpan(20, 59, 0);
            var slot45 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 3;

            ts = new TimeSpan(21, 00, 0);
            var slot46 = baseDate.Date + ts;
            ts = new TimeSpan(21, 59, 0);
            var slot47 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 4;

            ts = new TimeSpan(22, 00, 0);
            var slot48 = baseDate.Date + ts;
            ts = new TimeSpan(22, 59, 0);
            var slot49 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 5;

            ts = new TimeSpan(23, 00, 0);
            var slot50 = baseDate.Date + ts;
            ts = new TimeSpan(23, 59, 0);
            var slot51 = baseDate.Date + ts;
            if (now >= slot14 && now <= slot15)
                return 6;

            return 1;
        }

        public ActionResult UnwantedAccessError()
        {
            ClearSessionUser();
            ClearTempSessionUserId();
            ViewBag.IsLogout = true;
            return View("UnwantedAccessError");
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    filterContext.ExceptionHandled = true;

        //    ILogServerErrorRepository _Logger = new LogServerErrorRepository();
        //    _Logger.AddServerErrorLog(filterContext.Exception);

        //    filterContext.Result = RedirectToAction("Index", "ErrorHandler");
        //    filterContext.Result = new ViewResult
        //    {
        //        ViewName = "~/Views/ErrorHandler/Index.cshtml"
        //    };
        //}

        public ActionResult CheckLogoutRequirements()
        {
            LogoutAndClear();
            return RedirectToAction("Index", "Home", new { isLogout = true });
        }

        public void LogoutAndClear()
        {
            FormsAuthentication.SignOut();
            ClearAdminSessionUser();
            ClearSessionUser();
            ClearTempSessionUserId();

            HttpCookie currentUserCookie = HttpContext.Request.Cookies["ClientName"];
            HttpContext.Response.Cookies.Remove("ClientName");
            currentUserCookie.Expires = DateTime.Now.AddDays(-10);
            currentUserCookie.Value = null;
            HttpContext.Response.SetCookie(currentUserCookie);

            HttpCookie loginUserIDCookie = HttpContext.Request.Cookies["LoginUserID"];
            HttpContext.Response.Cookies.Remove("LoginUserID");
            loginUserIDCookie.Expires = DateTime.Now.AddDays(-10);
            loginUserIDCookie.Value = null;
            HttpContext.Response.SetCookie(loginUserIDCookie);
        }        

        public string RenderPartialToString(Controller controller, string partialViewName, EmailViewModel model, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            ViewEngineResult result = ViewEngines.Engines.FindPartialView(this.ControllerContext, @"../../Views/Shared/EmailTemplates/" + partialViewName);
            if (result.View != null)
            {
                controller.ViewData.Model = model;
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    using (HtmlTextWriter output = new HtmlTextWriter(sw))
                    {
                        ViewContext viewContext = new ViewContext(controller.ControllerContext, result.View, viewData, tempData, output);
                        result.View.Render(viewContext, output);
                    }
                }

                return sb.ToString();
            }

            return String.Empty;
        }
    }
}