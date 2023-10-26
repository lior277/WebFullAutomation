using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.SignDod
{
    [TestFixture(DataRep.Chrome)]
    public class VerifySignDodOnPlatformWithBankTransfer : TestSuitBase
    {
        #region Test Preparation
        public VerifySignDodOnPlatformWithBankTransfer(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IWebDriver _driver;
        private string _clientId;
        private string _currentUserApiKey;
        private string _dodName = "SignDodOnPlatformBankTransfer";
        private string _clientEmail;
        private string _tradingPlatformUrl;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

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
                .CreateClientRequest(_crmUrl, clientName);

            // get login Data for trading Platform
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>(_driver)
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            var dodLanguage = "es";

            // change Language to spain
            _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PutLanguage(_tradingPlatformUrl,
                loginData, _clientId, dodLanguage);
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete dod if exist
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
        public void VerifySignDodOnPlatformWithBankTransferTest()
        {
            var depositAmount = 10;
            var documentParams = new List<string> { "DEPOSIT_DATE" };

            var dodParams = new Dictionary<string, string> {{ "name", _dodName },
                { "language", "es" }, { "sendBy", "trading-platform"},
                { "depositType", "bank_transfer" } };

            // create document body
            var documentBody = _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .ComposeEmailBody(documentParams);

            // create document
            _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .CreateDodPipe(_crmUrl, dodParams, documentBody);

            // create deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, _clientId,
                depositAmount, apiKey: _currentUserApiKey);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IHistoryPageUi>()
                .ClickOnSignDod()
                .SetSignature()
                .ClickOnSaveSignatureButton()
                .VerifyApprovedSignaturAlert("File uploaded") // in spanish
                .VerifySignatur("Signed");
        }
    }
}