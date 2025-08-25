using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common;
using Model;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System;

namespace WebDeshiHutBazar
{
    public class ManageFabiaServiceController : BaseController
    {
        private readonly IImageProcessingService _ImageProcessingService;
        private readonly IBillManagementService _BillManagementService;
        private readonly IPostMangementService _PostManagementService;

        public ManageFabiaServiceController(IImageProcessingService imageProcessingService,
            IBillManagementService billManagementService,
            IPostMangementService postManagementService)
        {
            _ImageProcessingService = imageProcessingService;
            _BillManagementService = billManagementService;
            _PostManagementService = postManagementService;
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Index()
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            ClearProcessImageSession();
            ClearServiceImageSession();
            if (!IsCurrentSessionAnAdminUser())
                return View("UnwantedAccessError");
            var url = Url.Action("ManageSingleService", "ManagePostService", new { postid = "POST_ITEM_ID" });
            var listPostVM = await _PostManagementService.GetAllPosts(
                EnumCountry.Bangladesh, 
                EnumPostType.FabiaService, 
                url,
                CURRENCY_CODE);
            FabiaProviderViewModel objModel = new FabiaProviderViewModel();
            objModel.ListFabiaServiceCategory = listPostVM.ToList();
            return View(@"../../Areas/Admin/Views/ManageFabiaService/Index", objModel);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> ManageSingleService(long postid)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            ClearProcessImageSession();
            ClearServiceImageSession();
            if (!IsCurrentSessionAnAdminUser())
                return View("UnwantedAccessError");
            var objPostVM = await _PostManagementService.GetPostByPostIDForEdit(postid, CURRENCY_CODE);
            objPostVM.PostProcessViewModel.AV_StepNo = DropDownDataList.GetStepsList();
            objPostVM.PostProcessViewModel.AV_PaidBy = DropDownDataList.GetPaidByList();
            objPostVM.PostServiceViewModel.AV_PaidBy = DropDownDataList.GetPaidByList();
            objPostVM.PostProcessViewModel.StepNo = EnumStepNumber.Step1;
            objPostVM.PostProcessViewModel.PaidBy = EnumPaidBy.DeshiHutBazar;
            objPostVM.PostServiceViewModel.PaidBy = EnumPaidBy.Customer;
            objPostVM.PostProcessViewModel.PostID = postid;
            objPostVM.PostServiceViewModel.PostID = postid;
            SetImageSessions(objPostVM, postid);
            return View(@"../../Areas/Admin/Views/ManageFabiaService/ManageSingleService", objPostVM);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> DeleteSingleService(int postid)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var objConfig = await _PostManagementService.DeletePost(postid, GetSessionUserId(), COUNTRY_CODE);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> UpdateProcess(PostProcessViewModel processViewModel)
        {
            var image = GetServiceImageSession();
            if (image != null)
                processViewModel.StepImage = image;
            else
                processViewModel.StepImage = null;
            var res = await _PostManagementService.UpdatePostProcess(processViewModel);
            ClearProcessImageSession();
            ClearServiceImageSession();
            return Json(true);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> SaveProcess(PostProcessViewModel processViewModel)
        {
            var image = GetProcessImageSession();
            if (image != null)
                processViewModel.StepImage = image;
            var res = await _PostManagementService.AddPostProcess(processViewModel);
            ClearProcessImageSession();
            ClearServiceImageSession();
            return Json(true);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> RemoveProcess(long postId, long processId)
        {
            var res = await _PostManagementService.RemovePostProcess(postId, processId);
            ClearProcessImageSession();
            ClearServiceImageSession();
            return Json(true);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> UpdateService(PostServiceViewModel serviceViewModel)
        {
            var image = GetServiceImageSession();
            if (image != null)
                serviceViewModel.ServiceImage = image;
            else
                serviceViewModel.ServiceImage = null;
            var res = await _PostManagementService.UpdatePostService(serviceViewModel);

            ClearProcessImageSession();
            ClearServiceImageSession();
            return Json(true);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> SaveService(PostServiceViewModel processViewModel)
        {
            var image = GetServiceImageSession();
            if (image != null)
                processViewModel.ServiceImage = image;
            var res = await _PostManagementService.AddPostService(processViewModel);

            ClearProcessImageSession();
            ClearServiceImageSession();
            return Json(true);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> RemoveService(long postId, long serviceId)
        {
            var res = await _PostManagementService.RemovePostService(postId, serviceId);
            ClearProcessImageSession();
            ClearServiceImageSession();
            return Json(true);
        }

        [Authorize(Roles = "SuperAdmin")]
        private void SetImageSessions(PostViewModel objPostViewModel, long? postId)
        {
            if (ShouldSetFirstImage(objPostViewModel))
            {
                objPostViewModel.ListImages[0].FileID = 0;
                objPostViewModel.ListImages[0].PostID = null;
                SetManagePostImageSession(postId.Value, 1, objPostViewModel.ListImages[0]);
            }
            if (ShouldSetSecondImage(objPostViewModel))
            {
                objPostViewModel.ListImages[1].FileID = 0;
                objPostViewModel.ListImages[1].PostID = null;
                SetManagePostImageSession(postId.Value, 2, objPostViewModel.ListImages[1]);
            }
            if (ShouldSetThirdImage(objPostViewModel))
            {
                objPostViewModel.ListImages[2].FileID = 0;
                objPostViewModel.ListImages[2].PostID = null;
                SetManagePostImageSession(postId.Value, 3, objPostViewModel.ListImages[2]);
            }
            if (ShouldSetFourthImage(objPostViewModel))
            {
                objPostViewModel.ListImages[3].FileID = 0;
                objPostViewModel.ListImages[3].PostID = null;
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
        public async Task<JsonResult> UpdatePost(PostViewModel objPostVm)
        {
            SetImageInViewModel(objPostVm, objPostVm.PostID);
            var result = await _PostManagementService.UpdatePost(objPostVm, objPostVm.PostID, COUNTRY_CODE);
            var userId = objPostVm.UserID;
            SetImageSessions(objPostVm, objPostVm.PostID);
            return Json(userId);
        }        

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public PartialViewResult ImageLoadModifyMode(long? postId, long? serialNo)
        {
            byte[] bte;
            var img = GetManagePostImageSession(postId.Value, serialNo.Value);
            if (img != null)
            {
                bte = img.Image;
            }
            else
            {
                bte = null;
            }
            return PartialView(@"../../Areas/Admin/Views/ManageFabiaService/_Image", bte);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult ImageUploadModify(HttpPostedFileBase file, long? postId, long? serialNo)
        {
            //var appPath = HttpContext.Request.ApplicationPath;
            //if (appPath != null && appPath == "/")
            //    appPath = "";
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
                            var objFile = new FileViewModel { Image = imgByte, PostID = postId };

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

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<PartialViewResult> EditPostByPostId(long? postId)
        {
            if (!postId.HasValue)
            {
                return PartialView(@"../../Areas/Admin/Views/ManageFabiaService/_ModifyFabiaPost", null);
            }
            var objPostViewModel = await _PostManagementService.GetPostByPostIDForEdit(postId.Value, CURRENCY_CODE);
            if (Session == null)
            {
                return PartialView(@"../../Areas/Admin/Views/ManageFabiaService/_ModifyFabiaPost", objPostViewModel);
            }
            ClearManagePostImageSession(postId.Value);
            SetImageSessions(objPostViewModel, postId);
            return PartialView(@"../../Areas/Admin/Views/ManageFabiaService/_ModifyFabiaPost", objPostViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public PartialViewResult ImageLoadProcess()
        {
            var objImage = GetProcessImageSession();
            return PartialView(@"../../Areas/Admin/Views/ManageFabiaService/_Image", objImage);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public PartialViewResult ImageLoadService()
        {
            var objImage = GetServiceImageSession();
            return PartialView(@"../../Areas/Admin/Views/ManageFabiaService/_Image", objImage);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult ImageUploadProcess(HttpPostedFileBase file)
        {
            var appPath = HttpContext.Request.ApplicationPath;
            if (appPath != null && appPath == "/")
                appPath = "";
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
                            SetProcessImageSession(imgByte);
                            //if (processId.HasValue)
                            //{
                            //    var res = await _PostManagementService.UpdatePostProcess(imgByte, processId.Value);
                            //}
                        }
                    }
                }
            }
            return null;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult ImageUploadService(HttpPostedFileBase file)
        {
            var appPath = HttpContext.Request.ApplicationPath;
            if (appPath != null && appPath == "/")
                appPath = "";
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
                            SetServiceImageSession(imgByte);
                            //if (serviceId.HasValue)
                            //{
                            //    var res = await _PostManagementService.UpdatePostService(imgByte, serviceId.Value);
                            //}
                        }
                    }
                }
            }
            return null;
        }
    }
}
