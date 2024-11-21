// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents.SavingAccount
{
    [TestFixture]
    public class VerifyGlobalEventForDeleteClientApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _clientId;
        private string _userName;

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

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            _apiFactory
                .ChangeContext<IClientsApi>()
                .DeleteClientRequest(_crmUrl, _clientId, _currentUserApiKey);
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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyGlobalEventForDeleteClientApiTest()
        {
            var expectedType = "delete_leads_customers";

            // get global events
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl,
               _userName, _currentUserApiKey)
               .FirstOrDefault();   

            Assert.Multiple(() =>
            {
                Assert.True(actualGlobals.action_made_by.Equals(_userName),
                    $" actual action_made_by : {actualGlobals.action_made_by}" +
                    $" expected action_made_by: {_userName}");

                Assert.True(actualGlobals.user_id.Equals(_clientId),
                    $" actual  client Id : {actualGlobals.user_id}" +
                    $" expected client Id: {_clientId}");

                Assert.True(actualGlobals.type.Equals(expectedType),
                    $" actual type : {actualGlobals.user_id}" +
                    $" expected type: {expectedType}");
            });
        }
    }
}