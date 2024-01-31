// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.TestCase;

namespace TestsProject.Tests.ClientsPage
{
    [TestFixture]
    public class VerifyMarginCallColumnOnClientsTableApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private GetLoginResponse _loginData;
        private IDictionary<string, string> _groupData;
        private string _clientId;
        private string _clientEmail;   
        private int _depositAmount = 10000;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var tradeAmount = 2;
            var maintenance = 10; 
            var marginCall = 1;
            var _dbContext = new QaAutomation01Context();

            var tradeGroupForMarginCallAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 1,
                maintenance = maintenance,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0,
                margin_call = marginCall,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            var subject = "Margin Call Trade";
            var emailBodyParams = new List<string> { "CLIENT_ID" };
            var emailsParams = new Dictionary<string, string> {
                { "type", "margin_call_trade" },
                { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl,
               emailsParams, emailBodyParams);

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

            // login data
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // create trade to retrieve the current rate
            var tradeDetails = _apiFactory
                 .ChangeContext<ITradePageApi>()
                 .PostBuyAssetRequest(_tradingPlatformUrl,
                 tradeAmount, _loginData)
                 .GeneralResponse;

            var openPrice = tradeDetails.TradeRate;
           
            var userBalance =
                (from s in _dbContext.UserAccounts
                 where s.UserId == _clientId
                 select new ExpectedFinanceData
                 {
                     balance = (int)s.Balance
                     .MathRoundFromGeneric(0, MidpointRounding.ToPositiveInfinity),                   
                 }).FirstOrDefault()
                 .balance;

            // Calculate Trade Amount For Margin Call
            tradeAmount = _apiFactory
                .ChangeContext<ITradePageApi>()
                .CalculateTradeAmountForMarginCall(openPrice, tradeAmount,
                maintenance, (double)userBalance, marginCall);

            // create trade for margin call
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, _loginData);

            // Create Trade Group And Assign It To client
            _groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, tradeGroupForMarginCallAttributes, _clientId);

            _tradeGroupsIdsListForDelete.Add(_groupData.Keys.First());

            // Start Margin Call
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetStartMarginCallRequest(_crmUrl);
        }

        [TearDown]
        public void TearDown()
        { 
            try
            {
                _apiFactory
                    .ChangeContext<ITradeGroupApi>()
                    .DeleteTradeGroupRequest(_crmUrl, _tradeGroupsIdsListForDelete);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        public void VerifyMarginCallColumnOnClientsTableApiTest()
        {
            var expectedMarginCallNotifiedDate = DateTime.Now.ToString("yyyy-MM-dd");

            var actualMarginCallNotifiedDate = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientRequest(_crmUrl, _clientEmail)
                .GeneralResponse
                .data
                .FirstOrDefault()
                .margin_call_notified_date;

            Assert.True(actualMarginCallNotifiedDate.Contains(expectedMarginCallNotifiedDate),
                $" expected Margin Call Notified Date {expectedMarginCallNotifiedDate}" +
                $" actual Margin Call Notified Date: {actualMarginCallNotifiedDate}");
        }
    }
}