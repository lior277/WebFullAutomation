using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.CampaignPage.Dashboard
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyCampaignsTitlesFromListBarsDiagram : TestSuitBase
    {
        #region Test Preparation
        public VerifyCampaignsTitlesFromListBarsDiagram(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IWebDriver _driver;
        private string _subCampaignName;
        private string _campaignName;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // get first Affiliate ApiKey
            var userApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create first campaign and Affiliate
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl, apiKey: userApiKey);

            var campaignId = campaignData.Values.First();
            _campaignName = campaignData.Keys.First();

            // create client 
            var clientName = TextManipulation.RandomString();
            _subCampaignName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName,
                campaignId: campaignId, apiKey: userApiKey,
                subCampaign: _subCampaignName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ICampaignsPageUi>();
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
        public void VerifyCampaignsTitlesFromListBarsDiagramTest()
        {
            var expectedCampaignTitles = new List<string> { _campaignName, _subCampaignName };

           var actualCampaignsTitles =  _apiFactory
                .ChangeContext<ICampaignsPageUi>(_driver)
                .GetCampaignsTitlesFromListBarsDiagram();

            var actualCompareToExpected = actualCampaignsTitles
                .CompareTwoListOfString(expectedCampaignTitles);

            Assert.True(actualCompareToExpected.Count == 0,
               $" actual Campaigns Titles : {actualCampaignsTitles.ListToString()}" +
               $" expected Campaigns Titles : {expectedCampaignTitles.ListToString()}");
        }
    }
}