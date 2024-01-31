// Ignore Spelling: Crm

using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
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

namespace TestsProject.Tests.CrmTrade.RiskPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyRiskTable : TestSuitBase
    {
        #region Test Preparation
        public VerifyRiskTable(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;
        private string _clientId;
        private string _userName;
        private int _tradeAmount = 2;
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var bonusAmount = 10000;
            _driver = GetDriver();

            // create user
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, currency: "CAD",
                apiKey: currentUserApiKey);

            // create bonus
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostBonusRequest(_crmUrl, _clientId, bonusAmount);

            // get login Data for trading Platform
            var loginData = _apiFactory
                 .ChangeContext<ILoginApi>(_driver)
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            // buy asset
            _apiFactory
                 .ChangeContext<ITradePageApi>(_driver)
                 .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount, loginData);
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
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4740")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyRiskTableTest()
        {
            var clientName = _clientEmail.Split("@").First().UpperCaseFirstLetter();
            var date = DateTime.Now.ToString("dd/MM/yyyy");

            var tradeDitails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IRiskPageUi>(DataRep.RiskMenuItem)
                .SearchRisks(clientName)
                .ChangeContext<IRiskPageUi>(_driver)
                .WaitForNumOfElementInRiskTable(1)
                .ChangeContext<ISearchResultsUi>(_driver)
                .GetSearchResultDetails<SearchResultRisk>().First();

            Assert.Multiple(() =>
            {
                Assert.True(tradeDitails.available != null,
                    $" expected available not null" +
                    $" actual available: {tradeDitails.available}");

                Assert.True(tradeDitails.balance == "10,000.00 C$",
                    $" expected balance: 10,000.00 C$, " +
                    $" actual balance: {tradeDitails.balance}");

                Assert.True(tradeDitails.bonus == "10,000.00 C$",
                    $" expected bonus: 10, 000.00 C$, " +
                    $" actual bonus: {tradeDitails.bonus}");

                Assert.True(tradeDitails.fullname == $"{clientName} {clientName}",
                    $" expected full name: {clientName} {clientName}, " +
                    $" actual full name: {tradeDitails.fullname}");

                Assert.True(tradeDitails.pnlclosetrades != null,
                    $" expected pnl close trades: not null, " +
                    $" actual pnl close trades: {tradeDitails.pnlclosetrades}");

                Assert.True(tradeDitails.pnlopentrades != null,
                    $" expected pnl open trades: not null, " +
                    $" actual pnl open trades: {tradeDitails.pnlopentrades}");

                Assert.True(tradeDitails.salesagent.ToLower() == _userName,
                    $" expected sales agent: {_userName}, " +
                    $" actual sales agent: {tradeDitails.salesagent}");

                Assert.True(tradeDitails.swap != null,
                    $" expected swap: not null, " +
                    $" actual swap: {tradeDitails.swap}");

                Assert.True(tradeDitails.volume != null,
                    $" expected  volume: not null, " +
                    $" actual volume: {tradeDitails.volume}");
            });
        }
    }
}