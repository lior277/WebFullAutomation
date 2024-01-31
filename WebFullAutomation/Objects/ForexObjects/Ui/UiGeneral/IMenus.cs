// Ignore Spelling: Forex

using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public interface IMenus
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        void CheckIfPlatformLeftMenuExist();
        T ClickOnMenuItem<T>(string menuItemName = null) where T : class;
    }
}