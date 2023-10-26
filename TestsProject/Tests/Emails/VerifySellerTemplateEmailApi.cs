using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifySellerTemplateEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _userEmail;
        private string _userName;
        private string _userId;
        private string _currentUserApiKey;
        private string _clientEmail;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            // create user
            _userName = TextManipulation.RandomString();
            _userEmail = $"{_userName}{DataRep.TestimEmailPrefix}";

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName,
                emailPrefix: DataRep.TestimEmailPrefix);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
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
        public void VerifySellerTemplateEmailApiTest()
        {
            var subject = $"Test{TextManipulation.RandomString()}";

            var emailBodyParams = new List<string> { "SELLER_FIRST_NAME",
                "SELLER_LAST_NAME", "SELLER_EMAIL",
                "SELLER_PHONE", "SELLER_PHONE_EXTENSION" };

            var emailsParams = new Dictionary<string, string> { 
                { "type", "custom" }, { "language", "en" },
                { "subject", subject }, { "name", _userEmail }};

            // update the existing template
            var emailBody = _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateCustomEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams)
               .ComposeEmailBody(emailBodyParams);
          
            // send direct email from client card 
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .PostSendCustomEmailRequest(_crmUrl, emailBody,
                _clientId, _userEmail, subject, apiKey: _currentUserApiKey);

            // get direct email
            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .GetEmailsRequest(DataRep.TesimUrl, _clientEmail)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualEmailBody.Body.Contains(_userName.UpperCaseFirstLetter()),
                    $" expected SELLER_FIRST_NAME: {_userName}" +
                    $" actual SELLER_FIRST_NAME: {actualEmailBody.Body}");

                Assert.True(actualEmailBody.Body.Contains(_userName.UpperCaseFirstLetter()),
                    $" expected SELLER_LAST_NAME: {_userName}" +
                    $" actual SELLER_LAST_NAME: {actualEmailBody.Body}");

                Assert.True(actualEmailBody.Body.Contains(_userEmail),
                    $" expected SELLER_EMAIL: {_userEmail}" +
                    $" actual SELLER_EMAIL: {actualEmailBody.Body}");

                Assert.True(actualEmailBody.Body.Contains(DataRep.UserDefaultPhone),
                    $" expected SELLER_PHONE: {DataRep.UserDefaultPhone}" +
                    $" actual SELLER_PHONE: {actualEmailBody.Body}");

                Assert.True(actualEmailBody.Body.Contains(DataRep.UserDefaultPhone),
                    $" expected SELLER_PHONE_EXTENSION: {DataRep.UserDefaultPhone}" +
                    $" actual SELLER_PHONE_EXTENSION: {actualEmailBody.Body}");

                Assert.True(actualEmailBody.Subject == subject,
                    $" actual email subject: {actualEmailBody.Subject}" +
                    $" expected email subject: {subject}");
            });
        }
    }
}