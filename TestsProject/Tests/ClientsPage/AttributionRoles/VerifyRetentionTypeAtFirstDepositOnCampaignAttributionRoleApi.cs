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
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage.AttributionRoles
{
    [TestFixture]
    public class VerifyRetentionTypeAtFirstDepositOnCampaignAttributionRoleApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _campaignId;
        private string _expectedFtdSalesAgentId;
        private string _expectedRetentionSalesAgentId;
        private string _attributionRoleName = TextManipulation.RandomString();

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            var retentionType = "at first deposit";

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
                 ftdAgentIds: new string[] { _expectedFtdSalesAgentId },
                 retentionAgentIds: new string[] { _expectedRetentionSalesAgentId },
                 retentionType: retentionType);
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
        /// create user type retention
        /// create attribution role type campaingn with :
        /// retention type : At first deposit,
        /// FTD SELLERS: user type FTD,
        /// RETENTION SELLERS: user type retention
        /// create client
        /// create deposit
        /// verify after client creation client belong to FTD user 
        /// verify after first deposit client belong to retention use
        /// </summary>
        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyRetentionTypeAtFirstDepositOnCampaignAttributionRoleApiTest()
        {
            // create client with campaign
            var clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName,
                campaignId: _campaignId);

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

            Thread.Sleep(200); // wait for attribution role to create

            var actualRetentionSalesAgentId = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, clientId)
                .GeneralResponse
                .user
                .sales_agent;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actualFtdSalesAgentId.Equals(_expectedFtdSalesAgentId),
                    $" actual Ftd User Id: {actualFtdSalesAgentId} " +
                    $" expected Ftd User Id: {_expectedFtdSalesAgentId}");

                Assert.IsTrue(actualRetentionSalesAgentId.Equals(_expectedRetentionSalesAgentId),
                    $" actual Retention User Id: {actualRetentionSalesAgentId} " +
                    $" expected Retention User Id: {_expectedRetentionSalesAgentId}");
            });
        }
    }
}