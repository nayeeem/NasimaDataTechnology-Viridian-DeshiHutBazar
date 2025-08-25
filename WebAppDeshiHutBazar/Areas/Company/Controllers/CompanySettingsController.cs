using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Common;
using Model;

namespace WebDeshiHutBazar
{
    public partial class CompanySettingsController : BaseController
    {
        private readonly IRepoDropDownDataList _RepoDropdown;
        private readonly ICompanyService _CompanyService;
        private readonly IUserAccountService _UserAccountService;
        private readonly IImageProcessingService _ImageProcessingService;
        private readonly IBillManagementService _BillManagementService;
        private readonly IPostMangementService _PostManagementService;

        public CompanySettingsController() 
        { }

        public CompanySettingsController(
            ICompanyService companyService, 
            IUserAccountService userAccountService,
            IRepoDropDownDataList repoDropdown, 
            IImageProcessingService imageProcessingService,
            IBillManagementService billManagementService,
            IPostMangementService postManagementService) 
        {
            _RepoDropdown = repoDropdown;
            _CompanyService = companyService;
            _UserAccountService = userAccountService;
            _ImageProcessingService = imageProcessingService;
            _BillManagementService = billManagementService;
            _PostManagementService = postManagementService;
        }
       
        [Authorize(Roles = "Company, SuperAdmin")]
        public async Task<ViewResult> ManageAccount()
        {            
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();
            var objCompanyAccountViewModel = new CompanyAccountViewModel(CURRENCY_CODE);
            objCompanyAccountViewModel = await _CompanyService.GetCompanyByUserID(userId);
            SetSessionFiles(objCompanyAccountViewModel.TradeLicenseFile, objCompanyAccountViewModel.OwnerNIDFile);
            objCompanyAccountViewModel.PageName = "Manage Company Account Page";
            return View(@"../../Areas/Company/Views/CompanySettings/ManageAccount", objCompanyAccountViewModel);
        }

        [Authorize(Roles = "Company, SuperAdmin")]
        public async Task<ActionResult> UpdateCompany(CompanyAccountViewModel objCompanyAccount)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();
            SetSessionFiles(objCompanyAccount);
            if (objCompanyAccount.CompanyID == 0)
            {
                await _CompanyService.SaveCompany(objCompanyAccount);
            }
            else
            {
                await _CompanyService.UpdateCompany(objCompanyAccount);
            }
            var result = await _CompanyService.UpdateAccountPassword(objCompanyAccount);
            return RedirectToAction("ManageAccount", "CompanySettings");
        }

        private void SetSessionFiles(byte[] tradeLicenseFile, byte[] ownerNIDFile)
        {
            if (tradeLicenseFile != null && tradeLicenseFile.Length > 0)
            {
                Session["TradeLicenseFile"] = tradeLicenseFile;
            }
            if (ownerNIDFile != null && ownerNIDFile.Length > 0)
            {
                Session["NIDFile"] = ownerNIDFile;
            }
        }

        private void SetSessionFiles(CompanyAccountViewModel objCompanyAccountVM)
        {
            objCompanyAccountVM.TradeLicenseFile = Session["TradeLicenseFile"] != null ? (byte[]) Session["TradeLicenseFile"] : null;
            objCompanyAccountVM.OwnerNIDFile = Session["NIDFile"] != null ? (byte[])Session["NIDFile"] : null;            
        }

        [HttpPost]
        public JsonResult ImageTradeLicenseUpload(HttpPostedFileBase file)
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
                            Session["TradeLicenseFile"] = imgByte;
                        }
                    }

                }
            }
            return Json(true);
        }

        [HttpPost]
        public JsonResult ImageOwnerNIDUpload(HttpPostedFileBase file)
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
                            Session["NIDFile"] = imgByte;
                        }
                    }

                }
            }
            return Json(true);
        }

        [HttpGet]
        public PartialViewResult ImageLoadTradeLicense()
        {
            var imgByte = Session["TradeLicenseFile"];
            return PartialView(@"../../Areas/Company/Views/CompanySettings/_Image", imgByte);
        }

        [HttpGet]
        public PartialViewResult RemoveTradeLicenseFile()
        {
            Session["TradeLicenseFile"] = null;
            return PartialView(@"../../Areas/Company/Views/CompanySettings/_Image", new byte[1]);
        }

        [HttpGet]
        public PartialViewResult ImageLoadOwnerNIDFile()
        {
            var imgByte = Session["NIDFile"];
            return PartialView(@"../../Areas/Company/Views/CompanySettings/_Image", imgByte);
        }

        [HttpGet]
        public PartialViewResult RemoveOwnerNIDFile()
        {
            Session["NIDFile"] = null;
            return PartialView(@"../../Areas/Company/Views/CompanySettings/_Image", new byte[1]);
        }
    }
}