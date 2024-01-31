// Ignore Spelling: Ign Calander Donat Forex

using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public interface IDashboardPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ISearchResultsUi ClickOnRecentActivitiesComments();
        ISearchResultsUi ClickOnRecentActivitiesDeposit();
        ISearchResultsUi ClickOnRecentActivitiesLogin();
        ISearchResultsUi ClickOnRecentActivitiesOrders();
        ISearchResultsUi ClickOnRecentActivitiesRegister();
        IAgentProfileUi ClickOnSaleTitle(string userName);
        List<string> GetBackCardTotalDeposits();
        string GetBackCardTotalOrders();
        int GetBackCardTotalPendingWithdrawals(int amount);
        string GetBackTotalOrders();
        string GetFrontTotalOrdersGross(double ordersAmount);
        ISearchResultsUi SearchActiveCampaign(string campaignName);
        ISearchResultsUi SearchDeposit(string searchText);
        ISearchResultsUi SearchInActiveCampaign(string campaignName);
        ISearchResultsUi SearchOrderById(string orderId);
        ISearchResultsUi SearchWithdrawal(string searchText);
        IDashboardPageUi VerifyActiveCampaignsNotExist();
        IDashboardPageUi VerifyBackCardClientsData(List<string> expectedClientsData);
        string VerifyBackCardWithdrawalData();
        IDashboardPageUi VerifyBackCardWithdrawalData(List<string> expectedWithdrawalData);
        IDashboardPageUi VerifyBackCardWithdrawalForEurCurrency();
        IDashboardPageUi VerifyCalanderNotExist();
        IDashboardPageUi VerifyDepositsNotExist();
        IDashboardPageUi VerifyFrontCardNetTotalDeposit(string expectedNetDeposit);
        IDashboardPageUi VerifyFrontCardPnl(string expectedClosePnl);
        IDashboardPageUi VerifyFrontCardPnlEurCurrency();
        IDashboardPageUi VerifyFrontCardTotalCanCelledOrdersAmount(double canceledOrdersAmount);
        IDashboardPageUi VerifyFrontCardTotalCustomers(int numOfCustomers);
        IDashboardPageUi VerifyFrontCardTotalDeposit(string expectedDeposit);
        IDashboardPageUi VerifyFrontCardTotalWithdrawal(string expectedTotalWithdrawal);
        IDashboardPageUi VerifyFrontCardTotalWithdrawalEurCurrency();
        IDashboardPageUi VerifyLastRegistrationNotExist();
        IDashboardPageUi VerifyPerformanceAndDoNotExist();
        IDashboardPageUi VerifyWithdrawalsNotExist();
    }
}