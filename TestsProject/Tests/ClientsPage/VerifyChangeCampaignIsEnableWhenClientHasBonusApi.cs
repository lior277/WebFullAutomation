// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
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
    public class VerifyChangeCampaignIsEnableWhenClientHasBonusApi : TestSuitBase
    {
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _firstCampaignId;
        private string _secondCampaignId;
        private string _firstCampaignName;
        private string _secondCampaignName;
        private string _clientEmail;
        private string _userApiKey;
        private string _clientId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var bonusAmount = 10;

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName);

            _userApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            #region create first campaign
            // create campaign
            var  campaignData = _apiFactory
                 .ChangeContext<ISharedStepsGenerator>()
                 .CreateAffiliateAndCampaignApi(_crmUrl);
            #endregion

            _firstCampaignId = campaignData.Values.First();
            _firstCampaignName = campaignData.Keys.First();
            var affiliateId = campaignData.Values.Last();

            _secondCampaignName = $"second{_firstCampaignName}";

            #region create second campaign with the same affiliate
            // create second campaign with the same affiliate
            _secondCampaignId = _apiFactory
                 .ChangeContext<ICampaignPageApi>()
                 .PostCreateCampaignRequest(_crmUrl, _secondCampaignName,
                 affiliateId);
            #endregion

            #region create client With the first Campaign
            // create client
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName,
                campaignId: _firstCampaignId,apiKey: _userApiKey);
            #endregion

            // create bonus 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, bonusAmount,
                apiKey: _userApiKey);
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

        [Test]
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4769")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyChangeCampaignIsEnableWhenClientHasBonusApiTest()
        {
            var expectedCampaignName = _secondCampaignName;

            // admin can change the campaign of client with bonus, 
            var actualClientId = _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignCampaign(_crmUrl, new List<string> { _clientId },
                _secondCampaignId);

            var actualCampaignName = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientRequest(_crmUrl, _clientEmail)
                .GeneralResponse
                .data
                .FirstOrDefault()
                .campaign;

            Assert.Multiple(() =>
            {
                Assert.True(actualCampaignName == expectedCampaignName,
                   $" expected Campaign Name : {expectedCampaignName}" +
                   $" actual Campaign Name : {actualCampaignName}");
            });
        }
    }
}