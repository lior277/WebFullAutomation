// Ignore Spelling: Forex api

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Settings
{
    public class SecurityTabUi : ISecurityTabUi
    {
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        public SecurityTabUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's 
        private readonly By RegisterRemoveBlockCountryExp = By.CssSelector("div[formgroupname='register'] span[class='c-remove']");
        private readonly By LoginRemoveBlockCountryExp = By.CssSelector("div[formgroupname='login'] span[class='c-remove']");
        private readonly By RegisterBlockMessageExp = By.CssSelector("div[id='tooltip-register'][style='display: block;']");
        private readonly By LoginBlockMessageExp = By.CssSelector("div[id='tooltip-login'][style='display: block;']");
        #endregion Locator's 

        public ISecurityTabUi ClickOnRemoveRegisterBlockCountryButton()
        {
            _driver.SearchElement(RegisterRemoveBlockCountryExp)
                .ForceClick(_driver, RegisterRemoveBlockCountryExp);

            _driver.SearchElement(RegisterRemoveBlockCountryExp)
                .ForceClick(_driver, RegisterRemoveBlockCountryExp);

            return this;
        }

        public ISecurityTabUi ClickOnRemoveLoginBlockCountryButton()
        {
            _driver.SearchElement(LoginRemoveBlockCountryExp)
                .ForceClick(_driver, LoginRemoveBlockCountryExp);

            return this;
        }

        public string GetRegisterBlockMessage()
        {
            return _driver.SearchElement(RegisterBlockMessageExp)
                .GetElementText(_driver, RegisterBlockMessageExp);
        }

        public string GetLoginBlockMessage()
        {
            return _driver.SearchElement(LoginBlockMessageExp)
                .GetElementText(_driver, LoginBlockMessageExp);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
