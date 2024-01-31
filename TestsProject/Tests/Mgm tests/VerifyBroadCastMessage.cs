using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using AirSoftAutomationFramework.Objects.MgmObjects.Api;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Mgm_tests
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyBroadCastMessage : TestSuitBase
    {
        public VerifyBroadCastMessage(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;     
        private string _browserName;
        private GetLoginResponse _mgmLoginData;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _driver = GetDriver();

            _mgmLoginData = _apiFactory
                .ChangeContext<IMgmCreateUserApi>()
                .PostMgmLoginCookiesRequest(DataRep.MgmUrl,
                DataRep.MgmUserName.Split('@').First());
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
        [Category(DataRep.MgmSanityCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyBroadCastMessageTest()
        {
            var expectedBroadCastData = "Automation Message";

            var adminUserName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, adminUserName);

            var agentUserName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, agentUserName, role: "agent");

            _apiFactory
                .ChangeContext<IBroadcastMessageApi>()
                .MgmPostCreateBroadcastMessageRequest(DataRep.MgmUrl, _mgmLoginData);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(adminUserName);

            _apiFactory
                .ChangeContext<IBroadcastMessageUi>(_driver)
                .VerifyBroadCastTitle(expectedBroadCastData);

            _apiFactory
                .ChangeContext<IBroadcastMessageUi>(_driver)
                .VerifyBroadCastTitle(expectedBroadCastData)
                .ClickOnGotItButton();

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .ClickOnLogOut();

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(agentUserName);

            _apiFactory
                .ChangeContext<IBroadcastMessageUi>(_driver)
                .VerifyBroadCastTitle(expectedBroadCastData);

            _apiFactory
                .ChangeContext<IBroadcastMessageUi>(_driver)
                .VerifyBroadCastTitle(expectedBroadCastData);          
        }
    }
}