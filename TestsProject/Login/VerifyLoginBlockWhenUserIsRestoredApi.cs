// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Login
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyLoginBlockWhenUserIsRestoredApi : TestSuitBase
    {
        public VerifyLoginBlockWhenUserIsRestoredApi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IWebDriver _driver;
        private string _browserName;
        private string _fingerPrint;
        private string _userEmail;
        private GetUserResponse _getUserResponse;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();

            // create user
            var userName = TextManipulation.RandomString();
            _userEmail = userName + DataRep.EmailPrefix;

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            _fingerPrint = _apiFactory
                .ChangeContext<IUserApi>()
                .GetAllowedFingerPrintFromMongo(_userEmail);

            _getUserResponse = _apiFactory
                .ChangeContext<IUsersApi>(_driver)
                .GetUserByIdRequest(_crmUrl, userId)
                .GeneralResponse;
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyLoginBlockWhenUserIsRestoredApiTest()
        {
            var expectedLoginErrorMessage = "Invalid credentials";

            // delete user
            _apiFactory
                .ChangeContext<IUserApi>()
                .PatchDeleteOrRestoreUserRequest(_crmUrl, _getUserResponse.user._id);

            var actualLoginErrorMessage = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginCrmRequest(_crmUrl, _userEmail, _fingerPrint, checkStatusCode: false)
                .Message;

            // restore user
            _apiFactory
                .ChangeContext<IUserApi>()
                .PatchDeleteOrRestoreUserRequest(_crmUrl, _getUserResponse.user._id);

            // restore the user
            var actualUserEmail = _apiFactory
                  .ChangeContext<ILoginApi>()
                  .PostLoginCrmRequest(_crmUrl, _userEmail, _fingerPrint)
                  .GeneralResponse
                  .Email;

            Assert.Multiple(() =>
            {
                Assert.True(actualLoginErrorMessage.Contains(expectedLoginErrorMessage),
                    $" expected Login Error Message for hidden user: {expectedLoginErrorMessage}" +
                    $" actual Login Error Message for hidden user: {actualLoginErrorMessage}");

                Assert.True(actualUserEmail == _userEmail,
                   $" expected User Email: {_userEmail}" +
                   $" actual User Email: {actualUserEmail}");
            });
        }
    }
}