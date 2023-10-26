using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TempTests
{
    [TestFixture(DataRep.Chrome)]
    public class Examples : TestSuitBase
    {
        #region Test Preparation
        public Examples(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userName;
        private string _clientNameForDeposit;
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            //BeforeTest(_browserName);

            #region PreCondition

            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            //try
            //{
            //}
            //finally
            //{
            //    AfterTest(_driver);
            //    DriverDispose(_driver);
            //}
        }
        #endregion

        [Test]
        //[RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyFirstTimeDepositInBankingTest()
        {
            _driver = GetDriver();

            // create user
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName);

            // create client For Deposit
            _clientNameForDeposit = TextManipulation.RandomString();

            var clientIdForDeposit = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientNameForDeposit);

            // create client
            var clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            var clientsIdsForDeposit = new List<string> { clientIdForDeposit };

            _apiFactory
                .ChangeContext<IClientsApi>()
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, clientIdForDeposit, 10);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IDepositsPageUi>()
                .WaitForDepositTableToLoad();

            var depositDitails = _apiFactory
                  .ChangeContext<IHandleFiltersUi>(_driver)
                  .ClickToOpenFiltersMenu()
                  .MultiSelectDropDownPipe(DataRep.CampaignsFilter, "klk")
                  .VerifyFirstDepositFlag()
                  .GetSearchResultDetails<SearchResultDepositsBanking>().First();

            Assert.Multiple(() =>
            {

            });
        }
    }
}