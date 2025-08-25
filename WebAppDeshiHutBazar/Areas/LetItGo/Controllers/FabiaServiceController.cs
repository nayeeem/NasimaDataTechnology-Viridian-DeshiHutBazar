using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data;

using System.Threading.Tasks;




namespace WebDeshiHutBazar
{
    public class FabiaServiceController : PageingController
    {
        List<PostViewModel> listPostVM;
        private readonly IGroupPanelConfigService _GroupPanelConfigService;
        private readonly ILoggingService _LoggingService;
        private readonly IPostMangementService _PostMangementService;
        private readonly IPostMappingService _PostMappingService;
        private readonly IPostVisitService _PostVisitService;
        private readonly ILogPostRepository _ILogPostRepository;
        private readonly IFabiaProviderService _IProviderService;

        public FabiaServiceController() 
        { }

        public FabiaServiceController(ILoggingService loggingService,
            IGroupPanelConfigService groupPanelConfigService,
            IPostMangementService postMangementService,
            IPostMappingService postMappingService,
            IPostVisitService postVisitService,
            ILogPostRepository logPostRepository,
            IFabiaProviderService iFabiaProvideService,
            IEmailNotificationService emailService
            )
        {
            _LoggingService = loggingService;
            _GroupPanelConfigService = groupPanelConfigService;            
            _PostMangementService = postMangementService;
            _PostMappingService = postMappingService;
            listPostVM = new List<PostViewModel>();
            _PostVisitService = postVisitService;
            _ILogPostRepository = logPostRepository;
            _IProviderService = iFabiaProvideService;
            _EmailService = emailService;
        }

        [OutputCache(CacheProfile = "Cache30MinsItemDetail")]
        public async Task<ViewResult> ViewItemDetail(long postId)
        {
            var res = await _LoggingService.LogPostDetailPageVisit(EnumLogType.FabiaServiceLink, postId, COUNTRY_CODE, HttpContext.Session.SessionID);
            var postvm = await _PostMangementService.GetPostByPostIDForEdit(postId, CURRENCY_CODE);
            postvm.ListFabiaProvider = await _IProviderService.GetAllProviderByServiceBy(postId);
            postvm.PageName = "Fabia Item Details Page";
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
            var viewMoreUrl = Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" });
            postvm.ContentInfoViewModel.ListGroupPanelConfiguration =
                                     await _GroupPanelConfigService.GetAllPageGroupPanelConfigurations(
                                         EnumPublicPage.FabiaDetail,
                                         viewMoreUrl,
                                         viewPostDetUrl,
                                         COUNTRY_CODE,
                                         CURRENT_TIME_SLOT,
                                         null,
                                         0,
                                         null,
                                         0,
                                         null,
                                         CURRENCY_CODE);
            return View(@"../../Areas/LetItGo/Views/FabiaService/ViewFabiaItemDetail", postvm);
        }

        [HttpGet]
        public async Task<JsonResult> ProviderPhoneCollected(long providerID)
        {
            var providerObject = await _IProviderService.GetProviderByID(providerID);
            EmailViewModel objEmailViewModel = _EmailService.GetProviderContactViewModel(providerObject, providerID);
            objEmailViewModel.MessageBodyHTMLText = RenderPartialToString(this,
                "_ProviderPhoneCollected",
                objEmailViewModel,
                ViewData, TempData);
            objEmailViewModel.ReceiverEmail = providerObject.Email;
            _EmailService.SendProviderContactEmail(objEmailViewModel);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}