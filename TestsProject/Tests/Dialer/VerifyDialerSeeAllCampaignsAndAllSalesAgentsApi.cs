using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Dialer
{
    [TestFixture]
    public class VerifyDialerSeeAllCampaignsAndAllSalesAgentsApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _dialerApiKey;
        private string _mainOfficeCampaignId;
        private string _airsoftOfficeCampaignId;
        private string _userName;
        private string _userIdAirsoftOffice;
        private string _userIdMainOffice;
        private string _clientIdAirsoftOffice;
        private string _clientIdMainOffice;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            // create campaign for main office
            var campaignDataForMainOffice = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _mainOfficeCampaignId = campaignDataForMainOffice.Values.First();

            // create campaign for airsoft office
            var campaignDataForAirsoftOffice = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _airsoftOfficeCampaignId = campaignDataForAirsoftOffice.Values.First();

            // create dialer with single office
            var userName = TextManipulation.RandomString();

            var dialerId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName,
                 role: DataRep.AdminWithDialerRole);

            // get dialer ApiKey
            _dialerApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, dialerId);

            _userName = TextManipulation.RandomString();

            // create admin user that belong to different office then the client 
            userName = TextManipulation.RandomString();

            _userIdAirsoftOffice = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            // change the office of the user to airsoft
            // get airsoft User Data
            var airsoftUserData = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetActiveUsersRequest(_crmUrl)
                .userData
                .Where(p => p._id.Equals(_userIdAirsoftOffice))
                .FirstOrDefault();

            // get airsoft office id
            var airsoftOfficeId = _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .GetOfficesRequest(_crmUrl)
                .Where(p => p.city.Equals(DataRep.AirsoftOfficeName))
                .FirstOrDefault()
                ._id;

            airsoftUserData.office = airsoftOfficeId;

            // get admin airsoft user
            var user = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetUserByIdRequest(_crmUrl, _userIdAirsoftOffice)
                .GeneralResponse;

            user.user.office = airsoftOfficeId;

            // assign new office to user
            _apiFactory
                .ChangeContext<IUserApi>()
                .PutEditUserOfficeRequest(_crmUrl, user);

            // create client for user office airsoft
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientIdAirsoftOffice = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName, _airsoftOfficeCampaignId);

            var clientsIds = new List<string> { _clientIdAirsoftOffice };

            // connect admin airsoft user to Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userIdAirsoftOffice, clientsIds);

            userName = TextManipulation.RandomString();

            _userIdMainOffice = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            // create client for Main Office user
            clientName = TextManipulation.RandomString();

            _clientIdMainOffice = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName, _mainOfficeCampaignId);

            clientsIds.Clear();
            clientsIds.Add(_clientIdMainOffice);

            // connect admin user main office to Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userIdMainOffice, clientsIds);

            //Thread.Sleep(500);
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
        /// create user dialer single office
        /// create 2 campaigns
        /// create 2 users first belong to main office second to airsoft office
        /// create client with first campaign
        /// create client with second campaign
        /// connect the first client to first user
        /// connect the second client to second user
        /// verify dialer can see the first campaign
        /// verify dialer dont see the second campaign
        /// verify dialer can see the first user
        /// verify dialer dont see the second user
        /// verify dialer can see the first client
        /// verify dialer dont see the second client
        /// </summary>
        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyDialerSeeAllCampaignsAndAllSalesAgentsApiTest()
        {
            var dialerSeeCampaignsBelongToHim = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .GetCampaignsByDialerRequest(_crmUrl, _dialerApiKey)
                .Any(p => p._id.Equals(_mainOfficeCampaignId));

            // Affiliate connected to the Campaign can see the deposit
            var dialerDontSeeCampaignsBelongToHim = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .GetCampaignsByDialerRequest(_crmUrl, _dialerApiKey)
                .Any(p => p._id.Equals(_airsoftOfficeCampaignId));

            var dialerSeeUsersBelongToHim = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetAllSalesAgentsByDialerRequest(_crmUrl, _dialerApiKey)
                .Any(p => p._id.Equals(_userIdMainOffice));

            var dialerDontSeeUsersBelongToHim = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetAllSalesAgentsByDialerRequest(_crmUrl, _dialerApiKey)
                .Any(p => p._id.Equals(_userIdAirsoftOffice));

            var dialerSeeLeadsBelongToHim = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetAllLeadsRequest(_crmUrl, _clientIdMainOffice, _dialerApiKey)
                .Any(p => p.client_id.Equals(_clientIdMainOffice));

            var dialerDontSeeLeadsBelongToHim = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetAllLeadsRequest(_crmUrl, _clientIdMainOffice, _dialerApiKey)
                .Any(p => p.client_id.Equals(_clientIdAirsoftOffice));

            Assert.Multiple(() =>
            {
                Assert.True(dialerSeeCampaignsBelongToHim,
                    $" dialer see Campaign id : {_mainOfficeCampaignId}");

                Assert.False(dialerDontSeeCampaignsBelongToHim,
                    $" dialer dont see Campaign id : {_airsoftOfficeCampaignId}");

                Assert.True(dialerSeeUsersBelongToHim,
                    $" dialer see user id : {_userIdMainOffice}");

                Assert.False(dialerDontSeeUsersBelongToHim,
                    $" dialer dont see user id : {_userIdAirsoftOffice}");

                Assert.True(dialerSeeLeadsBelongToHim,
                    $" dialer see client id : {_clientIdMainOffice}");

                Assert.False(dialerDontSeeLeadsBelongToHim,
                    $" dialer dont see client id : {_clientIdAirsoftOffice}");
            });
        }
    }
}