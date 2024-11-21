// Ignore Spelling: Api

using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Affiliate
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyAgentCanCreateLoginLinkApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _clientName;
        private string _affiliateApiKey;
        private IWebDriver _driver;

        public VerifyAgentCanCreateLoginLinkApi(string browser) : base(browser) { }

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            _driver = GetDriver();

            // create first campaign and Affiliate
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            var campaignId = campaignData.Values.First();
            var affiliateId = campaignData.Values.Last();

            // get first Affiliate ApiKey
            _affiliateApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, affiliateId);

            // create client 
            _clientName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>(_driver)
                .CreateClientWithCampaign(_crmUrl, _clientName, campaignId,
                apiKey: _affiliateApiKey);
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
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }

        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyAgentCanCreateLoginLinkApiTest()
        {
            var expectedClientName = _clientName
                .UpperCaseFirstLetter();

            var actualLoginLink = _apiFactory
                .ChangeContext<ICreateClientApi>(_driver)
                .PostCreateLoginLinkRequest(_crmUrl, _clientId, _affiliateApiKey)
                .LoginLink;

            _driver.Navigate().GoToUrl(actualLoginLink);
            _driver.Manage().Window.Maximize();

            var actualClientName = _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .GetClientFirstName();

            Assert.True(actualClientName.Equals(expectedClientName),
               $" actual Client Name : {actualClientName}" +
               $" expected Client Name : {expectedClientName}");
        }
    }
}