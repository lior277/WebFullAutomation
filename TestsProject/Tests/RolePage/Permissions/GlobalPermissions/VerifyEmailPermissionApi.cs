// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.GlobalPermissions
{
    [TestFixture]
    public class VerifyEmailPermissionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userEmail;
        private string _userId;
        private string _subject;
        private string _currentUserApiKey;
        private GetRoleByNameResponse _roleData;
        private Dictionary<string, string> _emailsParams;
        private string _roleName;

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
           
            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, _roleData);

            // get the new role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, _roleName);

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

            _subject = $"Test{TextManipulation.RandomString()}";

            _emailsParams = new Dictionary<string, string> {
                { "type", "custom" }, { "language", "en" },
                { "subject", _subject }, { "body", _subject }, { "name", _userEmail }};

           _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .PostSaveCustomEmailRequest(_crmUrl, _emailsParams, _currentUserApiKey);
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
        public void VerifyEmailPermissionApiTest()
        {
            var expectedErrorMessage = "Method Not Allowed";

            _roleData.ErpPermissions.Remove("settings");
            _roleData.ErpPermissions.Remove("save_email");

            // edit role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _roleData);

            // save email with no permission
            var actualSaveEmailErrorMessage = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .PostSaveCustomEmailRequest(_crmUrl, _emailsParams,
                _currentUserApiKey, false);

           var email = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .GetCustomEmailsRequest(_crmUrl)
                .Where(p => p.name == _userEmail)
                .FirstOrDefault();

            email.body = _subject;

            _roleData.ErpPermissions.Remove("edit_email");

            // edit role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _roleData);

            // update email with no permission
            var actualEditEmailErrorMessage = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .PutCustomEmailRequest(_crmUrl, email, _currentUserApiKey, false);

            _roleData.ErpPermissions.Add("edit_email");
            _roleData.ErpPermissions.Remove("send_email");

            // edit role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _roleData);

            email.user_id = _userId;

            // send email with no permission
            var actualSendEmailErrorMessage = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .PostSendCustomEmailRequest(_crmUrl, " ", clientId: _userId, "", "",
                _currentUserApiKey, checkStatusCode: false);

            Assert.Multiple(() =>
            {
                Assert.True(actualSaveEmailErrorMessage == expectedErrorMessage,
                    $" expected Save Email Error Message: {expectedErrorMessage}" +
                    $" actual Save Email Error Message: {actualSaveEmailErrorMessage}");

                Assert.True(actualEditEmailErrorMessage == expectedErrorMessage,
                    $" expected Edit Email Error Message: {expectedErrorMessage}" +
                    $" actual Edit Email Error Message: {actualEditEmailErrorMessage}");

                Assert.True(actualSendEmailErrorMessage == expectedErrorMessage,
                    $" expected Send Email Error Message: {expectedErrorMessage}" +
                    $" actual Send Email Error Message: {actualSendEmailErrorMessage}");
            });
        }
    }
}