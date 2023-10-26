// Ignore Spelling: Nft

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using Microsoft.Graph;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.Nft
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyCreateNftTrade : TestSuitBase
    {
        public VerifyCreateNftTrade(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Members
        private IApplicationFactory _apiFactory;

        private readonly string _tradingPlatformUrl
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

            // deposit 10000 
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(prodCrmUrl, clientId, depositAmount, apiKey: prodApiKey);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(clientEmail, _tradingPlatformUrl);
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
        public void VerifyCreateNftTradeTest()
        {
            // not include the rate
            var expectedAssetName = DataRep.AssetNftSymbol.ToLower();
            var date = DateTime.Now.ToString("yyyy-MM-dd");

            var actualNftDetails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<INftPageUi>()
                .BuyNftPipe(expectedAssetName)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradePageUi>(DataRep.TradingOpenTradesMenuItem)
                .WaitForTradeTableToLoad()
                .ChangeContext<ISearchResultsUi>(_driver)
                .GetSearchResultDetails<SearchResultTrade>(tableName: "trade", cellsAndTitlesShouldBeEquals: false)
                .First();

            Assert.Multiple(() =>
            {
                Assert.True(actualNftDetails.opentime?.Contains(date),
                    $" expected date: {date}" +
                    $" actual date: {actualNftDetails.opentimegmt}");

                Assert.True(actualNftDetails.instrument == DataRep.AssetNftSymbol,
                    $" expected instrument: {DataRep.AssetNftSymbol}" +
                    $" actual instrument: {actualNftDetails.instrument}");

                Assert.True(actualNftDetails.type == "buy",
                    $" expected type: 'buy', " +
                    $" actual buy: {actualNftDetails.type}");

                Assert.True(actualNftDetails.amount == "1",
                    $" expected amount: '1', " +
                    $" actual amount: {actualNftDetails.amount}");
            });
        }
    }
}
