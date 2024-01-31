using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyMakeCallFromClientsPage : TestSuitBase
    {
        public VerifyMakeCallFromClientsPage(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;
        private string _browserName;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition   
            _driver = GetDriver();

            // create user
            var userName = TextManipulation.RandomString();
            var pbxName = TextManipulation.RandomString();

            // create office
            var officeName = TextManipulation.RandomString();

            var officeId = _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .PostCreateOfficeRequest(_crmUrl, officeName, pbxName)
                .GetOfficesByName(_crmUrl, officeName)
                ._id;

            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName, pbxName: pbxName,
               role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create client 
            var clientName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: currentUserApiKey);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName)
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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyMakeCallFromClientsPageTest()
        {
            // verify call icon
            _apiFactory
                .ChangeContext<IClientsPageUi>(_driver)
                .ClickOnPhoneIconButton()
                .ClickOnConfimationCallPoUp()
                .VerifyPhoneCallAnimation();

            // get user to delete the extension
            var userData = _apiFactory
                .ChangeContext<IUsersApi>(_driver)
                .GetUserByIdRequest(_crmUrl, _userId)
                .GeneralResponse;

            // edit user with no extension
            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PutEditUserOfficeRequest(_crmUrl, userData);

            _driver.Navigate().Refresh();

            // verify there is no call icon
            _apiFactory
                .ChangeContext<IClientsPageUi>(_driver)
                .VerifyNoPhoneCallButton();
        }
    }
}