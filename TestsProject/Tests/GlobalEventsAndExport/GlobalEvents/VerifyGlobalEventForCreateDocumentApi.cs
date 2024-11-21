// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents
{
    [TestFixture]
    public class VerifyGlobalEventForCreateDocumentApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _userName;
        private string _cryptoGroupName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _cryptoGroupName = TextManipulation.RandomString();

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
        public void VerifyGlobalEventForCreateDocumentApiTest()
        {
            var expectedActionMadeByUser = _userName;
            var expectedActionMadeByUserId = _userId;
            //var expectedFromBrowser = "Chrome";
            var expectedGlobal = false;
            var expectedType = "create_document";
            var expectedUserId = _userId;
            var documentParams = new List<string> { "SELLER_FIRST_NAME" };

            var emailsParams = new Dictionary<string, string> {
                { "type", "custom" }, { "language", "en" }, { "name", "Automation" }};

            // create document body
            var documentBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ComposeEmailBody(documentParams);

            // create document
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .CreateDocumentPipe(_crmUrl, emailsParams,
                documentBody, _currentUserApiKey);

            // get global events
            var actualGlobalEvents = _apiFactory
                .ChangeContext<IGlobalEventsApi>()
                .GetGlobalEventsByUserRequest(_crmUrl, _userName, _currentUserApiKey)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualGlobalEvents.type == expectedType,
                    $" actual type : {actualGlobalEvents.type}" +
                    $" expected type: {expectedType}");

                Assert.True(actualGlobalEvents.global == expectedGlobal,
                    $" actual expected Global : {actualGlobalEvents.global}" +
                    $" expected expected Global: {expectedGlobal}");

                Assert.True(actualGlobalEvents.action_made_by == expectedActionMadeByUser,
                    $" actual user name : {actualGlobalEvents.action_made_by}" +
                    $" expected user name: {expectedActionMadeByUser}");

                Assert.True(actualGlobalEvents.action_made_by_user_id == _userId,
                    $" actual action made by user id : {actualGlobalEvents.action_made_by_user_id}" +
                    $" expected action made by user id: {_userId}");

                Assert.True(actualGlobalEvents.user_id == _userId,
                    $" actual user Id : {actualGlobalEvents.user_id}" +
                    $" expected user Id: {_userId}");
            });
        }
    }
}