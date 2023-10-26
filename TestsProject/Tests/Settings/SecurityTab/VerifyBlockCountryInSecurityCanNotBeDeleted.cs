using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Settings.SecurityTab
{
    [TestFixture(DataRep.Chrome)]
    //[TestFixture(DataRep.Firefox)]
    public class VerifyBlockCountryInSecurityCanNotBeDeleted : TestSuitBase
    {
        #region Test Preparation
        public VerifyBlockCountryInSecurityCanNotBeDeleted(string browser) : base(browser)
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
            #region PreCondition
            _driver = GetDriver();
            BeforeTest(_browserName);

            var userName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IGeneralTabUi>(DataRep.CrmSettingsMenuItem)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ISecurityTabUi>(DataRep.SecurityTabUiMenuItem);
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

        // based on bug https://airsoftltd.atlassian.net/browse/AIRV2-3290
        // verify that default block country in security
        // cant be deleted by non super admin user 
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyBlockCountryInSecurityCanNotBeDeletedTest()
        {          
            var expectedBlockMessage = "You don't have permission to remove US country. Please contact your account manager.";

            var actualRegisterMessage = _apiFactory
                .ChangeContext<ISecurityTabUi>(_driver)
                .ClickOnRemoveRegisterBlockCountryButton()
                .GetRegisterBlockMessage();

            var actualLoginMessage = _apiFactory
                .ChangeContext<ISecurityTabUi>(_driver)
                .ClickOnRemoveLoginBlockCountryButton()
                .GetLoginBlockMessage();

            Assert.Multiple(() =>
            {
                Assert.True(actualRegisterMessage == expectedBlockMessage,
                    $" expected Register Message: {expectedBlockMessage}" +
                    $" actual Register Message: {actualRegisterMessage}");

                Assert.True(actualLoginMessage == expectedBlockMessage,
                    $" expected login Message: {expectedBlockMessage}" +
                    $" actual login Message: {actualLoginMessage}");
            });
        }
    }
}