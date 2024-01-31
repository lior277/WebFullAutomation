using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using DocumentFormat.OpenXml.Drawing;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage.Filters
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyHideAndUnHideFilterOnClientsPage : TestSuitBase
    {
        public VerifyHideAndUnHideFilterOnClientsPage(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region members
        private string _browserName;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IWebDriver _driver;
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        #endregion      

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();
            #region PreCondition

            // create user
            var userName = TextManipulation.RandomString();

           _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);
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
        [Description("Based on https://airsoftltd.atlassian.net/browse/AIRV2-5329")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyHideAndUnHideFilterOnClientsPageTest()
        {
            var hiddeFilterName = "Sales agents";

            // clients page
            var filterNotExist = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .ChangeContext<IHandleFiltersUi>(_driver)
                .ClickToOpenFiltersMenu()
                .ClickOnHideFilterBtn(DataRep.SalesAgentsFilter)
                .CheckIfFilterExist(DataRep.SalesAgentsFilter) == 0;

            var filterExist = _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                .ClickOnHiddenFiltersBtn()
                .CheckBoxFilterOnHiddenFiltersWindow(hiddeFilterName)
                .ClickOnApplyBtn()
                .CheckIfFilterExist(DataRep.SalesAgentsFilter) == 1;

            Assert.Multiple(() =>
            {
                Assert.True(filterNotExist,
                    $" expected filter Not Exist: true" +
                    $" actual expected filter Not Exist :  {filterNotExist}");

                Assert.True(filterExist,
                    $" expected filter Exist: true" +
                    $" actual expected filter Exist :  {filterExist}");
            });
        }
    }
}
