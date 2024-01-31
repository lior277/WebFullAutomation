// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.GlobalPermissions
{
    [TestFixture]
    public class VerifyCommentsFullHistoryPermissionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private GetRoleByNameResponse _roleData;
        private string _firstUserId;
        private string _secondUserId;
        private string _clientId;
        private string _firstUserApiKey;
        private string _secondUserApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _roleName = TextManipulation.RandomString();

            // create first user
            var userName = TextManipulation.RandomString();

            // create user
            _firstUserId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName, role: "agent");

            // create ApiKey
            _firstUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _firstUserId);

            // create client 
            var clientName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            var clientsIds = new List<string> { _clientId };

            // connect firs user to client
            _apiFactory
               .ChangeContext<IClientsApi>()
               .PatchMassAssignSaleAgentsRequest(_crmUrl, _firstUserId,
               clientsIds);

            // create comment
           _apiFactory
               .ChangeContext<ICommentsTabApi>()
               .PostCommentRequest(_crmUrl, _clientId, apiKey: _firstUserApiKey);

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, "agent");

            _roleData.Name = _roleName;            

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, _roleData);

            // create second user
            userName = TextManipulation.RandomString();

            // create user
            _secondUserId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName, role: _roleName);

            // connect second user to client
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _secondUserId,
                clientsIds);

            // create ApiKey
            _secondUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _secondUserId);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete role by name
                _apiFactory
                     .ChangeContext<IUserApi>()
                     .PutEditUserRoleRequest(_crmUrl, _secondUserId, DataRep.AdminRole);

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
        public void VerifyCommentsFullHistoryPermissionApiTest()
        {          
            var expectedClientIdWithPermmision = _clientId;
            string expectedClientIdWithoutPermmision = null;

            var actualClientIdWithPermmision  = _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .GetCommentsByClientIdRequest(_crmUrl, _clientId, _secondUserApiKey)
                .GeneralResponse
                .First()
                .UserId;

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, _roleName);

            // remove permission
            _roleData.ErpPermissions.Remove("comments_full_history");

            // edit role permission
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _roleData);

           var actualClientIdWithoutPermmision = _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .GetCommentsByClientIdRequest(_crmUrl, _clientId, _secondUserApiKey)
                .GeneralResponse
                .FirstOrDefault()?
                .UserId;

            Assert.Multiple(() =>
            {
                Assert.True(actualClientIdWithPermmision == expectedClientIdWithPermmision,
                    $" expected Client Id With Permission: {expectedClientIdWithPermmision}" +
                    $" actual Client Id With Permission: {actualClientIdWithPermmision}");

                Assert.True(actualClientIdWithoutPermmision == expectedClientIdWithoutPermmision,
                    $" expected Client Id Without Permission: {expectedClientIdWithoutPermmision}" +
                    $" actual Client Id Without Permission: {actualClientIdWithoutPermmision}");
            });
        }
    }
}