using System;
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
    public class VerifyCreateTakeProfitTrade : TestSuitBase
    {
        #region Test Preparation
        public VerifyCreateTakeProfitTrade(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;
        private string _clientId;
        private string _tradeId;
        private double _takeProfitRate;
        private GetLoginResponse _loginData;
        private string _assetNameShort = DataRep.AssetNameShort;
        private string _assetName = DataRep.AssetName;
        private Default_Attr _tradeGroupAttributes;
        private IWebDriver _driver;
        private string _browserName;
        private string _tradeGroupId;
        private int _depositAmount = 10000;
        
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
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostBonusRequest(_crmUrl, _clientId, _depositAmount);

            // get login Data for trading Platform
            _loginData = _apiFactory
                 .ChangeContext<ILoginApi>(_driver)
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            var tradeDetails = _apiFactory
                 .ChangeContext<ITradePageApi>(_driver)
                 .PostBuyAssetRequest(_tradingPlatformUrl,
                 tradeAmount, loginData: _loginData, assetSymble: _assetName)
                 .GeneralResponse;

            _tradeId = tradeDetails.TradeId;
            var currentRate = tradeDetails.TradeRate;

            // to open a take profit trade
            _takeProfitRate = currentRate + 100;
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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyCreateTakeProfitTradeTest()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var expectedTradeAmount = 2;
            var expectedCloseReasn = "Trade take profit close";

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl)          
                .ChangeContext<ITradePageUi>(_driver)
                .CreateTakeProfitTradePipe(_takeProfitRate.ToString(), _assetNameShort,
                expectedTradeAmount, verify: false);

            // Create Crypto Group And Assign It To client
            var groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                _tradeGroupAttributes, _clientId);

            // get last Trade Id
            var lastTradeId = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetTradesByStatusRequest(_tradingPlatformUrl, _loginData, "close")
                .GeneralResponse
                .Last()
                .id;

            // wait for trade to close
            _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .WaitForCfdTradeToClose(_tradingPlatformUrl,
                lastTradeId, _loginData);

            _tradeGroupId = groupData.Keys.First();

           var tradeDitails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradePageUi>(DataRep.TradingCloseTradeMenuItem)
                .WaitForTradeTableToLoad()
                .ChangeContext<ISearchResultsUi>(_driver)
                .GetSearchResultDetails<SearchResultTrade>(tableName:
                "trade", cellsAndTitlesShouldBeEquals: false  )
                .First();        

            Assert.Multiple(() =>
            {
                Assert.True(tradeDitails.opentime?.Contains(date),
                    $" expected date: {date}" +
                    $" actual date: {tradeDitails.opentimegmt}");

                Assert.True(tradeDitails.instrument == _assetNameShort,
                    $" expected instrument: {_assetNameShort}, " +
                    $" actual instrument: {tradeDitails.instrument}");

                Assert.True(tradeDitails.type == "buy",
                    $" expected type: 'buy', " +
                    $" actual buy: {tradeDitails.type}");

                Assert.True(tradeDitails.amount == expectedTradeAmount.ToString(),
                    $" expected amount: '1', " +
                    $" actual amount: {tradeDitails.amount}");

                Assert.True(tradeDitails.closereason == expectedCloseReasn,
                    $" expected close reason: {expectedCloseReasn} " +
                    $" actual close reason: {tradeDitails.closereason}");
            });
        }
    }
}