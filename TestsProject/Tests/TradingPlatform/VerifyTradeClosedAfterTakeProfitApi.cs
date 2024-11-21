// Ignore Spelling: Api

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
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture]
    public class VerifyTradeClosedAfterTakeProfitApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;
        private string _closeReason;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var bonusAmount = 10000;
            var tradeAmount = 2;

            var tradeGroupAttributes = new Default_Attr
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

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            _apiFactory
                  .ChangeContext<IFinancesTabApi>()
                  .PostBonusRequest(_crmUrl, clientId, bonusAmount);

            // get login Data for trading Platform
            var loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            var tradeDetails = _apiFactory // create trade to retrieve the current rate
                 .ChangeContext<ITradePageApi>()
                 .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, loginData)
                 .GeneralResponse;

            var currentRate = tradeDetails.TradeRate;
            var takeProfitRate = (double)(currentRate + 0.0001); // to open a takeProfit trade

            _apiFactory // create trade with take profit
               .ChangeContext<ITradePageApi>()
               .CreateTakeProfitApi(_tradingPlatformUrl, tradeAmount, loginData, takeProfitRate);

            var  groupData = _apiFactory // Create Cripto Group And Assign It To client
              .ChangeContext<ISharedStepsGenerator>()
              .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, tradeGroupAttributes, clientId);

            var  groupId = groupData.Keys.First();

            _closeReason = _apiFactory // get close trades
             .ChangeContext<ITradePageApi>()
             .GetTradesByStatusRequest(_tradingPlatformUrl, loginData, "close")
             .GeneralResponse[0]
             .close_reason;

            _apiFactory
            .ChangeContext<ITradeGroupApi>()
            .DeleteTradeGroupRequest(_crmUrl, groupId);         
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
        public void VerifyTradeClosedAfterTakeProfitApiTest()
        {          
                Assert.True(_closeReason == "trade tp close",
                          $"expected reason: trade tp close" +
                          $" actual reason: {_closeReason}");                
        }
    }
}