using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.UsersPage
{
    [TestFixture]
    public class VerifyDeleteUser : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientEmail;
        private string _clientName;
        private string _currentUserApiKey;
        private GetUserResponse _getUserResponse;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var userName = TextManipulation.RandomString();

           var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            _getUserResponse = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetUserByIdRequest(_crmUrl, userId)
                .GeneralResponse;

            // user ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            _clientName = TextManipulation.RandomString();
            _clientEmail = _clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                apiKey: _currentUserApiKey);

            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, userId,
                new List<string> { clientId }, apiKey: _currentUserApiKey);
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
        [Description("Based on jira : https://airsoftltd.atlassian.net/browse/AIRV2-5059")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyDeleteUserTest()
        {
            // delete user
            _apiFactory
                .ChangeContext<IUserApi>()
                .PatchDeleteOrRestoreUserRequest(_crmUrl, _getUserResponse.user._id);

            _clientName = TextManipulation.RandomString();

            var actualCreateClientErrorMessage = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                apiKey: _currentUserApiKey, checkStatusCode: false);

            var actualClientSalesAgent = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientRequest(_crmUrl, _clientEmail)
                .GeneralResponse
                .data
                .FirstOrDefault()
                .sales_agent;

          Assert.Multiple(() =>
            {
                Assert.True(actualCreateClientErrorMessage == "Not Found",
                    $" expected Create Client Error Message: Not Found" +
                    $" actual Create Client Error Message: Not Found");

                Assert.True(actualClientSalesAgent == null,
                    $" expected Client Sales Agent: null, " +
                    $" actual Client Sales Agent: {actualClientSalesAgent}");
            });
        }
    }
}
