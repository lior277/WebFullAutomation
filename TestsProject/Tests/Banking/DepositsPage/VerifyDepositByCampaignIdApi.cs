// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Banking.DepositsPage
{
    [TestFixture]
    public class VerifyDepositByCampaignIdApi : TestSuitBase
    {
        #region Test Preparation
        private readonly IApplicationFactory _apiFactory = new ApplicationFactory();
        private readonly string _crmUrl = Config.appSettings.CrmUrl;
        private readonly int _depositAmount = 400;
        private string _firstCampaignId;
        private List<string> _clientIds;
        private string _clientName;
        private string _firstAffiliateId;
        private string _secondAffiliateId;
        private string _depositId;
        private string _secondCampaignId;
        private string _firstAffiliateApiKey;
        private string _secondAffiliateApiKey;

        [SetUp]
        public void SetUp()
        {
           BeforeTest();
            #region PreCondition

            // create first campaign and Affiliate
            var firstCampaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            // create second campaign and Affiliate
            var secondCampaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _firstCampaignId = firstCampaignData.Values.First();
            _firstAffiliateId = firstCampaignData.Values.Last();
            _secondCampaignId = secondCampaignData.Values.First();
            _secondAffiliateId = secondCampaignData.Values.Last();

            // get first Affiliate ApiKey
            _firstAffiliateApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _firstAffiliateId);

            // get second Affiliate ApiKey
            _secondAffiliateApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _secondAffiliateId);

            // create client with first campaign
            _clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, _clientName,
                campaignId: _firstCampaignId);

            _clientIds = new List<string> { clientId};

            // deposit 400
            _depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId, _depositAmount);
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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyDepositByCampaignIdApiTest()
        {
            // Affiliate connected to the Campaign can see the deposit
            var affiliateCanSeeTheDeposit = _apiFactory
                   .ChangeContext<ICampaignPageApi>()
                   .GetDepositsByCampaignIdRequest(_crmUrl, _firstCampaignId, _firstAffiliateApiKey)
                   .GeneralResponse;

            // Affiliate not connected to the Campaign can not see the deposit
            var affiliateCanNotSeeTheDeposit = _apiFactory
                  .ChangeContext<ICampaignPageApi>()
                  .GetDepositsByCampaignIdRequest(_crmUrl, _firstCampaignId, _secondAffiliateApiKey, false)
                  .Message;

            _apiFactory
                 .ChangeContext<IClientsApi>()
                 .PatchMassAssignCampaign(_crmUrl, _clientIds, _secondCampaignId);

            // different Affiliate and different Campaign can not see the deposit
            var secondAffiliateCanNotSeeTheDeposit = _apiFactory
                 .ChangeContext<ICampaignPageApi>()
                 .GetDepositsByCampaignIdRequest(_crmUrl, _secondCampaignId, _secondAffiliateApiKey)
                 .GeneralResponse;

            Assert.Multiple(() =>
            {
                Assert.True(affiliateCanSeeTheDeposit.First().Id == _depositId,
                    " expected affiliate Can See The Deposit id: {_depositId}," +
                    $" actual affiliate Can See The Deposit id: {affiliateCanSeeTheDeposit.First().Id}");

                Assert.True(affiliateCanNotSeeTheDeposit == "",
                    " expected affiliate Can not See The Deposit id: null," +
                    $" actual affiliate Can not See The Deposit id: {affiliateCanNotSeeTheDeposit}");

                Assert.True(secondAffiliateCanNotSeeTheDeposit.Count == 0,
                    " expected second affiliate Can not See The Deposit id equal: 0," +
                    $" actual affiliate Can See The Deposit id: {secondAffiliateCanNotSeeTheDeposit.Count}");
            });
        }
    }
}