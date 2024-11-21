using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using NUnit.Framework;
using System;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.AccountPermissions
{
    [TestFixture]
    public class VerifyAccountsPermissionsApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _userId;
        private GetUserResponse _user;
        private GetRoleByNameResponse _roleData;
        private string _currentUserApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _roleName = TextManipulation.RandomString();

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            _roleData.Name = _roleName;
            _roleData.ErpPermissions.Remove("all_erp_users");
            _roleData.ErpPermissions.Remove("generate_api_key");
            _roleData.ErpPermissions.Remove("all_roles");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, _roleData);

            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName, role: _roleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            _user = _apiFactory
               .ChangeContext<IUsersApi>()
               .GetUserByIdRequest(_crmUrl, _userId)
               .GeneralResponse;
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
                AfterTest();
            }
        }
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyAccountsPermissionsApiTest()
        {
            var expectedError = "Method Not Allowed";

            var actualEditUserErrorMessage = _apiFactory
                .ChangeContext<IUserApi>()
                .PutEditUserRequest(_crmUrl, _user, _currentUserApiKey, false);

            var actualCreateApiKeyErrorMessage = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId, _currentUserApiKey, false);

            var actualGetRolesErrorMessage = _apiFactory
                .ChangeContext<IRolesApi>()
                .DeleteRoleRequest(_crmUrl, _roleName, _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualEditUserErrorMessage == expectedError,
                    $" expected Edit User Error Message: {expectedError}" +
                    $" actual Edit User Error Message: {actualEditUserErrorMessage}");

                Assert.True(actualCreateApiKeyErrorMessage == expectedError,
                    $" expected Create Api Key Error Message: {expectedError}" +
                    $" actual Create Api Key Error Message: {actualCreateApiKeyErrorMessage}");

                Assert.True(actualGetRolesErrorMessage == expectedError,
                    $" expected GetRoles Error Message: {expectedError}" +
                    $" actual GetRoles Error Message: {actualGetRolesErrorMessage}");
            });
        }
    }
}