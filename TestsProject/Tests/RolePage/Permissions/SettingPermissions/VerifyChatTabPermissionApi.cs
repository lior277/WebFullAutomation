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
    public class VerifyChatTabPermissionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _userId;
        private string _chatMessageId;
        private GetRoleByNameResponse _roleData;
        private string _userName;
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
            //roleData.ErpPermissions.Remove("settings");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, _roleData);

            // create user
            _userName = TextManipulation.RandomString();

            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName, role: _roleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // Get Chat Messages 
            _chatMessageId = _apiFactory
                .ChangeContext<IChatTabApi>()
                .GetChatMessagesRequest(_crmUrl)
                .First()
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
        public void VerifyChatTabPermissionApiTest()
        {
            var chatTabSectionNames = new List<string> { $"chat-messages/{_chatMessageId}"};
            var expectedErrorMessage = "Method Not Allowed";

            // get chat tab in settings
            var getSettingBySectionName = _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .GetSettingBySectionNameRequest(_crmUrl, chatTabSectionNames);

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, _roleName);

            var erpPermmisions = _roleData.ErpPermissions;
            erpPermmisions.Remove("settings");
            _roleData.ErpPermissions = erpPermmisions;

            // edit role and Remove "settings"
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _roleData);

            // update chat tab in settings
            var actualPutSettingWithErrorMessage = _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutSettingBySectionNameRequest(_crmUrl,
                getSettingBySectionName, _currentUserApiKey, false);

            // Get Chat Messages 
           var actualPatchEnableChatForUserErrorMessage = _apiFactory
                .ChangeContext<IChatTabApi>()
                .PatchEnableChatForUserRequest(_crmUrl, _userId,
                _currentUserApiKey, checkStatusCode: false);

            Assert.Multiple(() =>
            {
                Assert.True(actualPutSettingWithErrorMessage.Values.FirstOrDefault() == expectedErrorMessage,
                    $" expected Put Setting With Error Message: {expectedErrorMessage}" +
                    $" actual Put Setting With Error Message: {actualPutSettingWithErrorMessage.DictionaryToString()}");

                Assert.True(actualPatchEnableChatForUserErrorMessage == expectedErrorMessage,
                    $" expected Patch Enable Chat For User Error Message: {expectedErrorMessage}" +
                    $" actual Patch Enable Chat For User Error Message: {actualPatchEnableChatForUserErrorMessage}");
            });
        }
    }
}