using System;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public class TradeTabUi : ITradeTabUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        public TradeTabUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's
        private readonly By TradeTableIdColumnExp = By.CssSelector("table " +
            "thead th[aria-label='Id: activate to sort column ascending']:not([style*='padding-top'])");
        #endregion Locator's

        public ITradeTabUi VerifyIdColumnExist()
        {
            _driver.SearchElement(TradeTableIdColumnExp);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
