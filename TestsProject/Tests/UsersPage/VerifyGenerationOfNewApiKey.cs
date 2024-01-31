// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.UsersMenuUi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.UsersPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyGenerationOfNewApiKey : TestSuitBase
    {
        #region Test Preparation
        public VerifyGenerationOfNewApiKey(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            var firstUserName = TextManipulation.RandomString();

            // create sub user
            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, firstUserName);

            var SecondUserName = TextManipulation.RandomString();

            // create user
            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, SecondUserName,
                role: DataRep.AdminWithUsersOnlyRoleName,
                subUsers: new string[] { userId });

            var userEmail = SecondUserName + DataRep.EmailPrefix;

            _apiFactory
               .ChangeContext<ILoginPageUi>(_driver)
               .LoginPipe(userEmail);
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
        [Description("Based on jira: https://airsoftltd.atlassian.net/browse/AIRV2-5033")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyGenerationOfNewApiKeyTest()
        {
            var expectedFirstNewApiKeyMessage = "You are about to generate an API key. This" +
                " key will allow its owner to send/receive data from your CRM based on the role's" +
                " permission.   Consider this API key as sensitive data.   Thank you. GenerateCancel";

            // Get first Generated ApiKey Body Popup
            var actualFirstNewApiKeyMessage = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IUsersPageUi>()
                .ClickOnEditUserButton()
                .ClickOnSendApiKeyBtn()
                .GetGeneratedApiKeyBodyPopup();

            // generate the first api key
            var firstApiKey = _apiFactory
                .ChangeContext<IUserUi>(_driver)
                .ClickOnGenerateButton()
                .GetApiKey();

            var firstApiKeyStart = firstApiKey.Remove(4);
            var firstApiKeyEnd = firstApiKey.Remove(0, firstApiKey.Length - 5);

            // Get second Generated ApiKey Body Popup
            var actualSecondNewApiKeyMessage = _apiFactory
                .ChangeContext<IUserUi>(_driver)
                .ClickOnCloseUserCardButton()
                .ClickOnEditUserButton()
                .ClickOnSendApiKeyBtn()
                .GetGeneratedApiKeyBodyPopup();
            
            var expectedSecondNewApiKeyMessage = $"You are about to override an API key." +
                $" You are about to override an API key.   This user already has an API key." +
                $"   Generating a new API key will override the previous key and cancel all" +
                $" integrations using it. The actual key assigned to this user is:" +
                $" {firstApiKeyStart}...{firstApiKeyEnd}" +
                $"     If you press \"GENERATE\", this api key will not work anymore and all" +
                $" integration connected to it will crash.   Thank you. GenerateCancel";

            // generate the second api key
            var secondApiKey = _apiFactory
                .ChangeContext<IUserUi>(_driver)
                .ClickOnGenerateButton()
                .GetApiKey();

            // create client with the first api key
            var clientName = TextManipulation.RandomString();

            var actualCreateClientErrorMessage = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                 apiKey: firstApiKey, checkStatusCode: false);

            Assert.Multiple(() =>
            {
                Assert.True(actualFirstNewApiKeyMessage == expectedFirstNewApiKeyMessage,
                    $" expected First New ApiKey Message: {expectedFirstNewApiKeyMessage}, " +
                    $" actual First New ApiKey Message: {actualFirstNewApiKeyMessage}");

                Assert.True(actualSecondNewApiKeyMessage == expectedSecondNewApiKeyMessage,
                    $" expected Second New ApiKey Message: {expectedSecondNewApiKeyMessage}, " +
                    $" actual Second New ApiKey Message: {actualSecondNewApiKeyMessage}");

                Assert.True(actualCreateClientErrorMessage == "Not Found",
                    $" expected Create Client Error Message: not found, " +
                    $" actual Create Client Error Message: {actualCreateClientErrorMessage}");

                Assert.True(firstApiKey != secondApiKey,
                    $" firstApiKey {firstApiKey}, " +
                    $" second ApiKey: {secondApiKey}");
            });
        }
    }
}
