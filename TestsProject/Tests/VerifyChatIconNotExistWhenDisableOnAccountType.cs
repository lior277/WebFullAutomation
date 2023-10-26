using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;

using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Internals.Enums.EnumFactory;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Internals.Factory;

namespace TestsProject.Tests
{
    //[TestFixture(DataRep.Firefox)]
    [TestFixture(DataRep.Chrome)]
    public class VerifyChatIconNotExistWhenDisableOnAccountType : TestSuitBase
    {
        #region Test Preparation
        public VerifyChatIconNotExistWhenDisableOnAccountType(string browser) : base(browser) 
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientName;
        private string _userName;
        private string _browserName;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            // create user
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            _clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                apiKey: currentUserApiKey);

            var clientsIds = new List<string> { clientId };

            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .PatchMassAssignSaleAgentsRequest(_crmUrl, userId,
                clientsIds);

            // enable chat for user in settings
            _apiFactory
                .ChangeContext<IChatTabApi>(_driver)
                .PatchEnableChatForUserRequest(_crmUrl, userId);

            // create Account type with 
            var automationAccountType = _apiFactory
                .ChangeContext<ISalesTabApi>(_driver)
                .CreateAutomationAccountTypePipe(_crmUrl);

            automationAccountType.ChatEnabled = false;

            // update  Account type with Chat Enabled  = false
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
            informationTabResponse.sales_agent = userId;

            // update client card with the new account type
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, informationTabResponse);
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
        public void VerifyChatIconNotExistWhenDisableOnAccountTypeTest()
        {
            // Verify Chat Button Disable
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .SearchClientByEmail(_clientName)
                .ClickOnClientFullName()
                .VerifyChatButtonDisable()
                .ClickOnFastLoginBtn();

            // Verify Chat Btn Not Exist
            _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .VerifyChatBtnNotExist();        
        }
    }
}