// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using ConsoleApp;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage
{
    [TestFixture]
    public class VerifyChangeCampaignIsDisableForClientWithChargeBackApi : TestSuitBase
    {
        #region Test Preparation
        public VerifyChangeCampaignIsDisableForClientWithChargeBackApi() { }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _firstCampaignId;
        private string _secondCampaignId;
        private string _firstCampaignName;
        private string _secondCampaignName;
        private string _clientEmail;
        private string _userApiKey;
        private string _clientId;
        private string _depositId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var depositAmount = 10;
            var dbContext = new QaAutomation01Context();

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName);

            _userApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            #region create first campaign
            // create campaign
            var  campaignData = _apiFactory
                 .ChangeContext<ISharedStepsGenerator>()
                 .CreateAffiliateAndCampaignApi(_crmUrl);
            #endregion

            _firstCampaignId = campaignData.Values.First();
            _firstCampaignName = campaignData.Keys.First();
            var affiliateId = campaignData.Values.Last();

            _secondCampaignName = $"second{_firstCampaignName}";

            #region create second campaign with the same affiliate
            // create second campaign with the same affiliate
            _secondCampaignId = _apiFactory
                 .ChangeContext<ICampaignPageApi>()
                 .PostCreateCampaignRequest(_crmUrl, _secondCampaignName,
                 affiliateId);
            #endregion

            #region create client With the first Campaign
            // create client
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName, campaignId: _firstCampaignId,
                apiKey: _userApiKey);
            #endregion

            // create deposit 
            _depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, depositAmount,
                apiKey: _userApiKey);

            // get deposit id by amount
            var depositId =
              (from s in dbContext.FundsTransactions
               where (s.UserId == _clientId
               && s.OriginalAmount == depositAmount && s.Type == "deposit")
               select s.Id).First()
               .ToString();
            #endregion

            #region chargeback
            // chargeback
            var chargebackId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .DeleteChargeBackDepositRequest(_crmUrl, _clientId,
                depositAmount, depositId, apiKey: _userApiKey);           
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
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyChangeCampaignIsDisableForClientWithChargeBackApiTest()
        {
            var expectedClientId = _clientId;

            // admin cant change the campaign of client with chargeback, 
            var actualClientId = _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignCampaign(_crmUrl, new List<string> { _clientId },
                _secondCampaignId, _userApiKey)
                .GeneralResponse
                .FailedToMassAssignToCampaign
                .First();

            Assert.Multiple(() =>
            {
                Assert.True(actualClientId == expectedClientId,
                   $" expected Client Id : {expectedClientId}" +
                   $" actual Client Id : {actualClientId}");
            });
        }
    }
}