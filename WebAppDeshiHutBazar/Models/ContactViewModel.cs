using System.Collections.Generic;
using System.Web.Mvc;


namespace WebDeshiHutBazar
{
    public class ContactViewModel
    {
        public ContactViewModel()
        {
            AV_MessageTypeList = DropDownDataList.GetAllContactMessageType();
        }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string MessageCategory { get; set; }

        public List<SelectListItem> AV_MessageTypeList { get; set; }

        public string PageName { get; set; }
    }
}
