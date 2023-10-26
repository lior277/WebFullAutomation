// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyFirstDepositEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _currentUserApiKey;
        private string _clientEmail;
        private int _depositAmount = 500;
        private string _testimUrl = DataRep.TesimUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName,
             role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);
            #endregion

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                emailPrefix: DataRep.TestimEmailPrefix,
                apiKey: _currentUserApiKey);
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
        public void VerifyFirstDepositEmailApiTest()
        {
            var subject = "first deposit";
            var emailBodyParams = new List<string>
            { "DEPOSIT_AMOUNT", "DEPOSIT_TRASACTION_ID"};

            var emailsParams = new Dictionary<string, string> { 
                { "type", "first_deposit" }, { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the mail
           var actualDepositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount, apiKey: _currentUserApiKey);

            var actualTransactionId = _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .GetFinanceDataRequest(_crmUrl, _clientId)
               .GeneralResponse[0]
               .psp_transaction_id
               .ToString();

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

                Assert.True(actualEmailBody["DEPOSIT_TRASACTION_ID"].TrimStart().TrimEnd() == actualTransactionId,
                    $"actual deposit transaction id from email: {actualEmailBody["DEPOSIT_TRASACTION_ID"]}" +
                    $"expected deposit transaction id from deposit: {actualTransactionId}");

                Assert.True(email.Subject == subject,
                    $"actual email subject: {email.Subject} " +
                    $"expected email subject: {subject}");
            });
        }
    }
}