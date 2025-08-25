using System.Collections.Generic;
using System.Threading.Tasks;
using Common;

namespace WebDeshiHutBazar
{
    public interface IContentManagementDataService
    {
        Task<List<GroupPanelTemplateDisplayViewModel>>
           GetAllPageGroupPanelConfigurations(
                   EnumPublicPage pageName,
                   string viewMoreUrl,
                   string viewPostDetailsUrl,
                   EnumCountry country,
                   int dayTimeSlot);

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
                    decimal? price
                    );
     
        Task<GroupPanelConfigurationViewModel> GetSingleGroupPanelConfig(int panelConfigID, EnumCountry country);

        Task<GroupPanelConfigurationViewModel> GetSingleGroupConfigPosts(int panelConfigID, EnumCountry country);
    }
}
