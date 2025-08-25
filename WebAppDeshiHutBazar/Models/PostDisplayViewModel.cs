using System;
using Common;
using System.ComponentModel.DataAnnotations;

using System.Collections.Generic;
using Common.Language;

namespace WebDeshiHutBazar
{
    public class PostDisplayViewModel : BaseDisplayViewModel
    {
        public PostDisplayViewModel()
        {
            FabiaInformationViewModel = new FabiaInformationViewModel();
            ContentInfoViewModel = new ContentInfoViewModel();
        }
        public PostDisplayViewModel(EnumCurrency currency) : base(currency)
        {
            FabiaInformationViewModel = new FabiaInformationViewModel();
            ContentInfoViewModel = new ContentInfoViewModel();
        }

        public long CategoryID { get; set; }

        public long SubCategoryID { get; set; }

        public ContentInfoViewModel ContentInfoViewModel { get; set; }

        public string PageName { get; set; }

        public FabiaInformationViewModel FabiaInformationViewModel { get; set; }

        public List<FileViewModel> ListImages { get; set; }

        public List<PostCommentViewModel>  ListPostComments { get; set; }

        public byte[] Image { get; set; }

        public long UserID { get; set; }

        public long PostID { get; set; }
        
        [Display(Name="Title:")]
        public string Title { get; set; }
       
        [Display(Name = "Description:")]
        public string Description { get; set; }
                       
        [Display(Name = "Category:")]
        public string DisplayCategory { get; set; }
               
        [Display(Name = "Sub Category:")]
        public string DisplaySubCategory { get; set; }
       
        [Display(Name = "User Name:")]
        public string UserName { get; set; }
        
        [Display(Name = "Password:")]
        public string Password { get; set; }
        
        [Display(Name = "Retype Password:")]
        public string RePassword { get; set; }

        public string WebsiteUrl { get; set; }

        [Display(Name = "Email:")]
        public string Email { get; set; }
        
        [Display(Name = "Name:")]
        public string ClientName { get; set; }

        public string FaceBookUserClientName { get; set; }

        public string FaceBookUserEmail { get; set; }

        public string PosterContactNumber { get; set; }

        public string PosterName { get; set; }

        public string Currency { get; set; }

        public string SubCategoryCSS { get; set; }

        public string CategoryCSS { get; set; }

        [Display(Name = "Phone:")]
        public string Phone { get; set; }                
       
        [Display(Name = "Location:")]
        public long Location { get; set; }
             
        [Display(Name = "State:")]
        public string DisplayState { get; set; } 
       
        [Display(Name = "Address Detail:")]
        public string AddressDetail { get; set; }

        [Display(Name = "Area:")]
        public string AreaDescription { get; set; }

        [Display(Name = "Price:")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Discounted Price:")]
        [DataType(DataType.Currency)]
        public decimal DiscountedPrice { get; set; }

        public DateTime? PublishDate { get; set; }

        public string FormattedPriceValue { get; set; }

        public byte[] DisplayImage { get; set; }

        public string SearchTag { get; set; }
                
        public bool IsBrandNew { get; set; }
        
        public bool IsUsed { get; set; }
        
        public bool IsUrgent { get; set; }

        public bool IsPrivateSeller { get; set; }

        public bool IsCompanySeller { get; set; }

        public bool IsForSell { get; set; }

        public bool IsForRent { get; set; }

        public string UrgentDisplay
        {
            get
            {
                return IsUrgent ? "Urgent Deal" : string.Empty;
            }
        }
       
        public string NewItemDisplay
        {
            get
            {
                return IsBrandNew ? "New Item" : "Used Item";
            }
        }

        public int LikeCount { get; set; }
        
        public int Order { get; set; }
        
        public string PostItemDetailViewUrl { get; set; }       
                
        public string DealerTypeDisplay
        {
            get
            {
                if (IsPrivateSeller)
                    return "Private";
                if (IsCompanySeller)
                    return "Company";
                return "Private";
            }
        }

        public EnumPostStatus PostStatus { get; set; }

        [Display(Name = "CommentCommentButton", ResourceType = typeof(LanguageDefault))]
        public string Comment { get; set; }

        public long CommentsCount { get; set; }

        public EnumPostType? PostType { get; set; }

        public EnumOfferType OfferType { get; set; }
    }
}
