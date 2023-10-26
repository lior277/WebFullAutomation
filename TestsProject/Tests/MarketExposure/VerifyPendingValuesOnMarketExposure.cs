using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;

using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using ConsoleApp;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.MarketExposure
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyPendingValuesOnMarketExposure : TestSuitBase
    {
        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private int _depositAmount = 10000;
        private int _buyAmount = 2;
        private int _sellAmount = 3;
        private decimal _expectedPendingBuyPositionAmount;
        private decimal _expectedPendingSellPositionAmount;
        private IWebDriver _driver;
        private string _browserName;
        #endregion

        public VerifyPendingValuesOnMarketExposure(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]
        #region PreCondition
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();
            var assetName = DataRep.AssetName;

            // create user
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, currency: "EUR");

            var clientsIds = new List<string> { clientId };

            // login
            _apiFactory
            .ChangeContext<ILoginPageUi>(_driver)
            .LoginPipe(userName);

            // connect One User To One Client
            _apiFactory
               .ChangeContext<IClientsApi>(_driver)
               .PatchMassAssignSaleAgentsRequest(_crmUrl,
               userId, clientsIds);

            // get login cookies
            var loginCookies = _apiFactory
                .ChangeContext<ILoginApi>(_driver)
                .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            // deposit 1000
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, clientId, _depositAmount);

            #region buy asset
            // create trade 
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostBuyAssetRequest(_tradingPlatformUrl, _buyAmount,
                loginCookies, assetSymble: assetName)
                .GeneralResponse;
            #endregion

            var currentRate = tradeDetails.TradeRate;
            var rateForPending = (double)(currentRate + 100); // to open a pending trade  

            //  open buy pending trade
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostPendingBuyOrderRequest(_tradingPlatformUrl,
                _buyAmount, loginCookies, rateForPending);
           
            currentRate = tradeDetails.TradeRate;

            // pending buy amount in USD
            var pendingBuyAmountInUsd = _buyAmount * currentRate;

            // convert pending buy amount to EUR
            _expectedPendingBuyPositionAmount = _apiFactory
                .ChangeContext<IGeneral>(_driver)
                .PostCurrencyConversionRequest(_crmUrl, pendingBuyAmountInUsd, DataRep.DefaultUSDCurrencyName, "EUR");

            #region sell asset
            _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostSellAssetRequest(_tradingPlatformUrl, _sellAmount, loginCookies);
            #endregion

            currentRate = tradeDetails.TradeRate;
            rateForPending = (double)(currentRate + 100); // to open a pending trade  

            // pending sell amount in USD
            var sellPendingTradeIDetails = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostPendingSellOrderRequest(_tradingPlatformUrl, _sellAmount, loginCookies, rateForPending);

            currentRate = double.Parse(JObject.Parse(sellPendingTradeIDetails)["rate"].ToString());

            // pending sell amount in USD
            var pendingSellAmountInUsd = _sellAmount * currentRate;

            // convert pending sell amount to EUR
            _expectedPendingSellPositionAmount = _apiFactory
                .ChangeContext<IGeneral>(_driver)
                .PostCurrencyConversionRequest(_crmUrl, pendingSellAmountInUsd, DataRep.DefaultUSDCurrencyName, "EUR");
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

        // buy pending asset
        // sell pending asset
        // verify front long satistic box 
        // verify front short satistic box 
        // verify front total satistic box 
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyPendingValuesOnMarketExposureTest()
        {
            var totalNumOfTrades = 2;         
            var expectedPendingExposureAmount = _expectedPendingBuyPositionAmount - _expectedPendingSellPositionAmount;

            var actualPendingData = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IMarketExposurePageUi>()
                .ChangeContext<IMarketExposurePageUi>(_driver)
                .GetFrontCardPendingOrdersData();

            Assert.Multiple(() =>
            {
                Assert.True((actualPendingData["buyAmount"] - _expectedPendingBuyPositionAmount)
                    .MathAbsGeneric() <= 1,
                    $" expected Long Position amount: {_expectedPendingBuyPositionAmount}" +
                    $" actual Long Position amount: {actualPendingData["buyAmount"]}");

                Assert.True((actualPendingData["sellAmount"] - _expectedPendingSellPositionAmount)
                    .MathAbsGeneric() <= 1,
                    $" expected Short Position amount: {_expectedPendingSellPositionAmount}" +
                    $" actual Short Position amount: {actualPendingData["sellAmount"] }");

                Assert.True((actualPendingData["exposureAmount"] - expectedPendingExposureAmount)
                    .MathAbsGeneric() <= 1,
                    $" expected exposure Amount: {expectedPendingExposureAmount}" +
                    $" actualexposure Amount: {actualPendingData["exposureAmount"] }");

                Assert.True(actualPendingData.Values.Last() == totalNumOfTrades,
                    $" expected Long Position amount: {totalNumOfTrades}" +
                    $" actual Long Position amount: {actualPendingData.Values.Last()}");
            });
        }      
    }
}
