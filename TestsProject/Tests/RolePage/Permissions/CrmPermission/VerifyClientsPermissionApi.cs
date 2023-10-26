// Ignore Spelling: Crm Api

using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.CrmPermission
{
    [TestFixture]
    public class VerifyClientsPermissionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _userId;
        private string _currentUserApiKey;
        private string _userEmail;
        private string _clientEmail;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _roleName = TextManipulation.RandomString();

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            roleData.Name = _roleName;
            roleData.ErpPermissions.Remove("see_clients");
            roleData.ErpPermissions.Remove("all_attribution_rules");
            roleData.ErpPermissions.Add("show_only_unassigned_client");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, roleData);

            // create user
            var userName = TextManipulation.RandomString();
            _userEmail = userName + DataRep.EmailPrefix;

            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName, role: _roleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

         // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            // connect firs user to client
            _apiFactory
               .ChangeContext<IClientsApi>()
               .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
               new List<string> { clientId });
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete role by name
                _apiFactory
                     .ChangeContext<IUserApi>()
                     .PutEditUserRoleRequest(_crmUrl, _userId, DataRep.AdminRole);

                _apiFactory
                    .ChangeContext<IRolesApi>()
                    .DeleteRoleRequest(_crmUrl, _roleName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        public void VerifyClientsPermissionApiTest()
        {
            var attributionRoleName = "Automation";
            var expectedErrorMessage = "Method Not Allowed";
            var country = "austria";

            var actualGetCustomersWithErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientRequest(_crmUrl, _clientEmail, _currentUserApiKey, false)
                .Message;

            var actualCreateAttributionRuleErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .PostCreateAttributionRoleRequest(_crmUrl, attributionRoleName, 
                actualType: "country",
                countryNames: new string[] { country },
                ftdAgentIds: new string[] { _userId },
                apiKey: _currentUserApiKey, checkStatusCode: false);

            var actualShowOnlyUnassignedClientsErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientRequest(_crmUrl, _clientEmail, _currentUserApiKey, false)
                .Message;

            Assert.Multiple(() =>
            {
                Assert.True(actualGetCustomersWithErrorMessage == expectedErrorMessage,
                    $" expected Get Customers With Error Message : {expectedErrorMessage}" +
                    $" actual Get Customers With Error Message: {actualGetCustomersWithErrorMessage}");

                Assert.True(actualCreateAttributionRuleErrorMessage == expectedErrorMessage,
                    $" expected create Attribution Rule Error Message: {expectedErrorMessage}" +
                    $" actual create Attribution Rule Error Message: {actualCreateAttributionRuleErrorMessage}");

                Assert.True(actualShowOnlyUnassignedClientsErrorMessage == expectedErrorMessage,
                    $" expected Show Only Unassigned Clients Error Message: {expectedErrorMessage}" +
                    $" actual Show Only Unassigned Clients Error Message: {actualShowOnlyUnassignedClientsErrorMessage}");
            });
        }
    }
}