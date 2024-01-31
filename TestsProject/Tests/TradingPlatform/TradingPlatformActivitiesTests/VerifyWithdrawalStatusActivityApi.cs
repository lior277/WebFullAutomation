// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
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
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.TradingPlatformActivitiesTests
{
    [TestFixture]
    public class VerifyWithdrawalStatusActivityApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private string _clientEmail;
        private string _userName;
        private string _userId;
        private string _withdrawalId;
        private int _depositAmount = 100;
        private int _withdrawalAmount = 30;
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
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId, clientsIds);
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
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId,
                _depositAmount, apiKey: currentUserApiKey);
            #endregion

            #region create pending withdrawal 
            // create pending withdrawal
            _apiFactory
                .ChangeContext<IWithdrawalTpApi>()
                .PostPendingWithdrawalRequest(_tradingPlatformUrl, _loginData, _withdrawalAmount);
            #endregion

            #region procceed withdrawal
            // get withdrawal id
            _withdrawalId =
                (from s in _dbContext.FundsTransactions
                 where (s.UserId == _clientId && Math.Abs(s.Amount)
                 == _withdrawalAmount && s.Type == "withdrawal" && s.Status == "pending")
                 select s.Id).First()
                 .ToString();

            // procceed withdrawal
            _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PatchWithdrawalStatusRequest(_crmUrl, _clientId, _withdrawalId,
                withdrawalStatus: "in_process", apiKey: currentUserApiKey);
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
        public void VerifyWithdrawalStatusActivityApiTest()
        {
            var clientName = _clientEmail.Split('@').First();            
            var expectedClientId = _clientId;
            var expectedClientFullName = $"{clientName} {clientName}";
            var expectedErpUserId = _userId;
            var expectedErpUserName = _userName;
            var expectedErpNewStatus = "in_process";
            var expectedErpOldStatus = "pending";
            var expectedAmount = -30;
            var expectedWithdrawalId = _withdrawalId;
            var expectedType = "withdrawal_status";       
            var expectedActionMadeBy = _userName;
            var expectedActionMadeByUserId = _userId;

            var actualWithdrawalStatus = _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForActivityToRegister(_tradingPlatformUrl, expectedType, _loginData)
                .GetActivities(_tradingPlatformUrl, _loginData)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualWithdrawalStatus.user_id == expectedClientId,
                    $" expected Client Id : {expectedClientId}" +
                    $" actual Client Id: {actualWithdrawalStatus.user_id}");

                Assert.True(actualWithdrawalStatus.user_full_name == expectedClientFullName,
                    $" expected user full name : {expectedClientFullName}" +
                    $" actual user full name: {actualWithdrawalStatus.user_full_name}");

                Assert.True(actualWithdrawalStatus.erp_user_id == expectedErpUserId,
                    $" expected erp user id: {expectedErpUserId}" +
                    $" actual erp user id: {actualWithdrawalStatus.erp_user_id }");

                Assert.True(actualWithdrawalStatus.erp_username == expectedErpUserName,
                    $" expected erp username : {expectedErpUserName}" +
                    $" actual erp username: {actualWithdrawalStatus.erp_username}");

                Assert.True(actualWithdrawalStatus.new_status == expectedErpNewStatus,
                    $" expected new status: {expectedErpNewStatus}" +
                    $" actual new status: {actualWithdrawalStatus.new_status }");

                Assert.True(actualWithdrawalStatus.old_status == expectedErpOldStatus,
                    $" expected old status : {expectedErpOldStatus}" +
                    $" actual old status: {actualWithdrawalStatus.old_status}");

                Assert.True(actualWithdrawalStatus.amount == expectedAmount,
                    $" expected  amount : {expectedAmount}" +
                    $" actual  amount: {actualWithdrawalStatus.amount}");

                Assert.True(actualWithdrawalStatus.withdrawal_id == expectedWithdrawalId,
                    $" expected withdrawal id: {expectedWithdrawalId}" +
                    $" actual withdrawal id: {actualWithdrawalStatus.withdrawal_id}");

                Assert.True(actualWithdrawalStatus.type == expectedType,
                    $" expected type: {expectedType}" +
                    $" actual type: {actualWithdrawalStatus.type}");

                Assert.True(actualWithdrawalStatus.action_made_by == expectedActionMadeBy,
                    $" expected Action Made By: {expectedActionMadeBy}" +
                    $" actual Action Made By: {actualWithdrawalStatus.action_made_by }");

                Assert.True(actualWithdrawalStatus.action_made_by_user_id == expectedActionMadeByUserId,
                    $" expected Action Made By user Id : {expectedActionMadeByUserId}" +
                    $" actual Action Made By user Id: {actualWithdrawalStatus.action_made_by_user_id}");
            });
        }
    }
}