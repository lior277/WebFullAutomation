// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents.Banner
{
    [TestFixture]
    public class VerifyGlobalEventForMassDeleteUserApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _firstUserId;
        private string _expectedParentUserName;
        private string _secondUserId;
        private string _expectedParentUserId;
        private List<string> _expectedUsersNames = new List<string>();
        private string[] _expectedUsersIds = new string[2];

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create user
            _expectedParentUserName = TextManipulation.RandomString();

            _expectedParentUserId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _expectedParentUserName,
             role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _expectedParentUserId);
            #endregion

            var firstUserName = TextManipulation.RandomString();

            _firstUserId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, firstUserName, apiKey: _currentUserApiKey);

            var secondUserName = TextManipulation.RandomString();

            // create another user
            _secondUserId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, secondUserName, apiKey: _currentUserApiKey);

            _expectedUsersIds = new string[] { _firstUserId, _secondUserId };
            _expectedUsersNames.Add(firstUserName);
            _expectedUsersNames.Add(secondUserName);

            _apiFactory
               .ChangeContext<IUsersApi>()
               .DeleteMassUserRequest(_crmUrl, _expectedUsersIds, _currentUserApiKey);
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyGlobalEventForMassDeleteUserApiTest()
        {
            var expectedTypeList = new List<string>()
            { { "mass_delete_erp_users" } };

            var expectedGlobal = true;
            var expectedActionMadeByUser = _expectedParentUserName;
            var expectedActionMadeByUserId = _expectedParentUserId;
            var expectedUserFullName = $"{_expectedParentUserName}  {_expectedParentUserName}";
            var actualTypeList = new List<string>();
            var actualGlobalList = new List<bool>();
            var actualActionMadeByUserIdList = new List<string>();
            var actualActionMadeByList = new List<string>();
            var actualUserFullNameList = new List<string>();

            // get global event create saving account
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _expectedParentUserName, _currentUserApiKey)
               .Where(p => p.type == "mass_delete_erp_users")
               .FirstOrDefault();        

            Assert.Multiple(() =>
            {
                Assert.True(actualGlobals
                    .usernames.CompareTwoListOfString(_expectedUsersNames).Count == 0,
                    $" actual user names : {actualGlobals.usernames.ListToString()}" +
                    $" expected user names: {_expectedUsersNames.ListToString()}");

                Assert.True(actualGlobals.global == expectedGlobal,
                    $" actual  global  : {actualGlobals.global}" +
                    $" expected global : {expectedGlobal}");

                Assert.True(actualGlobals.action_made_by == expectedActionMadeByUser,
                    $" actual  action_made_by  : {actualGlobals.action_made_by}" +
                    $" expected action_made_by : {expectedActionMadeByUser}");

                Assert.True(actualGlobals.action_made_by_user_id == expectedActionMadeByUserId,
                    $" actual  action_made_by_user_id  : {actualGlobals.action_made_by_user_id}" +
                    $" expected action_made_by_user_id : {expectedActionMadeByUserId}");
            });
        }
    }
}