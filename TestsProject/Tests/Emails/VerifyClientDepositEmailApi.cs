// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyClientDepositEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _clientEmail;
        private int _depositAmount = 100;
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

            _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PostDepositRequest(_crmUrl, _clientId, _depositAmount);
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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyClientDepositEmailApiTest()
        {
            var subject = "Deposit Confirmation for automation";
            var emailBodyParams = new List<string> { "DEPOSIT_AMOUNT", "DEPOSIT_TRASACTION_ID" };
            var pspTransactionId = "505"; // this is the default value in public IFinancesTabApi SetPspTransactionId(string pspTransactionId = "505");
            var emailsParams = new Dictionary<string, string> { 
                { "type", "client_deposit" }, { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the mail
           var actualDepositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            var email = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(_testimUrl, _clientEmail, subject)
                .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email);

            Assert.Multiple(() =>
            {
                Assert.True(actualEmailBody["DEPOSIT_AMOUNT"] == $"{_depositAmount}.00", 
                    $" expected deposit amount: {_depositAmount}" +
                    $" actual deposit amount: {actualEmailBody["DEPOSIT_AMOUNT"]}");

                Assert.True(actualEmailBody["DEPOSIT_TRASACTION_ID"].TrimStart().TrimEnd() == pspTransactionId,
                    $" actual deposit transaction id from email: {actualEmailBody["DEPOSIT_TRASACTION_ID"]}" +
                    $" expected deposit transaction id from deposit: {pspTransactionId}");

                Assert.True(email.Subject == subject,
                    $" actual email subject: {email.Subject}" +
                    $" expected email subject: {subject}");
            });
        }
    }
}