using System.Web.Mvc;
using Common;
using Model;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebDeshiHutBazar
{
    public class ManageConfigurationController : BaseController
    {
        public readonly WebDeshiHutBazarEntityContext _Context;
        private readonly RepoDropDownDataList _DropDownDataList;
        private readonly IPriceConfigurationService _PriceConfigurationService;

        public ManageConfigurationController()
        { }

        public ManageConfigurationController(IPriceConfigurationService priceConfigurationService,
            RepoDropDownDataList dropDownDataList)
        {
            _Context = new WebDeshiHutBazarEntityContext();
            _DropDownDataList = dropDownDataList;
            _PriceConfigurationService = priceConfigurationService;
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Index(PostPriceConfigInformationViewModel objInformation)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();
            PostPriceConfigInformationViewModel objInformationModel = 
                new PostPriceConfigInformationViewModel();
            objInformationModel = await _PriceConfigurationService.GetInformationViewModel(objInformation);
            objInformationModel.PageName = "Manage Price Config Page";
            return View(@"../../Areas/Admin/Views/ManageConfiguration/Index", objInformationModel);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Details(int id)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var objConfig = await _PriceConfigurationService.GetSinglePriceConfig(id);
            var objSingleViewModel = await _PriceConfigurationService.GetNewPostPriceConfigViewModel(objConfig);
            return View(@"../../Areas/Admin/Views/ManageConfiguration/Details", objSingleViewModel);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Create()
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var objSingleViewModel = await _PriceConfigurationService.GetNewCreatePostConfigurationViewModel();
            return View(@"../../Areas/Admin/Views/ManageConfiguration/Create", objSingleViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Create(PostPriceConfigurationViewModel collection)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            if (!ModelState.IsValid)
                return RedirectToAction("Create", "ManageConfiguration");
            if (!IsCurrentSessionAnAdminUser())
                return View("UnwantedAccessError");
            try
            {
                if (collection.PackageID.HasValue && collection.CountryId.HasValue && collection.Currency.HasValue && collection.SubCategoryID.HasValue)
                {
                    var result = await _PriceConfigurationService.AddPriceConfig(collection, GetSessionUserId(), COUNTRY_CODE);                       
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Edit(int id)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var objSingleViewModel = await _PriceConfigurationService.GetSinglePriceConfigViewModel(id);
            return View(@"../../Areas/Admin/Views/ManageConfiguration/Edit", objSingleViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Edit(int id, PostPriceConfigurationViewModel collection)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            if (!ModelState.IsValid)
                return RedirectToAction("Edit", "ManageConfiguration");
            if (!IsCurrentSessionAnAdminUser())
                return View("UnwantedAccessError");
            try
            {
                _PriceConfigurationService.UpdatePriceConfig(id, collection, GetSessionUserId(),COUNTRY_CODE);                                    
                return RedirectToAction("Index", "ManageConfiguration");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var objSingleViewModel = await _PriceConfigurationService.GetSinglePriceConfigViewModel(id);
            objSingleViewModel.PageName = "Delete Price Config Page";
            return View(@"../../Areas/Admin/Views/ManageConfiguration/Delete", objSingleViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Delete(int id, PostPackageConfigurationViewModel collection)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            try
            {
                _PriceConfigurationService.DeletePriceConfig(id, GetSessionUserId(), COUNTRY_CODE);
                return RedirectToAction("Index", "ManageConfiguration");
            }
            catch
            {
                return View();
            }
        }
    }
}
