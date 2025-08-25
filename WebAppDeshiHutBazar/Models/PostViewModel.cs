using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Common;
using System.ComponentModel.DataAnnotations;
using Common.Language;

namespace WebDeshiHutBazar
{
    public class PostViewModel : BaseViewModel
    {
        public PostViewModel()
        {
            ListImages = new List<FileViewModel>();
            CategorySearchInfoModel = new CategorySearchInfoModel();
            ContentInfoViewModel = new ContentInfoViewModel();
            ItemDetailsCompanyAboutPanelDesktop = new GroupPanelConfigurationViewModel();
            ItemDetailsCompanyAboutPanelMobile = new GroupPanelConfigurationViewModel();
            ListPostProcess = new List<PostProcessViewModel>();
            ListPostService = new List<PostServiceViewModel>();
            PostServiceViewModel = new PostServiceViewModel();
            PostProcessViewModel = new PostProcessViewModel();
            FabiaInformationViewModel = new FabiaInformationViewModel();
            ListFabiaProvider = new List<FabiaProviderViewModel>();
        }
        public PostViewModel(EnumCurrency currency) : base(currency)
        {
            ListImages = new List<FileViewModel>();
            CategorySearchInfoModel = new CategorySearchInfoModel();
            ContentInfoViewModel = new ContentInfoViewModel();
            ItemDetailsCompanyAboutPanelDesktop = new GroupPanelConfigurationViewModel();
            ItemDetailsCompanyAboutPanelMobile = new GroupPanelConfigurationViewModel();
            ListPostProcess = new List<PostProcessViewModel>();
            ListPostService = new List<PostServiceViewModel>();
            PostServiceViewModel = new PostServiceViewModel(currency);
            PostProcessViewModel = new PostProcessViewModel(currency);
            FabiaInformationViewModel = new FabiaInformationViewModel();
            ListFabiaProvider = new List<FabiaProviderViewModel>();
        } 

        public long PostID { get; set; }

        [Display(Name="Title")]
        [Required(ErrorMessage = "The field Title is required!")]
        public string Title { get; set; }

        [MaxLength(4000)]
        [Display(Name = "Description")]
        public string Description { get; set; }
  
        [Display(Name = "Category")]
        [Required(ErrorMessage = "The field Category is required!")]
        public long? CategoryID { get; set; }

        [Display(Name = "Category")]
        public string DisplayCategory { get; set; }
    
        [Display(Name = "Sub Category")]
        [Required(ErrorMessage = "The field Sub-Category is required!")]
        public long? SubCategoryID { get; set; }
        
        [Display(Name = "Sub Category")]
        public string DisplaySubCategory { get; set; }

        [Display(Name = "User Name")]
        public long UserID { get; set; }
      
        [StringLength(25)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [MaxLength(8)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [MaxLength(8)]
        [Display(Name = "Retype Password")]
        public string RePassword { get; set; }       

        [StringLength(50)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Name")]
        public string ClientName { get; set; }
        
        [StringLength(40)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }                

        [Display(Name = "Location")]
        public long Location { get; set; }
       
        [Display(Name = "State")]
        [Required(ErrorMessage = "The field State is required!")]
        public long? StateID { get; set; }
        
        [Display(Name = "State")]
        public string DisplayState { get; set; } 

        [Display(Name = "Address Detail")]
        public string AddressDetail { get; set; }

        public long AddressID { get; set; }

        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal DiscountedPrice { get; set; }

        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal DeshiHutCommissionAmount { get; set; }

        [Display(Name = "Payment Gateway Commission (percent)")]
        public double PaymentGatewayCommision { get; set; }

        [Display(Name = "Portal Product Price")]
        public decimal PortalProductPrice { get; set; }

        [Display(Name = "Shop Share Amount")]
        public decimal ShopShareAmount { get; set; }

        [Display(Name = "Deshi Hut Bazar Share Amount")]
        public decimal DeshiHutBazarShareAmount { get; set; }

        public string FormattedPriceValue { get; set; }

        public List<FileViewModel> ListImages { get; set; }

        public bool IsBrandNew { get; set; }

        public bool IsUsed { get; set; }

        public bool IsUrgent { get; set; }        
            
        public bool IsRecent { get; set; }

        public int? LikeCount { get; set; }

        public int Order { get; set; }

        public string PostItemDetailViewUrl { get; set; }       

        [Display(Name = "Area")]
        public string AreaDescription { get; set; }

        public bool IsPrivateSeller { get; set; }

        public bool IsCompanySeller { get; set; }
        
        public bool IsForSell { get; set; }

        public bool IsForRent { get; set; }

        public FileViewModel MarketItemDisplayImage { get; set; }

        public string SearchTag { get; set; }

        public EnumPostStatus PostStatus { get; set; }

        public string FBUserID { get; set; }

        public string FBClientName { get; set; }

        public string FBEmail { get; set; }

        public long? UserPackageID { get; set; }

        public DateTime? PublishDate { get; set; }

        public string PosterContactNumber { get; set; }

        public string PosterName { get; set; }

        public long GroupPostID { get; set; }

        public int GroupPanelConfigID { get; set; }        
        
        public byte[] Image { get; set; }

        [Display(Name = "CommentCommentButton", ResourceType = typeof(LanguageDefault))]
        public string Comment { get; set; }
        
        public long? PriceLow { get; set; }

        public long? PriceHigh { get; set; }

        public string SearchKey { get; set; }

        public string SimpleSearchKey { get; set; }

        public long CommentsCount { get; set; }

        public string SubCategoryCSS { get; set; }

        public string CategoryCSS { get; set; }        

        public string WebsiteUrl { get; set; }

        public EnumPostType? PostType { get; set; }

        public EnumOfferType OfferType { get; set; }

        [Display(Name = "Post Type")]
        [Required]
        public long? PostTypeID { get; set; }

        public GroupPanelConfigurationViewModel ItemDetailsCompanyAboutPanelDesktop { get; set; }

        public GroupPanelConfigurationViewModel ItemDetailsCompanyAboutPanelMobile { get; set; }

        public FabiaInformationViewModel FabiaInformationViewModel { get; set; }

        public PostServiceViewModel PostServiceViewModel { get; set; }

        public PostProcessViewModel PostProcessViewModel { get; set; }

        public ContentInfoViewModel ContentInfoViewModel { get; set; }

        public List<PostCommentViewModel> ListPostComments { get; set; }

        public List<PostProcessViewModel> ListPostProcess { get; set; }

        public List<PostServiceViewModel> ListPostService { get; set; }

        public List<FabiaProviderViewModel> ListFabiaProvider { get; set; }

        public IEnumerable<SelectListItem> AV_State { get; set; }

        public IEnumerable<SelectListItem> AV_Category { get; set; }

        public IEnumerable<SelectListItem> AV_SubCategory { get; set; }

        public IEnumerable<SelectListItem> AV_PostType { get; set; }

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
    }
}
