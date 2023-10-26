// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
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
    public class VerifyTradeValuesWhenEditClosedTradeApi : TestSuitBase
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
            // buy chrono trade 
            var tradeDetails = _apiFactory
                .ChangeContext<IChronoTradePageApi>()
                .PostBuyChronoAssetApi(_tradingPlatformUrl, _loginData)
                .GeneralResponse;

            _tradeId = tradeDetails.TradeId;
            _currentRate = tradeDetails.TradeRate;

            // close trade 
            _apiFactory
                .ChangeContext<IOpenTradesPageApi>()
                .PatchCloseTradeRequest(_tradingPlatformUrl, _tradeId);
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]       
        public void VerifyTradeValuesWhenEditClosedTradeApiTest()
        {
            var expectedErrorMessage = "Method Not Allowed";
            var status = "open";
            var swapCommission = 2;
            var commision = 2;
            var swapLong = 0.1;
            var swapShort = 0.1;
            var closeAtLose = (_currentRate - 100).ToString();
            var closeAtProfit = (_currentRate + 0.1).ToString();

            // edit the trade
            var actualEditChronoErrorMessage = _apiFactory
                .ChangeContext<ITradesTabApi>()
                .PachtEditTradeByIdRequest(_tradingPlatformUrl, _tradeId,
                swapCommission, swapLong, swapShort, status, commision,
                closeAtLose, closeAtProfit, checkStatusCode: false);

            Assert.Multiple(() =>
            {
                Assert.True(actualEditChronoErrorMessage == expectedErrorMessage,
                    $" expected Edit Chrono Error Message : {expectedErrorMessage}" +
                    $" actual Edit Chrono Error Message: {actualEditChronoErrorMessage}");      
            });
        }
    }
}