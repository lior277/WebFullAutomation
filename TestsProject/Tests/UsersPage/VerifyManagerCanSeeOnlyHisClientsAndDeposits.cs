using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.UsersPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyManagerCanSeeOnlyHisClientsAndDeposits : TestSuitBase
    {
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _mailPerfix = DataRep.EmailPrefix;
        private List<string> _clientsIds;
        private List<string> _clientsNames;
        private string _browserName;
        private string _menagerName; 

        #region Test Preparation
        public VerifyManagerCanSeeOnlyHisClientsAndDeposits(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            var firstAgentName = TextManipulation.RandomString();
            var secondAgentName = TextManipulation.RandomString();
            _menagerName = TextManipulation.RandomString();

            // create first agent
            var firstAgentId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, firstAgentName, role: "agent");

            // create second agent
            var secondAgentId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, secondAgentName, role: "agent");

            // create client 
            _clientsNames = new List<string> { TextManipulation.RandomString(),
                TextManipulation.RandomString() };

            _clientsIds = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientsNames);

            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, firstAgentId, _clientsIds);

            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientsIds, 10000);

            // create the menager agent
            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _menagerName,
                role: "agent", subUsers: new string[] { firstAgentId, secondAgentId });
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
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyManagerCanSeeOnlyHisClientsAndDepositsTest()
        {
            var totalDeposits = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_menagerName)
                .ChangeContext<IDashboardPageUi>(_driver)
                .VerifyFrontCardTotalCustomers(2)
                .GetBackCardTotalDeposits();

            var clientsFullName = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .ChangeContext<ISearchResultsUi>(_driver)
                .GetClientsFullName();           

            Assert.Multiple(() =>
            {
                Assert.True(clientsFullName.Any(c => _clientsNames.Any(d => d == c)));

                Assert.True(totalDeposits.FirstOrDefault() == "20,000.00 $",
                    $" expexted total Deposits : 20,000.00 $ " +
                    $" actual total Deposits: {totalDeposits.FirstOrDefault()}");
            });
        }
    }
}
