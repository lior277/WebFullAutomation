// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.GlobalPermissions
{
    [TestFixture]
    public class VerifyEventsPermissionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _userId;
        private string _clientId;
        private string _campaignId;
        private string _cryptoGroupId;
        private string _bannerId;
        private string _currentUserApiKey;
        private string _userEmail;

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
            roleData.ErpPermissions.Remove("events");

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

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            // create campaign
            var campaignData = _apiFactory
                 .ChangeContext<ISharedStepsGenerator>()
                 .CreateAffiliateAndCampaignApi(_crmUrl);

            _campaignId = campaignData.Values.First();

            // get group by id
            _cryptoGroupId = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p.name == "Default")
                .FirstOrDefault()
                ._id;

            _bannerId = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .CreateBannerPipe(_crmUrl, DataRep.AutomationBannerName);
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
        public void VerifyEventsPermissionApiTest()
        {          
            var expectedErrorMessage = "Method Not Allowed";

            // create comment 
           var actualAddCommentErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .PostMassAssignCommentRequest(_crmUrl, new string[]
                { _clientId }, "Automation Comment",
                _currentUserApiKey, false);

            // delete comment 
            var actualDeleteCommentErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .DeleteMassAssignCommentsRequest(_crmUrl,
                new string[] { _clientId }, _currentUserApiKey, false);

            // Assign Sale Agents
            var actualAssignSaleAgentsErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { _clientId }, _currentUserApiKey, false);

            // Assign Random Sale Agents
            var actualAssignRandomSaleAgentsErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignRandomSaleAgentsRequest(_crmUrl, _userId,
                new string[] { _clientId }, _currentUserApiKey, false);

            // Assign Campaign
            var actualAssignCampaignErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignCampaign(_crmUrl,
                new List<string> { _clientId }, _campaignId, _currentUserApiKey, false)
                .Message;

            // Assign status
            var actualAssignStatusErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSalesStatusRequest(_crmUrl, "New",
                new List<string> { _clientId }, _currentUserApiKey, false);

            // Assign Compliance Status
            var actualAssignComplianceStatusErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignComplianceStatusRequest(_crmUrl, "Active",
                new List<string> { _clientId }, _currentUserApiKey, false);

            // Delete Clients
            var actualDeleteClientsErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .MassDeleteClientsRequest(_crmUrl,
                new string[] { _clientId }, _currentUserApiKey, false);

            // Assign Trading Group
            var actualAssignTradingGroupErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .MassPatchAssignTradingGroupRequest(_crmUrl, _cryptoGroupId,
                new string[] { _clientId }, _currentUserApiKey, false);

            // Assign Banner Message
            var actualAssignBannerMessageErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignBannerMessageRequest(_crmUrl,
                _bannerId, new string[] { _clientId }, _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualAddCommentErrorMessage == expectedErrorMessage,
                    $" expected Add Comment Error Message: {expectedErrorMessage}" +
                    $" actual Add Comment Error Message: {actualAddCommentErrorMessage}");

                Assert.True(actualDeleteCommentErrorMessage == expectedErrorMessage,
                    $" expected Delete Comment Error Message: {expectedErrorMessage}" +
                    $" actual Delete Comment Error Message: {actualDeleteCommentErrorMessage}");

                Assert.True(actualAssignSaleAgentsErrorMessage == expectedErrorMessage,
                    $" expected Assign Sale Agents Error Message: {expectedErrorMessage}" +
                    $" actual Assign Sale Agents Error Message: {actualAssignSaleAgentsErrorMessage}");

                Assert.True(actualAssignRandomSaleAgentsErrorMessage == expectedErrorMessage,
                    $" expected Assign Random Sale Agents ErrorMessage Error Message: {expectedErrorMessage}" +
                    $" actual Assign Random Sale Agents  Error Message: {actualAssignRandomSaleAgentsErrorMessage}");

                Assert.True(actualAssignCampaignErrorMessage == expectedErrorMessage,
                    $" expected Assign Campaign Error Message: {expectedErrorMessage}" +
                    $" actual AssignCampaign Error Message: {actualAssignCampaignErrorMessage}");

                Assert.True(actualAssignStatusErrorMessage == expectedErrorMessage,
                    $" expected Assign Status Error Message: {expectedErrorMessage}" +
                    $" actual Assign Status Error Message: {actualAssignStatusErrorMessage}");

                Assert.True(actualAssignComplianceStatusErrorMessage == expectedErrorMessage,
                    $" expected Assign Compliance Status Error Message: {expectedErrorMessage}" +
                    $" actual Assign Compliance Status Error Message: {actualAssignComplianceStatusErrorMessage}");

                Assert.True(actualDeleteClientsErrorMessage == expectedErrorMessage,
                    $" expected Delete Clients Error Message: {expectedErrorMessage}" +
                    $" actual Delete Clients Error Message: {actualDeleteClientsErrorMessage}");

                Assert.True(actualAssignTradingGroupErrorMessage == expectedErrorMessage,
                    $" expected Assign Trading Group Error Message: {expectedErrorMessage}" +
                    $" actual Assign Trading Group Error Message: {actualAssignTradingGroupErrorMessage}");

                Assert.True(actualAssignBannerMessageErrorMessage == expectedErrorMessage,
                    $" expected Assign Banner Message Error Message: {expectedErrorMessage}" +
                    $" actual Assign Banner Message Error Message: {actualAssignBannerMessageErrorMessage}");
            });
        }
    }
}