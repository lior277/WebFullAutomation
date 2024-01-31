// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.ExportPermissions
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyPermissionForExportApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _userId;
        private IWebDriver _driver;
        private string _browserName;
        private string _exportLink;
        private string _clientEmail;
        private string _currentUserApiKey;
        private string _userEmail;

        public VerifyPermissionForExportApi(string browser) : base(browser)
        {
            _browserName = browser;
        }


        [SetUp]
        public void SetUp()
        {
            var tradeAmount = 1;
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
            BeforeTest(_browserName);
            _driver = GetDriver();

            _roleName = TextManipulation.RandomString();

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            roleData.Name = _roleName;
            roleData.ErpPermissions.Remove("export_banking");
            roleData.ErpPermissions.Remove("export_clients");
            roleData.ErpPermissions.Remove("export_trades");
            roleData.ErpPermissions.Remove("export_users");
            roleData.ErpPermissions.Remove("export_risk");
            roleData.ErpPermissions.Remove("export_client_profile");
            roleData.ErpPermissions.Remove("export_pnl");
            roleData.ErpPermissions.Remove("export_active_campaigns");
            roleData.ErpPermissions.Remove("export_global_events");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, roleData);

            // create user
            var userName = TextManipulation.RandomString();
            _userEmail = userName + DataRep.EmailPrefix;

            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName, role: _roleName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            // connect One User To One Client
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { clientId });

            var fingerPrint = _apiFactory
                .ChangeContext<IUserApi>()
                .GetAllowedFingerPrintFromMongo(_userEmail);

            var shsec = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginCrmRequest(_crmUrl, _userEmail, fingerPrint)
                .GeneralResponse
                .Shsec;

            _exportLink = $"/api/users/trading-platform/export-user/{clientId}?shsec={shsec}";

            // create bonus for getting Table Columns
            _apiFactory
             .ChangeContext<IFinancesTabApi>()
             .PostBonusRequest(_crmUrl, clientId, 10, apiKey: _currentUserApiKey);

            // create Deposit for getting Table Columns
            _apiFactory
             .ChangeContext<IFinancesTabApi>()
             .PostDepositRequest(_crmUrl, clientId, 100000, apiKey: _currentUserApiKey);

            // get login data
            var loginData = _apiFactory
               .ChangeContext<ILoginApi>(_driver)
               .PostLoginToTradingPlatform(tradingPlatformUrl, _clientEmail)
               .GeneralResponse;

            // create Deposit for getting Table Columns
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(tradingPlatformUrl, tradeAmount, loginData)
                .GeneralResponse;

            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PatchCloseTradeRequest(tradingPlatformUrl,
                tradeDetails.TradeId, tradeAmount, loginData);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete role by name
                _apiFactory
                     .ChangeContext<IUserApi>()
                     .PutEditUserRoleRequest(_crmUrl, _userId, DataRep.AdminRole);

                _apiFactory
                    .ChangeContext<IRolesApi>()
                    .DeleteRoleRequest(_crmUrl, _roleName);
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
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyPermissionForExportApiTest()
        {
            var expectedExportTableErrorMessage = "Method Not Allowed";

            // export bonus table with no permission
            var actualExportBonusTableErrorMessage = _apiFactory
                .ChangeContext<IBonusPageApi>()
                .ExportBonusTablePipe(_crmUrl, _clientEmail, _userEmail,
                _currentUserApiKey, false);

            // export deposit table with no permission
            var actualExportDepositTableErrorMessage = _apiFactory
                .ChangeContext<IDepositsPageApi>()
                .ExportDepositsTablePipe(_crmUrl, _clientEmail, _userEmail,
                _currentUserApiKey, false);

            var actualExportClientsTableErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .ExportClientsTablePipe(_crmUrl, _clientEmail, _userEmail,
                _currentUserApiKey, false);

            var actualExportCloseTradeTableErrorMessage = _apiFactory
                .ChangeContext<IClosedTradesPageApi>()
                .ExportCloseTradeTablePipe(_crmUrl, _userEmail,
                _currentUserApiKey, checkStatusCode: false);

            var actualExportRisksTableErrorMessage = _apiFactory
                .ChangeContext<IRiskPageApi>()
                .PostExportRisksTablePipeRequest(_crmUrl, _clientEmail,
                _userEmail, _currentUserApiKey, checkStatusCode: false);

            var actualExportClientCardErrorMessage = _apiFactory
                .ChangeContext<IClientCardApi>(_driver)
                .GetExportClientCardRequest(_crmUrl, _exportLink,
                _currentUserApiKey, false)
                .Message;

            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/accounts/users")
                .VerifyExportTableButtonNotExist();

            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/groups/pnl")
                .VerifyExportTableButtonNotExist();

            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/campaigns/dashboard")
                .VerifyExportTableButtonNotExist();

            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/global-events")
                .VerifyExportTableButtonNotExist();

            Assert.Multiple(() =>
            {
                Assert.True(actualExportBonusTableErrorMessage == expectedExportTableErrorMessage,
                    $" expected Export Bonus Table Error Message: {expectedExportTableErrorMessage}" +
                    $" actual Export Bonus Table Error Message: {actualExportBonusTableErrorMessage}");

                Assert.True(actualExportDepositTableErrorMessage == expectedExportTableErrorMessage,
                    $" expected Export Deposit Table Error Message: {expectedExportTableErrorMessage}" +
                    $" actual Export Deposit Table Error Message: {actualExportDepositTableErrorMessage}");

                Assert.True(actualExportClientsTableErrorMessage == expectedExportTableErrorMessage,
                    $" expected Export Clients Table Error Message: {expectedExportTableErrorMessage}" +
                    $" actual Export Clients Table Error Message: {actualExportClientsTableErrorMessage}");

                Assert.True(actualExportCloseTradeTableErrorMessage == expectedExportTableErrorMessage,
                    $" expected Export Close Trade Table Error Message: {expectedExportTableErrorMessage}" +
                    $" actual Export Close Trade Table Error Message: {actualExportCloseTradeTableErrorMessage}");

                Assert.True(actualExportRisksTableErrorMessage == expectedExportTableErrorMessage,
                    $" expected Export Risks Table Error Message: {expectedExportTableErrorMessage}" +
                    $" actual Export Risks Table Error Message: {actualExportRisksTableErrorMessage}");

                Assert.True(actualExportClientCardErrorMessage == expectedExportTableErrorMessage,
                    $" expected Export Risks Table Error Message: {expectedExportTableErrorMessage}" +
                    $" actual Export Risks Table Error Message: {actualExportClientCardErrorMessage}");
            });
        }
    }
}
