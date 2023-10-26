// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyCampaignCapForCreateClientApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _affiliateApiKey;
        private string _campaignId;
        private string _campaignName;
        private string _clientName;
        private int _leadsLimit = 1;
        private string _timeFrame = "Daily";
        private string _affiliateEmail;
        private string _affiliateName;
        private string _firstCountryNameForLimitation = "austria";
        private string _secondCountryNameForLimitation = "bahrain";
        private string _testimUrl = DataRep.TesimUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create first campaign and Affiliate
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _campaignId = campaignData.Values.First();
            var affiliateId = campaignData.Values.Last();

            // get first Affiliate ApiKey
            _affiliateApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, affiliateId);

            // get affiliate email
            var email = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetUserByIdRequest(_crmUrl, affiliateId)
                .GeneralResponse
                .user
                .email;

            // get Campaign Data
            var getCampaignData = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .GetCampaignByIdRequest(_crmUrl, _campaignId);

            _campaignName = getCampaignData.Name;

            #region add limitation to the campaign
            var limitation = new List<Limitation>();

            limitation.Add(new Limitation()
            {
                country = _firstCountryNameForLimitation,
                timeframe = "Daily",
                leads_num = _leadsLimit
            });

            limitation.Add(new Limitation()
            {
                country = _secondCountryNameForLimitation,
                timeframe = "Daily",
                leads_num = _leadsLimit
            });

            _affiliateName = email.Split('@').First();
            _affiliateEmail = email.Split('@').First() + DataRep.TestimEmailPrefix;
            getCampaignData.cap.email = _affiliateEmail;
            getCampaignData.cap.limitations = limitation;
            #endregion

            // update Campaign 
            _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .PutCampaignByIdRequest(_crmUrl, getCampaignData, affiliateId);

            //create client
            _clientName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, _clientName,
                _campaignId, country: _firstCountryNameForLimitation, apiKey: _affiliateApiKey);

            _clientName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, _clientName,
                _campaignId, country: _secondCountryNameForLimitation, apiKey: _affiliateApiKey);
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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyCampaignCapForCreateClientApiTest()
        {
            var subject = "Registry limitation";

            var emailBodyParams = new List<string>
            { "AFFILIATE_NAME", "CAMPAIGN_NAME",
                "CAMPAIGN_ID", "LIMIT",
                "TIMEFRAME", "COUNTRY_NAME"};

            var emailsParams = new Dictionary<string, string> {
                { "type", "campaign_limit" },
                { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            _clientName = TextManipulation.RandomString();

            // create second client with campaign
            var actualErrorFirstLimitation = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, _clientName,
                _campaignId, country: _firstCountryNameForLimitation,
                apiKey: _affiliateApiKey, checkStatusCode: false);

            // create second client with campaign
            _clientName = TextManipulation.RandomString();

            var actualErrorSecondLimitation = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, _clientName,
                _campaignId, country: _secondCountryNameForLimitation,
                apiKey: _affiliateApiKey, checkStatusCode: false);

            // get First Country email body
            var actualFirstCountryEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(_testimUrl, _affiliateEmail, subject)
                .Where(p => p.Body.Contains(_firstCountryNameForLimitation))
                .FirstOrDefault();

            // get second Country email body
            var actualSecondCountryEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(_testimUrl, _affiliateEmail, subject)
                .Where(p => p.Body.Contains(_firstCountryNameForLimitation))
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualFirstCountryEmailBody.Body.Contains($"AFFILIATE_NAME={_affiliateName}") &&
                    actualSecondCountryEmailBody.Body.Contains($"CAMPAIGN_NAME={_campaignName}") &&
                    actualFirstCountryEmailBody.Body.Contains($"CAMPAIGN_ID={_campaignId}") &&
                    actualSecondCountryEmailBody.Body.Contains($"LIMIT={_leadsLimit}") &&
                    actualFirstCountryEmailBody.Body.Contains($"TIMEFRAME={_timeFrame}"),
                    $" actual body: {actualFirstCountryEmailBody.Body}");

                Assert.True(actualFirstCountryEmailBody.Subject == subject,
                    $" actual Subject: {actualFirstCountryEmailBody.Subject}" +
                    $" expected Subject: {subject}");
            });
        }
    }
}