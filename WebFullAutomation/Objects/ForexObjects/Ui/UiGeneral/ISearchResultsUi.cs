// Ignore Spelling: Forex

using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.RolesPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.UsersMenuUi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public interface ISearchResultsUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IClientCardUi ClickOnClientFullName(string clientName = null);
        ITradeGroupCardUi ClickOnEditGroupButton();
        IRolesPageUi ClickOnDeleteRoleButton();
        IRoleUi ClickOnEditRoleButton();
        IUserUi ClickOnEditUserButton();
        IClientsPageUi ConnectClientToCampaign(string campaignName);
        List<string> GetClientsFullName();
        IList<T> GetSearchResultDetails<T>(string tableName = null,
            string aditionalData = null,
            int expectedNumOfRows = 1,
            bool checkNumOfRows = true,
            bool cellsAndTitlesShouldBeEquals = true);
        IClientsPageUi SelectSalesAgent(string agentName);
        ISearchResultsUi VerifyDepositTableDashboard(string clientName, int amount);
        ISearchResultsUi VerifyEmptyTable();
        ISearchResultsUi VerifyFirstDepositFlag();
        ISearchResultsUi VerifyLastRegistration(string clientName, string country, string phone);
        IDashboardPageUi VerifyRecentActivities(string userName = null,
            string clientName = null, string comment = null);
        ISearchResultsUi VerifyRegistrationByTime();
    }
}