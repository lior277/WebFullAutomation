// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Affiliate
{
    [TestFixture]
    public class VerifyAffiliateCanSeeAllSalesStatusApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _affiliateApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create affiliate role with all permissions
            _apiFactory
                .ChangeContext<ICreateEditRoleApi>()
                .PostAffiliateRoleWithAllPermissionsRequest(_crmUrl);

            // Create Affiliate And Campaign
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl,
                roleName: DataRep.AffiliateWithAllPermissionRole);

            var affiliateId = campaignData.Values.Last();

            // get first Affiliate ApiKey
            _affiliateApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, affiliateId);
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
        /// verify affiliate can see languages, countries, salesStatus
        /// </summary>
        /// 
        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyAffiliateCanSeeAllSalesStatusApiTest()
        {
            var expectedStatuses = new List<string>() { "Call Back", "Deposit",
                "Double Phone Number", "Hot Lead", "Interested", "New",
                "No Age", "No Answer", "No Language", "Not Interested",
                "Voice Mail", "Wrong Number"};
             
            var actualSaleStatusList = _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .GetSalesStatusFromApiDocRequest(_crmUrl, _affiliateApiKey);

            var expectedExceptActual = expectedStatuses
                .OrderBy(x => x)
                .Except(actualSaleStatusList.OrderBy(x => x))
                .ToList();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(expectedExceptActual.Count() == 0,
                    $" Actual  Against expected list: {expectedExceptActual.ListToString()}" +
                    $" actual Status: {actualSaleStatusList.ListToString()} " +
                    $" expected Status: {expectedStatuses.ListToString()}");
            });
        }
    }
}