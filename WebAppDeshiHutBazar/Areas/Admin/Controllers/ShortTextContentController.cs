using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Common;

namespace WebDeshiHutBazar
{
    public partial class ShortTextContentController : BaseController
    {
        private readonly IPackageConfigurationService _PackageConfigurationService;
        private readonly IUserService _UserService;
        private readonly IImageProcessingService _ImageProcessingService;
        private readonly IUserAccountService _UserAccountService;
        private readonly IPostMangementService _PostManagementService;
        private readonly IBillManagementService _BillManagementService;
        private readonly IPaymentOptionService _PaymentOptionService;
        public ShortTextContentController(
                              IPackageConfigurationService packageConfigurationService,
                              IUserService userService,
                              IImageProcessingService imageProcessingService,
                              IUserAccountService userAccountService,
                              IPostMangementService postManagementService,
                              IBillManagementService billManagementService,
                              IPaymentOptionService paymentOptionService
                              ) 
        {
            _PackageConfigurationService = packageConfigurationService;
            _UserService = userService;
            _ImageProcessingService = imageProcessingService;
            _UserAccountService = userAccountService;
            _PostManagementService = postManagementService;
            _BillManagementService = billManagementService;
            _PaymentOptionService = paymentOptionService;
        }

        public ShortTextContentController() { }           
       
        [Authorize(Roles = "SuperAdmin")]
        public ViewResult NewTextContent()
        {
            ClearNewPostImageSessions();
            var objPostViewModel = new PostViewModel(CURRENCY_CODE);
            objPostViewModel.AV_PostType = DropDownDataList.GetPostTypeList();
            objPostViewModel.PostTypeID = null;
            objPostViewModel.PageName = "New Text Content Page";
            return View(@"../../Areas/Admin/Views/ShortTextContent/NewTextContent", objPostViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> SaveTextContent(PostViewModel objPostVM)
        {
            if (string.IsNullOrEmpty(objPostVM.PosterName))
            {
                return Json("PosterNameInvalid");
            }
            if(!objPostVM.PostTypeID.HasValue)
            {
                objPostVM.PostTypeID = (long)EnumPostType.ShortNote;
            }
            SetImageInViewModel(objPostVM);            
            var result = await _PostManagementService.SaveShortContentPost(objPostVM, COUNTRY_CODE, (EnumPostType) objPostVM.PostTypeID.Value, GetAdminSessionUser());
            ClearNewPostImageSessions();
            return Json(true);
        }

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
            return PartialView(@"../../Areas/LetItGo/Views/Post/_Image", objImage);
        }

        private void SetImageInViewModel(PostViewModel objPostVm)
        {
            List<FileViewModel> objFileList = new List<FileViewModel>();
            objFileList = GetSessionNewPostImage();
            objPostVm.ListImages = objFileList;
            ClearNewPostImageSessions();
        }

        [HttpGet]
        public JsonResult ImageRemove(long id)
        {
            RemoveSessionNewPostImage(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}