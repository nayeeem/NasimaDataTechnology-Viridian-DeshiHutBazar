using System.Web.Mvc;
using Common;
using System;
using System.Threading.Tasks;
using Model;
using System.Web.Security;
using System.Web;
using System.Configuration;
using System.Collections.Generic;

namespace WebDeshiHutBazar
{
    public class AccountController : BaseController
    {
        private readonly HashingCryptographyService _HashingService;
        private readonly IUserAccountService _UserAccountService;
        private readonly IUserService _UserService;
        private readonly IPackageConfigurationService _PackageConfigurationService;
        private readonly IEmailNotificationService _EmailNotificationService;

        public AccountController(
            IUserAccountService userAccountService,
            IUserService userService,
            IPackageConfigurationService packageConfigurationService,
            IEmailNotificationService emailNotificationService)
        {
            _HashingService = new HashingCryptographyService();
            _UserAccountService = userAccountService;
            _UserService = userService;
            _PackageConfigurationService = packageConfigurationService;
            _EmailNotificationService = emailNotificationService;
        }

        public AccountController()
        { }

        public ActionResult Signup()
        {
            var objModel = new SignUpViewModel();
            objModel.PageName = "Sign Up Page";
            objModel.IsPrivateSeller = true;
            objModel.IsCompanySeller = false;
            return View(objModel);
        }

        [HttpPost]
        public async Task<ActionResult> Signup(SignUpViewModel ojAccModel)
        {
            if(!ModelState.IsValid)
            {
                return View(ojAccModel);
            }
            var userEntity = await _UserAccountService.GetAuthorizedUser(ojAccModel.Email);
            if (userEntity != null)
            {
                return View(ojAccModel);
            }

            User userNewEntityObject = CreateNewUser(EnumCountry.Bangladesh, ojAccModel);
            var res = await _UserService.AddUser(userNewEntityObject);

            var user = await _UserAccountService.GetAuthorizedUser(ojAccModel.Email);
            long userId = -1;
            if (user != null)
            {
                userId = user.UserID;
            }
            return RedirectToAction("AuthorizeUser", "Account", new { userId = (long?)userId });
        }

        private User CreateNewUser(EnumCountry country, SignUpViewModel objPostVM)
        {
            var passwordVM = _HashingService.GetMessageDigest(objPostVM.Password);
            EnumUserAccountType accountType = objPostVM.IsCompanySeller == true ? EnumUserAccountType.IndividualAdvertiser : EnumUserAccountType.IndividualAdvertiser;
            var objUser = new User(objPostVM.Email, passwordVM.Digest, objPostVM.ClientName, accountType, passwordVM.Salt, country)
            {
                Phone = objPostVM.Phone
            };
            return objUser;
        }

        public ActionResult Login(bool? isSignin)
        {
            ViewBag.IsSignin = isSignin.HasValue && isSignin.Value;
            var objModel = new AccountViewModel();
            objModel.PageName = "Sign In Page";
            return View(objModel);            
        }

        [HttpPost]
        public async Task<ActionResult> Login(AccountViewModel ojAccModel)
        {
            long userId = -1;
            var isValidUser = await _UserAccountService.ValidateUserCredential(ojAccModel.Email, ojAccModel.Password);
            if (isValidUser == EnumAccountCredential.Invalid)
            {
                return View(ojAccModel);
            }
            var user = await _UserAccountService.GetAuthorizedUser(ojAccModel.Email);
            if (user != null)
            {
                userId = user.UserID;
            }
            return RedirectToAction("AuthorizeUser", "Account", new { userId = (long?)userId });
        }

        public async Task<ActionResult> AuthorizeUser(long? userId)
        {
            if (!userId.HasValue)
                return Json(-1, JsonRequestBehavior.AllowGet);
            var user = await _UserAccountService.GetAuthorizedUser(userId.Value);
            if (user == null)
                return Json(-1, JsonRequestBehavior.AllowGet);

            //For both admin and advertiser
            //Later if admin fail with pin, just clear this session user.
            SetTempSessionUserId(user.UserID);

            if (user.UserAccountType == EnumUserAccountType.SuperAdmin)
            {                
                return RedirectToAction("AdminLoginGateway", "Account");
            }

            //Advertiser users
            FormsAuthentication.SetAuthCookie(user.Email, false);
            var authTicket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(60), false, user.Roles);
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(authCookie);
            var clientNameCookie = new HttpCookie("ClientName", user.ClientName);
            HttpContext.Response.Cookies.Add(clientNameCookie);
            var userIDCookie = new HttpCookie("LoginUserID", user.UserID.ToString());
            HttpContext.Response.Cookies.Add(userIDCookie);

            SetSessionUser(new UserModel()
            {
                IsAdminUser = user.UserAccountType == EnumUserAccountType.SuperAdmin ? true : false,
                ClientName = user.ClientName,
                UserID = user.UserID,
                Email = user.Email,
                IsVerifiedUser = user.IsVerifiedAccount
            });
            return RedirectToAction("Index", "Home", false);
        }

        public async Task<ActionResult> AdminLoginGateway(AccountViewModel objModel)
        {
            if (string.IsNullOrEmpty(objModel.PinNumber))
            {
                AccountViewModel ojAccModel = new AccountViewModel(CURRENCY_CODE);
                var pin = "111155";
                var result = await _UserAccountService.UpdateAdminUserLoginGatewayPin(GetSessionUserId(), pin);
                ojAccModel.PageName = "Admin Gateway Page";
                return View(ojAccModel);
            }
            else
            {                
                //Super admins code
                var isValidUser = await _UserAccountService.ValidateAdminLoginGatewayCredential(objModel.PinNumber, objModel.PassCode);
                if (isValidUser == EnumAccountCredential.Invalid)
                {                    
                    objModel.PageName = "Admin Gateway Page";                    
                    return View(objModel);
                }

                var user = await _UserAccountService.GetAuthorizedUser(GetTempSessionUserId());
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    var authTicket = new FormsAuthenticationTicket(
                        1, 
                        user.Email, 
                        DateTime.Now, 
                        DateTime.Now.AddMinutes(20), 
                        false, 
                        user.Roles);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);
                    var clientNameCookie = new HttpCookie("ClientName", user.ClientName);
                    HttpContext.Response.Cookies.Add(clientNameCookie);
                    var userIDCookie = new HttpCookie("LoginUserID", user.UserID.ToString());
                    HttpContext.Response.Cookies.Add(userIDCookie);

                    SetSessionUser(new UserModel()
                    {
                        IsAdminUser = user.UserAccountType == EnumUserAccountType.SuperAdmin ? true : false,
                        ClientName = user.ClientName,
                        UserID = user.UserID,
                        Email = user.Email,
                        IsVerifiedUser = user.IsVerifiedAccount
                    });
                    return RedirectToAction("Index", "Home", false);
                }
                return View(objModel);
            }
        }

        public void LogOutMethod()
        {
            FormsAuthentication.SignOut();
            ClearAdminSessionUser();
            ClearSessionUser();
            ClearTempSessionUserId();

            HttpCookie currentUserCookie = HttpContext.Request.Cookies["ClientName"];
            HttpContext.Response.Cookies.Remove("ClientName");
            if (currentUserCookie != null)
            {
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                HttpContext.Response.SetCookie(currentUserCookie);
            }
            
            HttpCookie loginUserIDCookie = HttpContext.Request.Cookies["LoginUserID"];
            HttpContext.Response.Cookies.Remove("LoginUserID");
            if (loginUserIDCookie != null)
            {                
                loginUserIDCookie.Expires = DateTime.Now.AddDays(-10);
                loginUserIDCookie.Value = null;
                HttpContext.Response.SetCookie(loginUserIDCookie);
            }
        }

        public ActionResult LogOff()
        {
            LogOutMethod();
            return RedirectToAction("Index", "Home", new { isLogout = true });
        }

        [HttpPost]
        public async Task<JsonResult> AuthenticateUser(string email, string password)
        {
            long userId = -1;
            var isValidUser = await _UserAccountService.ValidateUserCredential(email, password);
            if (isValidUser == EnumAccountCredential.Invalid)
                return Json(userId, JsonRequestBehavior.AllowGet);
            var user = await _UserAccountService.GetAuthorizedUser(email);
            if (user != null)
                userId = user.UserID;
            return Json(userId, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AuthenticateCheckoutUser(string email, string password)
        {
            long userId = -1;
            var isValidUser = await _UserAccountService.ValidateUserCredential(email, password);
            if (isValidUser == EnumAccountCredential.Invalid)
                return Json(userId, JsonRequestBehavior.AllowGet);
            var user = await _UserAccountService.GetAuthorizedUser(email);
            if (user != null)
                userId = user.UserID;
            return RedirectToAction("ProceedCheckout", "ShoppingCart");
        }

        [HttpPost]
        public async Task<JsonResult> CheckUserAccountExistByEmail(string email)
        {
            var isValid = ValidationService.IsValidEmail(email);
            if (!isValid)
                return Json("EmailInvalid");
            var doesExists = await _UserAccountService.IsUserEmailAlreadyCreated(email);
            if (!doesExists)
                return Json("EmailNotFound");
            return Json("EmailFound");
        }

        [HttpPost]
        public async Task<JsonResult> SetSessionUser(long? UserId)
        {
            var user = this.GetSessionUser();
            if(user == null && UserId.HasValue)
            {
                var userEntity = await _UserAccountService.GetAuthorizedUser(UserId.Value);
                UserModel objUser = new UserModel
                {
                    UserID = userEntity.UserID,
                    ClientName = userEntity.ClientName,
                    IsAdminUser = userEntity.UserAccountType == EnumUserAccountType.SuperAdmin ? true : false,
                    IsVerifiedUser = userEntity.IsVerifiedAccount,
                    Email = userEntity.Email,
                    Name = userEntity.ClientName
                };
                SetSessionUser(objUser);
            }
            else if (!UserId.HasValue)
            {
                ClearSessionUser();
            }
            return null;
        }

        public ViewResult ResetPassword(bool? isSignin)
        {
            ViewBag.IsSignin = isSignin.HasValue && isSignin.Value;
            var objModel = new AccountViewModel() { PageName = "Reset Password Page" };
            return View(objModel);
        }

        [HttpPost]
        public async Task<ActionResult> EmailResetPassword(string email)
        {
            var isValid = ValidationService.IsValidEmail(email);
            if (!isValid  || string.IsNullOrEmpty(email))
                return RedirectToAction("ResetPassword");
            var newPassword = await _UserAccountService.UpdateResetPassword(email);
            try
            {
                var objEmailViewModel = _EmailService.GetResetPassViewModel(newPassword, email);
                objEmailViewModel.MessageBodyHTMLText = RenderPartialToString(this, "_ResetPassword", objEmailViewModel, ViewData, TempData);
                _EmailService.SendPasswordResetEmail(objEmailViewModel);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
            return View();
        }

        public RedirectResult LoginRedirect(string returnUrl)
        {
            var serverUrl = ConfigurationManager.AppSettings["ServerUrl"];
            return Redirect(serverUrl + "//Account/Login");
        }

        public async Task<ActionResult> VerifyAccount(string code)
        {
            try
            {
                var verifyThis = await _UserAccountService.GetVerifyUser(code);
                if(!verifyThis)
                {
                    ViewBag.AlreadyDone = true;
                }
                else
                {
                    ViewBag.AlreadyDone = false;
                }
                return View();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                ViewBag.AlreadyDone = true;
                return View();
            }
        }

        [HttpPost]
        public async Task<JsonResult> EmailVerify()
        {
            try
            {               
                var codeValue = "";
                var userModel = GetSessionUser();
                if (!userModel.IsVerifiedUser && userModel != null)
                {
                    codeValue = await _UserAccountService.UpdateVerifyCode(userModel.UserID);                    
                    var objEmailViewModel = _EmailService.GetVerifyEmailViewModel(codeValue);
                    objEmailViewModel.MessageBodyHTMLText = RenderPartialToString(this, "_VerifyEmail", objEmailViewModel, ViewData, TempData);
                    objEmailViewModel.ReceiverEmail = userModel.Email;
                    _EmailService.SendAccountVerifyEmail(objEmailViewModel);
                    return Json(false);
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return Json(false);
            }
        }
    }
}
