using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.UsersPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyLastLoginDataAfterUserLogin : TestSuitBase
    {
        #region Test Preparation
        public VerifyLastLoginDataAfterUserLogin(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private string _currentUserApiKey;
        private string _userId;
        private string _userName;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            _userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            _currentUserApiKey = _apiFactory
               .ChangeContext<IUserApi>()
               .PostCreateApiKeyRequest(_crmUrl, _userId);
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyLastLoginDataAfterUserLoginTest()
        {
            var expectedDate = DateTime.Now.ToString("MM/dd/yyyy");

            // create user with long first name
            var createUserErrorMessage = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetUserLastLoginsTimelineRequest(_crmUrl, _userId, _currentUserApiKey)
                .FirstOrDefault();            

            Assert.Multiple(() =>
            {
                Assert.True(createUserErrorMessage._id != null,
                    $" expected id: not null, " +
                    $" actual id: {createUserErrorMessage._id}");

                Assert.True(createUserErrorMessage.created_at.ToString("MM/dd/yyyy").Contains(expectedDate),
                    $" expected created_at: {expectedDate}, " +
                    $" actual created_at: {createUserErrorMessage.created_at}");

                Assert.True(createUserErrorMessage.erp_user_id == _userId,
                    $" expected erp_user_id: {_userId}, " +
                    $" actual erp_user_id: {createUserErrorMessage.erp_user_id}");

                Assert.True(createUserErrorMessage.erp_username == _userName,
                    $" expected _userName: {_userName}, " +
                    $" actual _userName: {createUserErrorMessage.erp_username}");

                Assert.True(createUserErrorMessage.from_browser == "Chrome",
                    $" expected from_browser: Chrome, " +
                    $" actual from_browser: {createUserErrorMessage.from_browser}");

                Assert.True(createUserErrorMessage.from_device == "no data",
                    $" expected from_device: no data, " +
                    $" actual from_device: {createUserErrorMessage.from_device}");

                Assert.True(createUserErrorMessage.logout_date.ToString("MM/dd/yyyy").Contains(expectedDate),
                    $" expected logout_date: not null, " +
                    $" actual logout_date: {createUserErrorMessage.logout_date}");

                Assert.True(createUserErrorMessage.platform == "erp",
                    $" expected platform: erp, " +
                    $" actual platform: {createUserErrorMessage.platform}");

                Assert.True(createUserErrorMessage.user_full_name == $"{_userName} {_userName}",
                    $" expected user_full_name: _userName, " +
                    $" actual user_full_name: {createUserErrorMessage.user_full_name}");

                Assert.True(createUserErrorMessage.user_id == _userId,
                    $" expected user_id: {_userId}, " +
                    $" actual user_id: {createUserErrorMessage.user_id}");       
            });
        }
    }
}
