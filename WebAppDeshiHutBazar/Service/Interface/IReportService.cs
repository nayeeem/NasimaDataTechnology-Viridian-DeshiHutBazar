using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;
using Model;


namespace WebDeshiHutBazar
{    
    public interface IReportService
    {
        void GetListVisitedPostCategory(List<LogPostAction> objListLogPosts, LogReportInformationViewModel objModel);

        void GetListVisitedPostSubCategory(List<LogPostAction> objListLogPosts, LogReportInformationViewModel objModel);

        void GetDateWiseTotalHomeVisitedCount(List<LogPostAction> listLogPosts, LogReportInformationViewModel objModel, DateTime date1, DateTime date2);

        void PrepareSlotList(int slot1, int slot2, int slot3, int slot4, int slot5, int slot6, LogReportInformationViewModel objModel);

        void GetListSubCategoryVisitsCount(List<LogPostAction> listLogPosts, LogReportInformationViewModel objModel);

        void GetListCategoryVisitsCount(List<LogPostAction> listLogPosts, LogReportInformationViewModel objModel);

        void GetListPostVisitsCount(List<LogPostAction> listLogPosts, LogReportInformationViewModel objModel);

        void GetListPostVisited(List<LogPostAction> listLogPosts, LogReportInformationViewModel objModel);

        void GetPageVisitsCount(List<LogPostAction> listLogPosts, LogReportInformationViewModel objModel);

        DateTime GetEndDate();

        TimeReport GetTimeBasedVisitorsCount(List<LogPostAction> listLogPosts, EnumReportLength reportLength);

        DateTime GetStartDate(EnumReportLength? reportLength, DateTime endDate);

        void GetDateWiseUserTotalPostVisitCount(
            List<LogPostAction> listLogPosts,
            LogReportInformationViewModel objModel,
            DateTime date1,
            DateTime date2,
            long[] userPostIDS);

        Task<List<UserReportViewModel>> GetUserPostList();

        void GetListUserPostCount(List<UserReportViewModel> listUserReportViewModel, LogReportInformationViewModel objModel);
    }
}