// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
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

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyTakeProfitEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private GetLoginResponse _loginData;
        private double _takeProfitRate;
        private string _clientEmail;
        private string _clientId;
        private int _depositAmount = 10000;
        private Default_Attr _tradeGroupAttributes;
        private string _testimUrl = DataRep.TesimUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
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
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
              emailPrefix: DataRep.TestimEmailPrefix);

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
                 .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, _loginData)
                 .GeneralResponse;

            var currentRate = tradeDetails.TradeRate;

            // to open a take profit trade
            _takeProfitRate = (double)(currentRate + 0.0001);            
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
        public void VerifyTakeProfitEmailApiTest()
        {
            var tradeAmount = 2;
            var expectedCloseReason = "trade tp close";
            var subject = "Take Profit order has been activated";
            var emailBodyParams = new List<string> { "TRADE_TP", "TRADE_CLOSE_REASON" };

            var emailsParams = new Dictionary<string, string> { 
                { "type", "trade_tp_close" }, { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            #region trigger the mail
            // trigger the mail
            // create trade with take profot
            _apiFactory
               .ChangeContext<ITradePageApi>()
               .CreateTakeProfitApi(_tradingPlatformUrl, tradeAmount, _loginData, _takeProfitRate);

            // Create Cripto Group And Assign It To client
            var  groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, _tradeGroupAttributes, _clientId);

            var  groupId = groupData.Keys.First();

            // get close trades
            var actualCloseTardeDitails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetTradesByStatusRequest(_tradingPlatformUrl, _loginData, "close");

            // delete cripto group
            _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .DeleteTradeGroupRequest(_crmUrl, groupId);
            #endregion

            var email = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(_testimUrl, _clientEmail, subject)
                .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email);

            var closeAtProfit = (Math.Truncate(Convert.ToDecimal(actualCloseTardeDitails.GeneralResponse[0].close_at_profit * 100)) / 100).ToString();

            Assert.Multiple(() =>
            {
                Assert.True(actualEmailBody["TRADE_TP"].StringToInt() == closeAtProfit.StringToInt(),
                    $" expected TRADE_TP: {closeAtProfit}" +
                    $" actual TRADE_TP: {actualEmailBody["TRADE_TP"]}");              

                Assert.True(email.Subject == subject,
                    $" expected email subject: {email.Subject}" +
                    $" actual email subject: {subject}");

                Assert.True(actualEmailBody["TRADE_CLOSE_REASON"].Contains(expectedCloseReason),
                    $" expected TRADE_CLOSE_REASON: {expectedCloseReason}" +
                    $" actual TRADE_CLOSE_REASON: {actualEmailBody["TRADE_CLOSE_REASON"]}");
            });
        }
    }
}