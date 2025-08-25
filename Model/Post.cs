using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    public class Post : BaseEntity
    {
        public Post() {
        }

        public Post(EnumCountry country, EnumCurrency currency
            ) {
            ImageFiles = new List<File>();
            IsBrandNew = false;
            IsUrgent = false;
            IsRecent = false;
            EnumCountry = country;
            PostStatus = EnumPostStatus.Payable;
            PostType = EnumPostType.Post;

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
            Currency = currency;
        }

        public Post(EnumCountry country, EnumPostType postType, User userobject, EnumCurrency currency) {
            ImageFiles = new List<File>();
            UserID = userobject.UserID;
            Currency = currency;
            EnumCountry = country;
            PostType = postType;
            if (postType != EnumPostType.Post)
            {
                PostStatus = EnumPostStatus.FreePosted;
                CategoryID = 0;
                SubCategoryID = 0;
                var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
                DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
                PublishDate = BaTime;
                IsBrandNew = false;
                IsUrgent = false;
                IsRecent = false;
                
                CreatedDate = BaTime;
                ModifiedDate = BaTime;
                EnumCountry = country;
            }                       
        }

        [Key]
        public long PostID { get; set; }

        public EnumPostType? PostType { get; set; }

        public DateTime? PublishDate { get; set; }

        public long UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public string PosterContactNumber { get; set; }

        public string PosterName { get; set; }

        public string WebsiteUrl { get; set; }

        public long AddressID { get; set; }

        [ForeignKey("AddressID")]
        public virtual PostAddress Address { get; set; }
        
        public string Title { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public long CategoryID { get; set; }

        public long SubCategoryID { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal DiscountedUnitPrice { get; set; }

        public decimal DeshiHutBazarCommissionAmount { get; set; }

        public double PaymentGatewayCommissionAmountPercent { get; set; }        
        
        public decimal PortalProductPrice { get; set; }
        
        public decimal ShopShareAmount { get; set; }
        
        public decimal DeshiHutBazarShareAmount { get; set; }

        public int AvailableTotalUnits { get; set; }

        public EnumCurrency Currency { get; set; }

        public bool IsBrandNew { get; set; }

        public bool IsUsed { get; set; }

        public bool IsUrgent { get; set; }

        public bool IsRecent { get; set; }

        public bool IsStudentDeal { get; set; }        

        public bool IsForSell { get; set; }

        public bool IsForRent { get; set; }

        public int? LikeCount { get; set; }

        public string SearchTag { get; set; }

        public EnumPostStatus PostStatus { get; set; }

        [ForeignKey("UserPackageID")]
        public UserPackage UserPackage { get; set; }

        public long? UserPackageID { get; set; }

        public virtual List<File> ImageFiles { get; set; }

        public virtual List<PostComment> ListComments { get; set; }

        public virtual List<PostProcess> ListPostProcess { get; set; }

        public virtual List<PostService> ListPostService { get; set; }

        public virtual List<GroupPanelPost> GroupPanelPost { get; set; }

        public virtual List<Provider> ListFabiaProvider { get; set; }

        public void SetPublishDate()
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            PublishDate = BaTime;
        }

        public string GetViewMoreURL(string URL, long? pSubCategoryID)
        {
            if (pSubCategoryID.HasValue)
            {
                return URL.Replace("SUB_CAT_ID", pSubCategoryID.ToString());
            }
            else if (SubCategoryID != 0)
            {
                return URL.Replace("SUB_CAT_ID", SubCategoryID.ToString());
            }
            else
            {
                return "";
            }
        }

        public string GetItemDetailsURL(string URL)
        {
            if (PostType == EnumPostType.Product || PostType == EnumPostType.Post || PostType == EnumPostType.FabiaService && !string.IsNullOrEmpty(URL))
            {
                return URL.Replace("POST_ITEM_ID", PostID.ToString());
            }
            else if (!string.IsNullOrEmpty(WebsiteUrl))
            {
                return WebsiteUrl;
            }
            else
            {
                return "";
            }
        }

        public string GetFormatedPriceValue(string currency)
        {
            var leftStr2 = "";
            var rightStr2 = "";
            var dotIndex = UnitPrice.ToString().IndexOf('.');
            if (dotIndex != -1)
            {
                leftStr2 = UnitPrice.ToString().Substring(0, dotIndex);
                rightStr2 = UnitPrice.ToString().Substring(dotIndex);
            }
            else
            {
                leftStr2 = UnitPrice.ToString();
            }
            var originalstr = leftStr2;
            if (originalstr == "0" || originalstr == "00")
            {
                return "0.00 " + currency;
            }
            var remainder = originalstr.Length % 3;
            var devident = originalstr.Length / 3;
            var finalstring = "";
            var j = remainder == 0 ? 1 : 5;
            var counter = 1;
            for (var i = 0; i < originalstr.Length; i++)
            {
                if (counter == remainder + 1 && remainder != 0)
                {
                    finalstring += ",";
                    finalstring += originalstr[i];
                    j = 1;
                }
                else
                {
                    if (j == 4)
                    {
                        finalstring += ",";
                        finalstring += originalstr[i];
                        j = 1;
                    }
                    else
                    {
                        finalstring += originalstr[i];
                    }
                }
                counter++;
                j++;
            }
            if (!string.IsNullOrEmpty(rightStr2))
            {
                finalstring = finalstring + rightStr2 + " " + currency;
            }
            else
            {
                finalstring = finalstring + " " + currency;
            }
            return finalstring;
        }

        public int GetCommentCount
        {
            get
            {
                return ListComments != null && ListComments.Count > 0 ? ListComments.Count : 0;
            }
        }

        public EnumPostType GetpostType
        {
            get
            {
                if (PostType.HasValue)
                    return PostType.Value;
                return EnumPostType.Post;
            }
        }

        public DateTime GetPublishDate
        {
            get
            {
                if (PublishDate.HasValue)
                    return PublishDate.Value;
                return DateTime.Today;
            }
        }

        public int GetLikeCount
        {
            get
            {
                if (LikeCount.HasValue)
                    return LikeCount.Value;

                return 0;
            }
        }
    }
}
