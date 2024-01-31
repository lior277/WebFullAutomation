// Ignore Spelling: Api

using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage.Filters
{
    [TestFixture(DataRep.Chrome)]
    [TestFixture]
    public class VerifyDepositFilterOnClientsPageApi : TestSuitBase
    {
        public VerifyDepositFilterOnClientsPageApi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region members
        private string _firstClientsId;
        private string _firstClientEmail;
        private string _secondClientEmail;
        private string _depositId;
        private int _firstDepositAmount = 400;
        private string _browserName;
        private string _userId;
        private IWebDriver _driver;
        private string _currentUserApiKey;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        #endregion      

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            _driver = GetDriver();

            var userName = TextManipulation.RandomString();

            // create user
            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            // create first client
            var firstClientName = TextManipulation.RandomString();
            _firstClientEmail = firstClientName + DataRep.EmailPrefix;

            _firstClientsId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, firstClientName,
                apiKey: _currentUserApiKey);

            // first deposit 400
            _depositId = _apiFactory
                 .ChangeContext<IFinancesTabApi>()
                 .PostDepositRequest(_crmUrl,
                 _firstClientsId, _firstDepositAmount,
                 apiKey: _currentUserApiKey);

            // create second client
            var secondClientName = TextManipulation.RandomString();
            _secondClientEmail = secondClientName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, secondClientName,
                apiKey: _currentUserApiKey);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName, _crmUrl);
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

        [Test]
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-5054")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyDepositFilterOnClientsPageApiTest()
        {
            var dbContext = new QaAutomation01Context();
            var filterName = "custom_deposit";

            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .ChangeContext<IHandleFiltersUi>(_driver)
                .ClickToOpenFiltersMenu()
                .SingleSelectDropDownPipe(DataRep.DepositFilter, "Has deposits")
                .ChangeContext<IClientsPageUi>(_driver)
                .WaitForNumOfRowsInClientsTable(1);

            var actualClientWithDeposit = _apiFactory
                .ChangeContext<ISearchResultsUi>(_driver)
                .GetSearchResultDetails<SearchResultClients>()
                .First();

            var actualClientNoDeposit = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByFilterRequest(_crmUrl, filterName, "no_deposits",
                _currentUserApiKey)
                .GeneralResponse
                .data
                .First();

            var between1And2WeeksAgo = DateTime.Now.AddDays(-10).ToString("yyyy-MM-ddTHH:mm:ss");

            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PutLastDepositDateRequest(_crmUrl, _firstClientsId, between1And2WeeksAgo,
                _currentUserApiKey);

            var actualClientDepositBetween1And2WeeksAgo = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByFilterRequest(_crmUrl, filterName, "deposit_between_1_and_2_weeks_ago",
                _currentUserApiKey)
                .GeneralResponse
                .data
                .First();

            var lessThanAWeek = DateTime.Now.AddDays(-5).ToString("yyyy-MM-ddTHH:mm:ss");

            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PutLastDepositDateRequest(_crmUrl, _firstClientsId, lessThanAWeek,
                _currentUserApiKey);

            var actualClientDepositLessThanAWeek = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByFilterRequest(_crmUrl, filterName, "deposit_less_than_a_week",
                _currentUserApiKey)
                .GeneralResponse
                .data
                .First();

            var moreThanAMonthAgo = DateTime.Now.AddMonths(-1).AddDays(-4)
                .ToString("yyyy-MM-ddTHH:mm:ss");

            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PutLastDepositDateRequest(_crmUrl, _firstClientsId, moreThanAMonthAgo,
                _currentUserApiKey);

            var actualClientDepositMoreThanAMonthAgo = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByFilterRequest(_crmUrl, filterName, "deposit_more_than_a_month_ago",
                _currentUserApiKey)
                .GeneralResponse
                .data
                .First();

            Assert.Multiple(() =>
            {
                Assert.True(actualClientWithDeposit.email == _firstClientEmail,
                    $" expected Client With Deposit: {_firstClientEmail}" +
                    $" actual Client With Deposit :  {actualClientWithDeposit.email}");

                Assert.True(actualClientNoDeposit.email == _secondClientEmail,
                   $" expected Client No Deposit: {_secondClientEmail}" +
                   $" actual Client No Deposit :  {actualClientNoDeposit.email}");

                Assert.True(actualClientDepositBetween1And2WeeksAgo.email == _firstClientEmail,
                   $" expected Client Deposit Between 1 And 2 Weeks Ago: {_firstClientEmail}" +
                   $" actual Client Deposit Between 1 And 2 Weeks Ago :  {actualClientDepositBetween1And2WeeksAgo.email}");

                Assert.True(actualClientDepositLessThanAWeek.email == _firstClientEmail,
                   $" expected Deposit Less Than A Week: {_firstClientEmail}" +
                   $" actual Deposit Less Than A Week :  {actualClientDepositLessThanAWeek.email}");

                Assert.True(actualClientDepositMoreThanAMonthAgo.email == _firstClientEmail,
                   $" expected Client Deposit More Than A Month Ago: {_firstClientEmail}" +
                   $" actual Client Deposit More Than A Month Ago :  {actualClientDepositMoreThanAMonthAgo.email}");
            });
        }
    }
}
