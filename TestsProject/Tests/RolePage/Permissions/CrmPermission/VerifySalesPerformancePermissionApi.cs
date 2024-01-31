// Ignore Spelling: Api Crm

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.sales;
using NUnit.Framework;
using System;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.CrmPermission
{
    [TestFixture]
    public class VerifySalesPerformancePermissionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _userId;
        private string _currentUserApiKey;
        private string _userEmail;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _roleName = TextManipulation.RandomString();

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            roleData.Name = _roleName;
            roleData.ErpPermissions.Remove("see_sales_performance");
            roleData.ErpPermissions.Remove("dashboard_sales_performance");
            roleData.ErpPermissions.Remove("sales_deposit");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, roleData);

            // create user
            var userName = TextManipulation.RandomString();
            _userEmail = userName + DataRep.EmailPrefix;

            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName, role: _roleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifySalesPerformancePermissionApiTest()
        {
            var expectedErrorMessage = "Method Not Allowed";

            var actualSalesPerformanceWithError = _apiFactory
                .ChangeContext<ISalesPageApi>()
                .GetSalesPerformanceRequest(_crmUrl, _currentUserApiKey, false);

            var actualApprovedDepositWithError = _apiFactory
                .ChangeContext<ISalesPageApi>()
                .GetApprovedDepositRequest(_crmUrl, _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualSalesPerformanceWithError == expectedErrorMessage,
                    $" expected Sales Performance Error Message: {expectedErrorMessage}" +
                    $" actual Sales Performance Error Message: {actualSalesPerformanceWithError}");

                Assert.True(actualApprovedDepositWithError == expectedErrorMessage,
                    $" expected Approved Deposit Error Message: {expectedErrorMessage}" +
                    $" actual Approved Deposit Error Message: {actualApprovedDepositWithError}");
            });
        }
    }
}