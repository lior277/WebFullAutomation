using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Banking
{
    public interface IDepositsPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IDepositsPageUi CheckIfIdColumnExist();
        string GetDepositStatusFilterValue();
        ISearchResultsUi SearchDeposit(string searchText);
        IDepositsPageUi SearchDepositByClientEmail(string clientEmail);
        IDepositsPageUi WaitForNumOfRowsInDepositTable();
        IDepositsPageUi WaitForDepositTableToLoad();
    }
}