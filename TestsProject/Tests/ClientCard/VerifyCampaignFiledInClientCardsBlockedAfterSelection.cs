using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyCampaignFiledInClientCardsBlockedAfterSelection : TestSuitBase
    {
        #region Test Preparation
        public VerifyCampaignFiledInClientCardsBlockedAfterSelection(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var crmUrl = Config.appSettings.CrmUrl;
            _driver = GetDriver();

            var userName = TextManipulation.RandomString();
            var userMail =userName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(crmUrl, userName);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userMail);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(crmUrl, clientName);

            var clientIds = new List<string> { clientId };

            var campaignIdAndName = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .CreateAffiliateAndCampaignApi(crmUrl);

            var campaignId = campaignIdAndName.Values.First();

            _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .PatchMassAssignCampaign(crmUrl, clientIds, campaignId);

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
        public void VerifyCampaignFiledInClientCardsBlockedAfterSelectionTest()
        {
            _apiFactory
                .ChangeContext<ISearchResultsUi>(_driver)
                .ClickOnClientFullName()
                .ChangeContext<IInformationTabUi>(_driver)
                .VerifyCampaignFiledDisable();           
        }
    }
}