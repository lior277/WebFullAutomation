// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyResetPasswordEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _clientEmail;
        private string _testimUrl = DataRep.TesimUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

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
        public void VerifyResetPasswordEmailApiTest()
        {
            var subject = "Reset Password";
            var emailBodyParams = new List<string> { "LINK_RESET_PASSWORD"};

            var emailsParams = new Dictionary<string, string> { 
                { "type", "reset_password" }, { "language", "en" }, { "subject", subject }};

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
                .FilterEmailBySubject(_testimUrl, _clientEmail, subject)
                .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email);

            Assert.Multiple(() =>
            {
                Assert.True(email.Body.Contains($"reset-password?id="),
                    $"actual email body {email.Body} not contain : trade.airsoftltd.com/reset-password?id=");

                Assert.True(email.Subject == subject,
                    $"actual email subject: {email.Subject}" +
                    $"expected email subject: {subject}");
            });
        }
    }
}