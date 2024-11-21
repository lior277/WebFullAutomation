// Ignore Spelling: menager

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Dashboard
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyPendingWithdrawalAndStatisticsOnDashboard : TestSuitBase
    {
        #region Test Preparation
        public VerifyPendingWithdrawalAndStatisticsOnDashboard(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private int _depositAmount = 500;
        private string _clientEmail;
        private string _clientId;
        private string _currentUserApiKey;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private int _withdrawalAmount = 40;
        private string _browserName;
        private string _menagerName;
        private string _firstAgentId;    
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            // create user
            var firstAgentName = TextManipulation.RandomString();
            _menagerName = TextManipulation.RandomString();

            // create first agent
            _firstAgentId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, firstAgentName);

            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _firstAgentId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                currency: "EUR", apiKey: _currentUserApiKey);

            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _firstAgentId,
                new List<string> { _clientId }, apiKey: _currentUserApiKey);

            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount,
                apiKey: _currentUserApiKey);

            // create the menager agent
            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _menagerName,
                role: DataRep.AdminWithUsersOnlyRoleName,
                subUsers: new string[] { _firstAgentId });
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
        public void VerifyPendingWithdrawalAndStatisticsOnDashboardTest()
        {
            var clientName = _clientEmail.Split('@').First();
            var date = DateTime.Now.ToString("dd/MM/yyyy");

            // get Boxes Statistics Data 
            var BoxesStatisticsData = _apiFactory
               .ChangeContext<IDashboardApi>(_driver)
               .GetBoxesStatisticsRequest(_crmUrl);

            // Withdrawal 
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostWithdrawalDepositRequest(_crmUrl, _clientId, _firstAgentId,
                _withdrawalAmount, currencyCode: "EUR", apiKey: _currentUserApiKey);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_menagerName);

            // get pending Withdrawal after new Withdrawal
            var expectedPendingWithdrawal = _apiFactory
                .ChangeContext<IDashboardPageUi>(_driver)
                .GetBackCardTotalPendingWithdrawals(_withdrawalAmount);

            // get pending Withdrawal details on pending Withdrawal table
            var WithdrawalDetails =  _apiFactory
                .ChangeContext<IDashboardPageUi>(_driver)
                .SearchWithdrawal(clientName)
                .GetSearchResultDetails<SearchResultWithdrawal>().First();

            Assert.Multiple(() =>
            {
                Assert.True(expectedPendingWithdrawal == _withdrawalAmount,
                    $" expected pending Withdrawal  : {_withdrawalAmount}" +
                    $" actual pending Withdrawal : {expectedPendingWithdrawal}");

                Assert.True(WithdrawalDetails.fullname?.Contains(clientName.UpperCaseFirstLetter()),
                    $"expected client name: {clientName.UpperCaseFirstLetter()}" +
                    $" actual client name contains: {WithdrawalDetails.fullname}");

                Assert.True(WithdrawalDetails.originalamount?.Contains(_withdrawalAmount.ToString()),
                    $"expected withdrawal amount: {_withdrawalAmount}, " +
                    $"expected withdrawal amount: {WithdrawalDetails.originalamount}");

                Assert.True(WithdrawalDetails?.dateofrequestgmt?.Contains(date),
                    $"expected date of deposit: {date}" +
                    $"expected date of deposit: {WithdrawalDetails.dateofrequestgmt}");
            });
        }
    }
}
