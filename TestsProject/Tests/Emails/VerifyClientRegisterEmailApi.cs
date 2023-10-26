// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyClientRegisterEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _testimUrl = DataRep.TesimUrl;
        private string _testimEmailPerfix = DataRep.TestimEmailPrefix;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
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
        public void VerifyClientRegisterEmailApiTest()
        {
            var subject = "Welcome To BrandName!";
            var clientName = TextManipulation.RandomString();
            var  clientEmail = clientName + _testimEmailPerfix;
            var emailBodyParams = new List<string> { "LINK_RESET_PASSWORD", "BRAND_NAME" };

            var emailsParams = new Dictionary<string, string> { 
                { "type", "client_register" }, { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the mail
            _apiFactory
               .ChangeContext<ICreateClientApi>()
               .RegisterClientWithCampaign(_crmUrl,
               clientName: clientName,
               emailPrefix: _testimEmailPerfix);

            var email = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(_testimUrl, clientEmail, subject)
                .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email);

            Assert.Multiple(() =>
            {
                Assert.True(email.Body.Contains($"LINK_RESET_PASSWORD="),
                    $"actual email body {email.Body} not contain : trade.airsoftltd.com/reset-password?id=");

                Assert.True(email.Body.Contains($"BRAND_NAME=BrandName"),
                    $"actual email body {email.Body} not contain : BRAND_NAME=BrandName");

                Assert.True(email.Subject == subject,
                    $" actual email subject: {email.Subject}" +
                    $" expected email subject: {subject}");
            });
        }
    }
}