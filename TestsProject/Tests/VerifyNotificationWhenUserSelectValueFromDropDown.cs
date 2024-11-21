using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;

using AirSoftAutomationFramework.Internals.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using TestsProject.TestsInternals;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using System.Threading;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;

namespace TestsProject.Tests
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyNotificationWhenUserSelectValueFromDropDown : TestSuitBase
    {
        #region Test Preparation
        public VerifyNotificationWhenUserSelectValueFromDropDown(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private string _userName;
        private string _clientName;   
        private string _groupName;
        private string _campaignName;
        private int _withdrawalId;
        private int _chargebackId;
        private GetLoginResponse _loginData;
        private Default_Attr _tradeGroupAttributes;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();
            var depositAmount = 10;
            var _dbContext = new QaAutomation01Context();
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

            _tradeGroupAttributes = new Default_Attr
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
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName);

            // get user apiKey
            var userApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            _clientName = TextManipulation.RandomString();
            var clientEmail = _clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                apiKey: userApiKey);

            // create deposit
            var depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId,
                depositAmount, apiKey: userApiKey);

            // chargeback
            var chargebackId = _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .DeleteChargeBackDepositRequest(_crmUrl, clientId,
                depositAmount, depositId, userApiKey);

            // get chargeback id
            _chargebackId =
                (from s in _dbContext.FundsTransactions
                 where (s.UserId == clientId && s.Type ==
                 "chargeback")
                 select s.Id)
                 .First();

            // create deposit for withdrawal
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId,
                depositAmount, apiKey: userApiKey);

            // get login Data for trading Platform
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            // Create Cripto Group
            _groupName = TextManipulation.RandomString();

            _apiFactory
               .ChangeContext<ITradeGroupApi>(_driver)
               .PostCreateTradeGroupRequest(_crmUrl, new List<object> { _tradeGroupAttributes },
               _groupName, apiKey: userApiKey).Trim('"');

            // create campaign
            var campaignIdAndName = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _campaignName = campaignIdAndName.Keys.First();

            // create withdrawal in tp
            _apiFactory
               .ChangeContext<IWithdrawalTpApi>()
               .PostPendingWithdrawalRequest(tradingPlatformUrl,
               _loginData, depositAmount);

            // get Withdrawal id
            _withdrawalId =
                (from s in _dbContext.FundsTransactions
                 where (s.UserId == clientId && Math.Abs(s.Amount)
                 == depositAmount && s.Type ==
                 "withdrawal" && s.Status == "pending")
                 select s.Id)
                 .First();

            var columnsNanmes = new List<string>() { "Status", "Trading group" };

            // add columns to client table 
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .PutTableColumnVisibilityRequest(_crmUrl, "clients",
                columnsNanmes, true, userApiKey);

            columnsNanmes = new List<string>() { "Transaction Status" };

            // add columns to withdrawals table 
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .PutTableColumnVisibilityRequest(_crmUrl, "withdrawals",
                columnsNanmes, true, userApiKey);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            Thread.Sleep(1000);
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
        public void VerifyNotificationWhenUserSelectValueFromDropDownTest()
        {
            var expectedChangeSalesAgentMessage = "Sales-agent changes were successfully saved.";
            var expectedChangeAssignToMessage = "Sales agent changes were successfully saved.";
            var expectedChangeCampaignMessage = "Campaign changes were successfully saved.";
            var expectedChangeTradingGroupMessage = "Trading-group changes were successfully saved.";
            var expectedChangePspMessage = "PSP changes were successfully saved.";
            var expectedTransactionStatusMessage = "Transaction Status changes were successfully saved.";

            // Navigate To clients Page
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/crm/clients");

            // check message on clients table
            _apiFactory
                .ChangeContext<IClientsPageUi>(_driver)
                .SearchClientByEmail(_clientName)
                .ChangeContext<IClientsPageUi>(_driver)
                .SelectSalesAgent("Unassign")
                .VerifyMessages(expectedChangeSalesAgentMessage)
                .SelectCampaign(_campaignName)
                .VerifyMessages(expectedChangeCampaignMessage)
                .SelectTradeGroup(_groupName)
                .VerifyMessages(expectedChangeTradingGroupMessage);

            // check message on withdrawal table
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/crm/banking/withdrawals")
                .ChangeContext<IWithdrawalsPageUi>(_driver)
                .SearchWithdrawal(_withdrawalId)
                .SelectAssignTo("Unassign")
                .VerifyMessages(expectedChangeAssignToMessage)
                .SelectPsp("airsoft-sandbox")
                .VerifyMessages(expectedChangePspMessage)
                .SelectTransactionStatus("rejected")
                .VerifyMessages(expectedTransactionStatusMessage);

            // check message on chargeback table
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/crm/banking/chargebacks")
                .ChangeContext<IChargebacksPageUi>(_driver)
                .SearchChargeback(_chargebackId)
                .SelectAssignTo("Unassign")
                .VerifyMessages(expectedChangeAssignToMessage)
                .SelectPsp("airsoft-sandbox")
                .VerifyMessages(expectedChangePspMessage);
        }
    }
}
