// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Banking.DepositsPage.Filters
{
    [TestFixture]
    public class VerifyCampaignIdFilterOnBankingDepositPageApi : TestSuitBase
    {
        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;
        private int _firstDepositAmount = 400;
        private string _currentUserApiKey;
        private string _firstCampaignId;
        private string _secondCampaignId;
        #endregion      

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var userName = TextManipulation.RandomString();

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName,
             role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            // create first campaign 
            var firstCampaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _firstCampaignId = firstCampaignData.Values.First();

            // create first client with first campaign
            var clientName = TextManipulation.RandomString();

            var firstClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName,
                campaignId: _firstCampaignId, apiKey: _currentUserApiKey);

            // deposit 400 for first client
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, firstClientId, _firstDepositAmount,
                originalCurrency: "EUR", apiKey: _currentUserApiKey);

            // create second campaign 
            var secondCampaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _secondCampaignId = secondCampaignData.Values.First();

            // create second client with second campaign
            clientName = TextManipulation.RandomString();

            var secondClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName,
                campaignId: _secondCampaignId, apiKey: _currentUserApiKey);

            // deposit 400 for second client
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, secondClientId,
                _firstDepositAmount, originalCurrency: "EUR", apiKey: _currentUserApiKey);

            #region connect One User To One Client 
            // connect One User To One Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { firstClientId, secondClientId }, apiKey: _currentUserApiKey);
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

        // create two campaigns
        // create two clients for each campaign
        // connect clients to user
        // verify search result in deposit banking
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyCampaignIdFilterOnBankingDepositPageApiTest()
        {
            var getDepositsByFirstCampaignId = _apiFactory
               .ChangeContext<IDepositsPageApi>()
               .GetDepositDataFromBankingRequest(_crmUrl, "campaign_id", _firstCampaignId, _currentUserApiKey)
               .GeneralResponse;

            var getDepositsBySecondCampaignId = _apiFactory
                .ChangeContext<IDepositsPageApi>()
                .GetDepositDataFromBankingRequest(_crmUrl, "campaign_id", _secondCampaignId, _currentUserApiKey)
                .GeneralResponse;

            Assert.Multiple(() =>
            {
                Assert.True(getDepositsByFirstCampaignId.recordsTotal == 1,
                    $" expected Total records: 1" +
                    $" actual Total records :  {getDepositsByFirstCampaignId.recordsTotal}");

                Assert.True(getDepositsByFirstCampaignId.data[0].status == "approved",
                    $" expected deposit status: approved" +
                    $" actual deposit status :  {getDepositsByFirstCampaignId.data[0].status}");

                Assert.True(getDepositsBySecondCampaignId.recordsTotal == 1,
                     $" expected Total records: 1" +
                     $" actual Total records :  {getDepositsBySecondCampaignId.recordsTotal}");

                Assert.True(getDepositsBySecondCampaignId.data[0].status == "approved",
                    $" expected deposit status: approved" +
                    $" actual deposit status :  {getDepositsBySecondCampaignId.data[0].status}");
            });
        }
    }
}
