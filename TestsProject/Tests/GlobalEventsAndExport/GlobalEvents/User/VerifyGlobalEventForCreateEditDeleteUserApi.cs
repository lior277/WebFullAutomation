// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents.Banner
{
    [TestFixture]
    public class VerifyGlobalEventForCreateEditDeleteUserApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _expectedUserName;
        private string _expectedUserId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create user
            _expectedUserName = TextManipulation.RandomString();

            _expectedUserId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _expectedUserName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _expectedUserId);
            #endregion

            var userName = TextManipulation.RandomString();

            // create another user
            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName,
               apiKey: _currentUserApiKey);

            var userData = _apiFactory
               .ChangeContext<IUsersApi>()
               .GetUserByIdRequest(_crmUrl, userId)
               .GeneralResponse;

            // edit Account Type 
            _apiFactory
                .ChangeContext<IUserApi>()
                .PutEditUserRequest(_crmUrl, userData, _currentUserApiKey);

            // delete the user
            _apiFactory
                .ChangeContext<IUserApi>()
                .PatchDeleteOrRestoreUserRequest(_crmUrl,
                userId, apiKey: _currentUserApiKey);

            // restore the user
            _apiFactory
                .ChangeContext<IUserApi>()
                .PatchDeleteOrRestoreUserRequest(_crmUrl,
                userId, _currentUserApiKey);
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
        [Description("BUG TYPE DELETE AND RESTORE")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyGlobalEventForCreateEditDeleteUserApiTest()
        {
            var expectedTypeList = new List<string>()
            { { "create_erp_user" }, { "edit_erp_user" },
                { "delete_erp_user" } , { "restore_erp_user" }};

            var expectedGlobal = true;
            var expectedActionMadeByUser = _expectedUserName;
            var expectedActionMadeByUserId = _expectedUserId;
            var expectedUserFullName = $"{_expectedUserName}  {_expectedUserName}";
            var actualTypeList = new List<string>();
            var actualGlobalList = new List<bool>();
            var actualActionMadeByUserIdList = new List<string>();
            var actualActionMadeByList = new List<string>();
            var actualUserFullNameList = new List<string>();

            // get global event create saving account
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _expectedUserName, _currentUserApiKey);

            actualGlobals.ForEach(p => actualTypeList.Add(p.type));
            actualGlobals.ForEach(p => actualGlobalList.Add(p.global));
            actualGlobals.ForEach(p => actualActionMadeByUserIdList.Add(p.action_made_by_user_id));
            actualGlobals.ForEach(p => actualActionMadeByList.Add(p.action_made_by));         

            Assert.Multiple(() =>
            {
                Assert.True(actualActionMadeByUserIdList.All(p => p.Equals(_expectedUserId)),
                    $" actual Action Made By User Id : {actualActionMadeByUserIdList.ListToString()}" +
                    $" expected Action Made By User Id: {_expectedUserId}");

                Assert.True(actualActionMadeByList.All(p => p.Equals(_expectedUserName)),
                    $" actual  Action Made By  : {actualActionMadeByList.ListToString()}" +
                    $" expected  Action Made By : {_expectedUserName}");

                Assert.True(actualTypeList.CompareTwoListOfString(expectedTypeList).Count == 0,
                    $" actual Type List : {actualTypeList.ListToString()}" +
                    $" expected type List: {expectedTypeList.ListToString()}");

                Assert.True(actualGlobalList.All(p => p.Equals(true)),
                    $" actual Global list : {actualGlobalList.ListToString()}" +
                    $" expected Global list: {expectedGlobal}");
            });
        }
    }
}