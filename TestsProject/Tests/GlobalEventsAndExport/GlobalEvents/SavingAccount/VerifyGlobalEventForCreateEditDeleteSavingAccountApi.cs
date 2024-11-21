// Ignore Spelling: Api

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

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents.SavingAccount
{
    [TestFixture]
    public class VerifyGlobalEventForCreateEditDeleteSavingAccountApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _userName;
        private string _expectedSavingAccountName;
        private string _expectedSavingAccountId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

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

            // verify default SA exist
            // create Saving Account
            _expectedSavingAccountName = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .PostCreateSavingAccountRequest(_crmUrl, apiKey: _currentUserApiKey);

            // get saving account id
            _expectedSavingAccountId = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .GetSavingAccountsRequest(_crmUrl, _currentUserApiKey)
                .SavingAccountData
                .Where(p => p.Name == _expectedSavingAccountName)
                .FirstOrDefault()
                .Id;

            // edit Saving Account
            _apiFactory
               .ChangeContext<ISalesTabApi>()
               .PutEditSavingAccountRequest(_crmUrl,
               _expectedSavingAccountId, _expectedSavingAccountName,
               apiKey: _currentUserApiKey);

            // delete Saving Account
            _apiFactory
               .ChangeContext<ISalesTabApi>()
               .DeleteSavingAccountRequest(_crmUrl, _expectedSavingAccountId,
               apiKey: _currentUserApiKey);
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
        public void VerifyGlobalEventForCreateEditDeleteSavingAccountApiTest()
        {
            var expectedTypeList = new List<string>()
            { { "saving_account_create" }, { "saving_account_change" }, { "delete_saving_account" } };

            var expectedGlobal = true;
            var expectedActionMadeByUser = _userName;
            var expectedActionMadeByUserId = _userId;
            var actualTypeList = new List<string>();
            var actualGlobalList = new List<bool>();
            var actualMadeByList = new List<string>();
            var actualMadeByUserIdList = new List<string>();
            var actualSavingAccountNameList = new List<string>();
            var actualSavingAccountIdList = new List<string>();

            // get global events
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _userName, _currentUserApiKey);

            actualGlobals.ForEach(p => actualGlobalList.Add(p.global));
            actualGlobals.ForEach(p => actualTypeList.Add(p.type));
            actualGlobals.ForEach(p => actualMadeByList.Add(p.action_made_by));
            actualGlobals.ForEach(p => actualMadeByUserIdList.Add(p.action_made_by_user_id));
            actualGlobals.ForEach(p => actualSavingAccountNameList.Add(p.saving_account_name));
            actualGlobals.ForEach(p => actualSavingAccountIdList.Add(p.saving_account_id));

            Assert.Multiple(() =>
            {
                Assert.True(actualGlobalList.All(p => p.Equals(true)),
                    $" actual Global list : {actualGlobalList.ListToString()}" +
                    $" expected Global list: {expectedGlobal}");

                Assert.True(actualTypeList.CompareTwoListOfString(expectedTypeList).Count == 0,
                    $" actual Type List : {actualTypeList.ListToString()}" +
                    $" expected type List: {expectedTypeList.ListToString()}");

                Assert.True(actualMadeByList.All(p => p.Equals(_userName)),
                    $" actual user name : {actualMadeByList.ListToString()}" +
                    $" expected user name: {_userName}");

                Assert.True(actualMadeByUserIdList.All(p => p.Equals(expectedActionMadeByUserId)),
                    $" actual user id : {actualMadeByUserIdList.ListToString()}" +
                    $" expected user name: {expectedActionMadeByUserId}");

                Assert.True(actualSavingAccountNameList.All(p => p.Equals(_expectedSavingAccountName)),
                    $" actual Sales Status Name : {actualSavingAccountNameList.ListToString()}" +
                    $" expected Sales Status Name: {_expectedSavingAccountName}");

                Assert.True(actualSavingAccountIdList[0].Equals(_expectedSavingAccountId) &&
                    actualSavingAccountIdList[1].Equals(_expectedSavingAccountId),
                    $" actual Sales Status Name : {actualSavingAccountIdList.ListToString()}" +
                    $" expected Sales Status Name: {_expectedSavingAccountId}");
            });
        }
    }
}