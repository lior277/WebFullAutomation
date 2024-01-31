// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests
{
    [TestFixture]
    public class VerifyAcceptingNewLeadsHoursWhenCreateNewLeadApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userApiKey;
        private string _affiliateId;
        private string _userId;
        private string _campaignName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _campaignName = TextManipulation.RandomString();

            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            _userApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create user
            userName = TextManipulation.RandomString();

            _affiliateId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName, role: "affiliate");
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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyAcceptingNewLeadsHoursWhenCreateNewLeadApiTest()
        {
            var expectedNewLeadBlockMessage = "registration is open between 12:00 and 12:00GMT +0";

            // create campaign
            var campaignId = _apiFactory
                 .ChangeContext<ICampaignPageApi>()
                 .PostCreateCampaignRequest(_crmUrl, _campaignName,
                 _affiliateId, acceptingLeadsHoursActive: true);

            // create client
            var clientName = TextManipulation.RandomString();

            var actualNewLeadBlockMessage = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName,
                campaignId: campaignId, apiKey: _userApiKey, checkStatusCode: false);

            // get campaign
            var campaignData = _apiFactory
                .ChangeContext<ICampaignsPageApi>()
                .GetCampaignByIdRequest(_crmUrl, campaignId)
                .GeneralResponse;

            campaignData.AcceptingLeadsHoursFrom = "00:00";
            campaignData.AcceptingLeadsHoursTo = "23:59";

            // update campaign 
            _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .PutCampaignByIdRequest(_crmUrl, campaignData, _affiliateId);

            // create client when enable on campaign
            clientName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName, campaignId: campaignId,
                apiKey: _userApiKey);

            Assert.Multiple(() =>
            {
                Assert.True(actualNewLeadBlockMessage == expectedNewLeadBlockMessage,
                   $" expected  New Lead Block Message: {expectedNewLeadBlockMessage}" +
                   $" actual New Lead Block Message : {actualNewLeadBlockMessage}");
            });
        }
    }
}