using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Dashboard
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyStatisticsBoxesOnDashboard : TestSuitBase
    {
        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private int _approvedWithdrawalAmount = 100;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private IWebDriver _driver;
        #endregion

        public VerifyStatisticsBoxesOnDashboard(string browser) : base(browser)
        {
            _browserName = browser;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();
            var _tradeAmount = 1;
            var crmUrl = Config.appSettings.CrmUrl;
            var mailPerfix = DataRep.EmailPrefix;
            var dbContext = new QaAutomation01Context();
            var tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
            var firstDepositAmount = 1000;
            var secondDepositAmount = 2000;
            var chargebackAmount = 200;
            var pendingWithdrawalAmount = 100;

            // create user
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(crmUrl, userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(crmUrl, userId);

            // create client
            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(crmUrl, clientName, currency: "EUR");

            var clientsIds = new List<string> { clientId };

            // connect One User To One Client
            _apiFactory
               .ChangeContext<IClientsApi>(_driver)
               .PatchMassAssignSaleAgentsRequest(crmUrl, userId,
               clientsIds);

            // login cookies
            var loginCookies = _apiFactory
                .ChangeContext<ILoginApi>(_driver)
                .PostLoginToTradingPlatform(tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            // deposit 400
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(crmUrl, clientId,
                firstDepositAmount, originalCurrency: "EUR");

            // deposit 200
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(crmUrl, clientId,
                secondDepositAmount, originalCurrency: "EUR");

            // get deposit id by amount
            var depositId =
              (from s in dbContext.FundsTransactions
               where (s.UserId == clientId
               && s.OriginalAmount == secondDepositAmount && s.Type == "deposit")
               select s.Id)
               .First()
               .ToString();

            // chargeback
            var chargebackId = _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .DeleteChargeBackDepositRequest(crmUrl,
                clientId, chargebackAmount, depositId)
                .ToString();

            // assign chargeback to user
            _apiFactory
               .ChangeContext<IChargebacksPageApi>(_driver)
               .PatchAssignChargebackToUserRequest(crmUrl, chargebackId, userId);

            // withdrawal
            _apiFactory
               .ChangeContext<IWithdrawalTpApi>(_driver)
               .CreateWithdrawalPipeApi(tradingPlatformUrl, loginCookies,
               _approvedWithdrawalAmount);

            // get pending Withdrawal id
            var withdrawalId =
                (from s in dbContext.FundsTransactions
                 where (s.UserId == clientId &&
                 Math.Abs(s.Amount) == _approvedWithdrawalAmount &&
                 s.Type == "withdrawal" && s.Status == "pending")
                 select s.Id).First()
                 .ToString();

            // proceed Withdrawal
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PatchWithdrawalStatusRequest(crmUrl,
                clientId, withdrawalId,
                apiKey: currentUserApiKey);

            // withdrawal
            _apiFactory
               .ChangeContext<IWithdrawalTpApi>(_driver)
               .CreateWithdrawalPipeApi(tradingPlatformUrl,
               loginCookies, pendingWithdrawalAmount);

            #region create trade 
            // create trade 
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostBuyAssetRequest(tradingPlatformUrl, _tradeAmount, loginCookies)
                .GeneralResponse;
            #endregion

            var tradeId = tradeDetails.TradeId;

            #region close trade 
            // close trade 
            _apiFactory
              .ChangeContext<IOpenTradesPageApi>(_driver)
              .PatchCloseTradeRequest(crmUrl, tradeId);

            // Update Pnl
            _apiFactory
             .ChangeContext<ITradePageApi>(_driver)
             .UpdateTradePnl(tradeId, 50);
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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]      
        public void VerifyStatisticsBoxesOnDashboardTest()
        {
            var pendingWithdrawalAmount = 100;
            var chargebackAmount = 200;

            var expectedWithdrawalData = new List<string> { $"Pending {pendingWithdrawalAmount}.00",
                $"Approved {_approvedWithdrawalAmount}.00", $"Chargeback {chargebackAmount}.00" };

            _apiFactory              
               .ChangeContext<IDashboardPageUi>(_driver)
               .VerifyBackCardWithdrawalData(expectedWithdrawalData)
               .VerifyBackCardWithdrawalForEurCurrency()
               .VerifyFrontCardTotalWithdrawal(_approvedWithdrawalAmount.ToString())
               .VerifyFrontCardTotalWithdrawalEurCurrency()
               .VerifyFrontCardPnl("50")
               .VerifyFrontCardPnlEurCurrency()
               .VerifyFrontCardTotalDeposit("3,000")
               .VerifyFrontCardNetTotalDeposit("900");       
        }
    }
}
