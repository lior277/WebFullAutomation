using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.UsersMenuUi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Linq;
using System.Text.Json.Nodes;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.UsersPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyCreateUser : TestSuitBase
    {
        #region Test Preparation
        public VerifyCreateUser(string browser) : base(browser)
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
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            var userName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);
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
        public void VerifyCreateUserTest()
        {
            var mailPerfix = DataRep.EmailPrefix;
            var userName = TextManipulation.RandomString();
            var email = userName + mailPerfix.ToLower();
            var exeededCharacter = "VerifySearchTradeInCloseTradesVerifySearchTradeInCloseTradesl";
            var excpectedFirstNameErrorMessage = "first_name length must be less than or equal to 60 characters long";
            var excpectedLastNameErrorMessage = "last_name length must be less than or equal to 60 characters long";
            var excpectedUserNameErrorMessage = "username length must be less than or equal to 60 characters long";
            var excpectedEmailErrorMessage = "email length must be less than or equal to 60 characters long";

            var excpectedPhoneErrorMessage = "phone can contain only numbers and the following symbols +-()" +
                ". Minimum length is 6 characters. Maximum length is 20 characters. ";

            //var excpectedExtentionErrorMessage = "extension can contain only numbers Maximum length is 7 characters.";

            var userDitails = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IUsersPageUi>()
                .ClickOnCreateUserButton()
                .CreateUserUiPipe(_crmUrl, userName)
                .SearchUser(userName)
                .GetSearchResultDetails<SearchResultUsers>()
                .First();

            // create user with long first name
            var createUserErrorMessage = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName: exeededCharacter,
                phone: exeededCharacter, checkStatusCode: false);

            var actualFirstNameErrorMessage = JsonConvert
                .DeserializeObject<GeneralDto>(createUserErrorMessage)
                .first_name
                .FirstOrDefault();

            var actualLastNameErrorMessage = JsonConvert
                .DeserializeObject<GeneralDto>(createUserErrorMessage)
                .last_name
                .FirstOrDefault();

            var actualUserNameErrorMessage = JsonConvert
                .DeserializeObject<GeneralDto>(createUserErrorMessage)
                .username
                .FirstOrDefault();

            var actualEmailErrorMessage = JsonConvert
                .DeserializeObject<GeneralDto>(createUserErrorMessage)
                .email
                .FirstOrDefault();

            var actualPhoneErrorMessage = JsonConvert
                .DeserializeObject<GeneralDto>(createUserErrorMessage)
                .phone
                .FirstOrDefault();

            //var actualExtensionErrorMessage = JsonConvert
            //    .DeserializeObject<GeneralDto>(createUserErrorMessage)
            //    .extension
            //    .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(userDitails.firstname == userName.UpperCaseFirstLetter(),
                    $" expected user first name: {userName}, " +
                    $"actual user first name: {userDitails.firstname}");

                Assert.True(userDitails.lastname == userName.UpperCaseFirstLetter(),
                    $" expected user last name: {userName}, " +
                    $" actual user last name: {userDitails.lastname}");

                Assert.True(userDitails.username == userName,
                    $" expected user name: {userName}, " +
                    $" actual user name: {userDitails.username}");

                Assert.True(userDitails.username == userName,
                   $" expected user name: {userName}, " +
                   $" actual user name: {userDitails.username}");

                Assert.True(userDitails.email == email,
                    $" expected user email: {email}, " +
                    $" actual user email: {userDitails.email}");

                Assert.True(userDitails.role == "admin",
                    $" expected user role: admin, " +
                    $" actual user role: {userDitails.role}");

                Assert.True(userDitails.affiliate == "False",
                    $" expected user affiliate: false, " +
                    $" actual user affiliate: {userDitails.affiliate}");

                Assert.True(actualFirstNameErrorMessage == excpectedFirstNameErrorMessage,
                    $" expected First Name Error Message: {excpectedFirstNameErrorMessage} " +
                    $" actual First Name Error Message: {actualFirstNameErrorMessage}");

                Assert.True(actualLastNameErrorMessage == excpectedLastNameErrorMessage,
                    $" expected Last Name Error Message: {excpectedLastNameErrorMessage} " +
                    $" actual Last Name Error Message: {actualLastNameErrorMessage}");

                Assert.True(actualUserNameErrorMessage == excpectedUserNameErrorMessage,
                    $" expected user Name Error Message: {excpectedUserNameErrorMessage} " +
                    $" actual user Name Error Message: {actualUserNameErrorMessage}");

                Assert.True(actualEmailErrorMessage == excpectedEmailErrorMessage,
                    $" expected Email Error Message: {excpectedEmailErrorMessage} " +
                    $" actual Email Error Message: {actualEmailErrorMessage}");

                Assert.True(actualPhoneErrorMessage.Contains(excpectedPhoneErrorMessage),
                    $" expected Phone Error Message: {excpectedPhoneErrorMessage} " +
                    $" actual Phone Error Message: {actualPhoneErrorMessage}");

                //Assert.True(actualExtensionErrorMessage.Contains(excpectedExtentionErrorMessage),
                //    $" expected Extension Error Message: {excpectedExtentionErrorMessage} " +
                //    $" actual Extension Error Message: {actualExtensionErrorMessage}");
            });
        }
    }
}
