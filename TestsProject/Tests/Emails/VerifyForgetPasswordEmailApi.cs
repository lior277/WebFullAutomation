// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyForgetPasswordEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory;
        private string _clientId;
        private string _clientEmail;
        private string _testimUrl = DataRep.TesimUrl;
        private GetLoginResponse _loginData;
        private string _crmUrl = Config.appSettings.CrmUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _apiFactory = new ApplicationFactory();
            var tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                emailPrefix: DataRep.TestimEmailPrefix);

            // login notification
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(tradingPlatformUrl, _clientEmail)
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
                AfterTest();
            }
        }
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyForgetPasswordEmailApiTest()
        {
            var subject = "Forget password";
            var expectedForgetPassword = "reset-password?id";
            var emailBodyParams = new List<string> { "LINK_RESET_PASSWORD" };

            var emailsParams = new Dictionary<string, string> { 
                { "type", "forget_password" }, { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the mail
           _apiFactory
                .ChangeContext<IClientCardApi>()
                .PostForgotPasswordRequest(_crmUrl, _clientEmail, _loginData);

            var email = _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .FilterEmailBySubject(_testimUrl, _clientEmail, subject)
               .First();

            var actualEmailBody = _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .ParseMailToKeyValuePair(email);

            Assert.Multiple(() =>
            {
                Assert.True(actualEmailBody["LINK_RESET_PASSWORD"].Contains(expectedForgetPassword),
                    $"actual email body {email.Body} not contain : {expectedForgetPassword}");

                Assert.True(email.Subject == subject,
                    $"actual email subject: {email.Subject}" +
                    $"expected email subject: {subject}");
            });
        }
    }
}