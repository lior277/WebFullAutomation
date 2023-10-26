using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using ConsoleApp;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.TradingPlatformActivitiesTests
{
    [TestFixture]
    public class VerifySavingAccountActivityApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private string _clientEmail;
        private string _userName;
        private string _userId;
        private string _depositId;
        private string _transferToSaActivitie = "transfer_to_sa";
        private GetActivitiesResponse _actualTransferToSa;
        private int _transferAmount = 5;
        private int _profit = 10;
        private int _saBalance;
        private int _depositAmount = 1000;
        private GetLoginResponse _loginData;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var dbContext = new QaAutomation01Context();

            // create user
            _userName = TextManipulation.RandomString();

            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName,
             role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            var currentUserApiKey = _apiFactory
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

            // CRM Transfer To Saving Account
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_crmUrl, _clientId, _transferAmount,
                currentUserApiKey);

            // get Transfer To Sa data
            _actualTransferToSa = _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForActivityToRegister(_tradingPlatformUrl,
                _transferToSaActivitie, _loginData)
                .GetActivities(_tradingPlatformUrl, _loginData)
                .Where(p => p.type == _transferToSaActivitie)
                .FirstOrDefault();

            _saBalance = _transferAmount + _profit;

            // create profit
            _apiFactory
                .ChangeContext<ISATabApi>()
                .CreateSaProfit(_crmUrl, _actualTransferToSa,
                _profit, _saBalance);

            // CRM Transfer To Balance
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToBalanceRequest(_crmUrl, _clientId,
                _transferAmount, currentUserApiKey); 
            
            Thread.Sleep(1000); // wait for Transfer To Balance
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
        public void VerifySavingAccountActivityApiTest()
        {
            var transferToBalanceActivitie = "transfer_to_balance";
            var clientName = _clientEmail.Split('@').First();
            var expectedClientId = _clientId;
            var expectedClientFullName = $"{clientName} {clientName}";
            var expectedTransferAmount = _transferAmount;
            var expectedCurrency = DataRep.DefaultUSDCurrencyName;
            var expectedErpUserId = _userId;
            var expectedErpUserName = _userName;
            var expectedTransferToBalanceType = "transfer_to_balance";
            var expectedTransferToSaType = "transfer_to_sa";
            var expectedActionMadeBy = _userName;
            var expectedActionMadeByUserId = _userId;

            var actualTransferToBalance = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetActivities(_tradingPlatformUrl, _loginData)
                .Where(p => p.type == transferToBalanceActivitie)
                .FirstOrDefault();

            var actualProfit = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetUsersSavingAccountsRequest(_tradingPlatformUrl, _loginData)
                .account
                .Where(p => p.action_type == "profit")
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualTransferToBalance.user_id == expectedClientId
                    && _actualTransferToSa.user_id == expectedClientId,
                    $" expected Transfer To Balance Client Id : {expectedClientId}" +
                    $" expected Transfer To sa Client Id : {expectedClientId}" +
                    $" actual Transfer To Balance Client Id: {actualTransferToBalance.user_id}" +
                    $" actual Transfer To sa Client Id: {_actualTransferToSa.user_id}");

                Assert.True(actualTransferToBalance.user_full_name == expectedClientFullName
                    && _actualTransferToSa.user_full_name == expectedClientFullName,
                    $" expected Transfer To Balance Client Full Name : {expectedClientFullName}" +
                    $" expected Transfer To sa Client Full Name : {expectedClientFullName}" +
                    $" actual Transfer To Balance Client Full Name: {actualTransferToBalance.user_full_name}" +
                    $" actual Transfer To sa Client Full Name: {_actualTransferToSa.user_full_name}");

                Assert.True(actualTransferToBalance.currency == expectedCurrency
                    && _actualTransferToSa.currency == expectedCurrency,
                    $" expected Transfer To Balance Currency : {expectedCurrency}" +
                    $" expected Transfer To sa Currency : {expectedCurrency}" +
                    $" actual Transfer To Balance Currency: {actualTransferToBalance.currency}" +
                    $" actual Transfer To sa Currency: {_actualTransferToSa.currency}");

                Assert.True(actualTransferToBalance.erp_user_id == expectedErpUserId
                    && _actualTransferToSa.erp_user_id == expectedErpUserId,
                    $" expected Transfer To Balance Erp User Id : {expectedErpUserId}" +
                    $" expected Transfer To sa Erp User Id : {expectedErpUserId}" +
                    $" actual Transfer To Balance Erp User Id: {actualTransferToBalance.erp_user_id}" +
                    $" actual Transfer To sa Erp User Id: {_actualTransferToSa.erp_user_id}");

                Assert.True(actualTransferToBalance.erp_username == expectedErpUserName
                    && _actualTransferToSa.erp_username == expectedErpUserName,
                    $" expected Transfer To Balance Erp User Name : {expectedErpUserName}" +
                    $" expected Transfer To sa Erp User Name : {expectedErpUserName}" +
                    $" actual Transfer To Balance Erp User Name: {actualTransferToBalance.erp_username}" +
                    $" actual Transfer To sa Erp User Name: {_actualTransferToSa.erp_username}");

                Assert.True(actualTransferToBalance.type == expectedTransferToBalanceType
                    && _actualTransferToSa.type == expectedTransferToSaType,
                    $" expected Transfer To Balance Type : {expectedTransferToBalanceType}" +
                    $" expected Transfer To sa Type : {expectedTransferToSaType}" +
                    $" actual Transfer To Balance Type: {actualTransferToBalance.type}" +
                    $" actual Transfer To sa Type: {_actualTransferToSa.type}");

                Assert.True(actualTransferToBalance.action_made_by == expectedActionMadeBy
                    && _actualTransferToSa.action_made_by == expectedActionMadeBy,
                    $" expected Transfer To Balance Action Made By : {expectedActionMadeBy}" +
                    $" expected Transfer To sa Action Made By : {expectedActionMadeBy}" +
                    $" actual Transfer To Balance Action Made By: {actualTransferToBalance.action_made_by}" +
                    $" actual Transfer To sa Action Made By: {_actualTransferToSa.action_made_by}");

                Assert.True(actualTransferToBalance.action_made_by_user_id == expectedActionMadeByUserId
                    && _actualTransferToSa.action_made_by_user_id == expectedActionMadeByUserId,
                    $" expected Transfer To Balance Action Made By User Id : {expectedActionMadeByUserId}" +
                    $" expected Transfer To sa Made By User Id : {expectedActionMadeByUserId}" +
                    $" actual Transfer To Balance Made By User Id: {actualTransferToBalance.action_made_by_user_id}" +
                    $" actual Transfer To sa Made By User Id: {_actualTransferToSa.action_made_by_user_id}");

                Assert.True(actualProfit.balance == _saBalance,
                    $" expected sa Balance : {_saBalance}" +
                    $" actual sa Balance: {actualProfit.balance}");

                Assert.True(actualProfit.amount == _profit,
                    $" expected sa profit : {_profit}" +
                    $" actual sa profit: {actualProfit.amount}");
            });
        }
    }
}