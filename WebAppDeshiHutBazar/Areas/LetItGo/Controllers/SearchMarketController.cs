using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using Common;

using System;

using Microsoft.Ajax.Utilities;
using Data;

namespace WebDeshiHutBazar
{
    public class SearchMarketController : PageingController
    {
        private readonly ILoggingService _LoggingService;
        private readonly IPostMangementService _PostMangementService;
        private readonly IGroupPanelConfigService _GroupPanelConfigService;
        private readonly ILogPostRepository _ILogPostRepository;

        public SearchMarketController()
        { }

        public SearchMarketController(ILoggingService loggingService,
            IPostMangementService postMangementService,
            IGroupPanelConfigService groupPanelConfigService,
            ILogPostRepository logPostRepository)
        {
            _LoggingService = loggingService;
            _PostMangementService = postMangementService;
            _GroupPanelConfigService = groupPanelConfigService;
            _ILogPostRepository = logPostRepository;
        }

        [OutputCache(CacheProfile = "Cache2Mins")]
        public async Task<ViewResult> Market(string tab, int pageNumber = 1)
        {
            var searchPostVM = GetSearchPostViewModel();
            MarketInfoViewModel objMarketInfoModel = new MarketInfoViewModel(CURRENCY_CODE)
            {
                PostViewModel = new PostViewModel(CURRENCY_CODE)
                {
                    AV_State = DropDownDataList.GetAllStateList(),
                    AV_Category = DropDownDataList.GetCategoryList(),
                    AV_SubCategory = DropDownDataList.GetSubCategoryList()
                }
            };
            if (searchPostVM != null)
            {
                objMarketInfoModel.PostViewModel = searchPostVM;
            }
            var res = await _LoggingService.LogEntirePageVisit(EnumLogType.AllItemMarketLink, COUNTRY_CODE, HttpContext.Session.SessionID);
            objMarketInfoModel = LoadTabSpecificPosts(objMarketInfoModel, pageNumber);
            objMarketInfoModel.CategorySearchInfoModel.PageLocation = "SearchMarket";
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
            var viewMoreUrl = Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" });
            objMarketInfoModel.PageName = "Search Result";
            return View(@"../../Areas/LetItGo/Views/SearchMarket/Market", objMarketInfoModel);
        }
        
        private void LoadCategorySearchInfo(MarketInfoViewModel objMarketInfoViewModel, string pageLocation)
        {
            SearchModel searchModel = GetSessionSearchModel();
            if (searchModel == null)
                searchModel = new SearchModel();
            objMarketInfoViewModel.CategorySearchInfoModel.SearchModel = searchModel;
            objMarketInfoViewModel.CategorySearchInfoModel.PageLocation = pageLocation;
        }
        
        private MarketInfoViewModel LoadTabSpecificPosts(MarketInfoViewModel objModel, int pageNumber)
        {
            List<PostViewModel> listPostVM = GetSearchResultListPostVM();
            if (listPostVM == null || listPostVM.Count == 0)
            {
                listPostVM = objModel.ListPostsAll;
            }
            LoadPageSpecificAllPosts(objModel, listPostVM, pageNumber);
            return objModel;
        }

        private void LoadPageSpecificAllPosts(MarketInfoViewModel objModel, List<PostViewModel> listPostsAll, int pageNumber)
        {
            objModel.PageingModelAll = SetPageingModel(objModel.PageingModelAll, listPostsAll.Count,
                                                                    pageNumber, MARKET_PAGE_SIZE
                                                                    , "Market", "SearchMarket", "", null);
            objModel.ListPostsAll = GetPostListForPage(listPostsAll, pageNumber, MARKET_PAGE_SIZE);
        }

        [HttpPost]
        public async Task<ViewResult> SimpleSearch(PostViewModel searchModel)
        {
            ClearSearchResultListPostVM();
            ClearSearchPostViewModel();
            var res = await _LoggingService.LogAdvancedSearch(searchModel, HttpContext.Session.SessionID);
            MarketInfoViewModel objMarketInfoModel = new MarketInfoViewModel(CURRENCY_CODE);
            List<PostViewModel> allPosts = new List<PostViewModel>();
            ViewBag.Title = "Search Result";
            allPosts = await GetSearchResultSimple(searchModel, COUNTRY_CODE);
            objMarketInfoModel.ListPostsAll = allPosts;
            
            LoadCategorySearchInfo(objMarketInfoModel, "SearchMarket");
            objMarketInfoModel.PageName = "Search Market Page";

            PostViewModel objPostVM = new PostViewModel(CURRENCY_CODE)
            {
                AV_State = DropDownDataList.GetAllStateList(),
                AV_Category = DropDownDataList.GetCategoryList(),
                AV_SubCategory = DropDownDataList.GetSubCategoryList(),
                CategoryID = searchModel.CategoryID,
                SubCategoryID = searchModel.SubCategoryID,
                SearchKey = searchModel.SearchKey,
                SimpleSearchKey = searchModel.SimpleSearchKey,
                StateID = searchModel.StateID,
                IsUrgent = searchModel.IsUrgent,
                IsUsed = searchModel.IsUsed,
                IsPrivateSeller = searchModel.IsPrivateSeller,
                IsCompanySeller = searchModel.IsCompanySeller,
                IsForSell = searchModel.IsForSell,
                IsForRent = searchModel.IsForRent,
                IsBrandNew = searchModel.IsBrandNew,
                PriceHigh = searchModel.PriceHigh,
                PriceLow = searchModel.PriceLow
            };
            objMarketInfoModel.PostViewModel = objPostVM;
            SetSearchPostViewModel(objPostVM);
            SetSearchResultListPostVM(objMarketInfoModel.ListPostsAll);
            objMarketInfoModel = LoadTabSpecificPosts(objMarketInfoModel,1);

            return View(@"../../Areas/LetItGo/Views/SearchMarket/Market", objMarketInfoModel);
        }

        [HttpPost]
        public async Task<ViewResult> AdvancedSearch(PostViewModel searchModel)
        {
            ClearSearchResultListPostVM();
            ClearSearchPostViewModel();
            var res = await _LoggingService.LogAdvancedSearch(searchModel, HttpContext.Session.SessionID);
            MarketInfoViewModel objMarketInfoModel = new MarketInfoViewModel(CURRENCY_CODE);
            List<PostViewModel> allPosts = new List<PostViewModel>();
            ViewBag.Title = "Search Result";
            allPosts = await GetSearchResult(searchModel);
            objMarketInfoModel.ListPostsAll = allPosts;
            LoadCategorySearchInfo(objMarketInfoModel, "SearchMarket");
            objMarketInfoModel.PageName = "Search Market Page";
            PostViewModel objPostVM = new PostViewModel(CURRENCY_CODE)
            {
                AV_State = DropDownDataList.GetAllStateList(),
                AV_Category = DropDownDataList.GetCategoryList(),
                AV_SubCategory = DropDownDataList.GetSubCategoryList(),
                CategoryID = searchModel.CategoryID,
                SubCategoryID = searchModel.SubCategoryID,
                SearchKey = searchModel.SearchKey,
                SimpleSearchKey = searchModel.SimpleSearchKey,
                StateID = searchModel.StateID,
                IsUrgent = searchModel.IsUrgent,
                IsUsed = searchModel.IsUsed,
                IsPrivateSeller = searchModel.IsPrivateSeller,
                IsCompanySeller = searchModel.IsCompanySeller,
                IsForSell = searchModel.IsForSell,
                IsForRent = searchModel.IsForRent,
                IsBrandNew = searchModel.IsBrandNew,
                PriceHigh = searchModel.PriceHigh,
                PriceLow = searchModel.PriceLow
            };
            objMarketInfoModel.PostViewModel = objPostVM;
            SetSearchPostViewModel(objPostVM);
            SetSearchResultListPostVM(objMarketInfoModel.ListPostsAll);
            LoadTabSpecificPosts(objMarketInfoModel, 1);
            return View(@"../../Areas/LetItGo/Views/SearchMarket/Market", objMarketInfoModel);
        }

        private async Task<List<PostViewModel>> GetSearchResultSimple(PostViewModel searchModel, EnumCountry country)
        {
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
            var listInitialAllPostDataset = await _PostMangementService.GetAllPosts(
                country, EnumPostType.Post, viewPostDetUrl, CURRENCY_CODE);
            List<PostViewModel> listFinalResult = new List<PostViewModel>();
            List<PostViewModel> listFilterBasedFinalPrimaryResult = new List<PostViewModel>();
            List<PostViewModel> listSearchKeyBasedPrimaryResult = new List<PostViewModel>();
            if (!string.IsNullOrEmpty(searchModel.SimpleSearchKey))
            {
                listSearchKeyBasedPrimaryResult = GetSimpleKeyBasedPrimaryResult(searchModel, listInitialAllPostDataset);
                if (IsThisCategoryBasedPrimarySet(searchModel))
                {
                    listFilterBasedFinalPrimaryResult = GetCategoryBasedPrimaryResult(searchModel, listInitialAllPostDataset);
                    listFilterBasedFinalPrimaryResult = GetFinalSetAfterFiltering(listFilterBasedFinalPrimaryResult, searchModel);
                }
                else
                {
                    listFilterBasedFinalPrimaryResult = listSearchKeyBasedPrimaryResult;
                    listFilterBasedFinalPrimaryResult = GetFinalSetAfterFilteringMainSet(listFilterBasedFinalPrimaryResult, searchModel);
                }
                listFinalResult = listFinalResult.Concat(listSearchKeyBasedPrimaryResult).Concat(listFilterBasedFinalPrimaryResult).ToList();
            }
            else if (string.IsNullOrEmpty(searchModel.SimpleSearchKey))
            {
                if (IsThisCategoryBasedPrimarySet(searchModel))
                {
                    listFilterBasedFinalPrimaryResult = GetCategoryBasedPrimaryResult(searchModel, listInitialAllPostDataset);
                    listFinalResult = GetFinalSetAfterFiltering(listFilterBasedFinalPrimaryResult, searchModel);
                }
                else
                {
                    listFinalResult = GetFinalSetAfterFilteringMainSet(listInitialAllPostDataset, searchModel);
                }
            }
            return listFinalResult.GroupBy(p => p.PostID).Select(g => g.First()).OrderByDescending(a => a.CreatedDate).ThenBy(a => a.CategoryID).ThenBy(a => a.SubCategoryID).ToList();
        }

        private async Task<List<PostViewModel>> GetSearchResult(PostViewModel searchModel)
        {
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
            var listInitialAllPostDataset = await _PostMangementService.GetAllPosts(COUNTRY_CODE, EnumPostType.Post, viewPostDetUrl, CURRENCY_CODE);
            List<PostViewModel> listFinalResult = new List<PostViewModel>();
            List<PostViewModel> listFilterBasedFinalPrimaryResult = new List<PostViewModel>();
            List<PostViewModel> listSearchKeyBasedPrimaryResult = new List<PostViewModel>();
            if (!string.IsNullOrEmpty(searchModel.SearchKey))
            {
                listSearchKeyBasedPrimaryResult = GetKeyBasedPrimaryResult(searchModel, listInitialAllPostDataset);
                if (IsThisCategoryBasedPrimarySet(searchModel))
                {
                    listFilterBasedFinalPrimaryResult = GetCategoryBasedPrimaryResult(searchModel, listInitialAllPostDataset);
                    listFilterBasedFinalPrimaryResult = GetFinalSetAfterFiltering(listFilterBasedFinalPrimaryResult, searchModel);
                }
                else
                {
                    listFilterBasedFinalPrimaryResult = listSearchKeyBasedPrimaryResult;
                    listFilterBasedFinalPrimaryResult = GetFinalSetAfterFilteringMainSet(listFilterBasedFinalPrimaryResult, searchModel);
                }
                listFinalResult = listFinalResult.Concat(listSearchKeyBasedPrimaryResult).Concat(listFilterBasedFinalPrimaryResult).ToList();
            }
            else if (string.IsNullOrEmpty(searchModel.SearchKey))
            {
                if (IsThisCategoryBasedPrimarySet(searchModel))
                {
                    listFilterBasedFinalPrimaryResult = GetCategoryBasedPrimaryResult(searchModel, listInitialAllPostDataset);
                    listFinalResult = GetFinalSetAfterFiltering(listFilterBasedFinalPrimaryResult, searchModel);
                }
                else
                {
                    listFinalResult = GetFinalSetAfterFilteringMainSet(listInitialAllPostDataset, searchModel);
                }
            }
            return listFinalResult.GroupBy(p => p.PostID).Select(g => g.First()).OrderByDescending(a => a.CreatedDate).ThenBy(a => a.CategoryID).ThenBy(a => a.SubCategoryID).ToList();
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
                return listCategoryBasedPrimaryResult.Where(a=>a.IsUrgent).ToList();
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
                return listCategoryBasedPrimaryResult.Where(a=> a.IsBrandNew).ToList();
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