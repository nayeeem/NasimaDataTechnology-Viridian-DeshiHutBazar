using System;
using Common;
using System.ComponentModel.DataAnnotations;

using System.Collections.Generic;

namespace WebDeshiHutBazar
{
    public class TemplateDisplayViewModel : BaseDisplayViewModel
    {
        public TemplateDisplayViewModel()
        {
            ListPostComments = new List<PostCommentViewModel>();
        }

        public TemplateDisplayViewModel(EnumCurrency currency) : base(currency)
        {
            ListPostComments = new List<PostCommentViewModel>();
        }  
        
        public long GroupPanelConfigID { get; set; }

        public EnumShowOrHide? ShowOrHide { get; set; }

        public long PostID { get; set; }
        
        [Display(Name="Title:")]
        public string Title { get; set; }
           
        public string WebsiteUrl { get; set; }

        [Display(Name = "Email:")]
        public string Email { get; set; }
        
        [Display(Name = "Name:")]
        public string ClientName { get; set; }
        
        public string PosterContactNumber { get; set; }

        public string PosterName { get; set; }

        [Display(Name = "Phone:")]
        public string Phone { get; set; }                
         
        [Display(Name = "State:")]
        public string DisplayState { get; set; } 
       
        [Display(Name = "Price:")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public DateTime PublishDate { get; set; }

        public string FormattedPriceValue { get; set; }

        public byte[] Image { get; set; }
     
        public bool IsBrandNew { get; set; }

        public string DisplayProductType { get; set; }
        
        public bool IsUrgent { get; set; }

        public string DisplayUrgentDeal { get; set; }
        
        public bool IsPrivateSeller { get; set; }

        public bool IsCompanySeller { get; set; }

        public string DisplaySellerType { get; set; }

        public bool IsForSell { get; set; }

        public bool IsForRent { get; set; }

        public bool IsUsed { get; set; }

        public string DisplayForSellRent { get; set; }
               
        public int LikeCount { get; set; }
        
        public int Order { get; set; }

        public EnumImageCategory? EnumImageCategory { get; set; }

        public string GroupPanelTitle { get; set; }

        public string PostItemDetailViewUrl { get; set; }       
         
        public EnumPostStatus PostStatus { get; set; }
        
        public long CommentsCount { get; set; }

        public EnumPostType PostType { get; set; }

        public EnumOfferType OfferType { get; set; }

        public string Comment { get; set; }

        public string Description { get; set; }

        public long CategoryID { get; set; }

        public long SubCategoryID { get; set; }

        public string DisplayCategory { get; set; }

        public string DisplaySubCategory { get; set; }

        public List<PostCommentViewModel> ListPostComments { get; set; }

        public string ViewMoreUrl { get; set; }     
        
        public string ImageCategoryClass { get; set; }

        public string ColumnClass { get; set; }

        public long GroupPostID { get; set; }

        public bool DisplayCartButton { get; set; }

        public bool ShowCartButton { get; set; }

    }
}
