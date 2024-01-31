using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents.Banner
{
    [TestFixture]
    public class VerifyGlobalEventForCreateEditDeleteAccountTypeApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _userName;
        private string _expectedAccountTypeName;
        private string _expectedAccountTypeId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create user
            _userName = TextManipulation.RandomString();

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName,
             role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            _expectedAccountTypeName = TextManipulation.RandomString();

            // create Account Type
            _apiFactory
                .ChangeContext<ISalesTabApi>()
                .PostCreateAccountTypeRequest(_crmUrl, _expectedAccountTypeName, _currentUserApiKey);

            // edit Account Type
            var accountTypeData = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .GetAccountTypesRequest(_crmUrl)
                .AccountTypeData
                .Where(p => p.AccountTypeName == _expectedAccountTypeName)
                .FirstOrDefault();

            _apiFactory
                .ChangeContext<ISalesTabApi>()
                .PutAccountTypeRequest(_crmUrl, accountTypeData, _currentUserApiKey);

            _expectedAccountTypeId = accountTypeData.AccountTypeId;

            _apiFactory
                .ChangeContext<ISalesTabApi>()
                .DeleteAccountTypeRequest(_crmUrl, _expectedAccountTypeId, _currentUserApiKey);
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
        public void VerifyGlobalEventForCreateEditDeleteAccountTypeApiTest()
        {
            var expectedTypeList = new List<string>()
            { { "create_account_type" }, { "edit_account_type" }, { "delete_account_type" } };

            var expectedGlobal = true;
            var expectedActionMadeByUser = _userName;
            var expectedActionMadeByUserId = _userId;
            var actualTypeList = new List<string>();
            var actualGlobalList = new List<bool>();
            var actualMadeByUserIdList = new List<string>();
            var actualMadeByList = new List<string>();
            var actualAccountTypsNameList = new List<string>();
            var actualAccountTypeIdList = new List<string>();
            var actualActionMadeByUserId = new List<string>();

            // get global event 
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _userName, _currentUserApiKey);

            actualGlobals.ForEach(p => actualAccountTypsNameList.Add(p.account_type_name));
            actualGlobals.ForEach(p => actualAccountTypeIdList.Add(p.account_type_id));
            actualGlobals.ForEach(p => actualTypeList.Add(p.type));
            actualGlobals.ForEach(p => actualGlobalList.Add(p.global));
            actualGlobals.ForEach(p => actualMadeByList.Add(p.action_made_by));
            actualGlobals.ForEach(p => actualActionMadeByUserId.Add(p.action_made_by_user_id));        

            Assert.Multiple(() =>
            {
                Assert.True(actualAccountTypeIdList[0].Equals(_expectedAccountTypeId) &&
                    actualAccountTypeIdList[1].Equals(_expectedAccountTypeId),
                    $" actual account type id : {actualAccountTypeIdList.ListToString()}" +
                    $" expected account type id: {_expectedAccountTypeId}");

                Assert.True(actualAccountTypsNameList.All(p => p.Equals(_expectedAccountTypeName)),
                    $" actual  account type name : {actualAccountTypsNameList.ListToString()}" +
                    $" expected  account type  name: {_expectedAccountTypeName}");

                Assert.True(actualTypeList.CompareTwoListOfString(expectedTypeList).Count == 0,
                    $" actual Type List : {actualTypeList.ListToString()}" +
                    $" expected type List: {expectedTypeList.ListToString()}");

                Assert.True(actualGlobalList.All(p => p.Equals(true)),
                    $" actual Global list : {actualGlobalList.ListToString()}" +
                    $" expected Global list: {expectedGlobal}");

                Assert.True(actualMadeByList.All(p => p.Equals(_userName)),
                    $" actual user name : {actualMadeByList.ListToString()}" +
                    $" expected user name: {_userName}");

                Assert.True(actualActionMadeByUserId.All(p => p.Equals(expectedActionMadeByUserId)),
                    $" actual user name : {actualActionMadeByUserId.ListToString()}" +
                    $" expected user name: {expectedActionMadeByUserId}");
            });
        }
    }
}