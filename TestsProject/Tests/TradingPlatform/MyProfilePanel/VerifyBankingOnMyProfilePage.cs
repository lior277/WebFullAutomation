using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
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
using System.Threading;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.MyProfilePanel
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyBankingOnMyProfilePage : TestSuitBase
    {
        #region Test Preparation
        public VerifyBankingOnMyProfilePage(string browser) : base(browser)
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
            var tradeAmount = 2;
            var pnlAmount = 500;
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

            // create trade 
            var tradeDetail = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, loginData)
                .GeneralResponse;

            //close trade
            _apiFactory
                .ChangeContext<IOpenTradesPageApi>(_driver)
                .PatchCloseTradeRequest(crmUrl, tradeDetail.TradeId);

            // wait for trade to close 
            _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .WaitForCfdTradeToClose(crmUrl, tradeDetail.TradeId, loginData);

            // update the pnl of the trade
            _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .UpdateClientPnl(clientId, tradeDetail.TradeId, pnlAmount);
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
        // create deposit 1000
        // create one trade
        // close the trade
        // update the pnl of the trade to 500
        // verify account roi = 50% the formula is total client pnl / total deposit
        // verify av mouth roi = 50% the formula is total client pnl / total deposit / months Passed From Create
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyBankingOnMyProfilePageTest()
        {
            var expectedAccountRoi = "5";
            var expectedAvMounthRoi = "5";

            //Thread.Sleep(10000); // wait for Account Roi and AvMounthRoi to update

            var actualAccountRoi = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradePageUi>(DataRep.MyProfileMenuItem)
                .WaitForMyProfileToLoad()
                .GetAccountRoi(expectedAccountRoi);

            var actualAvMounthRoi = _apiFactory
               .ChangeContext<ITradePageUi>(_driver)
               .AvMounthRoi(expectedAvMounthRoi);

            Assert.Multiple(() =>
            {
                Assert.True(expectedAccountRoi == actualAccountRoi,
                    $" expected Account Roi : {expectedAccountRoi}" +
                    $" actual Account Roi: {actualAccountRoi}");

                Assert.True(expectedAvMounthRoi == actualAvMounthRoi,
                   $" expected Av Mounth Roi: {expectedAvMounthRoi}" +
                   $" actual Av Mounth Roi: {actualAvMounthRoi}");
            });
        }
    }
}