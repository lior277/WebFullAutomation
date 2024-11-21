// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Affiliate
{
    [NonParallelizable]
    public class VerifyPixelWhenAffiliateHasNoPermissionsApi : TestSuitBase
    {
        #region Test Preparation
        public VerifyPixelWhenAffiliateHasNoPermissionsApi() { }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IDictionary<string, string> _campaignData;
        private string _pixelClientId;
        private string _clientName;
        private string _referanceId;
        private int _depositAmount = 100;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _clientName = TextManipulation.RandomString();
            _referanceId = TextManipulation.RandomString();

            #region PreCondition            
            var registerPixelParams = new List<string> { "COUNTRY",
                "CODELANG", "ID", "PARAM@referance_id"};

            var depositPixelParams = new List<string> { "COUNTRY",
                "CODELANG", "ID", "PARAM@referance_id"};

            // Compose Pixel params to string
            var registerPixel = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .ComposePixel(registerPixelParams);

            // Compose Pixel deposit params to string
            var depositPixel = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .ComposePixel(depositPixelParams);

            // create affiliate role with no permmisions
            _apiFactory
                .ChangeContext<ICreateEditRoleApi>()
                .PostAffiliateRoleWithNoPermissionsRequest(_crmUrl);

            // Create Affiliate And Campaign
            _campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl,
                roleName: DataRep.AffiliateWithNoPermissionRole);

            var campaignId = _campaignData.Values.First();
            var affiliateId = _campaignData.Values.Last();

             // get campaign
            var getCampaignData = _apiFactory
                 .ChangeContext<ICampaignsPageApi>()
                 .GetCampaignByIdRequest(_crmUrl, campaignId)
                 .GeneralResponse;

            getCampaignData.AcceptedDepositPixel = depositPixel;
            getCampaignData.RegisterPixel = registerPixel;

            // update campaign with pixel data
            _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .PutCampaignByIdRequest(_crmUrl, getCampaignData, affiliateId);

            var urlParams = $"campaign={campaignId}&referance_id={_referanceId}";

            // create client with pixel
            _pixelClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .RegisterClientWithCampaign(_crmUrl,
                clientName: _clientName, campaignId: campaignId,
                referanceId: _referanceId, actualUrlParams: urlParams);

            // create first deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _pixelClientId, _depositAmount);

            // create second deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _pixelClientId, _depositAmount);
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

        /// <summary>
        /// create campaign and affiliate
        /// update the campaign with pixel data for resister and deposit
        /// Register Client with campaign and reference id as free parameter
        /// create deposit
        /// create deposit
        /// verify the pixel response for affiliate with no permission
        /// </summary>
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyPixelWhenAffiliateHasNoPermissionsApiTest()
        {
            string expectedPhone = null;
            var expectedCountry = "afghanistan";
            var expectedCodeLang = "en";
            var expectedClientEmail = _clientName + DataRep.EmailPrefix;
            var tesimUrl = DataRep.TesimUrl;

            // get pixel response data
            var actualPixelResponse = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .GetPixelRequest(tesimUrl, _referanceId, 2);

            Assert.Multiple(() =>
            {
                Assert.True(actualPixelResponse
                    .Where(p => p.Codelang == expectedCodeLang).Count() == 2,
                    $" expected Codelang count euals to 2" +
                    $" actual Codelang count euals to : " +
                    $" {actualPixelResponse.Where(p => p.Codelang == expectedCodeLang).Count()}");

                Assert.True(actualPixelResponse
                    .Where(p => p.Country == expectedCountry).Count() == 2,
                    $" expected Country count euals to 2" +
                    $" actual Country count euals to :" +
                    $" {actualPixelResponse.Where(p => p.Country == expectedCountry).Count()}");

                Assert.True(actualPixelResponse
                    .Where(p => p.Phone == expectedPhone).Count() == 2,
                    $" expected Phone count euals to null" +
                    $" actual Phone count euals to :" +
                    $" {actualPixelResponse.Where(p => p.Phone == null).Count()}");

                Assert.True(actualPixelResponse
                    .Where(p => p.Id == _pixelClientId).Count() == 2,
                    $" expected Id count euals to 2" +
                    $" actual Id count euals to : " +
                    $"{actualPixelResponse.Where(p => p.Id == _pixelClientId).Count()}");

                Assert.True(actualPixelResponse
                    .Where(p => p.Mail == null).Count() == 2,
                    $" expected Mail count euals to 2" +
                    $" actual Mail count euals to :" +
                    $" {actualPixelResponse.Where(p => p.Mail == null).Count()}");

                Assert.True(actualPixelResponse
                    .Where(p => p.Deposit == 0).Count() == 2,
                    $" expected Deposit count euals to 2" +
                    $" actual Deposit count euals to :" +
                    $" {actualPixelResponse.Where(p => p.Deposit == 0).Count()}");

                Assert.True(actualPixelResponse
                    .Where(p => p.ReferanceId == _referanceId).Count() == 2,
                    $" expected Referance Id euals to 2, Referance Id: {_referanceId}" +
                    $" actual Referance Id euals to :" +
                    $" {actualPixelResponse.Where(p => p.ReferanceId == _referanceId).Count()}" +
                    $", Referance Id: {_referanceId}");
            });
        }
    }
}