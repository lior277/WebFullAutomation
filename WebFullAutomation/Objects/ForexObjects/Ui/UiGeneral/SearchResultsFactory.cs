// Ignore Spelling: api Forex

using System.Linq;
using System.Reflection;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public class SearchResultsFactory : ISearchResultsFactory
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        public SearchResultsFactory(IWebDriver driver, IApplicationFactory apiFactory)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }
        /// <summary>
        /// support one or tow row result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rowsData"></param>
        /// <returns></returns>
        public T InstanceFactory<T>()
        {
            var instance = (T)GetType()
                 .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                 .Where(m => m.ReturnType.Equals(typeof(T)))
                 .FirstOrDefault()?
                 .Invoke(this, null);
           
            return instance;
        }

        private SearchResultClients ClientTable()
        {
            return new SearchResultClients();
        }

        private SearchResultBulkTradeReport BulkTradeReportTable()
        {
            return new SearchResultBulkTradeReport();
        }

        private SearchResultBulkTrade BulkTradeTable()
        {
            return new SearchResultBulkTrade();
        }

        private SearchResultBulkTrading BulkTradingTable()
        {
            return new SearchResultBulkTrading();
        }

        private SearchResultBulkTradeHistory BulkTradeHistoryTable()
        {
            return new SearchResultBulkTradeHistory();
        }

        private SearchResultUsers UsersTable()
        {
            return new SearchResultUsers();
        }

        private SearchResultRoles RolesTable()
        {
            return new SearchResultRoles();
        }

        private SearchResultFinance DepositsTable()
        {
            return new SearchResultFinance();
        }

        private SearchResultDepositsBanking BankingDepositsTable()
        {
            return new SearchResultDepositsBanking();
        }

        private SearchResultActiveCampaign ActiveCampaignTable()
        {
            return new SearchResultActiveCampaign();
        }

        private SearchResultInactiveCampaign InActiveCampaignTable()
        {
            return new SearchResultInactiveCampaign();
        }

        private SearchResultWithdrawal WithdrawalTable()
        {
            return new SearchResultWithdrawal();
        }

        private SearchResultTrade TradesTable()
        {
            return new SearchResultTrade();
        }

        private SearchResultRisk RiskTable()
        {
            return new SearchResultRisk();
        }

        private SearchResultChronoTrade ChronoTable()
        {
            return new SearchResultChronoTrade();
        }

        private SearchResultTimeline TimelineTable()
        {
            return new SearchResultTimeline();
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
