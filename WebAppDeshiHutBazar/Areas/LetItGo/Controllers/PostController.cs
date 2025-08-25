using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Common;

namespace WebDeshiHutBazar
{
    public partial class PostController : BaseController
    {
        private readonly IPackageConfigurationService _PackageConfigurationService;
        private readonly IUserService _UserService;
        private readonly IImageProcessingService _ImageProcessingService;
        private readonly IUserAccountService _UserAccountService;
        private readonly IPostMangementService _PostManagementService;
        private readonly IBillManagementService _BillManagementService;
        private readonly IPaymentOptionService _PaymentOptionService;
        private readonly IGroupPanelConfigService _GroupPanelConfigService;

        public PostController(
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

        public PostController() { }

        public async Task<ViewResult> NewPost()
        {
            ClearNewPostImageSessions();
            var objPostViewModel = _PostManagementService.CreateNewPost(CURRENCY_CODE);
            objPostViewModel.Currency = CURRENCY_CODE;
            objPostViewModel.PageName = "New Post Page";
            var viewPostDetUrl = Url.Action("ViewItemDetail", "AllItemMarket", new { postid = "POST_ITEM_ID" });
            var viewMoreUrl = Url.Action("Market", "CategoryMarket", new { subcategoryid = "SUB_CAT_ID", pageNumber = "1" });
            objPostViewModel.ContentInfoViewModel.ListGroupPanelConfiguration =
                                     await _GroupPanelConfigService.GetAllPageGroupPanelConfigurations(
                                         EnumPublicPage.PostNewAd,
                                         viewMoreUrl,
                                         viewPostDetUrl,
                                         COUNTRY_CODE,
                                         CURRENT_TIME_SLOT,
                                         null, 0, null, 0, null, CURRENCY_CODE);

            return View(@"../../Areas/LetItGo/Views/Post/NewPost", objPostViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> SavePost(PostViewModel objPostVM)
        {
            var isValid = true;
            if (!string.IsNullOrEmpty(objPostVM.Email))
            {
                isValid = ValidationService.IsValidEmail(objPostVM.Email);
                if (!isValid)
                    return Json("EmailInvalid", JsonRequestBehavior.AllowGet);
            }

            if (!await _UserAccountService.IsAccountRegistered(objPostVM.Email))
            {
                SetImageInViewModel(objPostVM);
                var packageEntityObject = await _PackageConfigurationService.GetDefaultStartupPackage();
                var postID = await _PostManagementService.SavePost(
                    objPostVM, EnumCountry.Bangladesh, packageEntityObject, CURRENCY_CODE);
                var userEntity = await _UserAccountService.GetAuthorizedUser(objPostVM.UserID);

                FormsAuthentication.SetAuthCookie(userEntity.Email, false);
                var authTicket = new FormsAuthenticationTicket(1, userEntity.Email, DateTime.Now, DateTime.Now.AddMinutes(60), false, userEntity.Roles);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);
                var clientNameCookie = new HttpCookie("ClientName", userEntity.ClientName);
                HttpContext.Response.Cookies.Add(clientNameCookie);
                var userIDCookie = new HttpCookie("LoginUserID", userEntity.UserID.ToString());
                HttpContext.Response.Cookies.Add(userIDCookie);

                SetSessionUser(new UserModel()
                {
                    IsAdminUser = userEntity.UserAccountType == EnumUserAccountType.SuperAdmin ? true : false,
                    ClientName = userEntity.ClientName,
                    UserID = userEntity.UserID,
                    Email = userEntity.Email,
                    IsVerifiedUser = userEntity.IsVerifiedAccount
                });

                var url = Url.Action("PaymentOptions", "Post", new { subCategoryId = objPostVM.SubCategoryID, postId = postID });
                SimpleUrlViewModel objSimpleModel = new SimpleUrlViewModel(url, objPostVM.UserID);
                return Json(objSimpleModel, JsonRequestBehavior.AllowGet);
            }
            else if (await _UserAccountService.ValidateUserCredential(objPostVM.Email, objPostVM.Password) == EnumAccountCredential.Valid)
            {
                SetImageInViewModel(objPostVM);
                var postID = await _PostManagementService.SavePostForExistingUser(objPostVM, COUNTRY_CODE);
                var userEntity = await _UserAccountService.GetAuthorizedUser(objPostVM.Email);

                FormsAuthentication.SetAuthCookie(userEntity.Email, false);
                var authTicket = new FormsAuthenticationTicket(1, userEntity.Email, DateTime.Now, DateTime.Now.AddMinutes(60), false, userEntity.Roles);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);
                var clientNameCookie = new HttpCookie("ClientName", userEntity.ClientName);
                HttpContext.Response.Cookies.Add(clientNameCookie);
                var userIDCookie = new HttpCookie("LoginUserID", userEntity.UserID.ToString());
                HttpContext.Response.Cookies.Add(userIDCookie);

                SetSessionUser(new UserModel()
                {
                    IsAdminUser = userEntity.UserAccountType == EnumUserAccountType.SuperAdmin ? true : false,
                    ClientName = userEntity.ClientName,
                    UserID = userEntity.UserID,
                    Email = userEntity.Email,
                    IsVerifiedUser = userEntity.IsVerifiedAccount
                });

                var url = Url.Action("PaymentOptions", "Post", new { subCategoryId = objPostVM.SubCategoryID, postId = postID });
                SimpleUrlViewModel objSimpleModel = new SimpleUrlViewModel(url, userEntity.UserID);
                return Json(objSimpleModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("AuthenticationFailed", JsonRequestBehavior.AllowGet);
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
            return PartialView(@"../../Areas/LetItGo/Views/Post/_Image", objImage);
        }

        [HttpGet]
        public JsonResult ImageRemove(long id)
        {
            RemoveSessionNewPostImage(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private void SetImageInViewModel(PostViewModel objPostVm)
        {
            List<FileViewModel> objFileList = new List<FileViewModel>();
            objFileList = GetSessionNewPostImage();
            objPostVm.ListImages = objFileList;
            ClearNewPostImageSessions();
        }
        #endregion

        #region Ajax Calls
        [HttpPost]
        public JsonResult GetSubCategories(long categoryValueId)
        {
            var listSubCategories = DropDownDataList.GetSubCategoryList(categoryValueId);
            return Json(listSubCategories, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSubCategoryText(long subCategoryId)
        {
            var subcategorytext = BusinessObjectSeed.GetCatSubCategoryItemTextForId(subCategoryId);
            return Json(subcategorytext);
        }

        [HttpPost]
        public JsonResult GetCategoryText(long categoryId)
        {
            var categorytext = BusinessObjectSeed.GetCatSubCategoryItemTextForId(categoryId);
            return Json(categorytext);
        }

        [HttpGet]
        public async Task<JsonResult> LikeThisPost(long postId, string actionType)
        {
            var res = await _PostManagementService.LikeThisPost(postId, actionType);
            var res1 = await SendPostLikeEmail(postId, actionType);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private async Task<bool> SendPostLikeEmail(long postID, string actionType)
        {
            if (actionType == "Plus")
            {
                var objPostVM = await _PostManagementService.GetPostByPostIDForEdit(postID, CURRENCY_CODE);
                EmailViewModel objEmailViewModel = _EmailService.GetLikeViewModel(objPostVM, postID);
                objEmailViewModel.MessageBodyHTMLText = RenderPartialToString(this,
                    "_AdLiked",
                    objEmailViewModel,
                    ViewData, TempData);
                objEmailViewModel.ReceiverEmail = objPostVM.Email;
                _EmailService.SendLikeEmail(objEmailViewModel);
            }
            return true;
        }
        #endregion
    }
}