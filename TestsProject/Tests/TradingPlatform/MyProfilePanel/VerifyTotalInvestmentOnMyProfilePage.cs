using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.MyProfilePanel
{
    //[TestFixture(DataRep.Firefox)]
    [TestFixture(DataRep.Chrome)]
    public class VerifyTotalInvestmentOnMyProfilePage : TestSuitBase
    {
        #region Test Preparation
        public VerifyTotalInvestmentOnMyProfilePage(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _clientEmail; 
        private string _currentUserApiKey;   
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            var depositAmount = 1000;
            var withdrawalAmount = 300;
            var depositAmountForChargeback = 500;
            var depositAmountForDelete = 400;
            var bonusAmount = 300;
            var bonusAmountForDelete = 200;
            var crmUrl = Config.appSettings.CrmUrl;
            var dbContext = new QaAutomation01Context();
            _driver = GetDriver();

            // create user for the creation of api key
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // get ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // get login data
            var loginData = _apiFactory
               .ChangeContext<ILoginApi>(_driver)
               .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
               .GeneralResponse;

            // deposit 1000
            #region deposit 1000
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl,
                clientId, depositAmount);
            #endregion

            #region create withdrawal
            // create pending withdrawal
            _apiFactory
                .ChangeContext<IWithdrawalTpApi>(_driver)
                .PostPendingWithdrawalRequest(_tradingPlatformUrl, loginData, withdrawalAmount);

            // get withdrawal id
            var  withdrawalId =
                (from s in dbContext.FundsTransactions
                 where (s.UserId == clientId && Math.Abs(s.Amount) == 
                 withdrawalAmount && s.Type == "withdrawal" && s.Status == "pending")
                 select s.Id).First()
                 .ToString();

            // procceed withdrawal notification
            _apiFactory
               .ChangeContext<IFinancesTabApi>(_driver)
               .PatchWithdrawalStatusRequest(_crmUrl,
               clientId, withdrawalId, apiKey: _currentUserApiKey);
            #endregion

            // deposit 500 for chargeback
            #region deposit 500 for chargeback
            var  depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, clientId,
                depositAmountForChargeback);

            // chargeback
            var  chargebackId = _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .DeleteChargeBackDepositRequest(_crmUrl, clientId,
                depositAmountForChargeback, depositId);
            #endregion

            // deposit for delete
            depositId = _apiFactory
               .ChangeContext<IFinancesTabApi>(_driver)
               .PostDepositRequest(_crmUrl, clientId,
               depositAmountForDelete);

            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .DeleteFinanceItemRequest(_crmUrl, depositId);

            // add bonus
            _apiFactory
               .ChangeContext<IFinancesTabApi>(_driver)
               .PostBonusRequest(_crmUrl, clientId, bonusAmount);

            // bonus for delete
            var bonusId = _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostBonusRequest(_crmUrl, clientId, bonusAmountForDelete)
                .GeneralResponse
                .InsertId;

            // delete bonus
            _apiFactory
               .ChangeContext<IFinancesTabApi>(_driver)
               .DeleteFinanceItemRequest(_crmUrl, bonusId);
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
        public void VerifyTotalInvestmentOnMyProfilePageTest()
        {
            //TOTAL INVESTMENT
            //= total deposit + total bonus - chargeaback - del bonus - del deposit
            //If client have
            // deposit 1000
            // withdrawal 300
            // deposit 500
            // chargeback 500
            // deposit 400
            // delete deposit 400
            // bonus 300
            // bonus 300
            // delete bonus 300

            //then TOTAL INVESTMENT is = 1300 the withdrawal is not taking into account
            //1000 + 500 - 500 + 400 - 400 + 300 + 300 - 300
            var expectetTotalInvestment = 1300;

            var actualTotalInvestment = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradePageUi>(DataRep.MyProfileMenuItem)
                .GetTotalInvestmentValue();          

            Assert.Multiple(() =>
            {
                Assert.True(expectetTotalInvestment == actualTotalInvestment,
                    $"expected expectet Total Investment: {expectetTotalInvestment}" +
                    $" actual expectet Total Investment: {actualTotalInvestment}");
            });
        }
    }
}