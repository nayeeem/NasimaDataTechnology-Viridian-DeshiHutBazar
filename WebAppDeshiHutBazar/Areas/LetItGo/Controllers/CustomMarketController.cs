using System.Web.Mvc;
using System.Linq;
using Common;
using System.Threading.Tasks;

namespace WebDeshiHutBazar
{
    public partial class CategoryMarketController : PageingController
    {
        [OutputCache(CacheProfile = "Cache30Mins")]
        public async Task<ViewResult> CustomMarket(long subcategoryid, int pageNumber = 1)
        {
            var res = await _LoggingService.LogSpecialMarketPageVisit(subcategoryid, COUNTRY_CODE, HttpContext.Session.SessionID);
            var specificMarket = BusinessObjectSeed.GetCatSubCategoryItemTextForId(subcategoryid);
            ViewBag.Title = specificMarket;
            MarketInfoViewModel objMarketInfoModel = new MarketInfoViewModel(CURRENCY_CODE)
            {
                PostViewModel = new PostViewModel(CURRENCY_CODE)
                {
                    AV_State = DropDownDataList.GetAllStateList(),
                    AV_Category = DropDownDataList.GetCategoryList(),
                    AV_SubCategory = DropDownDataList.GetSubCategoryList()
                },              
                SubCategoryID = subcategoryid,
                CategoryID = BusinessObjectSeed.GetCategoryIDForSubCategoryID(subcategoryid),
                PageName = "Today's " + specificMarket + " Market",
                DisplaySubCategory = specificMarket              
            };            
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
            var viewMoreUrl = Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" });
            objMarketInfoModel.ContentInfoViewModel.ListGroupPanelConfiguration =
                                     await _GroupPanelConfigService.GetAllPageGroupPanelConfigurations(
                                         EnumPublicPage.SpecialMarketButton,
                                         viewMoreUrl,
                                         viewPostDetUrl,
                                         COUNTRY_CODE,
                                         CURRENT_TIME_SLOT,
                                         EnumMarketType.SubCategory, 
                                         subcategoryid, pageNumber, 
                                         MARKET_PAGE_SIZE, null,
                                         CURRENCY_CODE);
            var singleMarketConfigList = objMarketInfoModel.ContentInfoViewModel.ListGroupPanelConfiguration.Where(a => a.EnumPanelDisplayStyle == EnumPanelDisplayStyle.MarketPanel).ToList();
            if (singleMarketConfigList != null)
            {
                foreach (var listItem in singleMarketConfigList)
                {
                    if (listItem.EnumPublicPage == EnumPublicPage.AllMarket)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber, MARKET_PAGE_SIZE, "Market", "AllItemMarket", null, null);
                    }
                    else if (listItem.EnumPublicPage == EnumPublicPage.CategoryButtonMarket)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber, MARKET_PAGE_SIZE, "CatMarket", "CategoryMarket", null, subcategoryid);
                    }
                    else if (listItem.EnumPublicPage == EnumPublicPage.SpecialMarketButton)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber, MARKET_PAGE_SIZE, "CustomMarket", "CategoryMarket", null, subcategoryid);
                    }
                    else if (listItem.EnumPublicPage == EnumPublicPage.SubCategoryDropdownMarket)
                    {
                        listItem.PageingModelAll = SetPageingModel(listItem.PageingModelAll, listItem.TotalPostCount,
                                                    pageNumber, MARKET_PAGE_SIZE, "Market", "CategoryMarket", null, subcategoryid);
                    }
                }
            }
            objMarketInfoModel.ContentInfoViewModel.CategoryID = objMarketInfoModel.CategoryID;
            return View(@"../../Areas/LetItGo/Views/CategoryMarket/Market", objMarketInfoModel);
        }
    }
}