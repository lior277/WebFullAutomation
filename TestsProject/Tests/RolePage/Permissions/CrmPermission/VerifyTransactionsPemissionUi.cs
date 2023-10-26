// Ignore Spelling: Crm

using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.CrmPermission
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyTransactionsPermissionUi : TestSuitBase
    {
        #region Test Preparation
      
        public VerifyTransactionsPermissionUi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private string _roleName;
        private string _userId;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();
            _roleName = TextManipulation.RandomString();

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            roleData.Name = _roleName;
            roleData.ErpPermissions.Remove("see_transactions");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, roleData);

            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName, role: _roleName);

            // create client
            var clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
               .ChangeContext<ICreateClientApi>()
               .CreateClientRequest(_crmUrl, clientName);

            var clientsIds = new List<string> { clientId };

            // connect firs user to client
            _apiFactory
               .ChangeContext<IClientsApi>()
               .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
               clientsIds);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

            _driver.WaitForPageLoad("dashboard");
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
        public void VerifyTransactionsPermissionUiTest()
        {
            var expectedError = "AIRSOFT Error 404 something's" +
                " not right here  the page you are looking for" +
                " cannot be found!  TRY AGAIN RETURN TO HOME";

            var actualErrorBankingDeposits = _apiFactory
                 .ChangeContext<ISharedStepsGenerator>(_driver)
                 .NavigateToPageByName(_crmUrl, "/crm/banking/deposits", checkUrl: false)
                 .GetError404();

            var actualErrorBankingBonuses = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/crm/banking/bonuses", checkUrl: false)
                .GetError404();

            var actualErrorBankingWithdrawals = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/crm/banking/withdrawals", checkUrl: false)
                .GetError404();

            var actualErrorBankingChargebacks = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/crm/banking/chargebacks", checkUrl: false)
                .GetError404();

            Assert.Multiple(() =>
            {
                Assert.True(actualErrorBankingDeposits == expectedError,
                    $" expected Error Banking Deposits: {expectedError}" +
                    $" actual Error Banking Deposits: {actualErrorBankingDeposits}");

                Assert.True(actualErrorBankingBonuses == expectedError,
                    $" expected Error Banking Bonuses: {expectedError}" +
                    $" actual Error Banking Bonuses: {actualErrorBankingBonuses}");

                Assert.True(actualErrorBankingWithdrawals == expectedError,
                    $" expected Error Banking Withdrawals: {expectedError}" +
                    $" actual Error Banking Withdrawals: {actualErrorBankingWithdrawals}");

                Assert.True(actualErrorBankingChargebacks == expectedError,
                    $" expected Error Banking Chargebacks: {expectedError}" +
                    $" actual Error Banking Chargebacks: {actualErrorBankingChargebacks}");
            });
        }
    }
}
