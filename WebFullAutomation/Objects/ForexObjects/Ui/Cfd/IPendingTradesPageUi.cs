using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public interface IPendingTradesPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ISearchResultsUi SearchPendingTrades(string searchText);
        IPendingTradesPageUi CheckIfIdColumnExist();
    }
}