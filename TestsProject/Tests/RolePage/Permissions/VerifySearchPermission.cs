using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.RolesPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions
{
    [TestFixture(DataRep.Chrome)]
    public class VerifySearchPermission : TestSuitBase
    {
        public VerifySearchPermission(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _browserName;
        private IWebDriver _driver;
        private string _userName;
        private string _userId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _roleName = TextManipulation.RandomString();
            _driver = GetDriver();

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AgentRole);

            roleData.Name = _roleName;

            // create user
            _userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, roleData, currentUserApiKey);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IRolesPageUi>()
                .SearchRole(_roleName)
                .ClickOnEditRoleButton()
                .ClickOnPermissionTab();
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
        public void VerifySearchPermissionTest()
        {
            var permissionName = "Forward message ";

            var PermissionPositionBeforeSearch = _apiFactory
                .ChangeContext<IRoleUi>(_driver)
                .GetPermissionPosition(permissionName);

            var PermissionPositionAfterSearch = _apiFactory
                .ChangeContext<IRoleUi>(_driver)
                .SearchPermission(permissionName)
                .ClickOnAutoComplate(permissionName)
                .VerifyPermissionHighlighted(permissionName)
                .GetPermissionPosition(permissionName);

            Assert.Multiple(() =>
            {
                Assert.True(PermissionPositionAfterSearch - PermissionPositionBeforeSearch > 1000,
                    $" Permission position on the screen before search: {PermissionPositionBeforeSearch}" +
                    $" actual Permission position on the screen after search: {PermissionPositionAfterSearch}");
            });
        }
    }
}