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
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.CrmTrade.CloseTradePage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyCloseTradesTable : TestSuitBase
    {
        #region Test Preparation
        public VerifyCloseTradesTable(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientEmail;
        private string _clientId;
        private int _tradeAmount = 10;
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var bonusAmount = 10000;
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
            _driver = GetDriver();

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
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, bonusAmount);

            // get login Data for trading Platform
            var loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            // buy asset
            var tradeDetails = _apiFactory
                 .ChangeContext<ITradePageApi>()
                 .PostBuyAssetRequest(tradingPlatformUrl,
                  amount: _tradeAmount, loginData: loginData)
                 .GeneralResponse;

            var tradeId = tradeDetails.TradeId;

            // close trade
            _apiFactory
                 .ChangeContext<IOpenTradesPageApi>()
                 .PatchCloseTradeRequest(_crmUrl, tradeId);
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
        public void VerifyCloseTradesTableTest()
        {
            var clientName = _clientEmail.Split("@").First().UpperCaseFirstLetter();
            var date = DateTime.Now.ToString("dd/MM/yyyy");

            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClosedTradesPageUi>(DataRep.CrmCloseTradeMenuItem)
                .SearchCloseTrades(_clientId)
                .ChangeContext<IClosedTradesPageUi>(_driver)
                .WaitForCloseTradeTableToLoad();

            var tradeDitails = _apiFactory
                .ChangeContext<ISearchResultsUi>(_driver)
                .GetSearchResultDetails<SearchResultTrade>()
                .First();

            Assert.Multiple(() =>
            {
                Assert.True(tradeDitails.clientname.Contains(clientName),
                    $" expected client name: {clientName}" +
                    $" actual date: {tradeDitails.clientname}");

                Assert.True(tradeDitails.opentimegmt?.Contains(date),
                    $" expected open time: {date}, " +
                    $" actual open time: {tradeDitails.opentimegmt}");

                Assert.True(tradeDitails.type == "buy",
                    $" expected type: 'buy', " +
                    $" actual buy: {tradeDitails.type}");

                Assert.True(tradeDitails.asset == DataRep.AssetNameShort,
                    $" expected asset: {DataRep.AssetNameShort}, " +
                    $" actual buy: {tradeDitails.asset}");

                Assert.True(tradeDitails.amount?.ToString() == $"{_tradeAmount}.00",
                    $" expected amount: {_tradeAmount}, " +
                    $" actual amount: {tradeDitails.amount}");
            });
        }
    }
}