// Ignore Spelling: Api

using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
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
    public class VerifyNoPermissionsForCampaignsApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private GetRoleByNameResponse _roleData;
        private GetCampaignByIdResponse _campaignData;
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
            //_roleData.ErpPermissions.Remove("dashboard_campaigns");
            _roleData.ErpPermissions.Remove("campaign_full_update");
            _roleData.ErpPermissions.Remove("see_campaigns");

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
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _campaignId = campaignData.Values.First();
            _affiliateId = campaignData.Values.Last(); 

            // get campaign by id
            _campaignData = _apiFactory
                .ChangeContext<ICampaignsPageApi>()
                .GetCampaignByIdRequest(_crmUrl, _campaignId)
                .GeneralResponse;
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
            finally
            {
                AfterTest();
            }
        }
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyNoPermissionsForCampaignsApiTest()
        {
            var campaignName = TextManipulation.RandomString();
            var expectedErrorMessage = "Method Not Allowed";

            // get campaign by id
            var getCampaignByIdError = _apiFactory
                .ChangeContext<ICampaignsPageApi>()
                .GetCampaignByIdRequest(_crmUrl, _campaignId, _currentUserApiKey, false)
                .Message;

            // get campaigns
            var getCampaignsError = _apiFactory
                .ChangeContext<ICampaignsPageApi>()
                .GetCampaignsRequest(_crmUrl, _currentUserApiKey, false);

            // edit campaign
            var actualEditCampaignErrorMessage = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .PutCampaignByIdRequest(_crmUrl, _campaignData, 
                _affiliateId, _currentUserApiKey, false);

            // delete campaign
            var actualDeleteCampaignErrorMessage = _apiFactory
                .ChangeContext<ICampaignsPageApi>()
                .DeleteCampaignRequest(_crmUrl, _campaignData.Id, _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(getCampaignByIdError == expectedErrorMessage,
                    $" expected get Campaign by id  Error Message: {expectedErrorMessage}" +
                    $" actual get Campaign by id  Error Message: {getCampaignByIdError}");

                Assert.True(actualEditCampaignErrorMessage == expectedErrorMessage,
                   $" expected Edit Campaign Error Message: {expectedErrorMessage}" +
                   $" actual Edit Campaign Error Message: {actualEditCampaignErrorMessage}");

                Assert.True(actualDeleteCampaignErrorMessage == expectedErrorMessage,
                   $" expected Delete Campaign Error Message: {expectedErrorMessage}" +
                   $" actual Delete Campaign Error Message: {actualDeleteCampaignErrorMessage}");
            });
        }
    }
}