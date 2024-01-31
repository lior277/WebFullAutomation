// Ignore Spelling: Chrono Api

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.TradingPlatformActivitiesTests
{
    [TestFixture]
    public class VerifyCloseChronoActivitiesApi : TestSuitBase
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
        private int _depositAmount = 1000;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
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

            #region create chrono trade 
            // create chrono trade 
            var tradeDetails = _apiFactory
                .ChangeContext<IChronoTradePageApi>()
                .PostSellChronoAssetApi(_tradingPlatformUrl, _loginData);

            // wait 30 seconds for the chrono trade to close
            Thread.Sleep(TimeSpan.FromSeconds(33));

            _tradeId = tradeDetails.TradeId;

            _apiFactory
                .ChangeContext<IChronoTradePageApi>()
                .WaitForChronoTradeToClose(_tradingPlatformUrl,
                _tradeId, _loginData);
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
        public void VerifyCloseChronoActivityApiTest()
        {
            var clientName = _clientEmail.Split('@').First();
            var expectedClientFullName = $"{clientName} {clientName}";
            var expectedTradeId = _tradeId;
            var expectedAssetSymbol = "ETHUSD";
            var expectedPlatform = "cfd";
            var expectedCloseReason = "time_limit_ended";
            var expectedAssetLabel = "ETHUSD";
            var expectedType = "close_trade";
            var expectedActionMadeByClientId = _clientId;

            var actualActivitie = _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForActivityToRegister(_tradingPlatformUrl, expectedType, _loginData)
                .GetActivities(_tradingPlatformUrl, _loginData)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {              
                Assert.True(actualActivitie.user_full_name == expectedClientFullName,
                    $" expected Client Full Name: {expectedClientFullName}" +
                    $" actual Client Full Name: {actualActivitie.user_full_name}");

                Assert.True(actualActivitie.trade_id == expectedTradeId,
                    $" expected Trade Id: {expectedTradeId}" +
                    $" actual Trade Id: {actualActivitie.trade_id}");

                Assert.True(actualActivitie.asset_symbol == expectedAssetSymbol,
                    $" expected Asset Symbol: {expectedAssetSymbol}" +
                    $" actual Asset Symbol: {actualActivitie.asset_symbol}");

                Assert.True(actualActivitie.platform == expectedPlatform,
                    $" expected Platform: {expectedPlatform}" +
                    $" actual :Platform: {actualActivitie.platform}");

                Assert.True(actualActivitie.close_reason == expectedCloseReason,
                    $" expected Close Reason: {expectedCloseReason}" +
                    $" actual Close Reason:{actualActivitie.close_reason}");

                Assert.True(actualActivitie.asset_label == expectedAssetLabel,
                    $" expected Asset Label: {expectedAssetLabel}" +
                    $" actual Asset Label: {actualActivitie.asset_label}");

                Assert.True(actualActivitie.type == expectedType,
                    $" expected Type: {expectedType}" +
                    $" actual Type: {actualActivitie.type}");

                Assert.True(actualActivitie.user_id == expectedActionMadeByClientId,
                    $" expected Action Made By Client Id : {expectedActionMadeByClientId}" +
                    $" actual Action Made By Client Id: {actualActivitie.action_made_by_user_id}");
            });
        }
    }
}