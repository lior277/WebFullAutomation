// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage.AttributionRoles
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyDuplicateAttributionRuleErrorApi : TestSuitBase
    {
        public VerifyDuplicateAttributionRuleErrorApi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _campaignId;
        private IWebDriver _driver;
        private string _browserName;
        private string _ftdSalesAgentId;
        private string _countryAttributionRoleName;
        private string _campaignAttributionRoleName;
        private string _countryName = "anguilla";
        private string _campaignName;
        private string _ftdUserName;
        //private ExtentTest _etxtentTest;

        [SetUp]
        public void SetUp()
        {
            //_etxtentTest = BeforeTest(_browserName);
            //_etxtentTest.Info("Get Driver");
            BeforeTest();
            _driver = GetDriver();

            #region PreCondition
            // delete attribution role
            var AttributionRoleByCountry = _apiFactory
               .ChangeContext<IClientsApi>()
               .GetAttributionRolesRequest(_crmUrl)
               .GeneralResponse
               .Where(p => p.country.FirstOrDefault() == _countryName)
               .ToList();

            if (AttributionRoleByCountry != null)
            {
                _apiFactory
                    .ChangeContext<IClientsApi>()
                    .DeleteAttributionRolesRequest(_crmUrl, AttributionRoleByCountry);
            }

            // create user ftd Sales Agent
            _ftdUserName = TextManipulation.RandomString();

            _ftdSalesAgentId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _ftdUserName, salesType: "ftd");

            // Create Affiliate And Campaign
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _campaignId = campaignData.Values.First();
            _campaignName = campaignData.Keys.First();    

            // create attribution role for campaign
            _campaignAttributionRoleName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IClientsApi>()
                .PostCreateAttributionRoleRequest(_crmUrl,
                _campaignAttributionRoleName, new string[]
                { _campaignId }, ftdAgentIds: new string[]
                { _ftdSalesAgentId });

            // create attribution role for country
            _countryAttributionRoleName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IClientsApi>()
                .PostCreateAttributionRoleRequest(_crmUrl,
                _countryAttributionRoleName,
                actualType: "country",
                countryNames: new string[] { _countryName },
                ftdAgentIds: new string[] { _ftdSalesAgentId });

            // create attribution role for country and campaign
            var countryAndCampaignAttributionRoleName =
                TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IClientsApi>()
                .PostCreateAttributionRoleRequest(_crmUrl,
                countryAndCampaignAttributionRoleName,
                campaignId: new string[] { _campaignId },
                countryNames: new string[] { _countryName },
                ftdAgentIds: new string[] { _ftdSalesAgentId });
            #endregion
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete attribution role
                var attributionRole = _apiFactory
                    .ChangeContext<IClientsApi>()
                    .GetAttributionRolesRequest(_crmUrl)
                    .GeneralResponse
                    .Where(p => p.country?.FirstOrDefault() == _countryName
                    || p.campaign_id?.FirstOrDefault() == _campaignId)?
                    .ToList();

                if (attributionRole != null)
                {
                    _apiFactory
                        .ChangeContext<IClientsApi>()
                        .DeleteAttributionRolesRequest(_crmUrl, attributionRole);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }

        [Test]
        [Description("Based on tiket: https://airsoftltd.atlassian.net/browse/AIRV2-5401")]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyDuplicateAttributionRuleErrorApiTest()
        {
            var expectedDuplicateError = "This attribution" +
                " rule can’t be created due to duplicate entities.";

            var expectedDuplicateCountryAndCampaignError = $"'{_countryName}' " +
                $"already has a rule configurations for '{_campaignName}' campaign.";

            var expectedDuplicateNameError = $"The name is already in use";

            // create attribution role for duplicate name
            var actualDuplicaeNameError = _apiFactory
                .ChangeContext<IClientsApi>()
                .PostCreateAttributionRoleRequest(_crmUrl,
                _campaignAttributionRoleName,
                actualType: "country",
                countryNames: new string[] { _countryName },
                ftdAgentIds: new string[] { _ftdSalesAgentId },
                checkStatusCode: false);

            // create attribution role for duplicate campaign
            _campaignAttributionRoleName = TextManipulation
                .RandomString() + "dupCampaign";

            var actualDuplicaeCampaignError = _apiFactory
                .ChangeContext<IClientsApi>()
                .PostCreateAttributionRoleRequest(_crmUrl,
                _campaignAttributionRoleName,
                campaignId: new string[] { _campaignId },
                ftdAgentIds: new string[] { _ftdSalesAgentId },
                checkStatusCode: false);

            var _countryAttributionRoleName =
                TextManipulation.RandomString() + "dupCountry";

            // create attribution role for duplicate country
            var actualDuplicaeCountryError = _apiFactory
                .ChangeContext<IClientsApi>()
                .PostCreateAttributionRoleRequest(_crmUrl,
                 _countryAttributionRoleName,
                 actualType: "country",
                 countryNames: new string[] { _countryName },
                 ftdAgentIds: new string[] { _ftdSalesAgentId },
                 checkStatusCode: false);

            var countryAndCampaignAttributionRoleName =
                TextManipulation.RandomString() + "dupContryAndCampaign";

            // login
            var actualDuplicaeCountryAndCampaignError = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_ftdUserName, _crmUrl)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .ClickOnCreateAttributionRuleButton()
                .SetName(countryAndCampaignAttributionRoleName)
                .SelectType("campaign")
                .SelectCampaignPipe(_campaignName)
                .SelectCountryPipe(_countryName, false)
                .GetDuplicateAttributionMessage();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actualDuplicaeNameError.Contains(expectedDuplicateNameError),
                    $" actual {actualDuplicaeNameError} not contain: {expectedDuplicateNameError}");

                Assert.IsTrue(actualDuplicaeCampaignError.Contains(expectedDuplicateError),
                    $" actual {actualDuplicaeCampaignError} not contain: {expectedDuplicateError}");

                Assert.IsTrue(actualDuplicaeCountryError.Contains(expectedDuplicateError),
                    $" actual {actualDuplicaeCountryError} not contain: {expectedDuplicateError}");

                Assert.IsTrue(actualDuplicaeCountryAndCampaignError.Contains(expectedDuplicateCountryAndCampaignError),
                    $" actual {actualDuplicaeCountryAndCampaignError} not contain: {expectedDuplicateCountryAndCampaignError}");
            });
        }
    }
}