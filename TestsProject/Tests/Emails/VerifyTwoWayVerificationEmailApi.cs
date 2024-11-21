// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyTwoWayVerificationEmailApi : TestSuitBase
    {
        public VerifyTwoWayVerificationEmailApi(string browser) : base(browser) { }

        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userEmail;
        private string _testimUrl = DataRep.TesimUrl;
        private string _testimEmailPerfix = DataRep.TestimEmailPrefix;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _driver = GetDriver();
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
        public void VerifyTwoWayVerificationEmailApiTest()
        {
            var subject = "Email Verification - BrandName";
            var verificationLinkContains = "device-verification";
            var emailBodyParams = new List<string> { "VERIFICATION_LINK" };

            var emailsParams = new Dictionary<string, string> { 
                { "type", "2_way_verification" }, 
                { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>(_driver)
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the mail
            var userName = TextManipulation.RandomString();
            _userEmail = userName + _testimEmailPerfix;

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName,
                emailPrefix: _testimEmailPerfix);

            var email = _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .FilterEmailBySubject(_testimUrl, _userEmail, subject)
                .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .ParseMailToKeyValuePair(email);

            Assert.Multiple(() =>
            {
                Assert.True(actualEmailBody["VERIFICATION_LINK"].Contains(verificationLinkContains),
                    $" expected verification Link Contains: {verificationLinkContains}" +
                    $" actual verification Link: {actualEmailBody["VERIFICATION_LINK"]}");

                Assert.True(email.Subject == subject,
                    $"actual email subject: {email.Subject}" +
                    $"expected email subject: {subject}");
            });
        }
    }
}