// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyDepositRemainderEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;      
        private string _clientEmail;
        private string _currentUserApiKey;
        private string _userName;   

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            // create user
            _userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // update Remind About Deposit
            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutRemindAboutDepositRequest(_crmUrl); 
            #endregion
        }
        #endregion

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
        
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyDepositRemainderEmailApiTest()
        {
            var subject = "Deposit Remainder";
            var emailBodyParams = new List<string> { "SELLER_FIRST_NAME",
                "SELLER_LAST_NAME", "SELLER_EMAIL" };

            var emailsParams = new Dictionary<string, string> {
                { "type", "remind_deposit" }, { "language", "en" },
                { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // create client
            // trigger the mail
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                emailPrefix: DataRep.TestimEmailPrefix,
                apiKey: _currentUserApiKey);

            var email = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(DataRep.TesimUrl, _clientEmail, subject)
                .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email);

            // update Remind About Deposit
            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutRemindAboutDepositRequest(_crmUrl, 0.25);

            // create client
            // trigger the mail
            clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                emailPrefix: DataRep.TestimEmailPrefix,
                apiKey: _currentUserApiKey);

            // create deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId, 10);

            var actualEmailBodyForClientWithDeposit = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .GetEmailsRequest(DataRep.TesimUrl, _clientEmail);

            Assert.Multiple(() =>
            {
                Assert.True(actualEmailBody["SELLER_FIRST_NAME"] == _userName,
                    $" expected user first name: {_userName}" +
                    $" actual user first name: {actualEmailBody["SELLER_FIRST_NAME"]}");

                Assert.True(actualEmailBody["SELLER_LAST_NAME"] == _userName,
                    $" expected user last name: {_userName}" +
                    $" actual user last name: {actualEmailBody["SELLER_LAST_NAME"]}");

                Assert.True(actualEmailBody["SELLER_EMAIL"] == $"{ _userName}@auto.local",
                    $" expected user email: {_userName}@auto.local" +
                    $" actual user email: {actualEmailBody["SELLER_EMAIL"]}");

                Assert.True(email.Subject == subject,
                    $"actual email subject: {email.Subject} " +
                    $"expected email subject: {subject}");

                Assert.True(actualEmailBodyForClientWithDeposit
                    .All(p => !p.Subject.Contains(subject)),
                    $"actual email subject: {email.Subject} " +
                    $"expected email subject: {subject}");
            });
        }
    }
}