namespace Model
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new_db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostAddresses",
                c => new
                    {
                        AddressID = c.Long(nullable: false, identity: true),
                        StateID = c.Long(nullable: false),
                        AreaDescription = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressID);
            
            CreateTable(
                "dbo.AValues",
                c => new
                    {
                        ValueID = c.Long(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        Variable = c.Int(nullable: false),
                        ParentValueId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ValueID);
            
            CreateTable(
                "dbo.BikashBillTransactons",
                c => new
                    {
                        BikashBillId = c.Long(nullable: false, identity: true),
                        TransactionNumber = c.String(),
                        AgentNumber = c.String(),
                        PaidAmount = c.Double(nullable: false),
                        EntryDateTime = c.DateTime(nullable: false),
                        AdminApprovalStatus = c.Int(nullable: false),
                        UserId = c.Long(),
                        UserOrderID = c.Long(),
                        UserCreditOrderID = c.Long(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BikashBillId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.UserCreditOrders", t => t.UserCreditOrderID)
                .ForeignKey("dbo.UserOrders", t => t.UserOrderID)
                .Index(t => t.UserId)
                .Index(t => t.UserOrderID)
                .Index(t => t.UserCreditOrderID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Long(nullable: false, identity: true),
                        UserAccountType = c.Int(nullable: false),
                        IsUserBlocked = c.Boolean(nullable: false),
                        Email = c.String(nullable: false),
                        AdminPersonalEmail = c.String(),
                        ClientName = c.String(),
                        Website = c.String(),
                        Phone = c.String(),
                        Remarks = c.String(),
                        Password = c.String(nullable: false),
                        Salt = c.Binary(),
                        TempAdminPinNumber = c.String(),
                        AdminPassCode = c.String(),
                        Roles = c.String(),
                        IsVerifiedAccount = c.Boolean(nullable: false),
                        VerifyCode = c.String(),
                        AccountBalance = c.Double(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                        PackageConfig_PackageConfigID = c.Int(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.PackageConfigs", t => t.PackageConfig_PackageConfigID)
                .Index(t => t.PackageConfig_PackageConfigID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostID = c.Long(nullable: false, identity: true),
                        PostType = c.Int(),
                        PublishDate = c.DateTime(),
                        UserID = c.Long(nullable: false),
                        PosterContactNumber = c.String(),
                        PosterName = c.String(),
                        WebsiteUrl = c.String(),
                        AddressID = c.Long(nullable: false),
                        Title = c.String(),
                        Description = c.String(maxLength: 4000),
                        CategoryID = c.Long(nullable: false),
                        SubCategoryID = c.Long(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountedUnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeshiHutBazarCommissionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentGatewayCommissionAmountPercent = c.Double(nullable: false),
                        PortalProductPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShopShareAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeshiHutBazarShareAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AvailableTotalUnits = c.Int(nullable: false),
                        Currency = c.Int(nullable: false),
                        IsBrandNew = c.Boolean(nullable: false),
                        IsUsed = c.Boolean(nullable: false),
                        IsUrgent = c.Boolean(nullable: false),
                        IsRecent = c.Boolean(nullable: false),
                        IsStudentDeal = c.Boolean(nullable: false),
                        IsForSell = c.Boolean(nullable: false),
                        IsForRent = c.Boolean(nullable: false),
                        LikeCount = c.Int(),
                        SearchTag = c.String(),
                        PostStatus = c.Int(nullable: false),
                        UserPackageID = c.Long(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostID)
                .ForeignKey("dbo.PostAddresses", t => t.AddressID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.UserPackages", t => t.UserPackageID)
                .Index(t => t.UserID)
                .Index(t => t.AddressID)
                .Index(t => t.UserPackageID);
            
            CreateTable(
                "dbo.GroupPanelPosts",
                c => new
                    {
                        GroupPostID = c.Long(nullable: false, identity: true),
                        PostID = c.Long(),
                        GroupPanelConfigID = c.Int(),
                        SelectionDate = c.DateTime(),
                        PurchaseDate = c.DateTime(),
                        RemovalDate = c.DateTime(),
                        ExpireDate = c.DateTime(),
                        DisplayOrder = c.Int(nullable: false),
                        EnumSelectionType = c.Int(nullable: false),
                        FileID = c.Long(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GroupPostID)
                .ForeignKey("dbo.GroupPanelConfigs", t => t.GroupPanelConfigID)
                .ForeignKey("dbo.Posts", t => t.PostID)
                .ForeignKey("dbo.Files", t => t.FileID)
                .Index(t => t.PostID)
                .Index(t => t.GroupPanelConfigID)
                .Index(t => t.FileID);
            
            CreateTable(
                "dbo.GroupPanelConfigs",
                c => new
                    {
                        GroupPanelConfigID = c.Int(nullable: false, identity: true),
                        EnumGroupPanelStatus = c.Int(),
                        ShowOrHide = c.Int(),
                        Order = c.Int(),
                        EnumPanelDisplayStyle = c.Int(),
                        EnumPublicPage = c.Int(),
                        Device = c.Int(nullable: false),
                        GroupPanelTitle = c.String(),
                        GroupPanelTitleBangla = c.String(),
                        PanelConfigUserID = c.Long(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GroupPanelConfigID)
                .ForeignKey("dbo.Users", t => t.PanelConfigUserID)
                .Index(t => t.PanelConfigUserID);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileID = c.Long(nullable: false, identity: true),
                        FileName = c.String(),
                        Image = c.Binary(nullable: false),
                        PostID = c.Long(),
                        FileURL = c.String(),
                        EnumPhoto = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FileID)
                .ForeignKey("dbo.Posts", t => t.PostID)
                .Index(t => t.PostID);
            
            CreateTable(
                "dbo.PostComments",
                c => new
                    {
                        CommentID = c.Long(nullable: false, identity: true),
                        Comment = c.String(),
                        PostID = c.Long(),
                        Like = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Posts", t => t.PostID)
                .Index(t => t.PostID);
            
            CreateTable(
                "dbo.Providers",
                c => new
                    {
                        ProviderID = c.Long(nullable: false, identity: true),
                        FabiaServiceID = c.Long(),
                        UserID = c.Long(),
                        ServiceTitle = c.String(),
                        ProfileImage = c.Binary(),
                        ProviderName = c.String(),
                        ProviderPhone = c.String(),
                        ProviderWebsite = c.String(),
                        Remarks = c.String(maxLength: 4000),
                        ServiceDescription = c.String(maxLength: 4000),
                        ServiceCharge = c.Double(nullable: false),
                        StateID = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProviderID)
                .ForeignKey("dbo.Posts", t => t.FabiaServiceID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.FabiaServiceID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.PostProcesses",
                c => new
                    {
                        PostProcessID = c.Long(nullable: false, identity: true),
                        StepNo = c.Int(nullable: false),
                        StepName = c.String(),
                        Description = c.String(maxLength: 4000),
                        StepImage = c.Binary(),
                        PostID = c.Long(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AvailabilityDurationHour = c.Double(nullable: false),
                        PaidBy = c.Int(nullable: false),
                        ReasonPayment = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostProcessID)
                .ForeignKey("dbo.Posts", t => t.PostID)
                .Index(t => t.PostID);
            
            CreateTable(
                "dbo.PostServices",
                c => new
                    {
                        PostServiceID = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        ServicePolicy = c.String(maxLength: 4000),
                        TransportPolicy = c.String(maxLength: 4000),
                        Description = c.String(maxLength: 4000),
                        ServiceImage = c.Binary(),
                        PostID = c.Long(),
                        ServicePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Double(nullable: false),
                        PaidBy = c.Int(nullable: false),
                        ReasonPayment = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostServiceID)
                .ForeignKey("dbo.Posts", t => t.PostID)
                .Index(t => t.PostID);
            
            CreateTable(
                "dbo.UserPackages",
                c => new
                    {
                        UserPackageID = c.Long(nullable: false, identity: true),
                        UserID = c.Long(),
                        PackageID = c.Int(),
                        IssueDate = c.DateTime(nullable: false),
                        ExpireDate = c.DateTime(),
                        PackageName = c.String(),
                        Descriptinon = c.String(),
                        PackagePrice = c.Double(nullable: false),
                        Discount = c.Int(nullable: false),
                        TotalFreePost = c.Int(nullable: false),
                        TotalPremiumPost = c.Int(nullable: false),
                        PackageType = c.Int(nullable: false),
                        PackageStatus = c.Int(nullable: false),
                        SubscriptionPeriod = c.Int(nullable: false),
                        UserPremiumPostCount = c.Int(nullable: false),
                        UserFreePostCount = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserPackageID)
                .ForeignKey("dbo.PackageConfigs", t => t.PackageID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.PackageID);
            
            CreateTable(
                "dbo.PackageConfigs",
                c => new
                    {
                        PackageConfigID = c.Int(nullable: false, identity: true),
                        PackageName = c.String(),
                        Descriptinon = c.String(),
                        PackageTotalAllowedPost = c.Int(nullable: false),
                        TotalFreePost = c.Int(nullable: false),
                        TotalPremiumPost = c.Int(nullable: false),
                        PackageType = c.Int(nullable: false),
                        PackageStatus = c.Int(nullable: false),
                        PackagePrice = c.Double(nullable: false),
                        Discount = c.Int(nullable: false),
                        SubscriptionPeriod = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PackageConfigID);
            
            CreateTable(
                "dbo.UserOrderDetails",
                c => new
                    {
                        OrderDetailID = c.Int(nullable: false, identity: true),
                        UserOrderID = c.Long(),
                        PackageConfigID = c.Int(),
                        TotalAllowedPost = c.Int(nullable: false),
                        TotalFreePost = c.Int(nullable: false),
                        TotalPremiumPost = c.Int(nullable: false),
                        PackagePrice = c.Double(nullable: false),
                        PackageDiscount = c.Int(nullable: false),
                        PackageType = c.Int(nullable: false),
                        PackageStatus = c.Int(nullable: false),
                        SubscriptionPeriod = c.Int(nullable: false),
                        ItemBillAomunt = c.Double(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderDetailID)
                .ForeignKey("dbo.PackageConfigs", t => t.PackageConfigID)
                .ForeignKey("dbo.UserOrders", t => t.UserOrderID)
                .Index(t => t.UserOrderID)
                .Index(t => t.PackageConfigID);
            
            CreateTable(
                "dbo.UserOrders",
                c => new
                    {
                        UserOrderID = c.Long(nullable: false, identity: true),
                        UserID = c.Long(),
                        OrderDate = c.DateTime(nullable: false),
                        TotalBill = c.Double(),
                        OrderStatus = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserOrderID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.PriceConfigs",
                c => new
                    {
                        PostPriceConfigID = c.Int(nullable: false, identity: true),
                        OfferName = c.String(),
                        ConfigurationCountry = c.Int(),
                        CountryCurrency = c.Int(),
                        SubCategoryID = c.Long(),
                        OfferType = c.Int(nullable: false),
                        OfferPrice = c.Double(),
                        OfferDiscount = c.Int(),
                        OfferFreePost = c.Int(),
                        PackageConfigID = c.Int(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostPriceConfigID)
                .ForeignKey("dbo.PackageConfigs", t => t.PackageConfigID)
                .Index(t => t.PackageConfigID);
            
            CreateTable(
                "dbo.OrderBillItems",
                c => new
                    {
                        OrderBillItemID = c.Long(nullable: false, identity: true),
                        CompanyID = c.Long(nullable: false),
                        OrderBillID = c.Long(nullable: false),
                        ProductID = c.Long(),
                        ProductName = c.String(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitDiscountedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalUnits = c.Int(nullable: false),
                        TotalUnitsPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalGatewayShareAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalDeshiShareAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalCompanyShareAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillPaid = c.Boolean(nullable: false),
                        BillPaymentDate = c.DateTime(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                        User_UserID = c.Long(),
                    })
                .PrimaryKey(t => t.OrderBillItemID)
                .ForeignKey("dbo.Companies", t => t.CompanyID, cascadeDelete: true)
                .ForeignKey("dbo.OrderBills", t => t.OrderBillID, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.ProductID)
                .ForeignKey("dbo.Users", t => t.User_UserID)
                .Index(t => t.CompanyID)
                .Index(t => t.OrderBillID)
                .Index(t => t.ProductID)
                .Index(t => t.User_UserID);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyID = c.Long(nullable: false, identity: true),
                        Agreement = c.Boolean(nullable: false),
                        UserID = c.Long(),
                        CompanyName = c.String(),
                        CompanyWebsite = c.String(),
                        CompanyPhone = c.String(),
                        CompanyEmail = c.String(),
                        ShopContactEmail = c.String(),
                        ShopContactName = c.String(),
                        ShopContactPhoneNumber = c.String(),
                        Remarks = c.String(),
                        CompanyAddressID = c.Long(),
                        ShopAddressID = c.Long(),
                        BankName = c.String(),
                        BranchName = c.String(),
                        AccountHolderName = c.String(),
                        AccountNumber = c.String(),
                        ACHolderContactNumber = c.String(),
                        BkashAccountNumber = c.String(),
                        OwnerName = c.String(),
                        OwnerPhoneNumber = c.String(),
                        OwnerEmail = c.String(),
                        OwnerNIDFile = c.Binary(),
                        TradeLicenseFile = c.Binary(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyID)
                .ForeignKey("dbo.CompanyAddresses", t => t.CompanyAddressID)
                .ForeignKey("dbo.CompanyAddresses", t => t.ShopAddressID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.CompanyAddressID)
                .Index(t => t.ShopAddressID);
            
            CreateTable(
                "dbo.CompanyAddresses",
                c => new
                    {
                        CompanyAddressID = c.Long(nullable: false, identity: true),
                        StateID = c.Int(nullable: false),
                        City = c.String(),
                        Area = c.String(),
                        ZipCode = c.String(),
                        HouseNo = c.String(),
                        RoadNo = c.String(),
                        Block = c.String(),
                        ApartmentNo = c.String(),
                        AddressDetails = c.String(),
                        LandMark = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyAddressID);
            
            CreateTable(
                "dbo.OrderBills",
                c => new
                    {
                        BillID = c.Long(nullable: false, identity: true),
                        PurchaseOrderID = c.Long(nullable: false),
                        BillingDate = c.DateTime(nullable: false),
                        PaymentMethod = c.Int(nullable: false),
                        BillPaid = c.Boolean(nullable: false),
                        BillPaidDate = c.DateTime(),
                        TotalPayableAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransportPayableAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippingAddressID = c.Long(nullable: false),
                        BkashTransactionNumber = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BillID)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrderID, cascadeDelete: true)
                .ForeignKey("dbo.ShippingAddresses", t => t.ShippingAddressID, cascadeDelete: true)
                .Index(t => t.PurchaseOrderID)
                .Index(t => t.ShippingAddressID);
            
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        PurchaseOrderID = c.Long(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        OrderTotalPaymentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderConfirmed = c.Boolean(nullable: false),
                        OrderDelivered = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseOrderID);
            
            CreateTable(
                "dbo.PurchaseOrderItems",
                c => new
                    {
                        PurchaseOrderDetailID = c.Long(nullable: false, identity: true),
                        CompanyID = c.Long(nullable: false),
                        OrderID = c.Long(nullable: false),
                        ProductID = c.Long(),
                        ProductName = c.String(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitDiscountedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalUnits = c.Int(nullable: false),
                        TotalUnitsPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseOrderDetailID)
                .ForeignKey("dbo.Companies", t => t.CompanyID, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.ProductID)
                .ForeignKey("dbo.PurchaseOrders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.CompanyID)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.ShippingAddresses",
                c => new
                    {
                        ShippingAddressID = c.Long(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false),
                        CustomerEmail = c.String(nullable: false),
                        CustomerPhone = c.String(nullable: false),
                        StateID = c.Int(nullable: false),
                        City = c.String(nullable: false),
                        Area = c.String(nullable: false),
                        ZipCode = c.String(nullable: false),
                        HouseNo = c.String(),
                        RoadNo = c.String(),
                        Block = c.String(),
                        ApartmentNo = c.String(),
                        AddressDetails = c.String(),
                        LandMark = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ShippingAddressID);
            
            CreateTable(
                "dbo.UserCreditOrders",
                c => new
                    {
                        UserCreditOrderID = c.Long(nullable: false, identity: true),
                        UserID = c.Long(),
                        OrderDate = c.DateTime(nullable: false),
                        BillAmount = c.Double(),
                        OrderStatus = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserCreditOrderID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.DonateBikashBillTransactons",
                c => new
                    {
                        DonateBikashBillId = c.Long(nullable: false, identity: true),
                        TransactionNumber = c.String(),
                        AgentNumber = c.String(),
                        PaidAmount = c.Double(nullable: false),
                        EntryDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DonateBikashBillId);
            
            CreateTable(
                "dbo.LogBrowserInfoes",
                c => new
                    {
                        BrowserLogID = c.Long(nullable: false, identity: true),
                        Width = c.String(),
                        Height = c.String(),
                        Country = c.String(),
                        Zip = c.String(),
                        Region = c.String(),
                        City = c.String(),
                        LogDateTime = c.DateTime(nullable: false),
                        Lon = c.String(),
                        Lat = c.String(),
                        CountryCode = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BrowserLogID);
            
            CreateTable(
                "dbo.LogPostActions",
                c => new
                    {
                        LogID = c.Long(nullable: false, identity: true),
                        SessionNumber = c.String(),
                        PostID = c.Long(nullable: false),
                        PostTitle = c.String(),
                        CategoryID = c.Long(),
                        SubCategoryID = c.Long(),
                        LogType = c.Int(nullable: false),
                        DisplayLogType = c.String(),
                        SearchKey = c.String(),
                        CatMarketSubCategoryID = c.Long(nullable: false),
                        LogDateTime = c.DateTime(nullable: false),
                        PriceLow = c.Long(),
                        PriceHigh = c.Long(),
                        StateID = c.Long(),
                        AreaDescription = c.String(),
                        IsNew = c.Boolean(nullable: false),
                        IsUsed = c.Boolean(nullable: false),
                        IsUrgent = c.Boolean(nullable: false),
                        IsForSell = c.Boolean(nullable: false),
                        IsForRent = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LogID);
            
            CreateTable(
                "dbo.UserMessages",
                c => new
                    {
                        MessageID = c.Long(nullable: false, identity: true),
                        SenderUserID = c.Long(nullable: false),
                        ReceiverUserID = c.Long(nullable: false),
                        Msg = c.String(),
                        ParentMessageID = c.Long(nullable: false),
                        IsNewMessage = c.Boolean(nullable: false),
                        Key = c.Binary(),
                        IV = c.Binary(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MessageID);
            
            CreateTable(
                "dbo.OrderDeliveries",
                c => new
                    {
                        OrderDeliveryID = c.Long(nullable: false, identity: true),
                        PurchaseOrderID = c.Long(),
                        OrderItemID = c.Long(),
                        OrderBillID = c.Long(),
                        BillItemID = c.Long(),
                        ShippingAddressID = c.Long(nullable: false),
                        ShopAddressID = c.Long(),
                        ProductID = c.Long(nullable: false),
                        BillPayMethod = c.Int(nullable: false),
                        BillPaid = c.Boolean(nullable: false),
                        ItemCollectionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryPerson = c.String(),
                        DeliveryDate = c.DateTime(nullable: false),
                        DeliveryLastDate = c.DateTime(nullable: false),
                        BillCollected = c.Boolean(nullable: false),
                        ProductDelivered = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderDeliveryID)
                .ForeignKey("dbo.OrderBillItems", t => t.BillItemID)
                .ForeignKey("dbo.OrderBills", t => t.OrderBillID)
                .ForeignKey("dbo.PurchaseOrderItems", t => t.OrderItemID)
                .ForeignKey("dbo.Posts", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrderID)
                .ForeignKey("dbo.ShippingAddresses", t => t.ShippingAddressID, cascadeDelete: true)
                .ForeignKey("dbo.CompanyAddresses", t => t.ShopAddressID)
                .Index(t => t.PurchaseOrderID)
                .Index(t => t.OrderItemID)
                .Index(t => t.OrderBillID)
                .Index(t => t.BillItemID)
                .Index(t => t.ShippingAddressID)
                .Index(t => t.ShopAddressID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.LogPostVisits",
                c => new
                    {
                        PostVisitLogID = c.Long(nullable: false, identity: true),
                        VisitorEmail = c.String(),
                        VisitorPhoneNumber = c.String(),
                        PostID = c.Long(nullable: false),
                        PostTitle = c.String(),
                        CategoryName = c.String(),
                        SubCategoryName = c.String(),
                        PostItemPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StateName = c.String(),
                        Area = c.String(),
                        PosterName = c.String(),
                        PosterPhoneNumber = c.String(),
                        LogDateTime = c.DateTime(nullable: false),
                        AdvertiserUserID = c.Long(nullable: false),
                        AdvertiserAccountEmail = c.String(),
                        AdvertiserAccountClientName = c.String(),
                        PostImageFile1 = c.Binary(),
                        PostImageFile2 = c.Binary(),
                        PostImageFile3 = c.Binary(),
                        PostImageFile4 = c.Binary(),
                        PostVisitAction = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostVisitLogID);
            
            CreateTable(
                "dbo.LogServerErrors",
                c => new
                    {
                        ServerErrorLogID = c.Long(nullable: false, identity: true),
                        ErrorLogTime = c.DateTime(nullable: false),
                        Message = c.String(),
                        Source = c.String(),
                        MethodName = c.String(),
                        InnerExceptionMessage = c.String(),
                        Action = c.String(),
                        Controller = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ServerErrorLogID);
            
            CreateTable(
                "dbo.UserAccountBillTransactions",
                c => new
                    {
                        BillId = c.Long(nullable: false, identity: true),
                        PaidAmount = c.Double(nullable: false),
                        EntryDateTime = c.DateTime(nullable: false),
                        TransactionApprovalStatus = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BillId);
            
            CreateTable(
                "dbo.UserPackageHistories",
                c => new
                    {
                        UserPacHistoryId = c.Long(nullable: false, identity: true),
                        PackageId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserPacHistoryId);
            
            CreateTable(
                "dbo.LogUserSessions",
                c => new
                    {
                        UserSessionId = c.Int(nullable: false, identity: true),
                        ActiveUrl = c.String(),
                        ElementId = c.String(),
                        ElementClass = c.String(),
                        TargetUrl = c.String(),
                        ElementTagName = c.String(),
                        BrowserWidth = c.String(),
                        BrowserHeight = c.String(),
                        BrowserLogId = c.Long(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserSessionId);
            
            CreateTable(
                "dbo.LogMousePositions",
                c => new
                    {
                        MousePositionId = c.Int(nullable: false, identity: true),
                        Position = c.String(),
                        UserSessionId = c.Int(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EnumCountry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MousePositionId)
                .ForeignKey("dbo.LogUserSessions", t => t.UserSessionId)
                .Index(t => t.UserSessionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogMousePositions", "UserSessionId", "dbo.LogUserSessions");
            DropForeignKey("dbo.OrderDeliveries", "ShopAddressID", "dbo.CompanyAddresses");
            DropForeignKey("dbo.OrderDeliveries", "ShippingAddressID", "dbo.ShippingAddresses");
            DropForeignKey("dbo.OrderDeliveries", "PurchaseOrderID", "dbo.PurchaseOrders");
            DropForeignKey("dbo.OrderDeliveries", "ProductID", "dbo.Posts");
            DropForeignKey("dbo.OrderDeliveries", "OrderItemID", "dbo.PurchaseOrderItems");
            DropForeignKey("dbo.OrderDeliveries", "OrderBillID", "dbo.OrderBills");
            DropForeignKey("dbo.OrderDeliveries", "BillItemID", "dbo.OrderBillItems");
            DropForeignKey("dbo.BikashBillTransactons", "UserOrderID", "dbo.UserOrders");
            DropForeignKey("dbo.BikashBillTransactons", "UserCreditOrderID", "dbo.UserCreditOrders");
            DropForeignKey("dbo.UserCreditOrders", "UserID", "dbo.Users");
            DropForeignKey("dbo.OrderBillItems", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.OrderBillItems", "ProductID", "dbo.Posts");
            DropForeignKey("dbo.OrderBills", "ShippingAddressID", "dbo.ShippingAddresses");
            DropForeignKey("dbo.OrderBills", "PurchaseOrderID", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrderItems", "OrderID", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrderItems", "ProductID", "dbo.Posts");
            DropForeignKey("dbo.PurchaseOrderItems", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.OrderBillItems", "OrderBillID", "dbo.OrderBills");
            DropForeignKey("dbo.Companies", "UserID", "dbo.Users");
            DropForeignKey("dbo.Companies", "ShopAddressID", "dbo.CompanyAddresses");
            DropForeignKey("dbo.OrderBillItems", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.Companies", "CompanyAddressID", "dbo.CompanyAddresses");
            DropForeignKey("dbo.Posts", "UserPackageID", "dbo.UserPackages");
            DropForeignKey("dbo.UserPackages", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserPackages", "PackageID", "dbo.PackageConfigs");
            DropForeignKey("dbo.Users", "PackageConfig_PackageConfigID", "dbo.PackageConfigs");
            DropForeignKey("dbo.PriceConfigs", "PackageConfigID", "dbo.PackageConfigs");
            DropForeignKey("dbo.UserOrders", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserOrderDetails", "UserOrderID", "dbo.UserOrders");
            DropForeignKey("dbo.UserOrderDetails", "PackageConfigID", "dbo.PackageConfigs");
            DropForeignKey("dbo.Posts", "UserID", "dbo.Users");
            DropForeignKey("dbo.PostServices", "PostID", "dbo.Posts");
            DropForeignKey("dbo.PostProcesses", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Providers", "UserID", "dbo.Users");
            DropForeignKey("dbo.Providers", "FabiaServiceID", "dbo.Posts");
            DropForeignKey("dbo.PostComments", "PostID", "dbo.Posts");
            DropForeignKey("dbo.GroupPanelPosts", "FileID", "dbo.Files");
            DropForeignKey("dbo.Files", "PostID", "dbo.Posts");
            DropForeignKey("dbo.GroupPanelPosts", "PostID", "dbo.Posts");
            DropForeignKey("dbo.GroupPanelConfigs", "PanelConfigUserID", "dbo.Users");
            DropForeignKey("dbo.GroupPanelPosts", "GroupPanelConfigID", "dbo.GroupPanelConfigs");
            DropForeignKey("dbo.Posts", "AddressID", "dbo.PostAddresses");
            DropForeignKey("dbo.BikashBillTransactons", "UserId", "dbo.Users");
            DropIndex("dbo.LogMousePositions", new[] { "UserSessionId" });
            DropIndex("dbo.OrderDeliveries", new[] { "ProductID" });
            DropIndex("dbo.OrderDeliveries", new[] { "ShopAddressID" });
            DropIndex("dbo.OrderDeliveries", new[] { "ShippingAddressID" });
            DropIndex("dbo.OrderDeliveries", new[] { "BillItemID" });
            DropIndex("dbo.OrderDeliveries", new[] { "OrderBillID" });
            DropIndex("dbo.OrderDeliveries", new[] { "OrderItemID" });
            DropIndex("dbo.OrderDeliveries", new[] { "PurchaseOrderID" });
            DropIndex("dbo.UserCreditOrders", new[] { "UserID" });
            DropIndex("dbo.PurchaseOrderItems", new[] { "ProductID" });
            DropIndex("dbo.PurchaseOrderItems", new[] { "OrderID" });
            DropIndex("dbo.PurchaseOrderItems", new[] { "CompanyID" });
            DropIndex("dbo.OrderBills", new[] { "ShippingAddressID" });
            DropIndex("dbo.OrderBills", new[] { "PurchaseOrderID" });
            DropIndex("dbo.Companies", new[] { "ShopAddressID" });
            DropIndex("dbo.Companies", new[] { "CompanyAddressID" });
            DropIndex("dbo.Companies", new[] { "UserID" });
            DropIndex("dbo.OrderBillItems", new[] { "User_UserID" });
            DropIndex("dbo.OrderBillItems", new[] { "ProductID" });
            DropIndex("dbo.OrderBillItems", new[] { "OrderBillID" });
            DropIndex("dbo.OrderBillItems", new[] { "CompanyID" });
            DropIndex("dbo.PriceConfigs", new[] { "PackageConfigID" });
            DropIndex("dbo.UserOrders", new[] { "UserID" });
            DropIndex("dbo.UserOrderDetails", new[] { "PackageConfigID" });
            DropIndex("dbo.UserOrderDetails", new[] { "UserOrderID" });
            DropIndex("dbo.UserPackages", new[] { "PackageID" });
            DropIndex("dbo.UserPackages", new[] { "UserID" });
            DropIndex("dbo.PostServices", new[] { "PostID" });
            DropIndex("dbo.PostProcesses", new[] { "PostID" });
            DropIndex("dbo.Providers", new[] { "UserID" });
            DropIndex("dbo.Providers", new[] { "FabiaServiceID" });
            DropIndex("dbo.PostComments", new[] { "PostID" });
            DropIndex("dbo.Files", new[] { "PostID" });
            DropIndex("dbo.GroupPanelConfigs", new[] { "PanelConfigUserID" });
            DropIndex("dbo.GroupPanelPosts", new[] { "FileID" });
            DropIndex("dbo.GroupPanelPosts", new[] { "GroupPanelConfigID" });
            DropIndex("dbo.GroupPanelPosts", new[] { "PostID" });
            DropIndex("dbo.Posts", new[] { "UserPackageID" });
            DropIndex("dbo.Posts", new[] { "AddressID" });
            DropIndex("dbo.Posts", new[] { "UserID" });
            DropIndex("dbo.Users", new[] { "PackageConfig_PackageConfigID" });
            DropIndex("dbo.BikashBillTransactons", new[] { "UserCreditOrderID" });
            DropIndex("dbo.BikashBillTransactons", new[] { "UserOrderID" });
            DropIndex("dbo.BikashBillTransactons", new[] { "UserId" });
            DropTable("dbo.LogMousePositions");
            DropTable("dbo.LogUserSessions");
            DropTable("dbo.UserPackageHistories");
            DropTable("dbo.UserAccountBillTransactions");
            DropTable("dbo.LogServerErrors");
            DropTable("dbo.LogPostVisits");
            DropTable("dbo.OrderDeliveries");
            DropTable("dbo.UserMessages");
            DropTable("dbo.LogPostActions");
            DropTable("dbo.LogBrowserInfoes");
            DropTable("dbo.DonateBikashBillTransactons");
            DropTable("dbo.UserCreditOrders");
            DropTable("dbo.ShippingAddresses");
            DropTable("dbo.PurchaseOrderItems");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.OrderBills");
            DropTable("dbo.CompanyAddresses");
            DropTable("dbo.Companies");
            DropTable("dbo.OrderBillItems");
            DropTable("dbo.PriceConfigs");
            DropTable("dbo.UserOrders");
            DropTable("dbo.UserOrderDetails");
            DropTable("dbo.PackageConfigs");
            DropTable("dbo.UserPackages");
            DropTable("dbo.PostServices");
            DropTable("dbo.PostProcesses");
            DropTable("dbo.Providers");
            DropTable("dbo.PostComments");
            DropTable("dbo.Files");
            DropTable("dbo.GroupPanelConfigs");
            DropTable("dbo.GroupPanelPosts");
            DropTable("dbo.Posts");
            DropTable("dbo.Users");
            DropTable("dbo.BikashBillTransactons");
            DropTable("dbo.AValues");
            DropTable("dbo.PostAddresses");
        }
    }
}
