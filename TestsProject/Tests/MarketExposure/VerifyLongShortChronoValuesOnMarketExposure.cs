// Ignore Spelling: Chrono

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
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

namespace TestsProject.Tests.MarketExposure
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyLongShortChronoValuesOnMarketExposure : TestSuitBase
    {
        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _timeLimitValue = "15m";
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private int _depositAmount = 10000;
        private decimal _expectedLongPositionAmount;
        private decimal _expectedShortPositionAmount;
        private IWebDriver _driver;
        private string _browserName;
        #endregion

        public VerifyLongShortChronoValuesOnMarketExposure(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]

        #region PreCondition
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();

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

            // deposit 10000
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, clientId, _depositAmount);

            #region buy chrono asset
            // buy chrono asset
            var ChronoAssetDetails = _apiFactory
                .ChangeContext<IChronoTradePageApi>(_driver)
                .PostBuyChronoAssetApi(_tradingPlatformUrl,
                loginCookies, tradeTimeEnd: _timeLimitValue)
                .GeneralResponse;
            #endregion

            var  currentRate = ChronoAssetDetails.TradeRate;

            // convert long amount from USD to EUR
            _expectedLongPositionAmount = _apiFactory
                .ChangeContext<IGeneral>(_driver)
                .PostCurrencyConversionRequest(_crmUrl, currentRate, DataRep.DefaultUSDCurrencyName, "EUR");

            #region sell chrono asset
            ChronoAssetDetails = _apiFactory
                .ChangeContext<IChronoTradePageApi>(_driver)
                .PostSellChronoAssetApi(_tradingPlatformUrl, loginCookies, tradeTimeEnd: _timeLimitValue);
            #endregion                     

            currentRate = ChronoAssetDetails.TradeRate;

            // convert short amount from USD to EUR
            _expectedShortPositionAmount = _apiFactory
                .ChangeContext<IGeneral>(_driver)
                .PostCurrencyConversionRequest(_crmUrl, currentRate, DataRep.DefaultUSDCurrencyName, "EUR");
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

        // open buy chrono
        // open sell chrono
        // verify front long satistic box 
        // verify front short satistic box 
        // verify front total satistic box 
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyLongShortChronoValuesOnMarketExposureTest()
        {
            var numOfTrades = 1;
            var totalNumOfTrades = 2;

            var expectedTotalExposureAmount = _expectedLongPositionAmount - _expectedShortPositionAmount;

            var actualLongPositionData = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IMarketExposurePageUi>()
                .ChangeContext<IMarketExposurePageUi>(_driver)
                .GetFrontCardLongPositionData();

            var actualShortPositionData = _apiFactory
                .ChangeContext<IMarketExposurePageUi>(_driver)
                .GetFrontCardShortPositionData();

            var actualTotalExposureData = _apiFactory
                .ChangeContext<IMarketExposurePageUi>(_driver)
                .GetFrontCardTotalExposureData();

            Assert.Multiple(() =>
            {
                Assert.True((actualLongPositionData.Values.First() - _expectedLongPositionAmount)
                    .MathAbsGeneric() <= 1,
                    $" expected Long Position amount: {_expectedLongPositionAmount}" +
                    $" actual Long Position amount: {actualLongPositionData.Values.First()}");

                Assert.True(actualLongPositionData.Values.Last() == numOfTrades,
                    $" expected Long Position amount: {numOfTrades}" +
                    $" actual Long Position amount: {actualLongPositionData.Values.Last()}");

                Assert.True((actualShortPositionData.Values.First() - _expectedShortPositionAmount)
                    .MathAbsGeneric() <= 1,
                    $" expected Short Position amount: {_expectedShortPositionAmount}" +
                    $" actual Short Position amount: {actualShortPositionData.Values.First()}");

                Assert.True(actualShortPositionData.Values.Last() == numOfTrades,
                    $" expected Short Position amount: {numOfTrades}" +
                    $" actual Short Position amount: {actualShortPositionData.Values.Last()}");

                Assert.True((actualTotalExposureData.Values.First() - expectedTotalExposureAmount)
                    .MathAbsGeneric() <= 1,
                    $" expected Total Position amount: {expectedTotalExposureAmount}" +
                    $" actual Total Position amount: {actualTotalExposureData.Values.First()}");

                Assert.True(actualTotalExposureData.Values.Last() == totalNumOfTrades,
                    $" expected Total amount: {totalNumOfTrades}" +
                    $" actual Total amount: {actualTotalExposureData.Values.Last()}");
            });
        }
    }
}
