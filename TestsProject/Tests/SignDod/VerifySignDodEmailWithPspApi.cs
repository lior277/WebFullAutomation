using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.SignDod
{
    [TestFixture]
    public class VerifySignDodEmailWithPspApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _dodName = "AutoSignDodOnEmailWithPsp";
        private string _clientEmail;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

            // create user
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                emailPrefix: DataRep.TestimEmailPrefix);

            // update ES Language to active
            _apiFactory
                .ChangeContext<ILanguagesTab>()
                .PutTradingLanguagePipe(_crmUrl, "Español", "es");

            var dodAttributes = new List<string> { "CLIENT_ID" };

            var dodParams = new Dictionary<string, string> {{ "name", _dodName },
                { "language", "es" }, { "sendBy", "email"}, { "depositType", "psp" } };

            // create dod body
            var documentBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ComposeEmailBody(dodAttributes);

            // create dod
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .CreateDodPipe(_crmUrl, dodParams, documentBody);

            // get login Data for trading Platform
            var loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            var dodLanguage = "es";

            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PutLanguage(tradingPlatformUrl, loginData, clientId, dodLanguage);

            var pspInstanceId = _apiFactory
                .ChangeContext<IPspTabApi>()
                .PostCreateAirsoftSandboxPspRequest(_crmUrl)
                .FirstOrDefault()
                .Instances
                .Id;

            var depositAmount = 10;

            _apiFactory
                 .ChangeContext<IFinancesTabApi>()
                 .PostDepositRequest(_crmUrl, clientId, depositAmount,
                 actualMethod: "psp", transactionType: "psp",
                 nameForMethod: DataRep.AirsoftSandboxPspName,
                 pspInstanceId: pspInstanceId, apiKey: currentUserApiKey);
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete  DOD by name
                _apiFactory
                    .ChangeContext<IPlatformTabApi>()
                    .DeleteDod(_crmUrl, _dodName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                AfterTest();
            }
        }
        #endregion

        [Test]
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4149")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifySignDodEmailWithPspApiTest()
        {
            var subject = "AutoSignDodOnEmailWithPsp";
            var testimUrl = DataRep.TesimUrl;

            var emails = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(testimUrl, _clientEmail, subject);

            Assert.True(emails.Any(p => p.Subject.Equals(subject)),
                   $" actual email subject: {emails.Select(p => p.Subject.ToList().ListToString())}" +
                   $" expected email subject: {subject}");
        }
    }
}