// Ignore Spelling: Crm

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.UsersMenuUi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyCrmPasswordValidation : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config
            .appSettings.tradingPlatformUrl;

        private IWebDriver _driver;
        private string _browserName;
        private string _userEmail;

        public VerifyCrmPasswordValidation(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();

            // create user for the creation of api key
            var userName = TextManipulation.RandomString();
            _userEmail = userName + DataRep.TestimEmailPrefix;

            // create user
            var userId = _apiFactory
                 .ChangeContext<IUserApi>(_driver)
                 .CreateUserForUiPipe(_crmUrl, userName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName, _crmUrl);
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
        public void VerifyCrmPasswordValidationTest()
        {
            var passwordOptions = new List<string>() { "passwor",
                "passwordNew", "passwordN8w" };

            var expectedPasswordValidation = new Dictionary<string, bool>(){ { "passwor", true },
                { "passwordNew", true}, {"passwordN8w", true } };

            var actualValidPassword = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IUsersPageUi>()
                .ClickOnCreateUserButton()
                .ChangeContext<IUserUi>(_driver)
                .CheckIfPasswordValid(passwordOptions);

            //var actualValidPassword = _apiFactory
            //    .ChangeContext<ILoginPageUi>(_driver)
            //    .NavigateToUrl(_resetPasswordErpLink)
            //    .CheckIfPasswordValid(passwordOptions);

            var actualGreenCercles = _apiFactory
                .ChangeContext<IUserUi>(_driver)
                .CheckPasswordPolicy();

            var differance = actualValidPassword.Where(entry => expectedPasswordValidation[entry.Key]
            != entry.Value)
                .ToDictionary(entry => entry.Key, entry => entry.Value)
                .Count;

            Assert.Multiple(() =>
            {
                Assert.True(differance == 0,
                    $" expected Valid Password: {expectedPasswordValidation.DictionaryToString()}" +
                    $"actual Valid Password: {actualValidPassword.DictionaryToString()}");

                Assert.True(actualGreenCercles == 4,
                    $" expected Green Cercles: 4" +
                    $"actual Green Cercles: {actualGreenCercles}");
            });
        }
    }
}