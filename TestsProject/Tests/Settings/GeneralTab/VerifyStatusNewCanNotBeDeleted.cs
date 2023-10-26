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

namespace TestsProject.Tests.Settings.GeneralTab
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyStatusNewCanNotBeDeleted : TestSuitBase
    {
        #region Test Preparation
        public VerifyStatusNewCanNotBeDeleted(string browser) : base(browser)
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
            BeforeTest(_browserName);
            _driver = GetDriver();

            // create admin user  
            var userName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IGeneralTabUi>(DataRep.CrmSettingsMenuItem);
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

        // based on bug https://airsoftltd.atlassian.net/browse/AIRV2-4591
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyStatusNewCanNotBeDeletedTest()
        {
            var expectedNumOfButtonsOfLeadsStatusForNew = 1;

            var actualNumOfButtonsOfLeadsStatusForNew = _apiFactory
                .ChangeContext<IGeneralTabUi>(_driver)
                .GetNumOfButtonsOfLeadsStatusForNew();

            Assert.Multiple(() =>
            {
                Assert.True(actualNumOfButtonsOfLeadsStatusForNew
                    == expectedNumOfButtonsOfLeadsStatusForNew,
                    $" expected Register Message: {expectedNumOfButtonsOfLeadsStatusForNew}" +
                    $" actual Register Message: {actualNumOfButtonsOfLeadsStatusForNew}");
            });
        }
    }
}