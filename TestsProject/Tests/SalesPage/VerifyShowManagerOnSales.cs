using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
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
    public class VerifyShowManagerOnSales : TestSuitBase
    {
        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private int _depositAmount = 400;
        private IWebDriver _driver;
        private string _browserName;
        private string _userName; 
        #endregion

        public VerifyShowManagerOnSales(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);

            #region PreCondition
            _driver = GetDriver();

            // create Salas Agent Salary
            var salesAgentSalaryId = _apiFactory
                 .ChangeContext<ISalesTabApi>(_driver)
                 .CreateSalasAgentSalaryPipe(_crmUrl, _depositAmount);

            // create sub user

            var userName = TextManipulation.RandomString();

            var subUserId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            // create user
            _userName = TextManipulation.RandomString();
            var subUsersIds = new string[] { subUserId };

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName,
                role: DataRep.AdminWithUsersOnlyRoleName, subUsers: subUsersIds,
                salesAgentSalaryId: salesAgentSalaryId);

            // create first client 
            var clientName = TextManipulation.RandomString();

            var firstClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl,
                clientName, currency: "EUR");

            // create second client for conversion
            clientName = TextManipulation.RandomString();

            var secondClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl,
                clientName, currency: "EUR");

            var clientsIds = new List<string> { firstClientId, secondClientId };


            // connect Airsoft User To Client
            _apiFactory
               .ChangeContext<IClientsApi>(_driver)
               .PatchMassAssignSaleAgentsRequest(_crmUrl,
               subUserId, clientsIds);

            // deposit 400
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, firstClientId,
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
        public void VerifyShowManagerOnSalesTest()
        {
            var expectedDepositFilterData = new List<string>()
            { $"*{_userName}", "0%", "1" };

            var expectedConversionFilterData = new List<string>()
            { $"*{_userName}", "0%", "50.00" };

            var expectedTotalDepositFilterData = new List<string>()
            { $"*{_userName}", "0%", "400.00" };

            var actualDepositFilterData = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ISalesPageUi>()
                .ChangeContext<ISalesPageUi>(_driver)
                .ClickOnShowManager()
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
                    $" the differance between the lists of Deposit Filter: " +
                    $" {actualDepositFilterData.CompareTwoListOfString(expectedDepositFilterData).ListToString()}");

                Assert.True(actualConversionFilterData.CompareTwoListOfString(expectedConversionFilterData).Count() == 0,
                    $" the differance between the lists of Conversion Filter: " +
                    $" {actualConversionFilterData.CompareTwoListOfString(expectedConversionFilterData).ListToString()},");

                Assert.True(actualTotalDepositFilterData.CompareTwoListOfString(expectedTotalDepositFilterData).Count() == 0,
                    $" the differance between the lists of Total Deposit Filter: " +
                    $" {actualTotalDepositFilterData.CompareTwoListOfString(expectedTotalDepositFilterData).ListToString()}");
            });

        }
    }
}
