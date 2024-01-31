// Ignore Spelling: Api

using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage
{
    [TestFixture]
    public class VerifyMassAssignComplianceStatusApi : TestSuitBase
    {
        #region Test Preparation
        public VerifyMassAssignComplianceStatusApi() { }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;   
        private List<string> _clientsIds;      
        private string _currentUserApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            // create user
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client
            var clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            _clientsIds = new List<string> { clientId };

            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, userId, _clientsIds);
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
        public void VerifyMassAssignComplianceStatusApiTest()
        {
            var expectedErrorMessageForBlockByAdmin = "Method Not Allowed";

            // verify admin can set active satus
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignComplianceStatusRequest(_crmUrl, "Active",
                _clientsIds, _currentUserApiKey);

            // verify admin can not set active status to block
            var actualErrorMessageForBlockByAdmin = _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignComplianceStatusRequest(_crmUrl, "Block",
                _clientsIds, _currentUserApiKey, false);

            // verify super admin can set active status to block
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignComplianceStatusRequest(_crmUrl, "Block",
                _clientsIds);

            Assert.True(actualErrorMessageForBlockByAdmin == expectedErrorMessageForBlockByAdmin,
                $" actual Error Message For Block By Admin: {actualErrorMessageForBlockByAdmin}" +
                $" expected Error Message For Block By Admin : {expectedErrorMessageForBlockByAdmin}");
        }
    }
}