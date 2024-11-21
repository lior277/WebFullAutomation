// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;

using AirSoftAutomationFramework.Internals.Helpers;
using NUnit.Framework;
using System.Linq;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using TestsProject.TestsInternals;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.DTOs;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using Microsoft.Graph;
using static AirSoftAutomationFramework.Objects.DTOs.GetClientCardResponse;
using static AirSoftAutomationFramework.Objects.DTOs.GetInformationTabResponse;

namespace TestsProject.Tests
{
    [TestFixture]
    public class VerifyUsersOnlyCantChooseUnassignedValueApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _userId;
        private string _depositId;
        private InformationTab _informationTabResponse;
        private string _currentUserApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName, 
                role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create client
            var clientName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // connect One User To One Client
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { _clientId });

            // create deposit 
            _depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, 1000);

            // get information tab
            _informationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab;
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
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-5371")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyUsersOnlyCantChooseUnassignedValueApiTest()
        {
            // Assign Deposit to unassigned user
            var actualUnAssignInFinance = _apiFactory
                .ChangeContext<IDepositsPageApi>()
                .PatchAssignDepositToUserRequest(_crmUrl,
                _depositId, "null", _currentUserApiKey, false);

            // connect One User To One Client
            var actualMassUnAssignUserToClient = _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, "null",
                new List<string> { _clientId }, _currentUserApiKey, false);

            // connect One User To One Client
            var actualUnAssignUserToClient = _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchAssignSaleAgentRequest(_crmUrl, "null",
                new List<string> { _clientId }, _currentUserApiKey, false);

            _informationTabResponse.sales_agent = "null";

            // assign client to unassigned sales agent
            var actualUnAssignUserInClientCard = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, _informationTabResponse,
                apiKey: _currentUserApiKey, false)
                .Message;

            Assert.Multiple(() =>
            {
                Assert.True(actualUnAssignInFinance == "Failed Dependency",
                    $" actual UnAssign In Finance: {actualUnAssignInFinance}" +
                    $" expected UnAssign In Finance: Failed Dependency");

                Assert.True(actualMassUnAssignUserToClient == "Not Acceptable",
                    $" actual Mass UnAssign User To Client: {actualMassUnAssignUserToClient}" +
                    $" expected Mass UnAssign User To Client: Not Acceptable");

                Assert.True(actualUnAssignUserToClient == "Not Acceptable",
                    $" actual UnAssign User To Client: {actualUnAssignUserToClient}" +
                    $" expected UnAssign User To Client: Not Acceptable");

                Assert.True(actualUnAssignUserInClientCard == "Failed Dependency",
                    $" actual UnAssign User In Client Card: {actualUnAssignUserInClientCard}" +
                    $" expected UnAssign User In Client Card: Failed Dependency");
            });
        }
    }
}