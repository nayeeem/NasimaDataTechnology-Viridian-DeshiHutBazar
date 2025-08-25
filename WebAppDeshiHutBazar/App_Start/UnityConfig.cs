using System.Web.Mvc;
using Data;
using Unity;
using Microsoft.Practices.Unity;

namespace WebDeshiHutBazar

{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            Microsoft.Practices.Unity.IUnityContainer container = new Microsoft.Practices.Unity.UnityContainer();
            container.RegisterType<IBikashBillTransactionService, BikashBillTransactionService>();
            container.RegisterType <IEmailNotificationService, EmailService> ();
            container.RegisterType <IGroupPanelConfigService, GroupPanelConfigService> ();
            container.RegisterType <IImageProcessingService, ImageProcessingService> ();
            container.RegisterType <ILoggingService, LoggingService> ();
            container.RegisterType <IManageAccountSettingService, ManageAccountSettingService> ();
            container.RegisterType <IPackageConfigurationService, PackageConfigurationService> ();
            container.RegisterType <IPostMangementService, PostManagementService> ();
            container.RegisterType <IPostMappingService, PostMappingService> ();
            container.RegisterType <IPriceConfigurationService, PriceConfigurationService> ();
            container.RegisterType <IUserAccountService, UserAccountService> ();
            container.RegisterType <IUserCreditOrderService, UserCreditOrderService> ();
            container.RegisterType <IUserMessageService, UserMessageService> ();
            container.RegisterType <IUserOrderService, UserOrderService> ();
            container.RegisterType <IUserService, UserService> ();
            container.RegisterType<IBillManagementService, BillManagementService>();
            container.RegisterType<IPaymentOptionService, PaymentOptionService>();
            container.RegisterType<IRepoDropDownDataList, RepoDropDownDataList>();
            container.RegisterType<IPostVisitService, PostVisitService>();
            container.RegisterType<IFabiaProviderService, FabiaProviderServService>();
            container.RegisterType<IReportService, ReportService>();
            container.RegisterType<IAccountBillRepository, UserAccountBillRepository>();
            container.RegisterType <IPostAddressRepository, PostAddressRepository> ();
            container.RegisterType <IAValueRepository, AValueRepository> ();
            container.RegisterType <IBikashBillTransactionRepository, BikashBillTransactionRepository> ();
            container.RegisterType <IGroupPanelConfigRepository, GroupPanelConfigRepository> ();
            container.RegisterType <IGroupPanelPostRepository, GroupPanelPostRepository> ();
            container.RegisterType <ILogBrowserInfoRepository, LogBrowserInfoRepository> ();
            container.RegisterType <ILogServerErrorRepository, LogServerErrorRepository> ();
            container.RegisterType <ILogPostRepository, LogPostRepository> ();
            container.RegisterType <ILogUserSessionRepository, LogUserSessionRepository> ();
            container.RegisterType <IUserMessageRepository, UserMessageRepository> ();
            container.RegisterType <IPackageConfigRepository, PackageConfigRepository> ();
            container.RegisterType <IPostCommentRepository, PostRepository> ();
            container.RegisterType <IPostRepository, PostRepository> ();
            container.RegisterType <IPriceConfigRepository, PriceConfigRepository> ();
            container.RegisterType <IUserAccountRepository, UserAccountRepository> ();
            container.RegisterType <IUserCreditOrderRepository, UserCreditOrderRepository> ();
            container.RegisterType<IUserOrderRepository, UserOrderRepository>();
            container.RegisterType <IUserRepository, UserRepository> ();
            container.RegisterType<IUserPackageRepository, UserPackageRepository>();
            container.RegisterType<ILogPostVisitRepository, LogPostVisitRepository>();
            container.RegisterType<IFabiaProviderRepository, FabiaProviderRepository>();
            container.RegisterType<IPostQueryRepository, PostQueryRepository>();
            container.RegisterType<IShoppingCommand, ShoppingCommand>();
            container.RegisterType<IShoppingCommandService, ShoppingCommandService>();
            container.RegisterType<ICompanyRepository, CompanyRepository>();
            container.RegisterType<ICompanyService, CompanyService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver.Lib.UnityMVCDependencyResolver(container));
        }
    }
}