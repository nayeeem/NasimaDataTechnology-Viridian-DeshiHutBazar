using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data;

using System.Threading.Tasks;


using Model;
using System.Web;
using System.IO;
using System;
using System.Web.Security;

namespace WebDeshiHutBazar
{
    public class FabiaProviderController : PageingController
    {
        List<PostViewModel> listPostVM;
        private readonly IGroupPanelConfigService _GroupPanelConfigService;
        private readonly ILoggingService _LoggingService;
        private readonly IPostMangementService _PostMangementService;
        private readonly IPostMappingService _PostMappingService;
        private readonly IPostVisitService _PostVisitService;
        private readonly ILogPostRepository _ILogPostRepository;
        public readonly IRepoDropDownDataList _DropdownRepo;
        private readonly IUserAccountService _UserAccountService;
        private readonly IFabiaProviderService _FabiaProviderService;

        public FabiaProviderController()
        { }

        public FabiaProviderController(ILoggingService loggingService,
            IGroupPanelConfigService groupPanelConfigService,
            IPostMangementService postMangementService,
            IPostMappingService postMappingService,
            IPostVisitService postVisitService,
            ILogPostRepository logPostRepository,
            IRepoDropDownDataList dropdownList,
            IUserAccountService userAccountService,
            IFabiaProviderService fabiaProviderService
            )
        {
            _LoggingService = loggingService;
            _GroupPanelConfigService = groupPanelConfigService;
            _PostMangementService = postMangementService;
            _PostMappingService = postMappingService;
            listPostVM = new List<PostViewModel>();
            _PostVisitService = postVisitService;
            _ILogPostRepository = logPostRepository;
            _DropdownRepo = dropdownList;
            _UserAccountService = userAccountService;
            _FabiaProviderService = fabiaProviderService;
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> Index()
        {
            var userId = GetSessionUserId();
            var listPostVM = await _FabiaProviderService.GetAllProvider(userId);
            return View(@"../../Areas/LetItGo/Views/FabiaProvider/Index", listPostVM.AsEnumerable());
        }

        public async Task<ViewResult> NewProvider()
        {            
            FabiaProviderViewModel objProvider = new FabiaProviderViewModel();            
            objProvider.AV_State = DropDownDataList.GetAllStateList();
            objProvider.StateID = Convert.ToInt32(DropDownDataList.GetAllStateList().FirstOrDefault(a => a.Text.ToLower() == "dhaka").Value);
            objProvider.AV_FabiaServiceCategory = await _DropdownRepo.GetFabiaServiceCategoryList();
            objProvider.Currency = CURRENCY_CODE;
            objProvider.PageName = "New Provider Page";
            return View(@"../../Areas/LetItGo/Views/FabiaProvider/NewProvider", objProvider);
        }

        public async Task<JsonResult> SaveProvider(FabiaProviderViewModel objProvider)
        {
            var isValid = true;
            if (!string.IsNullOrEmpty(objProvider.Email))
            {
                isValid = ValidationService.IsValidEmail(objProvider.Email);
                if (!isValid)
                    return Json("EmailInvalid", JsonRequestBehavior.AllowGet);
            }
            var res = await _UserAccountService.IsAccountRegistered(objProvider.Email);
            if (!res)
            {               
                objProvider.ProfileImage = GetProviderProfileImageSession();
                var userID = await _FabiaProviderService.SaveProvider(objProvider);
                ClearProviderProfileImageSession();

                var userEntity = await _UserAccountService.GetAuthorizedUser(userID);
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
                return Json(Url.Action("Index", "FabiaProvider"));
            }
            else if (await _UserAccountService.ValidateUserCredential(objProvider.Email, objProvider.Password) ==
                EnumAccountCredential.Valid)
            {
                objProvider.ProfileImage = GetProviderProfileImageSession();
                objProvider.IsExistingUser = true;
                var userEntity  = await _UserAccountService.GetAuthorizedUser(objProvider.Email);
                objProvider.UserID = userEntity.UserID;
                var userID = await _FabiaProviderService.SaveProvider(objProvider);
                ClearProviderProfileImageSession();

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
                return Json(Url.Action("Index", "FabiaProvider"));
            }
            else
            {
                return Json("AuthenticationFailed", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ActionResult> ManageSingleProvider(long providerId)
        {            
            var objProvider = await _FabiaProviderService.GetProviderByID(providerId);
            objProvider.AV_State = DropDownDataList.GetAllStateList();
            objProvider.AV_FabiaServiceCategory = await _DropdownRepo.GetFabiaServiceCategoryList();
            objProvider.DisplayServiceCategory = objProvider.AV_FabiaServiceCategory.Any(a => a.Value == objProvider.FabiaServiceID.ToString())
                                                    ? objProvider.AV_FabiaServiceCategory.FirstOrDefault(a => a.Value == objProvider.FabiaServiceID.ToString()).Text
                                                    : "";
            objProvider.Currency = CURRENCY_CODE;
            objProvider.PageName = "New Provider Page";
            SetProviderProfileImageSession(objProvider.ProfileImage);
            return View(@"../../Areas/LetItGo/Views/FabiaProvider/ManageSingleProvider", objProvider);
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<JsonResult> UpdateProvider(FabiaProviderViewModel objProvider)
        {
            var isValid = true;
            if (!string.IsNullOrEmpty(objProvider.Email))
            {
                isValid = ValidationService.IsValidEmail(objProvider.Email);
                if (!isValid)
                    return Json("EmailInvalid", JsonRequestBehavior.AllowGet);
            }

            objProvider.ProfileImage = GetProviderProfileImageSession();           
            var postID = await _FabiaProviderService.UpdateProvider(objProvider);
            ClearProviderProfileImageSession();
            return Json(Url.Action("Index", "FabiaProvider"));
        }

        [HttpPost]
        public JsonResult ImageUploadProfile(HttpPostedFileBase file)
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
                            SetProviderProfileImageSession(imgByte);
                        }
                    }
                }
            }
            return Json(true);
        }
      
        [HttpGet]
        public PartialViewResult ImageLoadProfile()
        {
            var objImage = GetProviderProfileImageSession();
            return PartialView(@"../../Areas/LetItGo/Views/FabiaProvider/_Image", objImage);
        }
        
        public async Task<ActionResult> DeleteSingleProvider(long providerId)
        {
            var result = await _FabiaProviderService.DeleteProvider(providerId);            
            return RedirectToAction("Index");
        }
    }
}