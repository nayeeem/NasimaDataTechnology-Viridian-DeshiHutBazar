using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Data;

using System.Threading.Tasks;


using Microsoft.Ajax.Utilities;
using Model;
using Common;
using System.Web;
using System.IO;
using System;
using System.Net.Mail;
using System.Net;

namespace WebDeshiHutBazar
{
    public class RequestInformationController : BaseController
    {
        List<PostViewModel> listPostVM;
        private readonly IGroupPanelConfigService _GroupPanelConfigService;
        private readonly ILoggingService _LoggingService;
        private readonly IPostMangementService _PostMangementService;
        private readonly IPostMappingService _PostMappingService;
        private readonly IPostVisitService _PostVisitService;
        private readonly ILogPostRepository _ILogPostRepository;
        public readonly IRepoDropDownDataList _DropdownRepo;
        private readonly IUserAccountService _UserAccountService;
        private readonly IFabiaProviderService _FabiaProviderService;

       
        public RequestInformationController(ILoggingService loggingService,
            IGroupPanelConfigService groupPanelConfigService,
            IPostMangementService postMangementService,
            IPostMappingService postMappingService,
            IPostVisitService postVisitService,
            ILogPostRepository logPostRepository,
            IRepoDropDownDataList dropdownList,
            IUserAccountService userAccountService,
            IFabiaProviderService fabiaProviderService,
            IEmailNotificationService emailNotificationService
            )
        {
            _LoggingService = loggingService;
            _GroupPanelConfigService = groupPanelConfigService;
            _PostMangementService = postMangementService;
            _PostMappingService = postMappingService;
            listPostVM = new List<PostViewModel>();
            _PostVisitService = postVisitService;
            _ILogPostRepository = logPostRepository;
            _DropdownRepo = dropdownList;
            _UserAccountService = userAccountService;
            _FabiaProviderService = fabiaProviderService;
            _EmailService = emailNotificationService;
        }
               
        [HttpPost]
        public JsonResult SubmitProductRequest(RequestInfoViewModel objRequest)
        {
            EmailViewModel objEmailViewModel = _EmailService.GetRequestViewModel(objRequest.Message, objRequest.PhoneNumber);
            
            objEmailViewModel.MessageBodyHTMLText = RenderPartialToString(this,
                "_Request",
                objEmailViewModel,
                ViewData, TempData);
            _EmailService.SendRequestEmail(objEmailViewModel);
            return Json(true);
        }

        [HttpPost]
        public JsonResult SubmitImportRequest(RequestInfoViewModel objRequest)
        {
            EmailViewModel objEmailViewModel = _EmailService.GetImportViewModel(objRequest.Message, objRequest.PhoneNumber);            
            objEmailViewModel.MessageBodyHTMLText = RenderPartialToString(this,
                "_Import",
                objEmailViewModel,
                ViewData, TempData);
            _EmailService.SendImportEmail(objEmailViewModel);
            return Json(true);
        }

        [HttpPost]
        public JsonResult SubmitExportRequest(RequestInfoViewModel objRequest)
        {
            EmailViewModel objEmailViewModel = _EmailService.GetExportViewModel(objRequest.Message, objRequest.PhoneNumber);
            objEmailViewModel.MessageBodyHTMLText = RenderPartialToString(this,
                "_Export",
                objEmailViewModel,
                ViewData, TempData);
            _EmailService.SendExportEmail(objEmailViewModel);
            return Json(true);
        }       
    }
}