// Ignore Spelling: Unassign Crm

using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.CrmTrade.OpenTradePage.Filter
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyUnassignFilterOnOpenTradeTable : TestSuitBase
    {
        #region Test Preparation
        public VerifyUnassignFilterOnOpenTradeTable(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var bonusAmount = 10000;
            _driver = GetDriver();
            var tradeAmount = 2;

            // create user
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .PatchMassAssignSaleAgentsRequest(_crmUrl, userId,
                new List<string> { _clientId }, apiKey: currentUserApiKey);

            // add bonus
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostBonusRequest(_crmUrl, _clientId, bonusAmount);

            // get login Data 
            var loginData = _apiFactory
                 .ChangeContext<ILoginApi>(_driver)
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                 .GeneralResponse;

            // create trade
            _apiFactory
                 .ChangeContext<ITradePageApi>(_driver)
                 .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, loginData);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);
            #endregion
        }

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
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyUnassignFilterOnOpenTradeTableTest()
        {
            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IHandleFiltersUi>(DataRep.CrmOpenTradesMenuItem)
                .WaitForTableToLoad()
                .ClickToOpenFiltersMenu()
                .UnassignPipe(DataRep.SalesAgentsFilter)
                .ChangeContext<IOpenTradesPageUi>(_driver)
                .SearchOpenTrades(_clientId)
                .VerifyEmptyTable();
        }
    }
}