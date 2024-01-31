using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyBannerOnTradingPlatform : TestSuitBase
    {
        #region Test Preparation
        public VerifyBannerOnTradingPlatform(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userName;
        private string _clientId;
        private string _clientEmail;
        private string _userApiKey;
        private string _bannerName = "bannerForTP";
        private IWebDriver _driver;
        private string _browserName;
        #endregion

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var  tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
            _driver = GetDriver();

            #region Create admin user only 
            // create admin user only 
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName);

            // get ApiKey
            _userApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);
            #endregion

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _userApiKey);

            //var clientsIds = new List<string> { _clientId };

            #region Change banner
            // get banner
            var bannerId = _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .GetBannersRequest(_crmUrl)
                .Where(p => p.Name == _bannerName)
                .FirstOrDefault()?
                .Id;

            if (bannerId == null)
            {
                // create banner
                _apiFactory
                    .ChangeContext<IPlatformTabApi>(_driver)
                    .PostCreateBannerRequest(_crmUrl, _bannerName, _bannerName);
            }

            // get banner
            bannerId = _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .GetBannersRequest(_crmUrl)
                .Where(p => p.Name == _bannerName)
                .FirstOrDefault()?
                .Id;

            // change banner
            _apiFactory
                .ChangeContext<IBannerTabApi>(_driver)
                .PutBannerRequest(_crmUrl, _clientId, bannerId, _userApiKey);
            #endregion
         
            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, tradingPlatformUrl);
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
        public void VerifyBannerOnTradingPlatformTest()
        {
            _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .VerifyBanner(_bannerName);         
        }
    }
}