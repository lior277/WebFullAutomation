// Ignore Spelling: Api

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
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using ConsoleApp;
using NUnit.Framework;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.GetUsersSavingAccounts;

namespace TestsProject.Tests.TradingPlatform.TradingPlatformActivitiesTests
{
    [TestFixture]
    public class VerifyExportActivitiesApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private string _clientEmail;
        private string _userName;
        private string _tradeId;
        private string _userId;
        private string _depositId;
        private int _profit = 10;
        private int _depositAmount = 10000;
        private int _tradeAmount = 2;
        private int _transferAmount = 1;
        private Account[] _actualSavingAccount;
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
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount, apiKey: currentUserApiKey);
            #endregion

            #region create trade 
            // create trade 
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount, _loginData)
                .GeneralResponse;

            _tradeId = tradeDetails.TradeId;
            #endregion

            // CRM Transfer To Saving Account
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_crmUrl, _clientId, _transferAmount,
                currentUserApiKey);

            // get Transfer To Sa data
            var actualTransferToSa = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetActivities(_tradingPlatformUrl, _loginData)
                .Where(p => p.type == "transfer_to_sa")
                .FirstOrDefault();

            var saBalance = _transferAmount + _profit;

            // create profit
            _apiFactory
                .ChangeContext<ISATabApi>()
                .CreateSaProfit(_crmUrl, actualTransferToSa, _profit, saBalance);

            // CRM Transfer To Balance
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToBalanceRequest(_crmUrl, _clientId, _transferAmount,
                currentUserApiKey);

            _actualSavingAccount = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetUsersSavingAccountsRequest(_tradingPlatformUrl, _loginData)
                .account;
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
        [Category(DataRep.TestCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyExportActivitiesApiTest()
        {
            // expected export saving account 
            var expectedTypes = new List<string> { "deposit", "profit", "withdrawal" };
            var expectedDate = DateTime.Now.ToString("MM/dd/yyyy");
            var expectedSaAmounts = new List<string> { "1", "10", "1" };
            var expectedSaPercentages = new List<string> { "1", "1", "1" };
            var expectedSaBalances = new List<string> { "1", "11", "0" };

            // expected export trade 
            var expectedTradeId = _tradeId;
            var expectedByType = "Open trade";
            var expectedByPosition = "buy";
            var expectedInstrument = DataRep.AssetName;
            var expectedTradeAmount = _tradeAmount;
            var expectedTradeStatus = "open";

            // expected export deposit 
            var expectedTransactionId = _depositId;
            var expectedDepositAmount = _depositAmount;
            var expectedCurrency = DataRep.DefaultUSDCurrencyName;
            var expectedType = "Deposit";
            var expectedDepositStatus = "Approved";

            // export TP trades activities 
            var actualExportActivitieTrades = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetExportActivitiesRequest(_tradingPlatformUrl,
                _clientId, loginData: _loginData)
                .GeneralResponse
                ?.Sheets
                ?.Trades
                .FirstOrDefault();

            // export TP finance activities 
            var actualExportActivitieFinance = _apiFactory
               .ChangeContext<ITradePageApi>()
               .GetExportActivitiesRequest(_tradingPlatformUrl,
               _clientId, loginData: _loginData)
               .GeneralResponse
               .Sheets?
               .Finance?
               .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(_actualSavingAccount.Select(p => p.action_type)
                    .Except(expectedTypes).Count() == 0,
                    $" expected action types : {expectedTypes}" +
                    $" actual action types: " +
                    $"{_actualSavingAccount.Select(p => p.action_type).ListToString()}");

                Assert.True(_actualSavingAccount.Select(p => p.created_at.ToString())
                    .All(p => p.Contains(expectedDate)),
                    $" expected created at : {expectedDate}" +
                    $" actual created at: " +
                    $"{_actualSavingAccount.Select(p => p.created_at).ListToString()}");

                Assert.True(_actualSavingAccount.Select(p => p.amount.ToString())
                    .Except(expectedSaAmounts).Count() == 0,
                    $" expected amounts : {expectedSaAmounts}" +
                    $" actual amounts: " +
                    $"{_actualSavingAccount.Select(p => p.amount).ListToString()}");

                Assert.True(_actualSavingAccount.Select(p => p.sa_percentage.ToString())
                    .Except(expectedSaPercentages).Count() == 0,
                    $" expected sa percentage : {expectedSaPercentages}" +
                    $" actual sa percentage: " +
                    $"{_actualSavingAccount.Select(p => p.sa_percentage).ListToString()}");

                Assert.True(_actualSavingAccount.Select(p => p.balance.ToString())
                    .Except(expectedSaBalances).Count() == 0,
                    $" expected balance : {expectedSaBalances}" +
                    $" actual balance: " +
                    $"{_actualSavingAccount.Select(p => p.balance).ListToString()}");

                Assert.True(actualExportActivitieTrades.OrderId == expectedTradeId,
                    $" expected Order ID : {expectedTradeId}" +
                    $" actual Order ID: {actualExportActivitieTrades.OrderId}");

                Assert.True(actualExportActivitieTrades.ByType == expectedByType,
                    $" expected By Type : {expectedByType}" +
                    $" actual By Type: {actualExportActivitieTrades.ByType}");

                Assert.True(actualExportActivitieTrades.ByPosition == expectedByPosition,
                    $" expected By Position : {expectedByPosition}" +
                    $" actual By Position: {actualExportActivitieTrades.ByPosition}");

                Assert.True(actualExportActivitieTrades.Instrument == expectedInstrument,
                    $" expected Instrument : {expectedInstrument}" +
                    $" actual Instrument: {actualExportActivitieTrades.Instrument}");

                Assert.True(actualExportActivitieTrades.Amount == expectedTradeAmount,
                    $" expected amount: {expectedTradeAmount}" +
                    $" actual  amount: {actualExportActivitieTrades.Amount}");

                Assert.True(actualExportActivitieTrades.Status == expectedTradeStatus,
                    $" expected Status: {expectedTradeStatus}" +
                    $" actual Status: {actualExportActivitieTrades.Status}");

                Assert.True(actualExportActivitieFinance.TransactionId == expectedTransactionId,
                    $" expected Transaction ID : {expectedTransactionId}" +
                    $" actual Transaction ID: {actualExportActivitieFinance.TransactionId}");

                Assert.True(actualExportActivitieFinance.Amount == expectedDepositAmount,
                    $" expected Amount: {expectedDepositAmount}" +
                    $" actual Amount: {actualExportActivitieFinance.Amount}");

                Assert.True(actualExportActivitieFinance.Currency == expectedCurrency,
                    $" expected  currency: {expectedCurrency}" +
                    $" actual  currency: {actualExportActivitieFinance.Currency}");

                Assert.True(actualExportActivitieFinance.Type == expectedType,
                    $" expected Type: {expectedType}" +
                    $" actual Type: {actualExportActivitieFinance.Type}");

                Assert.True(actualExportActivitieFinance.Status == expectedDepositStatus,
                    $" expected Status: {expectedDepositStatus}" +
                    $" actual Status: {actualExportActivitieFinance.Status }");
            });
        }
    }
}
