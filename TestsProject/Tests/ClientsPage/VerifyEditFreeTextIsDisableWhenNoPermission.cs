using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyEditFreeTextIsDisableWhenNoPermission : TestSuitBase
    {
        public VerifyEditFreeTextIsDisableWhenNoPermission(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;
        private string _browserName;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition   
            _driver = GetDriver();

            // get brand regulation
            var brandRegulation = _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .GetBrandRegulationRequest(_crmUrl);


            brandRegulation.edit_free_text = true;

            // update brand regulation
            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutRegulationsRequest(_crmUrl, brandRegulation);

            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName,
               role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create client 
            var clientName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: currentUserApiKey);

            // login and click on edit free text button
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .ClickOnEditFreeTextBtn();           
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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyEditFreeTextIsDisableWhenNoPermissionTest()
        {
            var expectedNumOfDisabledFileds = 4;
            var expectedFreeText = "free text automation";
            var freeTextFiled = "free_text";

            // check if the free text except the first line is disabled
            var numOfFreeTextThatIsDisaable = _apiFactory
                .ChangeContext<IClientsPageUi>(_driver)
                .CheckIfFreeTextIsDisable();

            // set text to free text first line
            _apiFactory
                .ChangeContext<IClientsPageUi>(_driver)
                .SetFreeText(freeTextFiled, expectedFreeText)
                .ClickOnSaveFreeTextButton();

            // click on edit free text button to check the new text
            var actualFreeText = _apiFactory
                .ChangeContext<IClientsPageUi>(_driver)
                .ClickOnEditFreeTextBtn()
                .GetFreeText(freeTextFiled);

            Assert.Multiple(() =>
            {
                Assert.True(numOfFreeTextThatIsDisaable == expectedNumOfDisabledFileds,
                    $" expected Num Of Disable free text Fields: {expectedNumOfDisabledFileds}" +
                    $" actual Num Of Disable free text Fields: {numOfFreeTextThatIsDisaable}");

                Assert.True(actualFreeText == expectedFreeText,
                    $" expected Free Text: {expectedFreeText}" +
                    $" actual Free Text: {actualFreeText}");
            });
        }
    }
}