// Ignore Spelling: Api

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
    public class VerifyAddBonusActivitiesApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private string _clientEmail;
        private string _userName;
        private string _userId;
        private string _bonusId;
        private int _bonusAmount = 100;
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

            #region create ApiKey
            // create ApiKey
            var  currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

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

            #region bonus 
            // bonus 
            _bonusId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, _bonusAmount, apiKey: currentUserApiKey)
                .GeneralResponse
                .InsertId;
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
        public void VerifyAddBonusActivityApiTest()
        {
            var clientName = _clientEmail.Split('@').First();
            var expectedClientId = _clientId;
            var expectedClientFullName = $"{clientName} {clientName}";
            var expectedBonusAmount = _bonusAmount;
            var expectedCurrency = DataRep.DefaultUSDCurrencyName;
            var expectedTransactionId = _bonusId;
            var expectedType = "add_bonus";       
            var expectedErpUserId = _userId;
            var expectedErpUserName = _userName;
            var expectedActionMadeBy = _userName;
            var expectedActionMadeByUserId = _userId;

            var actualAddBonus = _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForActivityToRegister(_tradingPlatformUrl, expectedType, _loginData)
                .GetActivities(_tradingPlatformUrl, _loginData)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualAddBonus.user_id == expectedClientId,
                    $" expected Client Id : {expectedClientId}" +
                    $" actual Client Id: {actualAddBonus.user_id}");

                Assert.True(actualAddBonus.user_full_name == expectedClientFullName,
                    $" expected user full name : {expectedClientFullName}" +
                    $" actual user full name: {actualAddBonus.user_full_name}");

                Assert.True(actualAddBonus.bonus_amount == expectedBonusAmount,
                    $" expected amount : {expectedBonusAmount}" +
                    $" actual amount: {actualAddBonus.amount}");

                Assert.True(actualAddBonus.currency == expectedCurrency,
                    $" expected currency: {expectedCurrency}" +
                    $" actual currency: {actualAddBonus.currency}");

                Assert.True(actualAddBonus.transaction_id == expectedTransactionId,
                    $" expected transaction id: {expectedTransactionId}" +
                    $" actual transaction id: {actualAddBonus.transaction_id}");

                Assert.True(actualAddBonus.type == expectedType,
                    $" expected type: {expectedType}" +
                    $" actual type: {actualAddBonus.type}");

                Assert.True(actualAddBonus.erp_user_id == expectedErpUserId,
                    $" expected erp user id: {expectedErpUserId}" +
                    $" actual erp user id: {actualAddBonus.erp_user_id }");

                Assert.True(actualAddBonus.erp_username == expectedErpUserName,
                    $" expected erp username : {expectedErpUserName}" +
                    $" actual erp username: {actualAddBonus.erp_username}");

                Assert.True(actualAddBonus.action_made_by == expectedActionMadeBy,
                    $" expected Action Made By: {expectedActionMadeBy}" +
                    $" actual Action Made By: {actualAddBonus.action_made_by }");

                Assert.True(actualAddBonus.action_made_by_user_id == expectedActionMadeByUserId,
                    $" expected Action Made By user Id : {expectedActionMadeByUserId}" +
                    $" actual Action Made By user Id: {actualAddBonus.action_made_by_user_id}");
            });
        }
    }
}