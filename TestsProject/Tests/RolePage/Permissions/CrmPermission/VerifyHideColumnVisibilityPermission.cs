// Ignore Spelling: Crm

using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.CrmPermission
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyHideColumnVisibilityPermission : TestSuitBase
    {
        #region Test Preparation
      
        public VerifyHideColumnVisibilityPermission(string browser) : base(browser)
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
            roleData.ErpPermissions.Add("hide_column_visibility");

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

            var clientId =_apiFactory
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
        public void VerifyHideColumnVisibilityPermissionTest()
        {
            var expectedColumnVisibilityButtonExist = false;

           var actualColumnVisibilityButtonExist =  _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .CheckIfColumnsVisibilityButtonExist();

            Assert.Multiple(() =>
            {
                Assert.True(actualColumnVisibilityButtonExist == expectedColumnVisibilityButtonExist,
                    $" expected Column Visibility Button Exist: {expectedColumnVisibilityButtonExist}" +
                    $" actual Column Visibility Button Exist: {actualColumnVisibilityButtonExist}");
            });
        }
    }
}
