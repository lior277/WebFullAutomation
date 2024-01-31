using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public interface ITradeGroupsUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ITradeGroupCardUi ClickOnEditBtn();
        ISearchResultsUi SearchTradeGroup(string searchText);
    }
}