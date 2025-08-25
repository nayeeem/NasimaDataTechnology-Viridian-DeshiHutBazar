using System.Collections.Generic;
using Common;
namespace WebDeshiHutBazar
{
    public class GroupPanelTemplateDisplayViewModel
    {
        public GroupPanelTemplateDisplayViewModel()
        {
            ListGroupPost = new List<TemplateDisplayViewModel>();
            PageingModelAll = new PagingModel();
            FabiaInformationViewModel = new FabiaInformationViewModel();
        }       
        
        public List<TemplateDisplayViewModel> ListGroupPost { get; set; }

        public string SectionID { get; set; }

        public int GroupPanelConfigID { get; set; }
      
        public GroupPostInformationViewModel GroupPostInformationViewModel { get; set; }
        
        public string GroupPanelTitle { get; set; }
        
        public EnumShowOrHide? ShowOrHide { get; set; }

        public EnumDeviceType? Device { get; set; }

        public EnumColumn? Column { get; set; }
       
        public EnumImageCategory? EnumImageCategory { get; set; }

        public EnumPanelDisplayStyle? EnumPanelDisplayStyle { get; set; }

        public EnumCountry EnumCountry { get; set; }

        public int? Order { get; set; }

        public string ViewMoreUrl { get; set; }

        public EnumPublicPage? EnumPublicPage { get; set; }

        public long? NoOfRows { get; set; }

        public string DisplaySubCategory { get; set; }

        public string DisplayCategory { get; set; }

        public string Website { get; set; }

        public string CategoryID { get; set; }

        public string SubCategoryID { get; set; }

        public PagingModel PageingModelAll { get; set; }

        public string ImageCategoryClass
        {
            get
            {
                var temp = "";
                if (EnumImageCategory == Common.EnumImageCategory.HorizontalRectengle)
                {
                    if (Device == EnumDeviceType.Desktop)
                    {
                        temp = "desktop-hr-";
                    }
                    else
                    {
                        temp = "mobile-hr-";
                    }
                }
                else if (EnumImageCategory == Common.EnumImageCategory.VerticalRectengle)
                {
                    if (Device == EnumDeviceType.Desktop)
                    {
                        temp = "desktop-vr-";
                    }
                    else
                    {
                        temp = "mobile-vr-";
                    }
                }
                else if (EnumImageCategory == Common.EnumImageCategory.Suqare)
                {
                    if (Device == EnumDeviceType.Desktop)
                    {
                        temp = "desktop-sq-";
                    }
                    else
                    {
                        temp = "mobile-sq-";
                    }
                }

                if (Column == EnumColumn.One)
                {
                    return temp + "12";
                }
                else if (Column == Common.EnumColumn.Two)
                {
                    return temp + "6";
                }
                else if (Column == Common.EnumColumn.Three)
                {
                    return temp + "4";
                }
                else if (Column == Common.EnumColumn.Four)
                {
                    return temp + "3";
                }
                else if (Column == Common.EnumColumn.Six)
                {
                    return "desktop-2";
                }

                if (Device == EnumDeviceType.Desktop)
                {
                    return "desktop-sq-3";
                }
                else
                {
                    return "";
                }
            }
        }

        public string ColumnClass
        {
            get
            {
                if (Device == EnumDeviceType.Desktop)
                {
                    if (Column == Common.EnumColumn.One)
                    {
                        return "col-md-12";
                    }
                    else if (Column == Common.EnumColumn.Two)
                    {
                        return "col-md-6";
                    }
                    else if (Column == Common.EnumColumn.Three)
                    {
                        return "col-md-4";
                    }
                    else if (Column == Common.EnumColumn.Four)
                    {
                        return "col-md-3";
                    }
                    else
                    {
                        return "col-md-2";
                    }
                }
                else if (Device == EnumDeviceType.Mobile)
                {
                    if (Column == Common.EnumColumn.One)
                    {
                        return "col-xs-12";
                    }
                    else if (Column == Common.EnumColumn.Two)
                    {
                        return "col-xs-6";
                    }
                    else
                    {
                        return "col-xs-12";
                    }
                }
                else
                {
                    return "col-md-3";
                }
            }
        }

        public FabiaInformationViewModel FabiaInformationViewModel { get; set; }

        public int TotalPostCount { get; set; }

        public EnumGroupPanelStatus? PublishStatus { get; set; }

        public bool HasToShowPostsOrOrder { get; set; }
    }
}
