using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Model;

namespace Data
{
    public interface IGroupPanelConfigRepository
    {
        Task<List<GroupPanelConfig>> GetAllPublishedGroupPanelConfig(EnumCountry country);

        Task<List<GroupPanelConfig>> GetAllPublishedGroupPanelConfig(EnumCountry country,
                                                                    EnumPublicPage page,
                                                                    EnumGroupPanelStatus? isPublished);

        Task<List<GroupPanelConfig>> GetAllGroupPanelConfig(EnumCountry country);

        Task<bool> AddGroupPanelConfig(GroupPanelConfig objGroupPanelConfiguration, long currentUserID, EnumCountry country);

        Task<bool> UpdateGroupPanelConfig(GroupPanelConfig singleConfigEntity, long currentUserID, EnumCountry country);

        Task<bool> DeleteGroupPanelConfig(int id, long currentUserID, EnumCountry country);

        Task<bool> PublishGroupPanelConfig(EnumDeviceType device, EnumPublicPage? page, long currentUserID, EnumCountry country);

        Task<bool> SaveChanges();

        Task<GroupPanelConfig> GetSingleGroupPanelConfig(EnumCountry country, int groupPanelConfigID);

        Task<List<GroupPanelConfig>> GetAllUserGroupPanelConfig(EnumCountry country, long userId);
    }
}
