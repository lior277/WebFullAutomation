// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;

using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using ConsoleApp;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyWithdrawalSuspendedEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private int _depositAmount = 500;
        private int _withdrawalAmount = 40;
        private string _clientEmail;
        private string _currentUserApiKey;  
        private string _withdrawalId;
        private string _testimUrl = DataRep.TesimUrl;
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create user for the creation of api key
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName);

            // get ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                emailPrefix: DataRep.TestimEmailPrefix,
                apiKey: _currentUserApiKey);

            // login data
            var loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            _apiFactory
             .ChangeContext<IFinancesTabApi>()
             .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            _apiFactory
            .ChangeContext<IWithdrawalTpApi>()
            .PostPendingWithdrawalRequest(_tradingPlatformUrl, loginData, _withdrawalAmount);

            // get Withdrawal id
            _withdrawalId =
                (from s in _dbContext.FundsTransactions
                 where (s.UserId == _clientId && Math.Abs(s.Amount)
                 == _withdrawalAmount && s.Type == "withdrawal" && s.Status == "pending")
                 select s.Id).First()
                 .ToString();
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
        public void VerifyWithdrawalSuspendedEmailApiTest()
        {
            var subject = "Withdrawal Status";
            var emailBodyParams = new List<string> { "EMAIL BODY IS EMPTY" };
            var emailsParams = new Dictionary<string, string> {  
                { "type", "withdrawal_suspended" }, { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the mail
           _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PatchWithdrawalStatusRequest(_crmUrl, _clientId,
                _withdrawalId, withdrawalStatus: "rejected", apiKey: _currentUserApiKey);

            var email = _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .FilterEmailBySubject(_testimUrl, _clientEmail, subject)
               .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email);

            Assert.Multiple(() =>
            {
                Assert.True(email.Subject == subject,
                    $" actual email subject: {email.Subject}" +
                    $" expected email subject: {subject}");
            });
        }
    }
}