using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
    public class VerifyOfficeTabPermissionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _userId;
        private GetRoleByNameResponse _rolrData;
        private string _userName;
        private string _officeId;
        private string _trunkId;
        private string _dialerId;
        private string _currentUserApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _roleName = TextManipulation.RandomString();
            var officeName = TextManipulation.RandomString();
            var pbxName = TextManipulation.RandomString();

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
                .PostCreateUserRequest(_crmUrl, _userName,
                role: _roleName, pbxName: pbxName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create office
            _officeId = _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .PostCreateOfficeRequest(_crmUrl, officeName, pbxName)
                .GetOfficesByName(_crmUrl, officeName)
                ._id;

            // create and get trunk created
            _apiFactory
                 .ChangeContext<IOfficeTabApi>()
                 .PostCreateTrunkPipe(_crmUrl, officeId: _officeId, pbxName: pbxName);

            // get trunk there is only one trunk
            _trunkId = _apiFactory
                 .ChangeContext<IOfficeTabApi>()
                 .GetTrunkRequest(_crmUrl)
                 .FirstOrDefault()
                 ._id;

            // create Dialer get id
            _dialerId = _apiFactory
                 .ChangeContext<IOfficeTabApi>()
                 .PostCreateDialerRequest(_crmUrl, _officeId)
                 .GetDialers(_crmUrl)
                 .Where(p => p.office == _officeId)
                 .FirstOrDefault()
                 ._id;
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

                // delete dialer and office
                _apiFactory
                    .ChangeContext<IOfficeTabApi>()
                    .DeleteDialer(_crmUrl, _dialerId)
                    .DeleteOfficeByIdRequest(_crmUrl, _officeId);

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
        public void VerifyOfficeTabPermissionApiTest()
        {
            var officeTabSectionNames = new List<string> { $"offices/{_officeId}",
                "config/sales_dashboard", $"trunks/{_trunkId}", $"dialers/{_dialerId}" };

            var expectedErrorMessage = "Method Not Allowed";

            var getSettingBySectionName = _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .GetSettingBySectionNameRequest(_crmUrl, officeTabSectionNames);

            // get role by name
            _rolrData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, _roleName);

            var erpPermmisions = _rolrData.ErpPermissions;
            erpPermmisions.Remove("settings");
            _rolrData.ErpPermissions = erpPermmisions;

            // edit role and unable delete_comments
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _rolrData);

            var actualPutSettingWithErrorMessage = _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutSettingBySectionNameRequest(_crmUrl,
                getSettingBySectionName, _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualPutSettingWithErrorMessage.Values.All(p => p.Equals(expectedErrorMessage)),
                    $" expected Put Setting With Error Message: {expectedErrorMessage}" +
                    $" actual Put Setting With Error Message: {actualPutSettingWithErrorMessage.DictionaryToString()}");
            });
        }
    }
}