using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.GetAuditTradeGroupResponse;

namespace TestsProject.Tests.ClientsPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyCreateClient : TestSuitBase
    {
        #region Test Preparation
        public VerifyCreateClient(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _mailPerfix = DataRep.EmailPrefix;
        private string _browserName;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();

            var userName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyCreateClientTest()
        {            
            var firstName = TextManipulation.RandomString();
            var email = firstName + _mailPerfix;
            // 61 characters allowed only 60
            var clientNameExeededCharacter = "VerifySearchTradeInCloseTradesVerifySearchTradeInCloseTradesI";
            var expectedFirstNameErrorMessage = "\"first_name\" length must be less than or equal to 60 characters long";
            var expectedLastNameErrorMessage = "\"last_name\" length must be less than or equal to 60 characters long";
            var expectedEmailNameErrorMessage = "\"email\" length must be less than or equal to 60 characters long";

            // create client
            var clientDitails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .ChangeContext<IClientsPageUi>(_driver)
                .ClickOnCreateClientButton()
                .CreateClientUiPipe(email)
                .SearchClientByEmail(email)
                .GetSearchResultDetails<SearchResultClients>()
                .First();

            firstName = TextManipulation.RandomString();

            // create client with long first name
            var errorMessage = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl,
                clientNameExeededCharacter, checkStatusCode: false);

            // get the error message
            var actualFirstNameErrorMessage = JsonConvert
                .DeserializeObject<GeneralDto>(errorMessage)
                .first_name
                .FirstOrDefault();

            // create client with long last name
            errorMessage = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, firstName,
                lastName: clientNameExeededCharacter, checkStatusCode: false);

            // get the error message
            var actualFLastNameErrorMessage = JsonConvert
                .DeserializeObject<GeneralDto>(errorMessage)
                .last_name
                .FirstOrDefault();

            // create client with long email
            errorMessage = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, firstName,
                emailSuffix: clientNameExeededCharacter, checkStatusCode: false);

            // get the error message
            var actualEmailErrorMessage = JsonConvert
                .DeserializeObject<GeneralDto>(errorMessage)
                .email
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(clientDitails.email == email,
                    $" expected client email: {email}, " +
                    $" actual client email: {clientDitails.email}");

                Assert.True(actualFirstNameErrorMessage == expectedFirstNameErrorMessage,
                    $" expected First Name Error Message: {expectedFirstNameErrorMessage}, " +
                    $" actual First Name Error Message: {actualFirstNameErrorMessage}");

                Assert.True(actualFLastNameErrorMessage == expectedLastNameErrorMessage,
                    $" expected last Name Error Message: {expectedLastNameErrorMessage}, " +
                    $" actual last Name Error Message: {actualFLastNameErrorMessage}");

                Assert.True(actualEmailErrorMessage == expectedEmailNameErrorMessage,
                    $" expected email Error Message: {expectedEmailNameErrorMessage}, " +
                    $" actual email Error Message: {actualEmailErrorMessage}");
            });
        }
    }
}
