using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using ConsoleApp;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.TradingPlatformActivitiesTests
{
    [TestFixture]
    public class VerifyOpenPendingTradeActivityApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private string _clientEmail;
        private string _userName;
        private string _userId;
        private string _pendingTradeId;
        private int _depositAmount = 10000;
        private int _tradeAmount = 10;
        private double _currentRate;
        private double _rateForPending;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private GetLoginResponse _loginData;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            // create user
            _userName = TextManipulation.RandomString();

            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName,
             role: DataRep.AdminWithUsersOnlyRoleName);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            #region login data 
            // login data
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;
            #endregion

            #region deposit 
            // deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount);
            #endregion

            #region create trade 
            // create trade 
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount, _loginData)
                .GeneralResponse;
            #endregion

            _currentRate = tradeDetails.TradeRate;
            _rateForPending = _currentRate + 100; // to open a pending trade

            // create pending trade
            var pendingTrade = _apiFactory
               .ChangeContext<ITradePageApi>()
               .PostPendingBuyOrderRequest(_tradingPlatformUrl,
               _tradeAmount, _loginData, _rateForPending);

            _pendingTradeId = pendingTrade.TradeId;
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
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyOpenPendingTradeActivityApiTest()
        {
            var clientName = _clientEmail.Split('@').First();
            var expectedAssetSymbol = DataRep.AssetName;
            var expectedAssetLabel = DataRep.AssetNameShort;
            var expectedTransactionType = "buy";
            var expectedAmount = _tradeAmount;
            var expectedStatus = "pending";
            var expectedPlatform = "cfd";
            var expectedCurrentRate = _currentRate.MathRoundFromGeneric(2, MidpointRounding.ToEven);
            var expectedRateForPending = _rateForPending.MathRoundFromGeneric(5);
            var expectedpendingTradeId = _pendingTradeId;
            var expectedType = "open_trade_pending";
            var expectedClientId = _clientId;
            var expectedClientFullName = $"{clientName} {clientName}";
            var expectedActionMadeBy = clientName + clientName;
            var expectedActionMadeByClientId = _clientId;

            var actualOpenTradePending = _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForActivityToRegister(_tradingPlatformUrl, expectedType, _loginData)
                .GetActivities(_tradingPlatformUrl, _loginData)
                .FirstOrDefault();

            var currentRate = actualOpenTradePending.current_rate
                .MathRoundFromGeneric(2, MidpointRounding.ToEven);

            var actualCurrentRate = (currentRate - expectedCurrentRate)
                .MathAbsGeneric();

            Assert.Multiple(() =>
            {
                Assert.True(actualOpenTradePending.asset_symbol == expectedAssetSymbol,
                   $" expected Asset Symbol: {expectedAssetSymbol}" +
                   $" actual Asset Symbol: {actualOpenTradePending.asset_symbol}");

                Assert.True(actualOpenTradePending.asset_label == expectedAssetLabel,
                   $" expected Asset Label: {expectedAssetLabel}" +
                   $" actual Asset Label: {actualOpenTradePending.asset_label}");

                Assert.True(actualOpenTradePending.transaction_type == expectedTransactionType,
                   $" expected transaction type : {expectedTransactionType}" +
                   $" actual transaction type : {actualOpenTradePending.transaction_type}");

                Assert.True(actualOpenTradePending.amount == expectedAmount,
                   $" expected amount : {expectedAmount}" +
                   $" actual amount : {actualOpenTradePending.amount}");

                Assert.True(actualOpenTradePending.status == expectedStatus,
                   $" expected status : {expectedStatus}" +
                   $" actual status: {actualOpenTradePending.status}");

                Assert.True(actualOpenTradePending.platform == expectedPlatform,
                   $" expected Platform: {expectedPlatform}" +
                   $" actual :Platform: {actualOpenTradePending.platform}");

                Assert.True((currentRate - expectedCurrentRate).MathAbsGeneric() < 1,
                   $" expected current rate: {expectedCurrentRate}" +
                   $" actual current rate: {currentRate}");

                Assert.True(actualOpenTradePending.open_on_rate.ToString()
                    .Contains(expectedRateForPending.ToString()),
                   $" expected open on rate: {expectedRateForPending}" +
                   $" actual open on rate: {actualOpenTradePending.open_on_rate}");

                Assert.True(actualOpenTradePending.trade_id == expectedpendingTradeId,
                   $" expected Trade Id: {expectedpendingTradeId}" +
                   $" actual Trade Id: {actualOpenTradePending.trade_id}");

                Assert.True(actualOpenTradePending.user_id == expectedClientId,
                   $" expected user id : {expectedAmount}" +
                   $" actual user id : {actualOpenTradePending.user_id}");

                Assert.True(actualOpenTradePending.user_full_name == expectedClientFullName,
                   $" expected Client Full Name: {expectedClientFullName}" +
                   $" actual Client Full Name: {actualOpenTradePending.user_full_name}");

                Assert.True(actualOpenTradePending.type == expectedType,
                   $" expected Type: {expectedType}" +
                   $" actual Type: {actualOpenTradePending.type}");

                Assert.True(actualOpenTradePending.action_made_by == expectedActionMadeBy,
                    $" expected Action Made By: {expectedActionMadeBy}" +
                    $" actual Action Made By: {actualOpenTradePending.action_made_by }");

                Assert.True(actualOpenTradePending.action_made_by_user_id == expectedActionMadeByClientId,
                    $" expected Action Made By Client Id : {expectedActionMadeByClientId}" +
                    $" actual Action Made By Client Id: {actualOpenTradePending.action_made_by_user_id}");
            });
        }
    }
}