using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public class PendingTradesPageUi : IPendingTradesPageUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        public PendingTradesPageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's   
        private readonly By PendingTradesTableIdColumnExp = By.XPath("//th[text()='Id']");// By.CssSelector("table thead th[aria-label='Id: activate to sort column ascending']:not([style*='padding-top'])");
        private readonly By WaitForProcessingPendingTradeExp = By.CssSelector("div[id='openTradesTable_processing'][style='display: none;']");
        private readonly By PendingTradeTableRowsExp = By.XPath("//table[contains(@class,'search-result-trade')]/tbody/tr/td[not(contains(@class,'dataTables_empty'))]/parent::tr");
        #endregion Locator's     

        public ISearchResultsUi SearchPendingTrades(string searchText)
        {
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .SearchClient(searchText);

            WaitForPendingTradeTableToLoad();

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public IPendingTradesPageUi WaitForPendingTradeTableToLoad()
        {
            _driver.WaitForExactNumberOfElements(PendingTradeTableRowsExp, 1);

            return this;
        }

        public IPendingTradesPageUi CheckIfIdColumnExist()
        {
            _driver.SearchElement(PendingTradesTableIdColumnExp);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
