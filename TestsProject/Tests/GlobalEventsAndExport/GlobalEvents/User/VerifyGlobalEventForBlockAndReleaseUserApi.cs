// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents.Banner
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyGlobalEventForBlockAndReleaseUserApi : TestSuitBase
    {
        public VerifyGlobalEventForBlockAndReleaseUserApi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _expectedUserName;
        private string _expectedUserId;
        private string _secondUserName;
        private string _blockUserId;
        private string _browserName;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _driver = GetDriver();

            // create user
            _expectedUserName = TextManipulation.RandomString();

            // create user
            _expectedUserId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _expectedUserName, 
             role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _expectedUserId);
            #endregion

            _secondUserName = TextManipulation.RandomString();
            var userEmail = _secondUserName + DataRep.EmailPrefix;

            // create another user
            _blockUserId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _secondUserName,
               apiKey: _currentUserApiKey);

            // get default login Attempts 
            var loginAttemps = _apiFactory
                .ChangeContext<ISecurityTubApi>()
                .GetLoginSectionRequest(_crmUrl)
                .attempts;

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_secondUserName, _crmUrl, "WrongPassword");

            // make client blocked
            for (var i = 0; i < loginAttemps; i++)
            {
                _apiFactory
                   .ChangeContext<ILoginPageUi>(_driver)
                   .LoginPipe(_secondUserName, password: "WrongPassword");
            }

            // release blocked client
            _apiFactory
                .ChangeContext<ISecurityTubApi>()
                .PatchReleaseBlockUserRequest(_crmUrl, _blockUserId,  _currentUserApiKey);
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
        public void VerifyGlobalEventForBlockAndReleaseUserApiTest()
        {
            var expectedTypeList = new List<string>()
            { { "active_erp_user" }, { "create_erp_user" }};

            var expectedGlobal = true;
            var expectedActionMadeByUser = _expectedUserName;
            var expectedActionMadeByUserId = _expectedUserId;
            var expectedUserFullName = $"{_expectedUserName}  {_expectedUserName}";
            var actualTypeList = new List<string>();
            var actualGlobalList = new List<bool>();
            var actualActionMadeByUserIdList = new List<string>();
            var actualActionMadeByList = new List<string>();
            var actualUserFullNameList = new List<string>();
            var actualUserIdList = new List<string>();

            // get global event for create and releases 
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _expectedUserName, _currentUserApiKey);

            // get global event for block
            var actualGlobalForBlock = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, "System")
               .Where(p => p.user_full_name == $"{_secondUserName} {_secondUserName}");

            actualGlobals.ForEach(p => actualTypeList.Add(p.type));
            actualGlobals.ForEach(p => actualGlobalList.Add(p.global));
            actualGlobals.ForEach(p => actualActionMadeByUserIdList.Add(p.action_made_by_user_id));
            actualGlobals.ForEach(p => actualActionMadeByList.Add(p.action_made_by));
            actualGlobals.ForEach(p => actualUserIdList.Add(p.user_id));

            Assert.Multiple(() =>
            {
                Assert.True(actualGlobalForBlock.FirstOrDefault().user_id == _blockUserId,
                     $" actual block User Id : {actualGlobalForBlock.FirstOrDefault().user_id}" +
                     $" expected block User Id: {_blockUserId}");

                Assert.True(actualUserIdList.All(p => p.Equals(_blockUserId)),
                     $" actual User Id : {actualUserIdList.ListToString()}" +
                     $" expected User Id: {_blockUserId}");

                Assert.True(actualActionMadeByUserIdList.All(p => p.Equals(_expectedUserId)),
                     $" actual Action Made By User Id : {actualActionMadeByUserIdList.ListToString()}" +
                     $" expected Action Made By User Id: {_expectedUserId}");

                Assert.True(actualActionMadeByList.All(p => p.Equals(_expectedUserName)),
                     $" actual Action Made By  : {actualActionMadeByList.ListToString()}" +
                     $" expected Action Made By : {_expectedUserName}");

                Assert.True(actualTypeList.CompareTwoListOfString(expectedTypeList).Count == 0,
                     $" actual Type List : {actualTypeList.ListToString()}" +
                     $" expected type List: {expectedTypeList.ListToString()}");

                Assert.True(actualGlobalForBlock.FirstOrDefault().global.Equals(true),
                     $" actual Global for block list : {actualGlobalForBlock.FirstOrDefault().global}" +
                     $" expected Global for block  list: {expectedGlobal}");
            });
        }
    }
}