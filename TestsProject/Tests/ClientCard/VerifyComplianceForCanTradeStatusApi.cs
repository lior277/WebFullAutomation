// Ignore Spelling: Api

using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyComplianceForCanTradeStatusApi : TestSuitBase
    {
        public VerifyComplianceForCanTradeStatusApi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Members
        private string _currentUserApiKey;
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private string _clientEmail;
        private string _browserName;
        private string _tradeIdForClose;
        private int _withdrawalIdForCancel;
        private int _tradeAmount = 2;
        private int _withdrawalAmount = 10;
        private IWebDriver _driver;
        private GetLoginResponse _loginData;
        #endregion        

        [SetUp]
        public void SetUp()
        {
            #region Test Preparation
            BeforeTest();
            _driver = GetDriver();
            var depositAmount = 10000;

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            // get login Data for trading Platform
            _loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            // create deposit for trade
            _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PostDepositRequest(_crmUrl, _clientId,
               depositAmount, apiKey: _currentUserApiKey);

            // create withdrawal in tp
            _apiFactory
               .ChangeContext<IWithdrawalTpApi>()
               .PostPendingWithdrawalRequest(_tradingPlatformUrl,
               _loginData, _withdrawalAmount);

            // get Withdrawal id
            _withdrawalIdForCancel =
                (from s in _dbContext.FundsTransactions
                 where (s.UserId == _clientId && Math.Abs(s.Amount)  
                 == _withdrawalAmount && s.Type ==
                 "withdrawal" && s.Status == "pending")
                 select s.Id)
                 .First();

            // verify default SA exist
            // create Saving Account
            var expectedSavingAccountName = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .PostCreateSavingAccountRequest(_crmUrl, apiKey: _currentUserApiKey);

            var savingAccountId = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .GetSavingAccountsRequest(_crmUrl, _currentUserApiKey)
                .SavingAccountData
                .Where(p => p.Name == expectedSavingAccountName)
                .FirstOrDefault()
                .Id;

            var informationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab;

            informationTabResponse.saving_account_id = savingAccountId;
            informationTabResponse.sales_agent = "null";

            // new saving Account
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, informationTabResponse,
                _currentUserApiKey);

            // create tread
            var tradeData = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl,
               _tradeAmount, loginData: _loginData);

            _tradeIdForClose = tradeData.GeneralResponse.TradeId;

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .SearchClientByEmail(clientName)
                .ClickOnClientFullName();
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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyComplianceForCanTradeStatusApiTest()
        {
            var tradedingPlatformUrl = Config.appSettings.tradingPlatformUrl;
            var depositAmount = 1000;
            var transferToSAAmount = 20;
            var transferToBalanceAmount = 5;

            var expectetaCreateFinanceError = "Please activate your account before you proceed with this action." +
                " For more information, kindly contact your manager. Thank you";

            var expectetaCreateWithdrawalError = "active_your_account";

            var expectetComplianceStatusToolTipText = "Can trade Allowed Open/close trades Cancel withdrawal requests" +
                " Transfer from Balance to SA and from SA to Balance Not allowed Deposit and withdraw";

                // get ComplianceS status ToolTip Text 
               var actualComplianceStatusToolTipText = _apiFactory
                .ChangeContext<IInformationTabUi>(_driver)
                .SelectComplianceStatus("Can trade")
                .GetComplianceStatusToolTipText();

            // save customer
            _apiFactory
                .ChangeContext<IClientCardUi>(_driver)
                .ClickOnSave();

            // get login Data for trading Platform
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // tp Transfer To Saving Account
            _apiFactory
                .ChangeContext<ISavingAccountTpApi>()
                .PostTransferToSavingAccountFromTpRequest(tradedingPlatformUrl,
                _clientId, transferToSAAmount, _loginData);

            // tp Transfer To balance
            _apiFactory
                .ChangeContext<ISavingAccountTpApi>()
                .PostTransferToBalanceFromTpRequest(tradedingPlatformUrl,
                _clientId, transferToBalanceAmount, _loginData);

            // create deposit in tp with error
           var actualCreateDepositError = _apiFactory
                .ChangeContext<ITradeDepositPageApi>()
                .PostCreatePaymentRequestPipe(_tradingPlatformUrl,
                _loginData, depositAmount, checkStatusCode: false)
                .Message;

            // create withdrawal in tp
            var actualCreateWithdrawalError = _apiFactory
               .ChangeContext<IWithdrawalTpApi>()
               .PostPendingWithdrawalRequest(_tradingPlatformUrl,
               _loginData, _withdrawalAmount, checkStatusCode: false);

            // cancel withdrawal in tp
           _apiFactory
                .ChangeContext<IWithdrawalTpApi>()
                .PostCancelPendingWithdrawalRequest(_tradingPlatformUrl,
                _withdrawalIdForCancel, _loginData);

            // create trade
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl,
                _tradeAmount, loginData: _loginData);

            // close trade
            _apiFactory
                 .ChangeContext<ITradePageApi>()
                 .PatchCloseTradeRequest(_tradingPlatformUrl,
                 _tradeIdForClose, _tradeAmount, _loginData);

            Assert.Multiple(() =>
            {
                Assert.True(actualCreateDepositError == expectetaCreateFinanceError,
                    $" expected Create Deposit Error Message: {expectetaCreateFinanceError}," +
                    $" actual Create Deposit Error Message: {actualCreateDepositError}");

                Assert.True(actualCreateWithdrawalError.Contains(expectetaCreateWithdrawalError),
                    $" expected Create Withdrawal Error Message: {expectetaCreateWithdrawalError}," +
                    $" actual Create Withdrawal Error Message: {actualCreateWithdrawalError}");

                Assert.True(actualComplianceStatusToolTipText == expectetComplianceStatusToolTipText,
                    $" expected Compliance Status ToolTip Text: {expectetComplianceStatusToolTipText}," +
                    $" actual Compliance Status ToolTip Text: {actualComplianceStatusToolTipText}");
            });
        }
    }
}