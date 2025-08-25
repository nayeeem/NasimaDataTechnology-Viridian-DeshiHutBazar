using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Common;
using System.IO;
using System.Threading.Tasks;

namespace WebDeshiHutBazar
{
    public class ManageAdminPostController : PageingController
    {
        private readonly IImageProcessingService _ImageProcessingService;
        private readonly IBillManagementService _BillManagementService;
        private readonly IPostMangementService _PostManagementService;

        public ManageAdminPostController(IImageProcessingService imageProcessingService,
            IBillManagementService billManagementService,
            IPostMangementService postManagementService) 
        {           
            _ImageProcessingService = imageProcessingService;
            _BillManagementService = billManagementService;
            _PostManagementService = postManagementService;
        }

        public ManageAdminPostController() 
        { }

        private ManagePostViewModel LoadTabSpecificPosts(ManagePostViewModel objModel, int pageNumber, string action, string controller)
        {
            objModel.PageingModelAll = SetPageingModel(objModel.PageingModelAll, objModel.ListPostViewModel.Count,
                                                                      pageNumber, MARKET_PAGE_SIZE, action, controller, "", null);
            objModel.ListPostViewModel = GetPostListForPage(objModel.ListPostViewModel, pageNumber, MANAGE_POST_PAGE_SIZE);
            return objModel;
        }                        

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> OnlyAdminPosts(int pageNumber = 1)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var objManagePostVM = new ManagePostViewModel(CURRENCY_CODE)
            {
                ListPostViewModel = await _PostManagementService.GetAllAdminPosts(COUNTRY_CODE, CURRENCY_CODE)
            };
            objManagePostVM = LoadTabSpecificPosts(objManagePostVM,
                pageNumber,
                "OnlyAdminPosts",
                "ManageAdminPost");
            objManagePostVM.PageName = "Admin Posts Page";
            return View(@"../../Areas/Admin/Views/ManageAdminPost/ManagePublishedPosts", objManagePostVM);
        }        

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<PartialViewResult> EditPostByPostId(long? postId)
        {
            if (!postId.HasValue)
            {
                return PartialView(@"../../Areas/Admin/Views/ManageAdminPost/_ModifyPost", null);
            }
            var objPostViewModel = await _PostManagementService.GetPostByPostIDForEdit(postId.Value, CURRENCY_CODE);
            if (Session == null)
            {
                return PartialView(@"../../Areas/Admin/Views/ManageAdminPost/_ModifyPost", objPostViewModel);
            }
            ClearManagePostImageSession(postId.Value);
            SetImageSessions(objPostViewModel, postId);
            return PartialView(@"../../Areas/Admin/Views/ManageAdminPost/_ModifyPost", objPostViewModel);
        }

        [Authorize(Roles = "SuperAdmin")]
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

        [Authorize(Roles = "SuperAdmin")]
        private bool ShouldSetFirstImage(PostViewModel objPostViewModel)
        {
            return objPostViewModel.ListImages.Count > 0 && 
                objPostViewModel.ListImages != null && 
                objPostViewModel.ListImages[0] != null;
        }

        [Authorize(Roles = "SuperAdmin")]
        private bool ShouldSetSecondImage(PostViewModel objPostViewModel)
        {
            return objPostViewModel.ListImages.Count > 1 && 
                objPostViewModel.ListImages != null && 
                objPostViewModel.ListImages[1] != null;
        }

        [Authorize(Roles = "SuperAdmin")]
        private bool ShouldSetThirdImage(PostViewModel objPostViewModel)
        {
            return objPostViewModel.ListImages.Count > 2 && 
                objPostViewModel.ListImages != null && 
                objPostViewModel.ListImages[2] != null;
        }

        [Authorize(Roles = "SuperAdmin")]
        private bool ShouldSetFourthImage(PostViewModel objPostViewModel)
        {
            return objPostViewModel.ListImages.Count > 3 && 
                objPostViewModel.ListImages != null && 
                objPostViewModel.ListImages[3] != null;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<PartialViewResult> ViewPostByPostId(long? postId)
        {
            if (postId.HasValue)
            {
                var postVm = await _PostManagementService.GetPostByPostIDReadonly(postId.Value, CURRENCY_CODE);
                return PartialView(@"../../Areas/Admin/Views/ManageAdminPost/_ViewPost", postVm);
            }
            return PartialView(@"../../Areas/Admin/Views/ManageAdminPost/_ViewPost", null);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> UpdatePost(PostViewModel objPostVm)
        {
            SetImageInViewModel(objPostVm, objPostVm.PostID);
            var result = await _PostManagementService.UpdatePost(objPostVm, objPostVm.PostID, COUNTRY_CODE);
            var userId = objPostVm.UserID;
            return Json(userId);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> DeletePostByPostId(long postId)
        {
            if (postId == default(long))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var res = await _PostManagementService.DeletePost(postId, GetSessionUserId(), COUNTRY_CODE);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.InnerException.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public PartialViewResult ImageLoadModifyMode(long? postId, long? serialNo)
        {
            var objImage = GetManagePostImageSession(postId.Value, serialNo.Value);
            return PartialView(@"../../Areas/Admin/Views/ManageAdminPost/_Image", objImage ?? new FileViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult ImageUploadModify(HttpPostedFileBase file, long? postId, long? serialNo)
        {
            //var appPath = HttpContext.Request.ApplicationPath;
            //if (appPath != null && appPath == "/")
            //    _ = "";
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

        [Authorize(Roles = "SuperAdmin")]
        private void SetImageInViewModel(PostViewModel objPostVM, long postId)
        {
            var objFileList = new List<FileViewModel>();
            if ( GetManagePostImageSession(postId,1) != null)
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
        public JsonResult GetSubCategories(long categoryValueId)
        {
            var listSubCategories = DropDownDataList.GetSubCategoryList(categoryValueId);
            return Json(listSubCategories, JsonRequestBehavior.AllowGet);
        }
                
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult GetSubCategoryText(long subCategoryId)
        {
            var subcategorytext = BusinessObjectSeed.GetCatSubCategoryItemTextForId(subCategoryId);
            return Json(subcategorytext);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult GetCategoryText(long categoryId)
        {
            var categorytext = BusinessObjectSeed.GetCatSubCategoryItemTextForId(categoryId);
            return Json(categorytext);
        }
    }
}