// Ignore Spelling: Api

using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Affiliate
{
    [TestFixture]
    public class VerifyAffiliateCanSeeOnlyHisCampaignsApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _campaignId;
        private string _affiliateId;
        private string _affiliateApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            // create first campaign and Affiliate
            var firstCampaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _campaignId = firstCampaignData.Values.First();
            _affiliateId = firstCampaignData.Values.Last();

            // get first Affiliate ApiKey
            _affiliateApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _affiliateId);

            // create second campaign and Affiliate
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);
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
        /// pre condition
        /// campaign and affiliate connected     
        /// verify affiliate can see only his campaign
        /// </summary>
        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyAffiliateCanSeeOnlyHisCampaignsApiTest()
        {
            var affiliateCampaigns = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .GetCampaignsByAffiliateRequest(_crmUrl, _affiliateApiKey);

            Assert.Multiple(() =>
            {
                Assert.IsTrue( affiliateCampaigns.Count() == 1,
                    $" expected num of campaigns:'1'" +
                    $" actual num of campaigns: { affiliateCampaigns.Count()}");

                Assert.IsTrue(affiliateCampaigns[0] == _campaignId,
                    $" expected campaign Id: { _campaignId }" +
                    $" 5actual campaign Id: { affiliateCampaigns[0]}");
            });
        }
    }
}