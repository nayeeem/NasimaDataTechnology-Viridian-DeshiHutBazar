
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.WebPages;


namespace WebDeshiHutBazar
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Phone")
            {
                ContextCondition = (context =>
                ((context.GetOverriddenUserAgent() != null) &&
                ((context.GetOverriddenUserAgent().IndexOf("iPhone", StringComparison.OrdinalIgnoreCase) >= 0) ||
                (context.GetOverriddenUserAgent().IndexOf("iPod", StringComparison.OrdinalIgnoreCase) >= 0) ||
                (context.GetOverriddenUserAgent().IndexOf("Droid", StringComparison.OrdinalIgnoreCase) >= 0) ||
                (context.GetOverriddenUserAgent().IndexOf("Blackberry", StringComparison.OrdinalIgnoreCase) >= 0) ||
                (context.GetOverriddenUserAgent().StartsWith("Blackberry", StringComparison.OrdinalIgnoreCase)))))
            });
            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Tablet")
            {
                ContextCondition = (context => ((context.GetOverriddenUserAgent() != null) &&
              ((context.GetOverriddenUserAgent().IndexOf("iPad", StringComparison.OrdinalIgnoreCase) >= 0) ||
              (context.GetOverriddenUserAgent().IndexOf("Playbook", StringComparison.OrdinalIgnoreCase) >= 0) ||
              (context.GetOverriddenUserAgent().IndexOf("Transformer", StringComparison.OrdinalIgnoreCase) >= 0) ||
              (context.GetOverriddenUserAgent().IndexOf("Xoom", StringComparison.OrdinalIgnoreCase) >= 0))))
            });
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            ILoggingService _LogService = new LoggingService();
            _LogService.AddServerErrorLog(exception);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var roles = authTicket.UserData.Split(',');
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), roles);
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpCookie cookei = HttpContext.Current.Request.Cookies["Language"];
            if (cookei != null && cookei.Value != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookei.Value);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookei.Value);
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ng");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ng");
            }
        }
    }
}
