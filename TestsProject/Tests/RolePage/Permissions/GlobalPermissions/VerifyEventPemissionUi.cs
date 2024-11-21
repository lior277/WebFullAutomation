using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.GlobalPermissions
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyEventPermissionUi : TestSuitBase
    {
        #region Test Preparation
      
        public VerifyEventPermissionUi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private string _roleName;
        private string _clientId;
        private string _currentUserApiKey;
        private string _userId;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();
            _roleName = TextManipulation.RandomString();
            var expectedComment = "Automation Comment";

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            roleData.UsersOnly = true;
            roleData.Name = _roleName;
            roleData.ErpPermissions.Remove("events");

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

            // create client 
            var clientName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            var clientsIds = new List<string> { _clientId };

            // create first comment
            _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .PostCommentRequest(_crmUrl,
                _clientId, expectedComment, _currentUserApiKey, false);

            // connect firs user to client
            _apiFactory
               .ChangeContext<IClientsApi>()
               .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId, clientsIds);

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
        public void VerifyEventsPermissionUiTest()
        {
            var expectedEventButtonExist = false;

           var actualEventsButtonExist =  _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .CheckIfEventsButtonExist();

            Assert.Multiple(() =>
            {
                Assert.True(actualEventsButtonExist == expectedEventButtonExist,
                    $" expected Events Button Exist: {expectedEventButtonExist}" +
                    $" actual Events Button Exist: {actualEventsButtonExist}");
            });
        }
    }
}
