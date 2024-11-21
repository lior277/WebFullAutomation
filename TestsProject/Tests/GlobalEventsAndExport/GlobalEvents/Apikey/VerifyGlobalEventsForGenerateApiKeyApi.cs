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

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents.Banner
{
    [TestFixture]
    public class VerifyGlobalEventsForGenerateApiKeyApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _userName;

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

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);       

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId, _currentUserApiKey);
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
        public void VerifyGlobalEventsForGenerateApiKeyApiTest()
        {     
            // get global events
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _userName)
               .FirstOrDefault();
                        
            Assert.Multiple(() =>
            {
                Assert.True(actualGlobals.user_id.Equals(_userId),
                    $" actual user id : {actualGlobals.user_id}" +
                    $" expected user id: {_userId}");

                Assert.True(actualGlobals.user_full_name.Equals($"{_userName} {_userName}"),
                    $" actual erp user full name : {actualGlobals.user_full_name}" +
                    $" expected erp user full name: {_userName + _userName}");

                Assert.True(actualGlobals.erp_username.Equals(_userName),
                    $" actual erp user name : {actualGlobals.erp_username}" +
                    $" expected erp user name: {_userName}");

                Assert.True(actualGlobals.type.Equals("generate_api_key"),
                    $" actual type : {actualGlobals.type}" +
                    $" expected type: {"generate_api_key"}");

                Assert.True(actualGlobals.global.Equals(true),
                    $" actual type : {actualGlobals.global}" +
                    $" expected type: {true}");

                Assert.True(actualGlobals.action_made_by == _userName,
                    $" actual action made by : {actualGlobals.action_made_by}" +
                    $" expected action made by: {_userName}");

                Assert.True(actualGlobals.action_made_by_user_id.Equals(_userId),
                    $" actual action made by user id : {actualGlobals.action_made_by}" +
                    $" expected action made by user id: {_userId}");
            });
        }
    }
}