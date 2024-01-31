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

namespace TestsProject.Tests.CrmTrade.PendingTradePage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyPendingTradesTable : TestSuitBase
    {
        #region Test Preparation
        public VerifyPendingTradesTable(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;
        private string _clientId;
        private IWebDriver _driver;
        private string _browserName;
        private int _tradeAmount = 2;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var bonusAmount = 10000;
            _driver = GetDriver();
            var assetName = DataRep.AssetName;

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // create bonus
            _apiFactory
                 .ChangeContext<IFinancesTabApi>(_driver)
                 .PostBonusRequest(_crmUrl, _clientId, bonusAmount);

            // get login cookies
            var loginCookies = _apiFactory
                 .ChangeContext<ILoginApi>(_driver)
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            // buy asset
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount,
                loginCookies, assetSymble: assetName)
                .GeneralResponse;

            var currentRate = tradeDetails.TradeRate;
            var RateForPending = (double)(currentRate + 100); // to open a pending trade

            // create pending trade
            _apiFactory
               .ChangeContext<ITradePageApi>(_driver)
               .PostPendingBuyOrderRequest(_tradingPlatformUrl, _tradeAmount, loginCookies,
               RateForPending, assetName: assetName);
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyPendingTradesTableTest()
        {
            var clientName = _clientEmail.Split("@").First().UpperCaseFirstLetter();
            var date = DateTime.Now.ToString("dd/MM/yyyy");

            var tradeDitails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IPendingTradesPageUi>(DataRep.CrmPendingTradeMenuItem)
                .SearchPendingTrades(_clientId)
                .GetSearchResultDetails<SearchResultTrade>()
                .First();

            Assert.Multiple(() =>
            {
                Assert.True(tradeDitails.clientname?.Contains(clientName),
                    $" expected client name: {clientName}" +
                    $" actual client name: {tradeDitails.clientname}");

                Assert.True(tradeDitails.opentimegmt?.Contains(date),
                    $" expected open time: {date}, " +
                    $" actual open time: {tradeDitails.opentimegmt}");

                Assert.True(tradeDitails.type == "buy",
                    $"expected type: 'buy', " +
                    $" actual buy: {tradeDitails.type}");

                Assert.True(tradeDitails.asset == DataRep.AssetNameShort,
                    $"expected asset: {DataRep.AssetNameShort}, " +
                    $" actual asset: {tradeDitails.asset}");

                Assert.True(tradeDitails.amount?.ToString() == $"{_tradeAmount}.00",
                    $"expected amount: {_tradeAmount}, " +
                    $" actual amount: {tradeDitails.amount}");
            });
        }
    }
}