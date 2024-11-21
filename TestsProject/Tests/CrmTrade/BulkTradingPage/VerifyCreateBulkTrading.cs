// Ignore Spelling: Crm

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.CrmTrade.BulkTradingPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyCreateBulkTrading : TestSuitBase
    {
        #region Test Preparation
        public VerifyCreateBulkTrading(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _firstClientFullName;
        private string _secondClientFullName;
        private string _firstClientId;
        private string _clientNameWithoutMoney;
        private string _secondClientId;
        private string _thirdClientId;  
        private string _expectedFirstMassTradeGroup = "1";
        private string _expectedSecondMassTradeGroup = "2";
        private string _tradeExposer = "50";
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var bonusAmount = 1000000;
            var crmUrl = Config.appSettings.CrmUrl;
            _driver = GetDriver();
            var clientList = new List<string>();

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);
            #endregion

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

            // create client with no money
            _clientNameWithoutMoney = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientNameWithoutMoney, apiKey: currentUserApiKey);

            // create first client
            var clientName = TextManipulation.RandomString();
            _firstClientFullName = $"{clientName} {clientName}";

            _firstClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, apiKey: currentUserApiKey);

            clientList.Add(_firstClientId);

            // get first client card
            var firstInformationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>(_driver)
                .GetInformationTabRequest(_crmUrl, _firstClientId)
                .GeneralResponse
                .informationTab;

            firstInformationTabResponse.mass_trade = _expectedFirstMassTradeGroup;

            // update first client card with mass trade
            _apiFactory
                .ChangeContext<IInformationTabApi>(_driver)
                .PutInformationTabRequest(_crmUrl, firstInformationTabResponse,
                apiKey: currentUserApiKey);

            // create second client
            clientName = TextManipulation.RandomString();
            _secondClientFullName = $"{clientName} {clientName}";

            _secondClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, apiKey: currentUserApiKey);

            clientList.Add(_secondClientId);

            // get second client card
            var secondInformationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>(_driver)
                .GetInformationTabRequest(_crmUrl, _secondClientId)
                .GeneralResponse
                .informationTab;

            secondInformationTabResponse.mass_trade = _expectedFirstMassTradeGroup;

            // update first client card with mass trade  group
            _apiFactory
                .ChangeContext<IInformationTabApi>(_driver)
                .PutInformationTabRequest(_crmUrl, secondInformationTabResponse,
                apiKey: currentUserApiKey);           

            // create third client
            clientName = TextManipulation.RandomString();

            _thirdClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, apiKey: currentUserApiKey);

            clientList.Add(_thirdClientId);

            // get third client card
            var thirdInformationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>(_driver)
                .GetInformationTabRequest(_crmUrl, _thirdClientId)
                .GeneralResponse
                .informationTab;

            thirdInformationTabResponse.mass_trade = _expectedSecondMassTradeGroup;

            // update third client card with mass trade group
            _apiFactory
                .ChangeContext<IInformationTabApi>(_driver)
                .PutInformationTabRequest(_crmUrl, thirdInformationTabResponse,
                apiKey: currentUserApiKey);

            // create bonus
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostBonusRequest(_crmUrl, clientList, bonusAmount);
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
        public void VerifyCreateBulkTradingTest()
        {
            var date = DateTime.Now.ToString("dd/MM/yyyy");
            var expectedAssetName = DataRep.AssetName;

            // navigate to Bulk Group page
            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IBulkTradePageUi>();

            var actualClientsNamesInBulkTable = _apiFactory
               .ChangeContext<ISearchResultsUi>(_driver)
               .GetSearchResultDetails<SearchResultBulkTrading>(expectedNumOfRows: 3,
               cellsAndTitlesShouldBeEquals: false)
               .Select(p => p.fullname)
               .ToList();

            // Select Bulk Group
            _apiFactory
                .ChangeContext<IBulkTradePageUi>(_driver)
                .SelectBulkGroup(_expectedFirstMassTradeGroup);

            Thread.Sleep(500);

            // choose the clients
            _apiFactory
                .ChangeContext<IBulkTradePageUi>(_driver)
                .ClickOnClientCheckBox();

            // Click On Open Trade Button
            _apiFactory
                .ChangeContext<IBulkTradePageUi>(_driver)
                .ClickOnOpenTradeButton();

            var actualMassFullNames = _apiFactory
                .ChangeContext<IBulkTradePageUi>(_driver)
                .SelectAssetPipe(expectedAssetName)
                .SetExposer(_tradeExposer)
                .ClickOnSaveOpenTradeButton()
                .ClickOnConfirmButton()
                .GetSearchResultDetails<SearchResultBulkTradeReport>(expectedNumOfRows: 2,
                cellsAndTitlesShouldBeEquals: false);

            var actualFirstClientTradeId = _apiFactory
                .ChangeContext<ITradesTabApi>()
                .GetTradesRequest(_crmUrl, _firstClientId)
                .GeneralResponse
                .FirstOrDefault()
                .id;

            var actualSecondClientTradeId = _apiFactory
                .ChangeContext<ITradesTabApi>()
                .GetTradesRequest(_crmUrl, _secondClientId)
                .GeneralResponse
                .FirstOrDefault()
                .id;

            var actualThirdClientTradeId = _apiFactory
                .ChangeContext<ITradesTabApi>()
                .GetTradesRequest(_crmUrl, _thirdClientId)
                .GeneralResponse?
                .FirstOrDefault()?
                .id;

            Assert.Multiple(() =>
            {
                Assert.True(!actualClientsNamesInBulkTable.Contains(_clientNameWithoutMoney),
                    $" expected client name Without Money: {_clientNameWithoutMoney} " +
                    $" actual clients names: {actualClientsNamesInBulkTable.ListToString()}"); 

                Assert.True(actualMassFullNames.Any(p => p.id.Contains(_firstClientId)) &&
                    actualMassFullNames.Any(p => p.id.Contains(_secondClientId)),
                    $" expected Clients  Ids : {_firstClientId}, {_secondClientId} " +
                    $" actual Clients  Ids: {actualMassFullNames.Select(p => p.id).ListToString()}");

                Assert.True(actualMassFullNames.Any(p => p.fullname.Contains(_firstClientFullName)) &&
                    actualMassFullNames.Any(p => p.fullname.Contains(_secondClientFullName)),
                    $" expected Clients  full names : {_firstClientFullName}, {_secondClientFullName} " +
                    $" actual Clients  full names: {actualMassFullNames.Select(p => p.id).ListToString()}");

                Assert.True(actualMassFullNames.All(p => p.success == "Yes"),
                    $" expected First Client  success: Yes " +
                    $" actual First Client success: {actualMassFullNames.Select(p => p.success).ListToString()}");
       
                Assert.True(actualFirstClientTradeId != null,
                    $" expected First Client Trade Id: not null " +
                    $" actual First Client Trade Id: {actualFirstClientTradeId}");

                Assert.True(actualSecondClientTradeId != null,
                    $" expected Second Client Trade Id: not null " +
                    $" actual Second Client Trade Id: {actualSecondClientTradeId}");

                Assert.True(actualThirdClientTradeId == null,
                    $" expected Second Client Trade Id: null " +
                    $" actual Second Client Trade Id: {actualThirdClientTradeId}");
            });
        }
    }
}