using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.MyProfilePanel
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyWinPercentageOnMyProfilePage : TestSuitBase
    {
        #region Test Preparation
        public VerifyWinPercentageOnMyProfilePage(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;   
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);

            #region PreCondition
            var crmUrl = Config.appSettings.CrmUrl;
            var depositAmount = 10000;
            var tradeAmount = 1;
            var pnlOfTheFirstTrade = -2;
            var pnlOfTheSecondTrade = 2;
            var dbContext = new QaAutomation01Context();
            _driver = GetDriver();

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(crmUrl, clientName);

            // get login data
            var loginData = _apiFactory
               .ChangeContext<ILoginApi>(_driver)
               .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
               .GeneralResponse;

            // deposit 1000
            #region deposit 1000
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(crmUrl, clientId, depositAmount);
            #endregion

            // create two trades 
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, 2, loginData)
                .Select(p => p.TradeId)
                .ToList();

            //close all the trades
            _apiFactory
                .ChangeContext<IOpenTradesPageApi>(_driver)
                .PatchCloseTradeRequest(_tradingPlatformUrl, tradeDetails);

            // update the pnl of the first trade
            _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .UpdateTradePnl(tradeDetails.First(), pnlOfTheFirstTrade);

            // update the pnl of the second trade
            _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .UpdateTradePnl(tradeDetails.Last(), pnlOfTheSecondTrade);
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

        // create client
        // get login data
        // create deposit
        // create two trades 
        // close the two trades
        // update  the pnl of the first trade to 2
        // update  the pnl of the first trade to -2
        // verify win precentage formula: total win trades / total trades : 1 / 2
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyWinPercentageOnMyProfilePageTest()
        {
            var expectedWinPrecentageValue = "50.00";

            var actualWinPrecentageValue = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradePageUi>(DataRep.MyProfileMenuItem)
                .GetWinPercentageValue();          

            Assert.Multiple(() =>
            {
                Assert.True(expectedWinPrecentageValue == actualWinPrecentageValue,
                    $" expected win Percentage Value: {expectedWinPrecentageValue}" +
                    $" actual win Percentage Value: {actualWinPrecentageValue}");
            });
        }
    }
}