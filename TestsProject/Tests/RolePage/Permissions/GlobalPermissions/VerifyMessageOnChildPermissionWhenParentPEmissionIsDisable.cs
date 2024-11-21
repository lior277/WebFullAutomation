using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.RolesPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Threading;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.GlobalPermissions
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyMessageOnChildPermissionWhenParentPermissionIsDisable : TestSuitBase
    {
        public VerifyMessageOnChildPermissionWhenParentPermissionIsDisable(string browser) : base(browser)
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
            _driver = GetDriver();
            _roleName = TextManipulation.RandomString();

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AgentRole);

            roleData.Name = _roleName;
            roleData.ErpPermissions.Remove("all_client_profile");
            roleData.ErpPermissions.Remove("see_client_profile");

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

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            Thread.Sleep(1000); 

            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/accounts/roles")
                .ChangeContext<IRolesPageUi>(_driver)
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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyMessageOnChildPermissionWhenParentPermissionIsDisableTest()
        {
            var expectedPermissionDisableReasonMessage = "You can not enable this" +
                " permission without allowing access to 'Client profile'";

            var permissionName = "Email ";

            var actualPermissionDisableReasonMessage = _apiFactory
                .ChangeContext<IRoleUi>(_driver)
                .HoverOnPermissionByName(permissionName)
                .GetPermissionDisableReasonMessage();

            Assert.Multiple(() =>
            {
                Assert.True(actualPermissionDisableReasonMessage ==
                    expectedPermissionDisableReasonMessage,
                    $" expected Permission Disable Reason Message: {expectedPermissionDisableReasonMessage}" +
                    $" actual Permission Disable Reason Message: {actualPermissionDisableReasonMessage}");
            });
        }
    }
}