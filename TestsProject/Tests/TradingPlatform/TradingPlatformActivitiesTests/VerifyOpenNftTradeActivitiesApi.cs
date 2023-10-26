using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
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
using System.Collections.Generic;
using System.Linq;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.TradingPlatformActivitiesTests
{
    [TestFixture]
    public class VerifyOpenNftTradeActivitiesApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _clientId;
        private string _clientEmail;
        private string _userName;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;
        private string _tradeId;
        private int _depositAmount = 100000;
        private int _tradeAmount = 1;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
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

            #region buy nft 
            // create trade 
            var tradeDetails = _apiFactory
                .ChangeContext<INftPageApi>()
                .PostBuyNftRequest(_tradingPlatformUrl, _loginData)
                .GeneralResponse;

            _tradeId = tradeDetails.TradeId;
            #endregion
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
        public void VerifyOpenNftTradeActivityApiTest()
        {
            var clientName = _clientEmail.Split('@').First();
            var expectedAssetSymbol = DataRep.AssetNftLongSymbol;
            var expectedAssetLabel = DataRep.AssetNftSymbol;
            var expectedTransactionType = "buy";
            var expectedAmount = _tradeAmount;
            var expectedStatus = "open";
            var expectedPlatform = "cfd";
            var expectedTradeId = _tradeId;
            var expectedClientId = _clientId;
            var expectedClientFullName = $"{clientName} {clientName}";
            var expectedType = "open_trade";
            var expectedSystemType = "NFT";
            var expectedActionMadeBy = clientName + clientName;
            var expectedActionMadeByClientId = _clientId;

            var actualOpenTrade = _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForActivityToRegister(_tradingPlatformUrl, expectedType, _loginData)
                .GetActivities(_tradingPlatformUrl, _loginData)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualOpenTrade.asset_symbol == expectedAssetSymbol,
                    $" expected Asset Symbol: {expectedAssetSymbol}" +
                    $" actual Asset Symbol: {actualOpenTrade.asset_symbol}");

                Assert.True(actualOpenTrade.asset_label == expectedAssetLabel,
                    $" expected Asset Label: {expectedAssetLabel}" +
                    $" actual Asset Label: {actualOpenTrade.asset_label}");

                Assert.True(actualOpenTrade.transaction_type == expectedTransactionType,
                    $" expected transaction type : {expectedTransactionType}" +
                    $" actual transaction type : {actualOpenTrade.transaction_type}");

                Assert.True(actualOpenTrade.amount == expectedAmount,
                    $" expected amount : {expectedAmount}" +
                    $" actual amount : {actualOpenTrade.amount}");

                Assert.True(actualOpenTrade.status == expectedStatus,
                    $" expected status: {expectedStatus}" +
                    $" actual status: {actualOpenTrade.status}");

                Assert.True(actualOpenTrade.platform == expectedPlatform,
                    $" expected Platform: {expectedPlatform}" +
                    $" actual :Platform: {actualOpenTrade.platform}");

                Assert.True(actualOpenTrade.trade_id == expectedTradeId,
                    $" expected Trade Id: {expectedTradeId}" +
                    $" actual Trade Id: {actualOpenTrade.trade_id}");

                Assert.True(actualOpenTrade.user_id == expectedClientId,
                    $" expected user id : {expectedClientId}" +
                    $" actual user id : {actualOpenTrade.user_id}");

                Assert.True(actualOpenTrade.user_full_name == expectedClientFullName,
                    $" expected Client Full Name: {expectedClientFullName}" +
                    $" actual Client Full Name: {actualOpenTrade.user_full_name}");

                Assert.True(actualOpenTrade.type == expectedType,
                    $" expected Type: {expectedType}" +
                    $" actual Type: {actualOpenTrade.type}");

                Assert.True(actualOpenTrade.system_type == expectedSystemType,
                    $" expected system Type: {expectedSystemType}" +
                    $" actual system Type: {actualOpenTrade.type}");

                Assert.True(actualOpenTrade.action_made_by == expectedActionMadeBy,
                    $" expected Action Made By: {expectedActionMadeBy}" +
                    $" actual Action Made By: {actualOpenTrade.action_made_by}");

                Assert.True(actualOpenTrade.action_made_by_user_id == expectedActionMadeByClientId,
                    $" expected Action Made By Client Id : {expectedActionMadeByClientId}" +
                    $" actual Action Made By Client Id: {actualOpenTrade.action_made_by_user_id}");
            });
        }
    }
}