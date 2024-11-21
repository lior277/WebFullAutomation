// Ignore Spelling: Chrono Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.SettingPermissions
{
    [TestFixture]
    public class VerifyChronoTabPermissionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _userId;
        private string _userName;
        private GetRoleByNameResponse _roleData;
        private string _currentUserApiKey;

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

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, roleData);

            // create user
            _userName = TextManipulation.RandomString();

            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName, role: _roleName);

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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyChronoTabPermissionApiTest()
        {
            var generalTabSectionNames = new List<string> { "config/global-settings?chrono=true" };
            var expectedErrorMessage = "Method Not Allowed";

            var getSettingBySectionName = _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .GetSettingBySectionNameRequest(_crmUrl, generalTabSectionNames);

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, _roleName);

            var erpPermmisions = _roleData.ErpPermissions;
            erpPermmisions.Remove("settings");
            _roleData.ErpPermissions = erpPermmisions;

            // edit role and unable delete_comments
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _roleData);

            var actualPutSettingWithErrorMessage = _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutSettingBySectionNameRequest(_crmUrl,
                getSettingBySectionName, _currentUserApiKey, false);

            Assert.Multiple(() => 
            {
               Assert.True(actualPutSettingWithErrorMessage.Values.All(p => p.Equals(expectedErrorMessage)),
                   $" expected Get Setting With Error Message: {expectedErrorMessage}" +
                   $" actual Get Setting With Error Message: {actualPutSettingWithErrorMessage.DictionaryToString()}");
            });
        }
    }
}