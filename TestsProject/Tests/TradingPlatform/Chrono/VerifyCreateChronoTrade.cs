// Ignore Spelling: Chrono

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.Chrono
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyCreateChronoTrade : TestSuitBase
    {
        public VerifyCreateChronoTrade(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Members
        private IApplicationFactory _apiFactory;

        private readonly string _prodtradingPlatformUrl
            = Config.appSettings.tradingPlatformUrl;

        private IWebDriver _driver;
        private readonly string _browserName;
        #endregion Members        

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);

            #region PreCondition    
            _apiFactory = new ApplicationFactory();
            var depositAmount = 10000;
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
                .PostDepositRequest(prodCrmUrl, clientId, depositAmount, apiKey: prodApiKey);

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
        public void VerifyCreateChronoTradeTest()
        {
            // not include the rate
            var expectedAssetName = DataRep.AssetNameShort;
            var expectedBoost = "1:2";
            var expectedAction = "STOP";
            var expectedTime = "00:00:30";
            var expectedAssetInOpenOpenTrades = $"{expectedAssetName} BUY";

            var actualTradeConfirmationBodyPopupDetails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IChronoPageUi>()
                .BuyChronoPipe(expectedAssetName)
                .GetTradeConfirmationBodyPopup();

            var actualActivatedMessage = _apiFactory
                .ChangeContext<IChronoPageUi>(_driver)
                .ClickOnConfirmTradeButton()
                .VerifyChronoOrderActivatedMessage(DataRep.ChronoOrderActivatedMessage);

            var actualOpenChronoDetails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IChronoPageUi>(DataRep.ChronoOpenTradesMenuItem)
                .ChangeContext<ISearchResultsUi>(_driver)
                .GetSearchResultDetails<SearchResultChronoTrade>().First();

            var actualCloseChronoDetails = _apiFactory
                .ChangeContext<IChronoPageUi>(_driver)
                .WaitForChronoTimerToFinish(expectedTime)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IChronoPageUi>(DataRep.ChronoCloseTradeMenuItem)
                .ChangeContext<ISearchResultsUi>(_driver)
                .GetSearchResultDetails<SearchResultChronoTrade>()
                .First();

            Assert.Multiple(() =>
            {
                Assert.True(actualTradeConfirmationBodyPopupDetails.Contains(expectedAssetName) &&
                    actualTradeConfirmationBodyPopupDetails.Contains(expectedBoost) &&
                    actualTradeConfirmationBodyPopupDetails.Contains(expectedTime),
                    $" actual Trade Confirmation Body Popup: {actualTradeConfirmationBodyPopupDetails}" +
                    $" expected Asset Name:{expectedAssetName}, expected Boost: {expectedBoost}, " +
                    $" expected expectedTime: {expectedTime}");

                Assert.True(actualOpenChronoDetails.asset.Contains(expectedAssetInOpenOpenTrades) &&
                    actualOpenChronoDetails.action.Contains(expectedAction),
                    $" expected Open Chrono asset && action: {expectedAssetInOpenOpenTrades}, {expectedAction}" +
                    $" actual Open Chrono asset && action: {actualOpenChronoDetails.asset} , {actualOpenChronoDetails.action}");

                Assert.True(actualCloseChronoDetails.leverage.Contains(expectedBoost),
                    $" expected Open Chrono asset && boost: {expectedAssetInOpenOpenTrades}, {expectedBoost}" +
                    $" actual Open Chrono asset && boost: {actualOpenChronoDetails.asset} ," +
                    $" {actualCloseChronoDetails.leverage}");
            });
        }
    }
}
