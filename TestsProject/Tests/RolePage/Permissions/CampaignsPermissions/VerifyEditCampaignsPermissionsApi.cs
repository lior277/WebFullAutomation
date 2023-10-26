// Ignore Spelling: Api

using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.CampaignsPermissions
{
    [TestFixture]
    public class VerifyEditCampaignsPermissionsApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private GetRoleByNameResponse _roleData;
        private string _roleName;
        private string _userId;
        private string _campaignId;
        private string _affiliateId;
        private string _userName;
        private string _currentUserApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _roleName = TextManipulation.RandomString();

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            _roleData.Name = _roleName;
            _roleData.ErpPermissions.Remove("all_campaigns");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, _roleData);

            // create user
            _userName = TextManipulation.RandomString();

            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName, role: _roleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create campaign and Affiliate
            var firstCampaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _affiliateId = firstCampaignData.Values.Last();
            _campaignId = firstCampaignData.Values.First();
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
        public void VerifyEditCampaignsPermissionsApiTest()
        {
            var campaignName = TextManipulation.RandomString();
            var expectedErrorMessage = "Method Not Allowed";

            var actualCreateCampaignErrorMessage = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .PostCreateCampaignRequest(_crmUrl, campaignName, _userId,
                apiKey: _currentUserApiKey, checkStatusCode: false);

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, _roleName);

            _roleData.ErpPermissions.Add("all_campaigns");
            _roleData.ErpPermissions.Remove("campaign_full_update");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _roleData);

            // get campaign by id
            var campaignDada = _apiFactory
                .ChangeContext<ICampaignsPageApi>()
                .GetCampaignByIdRequest(_crmUrl, _campaignId)
                .GeneralResponse;

            // edit campaign
            var actualEditCampaignErrorMessage = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .PutCampaignByIdRequest(_crmUrl, campaignDada,
                _affiliateId, _currentUserApiKey, false);

            // delete campaign
            var actualDeleteCampaignErrorMessage = _apiFactory
                .ChangeContext<ICampaignsPageApi>()
                .DeleteCampaignRequest(_crmUrl, campaignDada.Id, _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualCreateCampaignErrorMessage == expectedErrorMessage,
                    $" expected Create Campaign Error Message: {expectedErrorMessage}" +
                    $" actual Create Campaign Error Message: {actualCreateCampaignErrorMessage}");

                Assert.True(actualEditCampaignErrorMessage.Contains("erp_user_id is not allowed"),
                   $" expected Edit Campaign Error Message: Contains erp_user_id is not allowed" +
                   $" actual Edit Campaign Error Message: {actualEditCampaignErrorMessage}");

                Assert.True(actualDeleteCampaignErrorMessage == expectedErrorMessage,
                   $" expected Delete Campaign Error Message: {expectedErrorMessage}" +
                   $" actual Delete Campaign Error Message: {actualDeleteCampaignErrorMessage}");
            });
        }
    }
}