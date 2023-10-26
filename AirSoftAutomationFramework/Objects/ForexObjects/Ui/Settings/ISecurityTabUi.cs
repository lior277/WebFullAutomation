// Ignore Spelling: Forex

using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Settings
{
    public interface ISecurityTabUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string GetLoginBlockMessage();
        string GetRegisterBlockMessage();
        ISecurityTabUi ClickOnRemoveLoginBlockCountryButton();
        ISecurityTabUi ClickOnRemoveRegisterBlockCountryButton();
    }
}