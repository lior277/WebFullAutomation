using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
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
    public class VerifyComplianceForActiveStatus : TestSuitBase
    {
        public VerifyComplianceForActiveStatus(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Members
        private string _currentUserApiKey;
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _clientEmail;
        private string _browserName;
        private IWebDriver _driver;
        #endregion        

        [SetUp]
        public void SetUp()
        {
            #region Test Preparation
            BeforeTest();
            _driver = GetDriver();

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyComplianceForActiveStatusTest()
        {
            var expectetComplianceStatusToolTipText = "Active Client can proceed" +
                " with financial and trade actions";

            // get ComplianceS status ToolTip Text 
            var actualComplianceStatusToolTipText = _apiFactory
                .ChangeContext<IInformationTabUi>(_driver)
                .SelectComplianceStatus("Can trade")
                .SelectComplianceStatus("Active")
                .GetComplianceStatusToolTipText();           

            Assert.Multiple(() =>
            {
                Assert.True(actualComplianceStatusToolTipText == expectetComplianceStatusToolTipText,
                    $" expected Compliance Status ToolTip Text: {expectetComplianceStatusToolTipText}," +
                    $" actual Compliance Status ToolTip Text: {actualComplianceStatusToolTipText}");
            });
        }
    }
}