using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Ui.Dashboard
{
    public interface IMgmDashboardUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IMgmDashboardUi ClickOnPerformanceFilterByName(string filterName);
        //IMgmDashboardUi ClickOnFastLoginBrandBtn();
        string GetPerformanceFilterValue();
    }
}