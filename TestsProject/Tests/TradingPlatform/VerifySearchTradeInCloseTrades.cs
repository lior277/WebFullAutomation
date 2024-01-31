using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
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
    public class VerifySearchTradeInCloseTrades : TestSuitBase
    {
        #region Test Preparation
        public VerifySearchTradeInCloseTrades(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _assetNameETH = DataRep.AssetName;
        private string _assetNameBTCUSD = "BTCUSD";
        private Default_Attr _tradeGroupAttributes;
        private IWebDriver _driver;
        private string _browserName;
        private string _tradeGroupId;
        private string _expectedRegularTradeTradeId;
        private int _depositAmount = 100000;
        
        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();
            var tradeAmount = 2;

            _tradeGroupAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 5,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = -0.1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostBonusRequest(_crmUrl, clientId, _depositAmount);

            // get login Data for trading Platform
            var  loginData = _apiFactory
                 .ChangeContext<ILoginApi>(_driver)
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                 .GeneralResponse;

            var tradeDetails = _apiFactory
                 .ChangeContext<ITradePageApi>(_driver)
                 .PostBuyAssetRequest(_tradingPlatformUrl,
                 tradeAmount, loginData: loginData, assetSymble: _assetNameETH)
                 .GeneralResponse;

            var currentRate = tradeDetails.TradeRate;
            var takeProfitRate = (double)(currentRate + 0.0001); // to open a takeProfit trade

            var tradeData = _apiFactory // create trade with take profit
                .ChangeContext<ITradePageApi>()
                .CreateTakeProfitApi(_tradingPlatformUrl,
                tradeAmount, loginData, takeProfitRate);

            // Create Crypto Group And Assign It To client to close the trade
            var groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                _tradeGroupAttributes, clientId);

            _tradeGroupId = groupData.Keys.First();

            tradeDetails = _apiFactory
                 .ChangeContext<ITradePageApi>(_driver)
                 .PostBuyAssetRequest(_tradingPlatformUrl,
                 tradeAmount, loginData: loginData, assetSymble: _assetNameBTCUSD)
                 .GeneralResponse;

            _expectedRegularTradeTradeId = tradeDetails.TradeId;

            _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PatchCloseTradeRequest(_tradingPlatformUrl,
                _expectedRegularTradeTradeId, tradeAmount, loginData);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(clientEmail, _tradingPlatformUrl);
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                _apiFactory
                    .ChangeContext<ITradeGroupApi>()
                    .DeleteTradeGroupRequest(_crmUrl, _tradeGroupId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        public void VerifySearchTradeInCloseTradesTest()
        {
            var expectedCloseReason = "Trade take profit close";

           var actualSearchById = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradePageUi>(DataRep.TradingCloseTradeMenuItem)
                .WaitForTradeTableToLoad()
                .ChangeContext<ITradePageUi>(_driver)
                .SearchTrade(_expectedRegularTradeTradeId)
                .GetSearchResultDetails<SearchResultTrade>(tableName:
                "trade", cellsAndTitlesShouldBeEquals: false  )
                .First();

            var actualSearchByAssetName = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradePageUi>(DataRep.TradingCloseTradeMenuItem)
                .WaitForTradeTableToLoad()
                .ChangeContext<ITradePageUi>(_driver)
                .SearchTrade(_assetNameETH)
                .GetSearchResultDetails<SearchResultTrade>(tableName:
                "trade", cellsAndTitlesShouldBeEquals: false)
                .First();

            var actualSearchByCloseReason = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradePageUi>(DataRep.TradingCloseTradeMenuItem)
                .WaitForTradeTableToLoad()
                .ChangeContext<ITradePageUi>(_driver)
                .SearchTrade(expectedCloseReason)
                .GetSearchResultDetails<SearchResultTrade>(tableName:
                "trade", cellsAndTitlesShouldBeEquals: false)
                .First();

            Assert.Multiple(() =>
            {
                Assert.True(actualSearchById.instrument == "BTC",
                    $" expected instrument when Search By Id: BTC" +
                    $" actual instrument when Search By Id: {actualSearchById.instrument}");

                Assert.True(actualSearchByAssetName.instrument == "ETH",
                    $" expected instrument when Search By Asset Name : ETH" +
                    $" actual instrument when Search By Asset Name : {actualSearchById.instrument}");

                Assert.True(actualSearchByCloseReason.closereason == expectedCloseReason,
                    $" expected close reason when Search By close reason: {expectedCloseReason}" +
                    $" actual close reason when Search By close reason: {actualSearchByCloseReason.closereason}");
            });
        }
    }
}