// Ignore Spelling: Chrono Api

using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture]
    public class VerifyEditChronoTradeIsDisableApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private GetLoginResponse _loginData;
        private string _tradeId;
        private double _currentRate;
        private string _clientId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var bonusAmount = 10000;
            var tradeAmount = 3;

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            _apiFactory
                 .ChangeContext<IFinancesTabApi>()
                 .PostBonusRequest(_crmUrl, _clientId, bonusAmount);

            // get login Data for trading Platform
            _loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                 .GeneralResponse;

            // create trade
             var tradeDetails = _apiFactory 
                 .ChangeContext<ITradePageApi>()
                 .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, _loginData)
                 .GeneralResponse;

            _tradeId = tradeDetails.TradeId;
            _currentRate = tradeDetails.TradeRate;

            // close trade 
            _apiFactory
                .ChangeContext<IOpenTradesPageApi>()
                .PatchCloseTradeRequest(_tradingPlatformUrl, tradeDetails.TradeId);
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
                AfterTest();
            }
        }
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]       
        public void VerifyEditChronoTradeIsDisableApiTest()
        {
            var expectedStatus = "open";
            var expectedSwapCommission = 2;
            var expectedCommision = 2;
            var expectedSwapLong = 0.1;
            var expectedSwapShort = 0.1;
            var expectedCloseAtLose = (_currentRate - 100).ToString();
            var expectedCloseAtProfit = (_currentRate + 0.1).ToString();

            // edit the trade
            _apiFactory
                .ChangeContext<ITradesTabApi>()
                .PachtEditTradeByIdRequest(_tradingPlatformUrl, _tradeId,
                expectedSwapCommission, expectedSwapLong, expectedSwapShort,
                expectedStatus, expectedCommision,
                expectedCloseAtLose, expectedCloseAtProfit);

            // get trade from client card
            var actualEditTradeData = _apiFactory
                .ChangeContext<ITradesTabApi>()
                .GetTradesRequest(_tradingPlatformUrl, _clientId)
                .GeneralResponse
                .First();

            Assert.Multiple(() =>
            {
                Assert.True(actualEditTradeData.status == expectedStatus,
                    $" expected Edit Trade Status: {expectedStatus} ",
                    $" actual Edit Trade : {actualEditTradeData.status}");

                Assert.True(actualEditTradeData.swap_commission == expectedSwapCommission,
                    $" expected Edit Trade Swap Commision : {expectedSwapCommission} ",
                    $" actual Edit Trade Swap Commision : {actualEditTradeData.swap_commission}");

                Assert.True(actualEditTradeData.commision == expectedCommision,
                    $" expected Edit Trade  Commision: {expectedCommision} ",
                    $" actual Edit Trade Commision : {actualEditTradeData.commision}");

                Assert.True(actualEditTradeData.swap_long == expectedSwapLong,
                    $" expected Edit Trade Swap Long : {expectedSwapLong} ",
                    $" actual Edit Trade Swap Long: {actualEditTradeData.swap_long}");

                Assert.True(actualEditTradeData.swap_short == expectedSwapShort,
                    $" expected Edit Trade Swap Short: {expectedSwapShort} ",
                    $" actual Edit Trade Swap Short: {actualEditTradeData.swap_short}");

                Assert.True(actualEditTradeData.close_at_loss.ToString() == expectedCloseAtLose,
                    $" expected Close At Loss : {expectedCloseAtLose} ",
                    $" actual Edit Close At Loss: {actualEditTradeData.close_at_loss}");

                Assert.True(actualEditTradeData.close_at_profit.ToString() == expectedCloseAtProfit,
                    $" expected Close At Profit : {expectedCloseAtProfit} ",
                    $" actual Close At Profit : {actualEditTradeData.close_at_profit}");
            });
        }
    }
}