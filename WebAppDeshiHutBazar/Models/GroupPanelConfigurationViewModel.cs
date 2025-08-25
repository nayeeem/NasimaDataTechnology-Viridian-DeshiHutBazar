using System.Web.Mvc;
using System.Collections.Generic;
using Common;
using System.ComponentModel.DataAnnotations;

namespace WebDeshiHutBazar
{
    public class GroupPanelConfigurationViewModel : BaseViewModel
    {
        public GroupPanelConfigurationViewModel()
        {
            ListGroupPost = new List<PostViewModel>();
            ListSelectPost = new List<PostViewModel>();            
            ListGroupTemplatePost = new List<TemplateDisplayViewModel>();
        }        

        [Display(Name = "Group Panel Configuration")]
        public int GroupPanelConfigID { get; set; }

        public string GroupPanelTitle { get; set; }

        public string GroupPanelTitleBangla { get; set; }

        [Display(Name = "Show / Hide")]
        public EnumShowOrHide? ShowOrHide { get; set; }

        public long? PanelConfigUserID { get; set; }
        
        [Display(Name = "Country")]
        public EnumCountry EnumCountry { get; set; }

        [Display(Name = "Order")]
        public int? Order { get; set; }

        public string ViewMoreUrl { get; set; }

        public string CategoryCSS { get; set; }

        public string SubCategoryCSS { get; set; }

        public EnumPublicPage? PublicPage { get; set; }

        public IEnumerable<SelectListItem> AV_Device { get; set; }

        public IEnumerable<SelectListItem> AV_ShowHide { get; set; }

        public IEnumerable<SelectListItem> AV_EnumPanelDisplayStyle { get; set; }

        public IEnumerable<SelectListItem> AV_EnumPublicPage { get; set; }

        public IEnumerable<SelectListItem> AV_Users { get; set; }

        [Display(Name = "Posts")]
        public List<PostViewModel> ListGroupPost { get; set; }

        [Display(Name = "Posts")]
        public List<TemplateDisplayViewModel> ListGroupTemplatePost { get; set; }

        public List<PostViewModel> ListSelectPost { get; set; }

        public GroupPostInformationViewModel GroupPostInformationViewModel { get; set; }

        public EnumPanelDisplayStyle? EnumPanelDisplayStyle { get; set; }

        public EnumDeviceType Device { get; set; }

        public string DisplayShow
        {
            get
            {
                if (ShowOrHide == EnumShowOrHide.Yes)
                {
                    return "Show";
                }
                else
                {
                    return "Hide";
                }
            }
        }

        public string DisplayDevice
        {
            get
            {
                return Device.ToString();
            }
        }
    }
}
