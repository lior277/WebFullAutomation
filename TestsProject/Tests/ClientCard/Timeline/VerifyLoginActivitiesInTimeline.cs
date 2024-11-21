// Ignore Spelling: TimeLine

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
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Internals.Enums.EnumFactory;

namespace TestsProject.Tests.ClientCard.TimeLine
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyLoginActivitiesInTimeline : TestSuitBase
    {
        #region Test Preparation
        public VerifyLoginActivitiesInTimeline(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userName;
        private string _clientName;
        private string _clientId;
        private IWebDriver _driver;
        private string _browserName;
        #endregion

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

            _driver = GetDriver();
            var depositAmount = 10;

            // create user for the creation of api key
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // get ApiKey
            var userApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client
            _clientName = TextManipulation.RandomString();
            var clientEmail = _clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                apiKey: userApiKey);

            _clientName = _clientName.UpperCaseFirstLetter();

            // get default login Attempts 
            var loginAttemps = _apiFactory
                .ChangeContext<ISecurityTubApi>(_driver)
                .GetLoginSectionRequest(_crmUrl)
                .attempts;

            var password = DataRep.Password;
            var badPasword = $"bad{password}";

            for (var i = 0; i < loginAttemps; i++)
            {
                // tp login with wrong password
                _apiFactory
                    .ChangeContext<ILoginPageUi>(_driver)
                    .LoginPipe(clientEmail, tradingPlatformUrl,
                    password: badPasword);
            }
             
            // wait for client to be unblock
            for (var i = 0; i < 10; i++)
            {
                var blocked = _apiFactory
                    .ChangeContext<ISecurityTubApi>()
                    .GetBlockUsersRequest(_crmUrl)
                    .Any(p => p._id == _clientId);

                if (!blocked)
                {    
                    Thread.Sleep(300); 

                    continue;
                }

                break;
            }

            // release client from block
            _apiFactory
                .ChangeContext<ISecurityTubApi>()
                .PatchReleaseBlockUserRequest(_crmUrl, _clientId);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName, _crmUrl);

            // login
            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .ChangeContext<ISearchResultsUi>(_driver)
                .ClickOnClientFullName()
                .ClickOnFastLoginBtn();

            // switch to crm
            _apiFactory
                .ChangeContext<IGeneral>(_driver)
                .SwitchToExistingWindow(TabToSwitch.First);

            var loginData = _apiFactory // logged in and is currently online 
                .ChangeContext<ILoginApi>(_driver)
                .PostLoginToTradingPlatform(tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            // create deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, _clientId, depositAmount, apiKey: userApiKey);

            // logout
            _apiFactory
                .ChangeContext<ICreateClientApi>(_driver)
                .GetLogoutTreadingPlatformRequest(tradingPlatformUrl, loginData);
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
        public void VerifyLoginActivitiesInTimelineTest()
        {
            var expectedDate = DateTime.Now.ToString("ddd MMM dd yyy");          
            var expectedLogout = $"{_clientName} {_clientName} has logged into the trading-platform from";
            var expectedLogin = $"{_userName} Has logged into account";

            var timelineDitails = _apiFactory
                .ChangeContext<IClientCardUi>(_driver)
                .ClickOnCaptureTab()
                .ClickOnTimelineTab()
                .ChangeContext<ITimelineTabUi>(_driver)
                .ClickOnLoginActivityFilterButton()
                .ChangeContext<ISearchResultsUi>(_driver)
                .GetSearchResultDetails<SearchResultTimeline>(aditionalData:
                _clientName, expectedNumOfRows: 2, cellsAndTitlesShouldBeEquals: false)
                .ToList();

            var actualimelineActions = new List<string>();
            timelineDitails.ForEach(p => actualimelineActions.Add(p.action));

            var actualDate = timelineDitails
                .Select(d => d.date)
                .ToList()
                .All(p => p.Contains(expectedDate));

            Assert.Multiple(() =>
            {
                Assert.True(actualDate,
                    $" expected: {expectedDate}" +
                    $" actual : different then {actualimelineActions.ListToString()}");

                Assert.True(actualimelineActions.First().Contains(expectedLogout),
                    $" expected: {expectedLogout}" +
                    $" actual : different then {actualimelineActions.First()}");

                Assert.True(actualimelineActions.Last() == expectedLogin,
                    $" expected: {expectedLogin}" +
                    $" actual : different then {actualimelineActions.Last()}");
            });
        }
    }
}