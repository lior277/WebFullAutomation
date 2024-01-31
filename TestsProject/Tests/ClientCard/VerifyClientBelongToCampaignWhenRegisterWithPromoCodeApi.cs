// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Banking.DepositsPage
{
    [TestFixture]
    public class VerifyClientBelongToCampaignWhenRegisterWithPromoCodeApi : TestSuitBase
    {
        #region Test Preparation
        private readonly IApplicationFactory _apiFactory = new ApplicationFactory();
        private readonly string _crmUrl = Config.appSettings.CrmUrl;
        private string _campaignName;
        private string _clientId;    

        [SetUp]
        public void SetUp()
        {
           BeforeTest();
            #region PreCondition

            // create first campaign and Affiliate
            var campaignCode = TextManipulation.RandomString();

            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl, campaignCode: campaignCode);

            _campaignName = campaignData.Keys.First();

            // register client with promo code
            var clientName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .RegisterClientWithPromoCode(_crmUrl,
                clientName: clientName, promoCode: campaignCode);
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
        [Description("https://airsoftltd.atlassian.net/browse/AIRV2-5683")]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyClientBelongToCampaignWhenRegisterWithPromoCodeApiTest()
        {
            var actualCampaignName = _apiFactory
                 .ChangeContext<IClientsApi>()
                 .GetClientRequest(_crmUrl, _clientId)
                 .GeneralResponse
                 .data
                 .FirstOrDefault()
                 .campaign;

            Assert.Multiple(() =>
            {
                Assert.True(actualCampaignName == _campaignName,
                    $" expected campaign id: {_campaignName}," +
                    $" actual campaign id: {actualCampaignName}");
            });
        }
    }
}