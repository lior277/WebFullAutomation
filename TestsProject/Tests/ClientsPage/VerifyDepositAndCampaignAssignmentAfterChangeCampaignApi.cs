// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage
{
    [TestFixture]
    public class VerifyDepositAndCampaignAssignmentAfterChangeCampaignApi : TestSuitBase
    {
        #region Test Preparation
        public VerifyDepositAndCampaignAssignmentAfterChangeCampaignApi() { }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _firstCampaignId;
        private string _secondCampaignId;
        private string _firstCampaignName;
        private string _secondCampaignName;
        private string _clientEmail;
        private string _currentUserApiKey;
        private string _affiliateApiKey;
        private string _clientId;
        private string _depositId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var depositAmount = 10;

            // create user
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName,
               role: DataRep.AdminWithUsersOnlyRoleName);

            #region create first campaign
            // create campaign
            var  campaignData = _apiFactory
                 .ChangeContext<ISharedStepsGenerator>()
                 .CreateAffiliateAndCampaignApi(_crmUrl);
            #endregion

            _firstCampaignId = campaignData.Values.First();
            _firstCampaignName = campaignData.Keys.First();
            var affiliateId = campaignData.Values.Last();

            _affiliateApiKey = _apiFactory
              .ChangeContext<IUserApi>()
              .PostCreateApiKeyRequest(_crmUrl, affiliateId);

            _secondCampaignName = $"second{_firstCampaignName}";

            #region create second campaign with the same affiliate
            // create second campaign with the same affiliate
            _secondCampaignId = _apiFactory
                 .ChangeContext<ICampaignPageApi>()
                 .PostCreateCampaignRequest(_crmUrl, _secondCampaignName,
                 affiliateId);
            #endregion

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);
            #endregion

            #region create client With the first Campaign
            // create client
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName,
                campaignId: _firstCampaignId, apiKey: _currentUserApiKey);
            #endregion

            // create deposit 
            _depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, depositAmount,
                apiKey: _currentUserApiKey);

            // super admin can't change the client campaign if the client has deposit
            // and the second campaign has another affiliate
            _apiFactory
              .ChangeContext<IClientsApi>()
              .PatchMassAssignCampaign(_crmUrl, new List<string> { _clientId },
              _secondCampaignId);      
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

        /// <summary>
        /// create user admin with users only
        /// create client
        /// create deposit for client
        /// connect the client to the first campaign
        /// admin change campaign to the second
        /// api doc get transaction  -  api key of the affiliate and the first campaign id
        /// verify the deposit belong to the first canpaingn
        /// verify the client belong to the second campaign - api doc client registration- api
        /// key of the affiliate and the second campain id
        /// </summary>
        [Test]
        [Description("based on jira: https://airsoftltd.atlassian.net/browse/AIRV2-4713")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyDepositAndCampaignAssignmentAfterChangeCampaignApiTest()
        {
            var expectedCampaignName = _secondCampaignName;
            var expectedSuperChangeCampain = 1;
            var expectedAdminChangeCampain = _clientId;
            var expectedSuperUnassignCampain = 1;

            // super admin can change the campaign(with the same affiliate)
            // of client with deposit, 
            var actualSuperChangeCampain = _apiFactory
              .ChangeContext<IClientsApi>()
              .PatchMassAssignCampaign(_crmUrl, new List<string> { _clientId },
              _secondCampaignId)
              .GeneralResponse
              .NModified;

            // admin can't change the campaign of client with or without deposit
            var actualAdminChangeCampaign = _apiFactory
              .ChangeContext<IClientsApi>()
              .PatchMassAssignCampaign(_crmUrl, new List<string> { _clientId },
              _firstCampaignId, _currentUserApiKey)
              .GeneralResponse
              .FailedToMassAssignToCampaign
              .First();

            // super admin can unassign client with deposit from campaign
            var actualSuperUnassignCampaign = _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignCampaign(_crmUrl, new List<string> { _clientId },
                _secondCampaignId)
                .GeneralResponse
                .NModified;

            // Affiliate connected to the Campaign can see the deposit
            var actualDepositId = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .GetDepositsByCampaignIdRequest(_crmUrl, _firstCampaignId, _affiliateApiKey)
                .GeneralResponse
                .First()
                .Id;

            var actualCampaignName = _apiFactory
               .ChangeContext<IClientsApi>()
               .GetClientRequest(_crmUrl, _clientEmail)
               .GeneralResponse
               .data
               .FirstOrDefault()
               .campaign;

            Assert.Multiple(() =>
            {
                Assert.True(actualSuperChangeCampain == expectedSuperChangeCampain,
                   $" expected Super Change Campaign : {expectedSuperChangeCampain}" +
                   $" actual Super Change Campaign : {actualSuperChangeCampain}");

                Assert.True(actualAdminChangeCampaign == expectedAdminChangeCampain,
                   $" expected Admin Change Campaign : {expectedAdminChangeCampain}" +
                   $" actual Admin Change Campaign: {actualAdminChangeCampaign}");

                Assert.True(actualSuperUnassignCampaign == expectedSuperUnassignCampain,
                   $" expected Super Unassign Campaign : {expectedSuperUnassignCampain}" +
                   $" actual Super Unassign Campaign : {actualSuperUnassignCampaign}");

                Assert.True(actualDepositId == _depositId,
                    $" actual Deposit id: {actualDepositId}" +
                    $" expected Deposit id : {_depositId}");

                Assert.True(actualCampaignName == expectedCampaignName,
                    $" actual Campaign Name: {expectedCampaignName}" +
                    $" expected Campaign Name : {actualCampaignName}");
            });
        }
    }
}