using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.RolesPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyCreateRole : TestSuitBase
    {
        #region Test Preparation
        public VerifyCreateRole(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private string _roleName;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);

            #region PreCondition
            _driver = GetDriver();
            _roleName = TextManipulation.RandomString();

            var userName = TextManipulation.RandomString();

            // create user
            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

            #endregion
        }

        [TearDown]
        public void TearDown()
        {      
            try
            {
                // delete created role
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
        public void VerifyCreateRoleTest()
        {
            var permissionName = "Create client ";

            var roleDetails = _apiFactory
                 .ChangeContext<IMenus>(_driver)
                 .ClickOnMenuItem<IRolesPageUi>()
                 .ClickOnCreateRoleButton()
                 .CreateRolePipe(_roleName, permissionName)
                 .SearchRole(_roleName)
                 .GetSearchResultDetails<SearchResultRoles>().First();

            Assert.IsTrue(
                roleDetails.name == _roleName,
                  $"expected role Name: admin, actual role Name: {roleDetails.name}");
        }
    }
}
