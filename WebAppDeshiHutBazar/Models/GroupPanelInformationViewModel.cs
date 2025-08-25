using System.Web.Mvc;
using System.Collections.Generic;
using Common;
namespace WebDeshiHutBazar
{
    public class GroupPanelInformationViewModel 
    {
        public GroupPanelInformationViewModel()
        {
            ListGroupPanelConfig = new List<GroupPanelConfigurationViewModel>();
            GroupPanelConfigurationViewModel = new GroupPanelConfigurationViewModel();
            ListGroupPanelTemplateConfiguration = new List<GroupPanelTemplateDisplayViewModel>();
            MenuObjectModel = new MenuObjectModel();
        }

        public EnumPublicPage? EnumPublicPage { get; set; }

        public EnumDeviceType? EnumDevice { get; set; }

        public string PageName { get; set; }

        public IEnumerable<SelectListItem> AV_Device { get; set; }

        public IEnumerable<SelectListItem> AV_ShowHide { get; set; }

        public IEnumerable<SelectListItem> AV_PageList { get; set; }

        public List<GroupPanelTemplateDisplayViewModel> ListGroupPanelTemplateConfiguration { get; set; }

        public List<GroupPanelConfigurationViewModel> ListGroupPanelConfig { get; set; }

        public GroupPanelConfigurationViewModel GroupPanelConfigurationViewModel { get; set; }        

        public MenuObjectModel MenuObjectModel { get; set; }
    }
}
