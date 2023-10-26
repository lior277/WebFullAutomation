using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm
{
    public interface IPlanningPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IPlanningPageUi ClickOnSearchResult();
        IPlanningPageUi ClickToOpenAgentFilter();
        IPlanningPageUi SearchAgent(string userName);
        int SearchComment(string commentTitle);
        IPlanningPageUi SelectAgentPipe(string userName);
    }
}