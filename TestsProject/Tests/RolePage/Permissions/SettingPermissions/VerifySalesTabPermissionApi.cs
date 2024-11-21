// Ignore Spelling: Api

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
    public class VerifySalesTabPermissionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _userId;
        private GetRoleByNameResponse _rolrData;
        private string _userName;
        private string _depositRangeId;
        private string _salasAgentSalaryId;
        private string _savingAccountId;
        private string _accountTypeId;
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
            roleData.ErpPermissions.Remove("settings");

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

            // verify default SA exist
            // create Saving Account
            var savingAccountName = _apiFactory
                 .ChangeContext<ISalesTabApi>()
                 .PostCreateSavingAccountRequest(_crmUrl);

            _savingAccountId = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .GetSavingAccountsRequest(_crmUrl)
                .SavingAccountData
                .Where(p => p.Name == savingAccountName)
                .FirstOrDefault()
                .Id;

            _accountTypeId = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .CreateAutomationAccountTypePipe(_crmUrl)
                .AccountTypeId;

            _depositRangeId = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .PostCreateDepositRangePipe(_crmUrl);

            _salasAgentSalaryId = _apiFactory
               .ChangeContext<ISalesTabApi>()
               .CreateSalasAgentSalaryPipe(_crmUrl, 10);

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
        public void VerifySalesTabPermissionApiTest()
        {
            var salesTabSectionNames = new List<string> { $"saving-account/{_savingAccountId}",
                $"account-types/{_accountTypeId}",
                $"deposit-range/{_depositRangeId}", $"salary/{_salasAgentSalaryId}"};

            var expectedErrorMessage = "Method Not Allowed";

            var getSettingBySectionName = _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .GetSettingBySectionNameRequest(_crmUrl, salesTabSectionNames);

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