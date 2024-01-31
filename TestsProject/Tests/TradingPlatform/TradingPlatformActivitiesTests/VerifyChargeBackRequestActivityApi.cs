// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
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
    public class VerifyChargeBackRequestActivityApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private string _clientEmail;
        private string _userName;
        private string _userId;
        private int _chargebackId;
        private string _depositId;
        private int _depositAmount = 100;
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

            // deposit 200
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount, apiKey: currentUserApiKey);

            // get deposit id by amount
            _depositId =
              (from s in _dbContext.FundsTransactions
               where (s.UserId == _clientId && s.OriginalAmount == _depositAmount && s.Type == "deposit")
               select s.Id).First().ToString();

            // chargeback
            _chargebackId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .DeleteChargeBackDepositRequest(_crmUrl, _clientId, _depositAmount, _depositId, apiKey: currentUserApiKey);
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
        public void VerifyChargeBackRequestActivityApiTest()
        {
            var clientName = _clientEmail.Split('@').First();
            var expectedErpUserId = _userId;
            var expectedErpUserName = _userName;
            var expectedClientId = _clientId;
            var expectedClientFullName = $"{clientName} {clientName}";
            var expectedAmount = _depositAmount;
            var expectedTransactionId = _depositId;
            var expectedTransactionType = "deposit"; // ask shai
            var expectedCurrency = DataRep.DefaultUSDCurrencyName;
            var expectedStatus = "approved";
            var expectedType = "chargeback";                        
            var expectedActionMadeBy = _userName;
            var expectedActionMadeByUserId = _userId;

            var actualChargebackRequest = _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForActivityToRegister(_tradingPlatformUrl, expectedType, _loginData)
                .GetActivities(_tradingPlatformUrl, _loginData)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualChargebackRequest.erp_user_id == expectedErpUserId,
                    $" expected erp user id: {expectedErpUserId}" +
                    $" actual erp user id: {actualChargebackRequest.erp_user_id }");

                Assert.True(actualChargebackRequest.erp_username == expectedErpUserName,
                    $" expected erp username : {expectedErpUserName}" +
                    $" actual erp username: {actualChargebackRequest.erp_username}");

                Assert.True(actualChargebackRequest.user_id == expectedClientId,
                    $" expected Client Id : {expectedClientId}" +
                    $" actual Client Id: {actualChargebackRequest.user_id}");

                Assert.True(actualChargebackRequest.user_full_name == expectedClientFullName,
                    $" expected user full name : {expectedClientFullName}" +
                    $" actual user full name: {actualChargebackRequest.user_full_name}");             

                Assert.True(actualChargebackRequest.amount == expectedAmount,
                    $" expected  amount : {expectedAmount}" +
                    $" actual  amount: {actualChargebackRequest.amount}");

                Assert.True(actualChargebackRequest.transaction_id == expectedTransactionId,
                    $" expected transaction id: {expectedTransactionId}" +
                    $" actual transaction id: {actualChargebackRequest.transaction_id}");

                Assert.True(actualChargebackRequest.transaction_type == expectedTransactionType,
                    $" expected transaction type: {expectedTransactionType}" +
                    $" actual transaction type: {actualChargebackRequest.transaction_type}");

                Assert.True(actualChargebackRequest.currency == expectedCurrency,
                    $" expected currency: {expectedCurrency}" +
                    $" actual currency: {actualChargebackRequest.currency}");

                Assert.True(actualChargebackRequest.status == expectedStatus,
                    $" expected status: {expectedStatus}" +
                    $" actual status: {actualChargebackRequest.status}");

                Assert.True(actualChargebackRequest.type == expectedType,
                    $" expected type: {expectedType}" +
                    $" actual type: {actualChargebackRequest.type}");

                Assert.True(actualChargebackRequest.action_made_by == expectedActionMadeBy,
                    $" expected Action Made By: {expectedActionMadeBy}" +
                    $" actual Action Made By: {actualChargebackRequest.action_made_by }");

                Assert.True(actualChargebackRequest.action_made_by_user_id == expectedActionMadeByUserId,
                    $" expected Action Made By user Id : {expectedActionMadeByUserId}" +
                    $" actual Action Made By user Id: {actualChargebackRequest.action_made_by_user_id}");
            });
        }
    }
}