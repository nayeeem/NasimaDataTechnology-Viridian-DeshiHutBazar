
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Common;
using System.ComponentModel.DataAnnotations;
using Model;
using WebDeshiHutBazar;


namespace WebDeshiHutBazar
{
    public class LogReportInformationViewModel : BaseViewModel
    {
        public LogReportInformationViewModel()
        {
            ListLogPosts = new List<LogPostAction>();
            ListPageVisitCount = new List<SelectListItem>();
            ListPostVisited = new List<LogPostAction>();
            InformationTimeBasedTotalVisits = new TimeReport();
            ListDateRangeTotalVisited = new List<SelectListItem>();
            ListTimeSlotTotalVisited = new List<SelectListItem>();
            ListCateTotalVisited = new List<SelectListItem>();
            ListProductVisitedCategoryTotalCount = new List<SelectListItem>();
            ListProductVisitedSubCategoryTotalCount = new List<SelectListItem>();
            ListUserReportViewModel = new List<UserReportViewModel>();
        }

        public LogReportInformationViewModel(EnumCurrency currency) : base(currency)
        {
            ListLogPosts = new List<LogPostAction>();
            ListPageVisitCount = new List<SelectListItem>();
            ListPostVisited = new List<LogPostAction>();
            InformationTimeBasedTotalVisits = new TimeReport();
            ListDateRangeTotalVisited = new List<SelectListItem>();
            ListTimeSlotTotalVisited = new List<SelectListItem>();
            ListCateTotalVisited = new List<SelectListItem>();
            ListProductVisitedCategoryTotalCount = new List<SelectListItem>();
            ListProductVisitedSubCategoryTotalCount = new List<SelectListItem>();
            ListUserReportViewModel = new List<UserReportViewModel>();
        }

        public IEnumerable<SelectListItem> AV_ReportLength { get; set; }

        public EnumReportLength? EnumReportLength { get; set; }

        public List<LogPostAction> ListLogPosts { get; set; }

        public List<SelectListItem> ListPageVisitCount { get; set; }

        public List<SelectListItem> ListPostVisitCount { get; set; }

        public List<SelectListItem> ListUserPostCount { get; set; }

        public List<SelectListItem> ListSubCategoryVisitCount { get; set; }

        public List<SelectListItem> ListCategoryVisitCount { get; set; }

        public List<LogPostAction> ListPostVisited { get; set; }

        public TimeReport InformationTimeBasedTotalVisits { get; set; }

        public List<SelectListItem> ListDateRangeTotalVisited { get; set; }

        public List<SelectListItem> ListTimeSlotTotalVisited { get; set; }

        public List<SelectListItem> ListCateTotalVisited { get; set; }

        public List<SelectListItem> ListProductVisitedCategoryTotalCount { get; set; }

        public List<SelectListItem> ListProductVisitedSubCategoryTotalCount { get; set; }

        public List<UserReportViewModel> ListUserReportViewModel { get; internal set; }
    }
}
