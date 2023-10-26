// Ignore Spelling: Kyc TimeLine Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using Microsoft.Graph;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyKycVerifiedEmailAndTimelineApi : TestSuitBase
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
        public void VerifyKycVerifiedEmailAndTimelineApiTest()
        {
            var subject = "kyc verified";
            var emailBodyParams = new List<string> { "CLIENT_ID" };

            var emailsParams = new Dictionary<string, string> {
                { "type", "kyc_verified" }, { "language", "en" },
                { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // create client
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                 emailPrefix: DataRep.TestimEmailPrefix,
                apiKey: _currentUserApiKey);

            var informationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, clientId)
                .GeneralResponse
                .informationTab;

            informationTabResponse.kyc_status = "Verified";

            // trigger the mail by changing the kyc status to Verified
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, informationTabResponse,
                apiKey: _currentUserApiKey);

            var email = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(DataRep.TesimUrl, _clientEmail, subject)
                .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email);

            Assert.Multiple(() =>
            {
                Assert.True(actualEmailBody["CLIENT_ID"] == clientId,
                    $" expected client Id: {clientId}" +
                    $" actual client Id: {actualEmailBody["CLIENT_ID"]}");

                Assert.True(email.Subject == subject,
                    $"actual email subject: {email.Subject} " +
                    $"expected email subject: {subject}");
            });
        }
    }
}