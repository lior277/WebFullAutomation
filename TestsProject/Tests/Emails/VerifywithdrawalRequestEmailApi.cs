// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyWithdrawalRequestEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private int _depositAmount = 500;
        private int _withdrawalAmount = 40;
        private string _clientEmail;
        private string _testimUrl = DataRep.TesimUrl;
        private GetLoginResponse _loginData;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;

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

            // login data
            _loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            _apiFactory
             .ChangeContext<IFinancesTabApi>()
             .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            _apiFactory
            .ChangeContext<IWithdrawalTpApi>()
            .PostPendingWithdrawalRequest(_tradingPlatformUrl, _loginData, _withdrawalAmount);          
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
        public void VerifyWithdrawalRequestEmailApiTest()
        {
            var subject = "Withdrawal request";
            var emailBodyParams = new List<string> { "EMAIL BODY IS EMPTY" };
            var emailsParams = new Dictionary<string, string> {  
                { "type", "withdrawal_request" }, { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the mail
            _apiFactory
             .ChangeContext<IWithdrawalTpApi>()
             .PostPendingWithdrawalRequest(_tradingPlatformUrl, _loginData, _withdrawalAmount);

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