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

namespace TestsProject.Tests.CrmTrade.OpenTradePage
{
    //[TestFixture(DataRep.Firefox)]
    [TestFixture(DataRep.Chrome)]
    public class VerifyOpenTradeTable : TestSuitBase
    {
        #region Test Preparation
        public VerifyOpenTradeTable(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;

        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

        private string _clientEmail;
        private string _clientId;
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
            var userName = TextManipulation.RandomString();

            // create user
            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostBonusRequest(_crmUrl, _clientId, bonusAmount);

            // get login Data for trading Platform
            var loginData = _apiFactory
                 .ChangeContext<ILoginApi>(_driver)
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            _apiFactory
                 .ChangeContext<ITradePageApi>(_driver)
                 .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount, loginData);

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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyOpenTradeTableTest()
        {
            var clientName = _clientEmail.Split("@")[0].UpperCaseFirstLetter();
            var date = DateTime.Now.ToString("dd/MM/yyyy");

            var tradeDitails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IOpenTradesPageUi>(DataRep.CrmOpenTradesMenuItem)
                .SearchOpenTrades(_clientId)
                .GetSearchResultDetails<SearchResultTrade>().First();

            Assert.Multiple(() =>
            {

                Assert.True(tradeDitails.clientname?.Contains(clientName),
                    $"expected client name: {clientName}" +
                    $" actual date: {tradeDitails.clientname}");

                Assert.True(tradeDitails.opentimegmt?.Contains(date),
                    $"expected opentime: {date}, " +
                    $" actual instrument: {tradeDitails.opentimegmt}");

                Assert.True(tradeDitails.type == "buy",
                    $"expected type: 'buy', " +
                    $" actual buy: {tradeDitails.type}");

                Assert.True(tradeDitails.asset == DataRep.AssetNameShort,
                    $"expected asset: {DataRep.AssetNameShort}, " +
                    $" actual buy: {tradeDitails.asset}");

                Assert.True(tradeDitails.amount?.ToString() == $"{_tradeAmount}.00",
                    $"expected amount: '1', " +
                    $" actual amount: {tradeDitails.amount}");
            });
        }
    }
}