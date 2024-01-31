using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture(DataRep.Chrome)]
    public class VerifySavingAccountCalculationOnClientCard : TestSuitBase
    {
        #region Test Preparation
        public VerifySavingAccountCalculationOnClientCard(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private string _clientName;
        private string _userName;
        private int _depositAmont = 10;   
        private int _transferToSaAmount = 5;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {            
            #region PreCondition
            _driver = GetDriver();
            BeforeTest(_browserName);
            var usersSavingAccount = new UsersSavingAccount();
            var dbContext = new QaAutomation01Context();

            // create user
            _userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            // create client 
            _clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                apiKey: currentUserApiKey);

            // create Deposit  
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId, _depositAmont, apiKey: currentUserApiKey);

            // Transfer from balance to Saving Account
            _apiFactory
                .ChangeContext<ISATabApi>(_driver)
                .PostTransferToSavingAccountRequest(_crmUrl, clientId, _transferToSaAmount,
                apiKey: currentUserApiKey);

            var today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            usersSavingAccount.ActionType = "profit";
            usersSavingAccount.UserId = clientId;
            usersSavingAccount.CreatedAt = DateTime.Parse(today);
            usersSavingAccount.Amount = 5;
            dbContext.Add(usersSavingAccount);

            var userRecord =
            (from s in dbContext.UserAccounts 
             where s.UserId == clientId select s)
            .First();

            userRecord.SavingAccountDeposits = _depositAmont;
            dbContext.VerifySaveForSqlManipulation();
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
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifySavingAccountCalculationOnClientCardTest()
        {
            var expectedSABalance = $"SA Balance: {_depositAmont}.00 $";
            var expectedSANetDeposit = $"SA Net Deposit: {_transferToSaAmount}.00 $";
            var expectedSAProfit = $"SA Profit: {_transferToSaAmount}.00 $";

            var actualSavingAccountTitlesText = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .SearchClientByEmail(_clientName)
                .ClickOnClientFullName()
                .ClickOnSavingAccountTab()
                .GetSavingAccountTitlesText();

            var actualSABalance = actualSavingAccountTitlesText
                    .Where(p => p.Contains("Balance"))
                    .FirstOrDefault();

            var actualSANetDeposit = actualSavingAccountTitlesText
                    .Where(p => p.Contains("Deposit"))
                    .FirstOrDefault();

            var actualSAProfit = actualSavingAccountTitlesText
                    .Where(p => p.Contains("Profit"))
                    .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualSABalance == expectedSABalance,
                    $" expected SA Balance: {expectedSABalance}" +
                    $" actual SA Balance: {actualSABalance}");

                Assert.True(actualSANetDeposit == expectedSANetDeposit,
                     $" expected SA Net Deposit: {expectedSANetDeposit}" +
                     $" actual SA Net Deposit: {actualSANetDeposit}");

                Assert.True(actualSAProfit == expectedSAProfit,
                      $" expected SA Profit: {expectedSAProfit}" +
                      $" actual SA Profit: {actualSAProfit}");
            });
        }
    }
}