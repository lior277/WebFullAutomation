
using AirSoftAutomationFramework.Internals.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using TestsProject.TestsInternals;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using Microsoft.Graph.Models;

namespace TestsProject.Tests
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyColumnsVisibilityAfterLogOut : TestSuitBase
    {
        #region Test Preparation
        public VerifyColumnsVisibilityAfterLogOut(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private string _clientEmail;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();

            var tableNames = new List<string>() { "clients", "deposits",
                "bonuses", "withdrawals", "chargebacks", "open_trades", "pending_trades",
                "close_trades", "client_finance_tab", "trade_tab" };

            var columnNanme = "ID";

            // create user
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

            // get user apiKey
            var userApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: userApiKey);

            var clientsIds = new List<string> { clientId };

            //// connect One User To 1 Client
            //_apiFactory
            //   .ChangeContext<IClientsApi>(_driver)
            //   .PatchMassAssignSaleAgentsRequest(_crmUrl, userId,
            //   clientsIds, apiKey: userApiKey);

            // remove ID column from all the tables
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .PutTableColumnVisibilityRequest(_crmUrl, tableNames,
                columnNanme, false, userApiKey);

            // add ID column to all the tables
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .PutTableColumnVisibilityRequest(_crmUrl, tableNames,
                columnNanme, true, userApiKey);
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

        // create user
        // remove the id column of the following tables: "clients",
        // "deposits", "bonuses", "withdrawals", "chargebacks", "open_trades", "pending_trades",
        // "close_trades", "client_finance_tab", "trade_tab"
        // the id column to the above tables
        // navigate to each table and verify the id column display
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyColumnsVisibilityAfterLogOutTest()
        {
            // client table
            _apiFactory
               .ChangeContext<IMenus>(_driver)
               .ClickOnMenuItem<IClientsPageUi>()
               .CheckIfIdColumnExist();

            // client card finance tab table
            _apiFactory
                .ChangeContext<IClientsPageUi>(_driver)
                .ChangeContext<ISearchResultsUi>(_driver)
                .ClickOnClientFullName()
                .ClickOnFinanceTab()
                .CheckIfIdColumnExist();

            // client card trade tab table
            _apiFactory
                .ChangeContext<IClientCardUi>(_driver)
                .ClickOnTradeTab()
                .VerifyIdColumnExist();

            // banking deposits table
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/crm/banking/deposits")
                .ChangeContext<IDepositsPageUi>(_driver)
                .WaitForDepositTableToLoad()
                .CheckIfIdColumnExist();

            // banking bonuses table
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/crm/banking/bonuses")
                .ChangeContext<IBonusesPageUi>(_driver)
                .CheckIfIdColumnExist();

            // banking withdrawals table
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/crm/banking/withdrawals")
                .ChangeContext<IWithdrawalsPageUi>(_driver)
                .CheckIfIdColumnExist();

            // banking chargebacks table
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/crm/banking/chargebacks")
                .ChangeContext<IChargebacksPageUi>(_driver)
                .CheckIfIdColumnExist();

            // cfd open trades table
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/groups/trades/open")
                .ChangeContext<IOpenTradesPageUi>(_driver)
                .CheckIfIdColumnExist();

            // cfd close trades table
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/groups/trades/close")
                .ChangeContext<IClosedTradesPageUi>(_driver)
                .CheckIfIdColumnExist();

            // cfd pending trades table
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/groups/trades/pending")
                .ChangeContext<IPendingTradesPageUi>(_driver)
                .CheckIfIdColumnExist();        
        }
    }
}
