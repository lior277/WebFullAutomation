// Ignore Spelling: Api

using System;
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
    public class VerifyMassDeleteUsersApi : TestSuitBase
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
        public void VerifyMassDeleteUsersApiTest()
        {
            var expectedTypeList = new List<string>()
            { { "mass_delete_erp_users" } };

            _apiFactory
             .ChangeContext<IUsersApi>()
             .DeleteMassUserRequest(_crmUrl, _expectedUsersIds, _currentUserApiKey);

           var actualDeletedUsersNames = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetDeletedUsersRequest(_crmUrl, _currentUserApiKey)
                .userData
                .Select(p => p.first_name)
                .ToList();

            Assert.Multiple(() =>
            {
                Assert.True(actualDeletedUsersNames
                    .CompareTwoListOfString(_expectedUsersNames).Count() == 0,
                    $" actual user names : {actualDeletedUsersNames.ListToString()}" +
                    $" expected user names: {_expectedUsersNames.ListToString()}");
            });
        }
    }
}