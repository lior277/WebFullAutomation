using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Login
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyLoginAfterResetPassword : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private IWebDriver _driver;
        private string _browserName;
        private string _userEmail;
        private string _clientEmail;
        private string _testimUrl = DataRep.TesimUrl;
        private string _resetPasswordTPLink;
        private string _resetPasswordErpLink;

        public VerifyLoginAfterResetPassword(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();
            var subjectResetTPPassword = "Reset Password";
            var emailBodyParams = new List<string> { "LINK_RESET_PASSWORD" };

            // create user for the creation of api key
            var userName = TextManipulation.RandomString();
            _userEmail = userName + DataRep.TestimEmailPrefix;

            // create user
            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName,
             emailPrefix: DataRep.TestimEmailPrefix);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
              emailPrefix: DataRep.TestimEmailPrefix);

            var emailsParams = new Dictionary<string, string> {
                { "type", "reset_password" }, { "language", "en" }, { "subject", subjectResetTPPassword }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the mail Reset Password
            _apiFactory
                .ChangeContext<IClientCardApi>()
                .PostResetPasswordRequest(_crmUrl, _clientId);

            var email = _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .FilterEmailBySubject(_testimUrl, _clientEmail, subjectResetTPPassword)
               .First();

            _resetPasswordTPLink = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email)
                .Values
                .Last();

            var subjectResetErpPassword = "Reset Password";
            emailBodyParams = new List<string> { "LINK_RESET_PASSWORD" };

            emailsParams = new Dictionary<string, string> {
                { "type", "forget_password" }, { "language", "en" }, { "subject", subjectResetErpPassword }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the mail
            _apiFactory
                .ChangeContext<IUsersApi>()
                .PostForgotPasswordRequest(_crmUrl, _userEmail);

            email = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(_testimUrl, _userEmail, subjectResetErpPassword)
                .First();

            _resetPasswordErpLink = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email)
                .Values
                .Last();
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
        public void VerifyLoginAfterResetPasswordTest()
        {
            var newPassword = DataRep.Password + "New";
            _driver.Navigate().GoToUrl(_resetPasswordTPLink);
            _driver.Manage().Window.Maximize();
            var changePasswordTPUrl = _driver.Url;

            // change Tp password
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .ChangePasswordPipe(newPassword, changePasswordTPUrl);

            var TPUrl = _driver.Url;

            _driver.Navigate().GoToUrl(_resetPasswordErpLink);
            var changePasswordErpUrl = _driver.Url;

            // change Erp password
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .ChangePasswordPipe(newPassword, changePasswordErpUrl);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userEmail, _crmUrl, newPassword);

            var crmUrl = _driver.Url;

            Assert.Multiple(() =>
            {
                Assert.True(TPUrl.Contains(_tradingPlatformUrl),
                    $"expected url: {TPUrl} Contains: {_tradingPlatformUrl}");

                Assert.True(crmUrl.Contains(crmUrl),
                    $"expected url: {crmUrl} Contains: {_crmUrl}");
            });
        }
    }
}