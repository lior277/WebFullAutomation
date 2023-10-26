using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
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
    public class VerifyStopLossEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private GetLoginResponse _loginData;
        private double _stopLossRate;
        private string _clientEmail;
        private string _clientId;
        private string _groupId;
        private GeneralDto _tradeDetails;
        private int _depositAmount = 10000;
        private int _tradeLeverage = 5;
        private Default_Attr _tradeGroupAttributes;
        private string _testimUrl = DataRep.TesimUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var tradeAmount = 2;

            _tradeGroupAttributes = new Default_Attr
            {
                commision = 0,
                leverage = _tradeLeverage,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0.1,
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
            _tradeDetails = _apiFactory
                 .ChangeContext<ITradePageApi>()
                 .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, _loginData)
                 .GeneralResponse;

            var currentRate = _tradeDetails.TradeRate;

            // to open a stop loss trade
            _stopLossRate = currentRate - 0.0001;
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete cripto group
                _apiFactory
                    .ChangeContext<ITradeGroupApi>()
                    .DeleteTradeGroupRequest(_crmUrl, _groupId);
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
        public void VerifyStopLossEmailApiTest()
        {
            var subject = "Stop Loss order has been activated";
            var tradeAmount = 2;

            var emailBodyParams = new List<string> { "TRADE_ID", "TRADE_ASSET_NAME",
                "TRADE_TYPE", "TRADE_AMOUNT", "TRADE_OPEN_PRICE",
                "TRADE_OPEN_TIME", "TRADE_SL", "TRADE_TP",
                "TRADE_CLOSE_TIME", "TRADE_CLOSE_PRICE", "TRADE_COMMISSION",
                "TRADE_MARGIN_M", "TRADE_PNL", "TRADE_CLOSE_REASON", "TRADE_LEVERAGE",
                "TRADE_ORDER_PRICE", "TRADE_CREATE_TIME" };

            var emailsParams = new Dictionary<string, string> {
                { "type", "trade_sl_close" }, { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            #region trigger the mail
            // trigger the mail
            // create trade with stop loss
            _tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .CreateStopLossApi(_tradingPlatformUrl, tradeAmount, _loginData, _stopLossRate);

            // Create Cripto Group And Assign It To client to close the stop loss
            var groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, _tradeGroupAttributes, _clientId);

            // wait for trade to close
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForCfdTradeToClose(_tradingPlatformUrl, _tradeDetails.Id, _loginData);

            _groupId = groupData.Keys.First();

            // get close trades
            var actualCloseTardeDitails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetTradesByStatusRequest(_tradingPlatformUrl, _loginData, "close")
                .GeneralResponse;
            #endregion

            // wait for stop loose email
            var email = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(_testimUrl, _clientEmail, subject)
                .First();

            // get email body
            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email);

            // expected Trade Open Price
            var expectedTradeOpenPrice = (int)Math
                .Round(actualCloseTardeDitails.First()
                .rate, 4, MidpointRounding.AwayFromZero);

            // expected trade close price
            var expectedTradeClosePrice = (int)Math
                .Round(actualCloseTardeDitails.First()
                .current_rate, 4, MidpointRounding.ToEven);

            // close at loss
            var expectedCloseAtLoss = (int)Math
                .Round(actualCloseTardeDitails
                .FirstOrDefault()
                .close_at_loss, 4, MidpointRounding.AwayFromZero);

            // expected close at profit
            var expectedCloseAtProfit = (int)Math
                .Round(actualCloseTardeDitails.First()
                .close_at_profit, 4, MidpointRounding.AwayFromZero);

            Assert.Multiple(() =>
            {
                Assert.True(actualEmailBody["TRADE_ID"] == actualCloseTardeDitails[0].id,
                    $" expected TRADE ID: {actualCloseTardeDitails[0].id}" +
                    $" actual TRADE ID: {actualEmailBody["TRADE_ID"]}" +
                    $" client id: {_clientEmail}");

                Assert.True(actualEmailBody["TRADE_ASSET_NAME"] ==
                    actualCloseTardeDitails[0].asset_symbol,
                    $" expected TRADE ASSET NAME: {actualCloseTardeDitails[0].asset_symbol}" +
                    $" actual TRADE ASSET NAME: {actualEmailBody["TRADE_ASSET_NAME"]}");

                Assert.True(actualEmailBody["TRADE_TYPE"] ==
                    actualCloseTardeDitails[0].transaction_type,
                    $" expected TRADE TYPE: {actualCloseTardeDitails[0].transaction_type}" +
                    $" actual TRADE TYPE: {actualEmailBody["TRADE_TYPE"]}");

                Assert.True(actualEmailBody["TRADE_AMOUNT"] ==
                    actualCloseTardeDitails[0].amount.ToString(),
                    $" expected TRADE AMOUNT: {actualCloseTardeDitails[0].amount}" +
                    $" actual TRADE AMOUNT: {actualEmailBody["TRADE_AMOUNT"]}");

                Assert.True(actualEmailBody["TRADE_OPEN_PRICE"]
                    .Contains(expectedTradeOpenPrice.ToString()),
                    $" expected TRADE OPEN PRICE: {expectedTradeOpenPrice}" +
                    $" actual TRADE OPEN PRICE: {actualEmailBody["TRADE_OPEN_PRICE"]}");

                Assert.True(!actualEmailBody["TRADE_OPEN_TIME"].Contains("undefined")
                    && !actualEmailBody["TRADE_OPEN_TIME"].Contains("null"),
                    $" expected TRADE OPEN_TIME not undefined" +
                    $" actual TRADE OPEN TIME: {actualEmailBody["TRADE_OPEN_TIME"]}");

                Assert.True(actualEmailBody["TRADE_SL"].Contains(expectedCloseAtLoss.ToString()),
                    $" expected TRADE SL: {expectedCloseAtLoss}" +
                    $" actual TRADE SL: {actualEmailBody["TRADE_SL"]}");

                Assert.True(actualEmailBody["TRADE_TP"].Contains(expectedCloseAtProfit.ToString()),
                    $" expected TRADE TP: {expectedCloseAtProfit}" +
                    $" actual TRADE TP: {actualEmailBody["TRADE_TP"]}");

                Assert.True(!actualEmailBody["TRADE_CLOSE_TIME"].Contains("null")
                    && !actualEmailBody["TRADE_CLOSE_TIME"].Contains("undefined"),
                    $" expected TRADE CLOSE TIME not undefined and not null" +
                    $" actual TRADE CLOSE TIME: {actualEmailBody["TRADE_CLOSE_TIME"]}");

                Assert.True(actualEmailBody["TRADE_CLOSE_PRICE"]
                    .Contains(expectedTradeClosePrice.ToString()),
                    $" expected TRADE CLOSE PRICE: {expectedTradeClosePrice}" +
                    $" actual TRADE CLOSE PRICE: {actualEmailBody["TRADE_CLOSE_PRICE"]}" +
                    $" client id: {_clientEmail}");

                Assert.True(actualEmailBody["TRADE_COMMISSION"] ==
                    actualCloseTardeDitails[0].commision.ToString(),
                    $" expected TRADE COMMISSION: {actualCloseTardeDitails[0].commision}" +
                    $" actual TRADE COMMISSION: {actualEmailBody["TRADE_COMMISSION"]}");

                Assert.True(!actualEmailBody["TRADE_MARGIN_M"].Contains("undefined")
                    && !actualEmailBody["TRADE_MARGIN_M"].Contains("null"),
                    $" expected TRADE MARGIN M not undefined" +
                    $" actual TRADE MARGIN M: {actualEmailBody["TRADE_MARGIN_M"]}");

                Assert.True(!actualEmailBody["TRADE_PNL"].Contains("undefined")
                    && !actualEmailBody["TRADE_PNL"].Contains("null"),
                    $" expected deposit amount not undefined" +
                    $" actual deposit amount: {actualEmailBody["TRADE_PNL"]}");

                Assert.True(actualEmailBody["TRADE_CLOSE_REASON"] ==
                    actualCloseTardeDitails[0].close_reason,
                    $" expected TRADE CLOSE REASON: {actualCloseTardeDitails[0].close_reason}" +
                    $" actual TRADE CLOSE REASON: {actualEmailBody["TRADE_CLOSE_REASON"]}");

                Assert.True(actualEmailBody["TRADE_LEVERAGE"] == _tradeLeverage.ToString(),
                    $" expected TRADE LEVERAGE {_tradeLeverage}" +
                    $" actual TRADE LEVERAGE: {actualEmailBody["TRADE_LEVERAGE"]}");

                Assert.True(!actualEmailBody["TRADE_ORDER_PRICE"].Contains("undefined")
                    && !actualEmailBody["TRADE_ORDER_PRICE"].Contains("null"),
                    $" expected TRADE ORDER PRICE not undefined" +
                    $" actual TRADE ORDER PRICE: {actualEmailBody["TRADE_ORDER_PRICE"]}");

                Assert.True(!actualEmailBody["TRADE_CREATE_TIME"].Contains("undefined")
                    && !actualEmailBody["TRADE_CREATE_TIME"].Contains("null"),
                    $" expected TRADE CREATE TIME not undefined" +
                    $" actual TRADE CREATE TIME: {actualEmailBody["TRADE_CREATE_TIME"]}");

                Assert.True(email.Subject == subject,
                    $" expected email subject: {email.Subject}" +
                    $" actual email subject: {subject}");
            });
        }
    }
}