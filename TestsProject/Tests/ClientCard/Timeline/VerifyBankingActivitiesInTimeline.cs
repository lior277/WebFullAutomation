// Ignore Spelling: TimeLine

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.MongoDb;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard.TimeLine
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyBankingActivitiesInTimeline : TestSuitBase
    {
        #region Test Preparation
        public VerifyBankingActivitiesInTimeline(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userName;
        private string _clientId;
        private int _bonusAmount = 1000;
        private string _withdrawalIdForSplit;
        private int _bonusForDeleteAmount = 10;
        private int _depositForChargebackAmount = 10;
        private int _depositForDeleteAmount = 20;
        private int _widrowalBonusAmount = 10;
        private int _widrowalDepositAmount = 10;
        private int _transferToSavingAccountAmount = 1;
        private int _transferToBalanceAmount = 1;
        private int _widrowalSplitAmount = 5; // _widrowalDepositAmount / 2
        private int _depositAmount = 1000;
        public string _withdrawalId;
        private IWebDriver _driver;
        private string _browserName;
        private string _orderId;
        private string _clientName;
        #endregion

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
            var dbContext = new QaAutomation01Context();
            var mongoDbAccess = new MongoDbAccess();
            _driver = GetDriver();
            var mongoDatabase = InitializeMongoClient.ConnectToCrmMongoDb;

            // create user for the creation of api key
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName, _crmUrl);

            // get ApiKey
            var userApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client
            _clientName = TextManipulation.RandomString();
            var clientEmail = _clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                apiKey: userApiKey);

            var clientIds = new List<string> { _clientId };
            _clientName = _clientName.UpperCaseFirstLetter();

            // create bonus for delete
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, _bonusForDeleteAmount, apiKey: userApiKey);

            // get bonus id 
            var bonusId =
               (from s in dbContext.FundsTransactions
                where (s.UserId == _clientId && s.Amount == _bonusForDeleteAmount && s.Type == "deposit_bonus")
                select s.Id)
                .First()
                .ToString();

            //  delete bonus
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .DeleteFinanceItemRequest(_crmUrl, bonusId);

            // create deposit for Delete
            var depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositForDeleteAmount,
                apiKey: userApiKey);

            // delete deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .DeleteFinanceItemRequest(_crmUrl, depositId);

            // create deposit for Chargeback
            depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositForChargebackAmount,
                apiKey: userApiKey);

            // chargeback
            var chargebackId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .DeleteChargeBackDepositRequest(_crmUrl, _clientId,
                _depositForChargebackAmount, depositId, userApiKey);

            // create deposit for Withdrawal
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId,
                _depositAmount, apiKey: userApiKey);

            // Withdrawal 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostWithdrawalDepositRequest(_crmUrl, _clientId, userId,
                _widrowalDepositAmount, apiKey: userApiKey);

            // get pending Withdrawal id
            _withdrawalId =
                (from s in dbContext.FundsTransactions
                 where (s.UserId == _clientId && s.Type == "withdrawal" && s.Status == "pending")
                 select s.Id)
                 .First().ToString();

            // proceed deposit Withdrawal
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PatchWithdrawalStatusRequest(_crmUrl,
                _clientId, _withdrawalId, apiKey: userApiKey);

            // Withdrawal Deposit pending for delete
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostWithdrawalDepositRequest(_crmUrl, _clientId, userId, _widrowalDepositAmount,
                apiKey: userApiKey);

            // get pending Withdrawal id
            _withdrawalIdForSplit =
                (from s in dbContext.FundsTransactions
                 where (s.UserId == _clientId && s.Type == "withdrawal" && s.Status == "pending")
                 select s.Id)
                 .First()
                 .ToString();

            // split Withdrawal(should be added)
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostSplitPendingWithdrawalRequest(_crmUrl, _clientId, _withdrawalIdForSplit,
                _widrowalSplitAmount, apiKey: userApiKey);

            // get pending Withdrawal id
            _withdrawalIdForSplit =
                (from s in dbContext.FundsTransactions
                 where (s.UserId == _clientId && s.Type == "withdrawal" && s.Status == "pending")
                 select s.Id)
                 .First()
                 .ToString();

            // delete pending Withdrawal
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .DeleteWithdrawalRequest(_crmUrl, _withdrawalIdForSplit);

            // create bonus for Withdrawal
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, _bonusAmount, apiKey: userApiKey);

            // Withdrawal part of the bonus
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostWithdrawalBonusRequest(_crmUrl, _clientId, _widrowalBonusAmount, apiKey: userApiKey);

            // Transfer from balance to Saving Account
            _apiFactory
                .ChangeContext<ISATabApi>(_driver)
                .PostTransferToSavingAccountRequest(_crmUrl, _clientId, _transferToSavingAccountAmount,
                apiKey: userApiKey);

            // Transfer from Saving Account to balance
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToBalanceRequest(_crmUrl, _clientId,
                _transferToBalanceAmount, apiKey: userApiKey);

            // get login data
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            // create pending deposit
            _apiFactory
                .ChangeContext<ITradeDepositPageApi>()
                .PostCreatePaymentRequestPipe(tradingPlatformUrl,
                loginData, depositAmount: _depositAmount);

            var currency = DataRep.DefaultUSDCurrencyName;

            _apiFactory
                .ChangeContext<IPspTabApi>()
                .PostPaymentNotificationRequestPipe(_crmUrl,
                _clientId, _depositAmount, currency);

            _orderId = mongoDbAccess
                .SelectAllDocumentsFromTable<PspLogsMongoTable>(mongoDatabase,
                DataRep.PspLogsTable)
                .Where(p => p.create_payment.request.body.user_id.Equals(_clientId))
                .FirstOrDefault()
                .order_id;
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyBankingActivitiesInTimelineTest()
        {
            _clientName = _clientName
                .UpperCaseFirstLetter();

            var expectedDate = DateTime.Now.ToString("ddd MMM dd yyy");

            #region expected data
            var expectedTimelineData = new List<string>();

            expectedTimelineData.Add($"{_userName} moved {_transferToSavingAccountAmount}.00 {DataRep.DefaultUSDCurrencyName} from SA to Balance");
            expectedTimelineData.Add($"{_userName} moved {_transferToBalanceAmount}.00 {DataRep.DefaultUSDCurrencyName} from Balance to SA");
            expectedTimelineData.Add($"{_userName} withdrew bonus of {_widrowalBonusAmount}.00 {DataRep.DefaultUSDCurrencyName}");
            expectedTimelineData.Add("Email client new bonus has been sent to client by System");
            expectedTimelineData.Add($"{_userName} Added a Bonus of {_bonusAmount}.00 {DataRep.DefaultUSDCurrencyName}");
            expectedTimelineData.Add("Email client new bonus has been sent to client by System");
            expectedTimelineData.Add($"{_userName} Added a Bonus of {_bonusForDeleteAmount}.00 {DataRep.DefaultUSDCurrencyName}");
            expectedTimelineData.Add($"{_userName} Changed WD from pending to approved, ID: {_withdrawalId}");

            expectedTimelineData.Add($"WD ID {_withdrawalIdForSplit} was split by" +
               $" {_userName} from {_widrowalDepositAmount} to {_widrowalDepositAmount / 2}");

            expectedTimelineData.Add("Email withdrawal request has been sent to client by System");
            expectedTimelineData.Add($"{_userName} made withdrawal request, amount: {_widrowalDepositAmount}.00 {DataRep.DefaultUSDCurrencyName}");
            expectedTimelineData.Add("Email withdrawal processed has been sent to client by System");
            expectedTimelineData.Add("Email withdrawal request has been sent to client by System");
            expectedTimelineData.Add($"{_userName} made withdrawal request, amount: {_widrowalDepositAmount}.00 {DataRep.DefaultUSDCurrencyName}");
            expectedTimelineData.Add("Email client deposit has been sent to client by System");
            expectedTimelineData.Add("Email client deposit has been sent to client by System");
            expectedTimelineData.Add($"{_userName} Added a Deposit of {_depositAmount}.00 {DataRep.DefaultUSDCurrencyName}");
            expectedTimelineData.Add($"{_userName} charged back deposit of {_depositForChargebackAmount}.00 {DataRep.DefaultUSDCurrencyName}");
            expectedTimelineData.Add("Email first deposit has been sent to client by System");
            expectedTimelineData.Add($"{_userName} Added a Deposit of {_depositForChargebackAmount}.00 {DataRep.DefaultUSDCurrencyName}");
            expectedTimelineData.Add("Email first deposit has been sent to client by System");
            expectedTimelineData.Add($"{_userName} Added a Deposit of {_depositForDeleteAmount}.00 {DataRep.DefaultUSDCurrencyName}");
            expectedTimelineData.Add($"{_userName} registered from API");
            expectedTimelineData.Add($"Current Username: {_userName} PSP airsoft-sandbox deposit status has changed to approved for deposit with order id {_orderId}");

            expectedTimelineData.Add($"{_clientName} {_clientName} has logged into the" +
                $" trading-platform and is currently online");
            #endregion

            var timelineDitails = _apiFactory
                 .ChangeContext<IMenus>(_driver)
                 .ClickOnMenuItem<IClientsPageUi>()
                 .SearchClientByEmail(_clientName)
                 .ClickOnClientFullName()
                 .ClickOnTimelineTab()
                 .SetNumOfLines()
                 .GetSearchResultDetails<SearchResultTimeline>(_clientName, checkNumOfRows: false)
                 .ToList();

            var actualTimelineActions = new List<string>();
            timelineDitails.ForEach(p => actualTimelineActions.Add(p.action));
           
            var actualDate = timelineDitails
                .Select(d => d.date)
                .ToList()
                .All(p => p.Contains(expectedDate));

            actualTimelineActions
                .RemoveAll(p => p.Equals("Email admin deposit has been sent to client by System"));

            actualTimelineActions
                .RemoveAll(p => p.Equals("Email dod has been sent to client by System"));

            //actualTimelineActions
            //    .RemoveAll(p => p.Contains("Current Username:"));

            var actualAgainstExpected = actualTimelineActions
                .CompareTwoListOfString(expectedTimelineData);

            Assert.Multiple(() =>
            {
                Assert.True(actualDate,
                    $" expected: {expectedDate}" +
                    $" actual : different then {actualTimelineActions.ListToString()}");

                Assert.True(actualAgainstExpected.Count == 0,
                    $" Actual  Against expected list: {actualAgainstExpected.ListToString()}" +
                    $" expected TimeLine Data: {expectedTimelineData.ListToString()}" +
                    $" actual TimeLine Data: {actualTimelineActions.ListToString()}");
            });
        }
    }
}