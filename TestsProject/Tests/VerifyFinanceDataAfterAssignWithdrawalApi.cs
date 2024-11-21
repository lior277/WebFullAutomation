// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;

using AirSoftAutomationFramework.Internals.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.TestCase;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;

namespace TestsProject.Tests
{
    [TestFixture("fees", 300, 300, 300, 100, 0, 0, 400, 100, 0)]
    [TestFixture("withdrawal", 300, 300, 300, 0, 100, 100, 300, 0, 100)]
    public class VerifyFinanceDataAfterAssignWithdrawalApi : TestSuitBase
    {
        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;
        private SearchResultOnBanking _searchResultWithdrawalBanking;
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private int _firstDepositAmount = 400;
        private int _approvedWithrawalAmount = 100;
        private GetBoxesStatisticsResponse _boxesStatisticsData;
        private Tuple<ExpectedFinanceData, List<double>> _financeData;
        private ExpectedFinanceData actualFinanceData;
        private double _balance;
        private double _equity;
        private double _available;
        private double _otherOnDashboard;
        private double _totalWithdrawal;
        private double _approvedWithdrawal;
        private double _netDeposit;
        private double _otherOnBanking;
        private double _realOnBanking;
        private string _withdrawalTitle;
        #endregion

        public VerifyFinanceDataAfterAssignWithdrawalApi(string withdrawalTitle, double balance,
           double equity, double available, double otherOnDashboard, double totalWithdrawal, double approvedWithdrawal,
           double netDeposit, double otherOnBanking, double realOnBanking)
        {
            _withdrawalTitle = withdrawalTitle;
            _balance = balance;
            _equity = equity;
            _available = available;
            _otherOnDashboard = otherOnDashboard;
            _totalWithdrawal = totalWithdrawal;
            _approvedWithdrawal = approvedWithdrawal;
            _netDeposit = netDeposit;
            _otherOnBanking = otherOnBanking;
            _realOnBanking = realOnBanking;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            // create user 
            var userName = TextManipulation.RandomString();
            var userEmail = userName + DataRep.EmailPrefix;

            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl,
             userName, role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);
            #endregion

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, currency: "EUR",
                apiKey: currentUserApiKey);

            var clientsIds = new List<string> { clientId };

            #region connect One User To One Client 
            // connect One User To One Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, userId,
                clientsIds, apiKey: currentUserApiKey);
            #endregion

            #region get login data
            // get login data
            var loginCookies = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;
            #endregion

            #region deposit 400
            // deposit 400
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId,
                _firstDepositAmount, originalCurrency: "EUR",
                apiKey: currentUserApiKey);
            #endregion

            #region get deposit id by amount
            // get deposit id by amount
            var depositId =
              (from s in _dbContext.FundsTransactions
               where (s.UserId == clientId && s.OriginalAmount ==
               _firstDepositAmount && s.Type == "deposit")
               select s.Id).First();
            #endregion

            #region create withdrawal
            // withdrawal
            _apiFactory
               .ChangeContext<IWithdrawalTpApi>()
               .CreateWithdrawalPipeApi(_tradingPlatformUrl, loginCookies, _approvedWithrawalAmount);
            #endregion

            #region get pending Withdrawal id
            // get pending Withdrawal id
            var withdrawalId =
                (from s in _dbContext.FundsTransactions
                 where (s.UserId == clientId && Math.Abs(s.Amount)
                 == _approvedWithrawalAmount && s.Type == "withdrawal" && s.Status == "pending")
                 select s.Id).First()
                 .ToString();
            #endregion

            #region update super admin tub -  check withdrawal title filed

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
            #endregion

            #region proceed Withdrawal
            // proceed Withdrawal
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PatchWithdrawalStatusRequest(_crmUrl, clientId,
                withdrawalId, apiKey: currentUserApiKey); // approve the withdrawal

            _apiFactory
                 .ChangeContext<IFinancesTabApi>()
                 .PatchAssignWithdrawalRequest(_crmUrl, withdrawalId,
                 _withdrawalTitle, apiKey: currentUserApiKey); // Assign Withdrawal title;
            #endregion

            #region get Boxes Statistics Data 
            // get Boxes Statistics Data 
            _boxesStatisticsData = _apiFactory
               .ChangeContext<IDashboardApi>()
               .GetBoxesStatisticsRequest(_crmUrl, currentUserApiKey);
            #endregion

            #region get finance data from client card
            _financeData = _apiFactory
               .ChangeContext<IFinanceFactoryApi>()
               .GetFinanceData(clientId);
            #endregion

            actualFinanceData = _financeData.Item1;

            #region get search Result from Withdrawal table on Withdrawal page
            _searchResultWithdrawalBanking = _apiFactory
               .ChangeContext<IWithdrawalsPageApi>()
               .GetWithdrawalDataFromBankingRequest(_crmUrl, "erp_user_id_assigned", userId, currentUserApiKey)
               .GeneralResponse;
            #endregion
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
                AfterTest();
            }
        }

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyFinanceDataAfterAssignWithdrawalApiTest()
        {
            Assert.Multiple(() =>
            {
                Assert.True(actualFinanceData.balance == _balance,
                    $" expected withdrawal Title: {_withdrawalTitle}," +
                    $" expected _balance: {_balance} for client Email: {_clientEmail}" +
                    $" actual _balance :  {actualFinanceData.balance}");

                Assert.True(actualFinanceData.equity == _equity,
                    $" expected withdrawal Title: {_withdrawalTitle}," +
                    $" expected _equity: {_equity} for client Email: {_clientEmail} " +
                    $" actual _equity :  {actualFinanceData.equity}");

                Assert.True(actualFinanceData.available == _available,
                    $" expected withdrawal Title: {_withdrawalTitle}," +
                    $" expected _available: {_available} for client Email: {_clientEmail} " +
                    $" actual _available :  {actualFinanceData.available}");

                Assert.True(_boxesStatisticsData.approved_total_other_withdrawals == _otherOnDashboard,
                    $" expected withdrawal Title: {_withdrawalTitle}," +
                    $" expected other On Dashboard: {_otherOnDashboard} for client Email: {_clientEmail} " +
                    $" actual other On Dashboard :  {_boxesStatisticsData.approved_total_other_withdrawals}");

                Assert.True(_boxesStatisticsData.approved_total_withdrawals == _totalWithdrawal,
                    $" expected withdrawal Title: {_withdrawalTitle}," +
                    $" expected total Withdrawal: {_totalWithdrawal} for client Email: {_clientEmail} " +
                    $" actual total Withdrawal :  {_boxesStatisticsData.approved_total_withdrawals}");

                Assert.True(_boxesStatisticsData.approved_total_withdrawals == _approvedWithdrawal,
                    $" expected withdrawal Title: {_withdrawalTitle}," +
                    $" expected approved Withdrawal: {_approvedWithdrawal} for client Email: {_clientEmail} " +
                    $" actual approved Withdrawal :  {_boxesStatisticsData.approved_total_withdrawals}");

                Assert.True(_boxesStatisticsData.net_deposit == _netDeposit,
                    $" expected withdrawal Title: {_withdrawalTitle}," +
                    $" expected net Deposit: {_netDeposit} for client Email: {_clientEmail} " +
                    $" actual net Deposit :  {_boxesStatisticsData.net_deposit}");

                Assert.True(_searchResultWithdrawalBanking.otherEuro == _otherOnBanking,
                    $" expected withdrawal Title: {_withdrawalTitle}," +
                    $" expected other On Banking: {_otherOnBanking} for client Email: {_clientEmail} " +
                    $" actual other On Banking :  {_searchResultWithdrawalBanking.otherEuro}");

                Assert.True(_searchResultWithdrawalBanking.totalEuro == _realOnBanking.ToString(),
                    $" expected withdrawal Title: {_withdrawalTitle}," +
                    $" expected real On Banking: {_realOnBanking} for client Email: {_clientEmail} " +
                    $" actual real On Banking :  {_searchResultWithdrawalBanking.totalEuro}");

                Assert.True(_searchResultWithdrawalBanking.data[0]
                    .title.Contains(_withdrawalTitle, StringComparison.OrdinalIgnoreCase),
                    $" expected withdrawal Title: {_withdrawalTitle} for client Email: {_clientEmail} " +
                    $" actual withdrawal Title :  {_searchResultWithdrawalBanking.data[0].title}");
            });
        }
    }
}


