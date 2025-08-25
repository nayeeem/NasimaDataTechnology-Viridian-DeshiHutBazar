using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Common;


namespace WebDeshiHutBazar
{
    public class CultureController : BaseController
    {
        public CultureController() { }

        [HttpGet]
        public JsonResult Change(string LanguageAbbrevation)
        {
            if (LanguageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);
            }

            HttpCookie cookei = new HttpCookie("Language");
            cookei.Value = LanguageAbbrevation;
            Response.Cookies.Add(cookei);

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}