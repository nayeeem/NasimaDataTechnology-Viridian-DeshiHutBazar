using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;
using Model;


namespace WebDeshiHutBazar
{
    public interface IGroupPanelConfigService
    {
        Task<List<GroupPanelTemplateDisplayViewModel>>
           GetAllPageGroupPanelConfigurations(
                   EnumPublicPage pageName,
                   string viewMoreUrl,
                   string viewPostDetailsUrl,
                   EnumCountry country,
                   int dayTimeSlot,
                   EnumCurrency currency);

        Task<List<GroupPanelTemplateDisplayViewModel>>
            GetAllPageGroupPanelConfigurations(
                    EnumPublicPage pageName,
                    string viewMoreUrl,
                    string viewPostDetailsUrl,
                    EnumCountry country,
                    int dayTimeSlot,
                    EnumMarketType? typeMarket,
                    long typeMarketCategoryID,
                    int? pageNumber,
                    int pageSize,
                    decimal? price,
                    EnumCurrency currency
                    );
     
        Task<bool> PublishAllGroupPanelConfig(EnumDeviceType device, EnumPublicPage page, long currentUserID, EnumCountry country);

        Task<GroupPanelConfigurationViewModel> GetSingleGroupPanelConfig(int panelConfigID, EnumCountry country);

        Task<bool> AddGroupPanelConfig(GroupPanelConfigurationViewModel objGroupPanelVM, long currentUserID,  EnumCountry country);

        Task<bool> AddSelectedPost(int postID, int groupConfigID, long fileId, EnumCountry country, long currentUserID);

        Task<bool> UpdateGroupPanelConfig(GroupPanelConfigurationViewModel objGroupPanelVM, EnumCountry country, long currentUserID);

        Task<bool> RemoveSelectedPost(int groupPostID, long currentUserID,EnumCountry country);

        Task<bool> DeleteGroupPanelConfig(int id, long currentUserID, EnumCountry country);

        Task<bool> UpdateDisplayOrder(PanelDisplayOrderViewModel objNewDisplayOrder, EnumCountry country, long currentUserID);

        Task<bool> UpdatePostDisplayOrder(List<int> listGroupPosts, long currentUserID, EnumCountry country);

        Task<GroupPanelConfigurationViewModel> GetSingleGroupConfigPosts(int panelConfigID, EnumCountry country, EnumCurrency currency);
    }
}
