using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using ConsoleApp;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.SalesPage
{
    //[TestFixture(DataRep.Firefox)]
    [TestFixture(DataRep.Chrome)]
    public class VerifyFinanceDataOnSales : TestSuitBase
    {
        #region Members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private int _firstDepositAmount = 10000;
        private int _chrgebackAmount = 10000;
        private int _secondDepositAmount = 10100;
        private int _thirdDepositAmount = 10200;
        private int _fourthDepositAmount = 10300;
        private int _withrawalAmount = 10;
        private string _firstClientName;
        private string _secondClientName;
        private string _thirdClientName;
        private string _thirdClientId;
        private string _userId;
        private IWebDriver _driver;
        private string _browserName;
        #endregion Members

        public VerifyFinanceDataOnSales(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            #region params
            _driver = GetDriver();
            var firstTradeAmount = 1;
            var secondTradeAmount = 2;
            var pnl = 90;
            var tradesIds = new List<string>();
            #endregion

            #region create user 

            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName,
                role: DataRep.AdminWithUsersOnlyRoleName);
            #endregion

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            #region create 3 client
            // create first client 
            _firstClientName = TextManipulation.RandomString();
            var firstClientEmail = $"{_firstClientName}{DataRep.EmailPrefix}";

            var firstClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _firstClientName, currency: "EUR");

            // create second client 
            _secondClientName = TextManipulation.RandomString();
            var secondClientEmail = $"{_secondClientName}{DataRep.EmailPrefix}";

            var secondClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _secondClientName, currency: "EUR");

            // create third client 
            _thirdClientName = TextManipulation.RandomString();

            _thirdClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _thirdClientName, currency: "EUR");

            var clientsIdsForTrades = new List<string> { firstClientId, secondClientId };
            var clientsIdsForConnectUser = new List<string> { firstClientId,
                secondClientId, _thirdClientId };
            #endregion

            #region connect One User To 3 Clients
            // connect One User To 3 Clients
            _apiFactory
               .ChangeContext<IClientsApi>(_driver)
               .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
               clientsIdsForConnectUser);
            #endregion

            #region deposit 400 for the first client
            // deposit 400 for the first client
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, firstClientId, _firstDepositAmount, originalCurrency: "EUR");
            #endregion

            #region deposit 200 for the second client
            // deposit 200 for the second client
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, secondClientId, _secondDepositAmount, originalCurrency: "EUR");
            #endregion

            #region deposit 100 for the third client
            // deposit 100 for the third client
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, _thirdClientId, _thirdDepositAmount, originalCurrency: "EUR");
            #endregion

            #region second deposit for the third client
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, _thirdClientId, _fourthDepositAmount, originalCurrency: "EUR");
            #endregion

            // first client login cookies
            var loginCookies = _apiFactory
                .ChangeContext<ILoginApi>(_driver)
                .PostLoginToTradingPlatform(_tradingPlatformUrl, firstClientEmail)
                .GeneralResponse;

            #region create first trade update pnl and close
            // create first trade 
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostBuyAssetRequest(_tradingPlatformUrl, firstTradeAmount, loginCookies)
                .GeneralResponse;

            var tradeId = tradeDetails.TradeId;
            tradesIds.Add(tradeId);

            // close trade 
            _apiFactory
                .ChangeContext<IOpenTradesPageApi>(_driver)
                .PatchCloseTradeRequest(_crmUrl, tradeId);
            #endregion

            // second client login cookies
            loginCookies = _apiFactory
                .ChangeContext<ILoginApi>(_driver)
                .PostLoginToTradingPlatform(_tradingPlatformUrl, secondClientEmail)
                .GeneralResponse;

            #region create second trade for second client update pnl and close
            // create second trade 
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostBuyAssetRequest(_tradingPlatformUrl,
                secondTradeAmount, loginCookies)
                .GeneralResponse;

            tradeId = tradeDetails.TradeId;
            tradesIds.Add(tradeId);

            // close trade 
            _apiFactory
              .ChangeContext<IOpenTradesPageApi>(_driver)
              .PatchCloseTradeRequest(_crmUrl, tradeId);
            #endregion

            // Update Pnl
            _apiFactory
             .ChangeContext<ITradePageApi>(_driver)
             .UpdateTradePnl(tradesIds, pnl);

            // Update Pnl
            _apiFactory
             .ChangeContext<ITradePageApi>(_driver)
             .UpdateClientPnl(clientsIdsForTrades, tradesIds, pnl);

            #region withrawal and chargeback        
            // withrawal for second client
            _apiFactory
               .ChangeContext<IWithdrawalTpApi>(_driver)
               .CreateWithdrawalPipeApi(_tradingPlatformUrl,
               loginCookies, _withrawalAmount);

            // get pending Withdrawal id
            var  withdrawalId =
                (from s in _dbContext.FundsTransactions
                 where (s.UserId == secondClientId &&
                 Math.Abs(s.Amount) == _withrawalAmount &&
                 s.Type == "withdrawal" && s.Status == "pending")
                 select s.Id).First()
                 .ToString();

            // proceed Withdrawal
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PatchWithdrawalStatusRequest(_crmUrl, secondClientId,
                withdrawalId, apiKey: currentUserApiKey);

            // get deposit id by amount
            var depositId =
              (from s in _dbContext.FundsTransactions
               where (s.UserId == firstClientId && s.OriginalAmount == _firstDepositAmount 
               && s.Type == "deposit")
               select s.Id)
               .First()
               .ToString();

            // chargeback 400 usd
            var chargebackId = _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .DeleteChargeBackDepositRequest(_crmUrl, firstClientId,
                _chrgebackAmount, depositId)
                .ToString();

            // asign chargeback to user
            _apiFactory
               .ChangeContext<IChargebacksPageApi>(_driver)
               .PatchAssignChargebackToUserRequest(_crmUrl, chargebackId, _userId);
            #endregion

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
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyFinanceDataOnSalesTest()
        {
            #region expectedData          
            var expectedBoxesTitles = new List<string>() { "FTD", "Retention", "Total Deposit",
                "Total Withdrawal",  "Total Chargebacks", "Net Deposit", "Deposit", "FTD",
                "Total PNL", "Conversion", "Total Volume", "Total deposit per currency" };

            var expectedBoxesValues = new List<string>() { "3", "30,300.00", "3",
                "10,300.00", "40,600.00", "0", "10.00", "10,000.00", "30,590.00", "6", "0", "3",
                "90.00", "75.00", "0.00", "40,600.00 €"  };
            #endregion

            var actualBoxesTitles = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ISalesPageUi>()
                .ChangeContext<ISalesPageUi>(_driver)
                .GetBoxesTitles();

            var actualBoxesValues = _apiFactory
                .ChangeContext<ISalesPageUi>(_driver)
                .GetBoxesValues();

            actualBoxesValues.RemoveAt(15); // the volume value removed because it should be calculated
                                            // so it cant be match to expected                                  

            var expectedAgainstActualBoxesTitles = actualBoxesTitles 
             .CompareTwoListOfString(expectedBoxesTitles);

            var expectedAgainstActualBoxesValues = actualBoxesValues 
             .CompareTwoListOfString(expectedBoxesValues);

            Assert.Multiple(() =>
            {
                Assert.True(expectedAgainstActualBoxesTitles.Count() == 0,
                        $" the difference Boxes Titles: {expectedAgainstActualBoxesTitles.ListToString()}");

                Assert.True(expectedAgainstActualBoxesValues.Count() == 0,
                        $" the difference Boxes Values: {expectedAgainstActualBoxesValues.ListToString()}");
            });
        }
    }
}
