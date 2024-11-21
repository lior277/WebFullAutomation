// Ignore Spelling: Api

using System;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
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
    public class VerifyBalanceOnClientCardEqualToSearchResultsApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;
        private string _clientId;
        private int _depositAmount = 10000;


        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var _tradeAmount = 10;

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // create deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            // get client login cookies
            var loginCookies = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // create  trade 
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount, loginCookies)
                .GeneralResponse;

            var  tradeId = tradeDetails.TradeId;

            // close trade
            _apiFactory
               .ChangeContext<IOpenTradesPageApi>()
               .PatchCloseTradeRequest(_crmUrl, tradeId);       
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
        /// <summary>
        /// pre condition
        /// create client  
        /// create deposit 
        /// create trade
        /// close trade
        /// Verify Balance On Client Card is Equal To _balance on client details from Search results
        /// </summary>
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyBalanceOnClientCardEqualToSearchResultsApiTest()
        {
            string clientSearchBalance = null;

            for (var i = 0; i < 1000; i++)
            {
                // wait for procedure to updade the _balance
                Thread.Sleep(100);

                clientSearchBalance = _apiFactory
                    .ChangeContext<IClientsApi>()
                    .GetClientRequest(_crmUrl, _clientEmail)
                    .GeneralResponse
                    .data
                    .FirstOrDefault()
                    .balance
                    .Split('$')[0]
                    .TrimEnd();

                if (clientSearchBalance.Contains(_depositAmount.ToString()))
                {
                    continue;
                }

                return;
            }

            var clientCardBalance = _apiFactory
               .ChangeContext<IClientsApi>()
               .GetClientByIdRequest(_crmUrl, _clientId)
               .GeneralResponse
               .user
               .balance;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(Convert.ToDouble(clientSearchBalance)
                    .Equals(Convert.ToDouble(clientCardBalance)),
                    $" Balance value from get client request :{clientSearchBalance}" +
                    $" Balance value from get client Card request: {clientCardBalance}");
            });
        }
    }
}