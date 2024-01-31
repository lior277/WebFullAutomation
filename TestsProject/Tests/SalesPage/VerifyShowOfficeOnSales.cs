using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.SalesPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyShowOfficeOnSales : TestSuitBase
    {
        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userName;
        private string _airsoftOffice = "Airsoft";
        private string _mainOffice = "Main Office";
        private int _depositAmount = 400;
        private IWebDriver _driver;
        private string _browserName;
        #endregion

        public VerifyShowOfficeOnSales(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);

            #region PreCondition
            _driver = GetDriver();

            // get airsoft office data
            var airsoftOffice = _apiFactory
               .ChangeContext<IOfficeTabApi>(_driver)
               .GetOfficesByName(_crmUrl, _airsoftOffice);

            // create user with airsoft office
            var userName = TextManipulation.RandomString();

            // create user
            var airsoftOfficeUserId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName,
                officeData: airsoftOffice);

            // get main Offices data
            var mainOffice = _apiFactory
                .ChangeContext<IOfficeTabApi>(_driver)
                .GetOfficesByName(_crmUrl);

            // create user with main Offices

            userName = TextManipulation.RandomString();

            // create user
            var mainOfficeUserId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl,
                userName, officeData: mainOffice);

            var subUsersIds = new string[] { airsoftOfficeUserId,
                mainOfficeUserId };

            // create user
            _userName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName,
                role: DataRep.AdminWithUsersOnlyRoleName, subUsers: subUsersIds);

            // create client For Airsoft User 
            var clientName = TextManipulation.RandomString();

            var firstClientIdAirsoftOffice = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, currency: "EUR");

            // create client for conversion
            clientName = TextManipulation.RandomString();

            var secondClientIdAirsoftOffice = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, currency: "EUR");

            var clientsIds = new List<string> 
            { firstClientIdAirsoftOffice, secondClientIdAirsoftOffice };

            // connect Airsoft User To Client
            _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .PatchMassAssignSaleAgentsRequest(_crmUrl,
               airsoftOfficeUserId, clientsIds);

            // deposit 400
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, firstClientIdAirsoftOffice,
                _depositAmount, originalCurrency: "EUR");

            // create client for main office user
            clientName = TextManipulation.RandomString();

            var clientIdMainOffice = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                currency: "EUR");

            clientsIds = new List<string> { clientIdMainOffice };

            // connect Main Office User To Client
            _apiFactory
               .ChangeContext<IClientsApi>(_driver)
               .PatchMassAssignSaleAgentsRequest(_crmUrl, mainOfficeUserId,
               clientsIds);

            // deposit 400
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, clientIdMainOffice,
                _depositAmount, originalCurrency: "EUR");

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);
        }
        #endregion

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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyShowOfficeOnSalesTest()
        {
            var expectedDepositFilterData = new List<string>()
            { $"{_airsoftOffice}", "0%", {_mainOffice}, "0%", "1.00", "1.00" };

            var expectedConversionFilterData = new List<string>()
            { $"{_airsoftOffice}", "0%", { _mainOffice }, "0%", "100.0", "50.0" };

            var expectedTotalDepositFilterData = new List<string>()
            { $"{_airsoftOffice}", "0%", { _mainOffice }, "0%", "400.00", "400.00" };

            var actualDepositFilterData = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ISalesPageUi>()
                .ChangeContext<ISalesPageUi>(_driver)
                .ClickOnShowOffice()
                .GetSalesProgressData();

            var actualConversionFilterData = _apiFactory
                .ChangeContext<ISalesPageUi>(_driver)
                .ClickOnSalesProgressFilterByName(DataRep.ProgressFilterConversion)
                .GetSalesProgressData();

            var actualTotalDepositFilterData = _apiFactory
                .ChangeContext<ISalesPageUi>(_driver)
                .ClickOnSalesProgressFilterByName(DataRep.ProgressFilterTotalDeposit)
                .GetSalesProgressData();

            Assert.Multiple(() =>
            {
                Assert.True(actualDepositFilterData.CompareTwoListOfString(expectedDepositFilterData).Count() == 0,
                    $" the difference between the lists of Deposit Filter: " +
                    $" {actualDepositFilterData.CompareTwoListOfString(expectedDepositFilterData).ListToString()}");

                Assert.True(actualConversionFilterData.CompareTwoListOfString(expectedConversionFilterData).Count() == 0,
                    $" the difference between the lists of Conversion Filter: " +
                    $" {actualConversionFilterData.CompareTwoListOfString(expectedConversionFilterData).ListToString()},");

                Assert.True(actualTotalDepositFilterData.CompareTwoListOfString(expectedTotalDepositFilterData).Count() == 0,
                    $" the difference between the lists of Total Deposit Filter: " +
                    $" {actualTotalDepositFilterData.CompareTwoListOfString(expectedTotalDepositFilterData).ListToString()}");
            });
        }
    }
}
