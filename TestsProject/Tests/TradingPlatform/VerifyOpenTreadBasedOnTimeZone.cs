using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
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
    [TestFixture(DataRep.Chrome, "UTC")]
    public class VerifyOpenTreadBasedOnTimeZone : TestSuitBase
    {
        #region Test Preparation     
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private GetLoginResponse _loginData;
        private string _clientEmail;
        private string _timezone;
        private GetAppleAsset _getAppleAsset;
        private IWebDriver _driver;
        private string _browserName;

        public VerifyOpenTreadBasedOnTimeZone(string browser, string timeZone) : base(browser, timeZone)
        {
            _timezone = timeZone;
            _browserName = browser;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostBonusRequest(_crmUrl, clientId);

            // get login Data for trading Platform
            _loginData = _apiFactory
                 .ChangeContext<ILoginApi>(_driver)
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            _getAppleAsset = _apiFactory
              .ChangeContext<ITradePageApi>(_driver)
              .GetAppleAssetRequest(_tradingPlatformUrl, _loginData);
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
        public void VerifyOpenTreadBasedOnTimeZoneTest()
        {
            var assetName = "Apple";
            string expectedAssetsStatus;
            string expectedAssetStatusOnAssetCard = null;
            string actualAssetStatusOnAssetCard = null;
            var expectedNumOfTrades = 4;
            var tradeAmount = 2;

            // from stock exchange
            var actualAssetStatusFromExchange = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .GetAssetStatusPipe(_getAppleAsset);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl);

            //Thread.Sleep(3000);

            //_driver.Navigate().Refresh();
            //_driver.WaitForAnimationToLoad(); // wait after refresh         

            if (actualAssetStatusFromExchange == "close")
            {
                expectedAssetsStatus = "pending";
                expectedAssetStatusOnAssetCard = "Trading is closed";

                // buy and sell closed asset ui
                _apiFactory
                   .ChangeContext<ITradePageUi>(_driver)
                   .SearchAssetPipe(assetName)
                   .ClickOnBuyButton()
                   .ClickOnCloseDealForCloedAssetButton();

                _driver.Navigate().Refresh();
                //_driver.WaitForAnimationToLoad();

                actualAssetStatusOnAssetCard = _apiFactory
                    .ChangeContext<ITradePageUi>(_driver)
                    .SearchAssetPipe(assetName)
                    .ClickOnSellButton()
                    .ClickOnCloseDealForCloedAssetButton()
                    .GetAssetStatus();

                // buy closed asset api
                _apiFactory
                     .ChangeContext<ITradePageApi>(_driver)
                     .PostBuyCloseAssetRequest(_tradingPlatformUrl,
                     tradeAmount, _loginData, assetSymble: assetName.ToUpper());

                // sell closed asset api
                _apiFactory
                     .ChangeContext<ITradePageApi>(_driver)
                     .PostSellCloseAssetRequest(_tradingPlatformUrl,
                     tradeAmount, _loginData, assetSymble: assetName.ToUpper());

                _apiFactory
                    .ChangeContext<IMenus>(_driver)
                    .ClickOnMenuItem<ITradePageUi>(DataRep.TradingPendingTradeMenuItem);
            }
            else
            {
                expectedAssetsStatus = "open";

                // refresh because apple asset should be up
                //_driver.Navigate().Refresh();
                //Thread.Sleep(3000);

                // buy asset ui
                _apiFactory
                   .ChangeContext<ITradePageUi>(_driver)
                   .SearchAssetPipe(assetName)
                   .ClickOnBuyButton()
                   .ClickOnCloseDealButton();

                // refresh because apple asset should be up
                //_driver.Navigate().Refresh();
                //Thread.Sleep(3000);

                // sell asset ui
                _apiFactory
                   .ChangeContext<ITradePageUi>(_driver)
                   .SearchAssetPipe(assetName)
                   .ClickOnSellButton()
                   .ClickOnCloseDealButton();

                // buy asset api
                _apiFactory
                    .ChangeContext<ITradePageApi>(_driver)
                    .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, _loginData,
                     assetSymble: assetName.ToUpper());

                // sell asset api
                _apiFactory
                    .ChangeContext<ITradePageApi>(_driver)
                    .PostSellAssetRequest(_tradingPlatformUrl, tradeAmount, _loginData,
                    assetSymble: assetName.ToUpper());
            }

            var actualTrades = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .GetClientTradesCountRequest(_tradingPlatformUrl, _loginData)
                .GeneralResponse
                .First();

            Assert.Multiple(() =>
            {
                // if trade is closed
                if (actualAssetStatusOnAssetCard != null)
                {
                    Assert.True(actualAssetStatusOnAssetCard == expectedAssetStatusOnAssetCard,
                        $" expected assets Status on asset card : {expectedAssetStatusOnAssetCard} on timezone : {_timezone}" +
                        $" actual Asset Status on asset card: {actualAssetStatusOnAssetCard}");
                }

                Assert.True(actualTrades.total == expectedNumOfTrades,
                        $" expected Num Of Trades : {actualTrades.total} on timezone : {_timezone}" +
                        $" actual Num Of Trades: {expectedNumOfTrades}");

                Assert.True(actualTrades.status == expectedAssetsStatus,
                        $" expected status in ui : {actualTrades.status} on timezone : {_timezone}" +
                        $" actual status in ui: {expectedAssetsStatus}");
            });
        }
    }
}