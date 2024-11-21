using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using AirSoftAutomationFramework.Objects.MgmObjects.Ui.Dashboard;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyBroadCastMessageAndForward : TestSuitBase
    {
        public VerifyBroadCastMessageAndForward(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;     
        private string _browserName;
        private string _firstUserName;
        private string _secondUserName;
        private string _thirdUserName;
        private string _expectedBroadCastData;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _driver = GetDriver();

            _expectedBroadCastData = "Automation Message";

            _firstUserName = TextManipulation.RandomString();
            _secondUserName = TextManipulation.RandomString();
            _thirdUserName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _firstUserName);

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _secondUserName);

            _apiFactory
              .ChangeContext<IUserApi>(_driver)
              .CreateUserForUiPipe(_crmUrl, _thirdUserName, role: "agent");

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_firstUserName);
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


        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyBroadCastMessageAndForwardTest()
        {
            _apiFactory
                .ChangeContext<IBroadcastMessageUi>(_driver)
                .ClickOnBroadcastButton()
                .SearchUser(_secondUserName)
                .CheckSelectedUser(_secondUserName)
                .ClickOnNextButton()
                .SetSubject(_expectedBroadCastData)
                .SetMessage(_expectedBroadCastData)
                .ClickOnNextButton()
                .ClickOnSendButton();

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .ClickOnLogOut();

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_secondUserName);

            _apiFactory
                .ChangeContext<IBroadcastMessageUi>(_driver)
                .VerifyBroadCastTitle(_expectedBroadCastData);

            _apiFactory
                .ChangeContext<IBroadcastMessageUi>(_driver)
                .VerifyBroadCastBody(_expectedBroadCastData);

            _apiFactory
                .ChangeContext<IBroadcastMessageUi>(_driver)
                .ClickOnForewordButton()
                .SearchUser(_thirdUserName)
                .CheckSelectedUser(_secondUserName)
                .ClickOnNextButton()
                .ClickOnNextButton()
                .ClickOnSendButton()
                .ClickOnGotItButton();

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .ClickOnLogOut();

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_thirdUserName);

            _apiFactory
                .ChangeContext<IBroadcastMessageUi>(_driver)
                .VerifyBroadCastTitle(_expectedBroadCastData);

            _apiFactory
                .ChangeContext<IBroadcastMessageUi>(_driver)
                .VerifyBroadCastBody(_expectedBroadCastData);
        }
    }
}