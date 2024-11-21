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
    public class VerifyPixelWhenAffiliateHasAllPermissionsApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientName;
        private string _referanceId;
        private int _depositAmount = 100;
        private string _pixelClientId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _clientName = TextManipulation.RandomString();
            _referanceId = TextManipulation.RandomString();

            var registerPixelParams = new List<string> { "PHONE", "MAIL", "COUNTRY",
                "CODELANG", "ID", "PARAM@referance_id"};

            var depositPixelParams = new List<string> { "PHONE", "MAIL", "COUNTRY",
                "CODELANG", "ID", "PARAM@referance_id", "DEPOSIT"};

            // Compose Pixel params to string
            var registerPixel = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .ComposePixel(registerPixelParams);

            // Compose Pixel deposit params to string
            var depositPixel = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .ComposePixel(depositPixelParams);

            // create affiliate role with all permmisions
            _apiFactory
                .ChangeContext<ICreateEditRoleApi>()
                .PostAffiliateRoleWithAllPermissionsRequest(_crmUrl);

            // Create Affiliate And Campaign
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl,
                roleName: DataRep.AffiliateWithAllPermissionRole);

            var campaignId = campaignData.Values.First();
            var affiliateId = campaignData.Values.Last();

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
        /// update the campaign with pixel data for register and deposit
        /// Register Client with campaign and reference id as free parameter
        /// create deposit
        /// create deposit
        /// verify the pixel response for affiliate with all remission
        /// </summary>
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyPixelWhenAffiliateHasAllPermmisionsApiTest()
        {
            var expectedPhone = DataRep.UserDefaultPhone;
            var expectedCountry = "afghanistan";
            var expectedCodeLang = "en";
            var expectedClientEmail = _clientName + DataRep.EmailPrefix;
            var tesimUrl = DataRep.TesimUrl;

            // get pixel response data
            var actualPixelResponse = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .GetPixelRequest(tesimUrl, _referanceId, 3);

            Assert.Multiple(() =>
            {
                Assert.True(actualPixelResponse
                    .Where(p => p.Codelang == expectedCodeLang).Count() == 3,
                    $" expected Code Lang count equals to 3" +
                    $" actual Code Lang count equals to : " +
                    $" {actualPixelResponse.Where(p => p.Codelang == expectedCodeLang).Count()}");

                Assert.True(actualPixelResponse
                    .Where(p => p.Country == expectedCountry).Count() == 3,
                    $" expected Country count equals to 3" +
                    $" actual Country count equals to :" +
                    $" {actualPixelResponse.Where(p => p.Country == expectedCountry).Count()}");

                Assert.True(actualPixelResponse
                    .Where(p => p.Phone == expectedPhone).Count() == 3,
                    $" expected Phone count equals to 3" +
                    $" actual Phone count equals to :" +
                    $" {actualPixelResponse.Where(p => p.Phone == expectedPhone).Count()}");

                Assert.True(actualPixelResponse
                    .Where(p => p.Id == _pixelClientId).Count() == 3,
                    $" expected Id count equals to 3" +
                    $" actual Id count equals to : " +
                    $"{actualPixelResponse.Where(p => p.Id == _pixelClientId).Count()}");

                Assert.True(actualPixelResponse
                    .Where(p => p.Mail == expectedClientEmail).Count() == 3,
                    $" expected Mail count equals to 3" +
                    $" actual Mail count equals to :" +
                    $" {actualPixelResponse.Where(p => p.Mail == expectedClientEmail).Count()}");

                Assert.True(actualPixelResponse
                    .Where(p => p.Deposit == 0).Count() == 1,
                    $" expected Deposit count equals to 1" +
                    $" actual Deposit count equals to :" +
                    $" {actualPixelResponse.Where(p => p.Deposit == 0).Count()}");

                Assert.True(actualPixelResponse
                    .Where(p => p.Deposit == _depositAmount).Count() == 2,
                    $" expected Deposit count equals to 2" +
                    $" actual Deposit count equals to :" +
                    $" {actualPixelResponse.Where(p => p.Deposit == _depositAmount).Count()}");

                Assert.True(actualPixelResponse
                   .Where(p => p.ReferanceId == _referanceId).Count() == 3,
                   $" expected Reference Id equals to 3, Reference Id: {_referanceId}" +
                   $" actual Reference Id equals to :" +
                   $" {actualPixelResponse.Where(p => p.ReferanceId == _referanceId).Count()}," +
                   $"  Reference Id: {_referanceId}");
            });
        }
    }
}