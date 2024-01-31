// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Affiliate
{
    [NonParallelizable]
    [TestFixture("no permissions", null, null, null, null)]
    [TestFixture("all permissions", "22254445", "Double Phone Number", "2", "receive Email")]
    public class VerifyAffiliateRolesForClientsRegistrationsApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IDictionary<string, string> _campaignData;
        private string _clientEmail;
        private string _campaignId;
        private string _affiliateApiKey;
        private string _description;
        private string _expectedSalesStatus2;
        private string _expectedPhone;
        private string _expectedSalesStatus;
        private string _expectedEmail;

        public VerifyAffiliateRolesForClientsRegistrationsApi(string description,
            string expectedPhone, string expectedSalesStatus, string expectedSalesStatus2,
            string expectedEmail)
        {
            _description = description;
            _expectedPhone = expectedPhone;
            _expectedSalesStatus = expectedSalesStatus;
            _expectedSalesStatus2 = expectedSalesStatus2;
            _expectedEmail = expectedEmail;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            if (_description == "no permissions")
            {
                // create affiliate role with no permissions
                _apiFactory
                    .ChangeContext<ICreateEditRoleApi>()
                    .PostAffiliateRoleWithNoPermissionsRequest(_crmUrl);

                // Create Affiliate And Campaign
                _campaignData = _apiFactory
                    .ChangeContext<ISharedStepsGenerator>()
                    .CreateAffiliateAndCampaignApi(_crmUrl,
                    roleName: DataRep.AffiliateWithNoPermissionRole);
            }
            if (_description == "all permissions")
            {
                // create affiliate role with all permissions
                _apiFactory
                    .ChangeContext<ICreateEditRoleApi>()
                    .PostAffiliateRoleWithAllPermissionsRequest(_crmUrl);

                // Create Affiliate And Campaign
                _campaignData = _apiFactory
                    .ChangeContext<ISharedStepsGenerator>()
                    .CreateAffiliateAndCampaignApi(_crmUrl,
                    roleName: DataRep.AffiliateWithAllPermissionRole);
            }

            _campaignId = _campaignData.Values.First();
            var affiliateId = _campaignData.Values.Last();

            // get first Affiliate ApiKey
            _affiliateApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, affiliateId);

            // create client with first campaign
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName,
                campaignId: _campaignId, freeText: "", apiKey: _affiliateApiKey);

            // get client card
            var informationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, clientId)
                .GeneralResponse
                .informationTab;

            // set sales status 2 
            informationTabResponse.sales_status2 = _expectedSalesStatus2;
            informationTabResponse.sales_agent = "null";

            // update client card
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, informationTabResponse);
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
        /// client connected to the campaign
        /// all affiliate roles are checked
        /// verify affiliate can see 
        /// client email, sale status, Phone
        /// </summary>
        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyAffiliateRolesForClientsRegistrationsApiTest()
        {
            var actualclientsRegistrations = _apiFactory
                .ChangeContext<IAffiliatesRequestsApi>()
                .GetClientRegistrationRequest(_crmUrl, _campaignId, _affiliateApiKey)
                .First();

            if (_expectedEmail == "receive Email")
            {
                _expectedEmail = _clientEmail;
            }        

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actualclientsRegistrations.phone == _expectedPhone,
                    $" description: {_description}, expected Phone: {_expectedPhone}" +
                    $" actual phone: {actualclientsRegistrations.phone}");

                Assert.IsTrue(actualclientsRegistrations.sales_status == _expectedSalesStatus,
                    $" expected Sales Status: {_expectedSalesStatus}" +
                    $" actual Sales Status: {actualclientsRegistrations.sales_status}");

                Assert.IsTrue(actualclientsRegistrations.sales_status2 == _expectedSalesStatus2,
                    $" expected Sales Status 2: {_expectedSalesStatus2}" +
                    $" actual Sales Status 2: {actualclientsRegistrations.sales_status2}");

                Assert.IsTrue(actualclientsRegistrations.email == _expectedEmail,
                    $" description: {_description}, expected email: {_expectedEmail}" +
                    $" actual email: {actualclientsRegistrations.email}");
            });
        }
    }
}