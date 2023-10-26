using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyMassAssign : TestSuitBase
    {
        public VerifyMassAssign(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IWebDriver _driver;
        private string _browserName;
        private string _campaignName;
        private string _firstClientId;
        private string _secondClientId;
        private string _firstClientEmail;
        private string _secondClientEmail;
        private string _userName;

        [SetUp]

        #region PreCondition
        public void SetUp()
        {
            BeforeTest(_browserName);           
            _driver = GetDriver();

            // create user
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName,
               role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _firstClientEmail = clientName + DataRep.EmailPrefix;

            _firstClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: currentUserApiKey);

            // create client
            clientName = TextManipulation.RandomString();
            _secondClientEmail = clientName + DataRep.EmailPrefix;

            _secondClientId = _apiFactory
                .ChangeContext<ICreateClientApi>(_driver)
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: currentUserApiKey);

            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .PatchMassAssignSaleAgentsRequest(_crmUrl, userId,
                new List<string> { _firstClientId, _secondClientId }, apiKey: currentUserApiKey);

            // Create Affiliate And Campaign
            var campaignIdAndName = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .CreateAffiliateAndCampaignApi(_crmUrl, apiKey: currentUserApiKey);

            _campaignName = campaignIdAndName.Keys.First();

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            Thread.Sleep(1000);
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
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }
        

        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyMassAssignTest()
        {
            var comment = "Automation comment";
            var newClientStatus1 = "Call Back";
            var newClientStatus2 = "1";
            var newTradeGroup = "Positive 3";

            var expectedTimelineTypes = new List<string>()
            { "change_sales_status", "change_sales_status",
                "change_sales_status2",
                "change_campaign", "delete_mass_comment",
                "create_comment", "client_register", "change_cfd_group" };

            var actualFirstClientDetails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .PerformMassAsignPipe(comment, 
                _userName, _campaignName, newClientStatus1, newClientStatus2, newTradeGroup)
                .ChangeContext<IClientsApi>(_driver)
                .GetClientRequest(_crmUrl, _firstClientEmail)
                .GeneralResponse
                .data
                .FirstOrDefault();

            var actualSecondClientDetails = _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .GetClientRequest(_crmUrl, _secondClientEmail)
                .GeneralResponse
                .data
                .FirstOrDefault();

            var actualFirstClientTimelineDetails = _apiFactory
                .ChangeContext<ITimeLineTabApi>(_driver)
                .GetTimelineRequest(_crmUrl, _firstClientId)
                .Select(p => p.type)
                .ToList();

            actualFirstClientTimelineDetails
                .RemoveAll(p => p.Equals("auto_email")); // remove the auto email deposit remainder 

            var actualFirstClientDifferance = actualFirstClientTimelineDetails
                .CompareTwoListOfString(expectedTimelineTypes);

            var actualSecondClientTimelineDetails = _apiFactory
                .ChangeContext<ITimeLineTabApi>(_driver)
                .GetTimelineRequest(_crmUrl, _secondClientId)
                .Select(p => p.type)
                .ToList();

            var actualSecondClientDifferance = actualFirstClientTimelineDetails
                .CompareTwoListOfString(expectedTimelineTypes);

            var actualFirstClientCryptoGroup = _apiFactory                
                .ChangeContext<IClientsApi>(_driver)
                .GetClientRequest(_crmUrl, _firstClientId)
                .GeneralResponse
                .data
                .FirstOrDefault();

            var actualSecondClientCryptoGroup = _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .GetClientRequest(_crmUrl, _secondClientId)
                .GeneralResponse
                .data
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualFirstClientDetails.campaign == _campaignName,
                    $" expected First Client campaign Name: {_campaignName}" +
                    $" actual First Client campaign Name: {actualFirstClientDetails.campaign}");

                Assert.True(actualFirstClientDetails.last_comment == null,
                    $" expected First Client  last comment time: null" +
                    $" actual First Client  last comment time: {actualFirstClientDetails.last_comment}");

                Assert.True(actualFirstClientDetails.sales_agent == _userName,
                    $" expected First Client sales agent: {_userName}, " +
                    $" actual First Client sales agent: {actualFirstClientDetails.sales_agent}");

                Assert.True(actualFirstClientDetails.sales_status == newClientStatus1,
                    $" expected First Client sales status: {newClientStatus1}, " +
                    $" actual First Client sales status: {actualFirstClientDetails.sales_status}");

                Assert.True(actualFirstClientDetails.sales_status2 == newClientStatus2,
                    $" expected First Client sales status: {newClientStatus2}, " +
                    $" actual First Client sales status: {actualFirstClientDetails.sales_status}");

                Assert.True(actualSecondClientDetails.campaign == _campaignName,
                    $" expected Second Client campaign Name: {_campaignName}" +
                    $" actual Second Client campaign Name: {actualSecondClientDetails.campaign}");

                Assert.True(actualSecondClientDetails.last_comment == null,
                    $" expected Second Client last comment time: null" +
                    $" actual Second Client last comment time: {actualSecondClientDetails.last_comment}");

                Assert.True(actualSecondClientDetails.sales_agent == _userName,
                    $" expected Second Client sales agent: {_userName}, " +
                    $" actual Second Client sales agent: {actualSecondClientDetails.sales_agent}");

                Assert.True(actualSecondClientDetails.sales_status == newClientStatus1,
                    $" expected Second Client sales status: {newClientStatus1}, " +
                    $" actual Second Client sales status: {actualSecondClientDetails.sales_status}");

                Assert.True(actualSecondClientDetails.sales_status2 == newClientStatus2,
                    $" expected Second Client sales status: {newClientStatus2}, " +
                    $" actual Second Client sales status: {actualSecondClientDetails.sales_status}");

                Assert.True(actualFirstClientDifferance.Count == 0,
                    $" Actual  Against expected list: {actualFirstClientDifferance.ListToString()} " +
                    $" expected first Client TimeLine Data: {expectedTimelineTypes.ListToString()}" +
                    $" actual first Client TimeLine Data: {actualFirstClientTimelineDetails.ListToString()}");

                Assert.True(actualSecondClientDifferance.Count == 0,
                    $" Actual  Against expected list: {actualSecondClientDifferance.ListToString()} " +
                    $" expected Second Client TimeLine Data: {expectedTimelineTypes.ListToString()}" +
                    $" actual Second Client TimeLine Data: {actualSecondClientDifferance.ListToString()}");

                Assert.True(actualFirstClientCryptoGroup.group == newTradeGroup,
                    $" expected crypto_group name: {newTradeGroup}" +
                    $" actual crypto_group name: {actualFirstClientCryptoGroup.group}");

                Assert.True(actualSecondClientCryptoGroup.group == newTradeGroup,
                    $" expected crypto_group name: {newTradeGroup}" +
                    $" actual crypto_group name: {actualSecondClientCryptoGroup.group}");
            });
        }
    }
}