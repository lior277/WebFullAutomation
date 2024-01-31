// Ignore Spelling: Chrono

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.Chrono
{
    // this test inclode change on setting so it run runs only on one browser  
    [TestFixture(DataRep.Chrome)]
    public class ChronoVerifyBlockOpenTradeAfterChangeBoostOptionsInSettings : TestSuitBase
    {
        #region Members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _prodtradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _prodApiKey = Config.appSettings.ApiKey;
        private string _prodCrmUrl = Config.appSettings.CrmUrl;
        private IWebDriver _driver;
        private string _browserName;
        #endregion Members

        public ChronoVerifyBlockOpenTradeAfterChangeBoostOptionsInSettings(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition    
            var _depositAmount = 40000;
            _driver = GetDriver();

            // restore default Boost Options values
            _apiFactory
                .ChangeContext<IChronoTabApi>(_driver)
                .PatchBoostOptionsToDefaultRequest(_prodCrmUrl, _prodApiKey);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_prodCrmUrl, clientName,
                apiKey: _prodApiKey);

            // deposit  
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_prodCrmUrl, clientId, _depositAmount, apiKey: _prodApiKey);

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
                // restore default Boost Options values
                _apiFactory
                    .ChangeContext<IChronoTabApi>(_driver)
                    .PatchBoostOptionsToDefaultRequest(_prodCrmUrl, _prodApiKey);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        public void ChronoVerifyBlockOpenTradeAfterChangeBoostOptionsInSettingsTest()
        {
            var assetName = DataRep.AssetNameShort;

            _apiFactory
                 .ChangeContext<IMenus>(_driver)
                 .ClickOnMenuItem<IChronoPageUi>();

            _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .SearchAssetPipe(assetName);

            _apiFactory
                .ChangeContext<IChronoPageUi>(_driver)
                .ClickOnBoostByName("X 300")
                .ChangeContext<IChronoTabApi>(_driver)
                .PatchMinAmountForBoostOptionRequest(_prodCrmUrl, _prodApiKey)
                .ChangeContext<IChronoPageUi>(_driver)
                .ClickBuyButton()
                .ClickOnConfirmTradeButton()
                .VerifyBlockChronoTradeMessage(DataRep.ChronoBlockTradeMessage);
            // after changing Boost Options to 2000000 in setting boost X 300 is trade is disable          
        }
    }
}
