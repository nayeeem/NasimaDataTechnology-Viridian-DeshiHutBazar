using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;
using Model;

namespace WebDeshiHutBazar
{
    public class UserSessionLogController : BaseController
    {
        public readonly ILoggingService _LoggingService;

        public UserSessionLogController(ILoggingService loggingService)
        {
            _LoggingService = loggingService;
        }

        public UserSessionLogController()
        { }

        [HttpPost]
        public async Task<JsonResult> LogSession(UserSessionViewModel sessionModel)
        {
            try 
            {
                var userSessionEntity = GetUserSessionObject(sessionModel);
                var result = await _LoggingService.LogUserSession(userSessionEntity);
                return Json("OK");
            }
            catch (Exception ex){
                var msg = ex.Message;
                return Json("FAILED");
            }
        }

        private LogUserSession GetUserSessionObject(UserSessionViewModel sessionModel)
        {
            LogUserSession userSession = new LogUserSession(COUNTRY_CODE)
            {
                BrowserWidth = sessionModel.BrowserWidth,
                BrowserHeight = sessionModel.BrowserHeight,
                ElementId = sessionModel.ElementId,
                ElementTagName = sessionModel.ElementTagName,
                ElementClass = sessionModel.ElementClass,
                TargetUrl = sessionModel.TargetUrl,
                EnumCountry = COUNTRY_CODE,
                BrowserLogId = (long?)GetBrowserId(),
                ActiveUrl = sessionModel.ActiveUrl
                
            };
            userSession.AddPositions(sessionModel.ListMousePosition);
            return userSession;
        }
    }
}