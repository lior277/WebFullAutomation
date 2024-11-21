// Ignore Spelling: Crm

using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.CrmTrade.OpenTradePage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyCloseTradeButtonNotExistForNoPermission : TestSuitBase
    {
        #region Test Preparation
        public VerifyCloseTradeButtonNotExistForNoPermission(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private string _roleName;
        private string _clientEmail;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
            _roleName = TextManipulation.RandomString();
            _driver = GetDriver();
            var tradeAmount = 2;
            var bonusAmount = 10000;
            var assetName = DataRep.AssetName;

            var updatePermissions = new Dictionary<List<string>, string>
            { { new List<string>() { "all_user_trades" }, "remove" } };

            // create admin View Trades Only role 
            _apiFactory
                .ChangeContext<IRolesApi>(_driver)
                .PostCreateCostomRoleRequest(_crmUrl, _roleName,
                permissionsToUpdate: updatePermissions);

            // create user
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName, role:
                _roleName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName, _crmUrl);

            // get user apiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: currentUserApiKey);

            var clientsIds = new List<string> { clientId };

            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostBonusRequest(_crmUrl, clientId, bonusAmount);

            // get login Data for trading Platform
            var loginData = _apiFactory
                 .ChangeContext<ILoginApi>(_driver)
                 .PostLoginToTradingPlatform(tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            // connect One User To 1 Client
            _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .PatchMassAssignSaleAgentsRequest(_crmUrl, userId, clientsIds);

            _apiFactory
                 .ChangeContext<ITradePageApi>(_driver)
                 .PostBuyAssetRequest(tradingPlatformUrl, tradeAmount,
                  loginData: loginData, assetSymble: assetName);
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            try
            {
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }

        // create user
        // create roll with "User trades" view only
        // navigage to clients 
        // search the client by id
        // click on full name
        // click on trade tab
        // verify "close" button not exist
        // navigate to cfd -> opent trade table
        // verify "close" button not exist
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyCloseTradeButtonNotExistForNoPermissionTest()
        {
            // cfd open trades table
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/groups/trades/open")
                .ChangeContext<IOpenTradesPageUi>(_driver)
                .ClickOnClientName()
                .ClickOnTradeTab()
                .ChangeContext<IOpenTradesPageUi>(_driver)
                .VerifyCloseTradeButtonNotExist();
        }
    }
}
