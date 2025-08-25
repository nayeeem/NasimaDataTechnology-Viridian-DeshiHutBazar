using System.Web.Mvc;

namespace WebDeshiHutBazar
{
    public class LetItGoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LetItGo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LetItGo_default",
                "LetItGo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}