using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    public class LogPostVisit : BaseEntity
    {
        public LogPostVisit() {
            
        }

        public LogPostVisit(string stateName, 
            string categoryName, 
            string subCategoryName, 
            Post post, 
            User advertiserUser, 
            string visitorEmail, 
            string visitorPhoneNumber, 
            EnumCountry country,
            EnumPostVisitAction visitAction)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            LogDateTime = BaTime;

            VisitorEmail = visitorEmail;
            VisitorPhoneNumber = visitorPhoneNumber;

            AdvertiserUserID = advertiserUser.UserID;
            AdvertiserAccountEmail = advertiserUser.Email;
            AdvertiserAccountClientName = advertiserUser.ClientName;

            CategoryName = categoryName;
            SubCategoryName = subCategoryName;

            PostID = post.PostID;
            PosterName = post.PosterName;
            PosterPhoneNumber = post.PosterContactNumber;

            StateName = stateName;
            Area = post.Address != null && !string.IsNullOrEmpty(post.Address.AreaDescription) ? post.Address.AreaDescription : "";

            PostTitle = post.Title;
            PostItemPrice = post.UnitPrice;

            PostImageFile1 = post.ImageFiles.Count > 0 && post.ImageFiles[0].Image != null ? post.ImageFiles[0].Image : null;
            PostImageFile2 = post.ImageFiles.Count > 1 && post.ImageFiles[1].Image != null ? post.ImageFiles[1].Image : null;
            PostImageFile3 = post.ImageFiles.Count > 2 && post.ImageFiles[2].Image != null ? post.ImageFiles[2].Image : null;
            PostImageFile4 = post.ImageFiles.Count > 3 && post.ImageFiles[3].Image != null ? post.ImageFiles[3].Image : null;

            PostVisitAction = visitAction;
        }        

        [Key]
        public long PostVisitLogID { get; set; }

        public string VisitorEmail { get; set; }

        public string VisitorPhoneNumber { get; set; }

        public long PostID { get; set; }

        public string PostTitle { get; set; }

        public string CategoryName { get; set; }        

        public string SubCategoryName { get; set; }

        public decimal PostItemPrice { get; set; }

        public string StateName { get; set; }

        public string Area { get; set; }

        public string PosterName { get; set; }

        public string PosterPhoneNumber { get; set; }

        public DateTime LogDateTime { get; set; }

        public long AdvertiserUserID { get; set; }

        public string AdvertiserAccountEmail { get; set; }

        public string AdvertiserAccountClientName { get; set; }

        public byte[] PostImageFile1 { get; set; }

        public byte[] PostImageFile2 { get; set; }

        public byte[] PostImageFile3 { get; set; }

        public byte[] PostImageFile4 { get; set; }

        public EnumPostVisitAction PostVisitAction { get; set; }

    }
}
