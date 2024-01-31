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
    public class VerifyComplianceForCannotWithdrawStatus : TestSuitBase
    {
        public VerifyComplianceForCannotWithdrawStatus(string browser) : base(browser)
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

            // create deposit 
            _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PostDepositRequest(_crmUrl, _clientId,
               depositAmount, apiKey: _currentUserApiKey);                      

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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyComplianceForCannotWithdrawStatusTest()
        {
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

            var expectedComplianceStatusToolTipText = "Cannot withdraw Allowed Client can" +
                " proceed with financial and trade actions Not allowed Withdraw";

            var expectedCreateWithdrawalError = "active_your_account";

            // get ComplianceS tatus ToolTip Text 
            var actualComplianceStatusToolTipText = _apiFactory
                .ChangeContext<IInformationTabUi>(_driver)
                .SelectComplianceStatus("Cannot withdraw")
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

            // create withdrawal in tp
            var actualCreateWithdrawalError = _apiFactory
               .ChangeContext<IWithdrawalTpApi>()
               .PostPendingWithdrawalRequest(_tradingPlatformUrl,
               _loginData, _withdrawalAmount, checkStatusCode: false);     

            Assert.Multiple(() =>
            {
                Assert.True(actualComplianceStatusToolTipText == expectedComplianceStatusToolTipText,
                    $" expected Compliance Status Tool Tip Text: {expectedComplianceStatusToolTipText}," +
                    $" actual Compliance Status Tool Tip Text: {actualComplianceStatusToolTipText}");

                Assert.True(actualCreateWithdrawalError.Contains(expectedCreateWithdrawalError),
                    $" expected Create Withdrawal Error Message: {expectedCreateWithdrawalError}," +
                    $" actual Create Withdrawal Error Message: {actualCreateWithdrawalError}");
            });
        }
    }
}