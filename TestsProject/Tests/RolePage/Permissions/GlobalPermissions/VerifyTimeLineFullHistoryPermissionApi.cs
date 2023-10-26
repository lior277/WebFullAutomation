// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
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
    public class VerifyTimeLineFullHistoryPermissionApi : TestSuitBase
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
               .PatchMassAssignSaleAgentsRequest(_crmUrl, _firstUserId, clientsIds);

            // create comment
            _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .PostCommentRequest(_crmUrl, _clientId, apiKey: _firstUserApiKey);

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            _roleData.Name = _roleName;

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, _roleData);

            userName = TextManipulation.RandomString();

            // create second user
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
        public void VerifyTimeLineFullHistoryPermissionAprTest()
        {
            var expectedTimeLineWithPermission = _firstUserId;
            var expectedTimeLineWithoutPermission = 0;

            // get TimeLine with  permission
            var actualTimeLineWithPermission = _apiFactory
                .ChangeContext<ITimeLineTabApi>()
                .GetTimelineRequest(_crmUrl, _clientId, _secondUserApiKey)
                .First()
                .erp_user_id;

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, _roleName);

            var erpPermmisions = _roleData.ErpPermissions;
            erpPermmisions.Remove("timeline_full_history");
            _roleData.ErpPermissions = erpPermmisions;
            _roleData.UsersOnly = true;

            // edit role permission
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _roleData);

            // get TimeLine with no permission
            var actualTimeLineWithoutPermission = _apiFactory
                .ChangeContext<ITimeLineTabApi>()
                .GetTimelineRequest(_crmUrl, _clientId, _secondUserApiKey);

            Assert.Multiple(() =>
            {
                Assert.True(actualTimeLineWithPermission == expectedTimeLineWithPermission,
                    $" expected TimeLine With Permission: {expectedTimeLineWithPermission}" +
                    $" actual TimeLine With Permission: {actualTimeLineWithPermission}");

                Assert.True(actualTimeLineWithoutPermission.Count == expectedTimeLineWithoutPermission,
                    $" expected TimeLine Without Permission: {expectedTimeLineWithoutPermission}" +
                    $" actual TimeLine Without Permission: {actualTimeLineWithoutPermission}");
            });
        }
    }
}