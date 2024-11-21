using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyTradeCantBeOpenWithZeroBalance : TestSuitBase
    {
        #region Test Preparation
        public VerifyTradeCantBeOpenWithZeroBalance(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;      
        private IWebDriver _driver;
        private string _browserName;
        private string _clientEmail;
        private GetLoginResponse _loginData;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            var crmUrl = Config.appSettings.CrmUrl;       
            _driver = GetDriver();

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(crmUrl, clientName,
                currency: DataRep.DefaultUSDCurrencyName);

            #region login data 
            // login data
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;
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
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4821," +
            " based on jira https://airsoftltd.atlassian.net/browse/AIRV2-5358")]

        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyTradeCantBeOpenWithZeroBalanceTest()
        {
            var assetName = "ETH";
            var expectedAmountErrorMessage = "X Insufficient funds";

            var actualAmountErrorMessage = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl)
                .ChangeContext<ITradePageUi>(_driver)
                .SearchAssetPipe(assetName)
                .ClickOnBuyButton()
                .ClickOnCloseDealWithErrorButton()
                .GetAmountErrorMessage();

            var actualOpenTrades = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetTradesByStatusRequest(_tradingPlatformUrl, _loginData, "open")
                .GeneralResponse
                .Count;

            Assert.Multiple(() =>
            {
                Assert.True(actualAmountErrorMessage == expectedAmountErrorMessage,
                    $" expected Amount Error Message : {expectedAmountErrorMessage}" +
                    $" actual Amount Error Message: {actualAmountErrorMessage}");

                Assert.True(actualOpenTrades == 0,
                   $" expected num of Open Trades : {0}" +
                   $" actual num of Open Trades: {actualOpenTrades}");
            });
        }
    }
}