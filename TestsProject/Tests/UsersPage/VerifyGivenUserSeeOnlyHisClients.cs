using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Linq;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.UsersPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyGivenUserSeeOnlyHisClients : TestSuitBase
    {
        #region Test Preparation
        public VerifyGivenUserSeeOnlyHisClients(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IWebDriver _driver;
        private string _agentUserName;
        private string _browserName;
        private string _userApiKey; 
        private string _clientEmail;
        private string _clientName; 

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            _userApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            var adminUserEmail = userName + DataRep.EmailPrefix;

            _agentUserName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl,
                _agentUserName, role: "agent", apiKey: _userApiKey);

            // create client 
            _clientName = TextManipulation.RandomString();
            _clientEmail = _clientName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl,
                _clientName, apiKey: _userApiKey);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(adminUserEmail);

            // navigate to clients page
            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>();
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
        [Category(DataRep.TestCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyGivenUserSeeOnlyHisClientsTest()
        {
            var agentUserEmail = _agentUserName + DataRep.EmailPrefix;

            // Select Sales Agent
            _apiFactory
                .ChangeContext<IClientsPageUi>(_driver)
                .SelectSalesAgent(_agentUserName);

            // login with agent User Name
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_agentUserName, _crmUrl,
                deleteCookies: true);

            // Add Finger Print
            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PatchAddFingerPrintPipe(_crmUrl, agentUserEmail);

           // login with agent User Name
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_agentUserName);

            var clientDitails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .WaitForClientsTableToLoad()
                .ChangeContext<ISearchResultsUi>(_driver)
                .GetSearchResultDetails<SearchResultClients>().First();

            Assert.Multiple(() =>
            {
                Assert.True(clientDitails.fullname == 
                    $"{_clientName.UpperCaseFirstLetter()} {_clientName.UpperCaseFirstLetter()}",
                    $"expected client full name: {_clientName}" +
                    $" actual client full name: {clientDitails.fullname}");

                Assert.True(clientDitails.country == "Afghanistan",
                    $"expected client country: Afghanistan, " +
                    $" actual client country:  {clientDitails.country}");

                Assert.True(clientDitails.balance == "0.00 $",
                    $"expected client _balance: 0.00 $" +
                    $" actual client _balance: {clientDitails.balance}");

                Assert.True(clientDitails.salesagent == _agentUserName,
                    $"expected client sales agent: {_agentUserName}" +
                    $" actual client salesa gent: {clientDitails.salesagent}");
            });
        }
    }
}
