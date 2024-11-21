// Ignore Spelling: Api

using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Agent
{
    [TestFixture]
    public class VerifyAgentCanGetClosedTradesApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradeId;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientId;      

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var bonusAmount = 10000;
            var tradeAmount = 10;

            // create client with first campaign
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);
            
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, bonusAmount);

            // get login Data for trading Platform
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            // create trade
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, loginData)
                .GeneralResponse;

            // close trade 
            _tradeId = tradeDetails.TradeId;
            #endregion
        }
        #endregion

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

        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyAgentCanGetClosedTradesApiTest()
        {
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostClosedTradesByAgentRequest(_tradingPlatformUrl);

            Assert.True(tradeDetails.trades.Any(p => p.id == _tradeId),
               $"actual trade Details not contains: {_tradeId}");
        }
    }
}