using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Common;
using Data;

namespace WebDeshiHutBazar
{
    public class ImageController : PageingController
    {
        private readonly IImageProcessingService _ImageProcessingService;
        private readonly ILoggingService _LoggingService;
        private readonly IPostMangementService _PostMangementService;
        private readonly IGroupPanelConfigService _GroupPanelConfigService;

        public ImageController(ILoggingService loggingService, 
            IImageProcessingService imageProcessingService,
            IPostMangementService postManagementService,
            IGroupPanelConfigService groupPanelConfigService)
        {
            _LoggingService = loggingService;
            _PostMangementService = postManagementService;
            _GroupPanelConfigService = groupPanelConfigService;
            _ImageProcessingService = imageProcessingService;
        }

        [OutputCache(CacheProfile = "Cache2Mins")]
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<JsonResult> ProcessImages()
        {
            if (GetSessionUserId() == -1)
                CheckLogoutRequirements();
            IFileRepository _Repo = new FileRepository();
            var files = await _Repo.GetFiles();
            foreach(var f in files)
            {
                f.Image = _ImageProcessingService.Resize_Picture(f.Image, 600, 600, 1);
                await _Repo.SaveFile(f);
            }
            return Json(true);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> DownloadFile(int fileID, long postID)
        {
            if (GetSessionUserId() == -1)
                CheckLogoutRequirements();

            var objPostViewModel = await _PostMangementService.GetPostByPostIDForEdit(postID, CURRENCY_CODE);
            var file = objPostViewModel.ListImages.FirstOrDefault(a => a.FileID == fileID);
            if (file == null)
                return File(new byte[1], System.Net.Mime.MediaTypeNames.Application.Octet, null);
            byte[] fileBytes = file.Image;
            var fileName = file.FileID.ToString().Replace(" ", "").Trim() + ".jpg";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [OutputCache(CacheProfile = "Cache2Mins")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ViewResult> Index(int pageNumber = 1)
        {
            if (GetSessionUserId() == -1)
                CheckLogoutRequirements();

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

            await _LoggingService.LogEntirePageVisit(EnumLogType.AllItemMarketLink, COUNTRY_CODE, HttpContext.Session.SessionID);

            objMarketInfoModel.CategorySearchInfoModel.PageLocation = "Image Download";
            objMarketInfoModel.ContentInfoViewModel.ListGroupPanelConfiguration = new List<GroupPanelTemplateDisplayViewModel>();
            objMarketInfoModel.PageName = "Search Market Page";
            return View(@"../../Areas/Admin/Views/Image/Index", objMarketInfoModel);
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
                                                                    pageNumber, MARKET_PAGE_SIZE, "Index", "Image", "", null);            
            objModel.ListPostsAll = GetPostListForPage(listPostsAll, pageNumber, MARKET_PAGE_SIZE);
        }
       
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ViewResult> SimpleSearchImage(PostViewModel searchModel)
        {
            ClearSearchResultListPostVM();
            ClearSearchPostViewModel();
            await _LoggingService.LogAdvancedSearch(searchModel, HttpContext.Session.SessionID);
            MarketInfoViewModel objMarketInfoModel = new MarketInfoViewModel(CURRENCY_CODE);
            ViewBag.Title = "Search Result";
            objMarketInfoModel.ListPostsAll = await GetSearchResultSimple(searchModel, COUNTRY_CODE);
            LoadCategorySearchInfo(objMarketInfoModel, "Image");
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
            return View(@"../../Areas/Admin/Views/Image/Index", objMarketInfoModel);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ViewResult> AdvancedSearchImage(PostViewModel searchModel)
        {
            ClearSearchResultListPostVM();
            ClearSearchPostViewModel();
            _ = await _LoggingService.LogAdvancedSearch(searchModel, HttpContext.Session.SessionID);
            MarketInfoViewModel objMarketInfoModel = new MarketInfoViewModel(CURRENCY_CODE);
            ViewBag.Title = "Search Result";
            objMarketInfoModel.ListPostsAll = await GetSearchResult(searchModel); ;
            LoadCategorySearchInfo(objMarketInfoModel, "Image");
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
            return View(@"../../Areas/Admin/Views/Image/Index", objMarketInfoModel);
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
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllMarket", new { postid = "POST_ITEM_ID" });
            var listInitialAllPostDataset = await _PostMangementService.GetAllPosts(
                COUNTRY_CODE, EnumPostType.Post, viewPostDetUrl, CURRENCY_CODE);
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

        private void SetImageSessions(PostViewModel objPostViewModel, long? postId)
        {
            if (ShouldSetFirstImage(objPostViewModel))
            {
                SetManagePostImageSession(postId.Value, 1, objPostViewModel.ListImages[0]);
            }
            if (ShouldSetSecondImage(objPostViewModel))
            {
                SetManagePostImageSession(postId.Value, 2, objPostViewModel.ListImages[1]);
            }
            if (ShouldSetThirdImage(objPostViewModel))
            {
                SetManagePostImageSession(postId.Value, 3, objPostViewModel.ListImages[2]);
            }
            if (ShouldSetFourthImage(objPostViewModel))
            {
                SetManagePostImageSession(postId.Value, 4, objPostViewModel.ListImages[3]);
            }
        }

        private bool ShouldSetFirstImage(PostViewModel objPostViewModel)
        {
            return objPostViewModel.ListImages.Count > 0 &&
                objPostViewModel.ListImages != null &&
                objPostViewModel.ListImages[0] != null;
        }

        private bool ShouldSetSecondImage(PostViewModel objPostViewModel)
        {
            return objPostViewModel.ListImages.Count > 1 &&
                objPostViewModel.ListImages != null &&
                objPostViewModel.ListImages[1] != null;
        }

        private bool ShouldSetThirdImage(PostViewModel objPostViewModel)
        {
            return objPostViewModel.ListImages.Count > 2 &&
                objPostViewModel.ListImages != null &&
                objPostViewModel.ListImages[2] != null;
        }

        private bool ShouldSetFourthImage(PostViewModel objPostViewModel)
        {
            return objPostViewModel.ListImages.Count > 3 &&
                objPostViewModel.ListImages != null &&
                objPostViewModel.ListImages[3] != null;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public PartialViewResult ImageLoadModifyMode(long? postId, long? serialNo)
        {
            var objImage = GetManagePostImageSession(postId.Value, serialNo.Value);
            return PartialView(@"../../Areas/Admin/Views/Image/_Image", objImage ?? new FileViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<PartialViewResult> EditPostByPostId(long? postId)
        {
            if (!postId.HasValue)
            {
                return PartialView(@"../../Areas/Admin/Views/Image/_ModifyPost", null);
            }
            var objPostViewModel = await _PostMangementService.GetPostByPostIDForEdit(postId.Value, CURRENCY_CODE);
            if (Session == null)
            {
                return PartialView(@"../../Areas/Admin/Views/Image/_ModifyPost", objPostViewModel);
            }
            ClearManagePostImageSession(postId.Value);
            SetImageSessions(objPostViewModel, postId);
            return PartialView(@"../../Areas/Admin/Views/Image/_ModifyPost", objPostViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<PartialViewResult> ViewPostByPostId(long? postId)
        {
            if (postId.HasValue)
            {
                var postVm = await _PostMangementService.GetPostByPostIDForEdit(postId.Value, CURRENCY_CODE);
                return PartialView(@"../../Areas/Admin/Views/Image/_ViewPost", postVm);
            }
            return PartialView(@"../../Areas/Admin/Views/Image/_ViewPost", null);
        }


        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult ImageUploadModify(HttpPostedFileBase file, long? postId, long? serialNo)
        {            
            if (file != null && file.ContentLength > 0)
            {
                if (file.ContentLength > (1024 * 1024 * 10))
                {
                }
                else
                {
                    if (!string.IsNullOrEmpty(file.ContentType) && file.FileName != null)
                    {
                        string extension = Path.GetExtension(file.FileName).ToLower();
                        if (extension.Equals(".jpg") || extension.Equals(".jpeg")
                            || extension.Equals(".png") || extension.Equals(".gif"))
                        {
                            var imgByte = new Byte[file.ContentLength];
                            file.InputStream.Read(imgByte, 0, file.ContentLength);
                            var objFile = new FileViewModel { Image = imgByte };

                            SetManagePostImageSession(postId.Value, serialNo.Value, objFile);
                        }
                    }
                }
            }
            return null;
        }

        private void SetImageInViewModel(PostViewModel objPostVM, long postId)
        {
            var objFileList = new List<FileViewModel>();
            if (GetManagePostImageSession(postId, 1) != null)
            {
                var fileVm1 = GetManagePostImageSession(postId, 1);
                fileVm1.Image = _ImageProcessingService.GetResizedImage(fileVm1.Image, 800, 500);
                objFileList.Add(fileVm1);
            }
            if (GetManagePostImageSession(postId, 2) != null)
            {
                var fileVm2 = GetManagePostImageSession(postId, 2);
                fileVm2.Image = _ImageProcessingService.GetResizedImage(fileVm2.Image, 800, 500);
                objFileList.Add(fileVm2);
            }
            if (GetManagePostImageSession(postId, 3) != null)
            {
                var fileVm3 = GetManagePostImageSession(postId, 3);
                fileVm3.Image = _ImageProcessingService.GetResizedImage(fileVm3.Image, 800, 500);
                objFileList.Add(fileVm3);
            }
            if (GetManagePostImageSession(postId, 4) != null)
            {
                var fileVm4 = GetManagePostImageSession(postId, 4);
                fileVm4.Image = _ImageProcessingService.GetResizedImage(fileVm4.Image, 800, 500);
                objFileList.Add(fileVm4);
            }
            objPostVM.ListImages = objFileList;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> UpdatePostImages(PostViewModel objPostVm)
        {
            SetImageInViewModel(objPostVm, objPostVm.PostID);
            _ = await _PostMangementService.UpdatePostImages(objPostVm, objPostVm.PostID, COUNTRY_CODE);
            var userId = objPostVm.UserID;
            return Json(userId);
        }


    }
}