// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture]
    public class VerifyOpenTradeValuesApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private GetLoginResponse _loginData;
        private string _clientEmail;
        private string _clientId;
        private int _openTradeAmount = 1;
        private double _commision = 0.01;
        private int _bonusAmount = 10000;
        private string _tradeGroupId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var tradeGroupttributes = new Default_Attr
            {
                commision = _commision,
                leverage = 1,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0.01,
                margin_call = null,
                swap_long = 0,
                swap_short = 0,
                swap_time = "12:00:00",
            };

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                 .ChangeContext<ICreateClientApi>()
                 .CreateClientRequest(_crmUrl, clientName);

            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, _bonusAmount);

            // get login Data for trading Platform
            _loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            var groupData = _apiFactory // Create Cripto Group And Assign It To client
               .ChangeContext<ISharedStepsGenerator>()
               .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, tradeGroupttributes, _clientId);

            _tradeGroupId = groupData.Keys.First();

            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl,
                amount: _openTradeAmount, loginData: _loginData);
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
                AfterTest();
            }
        }
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyOpenTradeValuesApiTest()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");

            var actualClientByIdResponse = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, _clientId)
                .GeneralResponse
                .user;

            var openTradesDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetTradesByStatusRequest(_tradingPlatformUrl, _loginData, "open")
                .GeneralResponse
                .First();

            var expectedEquity = (actualClientByIdResponse.balance
                + openTradesDetails.profit_loss)
                .MathRoundFromGeneric(0);

            //the maintains margin use 100% in the trade group
            var expectedMinMagin = (openTradesDetails.investment * 1)
                .MathRoundFromGeneric(2);

            //the concussion rate in the trade group
            var expectedComossion = (openTradesDetails.investment * _commision)
                .MathRoundFromGeneric(2);

            var expectedProfit = ((openTradesDetails.rate - openTradesDetails.current_rate)
                .MathAbsGeneric() * openTradesDetails.amount) + openTradesDetails.commision;

            var expectedAvailable = (_bonusAmount -
                 openTradesDetails.min_margin - openTradesDetails.profit_loss
                 .MathAbsGeneric())
                 .MathRoundFromGeneric(0);

            var actualEquity = actualClientByIdResponse.equity.MathRoundFromGeneric(0);

            Assert.Multiple(() =>
            {
                Assert.True(actualClientByIdResponse.balance == _bonusAmount,
                    $" expected balance: {_bonusAmount}" +
                    $" actual balance: {actualClientByIdResponse.balance}");

                Assert.True(actualEquity == expectedEquity,
                    $" expected equity : {expectedEquity}" +
                    $" actual equity: {actualEquity}");

                Assert.True(openTradesDetails.min_margin.MathRoundFromGeneric(2)
                    == expectedMinMagin,
                    $" expected min margin: {expectedMinMagin}" +
                    $" actual min margin: {openTradesDetails.min_margin}");

                Assert.True(openTradesDetails.commision.MathRoundFromGeneric(2)
                    .Equals(expectedComossion),
                    $" expected commission: {expectedComossion}, " +
                    $" actual commission: {openTradesDetails.commision}");

                var expected = ((openTradesDetails.profit_loss).MathAbsGeneric() - expectedProfit)
                .MathAbsGeneric() < 1;

                Assert.True(expected,
                    $" expected profit loss: {(openTradesDetails.profit_loss - expectedProfit).MathAbsGeneric()} " +
                    $" actual profit loss: the difference us more then one");

                var diferance = Math.Abs(actualClientByIdResponse
                    .available.MathRoundFromGeneric(0) - expectedAvailable);

                Assert.True(diferance < 1,
                    $" expected  available: {expectedAvailable} " +
                    $" actual Available: {actualClientByIdResponse.available}");
            });
        }
    }
}