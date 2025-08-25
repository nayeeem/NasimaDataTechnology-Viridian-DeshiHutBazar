using System.Collections.Generic;
using System.Web.Mvc;
using Common;

namespace WebDeshiHutBazar
{   
    public class ErrorHandlerController : BaseController
    {        
        public ErrorHandlerController() 
        { }
       
        public ActionResult Index(bool? isLogout)
        {
            return View();
        }
    }
}
