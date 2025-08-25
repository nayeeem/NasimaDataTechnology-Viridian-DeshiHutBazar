using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common;
using Model;

namespace WebDeshiHutBazar
{
    public class PackageConfigurationController : BaseController
    {
        public readonly WebDeshiHutBazarEntityContext _Context;

        public PackageConfigurationController()
        {
            _Context = new WebDeshiHutBazarEntityContext();
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Index()
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var listPackages = _Context.PostPackageConfigurations.ToList().OrderBy(a => a.PackageName).ToList();
            List<PostPackageConfigurationViewModel> objPackageList = new List<PostPackageConfigurationViewModel>();
            PostPackageConfigurationViewModel objSinglePackageViewModel;
            foreach (var item in listPackages)
            {
                objSinglePackageViewModel = new PostPackageConfigurationViewModel
                {
                    PackageConfigID = item.PackageConfigID,
                    PackageName = item.PackageName,
                    Description = item.Descriptinon,
                    PackagePrice = item.PackagePrice,
                    Discount = item.Discount,
                    TotalFreePost = item.TotalFreePost,
                    TotalPremiumPost = item.TotalPremiumPost,
                    PackageStatus = item.PackageStatus,
                    PackageType = item.PackageType,
                    SubscriptionPeriod = item.SubscriptionPeriod,
                    DisplayPackageStatus = LocationRelatedSeed.GetPackageStatusDescription((EnumPackageStatus)item.PackageStatus),
                    DisplayPackageType = LocationRelatedSeed.GetPackageTypeDescription((EnumPackageType)item.PackageType),
                    DisplaySubscriptionPeriod = LocationRelatedSeed.GetSubscriptionPeriodDescription((EnumPackageSubscriptionPeriod)item.SubscriptionPeriod)
                };
                objPackageList.Add(objSinglePackageViewModel);
            }
            PostPriceConfigInformationViewModel objModel = new PostPriceConfigInformationViewModel();
            objModel.ListConfig = objPackageList.ToList();
            return View(@"../../Areas/Admin/Views/PackageConfiguration/Index", objModel);
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Details(int id)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var objConfig = _Context.PostPackageConfigurations.FirstOrDefault(a => a.PackageConfigID == id);
            var objSinglePackageViewModel = new PostPackageConfigurationViewModel
            {
                PackageConfigID = objConfig.PackageConfigID,
                PackageName = objConfig.PackageName,
                Description = objConfig.Descriptinon,
                TotalFreePost = objConfig.TotalFreePost,
                TotalPremiumPost = objConfig.TotalPremiumPost,
                PackageStatus = objConfig.PackageStatus,
                PackageType = objConfig.PackageType,
                SubscriptionPeriod = objConfig.SubscriptionPeriod,
                DisplayPackageStatus = LocationRelatedSeed.GetPackageStatusDescription((EnumPackageStatus)objConfig.PackageStatus),
                DisplayPackageType = LocationRelatedSeed.GetPackageTypeDescription((EnumPackageType)objConfig.PackageType),
                DisplaySubscriptionPeriod = LocationRelatedSeed.GetSubscriptionPeriodDescription((EnumPackageSubscriptionPeriod)objConfig.SubscriptionPeriod),
                PackagePrice = objConfig.PackagePrice,
                Discount = objConfig.Discount,
                PageName = "Details Package Page"
            };
            return View(@"../../Areas/Admin/Views/PackageConfiguration/Details", objSinglePackageViewModel);
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Create()
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var objSinglePackageViewModel = new PostPackageConfigurationViewModel
            {
                AV_PackageStatus = DropDownDataList.GetPackageStatusList(),
                AV_PackageType = DropDownDataList.GetPackageTypeList(),
                AV_SubscriptionPeriod = DropDownDataList.GetSubscriptionPeriodList(),
                PageName = "Create Package Page"
            };
            return View(@"../../Areas/Admin/Views/PackageConfiguration/Create", objSinglePackageViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Create(PostPackageConfigurationViewModel collection)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            if (!ModelState.IsValid)
                return RedirectToAction("Create", "PackageConfiguration");
            if (!IsCurrentSessionAnAdminUser())
                return View("UnwantedAccessError");
            if (_Context.PostPackageConfigurations
                .Any(a => a.PackageType == EnumPackageType.StartUpPackage) 
                && collection.PackageType==EnumPackageType.StartUpPackage)
                return RedirectToAction("Index");
            try
            {
                PackageConfig objEntity = new PackageConfig(
                    collection.PackageStatus,
                    collection.PackageType,
                    collection.SubscriptionPeriod,
                    collection.PackageName,
                    collection.Description,
                    collection.TotalFreePost,
                    collection.TotalPremiumPost,
                    collection.PackagePrice,
                    collection.Discount,
                    COUNTRY_CODE
                    );
                objEntity.CreatedBy = GetSessionUserId();
                _Context.PostPackageConfigurations.Add(objEntity);
                _Context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Edit(int id)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var objConfig = _Context.PostPackageConfigurations.FirstOrDefault(a => a.PackageConfigID == id);
            var objSinglePackageViewModel = new PostPackageConfigurationViewModel
            {
                PackageConfigID = objConfig.PackageConfigID,
                PackageName = objConfig.PackageName,
                Description = objConfig.Descriptinon,
                TotalFreePost = objConfig.TotalFreePost,
                TotalPremiumPost = objConfig.TotalPremiumPost,
                PackageStatus = objConfig.PackageStatus,
                PackageType = objConfig.PackageType,
                DisplayPackageStatus = LocationRelatedSeed.GetPackageStatusDescription((EnumPackageStatus)objConfig.PackageStatus),
                DisplayPackageType = LocationRelatedSeed.GetPackageTypeDescription((EnumPackageType)objConfig.PackageType),
                DisplaySubscriptionPeriod = LocationRelatedSeed.GetSubscriptionPeriodDescription((EnumPackageSubscriptionPeriod)objConfig.SubscriptionPeriod),
                AV_PackageStatus = DropDownDataList.GetPackageStatusList(),
                AV_PackageType = DropDownDataList.GetPackageTypeList(),
                AV_SubscriptionPeriod = DropDownDataList.GetSubscriptionPeriodList(),
                PackagePrice = objConfig.PackagePrice,
                Discount = objConfig.Discount,
                PageName = "Edit Package Page"
            };
            return View(@"../../Areas/Admin/Views/PackageConfiguration/Edit", objSinglePackageViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Edit(int id, PostPackageConfigurationViewModel collection)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            if (!ModelState.IsValid)
                return RedirectToAction("Edit", "PackageConfiguration");            
            try
            {
                var objConfig = _Context.PostPackageConfigurations.FirstOrDefault(a => a.PackageConfigID == id);
                if (objConfig != null)
                {
                    objConfig.PackageName = collection.PackageName;
                    objConfig.Descriptinon = collection.Description;
                    objConfig.TotalFreePost = collection.TotalFreePost;
                    objConfig.TotalPremiumPost = collection.TotalPremiumPost;
                    objConfig.PackageStatus = collection.PackageStatus;
                    objConfig.PackageType = collection.PackageType;
                    objConfig.SubscriptionPeriod = collection.SubscriptionPeriod;
                    objConfig.PackagePrice = collection.PackagePrice;
                    objConfig.Discount = collection.Discount;
                    objConfig.UpdateModifiedDate(GetSessionUserId(), COUNTRY_CODE);
                    _Context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Delete(int id)
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            var objConfig = _Context.PostPackageConfigurations.FirstOrDefault(a => a.PackageConfigID == id);
            var objSinglePackageViewModel = new PostPackageConfigurationViewModel
            {
                PackageConfigID = objConfig.PackageConfigID,
                PackageName = objConfig.PackageName,
                Description = objConfig.Descriptinon,
                TotalFreePost = objConfig.TotalFreePost,
                TotalPremiumPost = objConfig.TotalPremiumPost,
                PackageStatus = objConfig.PackageStatus,
                PackageType = objConfig.PackageType,
                DisplayPackageStatus = LocationRelatedSeed.GetPackageStatusDescription((EnumPackageStatus)objConfig.PackageStatus),
                DisplayPackageType = LocationRelatedSeed.GetPackageTypeDescription((EnumPackageType)objConfig.PackageType),
                DisplaySubscriptionPeriod = LocationRelatedSeed.GetSubscriptionPeriodDescription((EnumPackageSubscriptionPeriod)objConfig.SubscriptionPeriod),
                PackagePrice = objConfig.PackagePrice,
                Discount = objConfig.Discount,
                PageName = "Delete Package Page"
            };
            return View(@"../../Areas/Admin/Views/PackageConfiguration/Delete",objSinglePackageViewModel);
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
                var objItem = _Context.PostPackageConfigurations.FirstOrDefault(a => a.PackageConfigID == id);
                _Context.PostPackageConfigurations.Remove(objItem);
                _Context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
