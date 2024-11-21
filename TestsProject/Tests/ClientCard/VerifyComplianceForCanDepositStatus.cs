using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
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
    public class VerifyComplianceForCanDepositStatus : TestSuitBase
    {
        public VerifyComplianceForCanDepositStatus(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Members
        private string _currentUserApiKey;
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private string _clientEmail;
        private string _pendingTradeId;   
        private string _chronoId;
        private int _tradeAmount = 2;
        private string _browserName;
        private string _tradeIdForClose;
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

            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId,
                depositAmount, apiKey: _currentUserApiKey);

            // get login Data for trading Platform
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // create trade
            var tradeData = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl,
                _tradeAmount, loginData: _loginData);

            _tradeIdForClose = tradeData.GeneralResponse.TradeId;
            var currentRate = tradeData.GeneralResponse.TradeRate;
            var tradeId = tradeData.GeneralResponse.TradeId;
            var rateForPending = (currentRate + 0.001); // to open a pending trade  

            //  open the pending trade
            var pendingTradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostPendingBuyOrderRequest(_tradingPlatformUrl,
                _tradeAmount, _loginData, rateForPending);

            _pendingTradeId = tradeData.GeneralResponse.TradeId;

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

            // buy chrono trade 
            var chronoData = _apiFactory
                .ChangeContext<IChronoTradePageApi>()
                .PostBuyChronoAssetApi(_tradingPlatformUrl, _loginData)
                .GeneralResponse;

            _chronoId = chronoData.TradeId;

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
        public void VerifyComplianceForCanDepositStatusTest()
        {
            var _dbContext = new QaAutomation01Context();
            var tradedingPlatformUrl = Config.appSettings.tradingPlatformUrl;
            var tradeAmount = 2;
            var depositAmount = 1000;
            var withdrawalAmount = 10;
            var transferToSAAmount = 20;
            var transferToBalanceAmount = 5;
            var expectetCreateTradeErrorMessage = "active_your_account_before_open_trade";
            var expectetCloseTradeErrorMessage = "active_your_account_before_close_trade";
            var expectetChangeAmountOfPendingErrorMessage = "active_your_account";

            var expectetComplianceStatusToolTipText = "Can deposit Allowed Deposit Make or cancel" +
                " a withdrawal request Transfer from Balance to SA and from SA to Balance Not allowed Nothing related to trading";

            // get ComplianceS status ToolTip Text 
            var actualComplianceStatusToolTipText = _apiFactory
                .ChangeContext<IInformationTabUi>(_driver)
                .SelectComplianceStatus("Can deposit")
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

            // create deposit in tp
            _apiFactory
                .ChangeContext<ITradeDepositPageApi>()
                .PostCreatePaymentRequestPipe(_tradingPlatformUrl,
                _loginData, depositAmount);

            // create withdrawal in tp
            _apiFactory
               .ChangeContext<IWithdrawalTpApi>()
               .PostPendingWithdrawalRequest(_tradingPlatformUrl,
               _loginData, withdrawalAmount);

            // get Withdrawal id
            var withdrawalId =
                (from s in _dbContext.FundsTransactions
                 where (s.UserId == _clientId && Math.Abs(s.Amount)
                 == withdrawalAmount && s.Type ==
                 "withdrawal" && s.Status == "pending")
                 select s.Id)
                 .First();

            var expectetCancelWithdrawalResponse = $"Withdrawal {withdrawalId} was canceled successfully";

            // cancel withdrawal in tp
            var actualCancelWithdrawalResponse = _apiFactory
               .ChangeContext<IWithdrawalTpApi>()
               .PostCancelPendingWithdrawalRequest(_tradingPlatformUrl,
               withdrawalId, _loginData);

            // create trade error message
            var actualCreateTradeErrorMessage = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl,
                tradeAmount, loginData: _loginData, checkStatusCode: false)
                .Message;

            // change pending trade amount
            var actualPendingTradeAmountError = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PatchPendingTradeRequest(_tradingPlatformUrl,
                _pendingTradeId, 10, _loginData, false);

            // close trade
            var actualCloseTradeErrorMessage = _apiFactory
                 .ChangeContext<ITradePageApi>()
                 .PatchCloseTradeRequest(_tradingPlatformUrl, _tradeIdForClose, _tradeAmount,
                 _loginData, checkStatusCode: false);

            // buy chrono trade 
            var actualCreateChronoErrorMessage = _apiFactory
                .ChangeContext<IChronoTradePageApi>()
                .PostBuyChronoAssetApi(_tradingPlatformUrl, _loginData, checkStatusCode: false)
                .Message;

            Assert.Multiple(() =>
            {
                Assert.True(actualCancelWithdrawalResponse == expectetCancelWithdrawalResponse,
                    $" expected Cancel Withdrawal: {expectetCancelWithdrawalResponse}," +
                    $" actual Cancel Withdrawal: {actualCancelWithdrawalResponse}");

                Assert.True(actualComplianceStatusToolTipText == expectetComplianceStatusToolTipText,
                    $" expected Compliance Status ToolTip Text: {expectetComplianceStatusToolTipText}," +
                    $" actual Compliance Status ToolTip Text: {actualComplianceStatusToolTipText}");

                Assert.True(actualCreateTradeErrorMessage == expectetCreateTradeErrorMessage,
                    $" expected Create Trade Error Message: {expectetCreateTradeErrorMessage}," +
                    $" actual Create Trade Error Message: {actualCreateTradeErrorMessage}");

                Assert.True(actualCloseTradeErrorMessage.Contains(expectetCloseTradeErrorMessage),
                    $" expected Close Trade Error Message: {expectetCloseTradeErrorMessage}," +
                    $" actual Close Trade Error Message: {actualCloseTradeErrorMessage}");

                Assert.True(actualPendingTradeAmountError.Contains(expectetChangeAmountOfPendingErrorMessage),
                    $" expected Pending Trade change Amount Error Message: {expectetChangeAmountOfPendingErrorMessage}," +
                    $" actual Pending Trade change Amount Error Message: {actualPendingTradeAmountError}");

                Assert.True(actualCreateChronoErrorMessage == expectetCreateTradeErrorMessage,
                   $" expected Create Chrono Error Message: {expectetCreateTradeErrorMessage}," +
                   $" actual Create Chrono Error Message: {actualCreateChronoErrorMessage}");
            });
        }
    }
}