using System;
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

namespace TestsProject.Tests.TradingPlatform
{
    //[TestFixture(DataRep.Firefox)]
    [TestFixture(DataRep.Chrome)]
    public class VerifyFavoritesAssets : TestSuitBase
    {
        #region Test Preparation
        public VerifyFavoritesAssets(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);
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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyFavoritesAssetsTest()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var assetName = "ETH";

            var actualNumOfFavoritesAssets = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl)
                .ChangeContext<ITradePageUi>(_driver)
                .SearchAssetPipe(assetName)
                .ClickOnAssetFavoriteBtn(assetName)
                .ClickOnFavoritesBtnOnTopMenu()
                .VerifyFavoritAssetOnAssetsList(assetName);

            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradePageUi>(DataRep.FavoritesMenuItem)
                .VerifyAssetOnFavoritesMenu(assetName);
               
            Assert.Multiple(() =>
            {
                Assert.True(actualNumOfFavoritesAssets == 1,
                    $" expected Num Of Favorites Assets: 1" +
                    $" expected Num Of Favorites Assets: {actualNumOfFavoritesAssets}");
            });
        }
    }
}