using System.Web.Mvc;
using Common;
using System;
using Data;
using System.Collections.Generic;
using Model;
using System.Threading.Tasks;
using System.Linq;

namespace WebDeshiHutBazar
{
    public class PostLogReportController : BaseController
    {
        public readonly IReportService _IReportService;
        public readonly ILogPostRepository _ILoggingRepository;
        public readonly IPostMangementService _IPostRepository;

        public PostLogReportController(IReportService iReportService, ILogPostRepository iLoggingService, IPostMangementService iPostRepository)
        {
            _IReportService = iReportService;
            _ILoggingRepository = iLoggingService;
            _IPostRepository = iPostRepository;
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> Index(EnumReportLength? reportLength)
        {           
            if(!reportLength.HasValue)
            {
                reportLength = EnumReportLength.LastOneMonth;
            }
            LogReportInformationViewModel objModel = new LogReportInformationViewModel(CURRENCY_CODE);
            objModel.AV_ReportLength = DropDownDataList.GetReportLengthList();
            objModel.EnumReportLength = reportLength;
            DateTime endDate = _IReportService.GetEndDate(); 
            DateTime startDate = _IReportService.GetStartDate(reportLength, endDate);
            var objListLogPosts = _ILoggingRepository.GetFullLogRecordsDateRanged(startDate, endDate);
            objModel.ListLogPosts = objListLogPosts;

            var listIDS = await DateBasedUserPosts(GetSessionUserId());
            var userPostList = GetUserPostList(objListLogPosts, listIDS);
            _IReportService.GetDateWiseUserTotalPostVisitCount(objListLogPosts, objModel, startDate, endDate, listIDS);
            _IReportService.GetListVisitedPostCategory(userPostList, objModel);
            _IReportService.GetListVisitedPostSubCategory(userPostList, objModel);
            _IReportService.GetListPostVisitsCount(userPostList, objModel);
            _IReportService.GetListPostVisited(userPostList, objModel);            
            return View(@"../../Areas/Advertizer/Views/PostLogReport/Index", objModel);
        }

        private List<LogPostAction> GetUserPostList(List<LogPostAction> objListLogPosts, long[] listIDS)
        {
            return objListLogPosts.Where(a => listIDS.Any(c => c == a.PostID) && a.LogType == EnumLogType.PostDetailLink).ToList();
        }

        private async Task<long[]> DateBasedUserPosts(long userID)
        {
            var listPostVM = await _IPostRepository.GetAllPublishedPostsByUserID(
                userID, 
                EnumCountry.Bangladesh, 
                CURRENCY_CODE);
            long[] listIDs = new long[listPostVM.Count];
            for(var i =0; i< listPostVM.Count; i++)
            {
                listIDs[i] = listPostVM[i].PostID;
            }
            return listIDs;
        }
    }    
}
