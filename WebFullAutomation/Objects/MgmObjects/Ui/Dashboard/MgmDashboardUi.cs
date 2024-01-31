// Ignore Spelling: api

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using OpenQA.Selenium;
using static AirSoftAutomationFramework.Internals.Enums.EnumFactory;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Ui.Dashboard
{
    public class MgmDashboardUi : IMgmDashboardUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _perfomanceFilterBtn = "//label[contains(.,'{0}')]";

        public MgmDashboardUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's          
        private readonly By BrandValueExp = By.CssSelector("span[class='brand-value']");
        private readonly By FastLoginBrandBtnExp = By.CssSelector("a[class='fast-login-brand']");
        #endregion Locator's     

        public IMgmDashboardUi ClickOnPerformanceFilterByName(string filterName)
        {
            var perfomanceFilterExp = By.XPath(string.Format(_perfomanceFilterBtn, filterName));

            _driver.SearchElement(perfomanceFilterExp)
                .ForceClick(_driver, perfomanceFilterExp);

            return this;
        }

        //public IMgmDashboardUi ClickOnFastLoginBrandBtn()
        //{
        //    var originalWindow = _driver.CurrentWindowHandle;

        //    _driver.SearchElement(FastLoginBrandBtnExp)
        //        .ForceClick(_driver, FastLoginBrandBtnExp);

        //    _driver.SwitchBetweenBrowsersTabs(_driver.Url, TabToSwitch.Last);
        //    _driver.WaitForUrlToChange("dashboard");

        //    return this;
        //}

        public string GetPerformanceFilterValue()
        {
            return _driver.SearchElement(BrandValueExp)
                .GetElementText(_driver)
                .Trim();
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
