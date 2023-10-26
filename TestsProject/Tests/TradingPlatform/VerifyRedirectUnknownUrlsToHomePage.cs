using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyRedirectUnknownUrlsToHomePage : TestSuitBase
    {
        #region Test Preparation
        public VerifyRedirectUnknownUrlsToHomePage(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);
            #endregion
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
        [Description("based on jira 5074")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyRedirectUnknownUrlsToHomePageTest()
        {
            var expectedUrl = $"{_tradingPlatformUrl}/trade";

            _apiFactory
               .ChangeContext<ILoginPageUi>(_driver)
               .LoginPipe(_clientEmail, _tradingPlatformUrl)
               .WaitForUrlToChange(expectedUrl);

            var tempTradingPlatformUrl = $"{_driver.Url.Split("com").First()}com";
            _driver.Navigate().GoToUrl(tempTradingPlatformUrl);
            var actualUrl = _driver.Url;

            Assert.Multiple(() =>
            {
                Assert.True(actualUrl == expectedUrl,
                    $"expected trading Platform Url: {_tradingPlatformUrl}" +
                    $" actual trading Platform Url: {actualUrl}");
            });
        }
    }
}