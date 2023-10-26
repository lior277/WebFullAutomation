using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Banking.DepositsPage.Filters
{
    [TestFixture(DataRep.Chrome, "766 * 373")]
    public class VerifyResolution : TestSuitBase
    {
        public VerifyResolution(string browser, string resolution) : base(browser, resolution)
        {
            _browserName = browser;
            _resolution = resolution;
        }

        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;
        private string _depositId;
        private string _clientEmail;
        private int _firstDepositAmount = 400;
        private string _userApiKey;
        private string _browserName;
        private IWebDriver _driver;
        private string _resolution;
        #endregion      

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
            _driver = GetDriver();

            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName, role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            _userApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create client
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, apiKey: _userApiKey);

            // create client
            clientName = TextManipulation.RandomString();

            var secondClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, apiKey: _userApiKey);

            // connect first User To first Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { clientId, secondClientId }, apiKey: _userApiKey);

            // first approved deposit 400 EUR 
            _depositId = _apiFactory
                 .ChangeContext<IFinancesTabApi>(_driver)
                 .PostDepositRequest(_crmUrl, clientId,
                 _firstDepositAmount, apiKey: _userApiKey);

            #region get login data
            // get login data
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>(_driver)
                .PostLoginToTradingPlatform(tradingPlatformUrl, _clientEmail)
                .GeneralResponse;
            #endregion

            // create pending deposit
            _apiFactory
                .ChangeContext<ITradeDepositPageApi>(_driver)
                .PostCreatePaymentRequestPipe(tradingPlatformUrl, loginData);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName, _crmUrl);
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

        // create admin user with users only
        // create  client 
        // create deposit for client
        // connect  client to  user
        // create pending deposit in platform  
        // navigate to crm banking deposit page
        // set FTD filter to: "Only FTD"
        // status filter is set on approved by default
        // verify only one approved deposit in search result 

        // based also on jira https://airsoftltd.atlassian.net/browse/AIRV2-4809
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyStatusFilterOnBankingDepositPageTest()
        {
            var chooseFtdValue = "Only FTD";
            var expectedErpAssigned = "none";
            string expectedErpUserIdAssigned = null;

            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IDepositsPageUi>()
                .WaitForDepositTableToLoad();

            var depositDitails = _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                .ClickToOpenFiltersMenu()
                .SingleSelectDropDownPipe(DataRep.FtdFilter, chooseFtdValue)
                .VerifyFirstDepositFlag()
                .GetSearchResultDetails<SearchResultDepositsBanking>()
                .First();

            _apiFactory
                .ChangeContext<IDepositsPageApi>(_driver)
                .PatchAssignDepositToUserRequest(_crmUrl, _depositId, "null");

            var actualErpAssigned = _apiFactory
                .ChangeContext<IDepositsPageApi>(_driver)
                .GetDepositDataFromBankingRequest(_crmUrl, "erp_user_id_assigned", null)
                .GeneralResponse
                .data
                .Where(p => p.email == _clientEmail)
                .FirstOrDefault()
                .erp_assigned;

            var actualErpUserIdAssigned = _apiFactory
                .ChangeContext<IDepositsPageApi>(_driver)
                .GetDepositDataFromBankingRequest(_crmUrl, "erp_user_id_assigned", null)
                .GeneralResponse
                .data
                .Where(p => p.email == _clientEmail)
                .FirstOrDefault()
                .erp_user_id_assigned;

            Assert.Multiple(() =>
            {
                Assert.True(depositDitails.email == _clientEmail,
                    $" expected client Email: {_clientEmail}" +
                    $" actual client Email :  {depositDitails.email}");

                Assert.True(actualErpAssigned == expectedErpAssigned,
                    $" expected Erp Assigned: {expectedErpAssigned}" +
                    $" actual Erp Assigned :  {actualErpAssigned}");

                Assert.True(actualErpUserIdAssigned == expectedErpUserIdAssigned,
                    $" expected Erp User Id Assigned: {expectedErpUserIdAssigned}" +
                    $" actual Erp User Id Assigned :  {actualErpUserIdAssigned}");
            });
        }
    }
}
