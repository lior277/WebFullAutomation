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
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.TradingPlatformActivitiesTests
{
    [TestFixture]
    public class VerifyAddDepositActivitiesApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private string _clientEmail;
        private string _userName;
        private string _userId;
        private string _depositId;
        private int _depositAmount = 1000;
        private GetLoginResponse _loginData;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

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

            var clientsIds = new List<string> { _clientId };

            #region connect One User To One Client 
            // connect One User To One Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                clientsIds);
            #endregion

            #region login data 
            // login data
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;
            #endregion

            #region deposit 
            // deposit 
            _depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId,
                _depositAmount, apiKey: currentUserApiKey);
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
        public void VerifyAddDepositActivityApiTest()
        {
            var clientName = _clientEmail.Split('@').First();
            var expectedClientId = _clientId;
            var expectedClientFullName = $"{clientName} {clientName}";
            var expectedErpUserIdAssigned = _userId;
            var expectedOriginalAmount = _depositAmount;
            var expectedOriginalCurrency = DataRep.DefaultUSDCurrencyName;
            var expectedDepositAmount = _depositAmount;
            var expectedCurrency = DataRep.DefaultUSDCurrencyName;
            var expectedTransactionId = _depositId;
            var expectedErpUserId = _userId;
            var expectedErpUserName = _userName;
            var expectedType = "add_deposit";                             
            var expectedActionMadeBy = _userName;
            var expectedActionMadeByUserId = _userId;

            var actualAddDeposit = _apiFactory
                .ChangeContext<ITradePageApi>()
               .WaitForActivityToRegister(_tradingPlatformUrl, expectedType, _loginData)
                .GetActivities(_tradingPlatformUrl, _loginData)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualAddDeposit.user_id == expectedClientId,
                    $" expected Client Id : {expectedClientId}" +
                    $" actual Client Id: {actualAddDeposit.user_id}");

                Assert.True(actualAddDeposit.user_full_name == expectedClientFullName,
                    $" expected user full name : {expectedClientFullName}" +
                    $" actual user full name: {actualAddDeposit.user_full_name}");

                Assert.True(actualAddDeposit.erp_user_id_assigned == expectedErpUserIdAssigned,
                    $" expected erp user id assigned : {expectedErpUserIdAssigned}" +
                    $" actual erp user id assigned: {actualAddDeposit.erp_user_id_assigned}");

                Assert.True(actualAddDeposit.original_amount == expectedOriginalAmount,
                    $" expected original amount: {expectedOriginalAmount}" +
                    $" actual original amount: {actualAddDeposit.original_amount}");

                Assert.True(actualAddDeposit.original_currency == expectedOriginalCurrency,
                    $" expected original currency: {expectedOriginalCurrency}" +
                    $" actual original currency: {actualAddDeposit.original_currency}");

                Assert.True(actualAddDeposit.amount == expectedDepositAmount,
                    $" expected amount : {expectedDepositAmount}" +
                    $" actual amount: {actualAddDeposit.amount}");

                Assert.True(actualAddDeposit.currency == expectedCurrency,
                    $" expected currency: {expectedCurrency}" +
                    $" actual currency: {actualAddDeposit.currency}");

                Assert.True(actualAddDeposit.original_currency == expectedOriginalCurrency,
                    $" expected original currency: {expectedOriginalCurrency}" +
                    $" actual original currency: {actualAddDeposit.original_currency}");

                Assert.True(actualAddDeposit.transaction_id == expectedTransactionId,
                    $" expected transaction id: {expectedTransactionId}" +
                    $" actual transaction id: {actualAddDeposit.transaction_id}");

                Assert.True(actualAddDeposit.erp_user_id == expectedErpUserId,
                    $" expected erp user id: {expectedErpUserId}" +
                    $" actual erp user id: {actualAddDeposit.erp_user_id }");

                Assert.True(actualAddDeposit.erp_username == expectedErpUserName,
                    $" expected erp username : {expectedErpUserName}" +
                    $" actual erp username: {actualAddDeposit.erp_username}");

                Assert.True(actualAddDeposit.type == expectedType,
                    $" expected type: {expectedType}" +
                    $" actual type: {actualAddDeposit.type}");           

                Assert.True(actualAddDeposit.action_made_by == expectedActionMadeBy,
                    $" expected Action Made By: {expectedActionMadeBy}" +
                    $" actual Action Made By: {actualAddDeposit.action_made_by }");

                Assert.True(actualAddDeposit.action_made_by_user_id == expectedActionMadeByUserId,
                    $" expected Action Made By user Id : {expectedActionMadeByUserId}" +
                    $" actual Action Made By user Id: {actualAddDeposit.action_made_by_user_id}");
            });
        }
    }
}