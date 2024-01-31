// Ignore Spelling: Sammery

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyTransactionsSammeryOnFinanceTab : TestSuitBase
    {
        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private int _firstDepositAmount = 500;
        private int _secondDepositAmount = 400;
        private int _approvedWithrawalAmount = 100;
        private int _pendingWithrawalAmount = 100;
        private int _chrgebackAmount = 400;
        private int _bonusAmount = 900;
        private int _sAAmount = 100;
        private IWebDriver _driver;
        private string _browserName;
        #endregion

        public VerifyTransactionsSammeryOnFinanceTab(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var currency = DataRep.DefaultUSDCurrencyName;
            _driver = GetDriver();

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, currency: currency,
                apiKey: currentUserApiKey);

            // connect One User To One Client
            _apiFactory
               .ChangeContext<IClientsApi>(_driver)
               .PatchMassAssignSaleAgentsRequest(_crmUrl, userId,
               new List<string> { clientId });

            // deposit 500
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, clientId, _firstDepositAmount);

            // deposit 400
            var depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, clientId, _secondDepositAmount);

            // chargeback 400 usd
            var chargebackId = _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .DeleteChargeBackDepositRequest(_crmUrl, clientId, _chrgebackAmount,
                depositId)
                .ToString();

            // asign chargeback to user
            _apiFactory
               .ChangeContext<IChargebacksPageApi>(_driver)
               .PatchAssignChargebackToUserRequest(_crmUrl, chargebackId, userId);

            // withrawal 100 usd
            _apiFactory
               .ChangeContext<IFinancesTabApi>(_driver)
               .PostWithdrawalDepositRequest(_crmUrl, clientId, userId,
               _approvedWithrawalAmount);

            // get pending Withdrawal id
            var withdrawalId =
                (from s in _dbContext.FundsTransactions
                 where (s.UserId == clientId && Math.Abs(s.Amount)
                 == _approvedWithrawalAmount && s.Type
                 == "withdrawal" && s.Status == "pending")
                 select s.Id).First()
                 .ToString();

            // proceed Withdrawal 100 usd
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PatchWithdrawalStatusRequest(_crmUrl, clientId, withdrawalId,
                apiKey: currentUserApiKey);

            // pending withrawal 100 usd
            _apiFactory
               .ChangeContext<IFinancesTabApi>(_driver)
               .PostWithdrawalDepositRequest(_crmUrl, clientId, userId,
               _pendingWithrawalAmount);

            // bonus 900 usd
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostBonusRequest(_crmUrl, clientId, _bonusAmount);

            // Transfer To Saving Account 100 usd
            _apiFactory
                .ChangeContext<ISATabApi>(_driver)
                .PostTransferToSavingAccountRequest(_crmUrl, clientId, _sAAmount);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .SearchClientByEmail(clientName)
                .ClickOnClientFullName()
                .ClickOnFinanceTab();
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
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }
        // create user with users only roll
        // create client
        // connect user to client
        // deposit 400, 500
        // chargeback the 400 deposit
        // create withdrawal of 100
        // approve the withdrawal
        // create pending withdrawal of 100
        // create 900 bonus
        // Transfer To Saving Account 100 usd
        // verify expected Transaction Sammery Data
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]      
        public void VerifyTransactionsSammeryOnFinanceTabTest()
        {
            var expectedTransactionSammeryData = new Dictionary<string, string>();
            expectedTransactionSammeryData.Add("Deposit", "900.00 $");
            expectedTransactionSammeryData.Add("Deposit bonus", "900.00 $");
            expectedTransactionSammeryData.Add("Chargeback", "400.00 $");
            expectedTransactionSammeryData.Add("Withdrawal requests", "100.00 $");
            expectedTransactionSammeryData.Add("Withdrawal processed", "100.00 $");
            expectedTransactionSammeryData.Add("Available to withdraw", "269.23 $"); // 500 - 100 - 100 = 300/900 שווה לשליש 69.23 זה שני שליש.
            expectedTransactionSammeryData.Add("Net Deposit", "400.00 $");
            expectedTransactionSammeryData.Add("Net Deposit Bonus", "900.00 $");
            expectedTransactionSammeryData.Add("SA Balance", "100.00 $");

            var actualTransactionSammeryData = _apiFactory
                .ChangeContext<IFinanceTabUi>(_driver)
                .GetTransactionSammery();

            var actualKeysAgainstAxpected = actualTransactionSammeryData.Keys.ToList()
                .CompareTwoListOfString(expectedTransactionSammeryData.Keys.ToList());

            var actualValuesAgainstAxpected = actualTransactionSammeryData.Values.ToList()
                .CompareTwoListOfString(expectedTransactionSammeryData.Values.ToList());

            Assert.Multiple(() =>
            {
                Assert.True(actualKeysAgainstAxpected.Count == 0,
                    $" actual Keys Against Axpected: {actualKeysAgainstAxpected.ListToString()}" +
                    $" expected Transaction keys Sammery Data: {expectedTransactionSammeryData.Keys.ListToString()}" +
                    $" actual Transaction keys Sammery Data: {actualTransactionSammeryData.Keys.ListToString()}");

                Assert.True(actualValuesAgainstAxpected.Count == 0,
                    $" actual Values Against Axpected: {actualValuesAgainstAxpected.ListToString()}" +
                    $" expected Transaction Values Sammery Data: {expectedTransactionSammeryData.Values.ListToString()}" +
                    $" actual Transaction Values Sammery Data: {actualTransactionSammeryData.Values.ListToString()}");
            });
        }
    }
}
