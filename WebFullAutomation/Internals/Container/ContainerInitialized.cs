using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.Logger;
using AirSoftAutomationFramework.Internals.DAL.MongoDb;
using AirSoftAutomationFramework.Internals.DAL.Sql;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.DocumentsAndFilesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.sales;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Profile;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.RolesPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.UsersMenuUi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Map;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.QAndA;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using AirSoftAutomationFramework.Objects.MgmObjects.Api;
using AirSoftAutomationFramework.Objects.MgmObjects.Api.QAndA;
using AirSoftAutomationFramework.Objects.MgmObjects.Api.Risk.AssetsCfd;
using AirSoftAutomationFramework.Objects.MgmObjects.Ui.Dashboard;
using Autofac;
using OpenQA.Selenium;
using WebFullAutomation.Internals.DAL.ApiAccsess;

namespace WebFullAutomation.Internals.Container
{
    public class ContainerInitialized
    {
        public IContainer ContainerConfigure(IWebDriver driver)
        {
            var builder = new ContainerBuilder();

            #region General
            builder.RegisterType<ApplicationFactory>().As<IApplicationFactory>();
            builder.RegisterType<WriteToFile>().As<IWriteToFile>();
            builder.RegisterType<Log4Net>().As<ILog4Net>();
            builder.RegisterType<MongoDbAccess>().As<IMongoDbAccess>();
            builder.RegisterType<WriteToFile>().As<IWriteToFile>();

            builder.RegisterType<General>().As<IGeneral>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<SharedStepsGenerator>().As<ISharedStepsGenerator>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<FileHandler>().As<IFileHandler>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<SalesTabApi>().As<ISalesTabApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<DashboardApi>().As<IDashboardApi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<GlobalEventsApi>().As<IGlobalEventsApi>()
            .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<HandleFiltersUi>().As<IHandleFiltersUi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<CustomLogger>().As<ICustomLogger>()
             .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<SearchResultsUi>().As<ISearchResultsUi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<SearchResultsFactory>().As<ISearchResultsFactory>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<TabsFactory>().As<ITabsFactory>()
                 .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ApiAccess>().As<IApiAccess>();
            builder.RegisterType<SqlDbAccess>().As<ISqlDbAccess>();

            builder.RegisterType<Menus>().As<IMenus>()
                //.UsingConstructor(typeof(IWebDriver))
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });
            #endregion

            #region Mgm
            builder.RegisterType<BroadcastMessageApi>().As<IBroadcastMessageApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<MgmDashboardApi>().As<IMgmDashboardApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<AssetsApi>().As<IAssetsApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<BroadcastMessageUi>().As<IBroadcastMessageUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<QEndAApi>().As<IQEndAApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<QAndAPageUi>().As<IQAndAPageUi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });
            #endregion

            #region Crm 
            builder.RegisterType<SavingAccountsTabUI>().As<ISavingAccountsTabUI>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<BulkTradePageUi>().As<IBulkTradePageUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<BulkTradePageApi>().As<IBulkTradePageApi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<AttributionRulePageUi>().As<IAttributionRulePageUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<FinanceFactoryApi>().As<IFinanceFactoryApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<RiskPageUi>().As<IRiskPageUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<PlanningPageUi>().As<IPlanningPageUi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<SecurityTubApi>().As<ISecurityTubApi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<SuperAdminTabUi>().As<ISuperAdminTabUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<AssetsOnFrontPageApi>().As<IAssetsOnFrontPageApi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<MarketExposurePageApi>().As<IMarketExposurePageApi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<OpenTradesPageApi>().As<IOpenTradesPageApi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<PendingTradesPageApi>().As<IPendingTradesPageApi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ClosedTradesPageApi>().As<IClosedTradesPageApi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<HourlyPnlPageApi>().As<IHourlyPnlPageApi>()
            .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<CampaignsMapPageUi>().As<ICampaignsMapPageUi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<HandleFiltersApi>().As<IHandleFiltersApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ChatTabApi>().As<IChatTabApi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ChatUi>().As<IChatUi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<TradeGroupsUi>().As<ITradeGroupsUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<TradeGroupCardUi>().As<ITradeGroupCardUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<SalesPageApi>().As<ISalesPageApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<LoginApi>().As<ILoginApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<DocumentsAndFilesTabApi>().As<IDocumentsAndFilesTabApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<DocumentsTabUI>().As<IDocumentsTabUI>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<GeneralTabUi>().As<IGeneralTabUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<SecurityTabUi>().As<ISecurityTabUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<CampaignsPageApi>().As<ICampaignsPageApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<WithdrawalsPageUi>().As<IWithdrawalsPageUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ChargebacksPageUi>().As<IChargebacksPageUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<BonusesPageUi>().As<IBonusesPageUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<TimelineTabUi>().As<ITimelineTabUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<BannerTabApi>().As<IBannerTabApi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<TradesTabApi>().As<ITradesTabApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<FinanceTabUi>().As<IFinanceTabUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<TradeTabUi>().As<ITradeTabUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ProfilePageUi>().As<IProfilePageUi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<RiskPageApi>().As<IRiskPageApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<BonusPageApi>().As<IBonusPageApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<WithdrawalsPageApi>().As<IWithdrawalsPageApi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<SalesPageUi>().As<ISalesPageUi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ChargeBacksPageApi>().As<IChargebacksPageApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<DepositsPageApi>().As<IDepositsPageApi>()
             .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<SATabApi>().As<ISATabApi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<OpenTradesPageUi>().As<IOpenTradesPageUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<TradeGroupApi>().As<ITradeGroupApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ClosedTradesPageUi>().As<IClosedTradesPageUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<PendingTradesPageUi>().As<IPendingTradesPageUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<FinancesTabApi>().As<IFinancesTabApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<InformationTabUi>().As<IInformationTabUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<BonusUi>().As<IBonusUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<SearchResultsUi>().As<ISearchResultsUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<AffiliatesRequestsApi>().As<IAffiliatesRequestsApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<UserUi>().As<IUserUi>()
             //.UsingConstructor(typeof(IWebDriver))
             .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<DepositUi>().As<IDepositUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<FinanceTabUi>().As<IFinanceTabUi>()
                  .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<DepositsPageUi>().As<IDepositsPageUi>()
                //.UsingConstructor(typeof(IWebDriver))
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });
            #endregion

            #region Trade
            builder.RegisterType<WithdrawalTpApi>().As<IWithdrawalTpApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<HistoryPageApi>().As<IHistoryPageApi>()
             .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });         

            builder.RegisterType<NftPageUi>().As<INftPageUi>()
            .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<NftPageApi>().As<INftPageApi>()
            .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<SavingAccountTpApi>().As<ISavingAccountTpApi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<MarketExposurePageUi>().As<IMarketExposurePageUi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ChronoPageUi>().As<IChronoPageUi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ChronoTradePageApi>().As<IChronoTradePageApi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ProfilePageApi>().As<IProfilePageApi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<TradeDepositPageApi>().As<ITradeDepositPageApi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<TradePageApi>().As<ITradePageApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<DeclarationOfDepositUi>().As<IDeclarationOfDepositUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<HistoryPageUi>().As<IHistoryPageUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<TradePageUi>().As<ITradePageUi>()
                //.UsingConstructor(typeof(IWebDriver))
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });
            #endregion

            #region Mgm
            builder.RegisterType<AssetsGroupApi>().As<IAssetsGroupApi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<AssetsCfdApi>().As<IAssetsCfdApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<MgmCreateUserApi>().As<IMgmCreateUserApi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });
            #endregion

            #region Shared
            builder.RegisterType<UsersPageUi>().As<IUsersPageUi>()
              //.UsingConstructor(typeof(IWebDriver))
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<InformationTabApi>().As<IInformationTabApi>()
              //.UsingConstructor(typeof(IWebDriver))
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<TimeLineTabApi>().As<ITimeLineTabApi>()
              //.UsingConstructor(typeof(IWebDriver))
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<CommentsTabApi>().As<ICommentsTabApi>()
             //.UsingConstructor(typeof(IWebDriver))
             .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<TimelineTabUi>().As<ITimelineTabUi>()
              //.UsingConstructor(typeof(IWebDriver))
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<CreateEditRoleApi>().As<ICreateEditRoleApi>()
             //.UsingConstructor(typeof(IWebDriver))
             .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ChronoTabApi>().As<IChronoTabApi>()
           //.UsingConstructor(typeof(IWebDriver))
           .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<OfficeTabApi>().As<IOfficeTabApi>()
                //.UsingConstructor(typeof(IWebDriver))
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<PlatformTabApi>().As<IPlatformTabApi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<LanguagesTab>().As<ILanguagesTab>()
                //.UsingConstructor(typeof(IWebDriver))
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<SuperAdminTubApi>().As<ISuperAdminTubApi>()
              //.UsingConstructor(typeof(IWebDriver))
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<AgentProfileUi>().As<IAgentProfileUi>()
            //.UsingConstructor(typeof(IWebDriver))
            .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<PspTabApi>().As<IPspTabApi>()
             //.UsingConstructor(typeof(IWebDriver))
             .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<RolesApi>().As<IRolesApi>()
            //.UsingConstructor(typeof(IWebDriver))
            .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<GeneralTabApi>().As<IGeneralTabApi>()
           //.UsingConstructor(typeof(IWebDriver))
           .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<NotificationsApi>().As<INotificationsApi>()
            //.UsingConstructor(typeof(IWebDriver))
            .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<PlanningTabApi>().As<IPlanningTabApi>()
             //.UsingConstructor(typeof(IWebDriver))
             .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ClientCardApi>().As<IClientCardApi>()
             //.UsingConstructor(typeof(IWebDriver))
             .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<UsersApi>().As<IUsersApi>()
                //.UsingConstructor(typeof(IWebDriver))
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<UserApi>().As<IUserApi>()
            //.UsingConstructor(typeof(IWebDriver))
            .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<CreateClientApi>().As<ICreateClientApi>()
                //.UsingConstructor(typeof(IWebDriver))
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<CreateClientUi>().As<ICreateClientUi>()
                //.UsingConstructor(typeof(IWebDriver))
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ClientCardUi>().As<IClientCardUi>()
               //.UsingConstructor(typeof(IWebDriver))
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<RoleUi>().As<IRoleUi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<RolesPageUi>().As<IRolesPageUi>()
                .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<DashboardPageUi>().As<IDashboardPageUi>()
                  .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<LoginPageUi>().As<ILoginPageUi>()
              //.UsingConstructor(typeof(IWebDriver))
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<CommentsTabUi>().As<ICommentsTabUi>()
               //.UsingConstructor(typeof(IWebDriver))
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ClientsPageUi>().As<IClientsPageUi>()
               //.UsingConstructor(typeof(IWebDriver))
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<CampaignsPageUi>().As<ICampaignsPageUi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<ClientsApi>().As<IClientsApi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<CampaignPageApi>().As<ICampaignPageApi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<CreateAffiliateApi>().As<ICreateAffiliateApi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<CreateAffiliateUi>().As<ICreateAffiliateUi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });

            builder.RegisterType<CreateCampaignUi>().As<ICreateCampaignUi>()
              .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });
            #endregion

            #region Dev mgm
            builder.RegisterType<MgmDashboardUi>().As<IMgmDashboardUi>()
               .WithParameters(new[] { new TypedParameter(typeof(IWebDriver), driver) });
            #endregion
            
            #region Register Kafka

            // add kafka
            services.AddKafka(
                kafka => kafka
                    .UseConsoleLog()
                    .AddCluster(
                        cluster => cluster
                            .WithBrokers(Configs.BootstrapServers.Split(";"))
                            .WithSecurityInformation(ha => ha.SecurityProtocol = SecurityProtocol.Ssl)
                            .AddConsumer(consumer =>
                            {
                                consumer
                                    .Topic(Configs.TopicConsumer)
                                    .WithAutoOffsetReset(AutoOffsetReset.Latest)
                                    .WithGroupId(Configs.GroupId)
                                    .WithWorkersCount(10)
                                    .WithBufferSize(10)
                                    .AddMiddlewares(middlewares => middlewares
                                        .AddSingleTypeDeserializer<ProviderEventDto, KafkaFlowUtf8JsonSerializer>()
                                        .AddTypedHandlers(handler => handler
                                            .AddHandler<KafkaMessageHandler>()));
                            })
                            .AddProducer<KafkaFlowProducer>(producer => producer
                                .DefaultTopic(Configs.TopicProducer)
                                .AddMiddlewares(middlewares => middlewares
                                    .AddSerializer<JsonCoreSerializer>()
                                ))));

            #endregion

            var container = builder.Build();

            return container;
        }
    }
}
