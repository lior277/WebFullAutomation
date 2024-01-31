using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents
{
    [TestFixture]
    public class VerifyGlobalEventForCreateEditDeleteOfficeApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _officeId;
        private string _userName;
        private string _expectedOfficeName;

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

            _expectedOfficeName = $"Test{TextManipulation.RandomString()}";

            // create office
            _apiFactory
               .ChangeContext<IOfficeTabApi>()
               .PostCreateOfficeRequest(_crmUrl,
               _expectedOfficeName, apiKey: _currentUserApiKey);

            // get office by name 
            var officeData = _apiFactory
               .ChangeContext<IOfficeTabApi>()
               .GetOfficesByName(_crmUrl, _expectedOfficeName, _currentUserApiKey);

            // update the office for the global event
            _apiFactory
               .ChangeContext<IOfficeTabApi>()
               .PutOfficeRequest(_crmUrl, officeData, _currentUserApiKey);

            _officeId = officeData._id;

            _apiFactory
              .ChangeContext<IOfficeTabApi>()
              .DeleteOfficeByIdRequest(_crmUrl, _officeId, _currentUserApiKey);
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
        public void VerifyGlobalEventForCreateEditDeleteOfficeApiTest()
        {
            var expectedTypes = new List<string>()
            { { "create_office" }, { "edit_office" }, { "delete_office" } };

            var expectedGlobal = true;
            var expectedActionMadeBy = _userName;
            var actualTypeList = new List<string>();
            var actualGlobalList = new List<bool>();
            var actualMadeByList = new List<string>();
            var actualMadeByUserIdList = new List<string>();
            var actualOfficeNameList = new List<string>();
            var actualOfficeId = new List<string>();

            // get global event create office
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, expectedActionMadeBy,
               _currentUserApiKey);

            actualGlobals.ForEach(p => actualTypeList.Add(p.type));
            actualGlobals.ForEach(p => actualGlobalList.Add(p.global));
            actualGlobals.ForEach(p => actualMadeByList.Add(p.action_made_by));
            actualGlobals.ForEach(p => actualMadeByUserIdList.Add(p.action_made_by_user_id));
            actualGlobals.ForEach(p => actualOfficeNameList.Add(p.office_name));
            actualGlobals.ForEach(p => actualOfficeId.Add(p.office_id));

            Assert.Multiple(() =>
            {
                Assert.True(actualTypeList.CompareTwoListOfString(expectedTypes).Count == 0,
                    $" actual Type List : {actualTypeList.ListToString()}" +
                    $" expected type List: {expectedTypes.ListToString()}");

                Assert.True(actualGlobalList.All(p => p.Equals(true)),
                    $" actual Global list : {actualGlobalList.ListToString()}" +
                    $" expected Global list: {expectedGlobal}");

                Assert.True(actualMadeByList.All(p => p.Equals(expectedActionMadeBy)),
                    $" actual Made By User name : {actualMadeByList.ListToString()}" +
                    $" expected Made By User name: {expectedActionMadeBy}");

                Assert.True(actualMadeByUserIdList.All(p => p.Equals(_userId)),
                    $" actual Made By User id : {actualMadeByUserIdList.ListToString()}" +
                    $" expected Made By User id: {_userId}");

                Assert.True(actualOfficeNameList.All(p => p.Equals(_expectedOfficeName)),
                    $" actual Office Name : {actualOfficeNameList.ListToString()}" +
                    $" expected Office Name: {_expectedOfficeName}");

                Assert.True(actualOfficeId?[0] == _officeId
                    && actualOfficeId?[1] == _officeId,
                    $" actual Office id : {actualOfficeId.ListToString()}" +
                    $" expected Office id: {_officeId}");
            });
        }
    }
}