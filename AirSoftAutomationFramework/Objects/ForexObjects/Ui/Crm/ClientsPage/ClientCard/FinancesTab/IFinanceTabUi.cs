using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard.FinancesTab
{
    public interface IFinanceTabUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IFinanceTabUi CheckIfIdColumnExist();
        IFinanceTabUi ClickOnFinanceButton(string financeButtonsName);
        IDictionary<string, string> GetTransactionSammery();
        ISearchResultsUi SearchFinance(string searchText);
    }
}