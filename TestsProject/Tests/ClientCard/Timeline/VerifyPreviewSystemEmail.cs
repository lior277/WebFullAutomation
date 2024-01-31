// Ignore Spelling: TimeLine

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard.TimeLine
{
    [NonParallelizable]
    [TestFixture(DataRep.Chrome)]
    public class VerifyPreviewSystemEmail : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private int _depositAmount = 100;
        private string _pspTransactionId;
        private IWebDriver _driver;

        public VerifyPreviewSystemEmail(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _driver = GetDriver();
            var dbContext = new QaAutomation01Context();

            var subject = "first deposit";
            var emailBodyParams = new List<string>
            { "DEPOSIT_AMOUNT", "DEPOSIT_TRASACTION_ID"};

            var emailsParams = new Dictionary<string, string> {
                { "type", "first_deposit" }, { "language", "en" },
                { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // create user for the creation of api key
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // get ApiKey
            var userApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, apiKey: userApiKey);

            var clientIds = new List<string> { clientId };

            _apiFactory
               .ChangeContext<IFinancesTabApi>(_driver)
               .PostDepositRequest(_crmUrl, clientId, _depositAmount, apiKey: userApiKey);

            // deposit id
            _pspTransactionId =
              (from s in dbContext.FundsTransactions
               where (s.UserId == clientId && s.Amount == _depositAmount && s.Type == "deposit")
               select s.PspTransactionId)
               .First();

            // login and navigate to time line tab
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .SearchClientByEmail(clientName)
                .ClickOnClientFullName()
                .ClickOnTimelineTab();           
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyPreviewSystemEmailTest()
        {
            var expectedEnvelopeBoxTitle = "first deposit";
            var expectedEnvelopeBoxBody = $"DEPOSIT_AMOUNT={_depositAmount}.00" +
                $",DEPOSIT_TRASACTION_ID={_pspTransactionId}";          

            var actualEnvelopeBoxTitle = _apiFactory
                .ChangeContext<ITimelineTabUi>(_driver)
                .ClickOnEnvelope()
                .GetEnvelopeBoxTitle();

            var actualEnvelopeBoxBody = _apiFactory
                .ChangeContext<ITimelineTabUi>(_driver)
                .GetEnvelopeBoxBody();

            Assert.Multiple(() =>
            {
                Assert.True(expectedEnvelopeBoxTitle == actualEnvelopeBoxTitle, 
                    $" expected EnvelopeBox Title: {expectedEnvelopeBoxTitle}" +
                    $" actual EnvelopeBox Title: {actualEnvelopeBoxTitle}");

                Assert.True(expectedEnvelopeBoxBody == actualEnvelopeBoxBody,
                    $" expected EnvelopeBox Body: {expectedEnvelopeBoxBody}" +
                    $" actual EnvelopeBox Body: {actualEnvelopeBoxBody}");
            });
        }
    }
}