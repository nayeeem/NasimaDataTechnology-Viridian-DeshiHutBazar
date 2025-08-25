using System.Web.Mvc;
using Common;
using System;
using Data;
using System.Threading.Tasks;

namespace WebDeshiHutBazar
{
    public class ApplicationLogReportController : BaseController
    {
        public readonly IReportService _IReportService;
        public readonly ILogPostRepository _ILoggingRepository;
        public readonly IUserAccountService _UserAccountService;

        public ApplicationLogReportController(IReportService iReportService, 
            ILogPostRepository iLoggingService,
            IUserAccountService userAccountService)
        {
            _IReportService = iReportService;
            _ILoggingRepository = iLoggingService;
            _UserAccountService = userAccountService;
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Index(EnumReportLength? reportLength)
        {
            if (!IsCurrentSessionAnAdminUser())
                return View("UnwantedAccessError");
            if(!reportLength.HasValue)
            {
                reportLength = EnumReportLength.Today;
            }
            LogReportInformationViewModel objModel = new LogReportInformationViewModel();
            objModel.AV_ReportLength = DropDownDataList.GetReportLengthList();
            objModel.EnumReportLength = reportLength;
            DateTime endDate = _IReportService.GetEndDate(); 
            DateTime startDate = _IReportService.GetStartDate(reportLength, endDate);
            var objListLogPosts = _ILoggingRepository.GetFullLogRecordsDateRanged(startDate, endDate);
            objModel.ListLogPosts = objListLogPosts;
            _IReportService.GetDateWiseTotalHomeVisitedCount(objListLogPosts, objModel, startDate, endDate);
            _IReportService.GetPageVisitsCount(objListLogPosts, objModel);
            _IReportService.GetListSubCategoryVisitsCount(objListLogPosts, objModel);
            _IReportService.GetListCategoryVisitsCount(objListLogPosts, objModel);            
            _IReportService.GetListVisitedPostCategory(objListLogPosts, objModel);
            _IReportService.GetListVisitedPostSubCategory(objListLogPosts, objModel);
            _IReportService.GetListPostVisitsCount(objListLogPosts, objModel);
            _IReportService.GetListPostVisited(objListLogPosts, objModel);
            objModel.ListUserReportViewModel = await _IReportService.GetUserPostList();
            _IReportService.GetListUserPostCount(objModel.ListUserReportViewModel, objModel);

            return View(@"../../Areas/Report/Views/ApplicationLogReport/Index", objModel);
        }        
    }    
}
