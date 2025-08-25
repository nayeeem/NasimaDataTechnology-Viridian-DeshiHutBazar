using System.Data.Entity;
namespace Model
{
    public class WebDeshiHutBazarEntityContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        //System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Model.Context.WebDeshiHutBazarEntityContext>());

        public WebDeshiHutBazarEntityContext() : base()
        {           
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<AValue> AValues { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<PostAddress> Addresses { get; set; }

        public DbSet<UserMessage> Messages { get; set; }

        public DbSet<File> Files { get; set; }
       
        public DbSet<LogPostAction> LogOfPosts { get; set; }

        public DbSet<LogBrowserInfo> LogOfBrowsers { get; set; }

        public DbSet<PostComment> PostComments { get; set; }

        public DbSet<PriceConfig> PostPriceConfigurations { get; set; }

        public DbSet<PackageConfig> PostPackageConfigurations { get; set; }

        public DbSet<BikashBillTransacton> BikashBillTransactons { get; set; }

        public DbSet<UserPackageHistory> UserPackageHistorys { get; set; }

        public DbSet<UserAccountBillTransaction> UserAccountBillTransactions { get; set; }

        public DbSet<LogUserSession> UserSessions { get; set; }

        public DbSet<LogServerError> ServerErrorLogs { get; set; }

        public DbSet<LogPostVisit> PostVisits { get; set; }

        public DbSet<UserPackage> UserPackages { get; set; }

        public DbSet<UserOrder> UserOrders { get; set; }

        public DbSet<UserOrderDetail> UserOrderDetails { get; set; }

        public DbSet<UserCreditOrder> UserCreditOrders { get; set; }

        public DbSet<GroupPanelConfig> GroupPanelConfigurations { get; set; }

        public DbSet<GroupPanelPost> GroupPanelPosts { get; set; }

        public DbSet<PostProcess> PostProcesses { get; set; }

        public DbSet<PostService> PostServices { get; set; }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<DonateBikashBillTransacton> DonateBikashBillTransactons { get; set; }

        public DbSet<ShippingAddress> ShippingAddresss { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public DbSet<PurchaseOrderItems> PurchaseOrderItems { get; set; }

        public DbSet<OrderBill> OrderBills { get; set; }

        public DbSet<OrderBillItem> OrderBillItems { get; set; }

        public DbSet<OrderDelivery> OrderDeliveries { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyAddress> CompanyAddresses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {                 
            base.OnModelCreating(modelBuilder); 
        }
    }
}
