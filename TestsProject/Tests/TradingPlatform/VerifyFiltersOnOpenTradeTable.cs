using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyFiltersOnOpenTradeTable : TestSuitBase
    {
        #region Test Preparation
        public VerifyFiltersOnOpenTradeTable(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;
        private string _clientId;
        private string _userName;
        private string _expectedBuyCfdTradeId;
        private string _expectedSellCfdTradeId;
        private string _expectedBuyChronoTradeId;
        private string _expectedSellChronoTradeId;
        private string _expectedBuyNftTradeId;
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var crmUrl = Config.appSettings.CrmUrl;
            var depositAmount = 10000;
            var tradeAmount = 2;
            var tradeTimeEnd = "30m"; // to keep the trade open
            _driver = GetDriver();

            // create user
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName);
            #endregion

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, currency: DataRep.DefaultUSDCurrencyName);

            var clientsIds = new List<string> { _clientId };

            // connect One User To One Client 
            _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .PatchMassAssignSaleAgentsRequest(_crmUrl,
                userId, clientsIds);

            // get login data
            var loginData = _apiFactory
               .ChangeContext<ILoginApi>(_driver)
               .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
               .GeneralResponse;

            // deposit 10000
            #region deposit 10000
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(crmUrl, _clientId, depositAmount);
            #endregion

            #region Chrono
            // sell chrono trade 
            var tradeDetails = _apiFactory
                .ChangeContext<IChronoTradePageApi>(_driver)
                .PostSellChronoAssetApi(_tradingPlatformUrl, loginData, tradeTimeEnd: tradeTimeEnd);

            _expectedSellChronoTradeId = tradeDetails.TradeId;

            // buy chrono trade 
            tradeDetails = _apiFactory
                .ChangeContext<IChronoTradePageApi>(_driver)
                .PostBuyChronoAssetApi(_tradingPlatformUrl,
                loginData, tradeTimeEnd: tradeTimeEnd)
                .GeneralResponse;

            _expectedBuyChronoTradeId = tradeDetails.TradeId;
            #endregion

            #region Cfd
            // create open trades 
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, loginData)
                .GeneralResponse;

            _expectedBuyCfdTradeId = tradeDetails.TradeId;

            // create sell trade
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostSellAssetRequest(_tradingPlatformUrl, tradeAmount, loginData)
                .GeneralResponse;

            _expectedSellCfdTradeId = tradeDetails.TradeId;
            #endregion

            #region Nft
            // create nft trade 
            tradeDetails = _apiFactory
                .ChangeContext<INftPageApi>()
                .PostBuyNftRequest(_tradingPlatformUrl, loginData)
                .GeneralResponse;

            _expectedBuyNftTradeId = tradeDetails.TradeId;
            #endregion

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IOpenTradesPageUi>(DataRep.CrmOpenTradesMenuItem)
                .SearchOpenTrades(_clientId);            
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
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyFiltersOnOpenTradeTableTest()
        {
            var date = DateTime.Now.ToString("dd/MM/yyyy");

            // set Asset Name filter
            _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                .ClickToOpenFiltersMenu()
                .MultiSelectDropDownPipe(DataRep.AssetsFilter,
                DataRep.AssetName);

            // set user Name filter
            _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                .MultiSelectDropDownPipe(DataRep.SalesAgentsFilter,
                new string[] { _userName });

            // set Cfd filter
            _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                .SingleSelectDropDownPipe(DataRep.TradeTypeFilter, "Cfd", "NFT");

            // set BUY filter
            var actualTradeIdOfCfdBuy = _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                .SingleSelectDropDownPipe(DataRep.TypeFilter, "BUY")
                .GetSearchResultDetails<SearchResultTrade>()
                .First()
                .id;

            // set sell filter
            var actualTradeIdOfCfdSell = _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                .SingleSelectDropDownPipe(DataRep.TypeFilter, "SELL", "BUY")
                .GetSearchResultDetails<SearchResultTrade>()
                .First()
                .id;

            // set Chrono filter
            _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                .SelectUnselectElementInMultiSelect(DataRep.TradeTypeFilter, "Chrono", "Cfd");

            // set BUY filter
            var actualTradeIdOfChronoBuy = _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                .SingleSelectDropDownPipe(DataRep.TypeFilter, "BUY", "SELL")
                .GetSearchResultDetails<SearchResultTrade>()
                .First()
                .id;

            // set sell filter
            var actualTradeIdOfChronoSell = _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                .SingleSelectDropDownPipe(DataRep.TypeFilter, "SELL", "BUY")
                .GetSearchResultDetails<SearchResultTrade>()
                .First()
                .id;

            // set Asset Name for nft filter
            _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                .SelectUnselectElementInMultiSelect(DataRep.AssetsFilter,
                DataRep.AssetNftLongSymbol, DataRep.AssetName);

            // set nft filter
            _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                .SelectUnselectElementInMultiSelect(DataRep.TradeTypeFilter, "NFT", "Chrono");

            // set BUY filter
            var actualTradeIdOfNftBuy = _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                .SingleSelectDropDownPipe(DataRep.TypeFilter, "BUY", "SELL")
                .GetSearchResultDetails<SearchResultTrade>()
                .First()
                .id;

            Assert.Multiple(() =>
            {
                Assert.True(actualTradeIdOfCfdBuy  == _expectedBuyCfdTradeId,
                    $" expected Trade Id Of Cfd Buy: {_expectedBuyCfdTradeId}" +
                    $" actual Trade Id Of Cfd Buy: {actualTradeIdOfCfdBuy}");

                Assert.True(actualTradeIdOfCfdSell  == _expectedSellCfdTradeId,
                    $" expected Trade Id Of Cfd sell: {_expectedSellCfdTradeId}" +
                    $" actual Trade Id Of Cfd sell: {actualTradeIdOfCfdSell}");

                Assert.True(actualTradeIdOfChronoBuy  == _expectedBuyChronoTradeId,
                    $" expected Trade Id Of chrono Buy: {_expectedBuyChronoTradeId}" +
                    $" actual Trade Id Of chrono Buy: {actualTradeIdOfChronoBuy}");

                Assert.True(actualTradeIdOfChronoSell  == _expectedSellChronoTradeId,
                    $" expected Trade Id Of chrono sell: {_expectedSellChronoTradeId}" +
                    $" actual Trade Id Of chrono sell: {actualTradeIdOfChronoSell}");

                Assert.True(actualTradeIdOfNftBuy == _expectedBuyNftTradeId,
                    $" expected Trade Id Of nft buy: {_expectedBuyNftTradeId}" +
                    $" actual Trade Id Of nft buy: {actualTradeIdOfNftBuy}");
            });
        }
    }
}