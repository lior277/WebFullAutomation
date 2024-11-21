// Ignore Spelling: Chrono

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.Chrono
{
    [TestFixture(DataRep.Chrome)]
    public class ChronoVerifyBlockTradeForZeroBalance : TestSuitBase
    {
        #region Members
        private readonly IApplicationFactory _apiFactory = new ApplicationFactory();
        private readonly string _prodtradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private IWebDriver _driver;
        private readonly string _browserName;
        #endregion Members

        public ChronoVerifyBlockTradeForZeroBalance(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);

            #region PreCondition    
            var prodCrmUrl = Config.appSettings.CrmUrl;
            var prodApiKey = Config.appSettings.ApiKey;
            _driver = GetDriver();

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(prodCrmUrl, clientName,
                apiKey: prodApiKey);

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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void ChronoVerifyBlockTradeForZeroBalanceTest()
        {
            var expectedBlockTradeMessage = "Insufficient available funds";
            var expectedNumOfDisabledBoosts = 6;
            var assetName = DataRep.AssetNameShort;

            var actualBlockTradeMessage = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IChronoPageUi>()
                .ChangeContext<ITradePageUi>(_driver)
                .SearchAssetPipe(assetName)
                .ChangeContext<IChronoPageUi>(_driver)
                .GetBlockTradeMessage();

            var actualNumOfDisabledBoosts = _apiFactory
                .ChangeContext<IChronoPageUi>(_driver)
                .GetDisabledBoosts()
                .Count;

            Assert.Multiple(() =>
            {
                Assert.True(actualBlockTradeMessage == expectedBlockTradeMessage,
                    $" actual Block Trade Message: {actualBlockTradeMessage}" +
                    $" expected Block Trade Message: {expectedBlockTradeMessage}");

                Assert.True(actualNumOfDisabledBoosts.Equals(expectedNumOfDisabledBoosts),
                    $" actual Num Of Disabled Boosts: {actualNumOfDisabledBoosts}" +
                    $" expected Num Of Disabled Boosts: {expectedNumOfDisabledBoosts}");
            });
        }
    }
}
