// Ignore Spelling: Chrono

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.Chrono
{
    //[TestFixture(DataRep.Firefox)]
    [TestFixture(DataRep.Chrome)]
    public class ChronoVerifyUserCanCloseTrade : TestSuitBase
    {
        #region Members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _prodtradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private IWebDriver _driver;
        private string _browserName;
        #endregion Members

        public ChronoVerifyUserCanCloseTrade(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);

            #region PreCondition    
            var _depositAmount = 10000;
            var prodCrmUrl = Config.appSettings.CrmUrl;
            var prodApiKey = Config.appSettings.ApiKey;
            _driver = GetDriver();

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(prodCrmUrl, clientName,
                apiKey: prodApiKey);

            // deposit 400 
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(prodCrmUrl, clientId, _depositAmount, apiKey: prodApiKey);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(clientEmail, _prodtradingPlatformUrl);
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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void ChronoVerifyUserCanCloseTradeTest()
        {
            var expectedAssetName = DataRep.AssetNameShort;
            var expectedStopTradePnlColorNameChromeFirafox = "f9a86501,fff9a865";

            var actualStopTradePnlColorName = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IChronoPageUi>()
                .ChangeContext<IChronoPageUi>(_driver)
                .BuyChronoPipe(expectedAssetName, timeLimit: "30m_period")
                .ClickOnConfirmTradeButton()
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IChronoPageUi>(DataRep.ChronoOpenTradesMenuItem)
                .ClickOnStopChronoTradeButton()
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IChronoPageUi>(DataRep.ChronoCloseTradeMenuItem)
                .GetEarlyStopPnlRgbColor()
                .ConvertRgbToColor()
                .Name;

            Assert.True(expectedStopTradePnlColorNameChromeFirafox.Contains(actualStopTradePnlColorName),
                $" actual Stop Trade Pnl Color: {actualStopTradePnlColorName}" +
                $" expected Stop Trade Pnl Color: {expectedStopTradePnlColorNameChromeFirafox}");
        }
    }
}
