using System.Collections.Generic;
namespace WebDeshiHutBazar
{
    public class ContentInfoViewModel 
    {
        public ContentInfoViewModel()  {
            ListGroupPanelConfiguration = new List<GroupPanelTemplateDisplayViewModel>();
        }
        
        public List<GroupPanelTemplateDisplayViewModel> ListGroupPanelConfiguration { get; set; }

        public long? CategoryID { get; set; }
    }
}
