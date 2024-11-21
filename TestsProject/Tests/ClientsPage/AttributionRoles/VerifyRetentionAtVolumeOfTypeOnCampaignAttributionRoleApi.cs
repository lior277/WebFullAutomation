// Ignore Spelling: Api

using System;
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
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Utils;
using Microsoft.Graph;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage.AttributionRoles
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyRetentionAtVolumeOfTypeOnCampaignAttributionRoleApi : TestSuitBase
    {
        public VerifyRetentionAtVolumeOfTypeOnCampaignAttributionRoleApi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _campaignId;
        private IWebDriver _driver;
        private string _browserName;
        private string _ftdUserName;
        private string _retentionUserName;    
        private string _adminUserName;
        private string _expectedFtdSalesAgentId;
        private string _expectedRetentionSalesAgentId;
        private string _attributionRoleName;
        private string _currentUserApiKey; 
        //private ExtentTest _etxtentTest; 

        [SetUp]
        public void SetUp()
        {
            //_etxtentTest = BeforeTest(_browserName);
            //_etxtentTest.Info("Get Driver");
            BeforeTest();
            _driver = GetDriver();          
            #region PreCondition
            var retentionType = "at volume of";
            var volume = 10;

            //_etxtentTest.Info(" create ftd user");
            // create ftd user
            _ftdUserName = TextManipulation.RandomString();

            _expectedFtdSalesAgentId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _ftdUserName, salesType: "ftd");

            _retentionUserName = TextManipulation.RandomString();

            // create retention user
            _expectedRetentionSalesAgentId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _retentionUserName, salesType: "retention");

            _adminUserName = TextManipulation.RandomString();

            // create admin user
            var adminUserId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _adminUserName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, adminUserId);

            // Create Affiliate And Campaign
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _campaignId = campaignData.Values.First();

            // create attribution role for at first deposit
            _attributionRoleName = TextManipulation.RandomString();
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PostCreateAttributionRoleRequest(_crmUrl, _attributionRoleName,
                 new string[] { _campaignId }, ftdAgentIds: new string[] { _expectedFtdSalesAgentId },
                 retentionAgentIds: new string[] { _expectedRetentionSalesAgentId },
                 retentionType: retentionType, actualVolume: volume);
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
                   .ToList() ?? throw new NullReferenceException("Attribution Role By Name is null ");

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
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }

        /// <summary>
        /// create campaingn
        /// create user type FTD
        /// create user type retention
        /// create attribution role type campaingn with :
        /// retention type : At volume of,
        /// ftd seller: user type FTD,
        /// retention seller: user type retention
        /// create client
        /// create deposit
        /// create deposit
        /// verify after first deposit client belong to ftd user 
        /// verify after second deposit that pass the volume of 10 client belong to retention use
        /// </summary>
        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyRetentionAtVolumeOfTypeOnCampaignAttributionRoleApiTest()
        {
            var firstDepositAmount = 9;
            var secondDepositAmount = 20;
            var expectedTimelineData = new List<string>();

            var expectedAssignToFtd = $"Client has automatically assigned to" +
                $" {_ftdUserName} by attribution rule {_attributionRoleName}";

            var expectedAssignToRettrntion = $"Client has automatically assigned to" +
                $" {_retentionUserName} by attribution rule {_attributionRoleName}";

            // create client with campaign
            var clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName,
                campaignId: _campaignId, apiKey: _currentUserApiKey);

            // create first deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId,
                firstDepositAmount, apiKey: _currentUserApiKey);

            var actualFtdSalesAgentId = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, clientId)
                .GeneralResponse
                .user
                .sales_agent;

            // create second deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId,
                secondDepositAmount, apiKey: _currentUserApiKey);

            Thread.Sleep(100); // wait for attribution rule

            var actualRetentionSalesAgentId = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, clientId)
                .GeneralResponse
                .user
                .sales_agent;

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_adminUserName, _crmUrl);

            var timelineDitails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .SearchClientByEmail(clientName)
                .ClickOnClientFullName()
                .ClickOnTimelineTab()
                .SetNumOfLines()
                .GetSearchResultDetails<SearchResultTimeline>(clientName,
                checkNumOfRows: false)
                .ToList();

            var actualTimelineActions = new List<string>();
            timelineDitails.ForEach(p => actualTimelineActions.Add(p.action.RemoveNewLine()));

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actualTimelineActions.Contains(expectedAssignToFtd),
                    $" actual not contain: {expectedAssignToFtd}"?.ToBold());

                Assert.IsTrue(actualTimelineActions.Contains(expectedAssignToRettrntion),
                    $" actual not contain: {expectedAssignToRettrntion}");

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