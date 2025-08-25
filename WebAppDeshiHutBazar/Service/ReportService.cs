using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;
using Model;


namespace WebDeshiHutBazar
{
    public class SeleclDoubleList
    {
        public SeleclDoubleList()
        {
            TodayList = new List<SelectListItem>();
        }
        public List<SelectListItem> TodayList { get; set; }
        public DateTime DateToday { get; set; }
    }

    public class TimeReport
    {
        public TimeReport()
        {
            TotalCountList = new List<SelectListItem>();
            ListAllDates = new List<SeleclDoubleList>();
        }
        public List<SeleclDoubleList> ListAllDates { get; set; }
        public List<SelectListItem> TotalCountList { get; set; }
    }

    public class ReportService : IReportService
    {
        private readonly IUserAccountService _UserAccountService;
        public ReportService(IUserAccountService userAccountService)
        {
            _UserAccountService = userAccountService;
        }

        public void GetListVisitedPostSubCategory(List<LogPostAction> objListLogPosts, LogReportInformationViewModel objModel)
        {
            List<SelectListItem> objListVisitedPostSelectListItem = new List<SelectListItem>();
            foreach (var item in objListLogPosts.Where(a=>a.LogType == EnumLogType.PostDetailLink).ToList())
            {
                var itemCategory = item.SubCategoryID;
                if (itemCategory.HasValue)
                {
                    if (!objListVisitedPostSelectListItem.Any(a => a.Text.Trim() == itemCategory.Value.ToString()))
                    {
                        SelectListItem objListItem = new SelectListItem();
                        objListItem.Text = itemCategory.Value.ToString();
                        objListItem.Value = "1";
                        objListVisitedPostSelectListItem.Add(objListItem);
                    }
                    else
                    {
                        var selectedItem = objListVisitedPostSelectListItem.FirstOrDefault(a => a.Text.Trim() == itemCategory.Value.ToString().Trim());
                        if (selectedItem != null)
                        {
                            if (!string.IsNullOrEmpty(selectedItem.Value))
                            {
                                var count = Convert.ToInt32(selectedItem.Value.Trim());
                                count += 1;
                                selectedItem.Value = count.ToString();
                            }
                        }
                    }
                }
            }
            foreach (var catItem in objListVisitedPostSelectListItem)
            {
                if (!string.IsNullOrEmpty(catItem.Text))
                {
                    var cateText = BusinessObjectSeed.GetCatSubCategoryItemTextForId(Convert.ToInt32(catItem.Text));
                    catItem.Text = cateText;
                }
            }
            objModel.ListProductVisitedSubCategoryTotalCount = objListVisitedPostSelectListItem.OrderByDescending(a => Convert.ToInt32(a.Value)).ToList();
        }

        public void GetListVisitedPostCategory(List<LogPostAction> objListLogPosts, LogReportInformationViewModel objModel)
        {
            List<SelectListItem> objListVisitedPostSelectListItem = new List<SelectListItem>();
            foreach (var item in objListLogPosts.Where(a=>a.LogType == EnumLogType.PostDetailLink).ToList())
            {
                var itemCategory = item.CategoryID;
                if (itemCategory.HasValue)
                {
                    if (!objListVisitedPostSelectListItem.Any(a => a.Text.Trim() == itemCategory.Value.ToString()))
                    {
                        SelectListItem objListItem = new SelectListItem();
                        objListItem.Text = itemCategory.Value.ToString();
                        objListItem.Value = "1";
                        objListVisitedPostSelectListItem.Add(objListItem);
                    }
                    else
                    {
                        var selectedItem = objListVisitedPostSelectListItem.FirstOrDefault(a => a.Text.Trim() == itemCategory.Value.ToString().Trim());
                        if (selectedItem != null)
                        {
                            if (!string.IsNullOrEmpty(selectedItem.Value))
                            {
                                var count = Convert.ToInt32(selectedItem.Value.Trim());
                                count += 1;
                                selectedItem.Value = count.ToString();
                            }
                        }
                    }
                }
            }
            foreach (var catItem in objListVisitedPostSelectListItem)
            {
                if (!string.IsNullOrEmpty(catItem.Text))
                {
                    var cateText = BusinessObjectSeed.GetCatSubCategoryItemTextForId(Convert.ToInt32(catItem.Text));
                    catItem.Text = cateText;
                }
            }
            objModel.ListProductVisitedCategoryTotalCount = objListVisitedPostSelectListItem.OrderByDescending(a => Convert.ToInt32(a.Value)).ToList();
        }

        public void GetDateWiseTotalHomeVisitedCount(
            List<LogPostAction> listLogPosts,
            LogReportInformationViewModel objModel,
            DateTime date1,
            DateTime date2)
        {
            List<SelectListItem> objListItems = new List<SelectListItem>();
            var stDate = date1;
            TimeSpan ts = new TimeSpan(00, 00, 0);
            stDate = stDate.Date + ts;
            var edDate = date2;
            ts = new TimeSpan(23, 59, 59);
            edDate = edDate.Date + ts;
            var vDate = stDate;
            SelectListItem objSingleItem;
            var slot1 = 0;
            var slot2 = 0;
            var slot3 = 0;
            var slot4 = 0;
            var slot5 = 0;
            var slot6 = 0;
            List<SelectListItem> objListCateItems = new List<SelectListItem>();
            for (; vDate.Date <= edDate.Date;)
            {
                ts = new TimeSpan(00, 00, 0);
                var stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(23, 59, 59);
                var edSlotDate = vDate.Date + ts;
                var countHomePageVisits = listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                                    a.CreatedDate <= edSlotDate &&
                                                                    a.LogType == EnumLogType.HomePageLink).Count();
                objSingleItem = new SelectListItem();
                int indexSpace = stSlotDate.ToString().IndexOf(' ');
                objSingleItem.Text = stSlotDate.ToString().Substring(0, indexSpace);
                objSingleItem.Value = countHomePageVisits.ToString();
                objListItems.Add(objSingleItem);
                ts = new TimeSpan(00, 00, 0);
                stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(03, 59, 59);
                edSlotDate = vDate.Date + ts;
                slot1 = slot1 + listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                    a.CreatedDate <= edSlotDate &&
                                                     a.LogType == EnumLogType.HomePageLink).Count();
                ts = new TimeSpan(04, 00, 0);
                stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(07, 59, 59);
                edSlotDate = vDate.Date + ts;
                slot2 = slot2 + listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                    a.CreatedDate <= edSlotDate &&
                                                     a.LogType == EnumLogType.HomePageLink).Count();
                ts = new TimeSpan(08, 00, 0);
                stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(11, 59, 59);
                edSlotDate = vDate.Date + ts;
                slot3 = slot3 + listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                    a.CreatedDate <= edSlotDate &&
                                                     a.LogType == EnumLogType.HomePageLink).Count();
                ts = new TimeSpan(12, 00, 0);
                stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(15, 59, 59);
                edSlotDate = vDate.Date + ts;
                slot4 = slot4 + listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                    a.CreatedDate <= edSlotDate &&
                                                    a.LogType == EnumLogType.HomePageLink).Count();
                ts = new TimeSpan(16, 00, 0);
                stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(19, 59, 59);
                edSlotDate = vDate.Date + ts;
                slot5 = slot5 + listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                    a.CreatedDate <= edSlotDate &&
                                                     a.LogType == EnumLogType.HomePageLink).Count();
                ts = new TimeSpan(20, 00, 0);
                stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(23, 59, 59);
                edSlotDate = vDate.Date + ts;
                slot6 = slot6 + listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                    a.CreatedDate <= edSlotDate &&
                                                     a.LogType == EnumLogType.HomePageLink).Count();
                vDate = vDate.AddDays(1);
            }
            objModel.ListDateRangeTotalVisited = objListItems;
            PrepareSlotList(slot1, slot2, slot3, slot4, slot5, slot6, objModel);
        }

        public void PrepareSlotList(int slot1, int slot2, int slot3, int slot4, int slot5, int slot6, LogReportInformationViewModel objModel)
        {
            List<SelectListItem> objListItems = new List<SelectListItem>();
            SelectListItem objItem = new SelectListItem();
            objItem.Text = "12:00AM - 04:00AM";
            objItem.Value = slot1.ToString();
            objListItems.Add(objItem);
            objItem = new SelectListItem();
            objItem.Text = "04:00AM - 08:00AM";
            objItem.Value = slot2.ToString();
            objListItems.Add(objItem);
            objItem = new SelectListItem();
            objItem.Text = "08:00AM - 12:00PM";
            objItem.Value = slot3.ToString();
            objListItems.Add(objItem);
            objItem = new SelectListItem();
            objItem.Text = "12:00PM - 4:00PM";
            objItem.Value = slot4.ToString();
            objListItems.Add(objItem);
            objItem = new SelectListItem();
            objItem.Text = "04:00PM - 08:00PM";
            objItem.Value = slot5.ToString();
            objListItems.Add(objItem);
            objItem = new SelectListItem();
            objItem.Text = "08:00PM - 12:00PM";
            objItem.Value = slot6.ToString();
            objListItems.Add(objItem);
            objModel.ListTimeSlotTotalVisited = objListItems;
        }

        public void GetListSubCategoryVisitsCount(List<LogPostAction> listLogPosts, LogReportInformationViewModel objModel)
        {
            objModel.ListSubCategoryVisitCount = listLogPosts.Where(a=>a.LogType == EnumLogType.SubCategoryMarketLink).GroupBy(info => BusinessObjectSeed.GetCateSubCategoryItemText(info.CatMarketSubCategoryID))
                        .Select(group => new SelectListItem
                        {
                            Text = group.Key,
                            Value = group.Count().ToString()
                        }).ToList()
                        .OrderByDescending(x => Convert.ToInt32(x.Value)).ToList();
        }

        public void GetListCategoryVisitsCount(List<LogPostAction> listLogPosts, LogReportInformationViewModel objModel)
        {
            objModel.ListCategoryVisitCount = listLogPosts.Where(a => a.LogType == EnumLogType.CategoryMarketLink).GroupBy(info => BusinessObjectSeed.GetCateSubCategoryItemText(info.CategoryID))
                        .Select(group => new SelectListItem
                        {
                            Text = group.Key,
                            Value = group.Count().ToString()
                        }).ToList()
                        .OrderByDescending(x => Convert.ToInt32(x.Value)).ToList();
        }

        public void GetListPostVisitsCount(List<LogPostAction> listLogPosts, LogReportInformationViewModel objModel)
        {
            objModel.ListPostVisitCount = listLogPosts.Where(a => a.LogType == EnumLogType.PostDetailLink && !string.IsNullOrEmpty(a.PostTitle))
                                                      .GroupBy(info => info.PostTitle)
                                                      .Select(group => new SelectListItem
                                                      {
                                                          Text = group.Key,
                                                          Value = group.Count().ToString()
                                                      }).ToList()
                                                      .OrderByDescending(x => Convert.ToInt32(x.Value)).Take(30).ToList();
        }

        public void GetListPostVisited(List<LogPostAction> listLogPosts, LogReportInformationViewModel objModel)
        {
            objModel.ListPostVisited = listLogPosts.Where(a => a.LogType == EnumLogType.PostDetailLink && !string.IsNullOrEmpty(a.PostTitle))
                .OrderBy(a => a.PostTitle)
                .GroupBy(info => info.PostTitle)
                .Select(g => g.FirstOrDefault()).ToList();
        }

        public void GetPageVisitsCount(List<LogPostAction> listLogPosts, LogReportInformationViewModel objModel)
        {
            var HomePageLink = listLogPosts.Count(a => a.LogType == EnumLogType.HomePageLink);
            var AllItemMarketLink = listLogPosts.Count(a => a.LogType == EnumLogType.AllItemMarketLink);
            var CategoryMarketLink = listLogPosts.Count(a => a.LogType == EnumLogType.CategoryMarketLink);
            var SubCategoryMarketLink = listLogPosts.Count(a => a.LogType == EnumLogType.SubCategoryMarketLink);
            var SpecialMarketLink = listLogPosts.Count(a => a.LogType == EnumLogType.SpecialMarketLink);
            var PostDetailLink = listLogPosts.Count(a => a.LogType == EnumLogType.PostDetailLink);
            var AdvancedSearchLink = listLogPosts.Count(a => a.LogType == EnumLogType.AdvancedSearchLink);
            var FabiaServiceLink = listLogPosts.Count(a => a.LogType == EnumLogType.FabiaServiceLink);
            objModel.ListPageVisitCount.Add(new SelectListItem() { Text = "Home Page Visit", Value = HomePageLink.ToString() });
            objModel.ListPageVisitCount.Add(new SelectListItem() { Text = "All Market", Value = AllItemMarketLink.ToString() });
            objModel.ListPageVisitCount.Add(new SelectListItem() { Text = "Sub-Category Dropdown", Value = SubCategoryMarketLink.ToString() });
            objModel.ListPageVisitCount.Add(new SelectListItem() { Text = "Post Detail Visit", Value = PostDetailLink.ToString() });
            objModel.ListPageVisitCount.Add(new SelectListItem() { Text = "Advanced Search", Value = AdvancedSearchLink.ToString() });
            objModel.ListPageVisitCount.Add(new SelectListItem() { Text = "Category Button", Value = CategoryMarketLink.ToString() });
            objModel.ListPageVisitCount.Add(new SelectListItem() { Text = "Fabia Button", Value = FabiaServiceLink.ToString() });
            objModel.ListPageVisitCount.Add(new SelectListItem() { Text = "Special Button", Value = SpecialMarketLink.ToString() });
        }

        public DateTime GetEndDate()
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime date2 = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            return date2;
        }

        public TimeReport GetTimeBasedVisitorsCount(List<LogPostAction> listLogPosts, EnumReportLength reportLength)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime date2 = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            DateTime date1 = GetStartDate(reportLength, date2);
            DateTime dStart = date1.Date;
            DateTime dEnd = date2.Date;
            List<SelectListItem> objListToday = new List<SelectListItem>();
            int eightPM = 0;
            int twelvePM = 0;
            int fourPM = 0;
            int fourAM = 0;
            int eightAM = 0;
            int twelveAM = 0;
            List<SeleclDoubleList> objAllDatesList = new List<SeleclDoubleList>();
            for (var d = dStart; d <= dEnd; d.AddDays(1))
            {
                var todayList = listLogPosts.Where(a => a.CreatedDate.Date == d && a.LogType == EnumLogType.HomePageLink).ToList();
                var dateTotalVisitors = todayList.Count;

                var time00AM04AM_Home_Visitors = todayList.Where(a =>
                                    a.CreatedDate.TimeOfDay.Ticks >= TimeSpan.FromHours(0).Ticks &&
                                    a.CreatedDate.TimeOfDay.Ticks <= TimeSpan.FromHours(4).Ticks).ToList();
                var time05AM08AM_Home_Visitors = todayList.Where(a =>
                                    a.CreatedDate.TimeOfDay.Ticks >= TimeSpan.FromHours(5).Ticks &&
                                    a.CreatedDate.TimeOfDay.Ticks <= TimeSpan.FromHours(8).Ticks).ToList();
                var time09AM12AM_Home_Visitors = todayList.Where(a =>
                                    a.CreatedDate.TimeOfDay.Ticks >= TimeSpan.FromHours(9).Ticks &&
                                    a.CreatedDate.TimeOfDay.Ticks <= TimeSpan.FromHours(12).Ticks).ToList();
                var time13AM16AM_Home_Visitors = todayList.Where(a =>
                                    a.CreatedDate.TimeOfDay.Ticks >= TimeSpan.FromHours(13).Ticks &&
                                    a.CreatedDate.TimeOfDay.Ticks <= TimeSpan.FromHours(16).Ticks).ToList();
                var time17AM20AM_Home_Visitors = todayList.Where(a =>
                                    a.CreatedDate.TimeOfDay.Ticks >= TimeSpan.FromHours(17).Ticks &&
                                    a.CreatedDate.TimeOfDay.Ticks <= TimeSpan.FromHours(20).Ticks).ToList();
                var time21AM24AM_Home_Visitors = todayList.Where(a =>
                                    a.CreatedDate.TimeOfDay.Ticks >= TimeSpan.FromHours(21).Ticks &&
                                    a.CreatedDate.TimeOfDay.Ticks <= TimeSpan.FromHours(24).Ticks).ToList();

                twelveAM = twelveAM + time00AM04AM_Home_Visitors.Count;
                fourAM = fourAM + time05AM08AM_Home_Visitors.Count;
                eightAM = eightAM + time09AM12AM_Home_Visitors.Count;
                twelvePM = twelvePM + time13AM16AM_Home_Visitors.Count;
                fourPM = fourPM + time17AM20AM_Home_Visitors.Count;
                eightPM = eightPM + time21AM24AM_Home_Visitors.Count;
                objListToday.Add(new SelectListItem() { Value = time00AM04AM_Home_Visitors.Count.ToString(), Text = "00.00 - 04.00" });
                objListToday.Add(new SelectListItem() { Value = time05AM08AM_Home_Visitors.Count.ToString(), Text = "05.00 - 08.00" });
                objListToday.Add(new SelectListItem() { Value = time09AM12AM_Home_Visitors.Count.ToString(), Text = "09.00 - 12.00" });
                objListToday.Add(new SelectListItem() { Value = time13AM16AM_Home_Visitors.Count.ToString(), Text = "13.00 - 16.00" });
                objListToday.Add(new SelectListItem() { Value = time17AM20AM_Home_Visitors.Count.ToString(), Text = "17.00 - 20.00" });
                objListToday.Add(new SelectListItem() { Value = time21AM24AM_Home_Visitors.Count.ToString(), Text = "21.00 - 24.00" });
                SeleclDoubleList objItem = new SeleclDoubleList();
                objItem.TodayList = objListToday;
                objItem.DateToday = d;
                objAllDatesList.Add(objItem);
            }
            TimeReport objInformation = new TimeReport();
            objInformation.ListAllDates = objAllDatesList;
            var objFinalList = new List<SelectListItem>();
            objFinalList.Add(new SelectListItem() { Value = twelveAM.ToString(), Text = "00.00 - 04.00" });
            objFinalList.Add(new SelectListItem() { Value = fourAM.ToString(), Text = "05.00 - 08.00" });
            objFinalList.Add(new SelectListItem() { Value = eightAM.ToString(), Text = "09.00 - 12.00" });
            objFinalList.Add(new SelectListItem() { Value = twelvePM.ToString(), Text = "13.00 - 16.00" });
            objFinalList.Add(new SelectListItem() { Value = fourPM.ToString(), Text = "17.00 - 20.00" });
            objFinalList.Add(new SelectListItem() { Value = eightPM.ToString(), Text = "21.00 - 24.00" });
            objInformation.TotalCountList = objFinalList;
            return objInformation;
        }

        public DateTime GetStartDate(EnumReportLength? reportLength, DateTime endDate)
        {
            if (!reportLength.HasValue)
            {
                return endDate.AddHours(-1);
            }
            return endDate.AddHours(-(int)reportLength);
        }

        public void GetDateWiseUserTotalPostVisitCount(
            List<LogPostAction> listLogPosts,
            LogReportInformationViewModel objModel,
            DateTime date1,
            DateTime date2,
            long[] userPostIDS)
        {
            List<SelectListItem> objListItems = new List<SelectListItem>();
            var stDate = date1;
            TimeSpan ts = new TimeSpan(00, 00, 0);
            stDate = stDate.Date + ts;
            var edDate = date2;
            ts = new TimeSpan(23, 59, 59);
            edDate = edDate.Date + ts;
            var vDate = stDate;
            SelectListItem objSingleItem;
            var slot1 = 0;
            var slot2 = 0;
            var slot3 = 0;
            var slot4 = 0;
            var slot5 = 0;
            var slot6 = 0;
            List<SelectListItem> objListCateItems = new List<SelectListItem>();
            for (; vDate.Date <= edDate.Date;)
            {
                ts = new TimeSpan(00, 00, 0);
                var stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(23, 59, 59);
                var edSlotDate = vDate.Date + ts;
                var countUserPostsVisitForToday = listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                                    a.CreatedDate <= edSlotDate &&
                                                                    a.LogType == EnumLogType.PostDetailLink &&
                                                                    userPostIDS.Any(c=> c == a.PostID)).Count();
                objSingleItem = new SelectListItem();
                int indexSpace = stSlotDate.ToString().IndexOf(' ');
                objSingleItem.Text = stSlotDate.ToString().Substring(0, indexSpace);
                objSingleItem.Value = countUserPostsVisitForToday.ToString();
                objListItems.Add(objSingleItem);



                ts = new TimeSpan(00, 00, 0);
                stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(03, 59, 59);
                edSlotDate = vDate.Date + ts;
                slot1 = slot1 + listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                    a.CreatedDate <= edSlotDate &&
                                                      a.LogType == EnumLogType.PostDetailLink &&
                                                                    userPostIDS.Any(c => c == a.PostID)).Count();
                ts = new TimeSpan(04, 00, 0);
                stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(07, 59, 59);
                edSlotDate = vDate.Date + ts;
                slot2 = slot2 + listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                    a.CreatedDate <= edSlotDate &&
                                                      a.LogType == EnumLogType.PostDetailLink &&
                                                                    userPostIDS.Any(c => c == a.PostID)).Count();
                ts = new TimeSpan(08, 00, 0);
                stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(11, 59, 59);
                edSlotDate = vDate.Date + ts;
                slot3 = slot3 + listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                    a.CreatedDate <= edSlotDate &&
                                                      a.LogType == EnumLogType.PostDetailLink &&
                                                                    userPostIDS.Any(c => c == a.PostID)).Count();
                ts = new TimeSpan(12, 00, 0);
                stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(15, 59, 59);
                edSlotDate = vDate.Date + ts;
                slot4 = slot4 + listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                    a.CreatedDate <= edSlotDate &&
                                                     a.LogType == EnumLogType.PostDetailLink &&
                                                                    userPostIDS.Any(c => c == a.PostID)).Count();
                ts = new TimeSpan(16, 00, 0);
                stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(19, 59, 59);
                edSlotDate = vDate.Date + ts;
                slot5 = slot5 + listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                    a.CreatedDate <= edSlotDate &&
                                                      a.LogType == EnumLogType.PostDetailLink &&
                                                                    userPostIDS.Any(c => c == a.PostID)).Count();
                ts = new TimeSpan(20, 00, 0);
                stSlotDate = vDate.Date + ts;
                ts = new TimeSpan(23, 59, 59);
                edSlotDate = vDate.Date + ts;
                slot6 = slot6 + listLogPosts.Where(a => a.CreatedDate >= stSlotDate &&
                                                    a.CreatedDate <= edSlotDate &&
                                                     a.LogType == EnumLogType.PostDetailLink &&
                                                                    userPostIDS.Any(c => c == a.PostID)).Count();
                vDate = vDate.AddDays(1);
            }
            objModel.ListDateRangeTotalVisited = objListItems;
            PrepareSlotList(slot1, slot2, slot3, slot4, slot5, slot6, objModel);
        }

        public async Task<List<UserReportViewModel>> GetUserPostList()
        {
            return await _UserAccountService.GetAllUsers(true);
        }

        public void GetListUserPostCount(List<UserReportViewModel> listUserReportViewModel, LogReportInformationViewModel objModel)
        {
            List<SelectListItem> objListVisitedPostSelectListItem = new List<SelectListItem>();
            foreach (var item in listUserReportViewModel.ToList())
            {
                SelectListItem objListItem = new SelectListItem();
                objListItem.Text = item.UserName;
                objListItem.Value = item.PostCount.ToString();
                objListVisitedPostSelectListItem.Add(objListItem);
            }
            objModel.ListUserPostCount = objListVisitedPostSelectListItem.ToList();
        }
    }
}