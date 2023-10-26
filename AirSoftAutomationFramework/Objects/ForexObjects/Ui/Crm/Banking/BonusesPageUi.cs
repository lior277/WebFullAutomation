using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Banking
{
    public class BonusesPageUi : IBonusesPageUi
    {
        private IWebDriver _driver;
        private IHandleFiltersUi _handleFilters;
        private readonly IApplicationFactory _apiFactory;

        public BonusesPageUi(IHandleFiltersUi handleFilters,
            IApplicationFactory apiFactory, IWebDriver driver)
        {
            _handleFilters = handleFilters;
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's   
        private readonly By BonusesTableIdColumnExp = By.XPath("//th[text()='Id']"); //By.CssSelector("table thead th[aria-label='Id']:not([style*='padding-top'])");
        #endregion Locator's     

        public IBonusesPageUi CheckIfIdColumnExist()
        {
            _driver.SearchElement(BonusesTableIdColumnExp, 100);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
