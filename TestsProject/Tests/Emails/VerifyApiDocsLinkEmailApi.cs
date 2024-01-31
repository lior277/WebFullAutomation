using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyApiDocsLinkEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _testimUrl = DataRep.TesimUrl;
        private string _userId;
        private string _userEmail;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            var userName = TextManipulation.RandomString();
            _userEmail = userName + DataRep.TestimEmailPrefix;

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName,
                emailPrefix: DataRep.TestimEmailPrefix);      
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
            }
            finally
            {
                AfterTest();
            }
        }
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyApiDocsLinkEmailApiTest()
        {
            var subject = "API Docs";
            var bodyContains = "users/docs?api_key";
            var emailBodyParams = new List<string> { "API_DOCS_LINK" };
            var emailsParams = new Dictionary<string, string> { 
                { "type", "api_docs_link" }, { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the email
            _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            var email = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(_testimUrl, _userEmail, subject)
                .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email);

            Assert.Multiple(() =>
            {
                Assert.True(actualEmailBody["API_DOCS_LINK"].Contains(bodyContains),
                    $" expected API DOCS LINK: { bodyContains}" +
                    $" actual API DOCS LINK: {actualEmailBody["API_DOCS_LINK"]}");

                Assert.True(email.Subject == subject,
                    $" actual email subject: {email.Subject}" +
                    $" expected email subject: {subject}");
            });
        }
    }
}