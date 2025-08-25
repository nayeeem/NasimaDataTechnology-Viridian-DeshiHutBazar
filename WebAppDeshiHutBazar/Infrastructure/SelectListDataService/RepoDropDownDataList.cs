using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;
using Data;
using Model;

namespace WebDeshiHutBazar
{
    public class RepoDropDownDataList :  IRepoDropDownDataList
    {
        private readonly IPackageConfigRepository _IPackageRepo;
        private readonly IUserService _UserService;
        private readonly IPostQueryRepository _PostQueryRepo;

        public RepoDropDownDataList(IPackageConfigRepository packageRepo,
            IUserService userService,
            IPostQueryRepository postRepo)
        {
            _IPackageRepo = packageRepo;
            _UserService = userService;
            _PostQueryRepo = postRepo;
        }

        public async Task<string> GetPackageNameText(int packageId)
        {
            var itemList = await GetAllPackages();
            var item =itemList.FirstOrDefault(a => a.ValueID == packageId);
            if (item != null)
                return item.Text;
            return "";
        }

        public async Task<IEnumerable<SelectListItem>> GetPackageList()
        {
            return GetAValueSelectList(await GetAllPackages()).AsEnumerable();
        }

        public async Task<List<AValueModel>> GetAllPackages()
        {
            List<AValueModel> objAValueList = new List<AValueModel>();
            objAValueList.Add(new AValueModel() { Text = "Select Package", ValueID = -1 });
            AValueModel objAValue;
            List<PackageConfig> listPackageConfiguration = await _IPackageRepo.GetAllActivePackages();
            foreach (var item in listPackageConfiguration)
            {
                objAValue = new AValueModel();
                objAValue.ValueID = item.PackageConfigID;
                objAValue.Text = item.PackageName;
                objAValueList.Add(objAValue);
            }
            return objAValueList;
        }

        public async Task<IEnumerable<SelectListItem>> GetUsersList()
        {
            return GetAValueSelectList(await GetAllUsers()).AsEnumerable();
        }

        public async Task<List<AValueModel>> GetAllUsers()
        {
            List<AValueModel> objAValueList = new List<AValueModel>();
            objAValueList.Add(new AValueModel() { Text = "Select User", ValueID = -1 });
            AValueModel objAValue;
            List<User> listPackageConfiguration = await _UserService.GetAllUser();
            foreach (var item in listPackageConfiguration)
            {
                objAValue = new AValueModel();
                objAValue.ValueID = item.UserID;
                objAValue.Text = item.ClientName + " (" + item.Email + ")";
                objAValueList.Add(objAValue);
            }
            return objAValueList;
        }

        public static IEnumerable<SelectListItem> GetAValueSelectList(List<AValueModel> listAValue)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            listAValue.ForEach(a =>
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = a.Text.Trim();
                objItem.Value = a.ValueID == -1 ? null : a.ValueID.ToString().Trim();
                objList.Add(objItem);
            });
            return objList.AsEnumerable();
        }

        public async Task<IEnumerable<SelectListItem>> GetFabiaServiceCategoryList()
        {
            var listFabiaPosts = await _PostQueryRepo.GetAllPosts(Common.EnumCountry.Bangladesh, Common.EnumPostType.FabiaService);
            List<SelectListItem> objList = new List<SelectListItem>();
            if (listFabiaPosts != null && listFabiaPosts.Count > 0)
            {
                foreach (var item in listFabiaPosts)
                {
                    SelectListItem objItem = new SelectListItem();
                    objItem.Value = item.PostID.ToString();
                    objItem.Text = item.Title;
                    objList.Add(objItem);
                }
            }
            return objList.AsEnumerable();
        }
    }
}