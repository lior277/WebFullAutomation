using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public interface ITimelineTabUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ITimelineTabUi ClickOnAccountActivityFilterButton();
        ITimelineTabUi ClickOnBankingActivityFilterButton();
        ITimelineTabUi ClickOnEnvelope();
        ITimelineTabUi ClickOnLoginActivityFilterButton();
        ITimelineTabUi ClickOnTradingActivityFilterButton();
        string GetEnvelopeBoxBody();
        string GetEnvelopeBoxTitle();
        ISearchResultsUi SetNumOfLines();
        ITimelineTabUi WaitForTimeLineTableToLoad();
    }
}