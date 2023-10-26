using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using System.Linq;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture]
    public class VerifyCloseTradeWithPartialAmountApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private GetLoginResponse _loginData;
        private GeneralDto _tradeDetails;
        private int _tradeAmount = 3;
        private string _clientEmail;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var bonusAmount = 10000;

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
            _loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            // create trade
            _tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount, _loginData)
                .GeneralResponse;
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
        public void VerifyCloseTradeWithPartialAmountApiTest()
        {
            var expectedCloseTradeAmount = 2;
            var expectedOpenTradeAmount = _tradeAmount - expectedCloseTradeAmount;

            // mass close the trade
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PatchCloseTradeWithAmountRequest(_tradingPlatformUrl, _tradeDetails.TradeId,
                _loginData, expectedCloseTradeAmount);

            // get close trade amount
            var actualCloseTradeAmount = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetTradesByStatusRequest(_tradingPlatformUrl, _loginData, "close")
                .GeneralResponse
                .First()
                .amount;

            // get open trade amount
            var actualOpenTradeAmount = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetTradesByStatusRequest(_tradingPlatformUrl, _loginData, "open")
                .GeneralResponse
                .First()
                .amount;

            Assert.Multiple(() =>
            {
                Assert.True(actualCloseTradeAmount == expectedCloseTradeAmount,
                        $" expected Close Trade Amount: {expectedCloseTradeAmount}" +
                        $" actual Close Trade Amount: {actualCloseTradeAmount}");

                Assert.True(actualOpenTradeAmount == expectedOpenTradeAmount,
                        $" expected Open Trade Amount: {expectedOpenTradeAmount}" +
                        $" actual Open Trade Amount: {actualOpenTradeAmount}");
            });
        }
    }
}