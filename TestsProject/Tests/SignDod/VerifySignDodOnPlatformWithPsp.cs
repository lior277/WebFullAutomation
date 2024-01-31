// Ignore Spelling: Psp

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.SignDod
{
    [NonParallelizable]
    [TestFixture(DataRep.Chrome)]
    public class VerifySignDodOnPlatformWithPsp : TestSuitBase
    {
        #region Test Preparation
        public VerifySignDodOnPlatformWithPsp(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IWebDriver _driver;
        private string _browserName;
        private string _clientId;
        private string _clientEmail;   
        private string _pspInstanceId;
        private string _currentUserApiKey;
        private string _dodName = "AutoSignDodOnPlatformWithPsp";

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            // create user
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            // get psp Instance Id
            _pspInstanceId = _apiFactory
               .ChangeContext<IPspTabApi>(_driver)
               .PostCreateAirsoftSandboxPspRequest(_crmUrl)
               .FirstOrDefault()
               .Instances
               .Id;
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete  DOD by name
                _apiFactory
                    .ChangeContext<IPlatformTabApi>(_driver)
                    .DeleteDod(_crmUrl, _dodName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }
        #endregion

        [Test]
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4149")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifySignDodOnPlatformWithPspTest()
        {
            var depositAmount = 10;
            var documentParams = new List<string> { "CLIENT_ID" };

            var emailsParams = new Dictionary<string, string> {{ "name", _dodName },
                { "language", "en" }, { "sendBy", "trading-platform"}, { "depositType", "psp" } };

            var tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;

            // create document body
            var documentBody = _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .ComposeEmailBody(documentParams);

            // create document
            _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .CreateDodPipe(_crmUrl, emailsParams, documentBody);

            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, _clientId, depositAmount, 
                transactionType: "psp", actualMethod: "psp",
                nameForMethod: DataRep.AirsoftSandboxPspName,
                pspInstanceId: _pspInstanceId, apiKey: _currentUserApiKey);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, tradingPlatformUrl)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IHistoryPageUi>()
                .ClickOnSignDod()
                .SetSignature()
                .ClickOnSaveSignatureButton()
                .VerifyApprovedSignaturAlert(DataRep.ApprovedSignatureMessage)
                .VerifySignatur("Signed");
        }
    }
}