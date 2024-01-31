using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public interface IOpenTradesPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IOpenTradesPageUi CheckIfIdColumnExist();
        IClientCardUi ClickOnClientName();
        IOpenTradesPageUi WaitForOpenTradeTableToLoad();
        ISearchResultsUi SearchOpenTrades(string searchText);
        IOpenTradesPageUi VerifyCloseTradeButtonNotExist();
    }
}