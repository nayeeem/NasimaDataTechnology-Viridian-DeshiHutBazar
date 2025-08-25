using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Common;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace WebDeshiHutBazar
{
    public class ManagePostController : PageingController
    {
        private readonly IImageProcessingService _ImageProcessingService;
        private readonly IBillManagementService _BillManagementService;
        private readonly IPostMangementService _PostManagementService;

        public ManagePostController(IImageProcessingService imageProcessingService,
            IBillManagementService billManagementService,
            IPostMangementService postManagementService) 
        {           
            _ImageProcessingService = imageProcessingService;
            _BillManagementService = billManagementService;
            _PostManagementService = postManagementService;
        }

        public ManagePostController() 
        { }

        private ManagePostViewModel LoadTabSpecificPosts(ManagePostViewModel objModel, int pageNumber, string action, string controller)
        {
            objModel.PageingModelAll = SetPageingModel(objModel.PageingModelAll, objModel.ListPostViewModel.Count, pageNumber, MARKET_PAGE_SIZE,
                action, controller, "", null);
            objModel.ListPostViewModel = GetPostListForPage(objModel.ListPostViewModel, pageNumber, MANAGE_POST_PAGE_SIZE);
            return objModel;
        }
                
        [Authorize(Roles = "Company, Advertiser")]
        public ActionResult Manage()
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();
            return RedirectToAction("OnlyPublishedPosts", "ManagePost");  
        }
        
        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> UnpaidPost(int pageNumber = 1)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var objManagePostVM = new ManagePostViewModel(CURRENCY_CODE)
            {
                ListPostViewModel =
                await _PostManagementService.GetAllUnpaidPostsByUserID(
                    userId,
                    COUNTRY_CODE,
                    CURRENCY_CODE)
            };
            objManagePostVM.PageName = "Unpaid Posts Page";
            objManagePostVM = LoadTabSpecificPosts(objManagePostVM,
                pageNumber,
                "UnpaidPost",
                "ManagePost");
            return View(@"../../Areas/Advertizer/Views/ManagePost/ManageUnpaidPosts", objManagePostVM);
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> OnlyPublishedPosts(int pageNumber = 1)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var objManagePostVM = new ManagePostViewModel(CURRENCY_CODE)
            {
                ListPostViewModel =
                await _PostManagementService.GetAllPublishedPostsByUserID(
                    userId,
                    COUNTRY_CODE,
                    CURRENCY_CODE)
            };
            objManagePostVM = LoadTabSpecificPosts(objManagePostVM,
                pageNumber,
                "OnlyPublishedPosts",
                "ManagePost");
            objManagePostVM.PageName = "Published Posts Page";
            return View(@"../../Areas/Advertizer/Views/ManagePost/ManagePublishedPosts", objManagePostVM);
        }        

        [HttpPost]
        [Authorize(Roles = "Company, Advertiser")]
        public async Task<PartialViewResult> EditPostByPostId(long? postId)
        {
            ClearNewPostImageSessions();
            if (!postId.HasValue)
            {
                return PartialView(@"../../Areas/Advertizer/Views/ManagePost/_ModifyPost", null);
            }
            var objPostViewModel = await _PostManagementService.GetPostByPostIDForEdit(
                postId.Value,
                CURRENCY_CODE);
            if (Session == null)
            {
                return PartialView(@"../../Areas/Advertizer/Views/ManagePost/_ModifyPost", objPostViewModel);
            }
            return PartialView(@"../../Areas/Advertizer/Views/ManagePost/_ModifyPost", objPostViewModel);
        }
       
        [HttpPost]
        [Authorize(Roles = "Company, Advertiser")]
        public async Task<PartialViewResult> ViewPostByPostId(long? postId)
        {
            ClearNewPostImageSessions();
            if (postId.HasValue)
            {
                var postVm = await _PostManagementService.GetPostByPostIDReadonly(
                    postId.Value,
                    CURRENCY_CODE);
                return PartialView(@"../../Areas/Advertizer/Views/ManagePost/_ViewPost", postVm);
            }
            return PartialView(@"../../Areas/Advertizer/Views/ManagePost/_ViewPost", null);
        }

        [HttpPost]
        [Authorize(Roles = "Company, Advertiser")]
        public async Task<JsonResult> UpdatePost(PostViewModel objPostVm)
        {
            SetImageInViewModel(objPostVm, objPostVm.PostID);
            var result = await _PostManagementService.UpdatePost(
                objPostVm,
                objPostVm.PostID,
                COUNTRY_CODE);
            var userId = objPostVm.UserID;
            return Json(userId);
        }

        [HttpPost]
        [Authorize(Roles = "Company, Advertiser")]
        public async Task<JsonResult> DeletePostByPostId(long postId)
        {
            if (postId == default(long))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var res = await _PostManagementService.DeletePost(
                    postId,
                    GetSessionUserId(),
                    COUNTRY_CODE);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }
        }

        #region Image Upload Methods
        [HttpPost]
        public JsonResult ImageUpload(HttpPostedFileBase file)
        {
            string appPath = HttpContext.Request.ApplicationPath;
            if (appPath == "/")
                appPath = "";
            if (file != null && file.ContentLength > 0)
            {
                if (file.ContentLength > POST_IMAGE_SIZE)
                {
                    return Json(false);
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
                            FileViewModel objFile = new FileViewModel { Image = imgByte, FileName = file.FileName, FileType = extension };
                            SetSessionNewPostImage(objFile);
                        }
                    }

                }
            }
            return Json(true);
        }

        [HttpGet]
        public PartialViewResult ImageLoad()
        {
            var objImageList = GetSessionNewPostImage();
            var objImage = objImageList.Last();
            return PartialView(@"../../Areas/LetItGo/Views/ManagePost/_Image", objImage);
        }

        [HttpGet]
        public JsonResult ImageRemove(long id)
        {
            if (!RemoveSessionNewPostImage(id))
            {
                _PostManagementService.DeletePostImage(id);
            }
            return Json(true);
        }

        private void SetImageInViewModel(PostViewModel objPostVm)
        {
            List<FileViewModel> objFileList = new List<FileViewModel>();
            objFileList = GetSessionNewPostImage();
            objPostVm.ListImages = objFileList;
            ClearNewPostImageSessions();
        }
        #endregion
        [Authorize(Roles = "Company, Advertiser")]
        private void SetImageInViewModel(PostViewModel objPostVM, long postId)
        {
            var objFileList = new List<FileViewModel>();
            var listImgs = GetSessionNewPostImage();
            objPostVM.ListImages = listImgs;
            ClearNewPostImageSessions();
        }

        [HttpPost]
        [Authorize(Roles = "Company, Advertiser")]
        public JsonResult GetSubCategories(long categoryValueId)
        {
            var listSubCategories = DropDownDataList.GetSubCategoryList(categoryValueId);
            return Json(listSubCategories, JsonRequestBehavior.AllowGet);
        }
                
        [HttpPost]
        [Authorize(Roles = "Company, Advertiser")]
        public JsonResult GetSubCategoryText(long subCategoryId)
        {
            var subcategorytext = BusinessObjectSeed.GetCatSubCategoryItemTextForId(subCategoryId);
            return Json(subcategorytext);
        }

        [HttpPost]
        [Authorize(Roles = "Company, Advertiser")]
        public JsonResult GetCategoryText(long categoryId)
        {
            var categorytext = BusinessObjectSeed.GetCatSubCategoryItemTextForId(categoryId);
            return Json(categorytext);
        }
    }
}