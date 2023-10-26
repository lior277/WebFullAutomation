// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using Microsoft.Graph.Models;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture]
    public class VerifyMakeCallCapabilityClientCardApi : TestSuitBase
    {
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _pbxName; 
        private string _currentUserApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition   

            // create user
            var userName = TextManipulation.RandomString();
            _pbxName = TextManipulation.RandomString();

            // create office
            var officeName = TextManipulation.RandomString();
            
            var officeData = _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .PostCreateOfficeRequest(_crmUrl, officeName, _pbxName)
                .GetOfficesByName(_crmUrl, officeName);

            // create user
            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName,
                pbxName: _pbxName, officeData: officeData);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            //// get user
            //var user = _apiFactory
            //    .ChangeContext<IUsersApi>()
            //    .GetUserByIdRequest(_crmUrl, userId)
            //    .GeneralResponse;

            //user.user.office = officeId;

            //// update user with new office
            //_apiFactory
            //   .ChangeContext<IUserApi>()
            //   .PutEditUserOfficeRequest(_crmUrl, user);

            // create client 
            var clientName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            var clientsIds = new List<string> { _clientId };

            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, userId, clientsIds);
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
        // based on ticket https://airsoftltd.atlassian.net/browse/AIRV2-4605
        // in settings office details add pbx and pbx type - handle in one time setup
        // create user with phone number extension 
        // create client 
        // verify Internal Server Error message for both phones
        [Test]       
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyMakeCallCapabilityClientCardApiTest()
        {
            var actualErrorFirstNumber = _apiFactory
                .ChangeContext<IClientCardApi>()
                .PostMakeCallRequest(_crmUrl, _clientId, _pbxName, "phone",
                _currentUserApiKey, false);

            var actualErrorSecondNumber = _apiFactory
                .ChangeContext<IClientCardApi>()
                .PostMakeCallRequest(_crmUrl, _clientId, _pbxName,
                "phone_2", _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualErrorFirstNumber.Contains("phone"),
                    $" expected Error First Number: phone" +
                    $" actual Error First Number: {actualErrorFirstNumber}");

                Assert.True(actualErrorSecondNumber.Contains("phone"),
                    $" expected Error second Number: phone" +
                    $" actualError second Number: {actualErrorSecondNumber}");             
            });
        }
    }
}