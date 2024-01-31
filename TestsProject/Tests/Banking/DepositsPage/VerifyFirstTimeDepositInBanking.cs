using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
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

namespace TestsProject.Tests.Banking.DepositsPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyFirstTimeDepositInBanking : TestSuitBase
    {
        #region Test Preparation
        public VerifyFirstTimeDepositInBanking(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userName;
        private string _clientNameForDeposit;   
        private string _campaignId;
        private string _campaignName;
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);

            #region PreCondition
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

            // create campaign and Affiliate
            var campaignIdAndName = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .CreateAffiliateAndCampaignApi(_crmUrl);

            var clientsIdsForDeposit = new List<string> { clientIdForDeposit };
            var allclienstIds = new List<string> { clientId };
            allclienstIds.AddRange(clientsIdsForDeposit);

            _campaignId = campaignIdAndName.Values.First();
            _campaignName = campaignIdAndName.Keys.First();

            _apiFactory
                 .ChangeContext<IClientsApi>()
                 .PatchMassAssignCampaign(_crmUrl,
                 allclienstIds.ToList(), _campaignId);

            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, userId,
                clientsIdsForDeposit);

           _apiFactory
                .ChangeContext<IClientsApi>()
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, clientIdForDeposit, 10);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyFirstTimeDepositInBankingTest()
        {
            var chooseCampaignName =  _campaignName;
            var date = DateTime.Now.ToString("dd/MM/yyyy");

            var depositClientName = _clientNameForDeposit
                .UpperCaseFirstLetter();

            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IDepositsPageUi>();

            var ff = _apiFactory
                .ChangeContext<IHandleFiltersUi>(_driver)
                //.WaitForTableToLoad()
                .ClickToOpenFiltersMenu()
                .MultiSelectDropDownPipe(DataRep.CampaignsFilter, chooseCampaignName)
                .VerifyFirstDepositFlag();

            var depositDitails = ff
                .GetSearchResultDetails<SearchResultDepositsBanking>()
                .First();

            Assert.Multiple(() =>
            {              
                Assert.True(depositDitails?.assignedto.Contains(_userName),
                    $" expected assigned to: {_userName}" +
                    $" actual assigned to: {depositDitails.assignedto}");

                Assert.True(depositDitails?.dateofdepositgmt.Contains(date),
                    $" expected date of deposit: {date}" +
                    $" actual date of deposit : {depositDitails.dateofdepositgmt}");

                Assert.True(depositDitails?.clientfullname.Contains(depositClientName),
                    $" expected client full name: {depositClientName}" +
                    $" actual client full name: {depositDitails.clientfullname}");

                Assert.True(depositDitails?.email.Contains(depositClientName.ToLower()),
                    $"expected email: {depositClientName}" +
                    $" actual email: {depositDitails.email}");

                Assert.True(depositDitails?.totaldeposits == "10.00",
                    $"expected total deposits: 10.00" +
                    $" actual total deposits: {depositDitails.totaldeposits}");

                Assert.True(depositDitails?.depositamount == "10.00",
                    $"expected deposit amount: 10.00" +
                    $" actual deposit amount: {depositDitails.depositamount}");

                Assert.True(depositDitails?.currency == DataRep.DefaultUSDCurrencyName,
                    $"expected currency: {DataRep.DefaultUSDCurrencyName}" +
                    $" actual currency: {depositDitails.currency}");

                Assert.True(depositDitails?.displayname == "bank transfer",
                    $"expected bank transfer: bank transfer" +
                    $" actual bank transfer: {depositDitails.displayname}");
            });
        }
    }
}