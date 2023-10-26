// Ignore Spelling: Forex Admin

using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Settings
{
    public interface ISuperAdminTabUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
    }
}