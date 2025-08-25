using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model;

namespace Model
{
    public class GroupPanelPost : BaseEntity
    {
        public GroupPanelPost() {
        }

        public GroupPanelPost(int orderDisplay,
                            GroupPanelConfig groupPanelConfig,
                            Post post,
                            File file,
                            EnumSelectionType enumSelecionType,
                            long currentUserId,
                            EnumCountry country
                            )
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            GroupPanelConfigID = groupPanelConfig.GroupPanelConfigID;
            PostID = post.PostID;
            DisplayOrder = orderDisplay;
            EnumSelectionType = enumSelecionType;            
            SetExpireDate(BaTime.AddDays(30), country);
            FileID = file.FileID;
            IsActive = true;
            CreatedBy = currentUserId;
        }

        public GroupPanelPost(int orderDisplay,
                            GroupPanelConfig groupPanelConfig,
                            Post post,
                            EnumSelectionType enumSelecionType,
                            long currentUserId,
                            EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            GroupPanelConfigID = groupPanelConfig.GroupPanelConfigID;
            PostID = post.PostID;
            DisplayOrder = orderDisplay;
            EnumSelectionType = enumSelecionType;            
            SetExpireDate(BaTime.AddDays(30), country);
            IsActive = true;
            CreatedBy = currentUserId;
        }

        [Key]
        public long GroupPostID { get; set; }

        [ForeignKey("PostID")]
        public virtual Post Post { get; set; }

        public long? PostID { get; set; }

        [ForeignKey("GroupPanelConfigID")]
        public virtual GroupPanelConfig GroupPanelConfig { get; set; }

        public int? GroupPanelConfigID { get; set; }

        public DateTime? SelectionDate { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? RemovalDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public int DisplayOrder { get; set; }
        
        public EnumSelectionType EnumSelectionType { get;  set; }        
        
        public void MarkPostRemoved()
        {
            IsActive = false;
        }

        public void SetSelectionDate(EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            SelectionDate = BaTime;
        }

        public void SetPurchaseDate(EnumCountry country)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            PurchaseDate = BaTime;
        }

        public void SetRemovalDate(EnumCountry country)
        {

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            RemovalDate = BaTime;
        }

        public void SetExpireDate(DateTime expireDate, EnumCountry country)
        {

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(expireDate, TimeZoneInfo.Local, BnTimeZone);
            ExpireDate = BaTime;
        }

        public long? FileID { get; set; }

        [ForeignKey("FileID")]
        public virtual File PrimaryImage { get; set; }
    }
}
