// Ignore Spelling: Chrono

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

namespace TestsProject.Tests.TradingPlatform.Chrono
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyNoChronoPlatformWhenDisableOnAccountType : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _accountTypeName;
        private IWebDriver _driver;
        private string _currentUserApiKey;
        private string _browserName;

        public VerifyNoChronoPlatformWhenDisableOnAccountType(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]
        public void SetUp()
        {
            _driver = GetDriver();
            _accountTypeName = TextManipulation.RandomString();
            BeforeTest(_browserName);

            // create user
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // create Account type with chrono disable
            var automationAccountType = _apiFactory
                .ChangeContext<ISalesTabApi>(_driver)
                .CreateAutomationAccountTypePipe(_crmUrl);

            automationAccountType.ChronoTrading = false;         

            // update automation account type with chrono false
            _apiFactory
                .ChangeContext<ISalesTabApi>(_driver)
                .PutAccountTypeRequest(_crmUrl, automationAccountType);

            var informationTabResponse = _apiFactory
               .ChangeContext<IInformationTabApi>()
               .GetInformationTabRequest(_crmUrl, clientId)
               .GeneralResponse
               .informationTab;

            informationTabResponse.account_type_id = automationAccountType.AccountTypeId;
            informationTabResponse.saving_account_id = "null";
            informationTabResponse.sales_agent = "null";

            // update client card with the new acoount type
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl,
                informationTabResponse, _currentUserApiKey, false);

            // login to platform
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(clientEmail, _tradingPlatformUrl);
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
        public void VerifyNoChronoPlatformWhenDisableOnAccountTypeTest()
        {
            // get global events
            var chronoMenuItemExist = _apiFactory
                .ChangeContext<IChronoPageUi>(_driver)
                .CheckIfChronoMenuItemExist();

            Assert.Multiple(() =>
            {
                Assert.True(chronoMenuItemExist.Equals(false),
                    $" actual chrono Menu Item Exist : {chronoMenuItemExist}" +
                    $" expected chrono Menu Item Exist: False");
            });
        }
    }
}