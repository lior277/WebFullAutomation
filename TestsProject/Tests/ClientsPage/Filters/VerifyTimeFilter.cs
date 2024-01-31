using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage.Filters
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyTimeFilter : TestSuitBase
    {
        #region Test Preparation
        public VerifyTimeFilter(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IDictionary<string, string> _campaignData;
        private IWebDriver _driver;
        public string _clientName;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            #region PreCondition
            _driver = GetDriver();

            // create user for the creation of api key
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // get ApiKey
            var apiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            _clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                apiKey: apiKey);

            _campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .CreateAffiliateAndCampaignApi(_crmUrl);

            var allClienstIds = new List<string> { clientId };
            var campaignId = _campaignData.Values.First();

            _apiFactory
                 .ChangeContext<IClientsApi>(_driver)
                 .PatchMassAssignCampaign(_crmUrl, allClienstIds.ToList(), campaignId);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyTimeFilterTest()
        {
            var campaignName = _campaignData.Keys.First();
            var campaignFilterValues = new string[] { campaignName };

            var clientDitails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .SetTimeFilter(DataRep.TodayFilter)
                .ChangeContext<IHandleFiltersUi>(_driver)
                .ClickToOpenFiltersMenu()
                .MultiSelectDropDownPipe(DataRep.CampaignsFilter, campaignFilterValues)
                .GetSearchResultDetails<SearchResultClients>()
                .First();

            Assert.Multiple(() =>
            {
                Assert.True(clientDitails.fullname.Contains(_clientName.UpperCaseFirstLetter()),
                     $" expected client full name : {_clientName.UpperCaseFirstLetter()}, " +
                     $" actual  client full name: {clientDitails.fullname}");

                Assert.True(clientDitails.campaign == campaignName,
                     $" expected  client campaign : {campaignName}, " +
                     $" actual  client campaign : {clientDitails.campaign}");
            });
        }
    }
}