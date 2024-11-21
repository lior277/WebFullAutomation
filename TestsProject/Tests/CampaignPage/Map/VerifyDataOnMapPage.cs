using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Map;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.CampaignPage.Map
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyDataOnMapPage : TestSuitBase
    {
        #region Test Preparation
        public VerifyDataOnMapPage(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IDictionary<string, string> _campaignData;
        private int _depositAmount = 10000;
        private int _secondDepositAmount = 500;
        private IWebDriver _driver;
        private string _userName;
        private string _userCountry = "afghanistan";
        private string _browserName;
        private string _campaignName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();
            var clientCurrency = "EUR";

            // create user
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName,
                country: _userCountry,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            _campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .CreateAffiliateAndCampaignApi(_crmUrl, apiKey: currentUserApiKey);

            var campaignId = _campaignData.Values.First();
            _campaignName = _campaignData.Keys.First();

            // create client
            var clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>(_driver)
                .CreateClientWithCampaign(_crmUrl, clientName, campaignId,
                clientCurrency, apiKey: currentUserApiKey);

            // add first deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, clientId,
                _depositAmount, originalCurrency: clientCurrency,
                apiKey: currentUserApiKey);

            // add second deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, clientId,
                _secondDepositAmount, originalCurrency: clientCurrency,
                apiKey: currentUserApiKey);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            Thread.Sleep(1000);

            // navigate to campaign map page
            _apiFactory                
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/campaigns/map-statistics");
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
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyDataOnMapPageTest()
        {
            #region expected test data
            var expectedLeadsFilterData = new List<string>() { "1", "1",
                _userCountry.UpperCaseFirstLetter(), _campaignName };

            var expectedConversionFilterData = new List<string>() { "100.00  (avg.)", "100.00",
                _userCountry.UpperCaseFirstLetter(), _campaignName };

            var expectedDepositFilterData = new List<string>() { "2", "2",
                _userCountry.UpperCaseFirstLetter(),  _campaignName };

            var expectedTotalFtdFilterData = new List<string>() { "1", "1",
                _userCountry.UpperCaseFirstLetter(), _campaignName };

            var expectedTotalDepositFilterData = new List<string>() { "10,500.00", "10,500.00",
                _userCountry.UpperCaseFirstLetter(), _campaignName };

            var expectedInActiveCampaignTableValues = new List<string>() 
            { _userCountry.UpperCaseFirstLetter(),
                "1", "1", "2", "100.00", "0.00", "10,500.00", "10,500.00" };
            #endregion

            var countrySymble = "AFG";
            var leadsFilter = "leads";
            var conversionFilter = "conversion";
            var depositFilter = "deposit";
            var totalFtdFilter = "total_ftd";
            var totalDepositFilter = "total_deposit";

            _apiFactory
                .ChangeContext<ICampaignsMapPageUi>(_driver)
                .VerifyCountryHighlightedBySymble(countrySymble);

            // Get Lead Filter Data
             var actualLeadsFilterData = _apiFactory
                .ChangeContext<ICampaignsMapPageUi>(_driver)
                .ClickOnFilterButtonByName(leadsFilter)
                .GetFilterDataPipe();

            // Get conversion Filter Data
            var actualConversionFilterData = _apiFactory
                .ChangeContext<ICampaignsMapPageUi>(_driver)
                .ClickOnFilterButtonByName(conversionFilter)
                .GetFilterDataPipe();

            // Get deposit Filter Data
            var actualDepositFilterData = _apiFactory
                .ChangeContext<ICampaignsMapPageUi>(_driver)
                .ClickOnFilterButtonByName(depositFilter)
                .GetFilterDataPipe();

            // Get total ftd Filter Data
            var actualTotalFtdFilterData = _apiFactory
                .ChangeContext<ICampaignsMapPageUi>(_driver)
                .ClickOnFilterButtonByName(totalFtdFilter)
                .GetFilterDataPipe();

            // Get total deposit Filter Data
            var actualTotalDepositFilterData = _apiFactory
                .ChangeContext<ICampaignsMapPageUi>(_driver)
                .ClickOnFilterButtonByName(totalDepositFilter)
                .GetFilterDataPipe();

            // Get In Active Campaign Table Values
            var actualInActiveCampaignTableValues = _apiFactory
                .ChangeContext<ICampaignsMapPageUi>(_driver)
                .GetiInActiveCampaignTableValues();

            Assert.Multiple(() =>
            {
                Assert.True(actualLeadsFilterData.CompareTwoListOfString(expectedLeadsFilterData).Count() == 0,
                    $" actual Leads Filter Data: {actualLeadsFilterData.ListToString()}" +
                    $" expected Leads Filter Data: {expectedLeadsFilterData.ListToString()}");

                Assert.True(actualConversionFilterData.CompareTwoListOfString(expectedConversionFilterData).Count() == 0,
                    $" actual Conversion Filter Data: {actualConversionFilterData.ListToString()}" +
                    $" expected Conversion Filter Data: {expectedConversionFilterData.ListToString()}");

                Assert.True(actualDepositFilterData.CompareTwoListOfString(expectedDepositFilterData).Count() == 0,
                    $" actual Deposit Filter Data: {actualDepositFilterData.ListToString()}" +
                    $" expected Deposit Filter Data: {expectedDepositFilterData.ListToString()}");

                Assert.True(actualTotalFtdFilterData.CompareTwoListOfString(expectedTotalFtdFilterData).Count() == 0,
                    $" actual Total Ftd Filter Data: {actualTotalFtdFilterData.ListToString()}" +
                    $" expected Total Ftd Filter Data: {expectedTotalFtdFilterData.ListToString()}");

                Assert.True(actualTotalDepositFilterData.CompareTwoListOfString(expectedTotalDepositFilterData).Count() == 0,
                    $" actual Total Deposit Filter Data: {actualTotalDepositFilterData.ListToString()}" +
                    $" expected TotalDeposit Filter Data: {expectedTotalDepositFilterData.ListToString()}");

                Assert.True(actualInActiveCampaignTableValues.CompareTwoListOfString(expectedInActiveCampaignTableValues).Count() == 0,
                    $" actual In Active Campaign Table Values: {actualInActiveCampaignTableValues.ListToString()}" +
                    $" expected In Active Campaign Table Values: {expectedInActiveCampaignTableValues.ListToString()}");
            });
        }
    }
}