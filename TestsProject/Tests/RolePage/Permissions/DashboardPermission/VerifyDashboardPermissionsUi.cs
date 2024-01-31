using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.DashboardPermission
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyDashboardPermissionsUi : TestSuitBase
    {

        public VerifyDashboardPermissionsUi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _roleName;
        private string _userId;
        private string _browserName;
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
            roleData.ErpPermissions.Remove("boxes_statistics");
            roleData.ErpPermissions.Remove("dashboard_sales_performance");
            roleData.ErpPermissions.Remove("dashboard_calendar");
            roleData.ErpPermissions.Remove("dashboard_campaigns");
            roleData.ErpPermissions.Remove("dashboard_transactions");
            roleData.ErpPermissions.Remove("dashboard_last_registration");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, roleData);

            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName, role: _roleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);
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
        public void VerifyDashboardPermissionsUiTest()
        {          
            var expectedErrorMessage = "Method Not Allowed";

            var actualBoxesStatisticsErrorMessage = _apiFactory
                .ChangeContext<IDashboardApi>()
                .GetBoxesStatisticsRequest(_crmUrl, _currentUserApiKey, false);

            _apiFactory
                .ChangeContext<IDashboardPageUi>(_driver)
                .VerifyCalanderNotExist()
                .VerifyDepositsNotExist()
                .VerifyPerformanceAndDoNotExist()
                .VerifyWithdrawalsNotExist()
                .VerifyLastRegistrationNotExist();

            var actualLastRegistrationErrorrMessage = _apiFactory
                .ChangeContext<IDashboardApi>()
                .GetLastRegistrationRequest(_crmUrl, _currentUserApiKey, false);
          
            Assert.Multiple(() =>
            {
                Assert.True(actualBoxesStatisticsErrorMessage == expectedErrorMessage,
                    $" expected Boxes Statistics Error Message: {expectedErrorMessage}" +
                    $" actual Boxes Statistics Error Message: {actualBoxesStatisticsErrorMessage}");

                Assert.True(actualLastRegistrationErrorrMessage == expectedErrorMessage,
                   $" expected Last Registration Error Message: {expectedErrorMessage}" +
                   $" actual Last Registration Error Message: {actualLastRegistrationErrorrMessage}");
            });
        }
    }
}