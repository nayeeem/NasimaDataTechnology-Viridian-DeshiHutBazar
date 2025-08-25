using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Common;

namespace WebDeshiHutBazar
{
    public partial class NewProductController : BaseController
    {
        private readonly IPackageConfigurationService _PackageConfigurationService;
        private readonly IUserService _UserService;
        private readonly IImageProcessingService _ImageProcessingService;
        private readonly IUserAccountService _UserAccountService;
        private readonly IPostMangementService _PostManagementService;
        private readonly IBillManagementService _BillManagementService;
        private readonly IPaymentOptionService _PaymentOptionService;
        private readonly IGroupPanelConfigService _GroupPanelConfigService;

        public NewProductController(
                              IPackageConfigurationService packageConfigurationService,
                              IUserService userService,
                              IImageProcessingService imageProcessingService,
                              IUserAccountService userAccountService,
                              IPostMangementService postManagementService,
                              IBillManagementService billManagementService,
                              IPaymentOptionService paymentOptionService,
                              IGroupPanelConfigService groupPanelConfigService,
                              IEmailNotificationService emailService
                              )
        {
            _PackageConfigurationService = packageConfigurationService;
            _UserService = userService;
            _ImageProcessingService = imageProcessingService;
            _UserAccountService = userAccountService;
            _PostManagementService = postManagementService;
            _BillManagementService = billManagementService;
            _PaymentOptionService = paymentOptionService;
            _GroupPanelConfigService = groupPanelConfigService;
            _EmailService = emailService;
        }

        public NewProductController() { }

        private void SetImageInViewModel(PostViewModel objPostVm)
        {
            List<FileViewModel> objFileList = new List<FileViewModel>();
            objFileList = GetSessionNewPostImage();
            objPostVm.ListImages = objFileList;
            ClearNewPostImageSessions();
        }

        [Authorize(Roles = "Company")]
        public ViewResult NewPost()
        {
            ClearNewPostImageSessions();
            var objPostViewModel = _PostManagementService.CreateNewPost(CURRENCY_CODE);
            objPostViewModel.Currency = CURRENCY_CODE;
            objPostViewModel.PageName = "New Post Page";
            objPostViewModel.UserID = GetSessionUserId();
            return View(@"../../Areas/Company/Views/NewProduct/NewProduct", objPostViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        public async Task<ActionResult> SavePost(PostViewModel objPostVM)
        {
            objPostVM.UserID = GetSessionUserId();
            SetImageInViewModel(objPostVM);
            var postID = await _PostManagementService.SaveProductPost(objPostVM, EnumCountry.Bangladesh);
            var url = Url.Action("OnlyPublishedPosts", "ManageProducts");
            SimpleUrlViewModel objSimpleModel = new SimpleUrlViewModel(url, objPostVM.UserID);
            return RedirectToAction("Manage", "ManageProducts");
        }

        #region Image Upload Methods
        [HttpPost]
        public JsonResult UploadCarousel(HttpPostedFileBase file)
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
                            FileViewModel objFile = new FileViewModel
                            {
                                Image = imgByte,
                                FileName = file.FileName,
                                FileType = extension,
                                EnumPhoto = EnumPhoto.Carousel
                            };
                            SetSessionNewPostImage(objFile);
                        }
                    }
                }
            }
            return Json(true);
        }

        [HttpPost]
        public JsonResult UploadThumbnail(HttpPostedFileBase file)
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
                            FileViewModel objFile = new FileViewModel
                            {
                                Image = imgByte,
                                FileName = file.FileName,
                                FileType = extension,
                                EnumPhoto = EnumPhoto.Thumbnail
                            };
                            SetSessionNewPostImage(objFile);
                        }
                    }
                }
            }
            return Json(true);
        }

        [HttpPost]
        public JsonResult UploadSquare(HttpPostedFileBase file)
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
                            FileViewModel objFile = new FileViewModel
                            {
                                Image = imgByte,
                                FileName = file.FileName,
                                FileType = extension,
                                EnumPhoto = EnumPhoto.Square
                            };
                            SetSessionNewPostImage(objFile);
                        }
                    }
                }
            }
            return Json(true);
        }

        [HttpPost]
        public JsonResult UploadRectangle(HttpPostedFileBase file)
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
                            FileViewModel objFile = new FileViewModel
                            {
                                Image = imgByte,
                                FileName = file.FileName,
                                FileType = extension,
                                EnumPhoto = EnumPhoto.Rectangle
                            };
                            SetSessionNewPostImage(objFile);
                        }
                    }
                }
            }
            return Json(true);
        }

        [HttpGet]
        public PartialViewResult LoadCarousel()
        {
            var objImageList = GetSessionNewPostImage();
            var objImage = objImageList.Last();
            return PartialView(@"../../Areas/Company/Views/NewProduct/_Image", objImage);
        }

        [HttpGet]
        public PartialViewResult LoadThumbnail()
        {
            var objImageList = GetSessionNewPostImage();
            var objImage = objImageList.Last();
            return PartialView(@"../../Areas/Company/Views/NewProduct/_Image", objImage);
        }

        [HttpGet]
        public PartialViewResult LoadSquare()
        {
            var objImageList = GetSessionNewPostImage();
            var objImage = objImageList.Last();
            return PartialView(@"../../Areas/Company/Views/NewProduct/_Image", objImage);
        }

        [HttpGet]
        public PartialViewResult LoadRectangle()
        {
            var objImageList = GetSessionNewPostImage();
            var objImage = objImageList.Last();
            return PartialView(@"../../Areas/Company/Views/NewProduct/_Image", objImage);
        }

        [HttpGet]
        public JsonResult ImageRemove(long id)
        {
            RemoveSessionNewPostImage(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Ajax Calls
        [HttpPost]
        public JsonResult GetSubCategories(long categoryValueId)
        {
            var listSubCategories = DropDownDataList.GetSubCategoryList(categoryValueId);
            return Json(listSubCategories, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}