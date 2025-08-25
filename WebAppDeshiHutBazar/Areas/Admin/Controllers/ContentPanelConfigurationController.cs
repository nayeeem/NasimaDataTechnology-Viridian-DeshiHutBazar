using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using Common;
using System.IO;
using System;
using System.Web;

namespace WebDeshiHutBazar
{
    public class ContentPanelConfigurationController : BaseController
    {
        private readonly IGroupPanelConfigService _GroupPanelConfigService;
        private readonly IRepoDropDownDataList _IRepoDropDownDataList;
        private readonly IPostMangementService _PostMangementService;

        public ContentPanelConfigurationController() 
        { }

        public ContentPanelConfigurationController(IGroupPanelConfigService groupPanelConfigService,
            IImageProcessingService imageProcessingService,
            IPostMangementService postManagementService,
            IRepoDropDownDataList repoDropDownDataList
            )
        {
            _GroupPanelConfigService = groupPanelConfigService;
            _IRepoDropDownDataList = repoDropDownDataList;
            _PostMangementService = postManagementService;
    }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Index(GroupPanelInformationViewModel model)
        {
            if (GetSessionUserId() == -1)
                CheckLogoutRequirements();

            EnumPublicPage? objPage = EnumPublicPage.Home;
            if (model != null && model.EnumPublicPage.HasValue)
                objPage = (EnumPublicPage)model.EnumPublicPage;

            EnumDeviceType? objDevice = EnumDeviceType.Desktop;
            if (model != null && model.EnumDevice.HasValue)
                objDevice = (EnumDeviceType)model.EnumDevice;
            var resultList = await _GroupPanelConfigService
                                                        .GetAllPageGroupPanelConfigurations(
                                                        objPage.Value,
                                                        Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" }),
                                                        Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" }),
                                                        COUNTRY_CODE,
                                                        CURRENT_TIME_SLOT,CURRENCY_CODE);
            List<SelectListItem> objListSubCates = new List<SelectListItem>();
            GroupPanelInformationViewModel objInformationModel = new GroupPanelInformationViewModel()
            {
                GroupPanelConfigurationViewModel = new GroupPanelConfigurationViewModel()
                {
                    AV_Device = DropDownDataList.GetDeviceTypeList(),
                    AV_ShowHide = DropDownDataList.GetShowHideList(),
                    AV_EnumPanelDisplayStyle = DropDownDataList.GetPanelDisplayPositionList(),
                    AV_EnumPublicPage = DropDownDataList.GetPageList(),
                    AV_Users = await _IRepoDropDownDataList.GetUsersList(),
                    PanelConfigUserID = 1,
                    PublicPage = objPage.Value,
                    Device = objDevice.Value,
                    ShowOrHide = EnumShowOrHide.Yes,
                    EnumPanelDisplayStyle = EnumPanelDisplayStyle.StarPanel,
                },
                AV_PageList = DropDownDataList.GetPageList(),
                AV_Device = DropDownDataList.GetDeviceTypeList(),
                PageName = "Configure Group Panel",
                EnumPublicPage = objPage.Value,
                EnumDevice = objDevice.Value
            };
            objInformationModel.ListGroupPanelTemplateConfiguration = resultList;
            return View(@"../../Areas/Admin/Views/ContentPanelConfiguration/Index", objInformationModel);
        }
        
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> AddSelectedPost(int postID, int groupConfigID, long fileId)
        {
            var listPostViewModel = await _GroupPanelConfigService.AddSelectedPost(
                postID, 
                groupConfigID, 
                fileId, 
                COUNTRY_CODE, 
                GetSessionUserId());
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> RemoveSelectedPost(int groupPostID)
        {
            var result = await _GroupPanelConfigService.RemoveSelectedPost(
                groupPostID, 
                GetSessionUserId(), 
                COUNTRY_CODE);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> AddNewGroupConfig(GroupPanelConfigurationViewModel objGroupConfig)
        {
            await _GroupPanelConfigService.AddGroupPanelConfig(
                objGroupConfig, 
                GetSessionUserId(), 
                COUNTRY_CODE);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> UpdateGroupConfig(GroupPanelConfigurationViewModel objGroupConfig)
        {
            _ = await _GroupPanelConfigService.UpdateGroupPanelConfig(
                objGroupConfig, 
                COUNTRY_CODE, 
                GetSessionUserId());
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult GetSubCategories(long categoryID)
        {
            IEnumerable<SelectListItem> listSubCategories = DropDownDataList.GetSubCategoryList(categoryID);
            return Json(listSubCategories, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> RemoveGroupConfig(int groupConfigID)
        {
            await _GroupPanelConfigService.DeleteGroupPanelConfig(
                groupConfigID, 
                GetSessionUserId(), 
                COUNTRY_CODE);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> PublishGroupPanelConfig(int device, int page, EnumCountry country)
        {
            if (device == 0 || page == 0)
                return Json(false);

            await _GroupPanelConfigService.PublishAllGroupPanelConfig(
                (EnumDeviceType)device, 
                (EnumPublicPage)page, 
                GetSessionUserId(),
                COUNTRY_CODE);
            return Json(true);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> UpdateDisplayOrder(PanelDisplayOrderViewModel objNewDisplayOrder)
        {
            if (objNewDisplayOrder == null)
                return Json(false);

            await _GroupPanelConfigService.UpdateDisplayOrder(
                objNewDisplayOrder, 
                COUNTRY_CODE, 
                GetSessionUserId());
            return Json(true);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> UpdatePostDisplayOrder(List<int> groupPostList)
        {
            if (groupPostList == null || groupPostList.Count==0)
                return Json(false);

            await _GroupPanelConfigService.UpdatePostDisplayOrder(
                groupPostList, 
                GetSessionUserId(), 
                COUNTRY_CODE);
            return Json(true);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<PartialViewResult> GetGroupPostsPartial(int groupConfigID)
        {
            var groupPanelConfigurationViewModel = await _GroupPanelConfigService.GetSingleGroupConfigPosts(
                groupConfigID, 
                COUNTRY_CODE,
                CURRENCY_CODE);
            return PartialView(@"../../Areas/Admin/Views/ContentPanelConfiguration/_DialogGroupPostsContent", groupPanelConfigurationViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<PartialViewResult> GetOrderGroupPostsPartial(int groupConfigID)
        {
            var groupPanelConfigurationViewModel = await _GroupPanelConfigService.GetSingleGroupConfigPosts(
                groupConfigID, 
                COUNTRY_CODE,
                CURRENCY_CODE);
            return PartialView(@"../../Areas/Admin/Views/ContentPanelConfiguration/_DialogOrderGroupPostsContent", groupPanelConfigurationViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<PartialViewResult> GetEditConfigPartial(int groupConfigID)
        {
            var groupPanelConfigurationViewModel = await _GroupPanelConfigService.GetSingleGroupPanelConfig(
                groupConfigID, 
                COUNTRY_CODE);
            return PartialView(@"../../Areas/Admin/Views/ContentPanelConfiguration/_DialogEditConfigContent", groupPanelConfigurationViewModel);
        }

        [HttpPost]
        public JsonResult GetTemplates(int pageID)
        {
            var result = DropDownDataList.GetPanelDisplayPositionList();
            if (pageID != 0)
            {
                EnumPublicPage enumPage = (EnumPublicPage)pageID;
                if (enumPage != EnumPublicPage.AdsDetail)
                {
                    result = result.Where(a => a.Value != "3333");
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<PartialViewResult> AdvancedSearchPanel(long? postTypeID, long categoryID, long subCategoryID)
        {            
            GroupPanelConfigurationViewModel objModel = new GroupPanelConfigurationViewModel();
            PostViewModel searchModel = new PostViewModel(CURRENCY_CODE)
            {
                PostTypeID = postTypeID
            };
            if (searchModel.PostTypeID.HasValue && searchModel.PostTypeID.Value == (Int64)EnumPostType.Post)
            {
                searchModel.CategoryID = categoryID;
                searchModel.SubCategoryID = subCategoryID;
                objModel.ListSelectPost = await GetSearchResult(searchModel);
            }
            else if (searchModel.PostTypeID.HasValue)
            {
                objModel.ListSelectPost = await _PostMangementService.GetAllPosts(
                    EnumCountry.Bangladesh, 
                    (EnumPostType)Convert.ToInt32(searchModel.PostTypeID), 
                    "", 
                    CURRENCY_CODE);
            }
            return PartialView(@"../../Areas/Admin/Views/ContentPanelConfiguration/_DialogSelectPostsContent", objModel);
        }

        private async Task<List<PostViewModel>> GetSearchResultSimple(PostViewModel searchModel, EnumCountry country)
        {
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllMarket", new { postid = "POST_ITEM_ID" });
            var listInitialAllPostDataset = await _PostMangementService.GetAllPosts(
                country, 
                EnumPostType.Post, 
                viewPostDetUrl, 
                CURRENCY_CODE);
            List<PostViewModel> listFinalResult = new List<PostViewModel>();
            List<PostViewModel> listFilterBasedFinalPrimaryResult = new List<PostViewModel>();
            List<PostViewModel> listSearchKeyBasedPrimaryResult = new List<PostViewModel>();
            if (!string.IsNullOrEmpty(searchModel.SimpleSearchKey))
            {
                listSearchKeyBasedPrimaryResult = GetSimpleKeyBasedPrimaryResult(
                    searchModel, 
                    listInitialAllPostDataset);
                if (IsThisCategoryBasedPrimarySet(searchModel))
                {
                    listFilterBasedFinalPrimaryResult = GetCategoryBasedPrimaryResult(
                        searchModel, 
                        listInitialAllPostDataset);
                    listFilterBasedFinalPrimaryResult = GetFinalSetAfterFiltering(
                        listFilterBasedFinalPrimaryResult, 
                        searchModel);
                }
                else
                {
                    listFilterBasedFinalPrimaryResult = listSearchKeyBasedPrimaryResult;
                    listFilterBasedFinalPrimaryResult = GetFinalSetAfterFilteringMainSet(
                        listFilterBasedFinalPrimaryResult, 
                        searchModel);
                }
                listFinalResult = listFinalResult
                    .Concat(listSearchKeyBasedPrimaryResult)
                    .Concat(listFilterBasedFinalPrimaryResult)
                    .ToList();
            }
            else if (string.IsNullOrEmpty(searchModel.SimpleSearchKey))
            {
                if (IsThisCategoryBasedPrimarySet(searchModel))
                {
                    listFilterBasedFinalPrimaryResult = GetCategoryBasedPrimaryResult(
                        searchModel, 
                        listInitialAllPostDataset);
                    listFinalResult = GetFinalSetAfterFiltering(
                        listFilterBasedFinalPrimaryResult, 
                        searchModel);
                }
                else
                {
                    listFinalResult = GetFinalSetAfterFilteringMainSet(
                        listInitialAllPostDataset, 
                        searchModel);
                }
            }
            return listFinalResult
                .GroupBy(p => p.PostID)
                .Select(g => g.First())
                .OrderByDescending(a => a.CreatedDate)
                .ThenBy(a => a.CategoryID)
                .ThenBy(a => a.SubCategoryID)
                .ToList();
        }

        private async Task<List<PostViewModel>> GetSearchResult(PostViewModel searchModel)
        {
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllMarket", new { postid = "POST_ITEM_ID" });
            var listInitialAllPostDataset = await _PostMangementService.GetAllPosts(
                COUNTRY_CODE, 
                EnumPostType.Post, 
                viewPostDetUrl, 
                CURRENCY_CODE);
            List<PostViewModel> listFinalResult = new List<PostViewModel>();
            List<PostViewModel> listFilterBasedFinalPrimaryResult = new List<PostViewModel>();
            List<PostViewModel> listSearchKeyBasedPrimaryResult = new List<PostViewModel>();
            if (!string.IsNullOrEmpty(searchModel.SearchKey))
            {
                listSearchKeyBasedPrimaryResult = GetKeyBasedPrimaryResult(searchModel, listInitialAllPostDataset);
                if (IsThisCategoryBasedPrimarySet(searchModel))
                {
                    listFilterBasedFinalPrimaryResult = GetCategoryBasedPrimaryResult(
                        searchModel, 
                        listInitialAllPostDataset);
                    listFilterBasedFinalPrimaryResult = GetFinalSetAfterFiltering(
                        listFilterBasedFinalPrimaryResult, 
                        searchModel);
                }
                else
                {
                    listFilterBasedFinalPrimaryResult = listSearchKeyBasedPrimaryResult;
                    listFilterBasedFinalPrimaryResult = GetFinalSetAfterFilteringMainSet(
                        listFilterBasedFinalPrimaryResult, 
                        searchModel);
                }
                listFinalResult = listFinalResult.Concat(listSearchKeyBasedPrimaryResult).Concat(listFilterBasedFinalPrimaryResult).ToList();
            }
            else if (string.IsNullOrEmpty(searchModel.SearchKey))
            {
                if (IsThisCategoryBasedPrimarySet(searchModel))
                {
                    listFilterBasedFinalPrimaryResult = GetCategoryBasedPrimaryResult(
                        searchModel, 
                        listInitialAllPostDataset);
                    listFinalResult = GetFinalSetAfterFiltering(
                        listFilterBasedFinalPrimaryResult, 
                        searchModel);
                }
                else
                {
                    listFinalResult = GetFinalSetAfterFilteringMainSet(
                        listInitialAllPostDataset, 
                        searchModel);
                }
            }
            return listFinalResult
                .GroupBy(p => p.PostID)
                .Select(g => g.First())
                .OrderByDescending(a => a.CreatedDate)
                .ThenBy(a => a.CategoryID)
                .ThenBy(a => a.SubCategoryID)
                .ToList();
        }

        private List<PostViewModel> GetFinalSetAfterFiltering(List<PostViewModel> listFilterBasedFinalPrimaryResult, PostViewModel searchModel)
        {
            listFilterBasedFinalPrimaryResult = GetIndivCompantyBasedFilteredResults(listFilterBasedFinalPrimaryResult, searchModel);
            listFilterBasedFinalPrimaryResult = GetStateBasedFilteredResults(listFilterBasedFinalPrimaryResult, searchModel);
            listFilterBasedFinalPrimaryResult = GetPriceFilteredResults(listFilterBasedFinalPrimaryResult, searchModel);
            listFilterBasedFinalPrimaryResult = GetNewOrUsedBasedFilteredResults(listFilterBasedFinalPrimaryResult, searchModel);
            listFilterBasedFinalPrimaryResult = GetRentOrSellBasedFilteredResults(listFilterBasedFinalPrimaryResult, searchModel);
            listFilterBasedFinalPrimaryResult = GetUrgentSellBasedFilteredResults(listFilterBasedFinalPrimaryResult, searchModel);
            return listFilterBasedFinalPrimaryResult;
        }

        private List<PostViewModel> GetFinalSetAfterFilteringMainSet(List<PostViewModel> listFilterBasedFinalPrimaryResult, PostViewModel searchModel)
        {
            listFilterBasedFinalPrimaryResult = GetIndivCompantyBasedFilteredResults(listFilterBasedFinalPrimaryResult, searchModel);
            listFilterBasedFinalPrimaryResult = GetPriceFilteredResults(listFilterBasedFinalPrimaryResult, searchModel);
            listFilterBasedFinalPrimaryResult = GetStateBasedFilteredResults(listFilterBasedFinalPrimaryResult, searchModel);
            listFilterBasedFinalPrimaryResult = GetNewOrUsedBasedFilteredResults(listFilterBasedFinalPrimaryResult, searchModel);
            listFilterBasedFinalPrimaryResult = GetRentOrSellBasedFilteredResults(listFilterBasedFinalPrimaryResult, searchModel);
            listFilterBasedFinalPrimaryResult = GetUrgentSellBasedFilteredResults(listFilterBasedFinalPrimaryResult, searchModel);
            return listFilterBasedFinalPrimaryResult;
        }

        private bool IsThisCategoryBasedPrimarySet(PostViewModel searchModel)
        {
            return searchModel.CategoryID.HasValue || searchModel.SubCategoryID.HasValue;
        }

        private List<PostViewModel> GetIndivCompantyBasedFilteredResults(List<PostViewModel> listCategoryBasedPrimaryResult, PostViewModel searchModel)
        {
            if (searchModel.IsPrivateSeller && searchModel.IsCompanySeller)
            {
                return listCategoryBasedPrimaryResult.ToList();
            }
            else if (searchModel.IsPrivateSeller)
            {
                return listCategoryBasedPrimaryResult.Where(a => a.IsPrivateSeller).ToList();
            }
            else if (searchModel.IsCompanySeller)
            {
                return listCategoryBasedPrimaryResult.Where(a => a.IsCompanySeller).ToList();
            }
            else
            {
                return listCategoryBasedPrimaryResult.ToList();
            }
        }

        private List<PostViewModel> GetUrgentSellBasedFilteredResults(List<PostViewModel> listCategoryBasedPrimaryResult, PostViewModel searchModel)
        {
            if (searchModel.IsUrgent)
            {
                return listCategoryBasedPrimaryResult.Where(a => a.IsUrgent).ToList();
            }
            else
            {
                return listCategoryBasedPrimaryResult.ToList();
            }
        }

        private List<PostViewModel> GetRentOrSellBasedFilteredResults(List<PostViewModel> listCategoryBasedPrimaryResult, PostViewModel searchModel)
        {
            if (searchModel.IsForRent && searchModel.IsForSell)
            {
                return listCategoryBasedPrimaryResult.ToList();
            }
            else if (searchModel.IsForSell)
            {
                return listCategoryBasedPrimaryResult.Where(a => a.IsForSell).ToList();
            }
            else if (searchModel.IsForRent)
            {
                return listCategoryBasedPrimaryResult.Where(a => a.IsForRent).ToList();
            }
            else
            {
                return listCategoryBasedPrimaryResult.ToList();
            }
        }

        private List<PostViewModel> GetPriceFilteredResults(List<PostViewModel> listCategoryBasedPrimaryResult, PostViewModel searchModel)
        {
            if (searchModel.PriceLow.HasValue && searchModel.PriceHigh.HasValue)
            {
                return listCategoryBasedPrimaryResult.Where(a =>
                                    a.Price >= (long)searchModel.PriceLow.Value &&
                                    a.Price <= (long)searchModel.PriceHigh.Value).ToList();
            }
            else if (searchModel.PriceLow.HasValue)
            {
                return listCategoryBasedPrimaryResult.Where(a =>
                                    a.Price >= (long)searchModel.PriceLow.Value).ToList();
            }
            else if (searchModel.PriceHigh.HasValue)
            {
                return listCategoryBasedPrimaryResult.Where(a =>
                                    a.Price <= (long)searchModel.PriceHigh.Value).ToList();
            }
            else
            {
                return listCategoryBasedPrimaryResult.ToList();
            }
        }

        private List<PostViewModel> GetStateBasedFilteredResults(List<PostViewModel> listCategoryBasedPrimaryResult, PostViewModel searchModel)
        {
            if (searchModel.StateID.HasValue)
            {
                return listCategoryBasedPrimaryResult.Where(a => a.StateID == searchModel.StateID).ToList();
            }
            return listCategoryBasedPrimaryResult;
        }

        private List<PostViewModel> GetNewOrUsedBasedFilteredResults(List<PostViewModel> listCategoryBasedPrimaryResult, PostViewModel searchModel)
        {
            if (searchModel.IsBrandNew && searchModel.IsUsed)
            {
                return listCategoryBasedPrimaryResult.ToList();
            }
            else if (searchModel.IsBrandNew)
            {
                return listCategoryBasedPrimaryResult.Where(a => a.IsBrandNew).ToList();
            }
            else if (searchModel.IsUsed)
            {
                return listCategoryBasedPrimaryResult.Where(a => a.IsUsed).ToList();
            }
            else
            {
                return listCategoryBasedPrimaryResult.ToList();
            }
        }

        private List<PostViewModel> GetCategoryBasedPrimaryResult(PostViewModel searchModel, List<PostViewModel> listPostEntities)
        {
            List<PostViewModel> listFinalPostResultsVM = new List<PostViewModel>();
            if (searchModel.SubCategoryID > 0)
            {
                listFinalPostResultsVM = GetSubCategoryList(searchModel, listPostEntities);
            }
            else
            {
                listFinalPostResultsVM = GetCategoryList(searchModel, listPostEntities);
            }
            return listFinalPostResultsVM;
        }

        private List<PostViewModel> GetKeyBasedPrimaryResult(PostViewModel searchModel, List<PostViewModel> listPostEntities)
        {
            List<PostViewModel> listSearchKeyBasedPrimaryResult = new List<PostViewModel>();
            if (!string.IsNullOrEmpty(searchModel.SearchKey))
            {
                string[] arrKeys = searchModel.SearchKey.Split(' ');
                var listPostsSubQueryResultsVM = new List<PostViewModel>();
                foreach (var key in arrKeys)
                {
                    listPostsSubQueryResultsVM = new List<PostViewModel>();
                    listPostsSubQueryResultsVM = listPostEntities.Where(a =>
                            (!string.IsNullOrEmpty(a.AreaDescription) && a.AreaDescription.ToLower().Contains(key.ToLower())) ||
                            (!string.IsNullOrEmpty(a.Title) && a.Title.ToLower().Contains(key.ToLower())) ||
                            (!string.IsNullOrEmpty(a.Description) && a.Description.ToLower().Contains(key.ToLower())) ||
                            (!string.IsNullOrEmpty(a.DisplayCategory) && a.DisplayCategory.ToLower().Contains(key.ToLower())) ||
                            (!string.IsNullOrEmpty(a.DisplaySubCategory) && a.DisplaySubCategory.ToLower().Contains(key.ToLower())) ||
                            (!string.IsNullOrEmpty(a.SearchTag) && a.SearchTag.ToLower().Contains(key.ToLower()))).ToList();
                    listSearchKeyBasedPrimaryResult = listSearchKeyBasedPrimaryResult.Concat(listPostsSubQueryResultsVM).ToList();
                }
            }
            return listSearchKeyBasedPrimaryResult;
        }

        private List<PostViewModel> GetSimpleKeyBasedPrimaryResult(PostViewModel searchModel, List<PostViewModel> listPostEntities)
        {
            List<PostViewModel> listSearchKeyBasedPrimaryResult = new List<PostViewModel>();
            if (!string.IsNullOrEmpty(searchModel.SimpleSearchKey))
            {
                string[] arrKeys = searchModel.SimpleSearchKey.Split(' ');
                var listPostsSubQueryResultsVM = new List<PostViewModel>();
                foreach (var key in arrKeys)
                {
                    listPostsSubQueryResultsVM = new List<PostViewModel>();
                    listPostsSubQueryResultsVM = listPostEntities.Where(a =>
                            (!string.IsNullOrEmpty(a.AreaDescription) && a.AreaDescription.ToLower().Contains(key.ToLower())) ||
                            (!string.IsNullOrEmpty(a.Title) && a.Title.ToLower().Contains(key.ToLower())) ||
                            (!string.IsNullOrEmpty(a.Description) && a.Description.ToLower().Contains(key.ToLower())) ||
                            (!string.IsNullOrEmpty(a.DisplayCategory) && a.DisplayCategory.ToLower().Contains(key.ToLower())) ||
                            (!string.IsNullOrEmpty(a.DisplaySubCategory) && a.DisplaySubCategory.ToLower().Contains(key.ToLower())) ||
                            (!string.IsNullOrEmpty(a.SearchTag) && a.SearchTag.ToLower().Contains(key.ToLower()))).ToList();
                    listSearchKeyBasedPrimaryResult = listSearchKeyBasedPrimaryResult.Concat(listPostsSubQueryResultsVM).ToList();
                }
            }
            return listSearchKeyBasedPrimaryResult;
        }

        private List<PostViewModel> GetSubCategoryList(PostViewModel searchModel, List<PostViewModel> listPostVM)
        {
            var listPostsSubQueryResultsVM = new List<PostViewModel>();
            listPostsSubQueryResultsVM = listPostVM.Where(a =>
                                a.SubCategoryID == searchModel.SubCategoryID)
                                .ToList();
            return listPostsSubQueryResultsVM.ToList();
        }

        private List<PostViewModel> GetCategoryList(PostViewModel searchModel, List<PostViewModel> listPostEntities)
        {
            var listPostsSubQueryResultsVM = new List<PostViewModel>();
            listPostsSubQueryResultsVM = listPostEntities.Where(a =>
                                a.CategoryID == searchModel.CategoryID)
                                .ToList();
            return listPostsSubQueryResultsVM;
        }                
    }
}
