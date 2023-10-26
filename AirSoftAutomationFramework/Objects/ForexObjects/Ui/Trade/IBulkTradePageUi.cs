// Ignore Spelling: Forex Crm

using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm
{
    public interface IBulkTradePageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IBulkTradePageUi ClickOnClientCheckBox();
        ISearchResultsUi ClickOnConfirmButton();
        IBulkTradePageUi ClickOnOpenTradeButton();
        IBulkTradePageUi ClickOnSaveOpenTradeButton();
        IBulkTradePageUi SelectAssetPipe(string value);
        BulkTradePageUi SelectBulkGroup(string BulkGroupName);
        IBulkTradePageUi SetExposer(string exposerValue);
    }
}