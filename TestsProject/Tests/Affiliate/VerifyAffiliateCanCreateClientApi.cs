// Ignore Spelling: Api

using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Affiliate
{
    [TestFixture]
    public class VerifyAffiliateCanCreateClientApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _affiliateApiKey;
        private string _campaignId;
        private string _affiliateId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            // create campaign and Affiliate
            var firstCampaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _affiliateId = firstCampaignData.Values.Last();
            _campaignId = firstCampaignData.Values.First();

            // get Affiliate ApiKey
            _affiliateApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _affiliateId);         
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

        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyAffiliateCanCreateClientApiTest()
        {
            // create client
            var clientName = TextManipulation.RandomString();
            var expectedClientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName, campaignId: _campaignId,
                apiKey: _affiliateApiKey);

            var actualClientEmail = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, clientId)
                .GeneralResponse
                .user
                .email;

            Assert.Multiple(() =>
            {
                Assert.True(actualClientEmail == expectedClientEmail,
                    $" actual client email: {actualClientEmail}" +
                    $" expected client email: {expectedClientEmail}");     
            });
        }
    }
}