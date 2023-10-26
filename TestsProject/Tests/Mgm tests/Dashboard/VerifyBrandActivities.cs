using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;

using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.MgmObjects.Api;
using ConsoleApp;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Mgm_tests.Dashboard
{
    [TestFixture]
    public class VerifyBrandActivities : TestSuitBase
    {      

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _currentUserApiKey;   
        private int _bonusAmount = 100;
        private int _depositForChargebackAmount = 10;
        private int _widrowalDepositAmount = 10;
        private GetLoginResponse _mgmLoginData;
        private int _depositAmount = 10000;
        private int _tradePnl = 50;

        [SetUp]
        public void SetUp()
        {
            #region PreCondition  
            BeforeTest();

            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

            var tradeAmount = 2;
            var dbContext = new QaAutomation01Context();          

            _mgmLoginData = _apiFactory
                .ChangeContext<IMgmCreateUserApi>()
                .PostMgmLoginCookiesRequest(DataRep.MgmUrl,
                DataRep.MgmUserName.Split('@').First());

            // create user
            var userName = TextManipulation.RandomString();
            var userEmail = userName + DataRep.EmailPrefix;

            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // enter the Email For Export in Super Admin Tub
            var brandRegulation = _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .GetBrandRegulationRequest(_crmUrl);

            DataRep.EmailListForExport.Add(userEmail);

            brandRegulation.export_data_email_url = DataRep
                .EmailListForExport.ToArray();

            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutRegulationsRequest(_crmUrl, brandRegulation);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, currency: "EUR",
                apiKey: _currentUserApiKey);

            // create deposit for Chargeback
            var depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId,
                _depositForChargebackAmount, originalCurrency: "EUR");

            // chargeback
            var chargebackId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .DeleteChargeBackDepositRequest(_crmUrl, _clientId,
                _depositForChargebackAmount, depositId);

            // create deposit for Withdrawal
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId,
                _depositAmount, originalCurrency: "EUR");

            // Withdrawal 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostWithdrawalDepositRequest(_crmUrl, _clientId, userId,
                _widrowalDepositAmount, currencyCode: "EUR");

            // get pending Withdrawal id
            var withdrawalId = _apiFactory
                 .ChangeContext<IFinancesTabApi>()
                 .GetFinanceDataRequest(_crmUrl, _clientId)
                 .GeneralResponse.Where(p => p.type.Equals("withdrawal"))
                 .FirstOrDefault()
                 .id
                 .ToString();

            // approved Withdrawal
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PatchWithdrawalStatusRequest(_crmUrl,
                _clientId, withdrawalId, apiKey: _currentUserApiKey);

            // Withdrawal for other_Withdrawal
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostWithdrawalDepositRequest(_crmUrl, _clientId, userId,
                _widrowalDepositAmount, currencyCode: "EUR");

            // get pending Withdrawal id
            withdrawalId = _apiFactory
                 .ChangeContext<IFinancesTabApi>()
                 .GetFinanceDataRequest(_crmUrl, _clientId)
                 .GeneralResponse.Where(p => p.status.Equals("pending"))
                 .FirstOrDefault()
                 .id
                 .ToString();

            // approved Withdrawal
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PatchWithdrawalStatusRequest(_crmUrl,
                _clientId, withdrawalId, apiKey: _currentUserApiKey);

            // Withdrawal title fees
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PatchAssignWithdrawalRequest(_crmUrl, withdrawalId);

            // create bonus 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, _bonusAmount);

            // first client login cookies
            var loginCookies = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            // create first trade 
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(tradingPlatformUrl, tradeAmount, loginCookies)
                .GeneralResponse;

            var tradeId = tradeDetails.TradeId;

            // close trade 
            _apiFactory
                .ChangeContext<IOpenTradesPageApi>()
                .PatchCloseTradeRequest(_crmUrl, tradeId);

            // Update client Pnl
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .UpdateTradePnl(tradeId, _tradePnl);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
            }
            finally
            {
                AfterTest();
            }
        }

        #endregion

        [Test]
        [Category(DataRep.MgmSanityCategory)]
        //[RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyBrandActivitiesTest()
        {
            #region Expected Data
            var expectedNetDeposits = _depositAmount - _widrowalDepositAmount;
            var expectedTotalDeposits = _depositAmount + _depositForChargebackAmount;
            var expectedNumOfDeposits = 2;
            var expectedNumOfFtds = 1;
            var expectedBonus = _bonusAmount;
            var expectedOtherWithdrawals = _depositForChargebackAmount;
            var expectedWithdrawals = _widrowalDepositAmount;
            var expectedChargebacks = _depositForChargebackAmount;
            var expectedTotalPnl = _tradePnl;
            #endregion

            var actualBrandsActivities = _apiFactory
                .ChangeContext<IMgmDashboardApi>()
                .GetBrandsActivitiesRequest(DataRep.MgmUrl, _mgmLoginData)
                .crmQaDevAuto;         

            Assert.Multiple(() =>
            {
                Assert.True(actualBrandsActivities.net_deposits == expectedNetDeposits,
                    $" actual Net Deposits: {actualBrandsActivities.net_deposits}" +
                    $" expected Net Deposits : {expectedNetDeposits}");

                Assert.True(actualBrandsActivities.deposits == expectedTotalDeposits,
                    $" actual Total Deposits: {actualBrandsActivities.deposits}" +
                    $" expected Total Deposits : {expectedTotalDeposits}");

                Assert.True(actualBrandsActivities.deposits_count == expectedNumOfDeposits,
                    $" actual Num Of Deposits: {actualBrandsActivities.deposits_count}" +
                    $" expected Num Of Deposits : {expectedNumOfDeposits}");

                Assert.True(actualBrandsActivities.ftds == expectedNumOfFtds,
                    $" actual Num Of Ftds: {actualBrandsActivities.ftds}" +
                    $" expected Num Of Ftds : {expectedNumOfFtds}");

                Assert.True(actualBrandsActivities.bonuses == expectedBonus,
                    $" actual .bonuses: {actualBrandsActivities.bonuses}" +
                    $" expected .bonuses : {expectedBonus}");

                Assert.True(actualBrandsActivities.withdrawals == expectedWithdrawals,
                    $" actual withdrawals: {actualBrandsActivities.withdrawals}" +
                    $" expected withdrawals : {expectedWithdrawals}");

                Assert.True(actualBrandsActivities.other_withdrawals
                    == expectedOtherWithdrawals,
                    $" actual other_withdrawals: {actualBrandsActivities.other_withdrawals}" +
                    $" expected other_withdrawals : {expectedOtherWithdrawals}");

                Assert.True(actualBrandsActivities.chargebacks == expectedChargebacks,
                    $" actual chargebacks: {actualBrandsActivities.chargebacks}" +
                    $" expected chargebacks : {expectedChargebacks}");

                Assert.True(actualBrandsActivities.pnl == expectedTotalPnl,
                    $" actual Total Pnl: {actualBrandsActivities.pnl}" +
                    $" expected Total Pnl : {expectedTotalPnl}");
            });
        }
    }
}