using System.Collections.Generic;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.MyProfilePanel
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyTradingOnMyProfilePage : TestSuitBase
    {
        #region Test Preparation
        public VerifyTradingOnMyProfilePage(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;
        private string _clientId;
        private int _pnlAmountForEachTrade = 5;
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var crmUrl = Config.appSettings.CrmUrl;
            var depositAmount = 10000;
            var tradeAmount = 2;
            var tradeIdsForClose = new List<string>();
            var tradeIdsForUpdatePnl = new List<string>();
            _driver = GetDriver();

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(crmUrl, clientName, currency: DataRep.DefaultUSDCurrencyName);

            // get login data
            var loginData = _apiFactory
               .ChangeContext<ILoginApi>()
               .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
               .GeneralResponse;

            // deposit 10000
            #region deposit 10000
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(crmUrl, _clientId, depositAmount);
            #endregion

            #region chrono
            // sell chrono trade 
            var tradeDetails = _apiFactory
                .ChangeContext<IChronoTradePageApi>()
                .PostSellChronoAssetApi(_tradingPlatformUrl, loginData);

            // close the chrono trade
            _apiFactory
                .ChangeContext<IChronoTradePageApi>()
                .PatchCloseChronoTradeRequest(_tradingPlatformUrl,
                tradeDetails.TradeId, loginData);

            tradeIdsForUpdatePnl.Add(tradeDetails.TradeId);

            // buy chrono trade 
            tradeDetails = _apiFactory
                .ChangeContext<IChronoTradePageApi>()
                .PostBuyChronoAssetApi(_tradingPlatformUrl, loginData)
                .GeneralResponse;

            tradeIdsForUpdatePnl.Add(tradeDetails.TradeId);

            // close the chrono trade
            _apiFactory
                .ChangeContext<IChronoTradePageApi>()
                .PatchCloseChronoTradeRequest(_tradingPlatformUrl,
                tradeDetails.TradeId, loginData);
            #endregion

            // create open trade
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, loginData)
                .GeneralResponse;

            tradeIdsForUpdatePnl.Add(tradeDetails.TradeId);
            tradeIdsForClose.Add(tradeDetails.TradeId);

            // create open trade
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, loginData)
                .GeneralResponse;

            tradeIdsForUpdatePnl.Add(tradeDetails.TradeId);
            tradeIdsForClose.Add(tradeDetails.TradeId);

            // create sell trade
            tradeDetails = _apiFactory
               .ChangeContext<ITradePageApi>()
               .PostSellAssetRequest(_tradingPlatformUrl, tradeAmount, loginData)
               .GeneralResponse;

            tradeIdsForUpdatePnl.Add(tradeDetails.TradeId);
            tradeIdsForClose.Add(tradeDetails.TradeId);
            var currentRate = tradeDetails.TradeRate;
            var rateForPending = (double)(currentRate + 100); // to open a pending trade  

            // open the pending trade
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostPendingBuyOrderRequest(_tradingPlatformUrl, tradeAmount, loginData, rateForPending);

            tradeIdsForClose.Add(tradeDetails.TradeId);

            //close all the trades
            _apiFactory
                .ChangeContext<IOpenTradesPageApi>()
                .PatchCloseTradeRequest(_tradingPlatformUrl, tradeIdsForClose);

            _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .UpdateTradePnl(tradeIdsForUpdatePnl, 5);

            _apiFactory
               .ChangeContext<ITradePageApi>(_driver)
               .UpdateTradePnl(tradeIdsForUpdatePnl, 5);
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

        // create client
        // get login data
        // create deposit 10000
        // create buy chrono  
        // create sell chrono 
        // sleep for 30000 for chrono close
        // create buy open trade that will stay open
        // create buy trade
        // create sell trade
        // create pending trade
        // close all the open trades
        // Get Total Pnl From Trades View
        // verify Total Trades = 5
        // verify Short Trades = 2
        // verify Long Trades = 3
        // verify Total Pnl Currency Sign = $
        // verify Short Trades Color = d4494901,ffd44949
        // verify Long Trades Color = 2edc8701,ff2edc87
        [Test]        
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyTradingOnMyProfilePageTest()
        {
            var expectedTotalTrades = "5";
            var expectedShortTrades = "2";
            var expectedLongTrades = "3";
            var expectedTotalPnl = _pnlAmountForEachTrade * 5;  // 5 is the num of trade update pnl for each trade        
            var expectedTotalPnlCurrencySign = "$";
            var expectedShortTradesColorChromeAndFirafox = "d4494901,ffd44949";
            var expectedLongTradesColorChromeAndFirafox = "2edc8701,ff2edc87";

            var actualTotalTrades = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradePageUi>(DataRep.MyProfileMenuItem)
                .GetTotalTradesValue();

            var actualShortTrades = _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .GetShortTradesValue();

            var actualLongTrades = _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .GetLongTradesValue();

            // expected Total Pnl 
            //_expectedTotalPnl = _apiFactory
            //    .ChangeContext<ITradePageApi>(_driver)
            //    .GetTotalPnlFromTradesView(_clientId);

            var actualTotalPnl = _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .GetTotalPnlValue()
                .MathAbsGeneric();

            var actualTotalPnlCurrencySign = _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .GetTotalPnlCurrencySign();

            var actualShortTradesColorName = _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .GetShortTradesRgbColorValue()
                .ConvertRgbToColor()
                .Name;

            var actualLongTradesColorName = _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .GetLongTradesRgbColorValue()
                .ConvertRgbToColor()
                .Name;

            Assert.Multiple(() =>
            {
                Assert.True(actualTotalTrades == expectedTotalTrades,
                    $" expected Total Trades Value: {expectedTotalTrades}" +
                    $" actual Total Trades Value: {actualTotalTrades}");

                Assert.True(actualShortTrades == expectedShortTrades,
                    $" expected Short Trades Value: {expectedShortTrades}" +
                    $" actual Short Trades Value: {actualShortTrades}");

                Assert.True(actualLongTrades == expectedLongTrades,
                    $" expected Long Trades Value: {expectedLongTrades}" +
                    $" actual Long Trades Value: {actualLongTrades}");

                Assert.True(actualTotalPnl == expectedTotalPnl,
                    $" expected Total Pnl Value: {expectedTotalPnl}" +
                    $" actual Total Pnl Value: {actualTotalPnl.MathAbsGeneric()}");

                Assert.True(actualTotalPnlCurrencySign.Contains(expectedTotalPnlCurrencySign),
                    $" expected Total Pnl Currency Sign: {expectedTotalPnlCurrencySign}" +
                    $" actual Total Pnl Currency Sign: {actualTotalPnlCurrencySign}");

                Assert.True(expectedShortTradesColorChromeAndFirafox.Contains(actualShortTradesColorName),
                    $" expected Short Trades Color name Firafox or chrome: {expectedShortTradesColorChromeAndFirafox}" +
                    $" actual Short Trades Color name Value: {actualShortTradesColorName}");

                Assert.True(expectedLongTradesColorChromeAndFirafox.Contains(actualLongTradesColorName),
                    $" expected long Trades Color name: {expectedLongTradesColorChromeAndFirafox}" +
                    $" actual long Trades Color name: {actualLongTradesColorName}");
            });
        }
    }
}