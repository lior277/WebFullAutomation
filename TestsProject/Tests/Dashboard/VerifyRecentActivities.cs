using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using Microsoft.Kiota.Abstractions;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.GetUserTimeLine;

namespace TestsProject.Tests.Dashboard
{
    [NonParallelizable]
    [TestFixture(DataRep.Chrome)]
    public class VerifyRecentActivities : TestSuitBase
    {
        #region Test Preparation
        public VerifyRecentActivities(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private int _depositAmount = 100;
        private string _browserName;
        private string _clientName;
        private string _clientId;
        private string _userIdForAssign;
        private string _userNameForAssign;
        private string _userId;
        private string _userApiKey;
        private string _userName;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var deposit = "100";
            _driver = GetDriver();

            _userName = TextManipulation.RandomString();
            var userEmail = _userName + DataRep.EmailPrefix;

            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName);

            // get ApiKey
            _userApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            // create client 
            _clientName = TextManipulation.RandomString();
            var clientEmail = _clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName, apiKey: _userApiKey);

            _userNameForAssign = TextManipulation.RandomString();

            // create user
            _userIdForAssign = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _userNameForAssign);

            // connect One User To One Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userIdForAssign,
                new List<string> { _clientId }, apiKey: _userApiKey);

            // search client
            _apiFactory
                 .ChangeContext<IMenus>(_driver)
                 .ClickOnMenuItem<IClientsPageUi>()
                 .SearchClientByEmail(clientEmail);

            // open client card
            _apiFactory
                .ChangeContext<ISearchResultsUi>(_driver)
                .ClickOnClientFullName();

            // create deposit
            _apiFactory
                .ChangeContext<IClientCardUi>(_driver)
                .ClickOnFinanceTab()
                .ClickOnFinanceButton("Deposit")
                .ChangeContext<IDepositUi>(_driver)
                .CreateDeposit(deposit);

            // create comment
            _apiFactory
                .ChangeContext<IClientCardUi>(_driver)
                .ClickOnCommentTab()
                .CreateCommentPipe()
                .ChangeContext<IClientCardUi>(_driver)
                .ClickOnCloseBtn()
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IDashboardPageUi>();
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
        public void VerifyRecentActivitiesTest()
        {
            var clientNameWithUpper = _clientName.UpperCaseFirstLetter();
            var expectedType = "change_sales_agent";
            var userNameWithUpper = _userName.UpperCaseFirstLetter();
            var expectedCreatedAt = DateTime.Now.ToString("yyyy-MM-dd");
            var depositComment = $"Added a Deposit of {_depositAmount}.00 {DataRep.DefaultUSDCurrencyName}";
            var timeLineParams = new Dictionary<string, object>
                { {"old_sales_agent_id","" }, { "new_sales_agent_id", _userIdForAssign },
                 { "no_limit", true }, { "page", 0 } };

            // deposit recent activity
            _apiFactory
                .ChangeContext<IDashboardPageUi>(_driver)
                .ClickOnRecentActivitiesDeposit()
                .VerifyRecentActivities(_userName, clientNameWithUpper, depositComment);

            // login recent activity
            _apiFactory
                .ChangeContext<IDashboardPageUi>(_driver)
                .ClickOnRecentActivitiesLogin()
                .VerifyRecentActivities(userNameWithUpper, comment: DataRep.LoginComment);

            // register recent activity
            _apiFactory
                .ChangeContext<IDashboardPageUi>(_driver)
                .ClickOnRecentActivitiesRegister()
                .VerifyRecentActivities(clientName: clientNameWithUpper,
                comment: DataRep.RegisterComment);

            // Comments recent activity
            _apiFactory
                .ChangeContext<IDashboardPageUi>(_driver)
                .ClickOnRecentActivitiesComments()
                .VerifyRecentActivities(_userName, clientNameWithUpper, DataRep.AddComment);

            // Get User Time Line
            var actualUserTimeLine = _apiFactory
                .ChangeContext<IDashboardApi>(_driver)
                .GetUserTimeLineRequest(_crmUrl, timeLineParams, _userApiKey)
                .Where(p => p.action_made_by == _userName)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualUserTimeLine.action_made_by == _userName,
                    $" expected action made by: {_userName}, " +
                    $" actual action made by: {actualUserTimeLine.action_made_by}");

                Assert.True(actualUserTimeLine.action_made_by_user_id == _userId,
                    $" expected action made by user Id: {_userId}, " +
                    $" actual action made by user Id: {actualUserTimeLine.action_made_by_user_id}");

                Assert.True(actualUserTimeLine.created_at.Contains(expectedCreatedAt),
                    $" expected created at: {expectedCreatedAt}, " +
                    $" actual created at: {actualUserTimeLine.created_at}");

                Assert.True(actualUserTimeLine.erp_user_id == _userId,
                    $" expected erp user id : {_userId}, " +
                    $" actual erp user id : {actualUserTimeLine.erp_user_id}");

                Assert.True(actualUserTimeLine.erp_username == _userName,
                    $" expected erp user name: {_userName}, " +
                    $" actual erp user name: {actualUserTimeLine.erp_username}");

                Assert.True(actualUserTimeLine.new_sales_agent_id == _userIdForAssign,
                    $" expected new sales agent id: {_userIdForAssign}, " +
                    $" actual new sales agent id: {actualUserTimeLine.new_sales_agent_id}");

                Assert.True(actualUserTimeLine.new_sales_agent_name == _userNameForAssign,
                    $" expected new sales agent name: {_userNameForAssign}, " +
                    $" actual new sales agent name: {actualUserTimeLine.new_sales_agent_name}");

                Assert.True(actualUserTimeLine.old_sales_agent_id == _userId,
                    $" expected old sales agent id: {_userId}, " +
                    $" actual old sales agent id: {actualUserTimeLine.old_sales_agent_id}");

                Assert.True(actualUserTimeLine.old_sales_agent_name == _userName,
                    $" expected old sales agent name: {_userName}, " +
                    $" actual old sales agent name: {actualUserTimeLine.old_sales_agent_name}");

                Assert.True(actualUserTimeLine.type == expectedType,
                    $" expected type: {expectedType}, " +
                    $" actual type: {actualUserTimeLine.type}");

                Assert.True(actualUserTimeLine.user_full_name == $"{_clientName} {_clientName}",
                    $" expected client name : {_clientName} {_clientName}, " +
                    $" actual client name : {actualUserTimeLine.user_full_name}");

                Assert.True(actualUserTimeLine.user_id == _clientId,
                    $" expected client id : {_clientId}, " +
                    $" actual client id : {actualUserTimeLine.user_id}");
            });
        }
    }
}
