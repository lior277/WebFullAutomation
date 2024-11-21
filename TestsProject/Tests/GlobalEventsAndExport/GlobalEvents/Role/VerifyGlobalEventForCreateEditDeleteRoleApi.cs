// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents.Banner
{
    [TestFixture]
    public class VerifyGlobalEventForCreateEditDeleteRoleApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _userName;
        private string _expectedRoleName = TextManipulation.RandomString();

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create user
            _userName = TextManipulation.RandomString();

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            roleData.Name = _expectedRoleName;

            // create role
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, roleData, _currentUserApiKey);

            // get role
            roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, _expectedRoleName);

            // edit role
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, roleData, _currentUserApiKey);

            // delete role
            _apiFactory
                .ChangeContext<IRolesApi>()
                .DeleteRoleRequest(_crmUrl, _expectedRoleName, _currentUserApiKey);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
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
        public void VerifyGlobalEventForCreateEditDeleteRoleApiTest()
        {
            var expectedTypeList = new List<string>()
            { { "create_role" }, { "edit_role" }, { "delete_role" } };

            var expectedGlobal = true;
            var expectedActionMadeBy = _userName;
            var expectedActionMadeByUserId = _userId;

            var actualGlobalList = new List<bool>();
            var actualTypeList = new List<string>();
            var actualMadeByList = new List<string>();
            var actualMadeByUserIdList = new List<string>();
            var actualRoleNameList = new List<string>();

            // get global events
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _userName, _currentUserApiKey);

            actualGlobals.ForEach(p => actualGlobalList.Add(p.global));
            actualGlobals.ForEach(p => actualTypeList.Add(p.type));
            actualGlobals.ForEach(p => actualMadeByList.Add(p.action_made_by));
            actualGlobals.ForEach(p => actualMadeByUserIdList.Add(p.action_made_by_user_id));
            actualGlobals.ForEach(p => actualRoleNameList.Add(p.role_name));

            Assert.Multiple(() =>
            {
                Assert.True(actualRoleNameList.All(p => p.Equals(_expectedRoleName)),
                    $" actual Role Name : {actualRoleNameList.ListToString()}" +
                    $" expected Role Name: {_expectedRoleName}");

                Assert.True(actualTypeList.CompareTwoListOfString(expectedTypeList).Count == 0,
                    $" actual Type List : {actualTypeList.ListToString()}" +
                    $" expected type List: {expectedTypeList.ListToString()}");

                Assert.True(actualGlobalList.All(p => p.Equals(true)),
                    $" actual Global list : {actualGlobalList.ListToString()}" +
                    $" expected Global list: {expectedGlobal}");

                Assert.True(actualMadeByList.All(p => p.Equals(_userName)),
                    $" actual user name : {actualMadeByList.ListToString()}" +
                    $" expected user name: {_userName}");

                Assert.True(actualMadeByUserIdList.All(p => p.Equals(_userId)),
                    $" actual user id : {actualMadeByUserIdList.ListToString()}" +
                    $" expected user id: {_userId}");
            });
        }
    }
}