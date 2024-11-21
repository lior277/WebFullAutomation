using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyCreateCloseTrade : TestSuitBase
    {
        #region Test Preparation
        public VerifyCreateCloseTrade(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;
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

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostBonusRequest(_crmUrl, clientId, bonusAmount);

            // get login Data for trading Platform
            var loginData = _apiFactory
                 .ChangeContext<ILoginApi>(_driver)
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            _apiFactory
                 .ChangeContext<ITradePageApi>(_driver)
                 .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount, loginData: loginData);
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
        public void VerifyCreateCloseTradeTest()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");

            var tradeDitails = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradePageUi>(DataRep.TradingOpenTradesMenuItem)
                .CloseTradePipe()
                .VerifyNumOfCloseTrades()
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradePageUi>(DataRep.TradingCloseTradeMenuItem)  
                .WaitForTradeTableToLoad()
                .ChangeContext<ISearchResultsUi>(_driver)
                .GetSearchResultDetails<SearchResultTrade>(tableName:
                "trade", cellsAndTitlesShouldBeEquals: false)
                .First();

            Assert.Multiple(() =>
            {
                Assert.True(tradeDitails.opentime?.Contains(date),
                          $"expected date: {date}" +
                          $" actual date: {tradeDitails.opentimegmt}");

                Assert.True(tradeDitails.instrument == DataRep.AssetNameShort || tradeDitails.instrument == DataRep.AssetName
                           || tradeDitails.instrument == DataRep.AssetName,
                          $"expected instrument: {DataRep.AssetNameShort}, " +
                          $" actual instrument: {tradeDitails.instrument}");

                Assert.True(tradeDitails.type == "buy",
                          $"expected type: 'buy', " +
                          $" actual buy: {tradeDitails.type}");

                Assert.True(tradeDitails.amount == _tradeAmount.ToString(),
                          $"expected amount: '1', " +
                          $" actual amount: {tradeDitails.amount}");
            });
        }
    }
}