// Ignore Spelling: Api

using System;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using Newtonsoft.Json;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage.AttributionRoles
{
    [TestFixture]
    public class VerifyRetentionTypeAtFirstDepositOnCampaignAndCountryAttributionRoleApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _campaignId;
        private string _expectedFtdSalesAgentId;
        private string _country = "afghanistan";
        private string _retentionType = "at first deposit";
        private string _expectedRetentionSalesAgentId;
        private string _attributionRoleName = TextManipulation.RandomString();

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            // create ftd user
            var userName = TextManipulation.RandomString();

            _expectedFtdSalesAgentId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName, salesType: "ftd");

            userName = TextManipulation.RandomString();

            // create retention user
            _expectedRetentionSalesAgentId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName, salesType: "retention");

            // Create Affiliate And Campaign
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _campaignId = campaignData.Values.First();

            // create attribution role for at first deposit
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PostCreateAttributionRoleRequest(_crmUrl,
                _attributionRoleName, new string[] { _campaignId }, 
                countryNames: new string[] { _country },
                ftdAgentIds: new string[] { _expectedFtdSalesAgentId },
                retentionAgentIds: new string[] { _expectedRetentionSalesAgentId },
                retentionType: _retentionType);

            Thread.Sleep(200); // wait for attribution role to create
            #endregion
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete attribution role
                var AttributionRoleByName = _apiFactory
                   .ChangeContext<IClientsApi>()
                   .GetAttributionRolesRequest(_crmUrl)
                   .GeneralResponse
                   .Where(p => p.name == _attributionRoleName)
                   .ToList();

                _apiFactory
                   .ChangeContext<IClientsApi>()
                   .DeleteAttributionRolesRequest(_crmUrl, AttributionRoleByName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                AfterTest();
            }
        }

        /// <summary>
        /// create campaingn
        /// create user type FTD
        /// create attribution role type campaingn and country with :
        /// retention type : At first deposit,
        /// FTD SELLERS: user type FTD,
        /// RETENTION SELLERS: user type retention
        /// create client with country
        /// create deposit
        /// verify after client creation client belong to FTD user 
        /// verify error for Attribution Role with the same name
        /// </summary>
        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyRetentionTypeAtFirstDepositOnCampaignAndCountryAttributionRoleApiTest()
        {
            var expectedDuplicateAttributionRoleMessage = "The name is already in use";

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client with campaign
            var clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName,
                campaignId: _campaignId, country: _country);

            var actualFtdSalesAgentId = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, clientId)
                .GeneralResponse
                .user
                .sales_agent;

            // create deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId, 10);

            var actualRetentionSalesAgentId = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, clientId)
                .GeneralResponse
                .user
                .sales_agent;

            // create attribution role with the same name
            var temp = _apiFactory
                .ChangeContext<IClientsApi>()
                .PostCreateAttributionRoleRequest(_crmUrl,
                _attributionRoleName,               
                campaignId: new string[] { _campaignId },
                countryNames: new string[] { _country },
                ftdAgentIds: new string[] { _expectedFtdSalesAgentId },
                retentionAgentIds: new string[] { _expectedRetentionSalesAgentId },
                retentionType: _retentionType,
                apiKey: currentUserApiKey, checkStatusCode: false);

            dynamic dynamicTemp = JsonConvert.DeserializeObject(temp);
            var actualDuplicateAttributionRoleMessage = dynamicTemp.name[0].Value;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actualFtdSalesAgentId.Equals(_expectedFtdSalesAgentId),
                    $" actual Ftd User Id: {actualFtdSalesAgentId} " +
                    $" expected Ftd User Id: {_expectedFtdSalesAgentId}");

                Assert.IsTrue(actualDuplicateAttributionRoleMessage == expectedDuplicateAttributionRoleMessage,
                    $" actual Duplicate Attribution Role Message: {actualFtdSalesAgentId} " +
                    $" expected Duplicate Attribution Role Message: {expectedDuplicateAttributionRoleMessage}");
            });
        }
    }
}