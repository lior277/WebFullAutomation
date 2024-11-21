using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.CampaignPage.Dashboard
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyInActiveCampaignCanBeCreated : TestSuitBase
    {
        #region Test Preparation
        public VerifyInActiveCampaignCanBeCreated(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            var userName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // login
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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyInActiveCampaignCanBeCreatedTest()
        {
            var date = DateTime.Now.ToString("dd/MM/yyyy");
            var affiliateName = TextManipulation.RandomString();
            var campaignName = TextManipulation.RandomString().UpperCaseFirstLetter();

            var InActiveCmpaignDitails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ICampaignsPageUi>()
                .ClickCreateAffiliateButton()
                .CreateAffiliateUiPipe(_crmUrl, affiliateName)
                .ClickCreateCampaignButton()
                .CreateCampaignUiPipe(affiliateName, campaignName)
                .SearchInActiveCampaign(campaignName)
                .GetSearchResultDetails<SearchResultInactiveCampaign>()
                .First();               

            Assert.Multiple(() =>
            {
                Assert.True(InActiveCmpaignDitails.campaignname == campaignName,
                    $" expected _campaignName: {campaignName}" +
                    $" actual _campaignName: {InActiveCmpaignDitails.campaignname}");

                Assert.True(InActiveCmpaignDitails.deal == "cpa",
                    $" expected: 'cpa'" +
                    $" actual: {InActiveCmpaignDitails.deal}");

                Assert.True(InActiveCmpaignDitails.creationdate.Contains(date),
                    $" expected: {date}, " +
                    $" actual: {InActiveCmpaignDitails.creationdate}");
            });
        }
    }
}