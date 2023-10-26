// Ignore Spelling: Api crm

using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using ConsoleApp;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    public class VerifyComplianceForBlockStatusApi : TestSuitBase
    {

        #region Members
        private string _currentUserApiKey;
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private string _clientEmail;
        private string _pendingTradeId;   
        private int _tradeAmount = 2;
        private string _tradeIdForClose;
        private GetLoginResponse _loginData;
        #endregion        

        [SetUp]
        public void SetUp()
        {
            #region Test Preparation
            BeforeTest();
            var depositAmount = 10000;

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

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

            // create deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId,
                depositAmount, apiKey: _currentUserApiKey);

            // get login Data for trading Platform
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // create tread
            var tradeData = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl,
                _tradeAmount, loginData: _loginData);

            _tradeIdForClose = tradeData.GeneralResponse.TradeId;
            var currentRate = tradeData.GeneralResponse.TradeRate;
            var rateForPending = currentRate + 5; // to open a pending trade
                                                        
            //  open the pending trade
            var pendingTradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostPendingBuyOrderRequest(_tradingPlatformUrl,
                _tradeAmount, _loginData, rateForPending);

            _pendingTradeId = pendingTradeDetails.TradeId;

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
            informationTabResponse.activation_status = "Block";
            informationTabResponse.sales_agent = "null";

            // update block client and change saving Account
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, informationTabResponse);

            // get login Data for trading Platform
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;
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
                AfterTest();
            }
        }

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyComplianceForBlockStatusApiTest()
        {
            var _dbContext = new QaAutomation01Context();
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
            var tradeAmount = 2;
            var depositAmount = 1000;
            var withdrawalAmount = 10;
            var transferToSAAmount = 20;
            var transferToBalanceAmount = 5;

            var expectetErrorMessage = "This account is blocked due to" +
                " suspicious activity and abnormal PL.Please contact your agent.Thank you!";

            // tp Transfer To Saving Account
            _apiFactory
                .ChangeContext<ISavingAccountTpApi>()
                .PostTransferToSavingAccountFromTpRequest(tradingPlatformUrl,
                _clientId, transferToSAAmount, _loginData);

            // tp Transfer To balance
            _apiFactory
                .ChangeContext<ISavingAccountTpApi>()
                .PostTransferToBalanceFromTpRequest(tradingPlatformUrl,
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

            var expectetCancelWithdrawalResponse =
                $"Withdrawal {withdrawalId} was canceled successfully";

            // cancel withdrawal in tp
            var actualCancelWithdrawalResponse = _apiFactory
               .ChangeContext<IWithdrawalTpApi>()
               .PostCancelPendingWithdrawalRequest(_tradingPlatformUrl,
               withdrawalId, _loginData);

            // create trade with error
            var actualCreateTradeErrorMessage = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl,
                tradeAmount, loginData: _loginData, checkStatusCode: false)
                .Message;

            // change pending trade amount
            var actualPendingTradeAmountError = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PatchPendingTradeRequest(_tradingPlatformUrl,
                _pendingTradeId, tradeAmount, _loginData, false);

            // close trade
            var actualCloseTradeErrorMessage = _apiFactory
                 .ChangeContext<ITradePageApi>()
                 .PatchCloseTradeRequest(_tradingPlatformUrl, _tradeIdForClose, _tradeAmount,
                 _loginData, checkStatusCode: false);

            Assert.Multiple(() =>
            {
                Assert.True(actualCancelWithdrawalResponse == expectetCancelWithdrawalResponse,
                    $" expected Cancel Withdrawal: {expectetCancelWithdrawalResponse}," +
                    $" actual Cancel Withdrawal: {actualCancelWithdrawalResponse}");

                Assert.True(actualPendingTradeAmountError.Contains(expectetErrorMessage),
                    $" expected Pending Trade change Amount Error Message: {expectetErrorMessage}," +
                    $" actual Pending Trade change Amount Error Message: {actualPendingTradeAmountError}");

                Assert.True(actualCreateTradeErrorMessage == expectetErrorMessage,
                    $" expected Create Trade Error Message: {expectetErrorMessage}," +
                    $" actual Create Trade Error Message: {actualCreateTradeErrorMessage}");        
            });
        }
    }
}