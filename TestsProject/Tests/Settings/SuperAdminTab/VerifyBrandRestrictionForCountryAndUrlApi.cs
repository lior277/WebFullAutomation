// Ignore Spelling: Admin Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Settings.SuperAdminTab
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyBrandRestrictionForCountryAndUrlApi : TestSuitBase
    {
        public VerifyBrandRestrictionForCountryAndUrlApi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _userEmail;      
        private string _clientEmail;
        private string _erpFingerPrint;
        private string _expectedRedirectUrl = "https://earbudsz.com/edifier-neo-buds-pro-review";
        private string _browserName;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _driver = GetDriver();
            var countryName = "united kingdom";
            var countryId = 220;

            _apiFactory
                .ChangeContext<ISuperAdminTubApi>(_driver)
                .PutBrandRestrictionRequest(_crmUrl, _expectedRedirectUrl);

            #region create user
            // create user
            var userName = TextManipulation.RandomString();
            _userEmail = userName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName, country: countryName);
            #endregion

            // create client
            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, country: countryName);
        
            _erpFingerPrint = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .GetAllowedFingerPrintFromMongo(_userEmail);

            _apiFactory
                .ChangeContext<ISuperAdminTubApi>(_driver)
                .PutBrandRestrictionRequest(_crmUrl, _expectedRedirectUrl, 
                countryId: countryId, countryName: countryName);
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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyBrandRestrictionForCountryAndUrlApiTest()
        {
            var expectedRedirectUrl = _expectedRedirectUrl;
            var expectedMessage = "Your country is blocked. Contact the support team for more information";

            var blockResponseErp = _apiFactory
                .ChangeContext<ILoginApi>(_driver)
                .PostLoginCrmRequest(_crmUrl, _userEmail, _erpFingerPrint, checkStatusCode: false)
                .GeneralResponse;          

            var blockResponseTrade = _apiFactory
                .ChangeContext<ILoginApi>(_driver)
                .PostLoginToTradingPlatform(_tradingPlatformUrl,
                _clientEmail, checkStatusCode: false)
                .Message;

            Assert.Multiple(() =>
            {
                Assert.True(blockResponseTrade.Contains(expectedMessage),
                    $" actual Message Erp : {blockResponseTrade}" +
                    $" expected Message Erp: {expectedMessage}");

                Assert.True(blockResponseTrade.Contains(expectedRedirectUrl),
                    $" actual Redirect Url Erp  : {blockResponseTrade}" +
                    $" expected Redirect Url Erp : {expectedRedirectUrl}");

                Assert.True(blockResponseTrade.Contains(expectedMessage),
                    $" actual Message Trade : {blockResponseTrade}" +
                    $" expected Message Trade: {expectedMessage}");

                Assert.True(blockResponseTrade.Contains(expectedRedirectUrl),
                    $" actual Redirect Url Trade  : {blockResponseTrade}" +
                    $" expected Redirect Url Trade : {expectedRedirectUrl}");
            });
        }
    }
}