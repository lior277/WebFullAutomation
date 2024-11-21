
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Dashboard
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyAgentProfileOnDashboard : TestSuitBase
    {
        public VerifyAgentProfileOnDashboard(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userName;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private int _firstDepositAmount = 10000;
        private int _secondDepositAmount = 11000;
        private int _thirdDepositAmount = 12000;
        private int _tradeClosePnl = 50;
        private string _firstClientName;
        private string _secondClientName;
        private string _thirdClientName;
        private string _thirdClientId;
        private string _browserName;
        private string _userId;
        private IWebDriver _driver;
        #endregion Members

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            #region params
            _driver = GetDriver();
            var firstTradeAmount = 1;
            var secondTradeAmount = 2;
            var assetName = DataRep.AssetName;
            #endregion

            #region create user 

            // create user
            _userName = TextManipulation.RandomString();

            // create user
            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            #endregion

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            #region create 3 client
            // create first client 
            _firstClientName = TextManipulation.RandomString();
            var firstClientEmail = _firstClientName + DataRep.EmailPrefix;

            var firstClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _firstClientName, currency: "EUR");

            // create second client 
            _secondClientName = TextManipulation.RandomString();
            var secondClientEmail = _secondClientName + DataRep.EmailPrefix;

            var secondClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _secondClientName, currency: "EUR");

            // create third client 
            _thirdClientName = TextManipulation.RandomString();

            _thirdClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _thirdClientName, currency: "EUR");

            var clientsIds = new List<string> { firstClientId,
                secondClientId, _thirdClientId };

            var tradeIds = new List<string>();
            #endregion

            #region connect One User To 3 Clients
            // connect One User To 3 Clients
            _apiFactory
               .ChangeContext<IClientsApi>()
               .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId, clientsIds);
            #endregion

            #region deposit 400 for the first client
            // deposit 400 for the first client
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, firstClientId,
                _firstDepositAmount, originalCurrency: "EUR");
            #endregion

            #region deposit 200 for the second client
            // deposit 200 for the second client
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, secondClientId,
                _secondDepositAmount, originalCurrency: "EUR");
            #endregion

            #region deposit 100 for the third client
            // deposit 100 for the third client
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _thirdClientId,
                _thirdDepositAmount, originalCurrency: "EUR");
            #endregion

            // first client login cookies
            var loginCookies = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, firstClientEmail)
                .GeneralResponse;

            #region create first trade update pnl and close
            // create first trade 
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl,
                firstTradeAmount, loginCookies, assetSymble: assetName)
                .GeneralResponse;

            tradeIds.Add(tradeDetails.TradeId);

            // close trade 
            _apiFactory
                .ChangeContext<IOpenTradesPageApi>(_driver)
                .PatchCloseTradeRequest(_crmUrl, tradeIds.First());
            #endregion

            // second client login cookies
            loginCookies = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, secondClientEmail)
                .GeneralResponse;

            #region create second trade for second client update pnl and close
            // create second trade 
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, secondTradeAmount, loginCookies,
                assetSymble: assetName)
                .GeneralResponse;

            tradeIds.Add(tradeDetails.TradeId);

            // close trade 
            _apiFactory
                .ChangeContext<IOpenTradesPageApi>()
                .PatchCloseTradeRequest(_tradingPlatformUrl, tradeIds);

            // Update Pnl
            _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .UpdateTradePnl(tradeIds, _tradeClosePnl);

            _driver.Navigate().Refresh();
            _driver.Navigate().Refresh();
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
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyAgentProfileOnDashboardTest()
        {
            #region expectedData
            var expectedSellerRoundProgressDataFirstTime = new List<string>() { "3 #",
                "33000.00 €", "3 #", "75 %", "0 #", "0 min" };

            var expectedTopTenDepositsData = new List<string>() { $"{_firstClientName}" +
                $" {_firstClientName} {_firstDepositAmount}.00" ,
                $"{_secondClientName} {_secondClientName} {_secondDepositAmount}.00" ,
                $"{_thirdClientName} {_thirdClientName} {_thirdDepositAmount}.00" ,};

            var expectedTopTenPnlData = new List<string>() { $"{_firstClientName}" +
                $" {_firstClientName} {_tradeClosePnl}.00",
                $"{_secondClientName} {_secondClientName} {_tradeClosePnl}.00" };

            var expectedPrevMounthData = new List<string>() { "100 % Prv Month",
                 "100 % Prv Month", "100 % Prv Month", "100 % Prv Month",
                "0 % Prv Month", "100 % Prv Month" };
            #endregion

            #region 3 customers 3 FTD  
            // 3 customers 3 FTD 
            // final conversion is 75%
            // 1 ftd / 3 clients + (2 ftd / 3 clients)/2 + (3 ftd / 3 clients)/2 = 75%
            var actualTopTenDepositsData = _apiFactory
                .ChangeContext<IDashboardPageUi>(_driver)
                .ClickOnSaleTitle(_userName)
                .ChangeContext<IAgentProfileUi>(_driver)
                .GetTopTenDeposits();

            var actualTopTenPnlData = _apiFactory
                .ChangeContext<IAgentProfileUi>(_driver)
                .GetTopTenPnl();

            var actualTopTenDepositAndTopTenPnlCurrencySign = _apiFactory
               .ChangeContext<IAgentProfileUi>(_driver)
               .GetTopStatisticsBoxCurrencySign();

            var actualPrevMounthData = _apiFactory
               .ChangeContext<IAgentProfileUi>(_driver)
               .GetPrevMouthData();

            var isChartExist = _apiFactory
                .ChangeContext<IAgentProfileUi>(_driver)
                .IsChartExist();

            var actualSellerRoundProgressDataFirstTime = _apiFactory
                .ChangeContext<IAgentProfileUi>(_driver)
                .GetSellerRoundProgressData();

            _apiFactory
                .ChangeContext<IAgentProfileUi>(_driver)
                .CloseAgentProfile();
            #endregion

            #region two customers 3 FTD    
            // two customers 3 FTD  conversion should stay the same because the subtracted client had FTD   
            var expectedSellerRoundProgressDataSecondTime =
                new List<string>() { "2 #", "33000.00 €", "3 #", "75 %", "0 #", "0 min" };
            // unassigns third Client from user

            _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .PatchMassAssignSaleAgentsRequest(_crmUrl, null, new List<string> { _thirdClientId });

            var actualSellerRoundProgressDataSecondTime = _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .ChangeContext<IDashboardPageUi>(_driver)
                .ClickOnSaleTitle(_userName)
                .ChangeContext<IAgentProfileUi>(_driver)
                .GetSellerRoundProgressData();
            #endregion

            Assert.Multiple(() =>
            {
                Assert.True(actualTopTenDepositsData.CompareTwoListOfString(expectedTopTenDepositsData).Count() == 0,
                    $" expected Top Ten Deposits Data: {expectedTopTenDepositsData.ListToString()}" +
                    $" actual Top Ten Deposits Data: {actualTopTenDepositsData.ListToString()}");

                Assert.True(actualTopTenPnlData.CompareTwoListOfString(expectedTopTenPnlData).Count() == 0,
                    $" expected Top Ten Pnl Data: {expectedTopTenPnlData.ListToString()}" +
                    $" actual Top Ten Pnl Data: {actualTopTenPnlData.ListToString()}");

                Assert.True(actualTopTenDepositAndTopTenPnlCurrencySign == 5,
                    $" expected Top Ten Pnl Data: 5" +
                    $" actual Top Statistic sBox Currency Sign:" +
                    $" {actualTopTenDepositAndTopTenPnlCurrencySign}");

                Assert.True(isChartExist.Equals(true),
                    $" expected is Chart Exist: true" +
                    $" actual is Chart Exist: {isChartExist}");

                Assert.True(actualPrevMounthData.CompareTwoListOfString(expectedPrevMounthData).Count() == 0,
                    $" expected Prev Mouth Data: {expectedPrevMounthData.ListToString()}" +
                    $" actual Prev Month Data: {actualPrevMounthData.ListToString()}");

                Assert.True(actualSellerRoundProgressDataFirstTime 
                    .CompareTwoListOfString(expectedSellerRoundProgressDataFirstTime).Count() == 0,
                    $" expected Seller Round Progress Data First Time:" +
                    $" {expectedSellerRoundProgressDataFirstTime.ListToString()}" +
                    $" actual Seller Round Progress Data First Time:" +
                    $" {actualSellerRoundProgressDataFirstTime.ListToString()}");

                Assert.True(actualSellerRoundProgressDataSecondTime 
                    .CompareTwoListOfString(expectedSellerRoundProgressDataSecondTime).Count() == 0,
                    $" expected Seller Round Progress Data Second Time: {expectedSellerRoundProgressDataSecondTime.ListToString()}" +
                    $" actual Seller Round Progress Data Second Time: {actualSellerRoundProgressDataSecondTime.ListToString()}");
            });
        }
    }
}
