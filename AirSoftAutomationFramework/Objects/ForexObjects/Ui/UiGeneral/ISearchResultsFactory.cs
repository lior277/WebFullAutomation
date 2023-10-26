// Ignore Spelling: Forex

using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public interface ISearchResultsFactory
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        T InstanceFactory<T>();
    }
}