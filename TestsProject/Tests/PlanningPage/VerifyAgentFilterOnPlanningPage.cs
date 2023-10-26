using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.PlanningPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyAgentFilterOnPlanningPage : TestSuitBase
    {
        #region Test Preparation
        public VerifyAgentFilterOnPlanningPage(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private string _comment; 
        private string _userNameForFirstComment;
        private string _userNameForSecondComment; 
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();
            _comment = "Filter comment by agent";

            // create user For First Comment
            _userNameForFirstComment = TextManipulation.RandomString();

           var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userNameForFirstComment);

            // create ApiKey For First Comment
            var userApiKeyForFirstComment = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create user For second Comment
            _userNameForSecondComment = TextManipulation.RandomString();

            userId = _apiFactory
                 .ChangeContext<IUserApi>(_driver)
                 .CreateUserForUiPipe(_crmUrl, _userNameForSecondComment);

            // create ApiKey
            var userApiKeyForSecondComment = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: userApiKeyForFirstComment);

            // create First Comment with first user
            _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .PostCommentRequest(_crmUrl, clientId, _comment,
                userApiKeyForFirstComment);

            // create second Comment with second user
            _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .PostCommentRequest(_crmUrl, clientId, _comment,
                userApiKeyForSecondComment);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userNameForFirstComment);
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
        [Description("Based on jira: https://airsoftltd.atlassian.net/browse/AIRV2-5045")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyAgentFilterOnPlanningPageTest()
        {
            var mailPerfix = DataRep.EmailPrefix;
            var userName = TextManipulation.RandomString();
            var email = userName + mailPerfix.ToLower();

            var actualNumOfComments = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IPlanningPageUi>()
                .SelectAgentPipe(_userNameForFirstComment)
                .SearchComment(_comment);

            Assert.Multiple(() => 
            {
                Assert.True(actualNumOfComments == 1,
                    $" expected Num Of Comments: 1, " +
                    $" actual Num Of Comments: {actualNumOfComments}");
            });
        }
    }
}
