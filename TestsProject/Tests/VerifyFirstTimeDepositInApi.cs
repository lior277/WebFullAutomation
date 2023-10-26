// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;

using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using TestsProject.TestsInternals;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Internals.Factory;

namespace TestsProject.Tests
{
    [TestFixture]
    public class VerifyFirstTimeDepositInApi : TestSuitBase
    {
        #region Test Preparation

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _campaignId;
        private string _depositClientId;
        private string _noDepositClientId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition        
            // create client 
            var clientName = TextManipulation.RandomString();

            _depositClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            var clientsIdsForDeposit = new List<string> { _depositClientId };

            clientName = TextManipulation.RandomString();

            _noDepositClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            var allclienstIds = new List<string> { _noDepositClientId };
            allclienstIds.AddRange(clientsIdsForDeposit);

            var campaignIdAndName = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _campaignId = campaignIdAndName.Values.First();

            _apiFactory
                .ChangeContext<IClientsApi>()
                 .PatchMassAssignCampaign(_crmUrl,
                 allclienstIds.ToList(), _campaignId);

            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientsIdsForDeposit, 10);
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

        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyFirstTimeDepositInApiTest()
        {
            var clientRegistrationDitails = _apiFactory
                .ChangeContext<IAffiliatesRequestsApi>()
                .GetClientRegistrationRequest(_crmUrl, _campaignId);

            var checkClientWithDeposit = clientRegistrationDitails
                    .Any(d => d._id == _depositClientId && d.has_ftd);

            var checkClientWithNoDeposit = clientRegistrationDitails
                    .Any(d => d._id == _noDepositClientId && !d.has_ftd);

            Assert.Multiple(() =>
            {
                Assert.True(checkClientWithDeposit, $"expected: {true} actual : {false}");
                Assert.True(checkClientWithNoDeposit, $"expected: {true} actual : {false}");
            });
        }
    }
}