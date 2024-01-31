using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public interface IClosedTradesPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IClosedTradesPageUi CheckIfIdColumnExist();
        string MoveToTooltip();
        IClosedTradesPageUi WaitForCloseTradeTableToLoad();
        ISearchResultsUi SearchCloseTrades(string searchText);
    }
}