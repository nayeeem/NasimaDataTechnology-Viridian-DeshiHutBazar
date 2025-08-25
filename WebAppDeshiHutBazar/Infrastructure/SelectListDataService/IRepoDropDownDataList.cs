using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;

namespace WebDeshiHutBazar
{
    public interface IRepoDropDownDataList
    {
        Task<string> GetPackageNameText(int packageId);

        Task<IEnumerable<SelectListItem>> GetPackageList();

        Task<List<AValueModel>> GetAllPackages();

        Task<IEnumerable<SelectListItem>> GetUsersList();

        Task<List<AValueModel>> GetAllUsers();

        Task<IEnumerable<SelectListItem>> GetFabiaServiceCategoryList();
    }
}