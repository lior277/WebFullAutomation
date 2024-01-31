using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.CampaignPage.Dashboard
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyActiveCampaignCanBeCreated : TestSuitBase
    {
        #region Test Preparation
        public VerifyActiveCampaignCanBeCreated(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IDictionary<string, string> _campaignData;
        private IWebDriver _driver;
        private string _userName;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            // create user
            _userName = TextManipulation.RandomString();

            // create user
            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            // create client
            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);        

            _campaignData = _apiFactory
                 .ChangeContext<ISharedStepsGenerator>(_driver)
                 .CreateAffiliateAndCampaignApi(_crmUrl);

            _apiFactory                
                 .ChangeContext<IMenus>(_driver)
                 .ClickOnMenuItem<IClientsPageUi>()
                 .SearchClientByEmail(clientEmail);
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
        public void VerifyActiveCampaignCanBeCreatedTest()
        {
            var date = DateTime.Now.ToString("dd/MM/yyyy");
            var campaignName = _campaignData.Keys.First();

            var activeCmpaignDitails = _apiFactory
                .ChangeContext<ISearchResultsUi>(_driver)
                .ConnectClientToCampaign(campaignName)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ICampaignsPageUi>()
                .SearchActiveCampaign(campaignName)
                .GetSearchResultDetails<SearchResultActiveCampaign>().First();

            Assert.Multiple(() =>
            {
                Assert.True(activeCmpaignDitails.name == campaignName.UpperCaseFirstLetter(),
                    $"expected: {campaignName}" +
                    $" actual : {activeCmpaignDitails.name}");

                Assert.True(activeCmpaignDitails.deal == "cpa",
                    $"expected: 'cpa'" +
                    $" actual: {activeCmpaignDitails.deal}");

                Assert.True(activeCmpaignDitails.creationdate.Contains(date),
                    $"expected: {date}, " +
                    $" actual: {activeCmpaignDitails.creationdate}");
            });
        }
    }
}