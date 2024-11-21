// Ignore Spelling: Api

using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture]
    public class VerifyWithdrawalAndTransferToSaDisabledForOpenTradeApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private string _clientEmail;
        private int _depositAmount = 10000;
        private string _currentUserApiKey;
        private GetLoginResponse _loginData;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var tradeAmount = 2;

            var tradeGroupAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 5,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = -0.1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

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

            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            // get login Data for trading Platform
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            _apiFactory // create trade to retrieve the current rate
                 .ChangeContext<ITradePageApi>()
                 .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, _loginData);

            var groupData = _apiFactory // Create Cripto Group And Assign It To client
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, tradeGroupAttributes, _clientId);

            var groupId = groupData.Keys.First();

            _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .DeleteTradeGroupRequest(_crmUrl, groupId);
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

        // create client
        // create deposit
        // create trade 
        // create cripto group with low spread
        // assign client to the cripto group
        // try to Withdrawal all the deposit amount
        // try to transfer to sa all the deposit amount
        // verify that when the trade is open you cant
        // Withdrawal the invest ampunt and the PNL
        // verify that when the trade is open you cant
        // Transfer To Sa the invest ampunt and the PNL700147
        [Test]
        [Description("based on jira : https://airsoftltd.atlassian.net/browse/AIRV2-4334")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyWithdrawalAndTransferToSaDisabledForOpenTradeApiTest()
        {
            var expecteAvailableWithdrawalMessage = "Invalid amount, available withdrawal is";
            var expecteAvailableTransferToSaMessage = "Invalid amount";

            var actualAvailableWithdrawal = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .GetAvailableWithdrawalByClientIdRequest(_crmUrl, _clientId)
                .GeneralResponse;

            actualAvailableWithdrawal.AvailableWithdrawal = (double)
                actualAvailableWithdrawal.AvailableWithdrawal
                .MathRoundFromGeneric(0, MidpointRounding.ToEven);

            var actualErrorWithdrawalMessage = _apiFactory
                .ChangeContext<IWithdrawalTpApi>()
                .PostPendingWithdrawalRequest(_tradingPlatformUrl, _loginData,
                _depositAmount, checkStatusCode: false);

            var actualAvailableTransferToSa = _apiFactory
                .ChangeContext<ISATabApi>()
                .GetSaBalanceByClientIdRequest(_crmUrl, _clientId);

            actualAvailableTransferToSa.AvailableDeposit = (double)
            actualAvailableWithdrawal.AvailableWithdrawal
            .MathRoundFromGeneric(0, MidpointRounding.ToEven);

            var actualErrorTransferToSaMessage =  _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_crmUrl, _clientId,
                _depositAmount, _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualErrorWithdrawalMessage.Contains(expecteAvailableWithdrawalMessage),
                    $" expected Available Withdrawal Message : {expecteAvailableWithdrawalMessage}" +
                    $" actual Available Withdrawal Message: {actualErrorWithdrawalMessage}");

                Assert.True(actualErrorTransferToSaMessage.Contains(expecteAvailableTransferToSaMessage),
                    $" expected Transfer To Sa Message : {expecteAvailableTransferToSaMessage}" +
                    $" actual Transfer To Sa Message: {actualErrorTransferToSaMessage}");

                Assert.True(actualAvailableWithdrawal.AvailableWithdrawal == actualAvailableTransferToSa.AvailableDeposit,
                    $" actual Available Withdrawal : {actualAvailableWithdrawal.AvailableWithdrawal}" +
                    $" actual Available Deposit: {actualAvailableTransferToSa.AvailableDeposit}");
            });
        }
    }
}