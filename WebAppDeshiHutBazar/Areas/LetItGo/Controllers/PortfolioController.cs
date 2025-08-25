using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data;

using System.Threading.Tasks;



namespace WebDeshiHutBazar
{
    public class PortfolioController : PageingController
    {
        List<PostViewModel> listPostVM;
        private readonly IGroupPanelConfigService _GroupPanelConfigService;
        private readonly ILoggingService _LoggingService;
        private readonly IPostMangementService _PostMangementService;
        private readonly IPostMappingService _PostMappingService;
        private readonly IUserService _UserService;

        public PortfolioController() 
        { }

        public PortfolioController(ILoggingService loggingService,
            IGroupPanelConfigService groupPanelConfigService,
            IPostMangementService postMangementService,
            IPostMappingService postMappingService,
            IUserService userService)
        {
            _LoggingService = loggingService;
            _GroupPanelConfigService = groupPanelConfigService;            
            _PostMangementService = postMangementService;
            _PostMappingService = postMappingService;
            listPostVM = new List<PostViewModel>();
            _UserService = userService;
        }

        [OutputCache(CacheProfile = "Cache60Mins")]
        public async Task<ViewResult> Portfolio(int pageNumber=1)
        {
            var res = _LoggingService.LogEntirePageVisit(EnumLogType.PortfolioLink, COUNTRY_CODE, HttpContext.Session.SessionID);
            var users = await _UserService.GetAllUser();
            UserViewModel objUserVM;
            List<UserViewModel> objlistUsers = new List<UserViewModel>();
            foreach (var user in users)
            {
                objUserVM = new UserViewModel(CURRENCY_CODE);
                objUserVM.Email = user.Email;
                objUserVM.Phone = user.Phone;
                objUserVM.UserID = user.UserID;
                objUserVM.IsCompanySeller = user.UserAccountType == EnumUserAccountType.Company ? true : false;
                objUserVM.IsPrivateSeller = user.UserAccountType == EnumUserAccountType.IndividualAdvertiser ? true : false;
                objUserVM.ClientName = user.ClientName;
                objUserVM.Website = user.Website;
                objUserVM.Remarks = user.Remarks;
                objUserVM.UserPortfolioListUrl = Url.Action("GetCompanyProducts", "Portfolio", new { userId = user.UserID });
                objlistUsers.Add(objUserVM);
            }
            PortfolioInfoViewModel objMarketInfoModel = new PortfolioInfoViewModel(CURRENCY_CODE)
            {
                ListUserAll = objlistUsers,
                PageName = "Today's Latest Ads",
                PostViewModel = new PostViewModel(CURRENCY_CODE)
                {
                    AV_State = DropDownDataList.GetAllStateList(),
                    AV_Category = DropDownDataList.GetCategoryList(),
                    AV_SubCategory = DropDownDataList.GetSubCategoryList()
                },
                
            };
         
            return View(@"../../Areas/LetItGo/Views/Portfolio/PortfolioDashboard", objMarketInfoModel);
        }


        [OutputCache(CacheProfile = "Cache60Mins")]
        public async Task<ViewResult> GetCompanyProducts(long userId = 0)
        {
            var res = _LoggingService.LogEntirePageVisit(EnumLogType.PortfolioLink, COUNTRY_CODE, HttpContext.Session.SessionID);
            MarketInfoViewModel objMarketInfoModel = new MarketInfoViewModel(CURRENCY_CODE)
            {
                PostViewModel = new PostViewModel(CURRENCY_CODE)
                {
                    AV_State = DropDownDataList.GetAllStateList(),
                    AV_Category = DropDownDataList.GetCategoryList(),
                    AV_SubCategory = DropDownDataList.GetSubCategoryList()
                },
                CategorySearchInfoModel = new CategorySearchInfoModel()
                {
                    PageLocation = "AllMarket"
                },
                PageName = "Today's Latest Ads"
            };
            objMarketInfoModel = await LoadPageingSpecificPosts(objMarketInfoModel, userId, "ViewItemDetail", "AllItemMarket");
            
            return View(@"../../Areas/LetItGo/Views/Portfolio/CompanyProducts", objMarketInfoModel);
        }

        private async Task<MarketInfoViewModel> LoadPageingSpecificPosts(MarketInfoViewModel objModel, long userId, string action, string controller)
        {
            var viewPostDetUrl = Url.Action(action, controller, new { postid = "POST_ITEM_ID" });
            listPostVM = await _PostMangementService.GetAllPublishedPostsByUserID(userId, 
                EnumCountry.Bangladesh,
                CURRENCY_CODE);
            objModel.ListPostsAll = listPostVM;
            return objModel;
        }
    }
}