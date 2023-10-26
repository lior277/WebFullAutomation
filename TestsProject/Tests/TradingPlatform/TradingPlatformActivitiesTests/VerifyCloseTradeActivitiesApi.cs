using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;

using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
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
    public class VerifyCloseTradeActivitiesApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private string _clientEmail;
        private string _userName;
        private string _userId;
        private string _tradeId;
        private int _depositAmount = 10000;
        private int _tradeAmount = 10;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private GetLoginResponse _loginData;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            _apiFactory = new ApplicationFactory();
            _crmUrl = Config.appSettings.CrmUrl;
            _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;

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
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount, _loginData )
                .GeneralResponse;
            #endregion

            #region close trade 
            // close trade 
            _tradeId = tradeDetails.TradeId;

            _apiFactory
              .ChangeContext<ITradePageApi>()
              .PatchCloseTradeWithAmountRequest(_tradingPlatformUrl,
              _tradeId, _loginData, _tradeAmount);

            // wait for trade to close
            _apiFactory
             .ChangeContext<ITradePageApi>()
             .WaitForCfdTradeToClose(_tradingPlatformUrl, _tradeId, _loginData);
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
        public void VerifyCloseTradeActivityApiTest()
        {
            var clientName = _clientEmail.Split('@').First();
            var expectedClientId = _clientId;
            var expectedClientFullName = $"{clientName} {clientName}";
            var expectedTradeId = _tradeId;
            var expectedAssetSymbol = DataRep.AssetName;
            var expectedPlatform = "cfd";
            var expectedCloseReason = "user_closed";
            var expectedAssetLabel = DataRep.AssetNameShort;
            var expectedType = "close_trade";
            var expectedActionMadeBy = clientName + clientName;
            var expectedActionMadeByClientId = _clientId;

            var actualCloseTrade = _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForActivityToRegister(_tradingPlatformUrl, expectedType, _loginData)
                .GetActivities(_tradingPlatformUrl, _loginData)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualCloseTrade.user_id == expectedClientId,
                    $" expected Client Id : {expectedClientId}" +
                    $" actual Client Id: {actualCloseTrade.user_id}");

                Assert.True(actualCloseTrade.user_full_name == expectedClientFullName,
                    $" expected Client Full Name: {expectedClientFullName}" +
                    $" actual Client Full Name: {actualCloseTrade.user_full_name}");

                Assert.True(actualCloseTrade.trade_id == expectedTradeId,
                    $" expected Trade Id: {expectedTradeId}" +
                    $" actual Trade Id: {actualCloseTrade.trade_id}");

                Assert.True(actualCloseTrade.asset_symbol == expectedAssetSymbol,
                    $" expected Asset Symbol: {expectedAssetSymbol}" +
                    $" actual Asset Symbol: {actualCloseTrade.asset_symbol}");

                Assert.True(actualCloseTrade.platform == expectedPlatform,
                    $" expected Platform: {expectedPlatform}" +
                    $" actual :Platform: {actualCloseTrade.platform}");

                Assert.True(actualCloseTrade.close_reason == expectedCloseReason,
                    $" expected Close Reason: {expectedCloseReason}" +
                    $" actual Close Reason:{actualCloseTrade.close_reason}");

                Assert.True(actualCloseTrade.asset_label == expectedAssetLabel,
                    $" expected Asset Label: {expectedAssetLabel}" +
                    $" actual Asset Label: {actualCloseTrade.asset_label}");

                Assert.True(actualCloseTrade.type == expectedType,
                    $" expected Type: {expectedType}" +
                    $" actual Type: {actualCloseTrade.type}");

                Assert.True(actualCloseTrade.action_made_by == expectedActionMadeBy,
                    $" expected Action Made By: {expectedActionMadeBy}" +
                    $" actual Action Made By: {actualCloseTrade.action_made_by }");

                Assert.True(actualCloseTrade.action_made_by_user_id == expectedActionMadeByClientId,
                    $" expected Action Made By Client Id : {expectedActionMadeByClientId}" +
                    $" actual Action Made By Client Id: {actualCloseTrade.action_made_by_user_id}");
            });
        }
    }
}