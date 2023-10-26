// Ignore Spelling: Api

using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage
{
    [TestFixture]
    public class VerifyNewTradingGroupWhenAssignNewValueInClientsApi : TestSuitBase
    {
        #region Test Preparation

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userApiKey;
        private string _clientId;
        private string _firstGroupId;
        private string _scondGroupId;
        private string _platformName;
        private string _secondGroupName;    

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var firstGroupName = "Default";
            _secondGroupName = "Negative 1";

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName);

            _userApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client
            // create client 
            var clientName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _userApiKey);

            // get platform
            _platformName = _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .GetGlobalSettingsRequest(_crmUrl)
                .platform;

            // get first group id by name
            _firstGroupId = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p.name.Equals(firstGroupName))
                .FirstOrDefault()
                ._id;

            // get first group id by name
            _scondGroupId = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p.name.Equals(_secondGroupName))
                .FirstOrDefault()
                ._id;

            #endregion
        }
        #endregion

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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyNewTradingGroupWhenAssignNewValueInClientsApiTest()
        {
            var expectedClientId = _clientId;

            //admin cant change the campaign of client with chargeback, 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchAssignTreadingGroupRequest(_crmUrl, _clientId,
                _scondGroupId, _firstGroupId, _platformName);

           var actualGroupName = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientRequest(_crmUrl, _clientId)
                .GeneralResponse
                .data
                .FirstOrDefault()
                .group;

            Assert.Multiple(() =>
            {
                Assert.True(actualGroupName == _secondGroupName,
                   $" expected Group Name : {_secondGroupName}" +
                   $" actual Group Name : {actualGroupName}");
            });
        }
    }
}