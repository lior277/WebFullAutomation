// Ignore Spelling: Forex api

using System;
using System.Linq;
using System.Reflection;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public class TabsFactory : ITabsFactory
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        public TabsFactory(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's
        private By FinanceTabExp = By.CssSelector("a[id='finance-tab-link']");
        private By CommentsTabExp = By.CssSelector("a[id='comments-tab-link']");
        private By InformationTabExp = By.CssSelector("a[id='client-info-tab-link']");
        private By TimelineTabExp = By.CssSelector("a[id='timeline-tab-link']");
        #region Trade
        private readonly By CryptoVsCryptoExp = By.CssSelector("li[class*='category-Crypto vs Crypto']");        
        #endregion
        #endregion Locator's

        public T ClickOnTab<T>(string tabName) where T : class
        {

            GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                  .Where(m => m.Name.Contains(typeof(T).Name.Substring(1)) || m.Name.Contains(tabName))
                  .FirstOrDefault()?
                  .Invoke(this, Array.Empty<T>());

            return _apiFactory.ChangeContext<T>(_driver);
        }       

        private ITabsFactory ClientCardFinancesTabUi()
        {          
            _driver.SearchElement(FinanceTabExp)
                .ForceClick(_driver, FinanceTabExp);

            return this;
        }     

        private ITabsFactory ClientCardCommentsTabUi()
        {
            _driver.SearchElement(CommentsTabExp)
                .ForceClick(_driver, CommentsTabExp);

            return this;
        }

        private ITabsFactory ClientCardInformationTabUi()
        {
            _driver.SearchElement(InformationTabExp)
                .ForceClick(_driver, InformationTabExp);

            return this;
        }

        private ITabsFactory CryptoVsCryptoTabUi()
        {
            _driver.SearchElement(CryptoVsCryptoExp)
                .ForceClick(_driver, CryptoVsCryptoExp);

            return this;
        }
    }
}
