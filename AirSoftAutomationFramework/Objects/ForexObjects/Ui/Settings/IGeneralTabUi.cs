// Ignore Spelling: Forex

using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Settings
{
    public interface IGeneralTabUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        int GetNumOfButtonsOfLeadsStatusForNew();
    }
}