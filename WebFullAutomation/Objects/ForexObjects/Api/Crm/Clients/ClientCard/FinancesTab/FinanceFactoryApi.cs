// Ignore Spelling: Forex api Crm

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.MongoDb;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Models;
using static AirSoftAutomationFramework.Objects.DTOs.TestCase;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab
{
    public class FinanceFactoryApi : IFinanceFactoryApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        private string _clientId;
        private string _currentUserApiKey;
        private static string _financeDataOfClientCardTable = DataRep.TableNameForFinanceDataTest;
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private IApiAccess _apiAccess;
        private GetLoginResponse _loginData;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradedingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        #endregion Members

        public FinanceFactoryApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public IFinanceFactoryApi FinanceFactoryByName(string clientId,
            GetLoginResponse loginData, string currentUserApiKey, TestCase testCase)
        {
            var logAction = "";
            try
            {
                _loginData = loginData;
                _clientId = clientId;
                _currentUserApiKey = currentUserApiKey;

                var methods = GetType()
                    .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance
                    | BindingFlags.IgnoreCase | BindingFlags.Public).ToList();

                foreach (var step in testCase.steps)
                {
                    logAction = step.action.RemoveWhiteSpace();
                    var action = step.action.RemoveWhiteSpace();
                    methods.Where(m => m.Name.Equals(action, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault()?
                        .Invoke(this, new[] { step });
                }

                return this;
            }
            catch (Exception ex)
            {
                var exceMessage = ($"{ex.Message} case id: {testCase.case_id} action: {logAction}");

                throw new Exception(exceMessage);
            }
        }

        public static IEnumerable<TestCaseData> RetrieveTestsFromFinanceDataOfClientCardTable()
        {
            Config.GetConfigurationFromMongoByBrandNamePipe();
            var ff = Config.appSettings.CrmUrl;
            IMongoDatabase mongoClient = null;
            var testsCaseFromMongo = new List<TestCase>();

            try
            {
                mongoClient = InitializeMongoClient.ConnectToAutomationMongoDb;
                var mongoDbAccess = new MongoDbAccess();

                testsCaseFromMongo = mongoDbAccess
                .SelectAllDocumentsFromTable<TestCase>(mongoClient,
                _financeDataOfClientCardTable);
            }
            catch (Exception ex)
            {
                var exceMessage = ($" mongo Client: {mongoClient}, Exception Message: {ex.Message}");

                throw new Exception(exceMessage);
            }

            //******
            // *** ONLY FOR DEBUG
            //var tests = new List<TestCase>();
            //testsCaseFromMongo.ForEach(p => tests.Add(p));
            //var test = tests.Where(p => p.case_id == 3)
            //.FirstOrDefault();

            //yield return new TestCaseData(test);
            // *** ONLY FOR DEBUG

            foreach (var testCase in testsCaseFromMongo)
            {
                yield return new TestCaseData(testCase);
            }
        }

        private void AddDeposit(Steps step)
        {
            var depositAmount = step.deposit_amount;
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, _clientId, depositAmount, apiKey: _currentUserApiKey);
        }

        private void DeleteDeposit(Steps step)
        {
            var depositId =
              (from s in _dbContext.FundsTransactions
               where (s.UserId == _clientId && s.Amount ==
               step.delete_deposit_amount && s.Type == "deposit")
               select s.Id).First()
               .ToString();

            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .DeleteFinanceItemRequest(_crmUrl, depositId, apiKey: _currentUserApiKey);
        }

        private void Chargeback(Steps step)
        {
            // get deposit id by amount
            var depositId =
              (from s in _dbContext.FundsTransactions
               where (s.UserId == _clientId &&
               s.Amount == step.chargeback_amount && s.Type == "deposit")
               select s.Id).First()
               .ToString();

            _apiFactory
               .ChangeContext<IFinancesTabApi>(_driver)
               .DeleteChargeBackDepositRequest(_crmUrl,
               _clientId, step.chargeback_amount, depositId, apiKey: _currentUserApiKey);
        }

        private void Withdrawal(Steps step)
        {
            var financeData = _apiFactory
                .ChangeContext<IWithdrawalTpApi>(_driver)
                .CreateWithdrawalPipeApi(_tradedingPlatformUrl,
                _loginData, step.withdrawal_amount);

            // get Withdrawal id
            var withdrawalId =
                (from s in _dbContext.FundsTransactions
                 where (s.UserId == _clientId && Math.Abs(s.Amount)
                 == step.withdrawal_amount && s.Type ==
                 "withdrawal" && s.Status == "pending")
                 select s.Id).First();
        }

        private void ProceedWithdrawal(Steps step)
        {
            var withdrawalId =
                (from s in _dbContext.FundsTransactions
                 where (s.UserId == _clientId && s.Type ==
                 "withdrawal" && s.Status == "pending")
                 select s.Id).First()
                 .ToString();

            var actualWithdrawalStatus = step?.withdrawal_status;

            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PatchWithdrawalStatusRequest(_crmUrl, _clientId, withdrawalId,
                withdrawalStatus: actualWithdrawalStatus, apiKey: _currentUserApiKey);
        }

        private void DeleteWithdrawal(Steps step)
        {
            var withdrawalId =
               (from s in _dbContext.FundsTransactions
                where (s.UserId == _clientId && Math.Abs(s.Amount)
                == step.withdrawal_amount && s.Type == "withdrawal" && s.Status == "approved")
                select s.Id).First()
                .ToString();

            _apiFactory
             .ChangeContext<IFinancesTabApi>(_driver)
             .DeleteWithdrawalRequest(_crmUrl, withdrawalId, apiKey: _currentUserApiKey);
        }

        private void ProceedSplitWithdrawal(Steps step)
        {
            var withdrawalId =
               (from s in _dbContext.FundsTransactions
                where (s.UserId == _clientId && s.Type == "withdrawal"
                && s.Status == "pending")
                select s.Id).ToList();

            var withdrawalsStatuses = step.statuses_ofsplit_withdrawals;

            for (var i = 0; i < withdrawalsStatuses.Count; i++)
            {
                _apiFactory
                    .ChangeContext<IFinancesTabApi>(_driver)
                    .PatchWithdrawalStatusRequest(_crmUrl, _clientId,
                    withdrawalId[i].ToString(), withdrawalStatus: withdrawalsStatuses[i],
                    apiKey: _currentUserApiKey);
            }
        }

        private void SplitWithdrawal(Steps step)
        {
            var withdrawalId =
               (from s in _dbContext.FundsTransactions
                where (s.UserId == _clientId && s.Type == "withdrawal" && s.Status == "pending")
                select s.Id).First()
                .ToString();

            _apiFactory
            .ChangeContext<IFinancesTabApi>(_driver)
            .PostSplitPendingWithdrawalRequest(_crmUrl, _clientId,
            withdrawalId, step.withdrawal_split_amount, apiKey: _currentUserApiKey);
        }

        private void TransferToSA(Steps step)
        {
            var amount = step.transfer_sa_amount;

            _apiFactory
               .ChangeContext<ISATabApi>(_driver)
               .PostTransferToSavingAccountRequest(_crmUrl, _clientId,
               amount, apiKey: _currentUserApiKey);
        }

        private void TransferToBalance(Steps step)
        {
            var amount = step.transfer_balance_amount;

            _apiFactory
               .ChangeContext<ISATabApi>(_driver)
               .PostTransferToBalanceRequest(_crmUrl, _clientId,
               amount, apiKey: _currentUserApiKey);
        }

        private void OpenTrade(Steps step)
        {
            _apiFactory
               .ChangeContext<ITradePageApi>(_driver)
               .CreateTradePipeApi(_tradedingPlatformUrl, _loginData, step);
        }

        private void ChangeSpred(Steps step)
        {
            var trade =
              (from s in _dbContext.Trades
               where (s.UserId == _clientId && s.Amount == step.trade_amount)
               select s)
               .First();

            trade.Spread = step.spred;
            _dbContext.VerifySaveForSqlManipulation();
        }

        private void CloseTrade(Steps step)
        {
            var tradeId =
               (from s in _dbContext.Trades
                where (s.UserId == _clientId && s.Amount == step.trade_amount)
                select s.Id)
                .First()
                .ToString();

            _apiFactory
                .ChangeContext<IOpenTradesPageApi>(_driver)
                .PatchCloseTradeRequest(_crmUrl, tradeId);

            UpdatePnl(step);
        }

        private void DeleteTrade(Steps step)
        {
            var tradeId =
               (from s in _dbContext.Trades
                where (s.UserId == _clientId && s.Amount == step.trade_amount)
                select s.Id)
                .First()
                .ToString();

            _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .DeleteTradeRequest(_crmUrl, tradeId);
        }

        //public void UpdatePnlPublic(int tradeAmount, int pnlAmount = 50, string clientId = null)
        //{
        //    var tradeRecord =
        //         (from s in dbContext.Trades
        //          where (s.UserId == clientId && s.Amount == tradeAmount && s.Status == "close")
        //          select s).First();

        //    tradeRecord.ClosedProfitLoss = pnlAmount;
        //    dbContext.SaveChanges();

        //    var commandText = $"UPDATE `user_account` SET `closed_profit_loss` =get_user_closed_profit_loss('{clientId}',{0}), `open_profit_loss` =get_user_open_profit_loss('{clientId}',{0}), `open_investments` =get_user_open_investment('{clientId}',{0}), `min_margin` =get_user_min_margin('{clientId}',{0}) where user_id ='{clientId}'";
        //    dbContext.Database.ExecuteSqlRaw(commandText);
        //    dbContext.SaveChanges();
        //}

        private void UpdatePnl(Steps step)
        {
            var tradeRecord =
                 (from s in _dbContext.Trades
                  where (s.UserId == _clientId && s.Amount == step.trade_amount && s.Status == "close")
                  select s).First();

            if (step.pnl_amount == 0)
            {
                step.pnl_amount = 50;
            }
            tradeRecord.ClosedProfitLoss = step.pnl_amount;
            _dbContext.VerifySaveForSqlManipulation();

            var commandText = $"UPDATE" +
                $" `user_account` SET" +
                $" `closed_profit_loss`" +
                $" =get_user_closed_profit_loss('{_clientId}',{0})," +
                $" `open_profit_loss` =get_user_open_profit_loss('{_clientId}',{0})," +
                $" `open_investments` =get_user_open_investment('{_clientId}',{0})," +
                $" `min_margin` =get_user_min_margin('{_clientId}',{0}) where user_id ='{_clientId}'";
            _dbContext.Database.ExecuteSqlRaw(commandText);
            _dbContext.VerifySaveForSqlManipulation();
        }

        private void AddBonus(Steps step)
        {
            var bonusAmount = step.bonus_amount;
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostBonusRequest(_crmUrl, _clientId,
                bonusAmount, apiKey: _currentUserApiKey);
        }

        private void WithdrawalBonus(Steps step)
        {
            var withdrawalamount = step.withdrawal_bonus_amount;

            _apiFactory
             .ChangeContext<IFinancesTabApi>(_driver)
             .PostWithdrawalBonusRequest(_crmUrl, _clientId,
             withdrawalamount, apiKey: _currentUserApiKey);
        }

        private void DeleteBonus(Steps step)
        {
            var bonusId =
             (from s in _dbContext.FundsTransactions
              where (s.UserId == _clientId &&
              s.Amount == step.delete_bonus_amount && s.Type == "deposit_bonus")
              select s.Id).First()
              .ToString();

            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .DeleteFinanceItemRequest(_crmUrl, bonusId);
        }

        private void ResetAccount(Steps step)
        {
            if (step is null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .DeleteResetAccountRequest(_crmUrl, _clientId);
        }

        public Tuple<ExpectedFinanceData, List<double>> GetFinanceData(string clientId = null)
        {
            _clientId = clientId ?? _clientId;
            // var actualSteps = new Steps();
            var actualAcountValueColunm = new List<double>();

            // actual actual Finance Data from sql db
            var actualFinanceData =
                (from s in _dbContext.UserAccounts
                 where s.UserId == _clientId
                 select new ExpectedFinanceData
                 {
                     balance = (int)s.Balance.MathRoundFromGeneric(0, MidpointRounding.ToPositiveInfinity),
                     available = (int)s.Available.MathRoundFromGeneric(0, MidpointRounding.ToPositiveInfinity),
                     min_margin = (int)s.MinMargin.MathRoundFromGeneric(0, MidpointRounding.ToPositiveInfinity),
                     bonus = (int)s.Bonus.MathRoundFromGeneric(0, MidpointRounding.ToPositiveInfinity),
                     equity = (int)s.Equity.MathRoundFromGeneric(0, MidpointRounding.ToPositiveInfinity),
                 }).FirstOrDefault();

            // fill actual Account_Values values from sql db
            (from s in _dbContext.FundsTransactions
             where (s.Type == "deposit" || s.Type == "deposit_bonus")
             && !s.IsDeleted
             && s.IsChargeback == 0
             && s.UserId == _clientId

             orderby s.Id ascending
             select s)
                .ToList()
                .ForEach(u => actualAcountValueColunm.Add(Convert.ToDouble(string.Format("{0:0.00}", u.AccountValue))));

            return Tuple.Create(actualFinanceData, actualAcountValueColunm);
        }

        //public List<FundsSnapshot> GetSnapshotTableIsData(string clientId = null)
        //{
        //    Thread.Sleep(2000);
        //    _clientId = clientId ?? _clientId;
        //    var actualTableColumns = _dbContext
        //        .FundsSnapshots.ToList().Where(h => h.UserId == _clientId).ToList();

        //    return actualTableColumns;
        //}

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}