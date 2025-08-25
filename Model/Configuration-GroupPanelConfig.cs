using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    public class GroupPanelConfig : BaseEntity
    {
        public GroupPanelConfig() {
        }

        public GroupPanelConfig(
            int? order,
            EnumShowOrHide? showOrHide,
            EnumDeviceType device,
            EnumGroupPanelStatus? enumGroupPanelStatus,
            long currentUserId,
            EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            ShowOrHide = showOrHide;
            Order = order;
            Device = device;
            EnumGroupPanelStatus = enumGroupPanelStatus;
            CreatedBy = currentUserId;
            ListPanelPost = new List<GroupPanelPost>();
        }

        [Key]
        public int GroupPanelConfigID { get; set; }

        public EnumGroupPanelStatus? EnumGroupPanelStatus { get; set; }

        public EnumShowOrHide? ShowOrHide { get; set; }

        public int? Order { get; set; }

        public EnumPanelDisplayStyle? EnumPanelDisplayStyle { get; set; }

        public EnumPublicPage? EnumPublicPage { get; set; }

        public EnumDeviceType Device { get; set; }

        public string GroupPanelTitle { get; set; }

        public string GroupPanelTitleBangla { get; set; }

        [ForeignKey("PanelConfigUserID")]
        public virtual User PanelConfigUser { get; set; }

        public long? PanelConfigUserID { get; set; }

        public virtual List<GroupPanelPost> ListPanelPost { get;  set; }        
    }
}
