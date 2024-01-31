// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage.AttributionRoles
{
    [TestFixture("israel")]
    [TestFixture("il")]
    public class VerifyBlockingCountryOnCampaignApi : TestSuitBase
    {
        #region Test Preparation
        private string _blockCountryName;

        public VerifyBlockingCountryOnCampaignApi(string blockCountryName) 
        {
            _blockCountryName = blockCountryName;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IDictionary<string, string> _campaignData;
        private string _affiliateApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            _campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl, new string[] { "israel" });

            var affiliateId = _campaignData.Values.Last();

            // create ApiKey
            _affiliateApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, affiliateId);         
            #endregion
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
        #endregion

        // based on bug https://airsoftltd.atlassian.net/browse/AIRV2-4569
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyBlockingCountryOnCampaignApiTest()
        {
            var expectedBlockCountryMessage = "blocked_country";
            var expectedMessage = "Your country is blocked." +
                " Contact the support team for more information";

            var campaignId = _campaignData.Values.First();

            // create client
            var clientName = TextManipulation.RandomString();

            var actualBlockCountryMessage = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName, campaignId: campaignId, 
                country: _blockCountryName, apiKey: _affiliateApiKey,
                checkStatusCode: false);

            Assert.Multiple(() =>
            {
                Assert.True(actualBlockCountryMessage.Contains(expectedBlockCountryMessage),
                    $" expected Block Country Message: {expectedBlockCountryMessage}" +
                    $" actual Block Country Message: {actualBlockCountryMessage}");

                Assert.True(actualBlockCountryMessage.Contains(expectedMessage),
                    $" expected  Message: {expectedMessage}" +
                    $" actual  Message: {actualBlockCountryMessage}");
            });
        }
    }
}