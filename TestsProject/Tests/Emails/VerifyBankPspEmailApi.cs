// Ignore Spelling: Psp Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyBankPspEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _clientEmail;
        private string _testimUrl = DataRep.TesimUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private GetLoginResponse _loginData;

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

            _loginData = _apiFactory
             .ChangeContext<ILoginApi>()
             .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
             .GeneralResponse;

            // create psp for deposit page
            //_apiFactory
            //  .ChangeContext<IPspTabApi>()
            //  .PostCreateBankTransferPspRequest(_crmUrl);
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
        public void VerifyBankPspEmailApiTest()
        {
            var pspTitle = "bank transfer title";
            var pspBody = "bank transfer body";
            var subject = "Wire Bank Details - BRAND_NAME";
            var emailBodyParams = new List<string> { "WALLET_TITLE", "WALLET_BODY" };
            var emailsParams = new Dictionary<string, string> {  { "type", "bank_psp" },
                { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the mail
          _apiFactory
               .ChangeContext<ITradeDepositPageApi>()
               .PostSendCustomPspBankDetailsRequest(_tradingPlatformUrl, pspBody, pspTitle, _loginData);

            var email = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(_testimUrl, _clientEmail, subject)
                .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email);

            Assert.Multiple(() =>
            {
                Assert.True(actualEmailBody["WALLET_TITLE"] == pspTitle,
                    $" expected TITLE: {pspTitle}" +
                    $" actual TITLE: {actualEmailBody["WALLET_TITLE"]}");

                Assert.True(actualEmailBody["WALLET_BODY"].TrimEnd().TrimStart() == pspBody,
                    $" expected BODY: {pspBody}" +
                    $" actual BODY: {actualEmailBody["WALLET_BODY"]}");

                Assert.True(email.Subject == subject,
                    $" actual email subject: {email.Subject}" +
                    $" expected email subject: {subject}");
            });
        }
    }
}